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





        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Bot bot1 = new Bot();


            Thread bot1Thread = bot1.CreateBot();

            Point location = new Point();

            WindowHandler BotHandle = bot1.getBot();

            while (bot1Thread.IsAlive)
            {
                
                //bot1.getKeyBoard().sendKeyPresses("HelloWorld", 100, 600);
                //bot1.getMouse().moveMouse(new Point(30, 30));

                if (BotHandle.getMouse().cursorDistance(new Point(750, 900)) > 5)
                    BotHandle.getMouse().move(new Point(750, 900));
                else
                    BotHandle.getMouse().move(new Point(500, 600));


                Console.WriteLine("Bot1 pid: " + bot1.getPid());



                System.Threading.Thread.Sleep(1000);
            }

        }

    }
}
