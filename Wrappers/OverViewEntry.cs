using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Eryan.Wrappers
{
    class OverViewEntry
    {
        List<string> sections;
        Regex tokenizer;
        int absoluteTop, absoluteLeft, height, width;

        public OverViewEntry(string unparsedEntry, int absoluteTop, int absoluteLeft, int height, int width)
        {
            sections = new List<string>();
            this.absoluteLeft = absoluteLeft;
            this.absoluteTop = absoluteTop;
            this.height = height;
            this.width = width;
            parseEntry(unparsedEntry);
        }

        public void parseEntry(string unparsedEntry)
        {
            tokenizer = new Regex(@"<t>");
            string[] splitString = tokenizer.Split(unparsedEntry);
            tokenizer = new Regex("<right>");
            
            splitString[1] = tokenizer.Split(splitString[1])[1];

            foreach (string split in splitString)
            {
                if (split.Equals(""))
                    continue;
                Console.WriteLine(split);
                sections.Add(split);
            }

            
        }
    }
}
