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
    public class OverViewEntry : InterfaceEntry
    {
        List<string> sections;
        Regex tokenizer;
        int distance;
        //int absoluteTop, absoluteLeft, height, width;

        /// <summary>
        /// Builds an overview entry
        /// </summary>
        /// <param name="unparsedEntry">The unparsed eve client overview entry text</param>
        /// <param name="absoluteTop">The lowest Y coordinate of the overview entry</param>
        /// <param name="absoluteLeft">The lowest X coordinate of the overview entry</param>
        /// <param name="height">The height of the entry on screen</param>
        /// <param name="width">The width of the entry on screen</param>
        public OverViewEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
        {
            sections = new List<string>();
            this.y = absoluteTop;
            this.x = absoluteLeft;
            this.height = height;
            this.width = width;
            parseEntry(unparsedEntry);
        }

        /// <summary>
        /// Tokenize the entry into sections
        /// </summary>
        /// <param name="unparsedEntry">The unparsed overview entry from the client</param>
        public void parseEntry(string unparsedEntry)
        {
            tokenizer = new Regex(@"<t>");
            string[] splitString = tokenizer.Split(unparsedEntry);
            tokenizer = new Regex("<right>");

            //Console.WriteLine(unparsedEntry);

            if (splitString.Count() > 1)
            {
                string[] tokenized = tokenizer.Split(splitString[1]);
                if(tokenized.Count() > 1)
                    splitString[1] = tokenizer.Split(splitString[1])[1];
            }

            foreach (string split in splitString)
            {
                if (split.Equals(""))
                    continue;
                Regex reg = new Regex("[0-9]*,*[0-9]+");
                String nums;

                if ((nums = reg.Match(split).Value) != "")
                {
                    //Console.WriteLine(nums);
                    reg = new Regex(",");
                    if (reg.Match(split).Value != "")
                    {
                        distance = Convert.ToInt32(nums[0] + nums.Substring(2, 3));
                    }
                    else
                    {
                        reg = new Regex("km");
                        if (reg.Match(split).Value != "")
                        {
                            distance = Convert.ToInt32(nums) * 1000;
                            //Console.WriteLine("Distance is " + distance);
                        }
                        else
                            distance = Convert.ToInt32(nums);
                    }

                }
                //Console.WriteLine(split);
                sections.Add(split);
            }

            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(300);
            foreach (string section in sections)
            {
                sb.Append(section);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the distance between our ship and this overview entry
        /// </summary>
        public int Distance
        {
            get
            {
                return distance;
            }
        }


        /// <summary>
        /// Returns all sections of the overview entry as strings
        /// </summary>
        public List<string> Sections
        {
            get
            {
                return sections;
            }
        }

        /// <summary>
        /// Sort by distance from player
        /// </summary>
        /// <param name="T">The object to compare against</param>
        /// <returns></returns>
        public int CompareTo(OverViewEntry T)
        {
            return distance.CompareTo(T.distance);
        }

    }
}
