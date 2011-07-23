using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Wrapper for Eve Items
    /// </summary>
    public class Item : InterfaceEntry
    {
        int quantity;
        string volume;
        string name;
        string meta;

        public Item(int quantity, string volume, string name, string meta, int width, int height, int X, int Y)
        {
            this.quantity = quantity;
            this.volume = volume;
            this.name = name;
            this.meta = meta;
            this.x = X;
            this.y = Y;
            this.height = height;
            this.width = width;
        }

        /// <summary>
        /// Get the item's stack quantity
        /// </summary>
        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        /// <summary>
        /// Get the item's volume
        /// </summary>
        public string Volume
        {
            get
            {
                return volume;
            }
        }

        /// <summary>
        /// Item's name
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Item's meta level
        /// </summary>
        public string Meta
        {
            get
            {
                return meta;
            }
        }

    }
}
