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
    public partial class Client 
    {
        Thread[] botWindowThreads = new Thread[20];
        WindowHandler[] bots = new WindowHandler[20];
       
        public Client()
        {
       //     InitializeComponent();

//            Load += new EventHandler(Program_Load);
            
        }


        private void Program_Load(object sender, EventArgs e)
        {
            CreateBot();
        }

        //Stub
        public Thread CreateBot()
        {
            botWindowThreads[0] = new Thread(new ParameterizedThreadStart(ShowForm));
            bots[0] = new WindowHandler();
            botWindowThreads[0].Start(bots[0]);
            
            return botWindowThreads[0];
        }

        public WindowHandler getBotByOrder(int bot)
        {
            return bots[bot];
        }

        public WindowHandler getBot(uint processId)
        {
            foreach (WindowHandler window in bots)
            {
                if (window.getPid() == processId)
                    return window;
            }
            return null;
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
