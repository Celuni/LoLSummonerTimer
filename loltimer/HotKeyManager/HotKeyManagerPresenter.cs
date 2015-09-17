using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Loltimer.SingleHotKey;
using System.Windows;



namespace Loltimer.HotKeyManager
{
    /// <summary>
    /// IS A SINGLETON.
    /// 
    /// </summary>
   public class HotKeyManagerPresenter
    {

        private IntPtr ptr;
        private HotKeyManagerView managerView;
        private HotKeyManagerModel managerModel;
        private List<SingleHotKeyPresenter> singleHotKeyPresenters;
       



        public HotKeyManagerPresenter(IntPtr ptr)
        {

            this.ptr = ptr;
            this.managerView = new HotKeyManagerView() ;
            this.managerModel = new HotKeyManagerModel();
            this.singleHotKeyPresenters = new List<SingleHotKeyPresenter>();
            for (int i = 0; i < GlobalVars.numOfPanels; i++)
            {
                SingleHotKeyPresenter addThisSingleHotKeyPresenter = new SingleHotKeyPresenter(managerModel.GetCurrentHotKeys()[i], "Champion " + (i + 1).ToString(), i + 1);
                singleHotKeyPresenters.Add(addThisSingleHotKeyPresenter);

                //Add a data context changed listener to each single hotkey.
                addThisSingleHotKeyPresenter.GetSingleHotKeyView().CurrentHotkey.DataContextChanged += (sender, e) =>
                {
                    //when this is called, the currentKey in singleHotKeyModel has registered the new hotkey
                    //e.newValue is the id of the champion
                    int idOfChampionChanged =(int)e.NewValue;
                    Keys newKeyToChangeTo = singleHotKeyPresenters[idOfChampionChanged-1].GetCurrentHotkey();
                    NativeMethods.UnregisterHotKey(ptr, idOfChampionChanged);
                    NativeMethods.RegisterHotKey(ptr, idOfChampionChanged, 0, (int)newKeyToChangeTo);
                    this.managerModel.ChangeAHotkey(idOfChampionChanged, newKeyToChangeTo);
                    
                };
                managerView.HotkeysStackPanel.Children.Insert(i, addThisSingleHotKeyPresenter.GetSingleHotKeyView());
            }
            RegisterDefaultHotKeys();
            this.managerView.Done.Click += Done_Click;
            this.managerView.Initialized += Window_Initialized;
        }

        public void RegisterDefaultHotKeys()
        {
            for (int i = 1; i <= GlobalVars.numOfPanels; i++)
                NativeMethods.RegisterHotKey(ptr, i, 0, (int)GlobalVars.defaultKeys[i - 1]);

        }
        /// <summary>
        /// Unregister Hotkey based on IDs
        /// </summary>
        public void UnregisterHotkeys()
        {
            for (int i = 1; i <= GlobalVars.numOfPanels; i++)
                NativeMethods.UnregisterHotKey(ptr, i);

        }
        public void CloseView()
        {
            this.managerView.Close();
        }
        public void ShowView()
        {
            this.managerView.Show();
        }
        public void HideView()
        {
            this.managerView.Hide();
        }
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            HideView();

        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            HideView();

        }

    }

}
