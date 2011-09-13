using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Util
{
    public class Logger
    {
        string scriptname;
        UI.LogViewer logViewer;

        public Logger(UI.WindowHandler wh, string ScriptName)
        {
            scriptname = ScriptName;
            logViewer = wh.LOGVIEWER;
        }



        public void log(string message)
        {
            logViewer.LogBox.Text += scriptname + ": " + message + "\n";
        }

        public string ScriptName
        {
            get
            {
                return scriptname;
            }

            set
            {
                scriptname = value;
            }
        }
    }
}
