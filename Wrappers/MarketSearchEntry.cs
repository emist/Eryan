using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    public class MarketSearchEntry : InterfaceEntry
    {
        String name;

        /// <summary>
        /// Takes an unparsed market search entry entry and tokenize its elements
        /// </summary>
        /// <param name="unparsedEntry">Tbe entry</param>
        /// <param name="absoluteTop">Top y value of the entry icon</param>
        /// <param name="absoluteLeft">Leftmost x value of the entry icon</param>
        /// <param name="height">Height of the entry icon</param>
        /// <param name="width">Width of the entry icon</param>
        public MarketSearchEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
        {
            this.x = absoluteLeft;
            this.y = absoluteTop;
            this.height = height;
            this.width = width;
            parseEntry(unparsedEntry);
        }

        /// <summary>
        /// Tokenize the entry into sections
        /// </summary>
        /// <param name="unparsedEntry">The unparsed market entry from the client</param>
        public void parseEntry(string unparsedEntry)
        {
            name = unparsedEntry;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

    }
}
