using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Loltimer
{
    /// <summary>
    /// Interaction logic for ChampionPanelRE.xaml
    /// </summary>
    public partial class ChampionTimerView : UserControl
    {
        //has components:
        /* championImage
         * championTextBox
         * -flash image
         * - utilComboBox
         * startButton
         * resetButton
         * progBarWithText
         */

        public ProgressBarWithTextControl GetProgBarWithText()
        {
            return ProgBarWithText;
        }
        public Button GetStartButton(){
            return this.StartButton;
        }
        public Button GetResetButton()
        {
            return this.ResetButton;
        }
        public TextBox GetChampionTextBox()
        {
            return this.ChampionTextBox;
        }
        public ComboBox GetUtilComboBox()
        {
            return this.UtilComboBox;
        }

        public void SetChampionImage(string iconName)
        {
            try
            {
                this.ChampionImage.Source = new BitmapImage(new Uri("..\\..\\Icons\\"+iconName.Replace(" ", "_")+"Square.png", UriKind.Relative)); ;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem loading icon for " + iconName);
            }
        }


        public ChampionTimerView()
        {
            InitializeComponent();
            UtilComboBox.SelectedIndex = 0;
            ResetButton.IsEnabled = false;
           
            //ImageBrush tripleBarImg = new ImageBrush();
            //tripleBarImg.ImageSource =  new BitmapImage(new Uri("..\\..\\Icons\\tripleBar.png", UriKind.Relative));
            //tripleBarButton.Background = tripleBarImg;
        }

        public bool IsRunningTimerCurrently()
        {
            if (this.StartButton.IsEnabled)
                return false;
            return true;
        }

        public Ellipse GetEllipse()
        {
            return this.Ellipse;
        }


    }
}
