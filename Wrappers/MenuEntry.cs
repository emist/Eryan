using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Eryan.Wrappers
{
    /// <summary>
    /// Wraps an overview entry
    /// </summary>
    public class MenuEntry : InterfaceEntry
    {
        String text;

        /// <summary>
        /// Builds an menuentry
        /// </summary>
        /// <param name="text">The unparsed eve client menu entry text</param>
        /// <param name="absoluteTop">The lowest Y coordinate of the menu entry</param>
        /// <param name="absoluteLeft">The lowest X coordinate of the menu entry</param>
        /// <param name="height">The height of the entry on screen</param>
        /// <param name="width">The width of the entry on screen</param>
        public MenuEntry(string text, int absoluteTop, int absoluteLeft, int height, int width)
        {
            this.text = text;
            this.y = absoluteTop;
            this.x = absoluteLeft;
            this.height = height;
            this.width = width;
        }


        /// <summary>
        /// Get the text of this menu entry
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
        }

    }
}
