using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Syringe;
using System.Text;
using System.IO;
using Eryan.Factories;
using Eryan.IPC;
using Eryan.Responses;
using Eryan.UI;

namespace Eryan
{
    static class Client
    {

       
        static ClientWindow cWindow;

        /// <summary>
        /// Spawns the Eryan Client on a new thread
        /// </summary>
        public static void createWindow()
        {
            cWindow = new ClientWindow();
            Application.Run(cWindow);
        }

        /// <summary>
        /// The main entry point of the application
        /// </summary>

        [STAThread]
        static void Main()
        {

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EulaView eula;

            if (!System.IO.File.Exists("r"))
            {
                eula = new UI.EulaView();
                Application.Run(eula);
                if (eula.Status == false)
                {
                    MessageBox.Show("You must accept the EULA in order to use Eryan(E2)", "EULA Declined");
                    return;
                }
                else
                {
                    System.IO.File.Create("r");
                }
            }


            Thread ClientThread = new Thread(new ThreadStart(createWindow));
            ClientThread.SetApartmentState(ApartmentState.STA);
            ClientThread.Start();

            while (cWindow == null)
            {
                Thread.Sleep(300);
            }

            
            /////DEBUGGING STUFF

 
            /*
            Executor injector = new Executor();
            String dll = "C:\\Black.dll";
            injector.Inject(dll, "ExeFile");

            if (injector.getSyringe() == null)
                return;

            injector.getSyringe().CallExport(dll, "dropServer");


            //Communicator comm = new Communicator("\\\\.\\pipe\\TestChannel");


            injector.getSyringe().CallExport(dll, "startServer");

            /*
            Response resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYNAMELOGIN, "username", Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return;

            
            resp.HandleResponse();
            */

            /*
            Response resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTLOGIN, "Username", Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return;

            resp.HandleResponse();
            */


            /*
            Response resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, "BOURYNES VII - ASTEROID BELT 1", Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return;

            resp.HandleResponse();
            
            
            Console.WriteLine("Response name " + ((List<string>)resp.Data)[0]) ;

            while (true)
            {
                Thread.Sleep(1000);
            }
            */

            
            cWindow.createBot();

            
            while(cWindow.getBots().Count < 1)
                Thread.Sleep(100);

            Bot bot1 = cWindow.getBots()[0];
            
            
            WindowHandler BotHandle = bot1.getHandle();



            
            while (true)
            {
                
                if ( bot1 != null)
                {
                    bot1.update();
                }
            
                System.Threading.Thread.Sleep(1000);
            }
            

        }

    }
}
