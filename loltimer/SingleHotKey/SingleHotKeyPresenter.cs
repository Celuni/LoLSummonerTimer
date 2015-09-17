using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Loltimer.SingleHotKey
{
    public class SingleHotKeyPresenter
    {
        private SingleHotKeyModel singleHotKeyModel;
        private SingleHotKeyView singleHotKeyView;

        public SingleHotKeyView GetSingleHotKeyView()
        {
             return singleHotKeyView;
        }
        
        

        public SingleHotKeyPresenter(Keys k, String s, int id)
        {
            singleHotKeyModel = new SingleHotKeyModel(k, s, id);
            singleHotKeyView = new SingleHotKeyView();

            singleHotKeyView.ChangeHotKey.Click += ChangeHotkey_Click;
            singleHotKeyView.SetChampionIDText(s);
            singleHotKeyView.SetCurrentHotKeyText(k.ToString());
        }


        //------------------------------------ADD EVENT HANDLERS FOR VIEW
        /// <summary>
        /// SHOULD ADD THIS TO THE BUTTON IN ADDITION TO HAVING THE SINGLEHOTKEY CHANGE PASS IN ITS ID .
        /// SO HAVE 2 += listeners for the change hotkey Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeHotkey_Click(object sender, RoutedEventArgs e)
        {
            string temp = singleHotKeyView.GetChampionIDText();
            //this.ChangeHotkey.IsEnabled = false;
            singleHotKeyView.SetChampionIDText("Press New Hotkey...");
            //how to get ID of what champion's hotkey we are changing?
            //Set listener for the window for the next Button Hit
            singleHotKeyView.KeyDown += SingleHotkeyChange_KeyDown;



            //unregister old,
            //register new
            //update text labels
            //update arrays of current hotkeys currently

        }
        private void SingleHotkeyChange_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.MessageBox.Show("Registered "+ e.Key.ToString());
            ModifyHotkey((Keys) KeyInterop.VirtualKeyFromKey(e.Key));
            e.Handled = true;//some bug made this event fire twice.
        }

        private void ModifyHotkey(Keys k)
        {
            singleHotKeyModel.CurrentKey = k;
            singleHotKeyView.SetChampionIDText("Champion "+(singleHotKeyModel.Id).ToString());
            singleHotKeyView.SetCurrentHotKeyText(k.ToString());
            //eh, what does data context even mean
            singleHotKeyView.CurrentHotkey.DataContext = singleHotKeyModel.Id;
            singleHotKeyView.KeyDown -= SingleHotkeyChange_KeyDown;
        }

        public Keys GetCurrentHotkey()
        {
            return this.singleHotKeyModel.CurrentKey;
        }
    }
}
