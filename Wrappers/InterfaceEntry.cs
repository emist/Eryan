using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Eryan.Wrappers
{
    /// <summary>
    /// Base class for all interface objects
    /// </summary>
    public class InterfaceEntry
    {
        /// <summary>
        /// The x, y, width and height of the interface, all entries inherit this
        /// </summary>
        protected int x, y, width, height;

        /// <summary>
        /// Get the X position of this item in the Eve client
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Get the Y position of this item in the Eve client
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }


        /// <summary>
        /// Get the width of this item in the Eve client
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
        }
        
        /// <summary>
        /// Get the Height of this item in the Eve Client
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
        }



                
    }

    

}
