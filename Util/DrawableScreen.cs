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


    public class DrawableScreen : Utils
    {

        WindowHandler wh;
        ClientWindow cw;
        PreciseMouse pm;
        KeyBoard kb;
        Mouse m;

        private Font systemFont = new Font("Impact", 16);

        [DllImport(@"C:\\mouseDLL.dll")]
        public static extern void dllMoveMouse(IntPtr handle, int x, int y);


        public DrawableScreen(ClientWindow cw, WindowHandler wh) : base()
        {
            this.wh = wh;
            this.cw = cw;
            pm = wh.PMOUSE;
            m = wh.MOUSE;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawString("Eryan 2.0", systemFont, new Point(20, 50));
            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //cw.tabControl1.Invalidate();
          //  wh.Invalidate();
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
            this.Invalidate();   
            base.OnMouseMove(e);
        }


        protected override CreateParams CreateParams
        {
            get
            {
                // Set the WS_EX_TRANSPARENT flag without enabling click-through
                CreateParams createParams = base.CreateParams;
                //if(Version.GetCurrentWindowsVersion() != Version.WindowsVersions.Win7)
                //    createParams.ExStyle |= 0x00000020;
                return createParams;
            }
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
