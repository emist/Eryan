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

        /// <summary>
        /// Builds an item entry
        /// </summary>
        /// <param name="quantity">Quantity we hold</param>
        /// <param name="volume">The volume it takes up</param>
        /// <param name="name">Name of the item</param>
        /// <param name="meta">Meta level of the item</param>
        /// <param name="width">Width of the item icon</param>
        /// <param name="height">Height of the item icon</param>
        /// <param name="X">Leftmost x coordinate of the item</param>
        /// <param name="Y">Top y coordinate of the item</param>
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
