using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using Eryan.Script;
using Eryan.Wrappers;

namespace Eryan.Script.Scripts
{
    /// <summary>
    /// Logs into the Eve account
    /// </summary>
    public class AutoLoger : Scriptable
    {

        public override Boolean onStart()
        {
            name = "Autologer";
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        public override int run()
        {
            if (ESession.isLoading())
                return 6000;
            
            if (ESession.atLogin())
            {
                ESession.login("user", "pass");
                return 3000;
            }

            if (ESession.atCharSel())
                ESession.click(ESession.getEnterButton());
           

            return 200;
        }
    }
}
