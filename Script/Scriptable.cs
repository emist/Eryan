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

    public abstract class Scriptable : MarshalByRefObject
    {
        public Mouse m;
        public PreciseMouse pm;
        public MenuHandler menuHandler;
        public Communicator comm;
        public OverviewHandler overViewHandler;

        public void initializeInputs(PreciseMouse pm, Mouse m, MenuHandler mh, Communicator comm, OverviewHandler over)
        {
            this.m = m;
            this.pm = pm;
            this.menuHandler = mh;
            this.comm = comm;
            this.overViewHandler = over;
        }

        public abstract Boolean onStart();

        public abstract Boolean onFinish();

        public abstract int run();

    }
   
}
