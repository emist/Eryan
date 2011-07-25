using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Wrapper for the Selected item box
    /// </summary>
    public class SelectedItem
    {
        string name;
        int distance;

        /// <summary>
        /// Build a wrapper of currently selected item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="distance">Distance to the item</param>
        public SelectedItem(string name, int distance)
        {
            this.name = name;
            this.distance = distance;
        }

        /// <summary>
        /// The name of the selected item
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The distance to the selected item in meters
        /// </summary>
        public int Distance
        {
            get
            {
                return distance;
            }
        }
    }
}
