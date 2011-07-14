using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    /// <summary>
    /// Generic Event Listener
    /// </summary>
    public interface Observer
    {
        /// <summary>
        /// All observers must implement this
        /// </summary>
        /// <param name="e"></param>
        void update(Event e);
    }
}
