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

        //Thread[] botWindowThreads = new Thread[20];
        //WindowHandler[] bots = new WindowHandler[20];

        public Bot()
        {
       //     InitializeComponent();

//            Load += new EventHandler(Program_Load);
            
        }


        private void Program_Load(object sender, EventArgs e)
        {
            //CreateBot();
        }




        public Thread CreateBot()
        {
            botThread = new Thread(new ParameterizedThreadStart(ShowForm));
            bot = new WindowHandler();
            botThread.Start(bot);
            return botThread;
        }

        public WindowHandler getBot()
        {
            return bot;
        }

        public uint getPid()
        {
            return bot.getPid();
        }

        public void ShowForm(object firm)
        {
            Application.Run(firm as Form);
        }

        public void update()
        {
  
        }

        //Destroy Bot
        public Boolean DestroyBot()
        {
            return true;
        }


    }
}
