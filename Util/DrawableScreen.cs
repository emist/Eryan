using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Eryan.Input;
using Eryan.UI;


//Catch on key down events

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
        Mouse m;

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
            m = wh.MOUSE;
            if (cw.AllowInput)
            {
                dllMoveMouse(pm.APPWIN, e.X, e.Y);
                pm.x = e.X;
                pm.y = e.Y;
                m.x = e.X;
                m.y = e.Y;
            }
                
            base.OnMouseMove(e);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            pm = wh.PMOUSE;
            if(cw.AllowInput)
            {
                if(e.Button.Equals(MouseButtons.Left))
                    pm.holdLeftButton();
                else if(e.Button.Equals(MouseButtons.Right))
                    pm.holdRightButton();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            pm = wh.PMOUSE;
            if(cw.AllowInput)
            {
                if(e.Button.Equals(MouseButtons.Left))
                    pm.releaseLeftButton();
                else if(e.Button.Equals(MouseButtons.Right))
                    pm.releaseRightButton();
            }
            base.OnMouseUp(e);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (cw.AllowInput)
            {
                kb = wh.getKeyBoard();
                kb.keyDown(((char)e.KeyValue));
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (cw.AllowInput)
            {
                kb = wh.getKeyBoard();
                kb.keyUp((char)e.KeyValue);
            }
            base.OnKeyUp(e);
        }

        /*

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (cw.AllowInput)
            {
                kb = wh.getKeyBoard();
                kb.sendChar(e.KeyChar);
            }
            base.OnKeyPress(e);
        }
        */
    }
}
