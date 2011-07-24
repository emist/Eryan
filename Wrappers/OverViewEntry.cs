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

        public OverViewEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
        {
            sections = new List<string>();
            this.y = absoluteLeft;
            this.x = absoluteTop;
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
                Regex reg = new Regex("[0-9]+");
                String nums;
                if ((nums = reg.Match(split).Value) != "")
                {
                    reg = new Regex("km");
                    if (reg.Match(split).Value != "")
                    {
                        distance = Convert.ToInt32(nums) * 1000;
                        Console.WriteLine("Distance is " + distance);
                    }                        
                 
                }
                Console.WriteLine(split);
                sections.Add(split);
            }

            
        }

        /// <summary>
        /// Returns the X coordinate of this entry
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }
        
        /// <summary>
        /// Returns the Y coordinate of this entry
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Returns the width of this entry
        /// </summary>
        public int Width
        {
            get
            {
                return width;
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
        /// Returns the height of this entry
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
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

    }
}
