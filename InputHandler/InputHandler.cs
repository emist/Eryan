using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eryan.Input;
using Eryan.IPC;

namespace Eryan.InputHandler
{
    /// <summary>
    /// The base class for all Input Handlers
    /// </summary>
    public abstract class InputHandler
    {
        /// <summary>
        /// Every input handler needs a mouse
        /// </summary>
        protected Mouse m;
        /// <summary>
        /// Every input handler needs a communicator
        /// </summary>
        protected Communicator comm;

        /// <summary>
        /// Every input handler needs a precise mouse
        /// </summary>
        protected PreciseMouse pm;

        /// <summary>
        /// Some input handlers need a keyboard
        /// </summary>
        protected KeyBoard kb;


        /// <summary>
        /// Return a reference to this handler's keyboard
        /// </summary>
        public KeyBoard KEYBOARD
        {
            get
            {
                return kb;
            }
        }
        
        /// <summary>
        /// Return a reference to this handler's mouse
        /// </summary>
        public Mouse MOUSE
        {
            get
            {
                return m;
            }
        }

        /// <summary>
        /// Return a reference to this handler's Precise Mouse
        /// </summary>
        public PreciseMouse PMOUSE
        {
            get
            {
                return pm;
            }
        }

        /// <summary>
        /// Synchronizes the mouse to the precise mouse
        /// </summary>
        /// <param name="pm">The precisemouse reference to synchronize to</param>
        public void synchronizePreciseMouse(PreciseMouse pm)
        {
            m.x = pm.x;
            m.y = pm.y;
        }

        /// <summary>
        /// Synchronizes the precise mouse to the mouse
        /// </summary>
        /// <param name="m">The mouse instance to synchronize to</param>
        public void synchronizeMouse(Mouse m)
        {
            pm.x = m.x;
            pm.y = m.y;
        }
   
    }
}
