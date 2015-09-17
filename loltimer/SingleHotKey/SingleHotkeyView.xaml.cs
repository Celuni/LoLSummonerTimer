using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Loltimer
{
    /// <summary>
    /// Interaction logic for SingleHotkeyChange.xaml
    /// </summary>
    public partial class SingleHotKeyView : System.Windows.Controls.UserControl
    {
        //SHOULD GO IN HOTKEYMANAGER

        //private int id;
        //private static int idDispenser = 0;
        //public int Id
        //{
        //    get { return id; }
        //    set { id = value;  }
        //}


        public SingleHotKeyView()
        {
            InitializeComponent();
            
        }

        public string GetChampionIDText()
        {
            return this.ChampionID.Text;
        }

        public void SetChampionIDText(string text)
        {
            this.ChampionID.Text = text;
        }

        public void SetCurrentHotKeyText(string text)
        {
            this.CurrentHotkey.Text = text;
        }


    }
}
