using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Pipes;


//"\\\\.\\pipe\\TestChannel"

namespace Eryan.IPC
{
    /// <summary>
    /// Named pipe representation
    /// </summary>
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

        

        /// <summary>
        /// Constructs a pipe with the given name
        /// </summary>
        /// <param name="name">Name of the pipe to build</param>
        public Pipe(String name)
        {
            pipeName = name;
        }

        public bool initialize()
        {
            const short FILE_ATTRIBUTE_NORMAL = 0x80;
            const short INVALID_HANDLE_VALUE = -1;
            const uint GENERIC_READ = 0x80000000;
            const uint GENERIC_WRITE = 0x40000000;
            const uint CREATE_NEW = 1;
            const uint CREATE_ALWAYS = 2;
            const uint OPEN_EXISTING = 3;

            //Console.WriteLine("Waiting for pipe");
            if (!WaitNamedPipe(pipeName, 2500))
            {
                Console.WriteLine("Error: cannot wait for the named pipe.");
                Console.WriteLine(Marshal.GetLastWin32Error());
                return false;
            }


            npipe = CreateFile(pipeName,
                    FileAccess.ReadWrite,
                    0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);



            if ((int)npipe == INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("Error: cannot open named pipe\n");
                return false;
            }
            return true;

        }
        /// <summary>
        /// Calls functions on Black
        /// </summary>
        /// <param name="fcall">The functionCall object representing the function to be called</param>
        /// <returns>The byte representation of the Response object</returns>
        public byte[] pipeClient(eveobjects.functionCall fcall)
        {
            lock (this)
            {


                //Console.WriteLine("Connecting to pipe");


                uint bread;
                uint bsent;
                byte[] buf = new byte[10000];

                NativeOverlapped n = new NativeOverlapped();



                //Console.WriteLine("Writing to server");

                if (!WriteFile(npipe, fcall.ToByteArray(), (uint)fcall.SerializedSize, out bsent, ref n))
                {
                    Console.WriteLine(Marshal.GetLastWin32Error());
                    Console.WriteLine(pipeName);
                    Console.WriteLine("Error writing the named pipe\n");
                    //return null;
                }

                byte[] recvdata = new byte[500];

                //Console.WriteLine("Reading from server");


                ReadFile(npipe, buf, 10000, out bread, IntPtr.Zero);

                byte[] tempbuf = new byte[bread];


                for (int i = 0; i < bread; i++)
                    tempbuf[i] = buf[i];

                buf = tempbuf;


                return buf;
            }
        }

        /// <summary>
        /// Check if the pipe is ready for reading/writing
        /// </summary>
        /// <returns>True is ready, false if not</returns>
        public bool isReady()
        {
            //Console.WriteLine("Waiting on pipe");
            /*
            if (!WaitNamedPipe(pipeName, 2500))
            {
                Console.WriteLine("Error: cannot wait for the named pipe.");
                Console.WriteLine(Marshal.GetLastWin32Error());
                return false;
            }
             */
            return true;
        }

    }
}
