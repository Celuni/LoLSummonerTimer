using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Loltimer.ChampionTimer
{
    public class ChampionTimerPresenter
    {
        private ChampionTimerView champTimerView;
        private ChampionTimerModel champTimerModel;

        public ChampionTimerView GetChampionTimerView()
        {
            return this.champTimerView;
        }

        public ChampionTimerPresenter(int id)
        {
            champTimerView = new ChampionTimerView();
            champTimerModel = new ChampionTimerModel(id, champTimerView.GetChampionTextBox().Text);
            SetDefaultChampionTextBoxField();
            champTimerView.GetProgBarWithText().SetProgMax(champTimerModel.GetGoalTime());
            //add listeners to the buttons
            champTimerView.GetStartButton().Click += StartClick_EventHandler;
            champTimerView.GetResetButton().Click += ResetClickWithSpeech;
            champTimerView.GetChampionTextBox().KeyDown += OnKeyDownHandler;
            champTimerView.GetUtilComboBox().SelectionChanged += ComboBox_SelectionChanged;
            champTimerModel.GetTimer().AddActionToTick(IncrementProgressBar);

        }


        private void StartClick_EventHandler(object sender, System.EventArgs e)
        {
            StartClickAction();
        }

        public void StartClickAction()
        {
            this.champTimerModel.GetTimer().SayThis("Starting " + champTimerModel.ChampionName + " Timer!");
            this.champTimerModel.GetTimer().StartTimer();
            champTimerView.GetResetButton().IsEnabled = true;
            champTimerView.GetStartButton().IsEnabled = false;
            champTimerView.GetUtilComboBox().IsEnabled = false;
            champTimerView.GetChampionTextBox().IsEnabled = false;
        }

        /// <summary>
        /// For when the timer is reset from user interrupt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ResetClickWithSpeech(object sender, System.EventArgs e)
        {
            this.champTimerModel.GetTimer().SayThis("Stopping " + champTimerModel.ChampionName + " Timer!");
            this.champTimerModel.GetTimer().ResetTimer();
            this.champTimerView.GetResetButton().IsEnabled = false;
            this.champTimerView.GetStartButton().IsEnabled = true;
            this.champTimerView.GetUtilComboBox().IsEnabled = true;
            this.champTimerView.GetChampionTextBox().IsEnabled = true;
            this.champTimerView.GetProgBarWithText().ResetProgressBar();
        }

        /// <summary>
        /// For when the time completes normally, no 'reset chapion $id timer' speech necessary. The program will already notify 
        /// "champion $id flash back up."
        /// </summary>
        public void ResetClickWithoutSpeech()
        {
            this.champTimerModel.GetTimer().ResetTimer();
            this.champTimerView.GetResetButton().IsEnabled = false;
            this.champTimerView.GetStartButton().IsEnabled = true;
            this.champTimerView.GetUtilComboBox().IsEnabled = true;
            this.champTimerView.GetChampionTextBox().IsEnabled = true;
            this.champTimerView.GetProgBarWithText().ResetProgressBar();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.champTimerModel.GetTimer().UtilityPoints = Int32.Parse(this.champTimerView.GetUtilComboBox().SelectedItem.ToString());

        }

        /// <summary>
        /// When the user hits Enter on the ChampionTextBox, we auto change the ChampionName For them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                this.champTimerModel.ChampionName = this.champTimerView.GetChampionTextBox().Text;
                //also change the image for the icon
                this.champTimerView.SetChampionImage(this.champTimerView.GetChampionTextBox().Text);
            }
        }

        public void IncrementProgressBar(object sender, EventArgs e)
        {
            //update bar with new time
            this.champTimerView.GetProgBarWithText().UpdateProgBar(this.champTimerModel.GetTimer().Time);
            if (this.champTimerModel.GetTimer().Time == 0)
            {
                this.champTimerModel.GetTimer().SayThis(champTimerModel.ChampionName + "s flash is back up!");
                ResetClickWithoutSpeech();
            }
        }

        public void SetDefaultChampionTextBoxField()
        {
            this.champTimerModel.ChampionName = "Champion" + this.champTimerModel.Id.ToString();
            this.champTimerView.ChampionTextBox.Text = "Champion" + this.champTimerModel.Id.ToString();
            
        }


    }
}
