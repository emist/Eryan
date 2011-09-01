using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;


using Eryan.Wrappers;
using Eryan.UI;
using Eryan.IPC;
using Eryan.InputHandler;
using Eryan.Input;
using Eryan.Responses;
using Eryan.Factories;


namespace Eryan.InputHandler
{
    public class LocalHandler:ChatHandler
    {
        Communicator com;
        Mouse m;
        PreciseMouse pm;
        Random ran;
        MenuHandler mh;

        public LocalHandler(WindowHandler wh)
        {
            com = wh.COMMUNICATOR;
            m = wh.MOUSE;
            pm = wh.PMOUSE;
            ran = new Random();
            mh = wh.MENU;
        }

        public override InterfaceEntry userlistScrollBar()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCHATSCROLLBAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new InterfaceEntry(iresp.X, iresp.Y, iresp.Width, iresp.Height);

        }

        public override bool userlistScrollDown()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCHATSCROLLBAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 3, iresp.X + iresp.Width - 3), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(300);
            m.drag(new Point(iresp.X, iresp.Y + iresp.Height));
            pm.synchronize(m);

            return true;
        }

        public bool userlistScrollToBottom()
        {
            InterfaceEntry scrollbar = userlistScrollBar();
            if (scrollbar == null)
                return false;

            int bottom = userlistBottom();

            m.move(new Point(ran.Next(scrollbar.X, scrollbar.X + 3), ran.Next(scrollbar.Y, scrollbar.Y + scrollbar.Height-5)));
            Thread.Sleep(ran.Next(100, 200));
            m.drag(new Point(ran.Next(scrollbar.X, scrollbar.X + 8), ran.Next(bottom - 5, bottom + 5)));
            pm.synchronize(m);

            return true;
        }

        public bool userlistScrollToTop()
        {
            InterfaceEntry scrollbar = userlistScrollBar();
            if (scrollbar == null)
                return false;

            int top = userlistTop();

            m.move(new Point(ran.Next(scrollbar.X, scrollbar.X + 3), ran.Next(scrollbar.Y, scrollbar.Y + scrollbar.Height - 5)));
            Thread.Sleep(ran.Next(100, 200));
            m.drag(new Point(ran.Next(scrollbar.X, scrollbar.X + 8), ran.Next(top - 5, top + 5)));
            pm.synchronize(m);


            return true;
        }

        public override bool userlistScrollUp()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCHATSCROLLBAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 3, iresp.X + iresp.Width - 3), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(300);
            m.drag(new Point(iresp.X, iresp.Y - iresp.Height));
            pm.synchronize(m);
            return true;   
        }

        public override InterfaceEntry userlistGetEntry(string charname)
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.FINDPLAYERINLOCAL, charname, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }
            return new InterfaceEntry(iresp.X, iresp.Y, iresp.Width, iresp.Height);
        }

        public override int userlistBottom()
        {
            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCHATBOTTOM, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return -1;
            }

            return Convert.ToInt32((string)sresp.Data);
        }

        public override int userlistTop()
        {
            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOCALCHATTOP, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return -1;
            }

            return Convert.ToInt32((string)sresp.Data);
        }

        public override bool openMenu(string charName)
        {
            int passes = 0;
            bool direction = false;
            bool found = false;
            InterfaceEntry entry = userlistGetEntry(charName);

            while (passes < 2 && entry != null)
            {

                InterfaceEntry userlist = userlistScrollBar();
                if (userlist != null)
                {

                    if (entry.X != 0)
                    {
                        found = true;
                        break;
                    }

                    if (userlist.Y + userlist.Height + 30 >= userlistBottom())
                    {
                        passes++;
                        direction = false;
                    }

                    if (userlist.Y - 30 <= userlistTop())
                    {
                        passes++;
                        direction = true;
                    }

                    if (!direction)
                        userlistScrollUp();

                    else
                        userlistScrollDown();

                    entry = userlistGetEntry(charName);
                    Thread.Sleep(200);
                }


            }
            if (found)
                mh.open(entry);
            return found;
        }
    }
}
