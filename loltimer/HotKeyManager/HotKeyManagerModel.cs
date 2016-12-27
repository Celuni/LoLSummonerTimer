using System.Collections.Generic;
using System.Windows.Forms;
using Loltimer.SingleHotKey;

namespace Loltimer
{
    public class HotKeyManagerModel
    {
        private List<Keys> currentHotKeys;

        public List<Keys> GetCurrentHotKeys()
        {
            return currentHotKeys;
        }

        public HotKeyManagerModel()
        {
            currentHotKeys = GlobalVars.defaultKeys;
        }

        public void ChangeAHotkey(int idOfChampion, Keys keyToChangeTo)
        {
            currentHotKeys[idOfChampion-1] = keyToChangeTo;
        }

    }
}
