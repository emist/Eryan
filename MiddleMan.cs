using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eryan
{
    class MiddleMan
    {
        WindowHandler botWindow = new WindowHandler();

        public void spawnBot()
        {
            Application.Run(botWindow);   
        }

    }
}
