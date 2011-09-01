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
    /// Closes interfaces that might be occupying the screen
    /// </summary>
    public class InterfaceCloser : Scriptable
    {


        public override Boolean onStart()
        {
            name = "InterfaceCloser";
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        public override int run()
        {
            if (ESession.isSystemMenuOpen())
            {
                EKeyboard.sendChar((char)0x1B);
                Thread.Sleep(600);
            }

            InterfaceEntry ok = ESession.getOkButton();
            InterfaceEntry cancel = ESession.getCancelButton();

            if (ok != null || cancel != null)
            {
                if (cancel != null)
                    EMouse.move(new Point(ERandom.Next(cancel.X, cancel.X + cancel.Width), ERandom.Next(cancel.Y, cancel.Y + cancel.Height)));
                else
                    EMouse.move(new Point(ERandom.Next(ok.X, ok.X + ok.Width), ERandom.Next(ok.Y, ok.Y + ok.Height)));

                EMouse.click(true);
                Thread.Sleep(500);
            }

            return 200;


        }
    }
}
