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

            
            Client client = new Client();
            

            Thread clientThread = client.CreateBot();

            while (clientThread.IsAlive)
            {
                WindowHandler bot1 = client.getBotByOrder(0);
                bot1.getKeyBoard().sendKeyPresses("HelloWorld", 100, 600);
                bot1.getMouse();

                

                System.Threading.Thread.Sleep(1000);
            }





           
            //System.Console.WriteLine("Program Exited");


            

            


            
        }
    }
}
