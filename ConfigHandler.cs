using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Eryan
{
    public class ConfigHandler
    {

        public List<string> readconfig()
        {
            string path = System.Windows.Forms.Application.ExecutablePath;
            path = path.Substring(0, path.Length - 9);
            StreamReader reader = new StreamReader(path + "\\config.txt");
            string item;
            List<string> items = new List<string>();
            while ((item = reader.ReadLine()) != null)
            {
                items.Add(item);
            }

            return items;
        }
    }
}
