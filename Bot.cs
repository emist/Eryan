using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Eryan
{
    public partial class Bot 
    {

        Thread botThread;
        WindowHandler bot;

        private delegate void clientWindowDelegate(ClientWindow cw);
        //Thread[] botWindowThreads = new Thread[20];
        //WindowHandler[] bots = new WindowHandler[20];

        public Bot()
        {
       //     InitializeComponent();

//            Load += new EventHandler(Program_Load);
            
        }

        public override bool Equals(object obj)
        {
            return obj is Bot && this == (Bot)obj;
        }

        private void Program_Load(object sender, EventArgs e)
        {
            //CreateBot();
        }


        public void initializeBot(ClientWindow cw)
        {
            
            //botThread = new Thread(new ParameterizedThreadStart(SpawnForm));
            bot = new WindowHandler();
            bot.bringToFront();
            bot.setTopLevel(false);
            bot.setVisible(true);
            bot.setFormBorderStyle(FormBorderStyle.None);
            bot.setDockStyle(DockStyle.Fill);
            

            cw.addControlToTab(bot);
            //botThread.Start(bot);
            
        }

        public WindowHandler getHandle()
        {
            return bot;
        }

        public uint getPid()
        {
            return bot.getPid();
        }

        public Thread getThread()
        {
            return botThread;
        }

        public void SpawnForm(object firm)
        {
            Application.Run(firm as Form);
        }

        public void update()
        {

            if (bot.getMouse().cursorDistance(new Point(750, 900)) > 5)
                bot.getMouse().move(new Point(750, 900));
            else
                bot.getMouse().move(new Point(500, 600));

            Console.WriteLine("Bot pid = " + getPid());
        }

        //Destroy Bot
        public Boolean DestroyBot()
        {
            return true;
        }


    }
}
