using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Loltimer
{
    public static class GlobalVars
    {
        public const int numOfPanels = 5;
        public static readonly List<Keys> defaultKeys = new List<Keys> { Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12 };
        public static Boolean secondsOnly = true;
    }
}
