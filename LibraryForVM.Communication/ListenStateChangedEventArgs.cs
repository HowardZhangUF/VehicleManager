using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForVM
{
    public class ListenStateChangedEventArgs : EventArgs
    {
        public DateTime OccurTime { get; private set; }
        public int Port { get; private set; }
        public bool IsListened { get; private set; }

        public ListenStateChangedEventArgs(DateTime OccurTime, int Port, bool IsListened) : base()
        {
            this.OccurTime = OccurTime;
            this.Port = Port;
            this.IsListened = IsListened;
        }
    }
}
