using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Eryan.Wrappers
{
    /// <summary>
    /// Wraps a target entry
    /// </summary>
    public class MarketEntry : InterfaceEntry
    {
        String name;
        int distance;
        Regex tokenizer;
        string unparsed;
        double price;
        string location;
        string expiration;
        int quantity;
        string type;

        /// <summary>
        /// Takes an unparsed target entry and tokenize its elements
        /// </summary>
        /// <param name="unparsedEntry">Tbe entry</param>
        /// <param name="absoluteTop">Top y value of the entry icon</param>
        /// <param name="absoluteLeft">Leftmost x value of the entry icon</param>
        /// <param name="height">Height of the entry icon</param>
        /// <param name="width">Width of the entry icon</param>
        public MarketEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
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
            unparsed = unparsedEntry;
            
            unparsedEntry = unparsedEntry.Replace("<t>", "|");
            unparsedEntry = unparsedEntry.Replace("<right>", "");

            List<string> mentry = unparsedEntry.Split('|').ToList<string>();

            quantity = Convert.ToInt32(mentry[1]);
            price = Convert.ToDouble(mentry[2].Substring(0, mentry[2].Length-3));
            location = mentry[3];
            if (mentry[4].Equals("Region"))
            {
                type = "Buy";
                expiration = mentry[6];
            }
            else
            {
                type = "Sell";
                expiration = mentry[4];
            }



        }

        /// <summary>
        /// Return the quantity
        /// </summary>
        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        /// <summary>
        /// Get the price of the order
        /// </summary>
        public double Price
        {
            get
            {
                return price;
            }
        }

        /// <summary>
        /// Location where its being sold/bought
        /// </summary>
        public string Location
        {
            get
            {
                return location;
            }
        }


        /// <summary>
        /// Expiration
        /// </summary>
        public string Expiration
        {
            get
            {
                return expiration;
            }
        }

        public string Unparsed
        {
            get
            {
                return unparsed;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
        }
    }
}
