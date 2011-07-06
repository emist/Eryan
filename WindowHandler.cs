using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System;
using System.Drawing.Drawing2D;



namespace Eryan
{

    public partial class WindowHandler : Form
    {
        private Utils drawingScreen;
        //private Utils currentTransparency = new Utils();
        Executor injector = new Executor();
        private delegate void showFormDelegate(Form form);
        private delegate void drawingScreenDelegate();
        private bool loaded = false;
        private Font systemFont = new Font("Impact", 16);
        


        //private Eve interaction objects
        KeyBoard keyboard = new KeyBoard();
        Mouse mouse = new Mouse();


        //Events we listen to
        private const uint EVENT_OBJECT_DESTROY = (uint)0x00008001L;
        private const uint EVENT_OBJECT_CREATE = (uint)0x00008000L;
        private const uint WM_PAINT = 0x000F;

        IntPtr m_hhook;


       
        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 0x8;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int WS_EX_MDICHILD = 0x40;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOSIZE = 0x1;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CLOSE = 0x10;
        private const int WS_CHILD = 0x40000000;
        enum ShowWindowCommands : int
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>      
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position.
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
            /// that owns the window is not responding. This flag should only be
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
             public int _Left;
             public int _Top;
             public int _Right;
             public int _Bottom;
        }

  
        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.DrawString("LOOOOOOOOOOOL", Font, Brushes.Blue, 50, 75);
        }
         */

        private Boolean created = false;
        private IntPtr appWin;
        private String exeName = "C:\\Program Files\\CCP\\EVE\\bin\\ExeFile.exe";
        private String processName = "ExeFile";
        private uint pid = 0;
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
                                         IntPtr hwnd, int idObject, int idChild,
                                         uint dwEventThread, uint dwmsEventTime);

        private WinEventDelegate winDel;
       

        //WinApi calls
        
        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, [Out] StringBuilder lpClassName,
        int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
                    CharSet = CharSet.Unicode, ExactSpelling = true,
                    CallingConvention = CallingConvention.StdCall)]

        private static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        private static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
                                                    WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread,
                                                    uint dwflags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, IntPtr lparam);


        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError=true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        public WindowHandler()
        {
            InitializeComponent();
            Load += new EventHandler(Program_Load);
            FormClosing += new FormClosingEventHandler(Program_FormClosing);
            winDel = new WinEventDelegate(HandleWindowChanges);
            MouseDown += new MouseEventHandler(Form1_MouseDown);
            drawingScreen = new Utils();
            
            //Move += new Form
            
     
            //TransparencyKey = Color.Magenta;
            //BackColor = Color.Magenta;
            //Paint += PaintRectangle;

        }

      
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        private void Program_Load(object sender, EventArgs e)
        {
                m_hhook = SetWinEventHook(EVENT_OBJECT_CREATE,
                EVENT_OBJECT_DESTROY, IntPtr.Zero, winDel, 0, 0, 0);
            
        }


        private void Program_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWinEvent(m_hhook);
           
        }


        public uint getPid()
        {
            uint outpid = 0;
            GetWindowThreadProcessId(appWin, out outpid);
            return outpid;
        }

        //Accessor to the Input Objects

        public KeyBoard getKeyBoard()
        {
            return keyboard;
        }

        public Mouse getMouse()
        {
            return mouse;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Rectangle rc = RectangleDrawer.Draw(this);
            //Console.WriteLine(rc.ToString());
        }

     

 


        private void HandleWindowChanges(IntPtr hWinEventHook, uint eventType,
                                         IntPtr hwnd, int idObject, int idChild,
                                         uint dwEventThread, uint dwmsEventTime)
        {

            if (EVENT_OBJECT_CREATE == eventType)
            {

                     
                 
                   

                    // Mark that control is created
               

                    // Initialize handle value to invalid
             

                    // Start the remote application
              
                    StringBuilder sb = new StringBuilder(300);

                    GetWindowText(appWin, sb, sb.Capacity);

                    if (!sb.ToString().Equals("EVE"))
                    {

                        
                        GetWindowText(hwnd, sb, sb.Capacity);

                        if (sb.ToString().Equals("EVE"))
                        {
                            appWin = hwnd;

                            RECT eveWindowRect = new RECT();

                            ShowWindow(appWin, ShowWindowCommands.Hide);
                            // Put it into this form

                            SetParent(appWin, this.Handle);


                            // Remove border and whatnot
                            SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE);

                            HandleRef windowRef = new HandleRef(this, appWin);

                            GetWindowRect(windowRef, out eveWindowRect);

                            // Move the window to overlay it on this window
                            int eveWindowWidth = eveWindowRect._Right - eveWindowRect._Left;
                            int eveWindowHeight = eveWindowRect._Bottom - eveWindowRect._Top;

                            //MoveWindow(appWin, 0, 0, eveWindowWidth-10, eveWindowHeight, true);




                            this.Width = eveWindowWidth - 10;
                            this.Height = eveWindowHeight - 10;

                            this.Size = new Size(this.Width, this.Height);
                            Point tmpLocation = Location;



                            keyboard.setWindowHandle(appWin);

                            this.Location = new Point(eveWindowRect._Left, eveWindowRect._Top);

                            loaded = true;

                            handleDrawingScreen();



                        }
                        
                    }
                    else
                    {
                        loaded = true;
                    }


                 }
                           

        }


        public void safeShow(Form form)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new showFormDelegate(safeShow), new object[] {form});
                return;
            }

            form.Show(this);

        }

        protected Boolean IsVisible()
        {
            foreach (Screen scrn in Screen.AllScreens)
                // You may prefer Intersects(), rather than Contains() <br/>
                if (scrn.Bounds.IntersectsWith(this.Bounds))
                    return true;

            return false;
        }


        protected override void OnMove(EventArgs e)
        {


            if (this.IsVisible())
            {
                handleDrawingScreen();
            }
            

            //base.OnMove(e);
 
        
        }


        protected override void OnVisibleChanged(EventArgs e)
        {

            // If control needs to be initialized/created
            if (created == false)
            {

                // Mark that control is created
                created = true;

                // Initialize handle value to invalid
                appWin = IntPtr.Zero;

                // Start the remote application
                Process p = null;
                try
                {

                    // Start the process
                    p = System.Diagnostics.Process.Start(this.exeName);

                    // Wait for process to be created and enter idle condition
                    p.WaitForInputIdle();

                    // Get the main handle
                    appWin = p.MainWindowHandle;

                }
                catch (Exception ex)
                {

                    /*
                    Process temp = null;
                    foreach (Process process in Process.GetProcessesByName(processName))
                    {
                        if (temp == null)
                            temp = process;
                        else if (process.Id > temp.Id)
                            temp = process;

                    }

                    if (temp != null)
                    {
                        appWin = temp.MainWindowHandle;
                    }
                    else    
                        MessageBox.Show(this, ex.Message, "Error");
                     */
                }

                // Put it into this form
                SetParent(appWin, this.Handle);

                // Remove border and whatnot
                SetWindowLong(appWin, GWL_STYLE, WS_VISIBLE);

                // Move the window to overlay it on this window
                MoveWindow(appWin, 0, 0, this.Width, this.Height, true);
                GetWindowThreadProcessId(appWin, out pid);


                //keyboard.setWindowHandle(appWin);

            }

            base.OnVisibleChanged(e);


            if (this.IsVisible())
            {
                handleDrawingScreen();
            }
        }


        protected override void OnHandleDestroyed(EventArgs e)
        {
            // Stop the application
            if (appWin != IntPtr.Zero)
            {

                // Post a colse message
                PostMessage(appWin, WM_CLOSE, 0, 0);

                // Delay for it to get the message
                System.Threading.Thread.Sleep(1000);

                // Clear internal handle
                appWin = IntPtr.Zero;

                pid = 0;

            }

            base.OnHandleDestroyed(e);
        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = drawingScreen.getGraphicsInstance();
            g.DrawString("LOOOOL", Font, Brushes.Orange, new Point(0, 0));
            base.OnPaint(e);
        }
        */

        private void handleDrawingScreen()
        {
            
            if(this.InvokeRequired)
            {
                this.BeginInvoke(new drawingScreenDelegate(handleDrawingScreen), new object[] {});
                return;
            }
            
            if (drawingScreen != null)
                if (drawingScreen.IsVisible())
                    drawingScreen.hideForm();

            if (!this.IsVisible())
            {
                return;
            }

            drawingScreen.setSize(new Size(this.Size.Width-10, this.Size.Height - 50));
            drawingScreen.setLocation(new Point(this.Location.X+5, this.Location.Y + 50));
            drawingScreen.setBackColor(Color.DarkGray);
            drawingScreen.setTransparencyKey();
            drawingScreen.setFormBorderStyle(FormBorderStyle.None);
            drawingScreen.setControlBox(false);
            drawingScreen.showInTaskbar(false);
            drawingScreen.setStartPosition(FormStartPosition.Manual);
            drawingScreen.setAutoScaleMode(AutoScaleMode.None);
            drawingScreen.bringToFront();
            drawingScreen.showForm();
            drawingScreen.setOwner(this);
            if (loaded)
            {
                drawingScreen.drawString("Eryan 2.0", systemFont, new Point(0, 0));
            }
            //System.Console.WriteLine(this.Size.ToString());
                        
        }

        protected override void OnResize(EventArgs e)
        {
   
            if (this.appWin != IntPtr.Zero)
            {

               
                // Move the window to overlay it on this window
                MoveWindow(appWin, 0, 0, this.Width - 10, this.Height, true);
                //Rectangle rc = RectangleDrawer.Draw(this);
               
            }

            System.Console.WriteLine( (appWin == IntPtr.Zero) + "");


            MoveWindow(appWin, 0, 0, this.Width - 10, this.Height, true);

            /*
            if (currentTransparency != null)
            {
                //currentTransparency.Hide();
                currentTransparency.hideForm();
            }
            currentTransparency.updatePlexiglass(this);
            currentTransparency.Owner = this;
            currentTransparency.bringToFront();
             */


            if (created == true)
            {
                handleDrawingScreen();
            }

           

            base.OnResize(e);

        }

    }
}

