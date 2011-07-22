using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Input;
using Eryan.InputHandler;
using Eryan.IPC;

namespace Eryan.Script
{
    /// <summary>
    /// Scriptable interface must be implemented by all Eryan scripts
    /// </summary>

    public abstract class Scriptable
    {
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
        /// Internal use, the Bot will initialize the script with its input handlers once its loaded into memory
        /// </summary>
        /// <param name="pm">The bot's precisemouse reference</param>
        /// <param name="m">The bot's mouse reference</param>
        /// <param name="mh">The bot's menuhandler reference</param>
        /// <param name="comm">The bot's communicator reference</param>
        /// <param name="over">The bot's overviewhandler reference</param>
        public void initializeInputs(PreciseMouse pm, Mouse m, MenuHandler mh, Communicator comm, OverviewHandler over)
        {
            this.EMouse = m;
            this.EPreciseMouse = pm;
            this.EMenuHandler = mh;
            this.ECommunicator = comm;
            this.EOverViewHandler = over;
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

    }
   
}
