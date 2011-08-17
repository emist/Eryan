using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Security.Policy;

using Eryan.Responses;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.InputHandler;
using Eryan.Script;
using Eryan.UI;
using Eryan.Input;
using Eryan.Util;



//Make the createpipe server take a parameter with the name of the pipe so that randomized pipe names can be used

//Looting from containers/wrecks(owner/maybe just use color in the overview/yellow not ours, white ours)

//Readout eve.session(IP, char id, etc)
//Implement open cargo(alt+c would be best)
//Write a Logger class and replace all these WriteLines


namespace Eryan
{

    /// <summary>
    /// Bot routines
    /// </summary>
    public partial class Bot 
    {
      

        Thread botThread;
        WindowHandler bot;
        Response resp;
        public Communicator com;
        public MenuHandler menuHandler;
        public Scriptable script;
        public Boolean running = true;
        public OverviewHandler over;
        Assembly assembly;
        public Boolean paused = true;
        public Boolean initialized = false;
        public Boolean input = false;
        public string scriptName;
        private List<Scriptable> backgroundScripts;
       

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

            backgroundScripts = new List<Scriptable>();
            Script.Scripts.InterfaceCloser icloser = new Script.Scripts.InterfaceCloser();
            icloser.initializeInputs(bot);
            backgroundScripts.Add(icloser);

            this.cw = cw;
            //DEBUGGING STUFF
            //com = new Communicator("\\\\.\\pipe\\TestChannel");
            menuHandler = new MenuHandler(bot.MOUSE, bot.PMOUSE, bot.COMMUNICATOR, bot.KEYBOARD);
            over = new OverviewHandler(bot.MENU, bot.MOUSE, bot.PMOUSE, bot.COMMUNICATOR);
            
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
        /// Loads the script assembly
        /// </summary>
        /// <param name="name">Name of the assembly to be loaded, namespace must be Script and main class must be named Script</param>
        /// <returns></returns>

        public Scriptable loadScript(string name)
        {

            assembly = null;
            
            try
            {
                assembly = Assembly.Load(loadFile(name));
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

            if (assembly == null)
            {
                Console.WriteLine("Couldn't load assembly");
                return null; 
            }
            
            Type script = assembly.GetType("Script.Script");

            if (script == null)
            {
                Console.WriteLine("Script Type is null");
                return null;
            }

            Scriptable scriptInstance = (Scriptable)Activator.CreateInstance(script);

            this.script = scriptInstance;
            return scriptInstance;
            
            
        }

        /// <summary>
        /// Unload the userDomain
        /// </summary>
        public void unloadUserDomain()
        {
            scriptName = null;
            script = null;
            running = false;
        }


        public byte[] loadFile(string filename)
        {

            FileStream fs = new FileStream(filename, FileMode.Open);

            byte[] buffer = new byte[(int)fs.Length];

            fs.Read(buffer, 0, buffer.Length);

            fs.Close();

            return buffer;

        }

        /// <summary>
        /// Bot's logic loop
        /// </summary>
        public void update()
        {

            
            if (paused)
            {
                //Console.WriteLine("Not running");
                return;
            }

            if (script == null)
            {
                if(scriptName != null)
                    script = loadScript(this.scriptName);
                return;
            }

            if (!running)
            {
                Console.WriteLine("Not running, Click Run");
                return;
            }

            if (!initialized)
            {
                script.initializeInputs(bot);
                initialized = !initialized;
            }

            //Loaded script needs to be on its own thread...maybe not

            int sleep = 0;
            
                //Run background scripts
            foreach (Scriptable bgScript in backgroundScripts)
            {
                try
                {
                    bgScript.run();
                }

                catch (Exception e)
                {
                    Console.WriteLine("background script fucked up");
                    Console.WriteLine(e.ToString());
                }
                
            }
            try
            {
                sleep = script.run();
            }
            catch (Exception e)
            {
                Console.WriteLine("user script fucked up");
                Console.WriteLine(e.ToString());
            }



            if (sleep < 1)
                sleep = 300;

            Thread.Sleep(sleep);
            
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
