using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    /// <summary>
    /// Generic Event disptacher
    /// </summary>
    interface Subject
    {
        /// <summary>
        /// Adding an observer to the dispatcher
        /// </summary>
        /// <param name="o">The observer to add</param>
        void registerObserver(Observer o);

        /// <summary>
        /// Removing an observer from the dispatcher
        /// </summary>
        /// <param name="o">The observer to remove</param>
        void removeObserver(Observer o);

        /// <summary>
        /// Dispatch event to the observers
        /// </summary>
        void notifyObservers();
    }

}
