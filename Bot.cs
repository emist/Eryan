using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;


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
                bot1.getMouse().moveMouse(new Point(300, 300));
                bot1.getMouse().move(new Point(50, 60));
                Console.WriteLine("Bot1 pid: " + bot1.getPid());
                
                

                System.Threading.Thread.Sleep(1000);
            }





           
            //System.Console.WriteLine("Program Exited");


            

            


            
        }
    }
}
