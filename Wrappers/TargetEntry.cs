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
    public class TargetEntry : InterfaceEntry
    {
        String name;
        int distance;
        Regex tokenizer;


        /// <summary>
        /// Takes an unparsed target entry and tokenize its elements
        /// </summary>
        /// <param name="unparsedEntry">Tbe entry</param>
        /// <param name="absoluteTop">Top y value of the entry icon</param>
        /// <param name="absoluteLeft">Leftmost x value of the entry icon</param>
        /// <param name="height">Height of the entry icon</param>
        /// <param name="width">Width of the entry icon</param>
        public TargetEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
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
        /// <param name="unparsedEntry">The unparsed Target entry from the client</param>
        public void parseEntry(string unparsedEntry)
        {
            Regex reg = new Regex("<br>");
            int nums = 0;
            string num = ""; 
            string[] split = reg.Split(unparsedEntry);
            reg = new Regex("[0-9]+");
            if (split.Count() > 1)
            {
                foreach (Capture capture in reg.Matches(split[1]))
                {
                    num += capture.Value;
                }

                if (num == "")
                {
                    throw new Exception("No numbers in the target entry");
                }

                nums = Convert.ToInt32(num);
                reg = new Regex("km");
                if (reg.Match(split[1]).Value != "")
                {
                    distance = nums * 1000;
                }

                distance = nums;
                name = split[0];
            }
        }


        public int Distance
        {
            get
            {
                return distance;
            }
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
