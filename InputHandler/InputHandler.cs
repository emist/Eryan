using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eryan.Input;
using Eryan.IPC;

namespace Eryan.InputHandler
{
    public abstract class InputHandler
    {
        protected Mouse m;
        protected Communicator comm;
        protected PreciseMouse pm;
        
        public Mouse MOUSE
        {
            get
            {
                return m;
            }
        }

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
