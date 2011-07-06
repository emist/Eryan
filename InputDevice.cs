using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Eryan
{
    public class InputDevice
    {
        protected IntPtr appWin = IntPtr.Zero;
        protected Random random = new Random();
        protected uint pid = 0;


        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        protected static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
                  CharSet = CharSet.Unicode, ExactSpelling = true,
                  CallingConvention = CallingConvention.StdCall)]
        protected static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        protected static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public void setWindowHandle(IntPtr hwnd)
        {
            appWin = hwnd;
        }

        protected int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        protected Utils fetchScreen(uint pid)
        {
            return DrawAbleScreenFetcher.fetch(pid);
        }

        protected uint getPid()
        {
            return pid;
        }

        public void setPid(uint pid)
        {
            this.pid = pid;
        }

    }
}
