using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    public class EryanEventHandler:BaseSubject
    {
        Event e;
        public void dispatch(Event e)
        {
            this.e = e;
            this.notifyObservers();
        }

    }
}
