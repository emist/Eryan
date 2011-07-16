using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Eryan.Wrappers;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.InputHandler;


namespace Eryan
{
    public partial class Bot 
    {

        Thread botThread;
        WindowHandler bot;
        Response resp;
        Communicator com;
        MenuHandler menuHandler;
        
        //This needs to go when the buttons get implemented properly
        ClientWindow cw;

        bool responseSet = false;


        private delegate void clientWindowDelegate(ClientWindow cw);
        
        public Bot()
        {}

        /// <summary>
        /// Check if obj is the same object in memory as this, used solely for the BotFetcher
        /// </summary>
        /// <param name="obj">Other object reference</param>
        /// <returns>True if obj is us, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return obj is Bot && this == (Bot)obj;
        }


        /// <summary>
        /// Initializes the bot by creating a new WindowHandler and adding it as a tab to the ClientWindow
        /// </summary>
        /// <param name="cw">A reference to the ClientWindow</param>
        public void initializeBot(ClientWindow cw)
        {
            bot = new WindowHandler(cw);
            bot.bringToFront();
            bot.setTopLevel(false);
            bot.setVisible(true);
            
            bot.setFormBorderStyle(FormBorderStyle.None);
            bot.setDockStyle(DockStyle.Fill);

            cw.tabControl1.TabPages[0].Controls.Add(bot);
            cw.tabControl1.TabPages[0].Text = "Bot";

            this.cw = cw;
            //DEBUGGING STUFF
            com = new Communicator("\\\\.\\pipe\\TestChannel");
            menuHandler = new MenuHandler(bot.getMouse(), com);
            
        }

        /// <summary>
        /// Gets a reference to the handler in charge of this bot
        /// </summary>
        /// <returns>Returns a reference to the WindowHandler in charge of this bot</returns>
        public WindowHandler getHandle()
        {
            return bot;
        }

        /// <summary>
        /// Get the PID associated with this bot(The EVE process)
        /// </summary>
        /// <returns>The PID associated with this bot.</returns>
        public uint getPid()
        {
            return bot.getPid();
        }

        /// <summary>
        /// Gets the reference to this bot's thread.
        /// </summary>
        /// <returns>Reference to the bot's thread.</returns>
        public Thread getThread()
        {
            return botThread;
        }

        /// <summary>
        /// Unusued
        /// </summary>
        /// <param name="firm"></param>
        public void SpawnForm(object firm)
        {
            Application.Run(firm as Form);
        }


        /// <summary>
        /// Bot's logic loop
        /// </summary>
        public void update()
        {
            if (!cw.running)
            {
                Console.WriteLine("Not running");
                return;
            }

            Console.WriteLine("Bot running");

            /*
             * BS DEBUGGING STUFF
             */


            menuHandler.select("planets");   
            menuHandler.select("bourynes I");
            menuHandler.click("warp to within 0 m");
                       

            
            /*
            if (bot.getMouse().cursorDistance(new Point( ((InterfaceResponse)resp).X, ((InterfaceResponse)resp).Y)) > 5)
                bot.getMouse().move(new Point( ((InterfaceResponse)resp).X, ((InterfaceResponse)resp).Y));
            else
                bot.getMouse().move(new Point(500, 600));
             */
            
            Console.WriteLine("Bot pid = " + getPid());
        }

        /// <summary>
        /// Cleanup the bot
        /// </summary>
        /// <returns>Whether the bot was cleaned up successfully</returns>
        public Boolean DestroyBot()
        {
            return true;
        }


    }
}
