using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Loltimer.HotKeyManager;
using Loltimer.ChampionTimer;
using System.Collections.Generic;
using RiotSharp;
using RiotSharp.SummonerEndpoint;

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
            ptr = NativeMethods.FindWindow(null,this.DataContext.ToString());
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

        private void SummonerNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string championName = SummonerNameTextBox.Text;
            var api = RiotApi.GetInstance("1cc36623-f0aa-4a42-a3f1-942c0da13e7f");
            Summoner summoner;
            try
            {
                summoner = api.GetSummoner(Region.na, championName);
                var currentGame = api.GetCurrentGame(Platform.NA1, summoner.Id);
                int friendlyID = currentGame.Participants ;
                foreach (var p in currentGame.Participants){
                    if(p.TeamId)
                }
            }
            catch (RiotSharpException ex)
            {
                //Handle the exception however you want.
            }
           


        }

    }
}

