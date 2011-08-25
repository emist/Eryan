using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Input;
using Eryan.InputHandler;
using Eryan.IPC;
using Eryan.Wrappers;
using Eryan.UI;

namespace Eryan.Script
{
    /// <summary>
    /// Scriptable interface must be implemented by all Eryan scripts
    /// </summary>

    public abstract class Scriptable
    {
        /// <summary>
        /// The script name
        /// </summary>
        protected string name = "Unamed";

        /// <summary>
        /// Is the script enabled
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Bot's Mouse handler
        /// </summary>
        public Mouse EMouse;

        /// <summary>
        /// Bot's PreciseMouse reference
        /// </summary>
        public PreciseMouse EPreciseMouse;
        /// <summary>
        /// Bot's MenuHandler reference 
        /// </summary>
        public MenuHandler EMenuHandler;

        /// <summary>
        /// Bot's communicator reference
        /// </summary>
        public Communicator ECommunicator;

        /// <summary>
        /// Bot's OverViewHandler reference
        /// </summary>
        public OverviewHandler EOverViewHandler;

        /// <summary>
        /// Reference to the bot's station handler
        /// </summary>
        public Station EStationHandler;

        /// <summary>
        /// Reference to our player's ship
        /// </summary>
        public Ship MyShip;

        /// <summary>
        /// Reference to our current eve session
        /// </summary>
        public Session ESession;

        /// <summary>
        /// Reference to this bot's camera handler
        /// </summary>
        public Camera ECamera;


        /// <summary>
        /// Reference to this bot's keyboard
        /// </summary>
        public KeyBoard EKeyboard;

        /// <summary>
        /// Random generator for script use
        /// </summary>
        public Random ERandom;

        /// <summary>
        /// This bot's local chat handler
        /// </summary>
        public LocalHandler ELocalHandler;

        /// <summary>
        /// Internal use, the Bot will initialize the script with its input handlers once its loaded into memory
        /// </summary>
        /// <param name="bot">The reference to the bot's windowhandler</param>
        public void initializeInputs(WindowHandler bot)
        {
            this.EMouse = bot.MOUSE;
            this.EPreciseMouse = bot.PMOUSE;
            this.EMenuHandler = bot.MENU;
            this.ECommunicator = bot.COMMUNICATOR;
            this.EOverViewHandler = bot.OVERVIEW;
            this.EStationHandler = bot.STATION;
            this.MyShip = bot.SHIP;
            this.ESession = bot.SESSION;
            this.ECamera = bot.CAMERA;
            this.EKeyboard = bot.KEYBOARD;
            this.ELocalHandler = bot.LOCAL;
            ERandom = new Random();
            
        }

        /// <summary>
        /// To be used by the script to initialize its state variables
        /// </summary>
        /// <returns>Return true if everything went fine, false otherwise</returns>
        public abstract Boolean onStart();

        /// <summary>
        /// Script's cleanup code
        /// </summary>
        /// <returns>Returns true if it cleaned up successfully</returns>
        public abstract Boolean onFinish();

        /// <summary>
        /// The main loop for scripts
        /// </summary>
        /// <returns>Returns the amount in miliseconds you want Eryan to keep your script thread sleeping for</returns>
        public abstract int run();

        /// <summary>
        /// Return if the script is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    
    }
   
}
