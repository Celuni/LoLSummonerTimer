using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Loltimer
{
    public class ChampionPanel
    {
        private TextBox champTextBoxCopy;
        private String championName;
        private ComboBox utilityBox;
        private Button start;
        private Button reset;
        private TimerWithSpeech timer;

        //properties
        public Button Start
        {
            get { return start; }
        }
        public Button Reset
        {
            get { return reset; }
        }

        public String ChampionName
        {
            get { return championName; }
            set { championName = value; }
        }


        public ChampionPanel(TextBox champTextBox, ComboBox parentComboBox, Button parentStart, Button parentReset, ProgBarWithText parentProgBar)
        {
            this.champTextBoxCopy = champTextBox;
            champTextBox.KeyDown += OnKeyDownHandler;
            this.championName = champTextBox.Text;
            this.utilityBox = parentComboBox;
            this.utilityBox.SelectedIndex = 0;
            this.start = parentStart;
            this.reset = parentReset;
            this.timer = new TimerWithSpeech(this, parentProgBar);
            start.Click += StartClick;
            reset.Click += ResetClick;
            this.reset.IsEnabled = false;
            parentComboBox.SelectionChanged += ComboBox_SelectionChanged;
        }


        public void StartClick(object sender, System.EventArgs e)
        {
            timer.Start();
            this.reset.IsEnabled = true;
            this.start.IsEnabled = false;
            this.utilityBox.IsEnabled = false;
            this.timer.SayThis("Starting " + championName + " Timer!");
        }
        public void ResetClick(object sender, System.EventArgs e)
        {
            this.timer.SayThis("Stopping " + championName + " Timer!");
            timer.Stop();
            this.reset.IsEnabled = false;
            this.start.IsEnabled = true;
            this.utilityBox.IsEnabled = true;
            this.timer.ResetProgressBarAndTimer();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.UtilityPoints = Int32.Parse(utilityBox.SelectedItem.ToString());
        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                this.championName = champTextBoxCopy.Text;
            }
        }
    }
}
