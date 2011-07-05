using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Eryan
{
    static class Bot
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //WindowHandler bot = new WindowHandler();
            //Application.Run(bot);

            Client client = new Client();
            //Application.Run(client);

            Thread clientThread = client.CreateBot();

            while (clientThread.IsAlive)
            {
                WindowHandler bot1 = client.getBot(0);
                bot1.sendKeyPresses("");
                System.Threading.Thread.Sleep(1000);
            }

            System.Console.WriteLine("Program Exited");


            //System.Threading.Thread.Sleep(3000);

            


            
        }
    }
}
