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
        public Mouse m;
        public PreciseMouse pm;
        public MenuHandler menuHandler;
        public Communicator comm;

        public void initializeInputs(PreciseMouse pm, Mouse m, MenuHandler mh, Communicator comm)
        {
            this.m = m;
            this.pm = pm;
            this.menuHandler = mh;
            this.comm = comm;
        }

        public abstract Boolean onStart();

        public abstract Boolean onFinish();

        public abstract int run();

    }
   
}
