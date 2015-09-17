using System;
using System.Windows.Forms;

namespace Loltimer.SingleHotKey
{
    public class SingleHotKeyModel
    {
        private Keys currentKey;
        private String champion;
        private int id;

        public Keys CurrentKey
        {
            get { return currentKey; }
            set { currentKey = value; }
        }

        public String Champion
        {
            get { return champion; }
            set { champion = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        

        public SingleHotKeyModel(Keys k, String c, int i)
        {
            this.currentKey = k;
            this.Champion = c;
            this.id = i;
        }
            
    }
}
