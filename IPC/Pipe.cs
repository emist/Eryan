using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

//"\\\\.\\pipe\\TestChannel"

namespace Eryan.IPC
{
    class Pipe
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer,
           uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
           [In] ref System.Threading.NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll")]
        static extern bool WaitNamedPipe(string lpNamedPipeName, uint nTimeOut);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
           string fileName,
           [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
           [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
           IntPtr securityAttributes,
           [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
           int flags,
           IntPtr template);

        IntPtr npipe;
        String pipeName;

        public Pipe(String name)
        {
            pipeName = name;
        }

        public byte[] pipeClient()
        {
            const short FILE_ATTRIBUTE_NORMAL = 0x80;
            const short INVALID_HANDLE_VALUE = -1;
            const uint GENERIC_READ = 0x80000000;
            const uint GENERIC_WRITE = 0x40000000;
            const uint CREATE_NEW = 1;
            const uint CREATE_ALWAYS = 2;
            const uint OPEN_EXISTING = 3;


            if (!WaitNamedPipe(pipeName, 25000))
            {
                Console.WriteLine("Error: cannot wait for the named pipe.");
                return null;
            }

            npipe = CreateFile(pipeName,
                                FileAccess.ReadWrite,
                                0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);

            if ((int)npipe == INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("Error: cannot open named pipe\n");
                return null;
            }

            NativeOverlapped n = new NativeOverlapped();
            byte[] buf = new byte[300];

            uint bread;

            if (!WriteFile(npipe, Encoding.ASCII.GetBytes("login"), (uint)Encoding.ASCII.GetBytes("login").Length, out bread, ref n))
            {
                Console.WriteLine("Error writing the named pipe\n");
                return null;
            }
            if (ReadFile(npipe, buf, 1024, out bread, IntPtr.Zero))
            {
                buf[bread] = (byte)0;

                string output = Encoding.ASCII.GetString(buf, 0, Array.IndexOf(buf, (byte)0));
                output = output.Trim();
                Console.WriteLine("Received: '" + output + "'");
            }

            return buf;
        }

    }
}
