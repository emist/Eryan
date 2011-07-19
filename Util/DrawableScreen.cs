using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Eryan.Input;
using Eryan.UI;

namespace Eryan.Util
{
    /// <summary>
    /// The Eryan overlay, for internal use
    /// </summary>
    
    public class DrawableScreen : Utils
    {
        WindowHandler wh;
        ClientWindow cw;
        PreciseMouse pm;
        KeyBoard kb;

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMoveMouse(IntPtr handle, int x, int y);


        public DrawableScreen(ClientWindow cw, WindowHandler wh) : base()
        {
            this.wh = wh;
            this.cw = cw;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            pm = wh.PMOUSE;
            if (cw.AllowInput)
            {
                dllMoveMouse(pm.APPWIN, e.X, e.Y);
                pm.x = e.X;
                pm.y = e.Y;
            }
                
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            pm = wh.PMOUSE;

            if (cw.AllowInput)
            {
                if (e.Button.Equals(MouseButtons.Left))
                    pm.click(pm.getX(), pm.getY(), true, 0);
                else if (e.Button.Equals(MouseButtons.Right))
                    pm.click(pm.getX(), pm.getY(), false, 0);
                base.OnMouseClick(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (cw.AllowInput)
            {
                kb = wh.getKeyBoard();
                kb.sendChar(e.KeyChar);
            }
            base.OnKeyPress(e);
        }

    }
}
