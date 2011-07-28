using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eryan.Wrappers;


namespace Eryan.Wrappers
{
    public class Button : InterfaceEntry
    {
        string name;

        public Button(string name, int x, int y, int height, int width)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.name = name;
        }
    }
}
