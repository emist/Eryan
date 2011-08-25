using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eryan.Responses;
using Eryan.IPC;
using Eryan.Input;
using Eryan.InputHandler;
using Eryan.Wrappers;

namespace Eryan.InputHandler
{
    public abstract class ChatHandler
    {
        public abstract bool userlistScrollDown();
        public abstract bool userlistScrollUp();
        public abstract bool openMenu(string charname);
        public abstract InterfaceEntry userlistGetEntry(string charname);
        public abstract int userlistBottom();
        public abstract int userlistTop();
        public abstract InterfaceEntry userlistScrollBar();

    }
}
