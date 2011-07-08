using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Eryan
{
    static class Eryan
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        

        public static void SpawnForm(object firm)
        {
            Application.Run(firm as Form);
        }

        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            ClientWindow cWindow = new ClientWindow();
            Thread ClientThread = new Thread(new ParameterizedThreadStart(SpawnForm));
            ClientThread.Start(cWindow);

            Point location = new Point();
     
            Bot bot1 = cWindow.createBot();

            while (bot1 == null)
                Thread.Sleep(100);

            //WindowHandler BotHandle = bot1.getHandle();


            /*

            while (true)
            {
                
                //bot1.getKeyBoard().sendKeyPresses("HelloWorld", 100, 600);
                //bot1.getMouse().moveMouse(new Point(30, 30));
                if (bot1 != null)
                {
                    if (BotHandle.getMouse().cursorDistance(new Point(750, 900)) > 5)
                        BotHandle.getMouse().move(new Point(750, 900));
                    else
                        BotHandle.getMouse().move(new Point(500, 600));


                    Console.WriteLine("Bot1 pid: " + bot1.getPid());
                }
            */


                System.Threading.Thread.Sleep(1000);
            //}


        }

    }
}
