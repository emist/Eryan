using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Eryan.Singleton;


namespace Eryan.Input
{

    /// <summary>
    /// Superclass for all input devices
    /// </summary>
    public class InputDevice
    {
        /// <summary>
        /// Window Handle that this input device is attached to
        /// </summary>
        protected IntPtr appWin = IntPtr.Zero;

        /// <summary>
        /// Random number generator to be shared among subclasses
        /// </summary>
        protected Random random = new Random();

        /// <summary>
        /// The pid of the EVE process this device is attached to
        /// </summary>
        protected uint pid = 0;


        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        protected static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
                  CharSet = CharSet.Unicode, ExactSpelling = true,
                  CallingConvention = CallingConvention.StdCall)]
        protected static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr FindWindow(IntPtr lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

       
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        protected static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, UInt32 wParam, IntPtr lparam);

        /// <summary>
        /// Associate this device to a windows handler
        /// </summary>
        /// <param name="hwnd"></param>

        public void setWindowHandle(IntPtr hwnd)
        {
            appWin = hwnd;
        }

        /// <summary>
        /// Build the LPARAM, internal winAPI parameter
        /// </summary>
        /// <param name="LoWord"></param>
        /// <param name="HiWord"></param>
        /// <returns></returns>
        protected int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        /// <summary>
        /// Get the bot's drawable screen reference
        /// </summary>
        /// <param name="pid">The bot's attached pid</param>
        /// <returns>The drawing screen</returns>
        protected Utils fetchScreen(uint pid)
        {
            return DrawAbleScreenFetcher.fetch(pid);
        }

        /// <summary>
        /// Gets the pid that the bot who owns this inputdevice is attached to
        /// </summary>
        /// <returns>The attached pid</returns>
        public uint getPid()
        {
            return pid;
        }
        
        /// <summary>
        /// Associate this device to an EVE process id.
        /// </summary>
        /// <param name="pid">The EVE process id to associate to</param>
        public void setPid(uint pid)
        {
            this.pid = pid;
        }

    }
}
