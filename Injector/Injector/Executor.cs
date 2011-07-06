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
        [StructLayout(LayoutKind.Sequential)]
        struct MessageStruct
        {
            [CustomMarshalAs(CustomUnmanagedType.LPWStr)]
            public string Text;
            [CustomMarshalAs(CustomUnmanagedType.LPWStr)]
            public string Caption;
        }

        public void Inject(String dll, String process)
        {
            
            //String dll = "C:\\Users\\emist\\Documents\\Visual Studio 2008\\Projects\\InjectDLL\\Debug\\InjectDLL.dll";

            Console.WriteLine("Trying to inject " + dll + " into " + process);
            MessageStruct messageData = new MessageStruct() { Text = "Custom Message", Caption = "Custom Message Box" };
            Injector syringe = new Injector(Process.GetProcessesByName(process)[0]);
            syringe.InjectLibrary(dll);
            Console.WriteLine(dll + " injected into notepad, trying to call void Initialise() with no parameters");
            Console.ReadLine();
            syringe.CallExport("InjectDLL.dll", "Initialize");
            Console.WriteLine("Waiting...");
               // Console.ReadLine();
               // syringe.CallExport("InjectDLL.dll", "process_expression");
               // Console.ReadLine();
                //Console.WriteLine("Trying to call InitWithMessage( PVOID ) with custom data {0}", messageData);
                //Console.ReadLine();
                //syringe.CallExport(dll, "InitWithMessage", messageData);
                //Console.WriteLine("Waiting...");
                //Console.ReadLine();
            //syringe.EjectLibrary(dll);
            Console.WriteLine(dll + " should be ejected");
            Console.ReadLine();
        }
    }

}
