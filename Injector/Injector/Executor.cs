using System;
using System.Collections.Generic;
using System.Text;
using Syringe;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Eryan
{
    public class Executor
    {
        Injector syringe;



        [StructLayout(LayoutKind.Sequential)]
        struct MessageStruct
        {
            [CustomMarshalAs(CustomUnmanagedType.LPWStr)]
            public string Text;
            [CustomMarshalAs(CustomUnmanagedType.LPWStr)]
            public string Caption;
        }

        public Injector getSyringe()
        {
            return syringe;
        }

        public void Inject(String dll, String process)
        {
            
            //String dll = "C:\\Users\\emist\\Documents\\Visual Studio 2008\\Projects\\InjectDLL\\Debug\\InjectDLL.dll";

            Console.WriteLine("Trying to inject " + dll + " into " + process);
            MessageStruct messageData = new MessageStruct() { Text = "Custom Message", Caption = "Custom Message Box" };
            syringe = new Injector(Process.GetProcessesByName(process)[0]);
            syringe.InjectLibrary(dll);
            Console.WriteLine(dll + " injected into " + process);
         
        }
    }

}
