using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Loltimer.HotKeyManager;
using Loltimer.ChampionTimer;
using System.Collections.Generic;
using RiotSharp;
using RiotSharp.SummonerEndpoint;
using System.Collections;

namespace Loltimer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IntPtr ptr;

        private DragAndDrop dnd;
        private List<ChampionTimerPresenter> championPanelPresenters;
        private HotKeyManagerPresenter hotkeyManager;
        //pls no steal my api key....
        RiotApi api = RiotApi.GetInstance("1cc36623-f0aa-4a42-a3f1-942c0da13e7f");
        StaticRiotApi staticApi = StaticRiotApi.GetInstance("1cc36623-f0aa-4a42-a3f1-942c0da13e7f");

        public MainWindow()
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = "League Summoner Timer by Will Wen";
            InitializeComponent();
            championPanelPresenters = new List<ChampionTimerPresenter>();
            for (int i = 0; i < GlobalVars.numOfPanels; i++)
            {
                this.championPanelPresenters.Add(new ChampionTimerPresenter(i + 1));//i+1 because ID [1,5] not [0,4]
                this.ChampionStackPanel.Children.Add(championPanelPresenters[i].GetChampionTimerView()); //add the views to the stack panel
            }
            //registers drag and drop
            dnd = new DragAndDrop(ChampionStackPanel, championPanelPresenters);
            RegisterMenuListeners();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ptr = NativeMethods.FindWindow(null, this.DataContext.ToString());
            hotkeyManager = new HotKeyManagerPresenter(ptr);
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            hotkeyManager.UnregisterHotkeys();
            hotkeyManager.CloseView();
        }



        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        /// <summary>
        /// When a registered hotkey is hit, we use the ID of the hotkey to start/reset the appropriate champion panel timer.
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"> ID of the hotkey. We registered this in HotKeyManagerPresenter</param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0312)
            {
                int i = wParam.ToInt32() - 1;
                ChampionTimerPresenter presenter = championPanelPresenters[i];
                if (presenter.GetChampionTimerView().IsRunningTimerCurrently())
                    presenter.ResetClickWithSpeech(null, null);
                else
                    presenter.StartClickAction();

            }
            return IntPtr.Zero;
        }

        private void RegisterMenuListeners()
        {
            SetToDefaultItem.Click += (o, e) =>
            {
                hotkeyManager.UnregisterHotkeys();
                hotkeyManager.RegisterDefaultHotKeys();
            };
        }




        private void SecondsOnlyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ToggleTimeFormat();
        }

        private void ChangeHotkeysItem_Click(object sender, RoutedEventArgs e)
        {

            hotkeyManager.ShowView();
        }

        private void SecondsOnlyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleTimeFormat();
        }
        private void ToggleTimeFormat()
        {
            GlobalVars.secondsOnly = !GlobalVars.secondsOnly;
        }

        private void SummonerNameTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == System.Windows.Input.Key.Return)
            {
                string summonerName = SummonerNameTextBox.Text;

                Summoner summoner;
                RiotSharp.CurrentGameEndpoint.CurrentGame currentGame;
                try
                {
                    summoner = api.GetSummoner(Region.na, summonerName);
                }
                catch (RiotSharpException ex)
                {
                    System.Windows.Forms.MessageBox.Show("That Summoner does not exist");
                    return;
                }
                try
                {
                    currentGame = api.GetCurrentGame(Platform.NA1, summoner.Id);
                }
                catch (RiotSharp.RiotSharpException ex)
                {
                    //Handle the exception however you want.

                    System.Windows.Forms.MessageBox.Show("Could not find that summoner in a game");
                    return;
                }
                List<RiotSharp.CurrentGameEndpoint.Participant> team1 = new List<RiotSharp.CurrentGameEndpoint.Participant>();
                List<RiotSharp.CurrentGameEndpoint.Participant> team2 = new List<RiotSharp.CurrentGameEndpoint.Participant>();
                int teamIDofSummoner = -1;
                //split the participants into their teams, ally and enemy
                foreach (var p in currentGame.Participants)
                {
                    if (summoner.Name.Equals(p.SummonerName))
                    {
                        teamIDofSummoner = (int)p.TeamId;
                    }
                    if (p.TeamId == 100) //ids are 100 or 200
                    {
                        team1.Add(p);
                    }
                    else
                    {
                        team2.Add(p);
                    }
                }

                Console.WriteLine("Main summoner's enemies are: ");
                List<RiotSharp.CurrentGameEndpoint.Participant> enemyTeam;
                if (teamIDofSummoner == 100)
                    enemyTeam = team2;
                else
                    enemyTeam = team1;

                foreach (var p in enemyTeam)
                {
                    Console.WriteLine("Player " + p.SummonerName + " is playing " + staticApi.GetChampion(Region.na, (int)p.ChampionId).Name);


                    if (staticApi.GetSummonerSpell(Region.na,
                        (RiotSharp.StaticDataEndpoint.SummonerSpell)p.SummonuerSpell1).Name.Equals("Flash") ||
                        staticApi.GetSummonerSpell(Region.na,
                        (RiotSharp.StaticDataEndpoint.SummonerSpell)p.SummonerSpell2).Name.Equals("Flash"))
                    {
                        Console.WriteLine("and they have Flash.");
                    }

                    /**
                     * masteryId	long	The ID of the mastery
                     * rank	        int	    The number of points put into this mastery by the user
                     */
                    var m = p.Masteries;
                    foreach (var mastery in m)
                    {
                        var masteryDetail = staticApi.GetMastery(Region.na, (int) mastery.MasteryId);
                        if (masteryDetail.Name.Equals("Insight")) //also id == 6241
                        {

                            int id = masteryDetail.Id;
                            Console.WriteLine("And they have the insight mastery.");
                        }

                        
                    }

                }




            }



        }
    }

}

