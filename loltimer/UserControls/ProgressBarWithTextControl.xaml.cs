using System.Windows;
using System.Windows.Controls;

namespace Loltimer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ProgressBarWithTextControl : UserControl
    {


        public ProgressBarWithTextControl()
        {
            InitializeComponent();
            //if you dont say what textBlock1 is , then changing the value will call progressBar_valuechagned
            ProgBar.Maximum = 60 * 5;
            ProgBar.Value = ProgBar.Maximum;
            ProgBar.ValueChanged += ProgressBar_ValueChanged;


        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = ((ProgressBar)sender).Value;
            if (value == ((ProgressBar)sender).Maximum)
            {
                ProgText.Text = "Time Left";
            }
            else
            {
                ProgText.Text = value.ToString();
            }

        }

        public void SetProgMax(int max)
        {
            ProgBar.Maximum = max;
        }

        public void UpdateProgBar(int value)
        {
            ProgBar.Value = value;
            if (!GlobalVars.secondsOnly)
            {
                ProgText.Text = value.ToString();
            }
            else
            {
                int min = value / 60;
                int sec = value % 60;
                if (sec < 10)
                    ProgText.Text = min.ToString() + ":0" + sec.ToString();
                else
                    ProgText.Text = min.ToString() + ":" + sec.ToString();

            }

        }


        public void ResetProgressBar()
        {
            ProgBar.Value = ProgBar.Maximum;
            ProgText.Text = "Time Left";

        }
    }
}
