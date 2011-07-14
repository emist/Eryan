using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    /// <summary>
    /// Event dispatcher, unused
    /// </summary>
    public class EryanEventHandler:BaseSubject
    {
        Event e;
        /// <summary>
        /// Dispatch an event to the listeners
        /// </summary>
        /// <param name="e">Event to dispatch</param>
        public void dispatch(Event e)
        {
            this.e = e;
            this.notifyObservers();
        }

    }
}
