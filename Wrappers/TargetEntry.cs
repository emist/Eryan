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
    public class TargetEntry
    {
        List<string> sections;
        Regex tokenizer;
        int absoluteTop, absoluteLeft, height, width;

        public TargetEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
        {
            sections = new List<string>();
            this.absoluteLeft = absoluteLeft;
            this.absoluteTop = absoluteTop;
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
            tokenizer = new Regex(@"<t>");
            string[] splitString = tokenizer.Split(unparsedEntry);
            tokenizer = new Regex("<right>");
            
           /*
            splitString[1] = tokenizer.Split(splitString[1])[1];

            foreach (string split in splitString)
            {
                if (split.Equals(""))
                    continue;
                Console.WriteLine(split);
                sections.Add(split);
            }
            */
         

        }

        /// <summary>
        /// Returns the X coordinate of this entry
        /// </summary>
        public int X
        {
            get
            {
                return absoluteLeft;
            }
        }

        /// <summary>
        /// Returns the Y coordinate of this entry
        /// </summary>
        public int Y
        {
            get
            {
                return absoluteTop;
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

    }
}
