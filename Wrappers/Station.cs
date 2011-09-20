using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using Eryan.Wrappers;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.Input;
using Eryan.InputHandler;
using Eryan.Responses;
using Eryan.UI;

namespace Eryan.Wrappers
{
    public class Station
    {
        PreciseMouse pm;
        Mouse m;
        MenuHandler mh;
        Communicator com;
        Random ran;

        /// <summary>
        /// Builds station handler with current mouse, pmouse, menu handler and communicator 
        /// </summary>
        /// <param name="m">Bot's mouse</param>
        /// <param name="pm">Bot's precise mouse</param>
        /// <param name="mh">Bot's menu handler</param>
        /// <param name="com">Bot's communicator</param>
        public Station(Mouse m, PreciseMouse pm, MenuHandler mh, Communicator com)
        {
            this.m = m;
            this.pm = pm;
            this.mh = mh;
            this.com = com;
            ran = new Random();
        }

        public Station(WindowHandler wh)
        {
            this.m = wh.MOUSE;
            this.pm = wh.PMOUSE;
            this.mh = wh.MENU;
            this.com = wh.COMMUNICATOR;
            ran = new Random();
        }

        /// <summary>
        /// Check if the given item is in the Station hangar
        /// </summary>
        /// <param name="name">The name of the item we are looking for</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool isItemInHangar(string name)
        {
            ItemResponse items = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETHANGARITEMS, Response.RESPONSES.ITEMRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return false;
            }

            foreach (Item it in (List<Item>)items.Data)
            {
                if (it.Name.ToLower().Equals(name.ToLower()))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Withdraw the given item stack from the station hangar by name
        /// </summary>
        /// <param name="name">Item name to withdraw</param>
        /// <returns>True on success, false otherwise</returns>
        public bool withdrawItem(string name)
        {
            if (!isItemInHangar(name))
                return false;

            ItemResponse items = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETHANGARITEMS, Response.RESPONSES.ITEMRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return false;
            }


            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("hangar is null");
                return false;
            }

            foreach (Item it in (List<Item>)items.Data)
            {
                if (it.Name.ToLower().Equals(name.ToLower()))
                {
                    m.move(new Point(ran.Next(it.X + 5, it.X + it.Width - 5), ran.Next(it.Y + 5, it.Y + it.Height - 5)));
                    Thread.Sleep(200);
                    m.holdLeftButton();
                    Thread.Sleep(200);
                    m.move(new Point(ran.Next(iresp.X + 5, iresp.X + iresp.Width - 5), ran.Next(iresp.Y + 5, iresp.Y + iresp.Height - 5)));
                    Thread.Sleep(200);
                    m.releaseLeftButton();
                    pm.synchronize(m);
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Stack All the items in the station hangar
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool stackHangarItems()
        {
            List<Rectangle> recs = new List<Rectangle>();

            ItemResponse items = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETHANGARITEMS, Response.RESPONSES.ITEMRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return false;
            }

            foreach (Item it in (List<Item>)items.Data)
            {
                recs.Add(new Rectangle(it.X, it.Y, it.Width, it.Height));
            }

            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSTATIONHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("hangar is null");
                return false;
            }

            Point pt = new Point(ran.Next(iresp.X, iresp.X + iresp.Width), ran.Next(iresp.Y + 30, iresp.Y + iresp.Height));

            while (!mh.isEmpty(recs, pt))
                pt = new Point(ran.Next(iresp.X, iresp.X + iresp.Width), ran.Next(iresp.Y + 30, iresp.Y + iresp.Height));
            
            mh.open(pt);
            Thread.Sleep(ran.Next(200, 300));
            mh.select(MenuHandler.MENUITEMS.STACKALL);
            Thread.Sleep(ran.Next(200, 300));
            mh.click(MenuHandler.MENUITEMS.STACKALL);


            return false;
        }

        /// <summary>
        /// Check if the hangar is open
        /// </summary>
        /// <returns>Returns true if it is, false otherwise</returns>
        public bool isHangarOpen()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSTATIONHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            return iresp != null;
        }

        /// <summary>
        /// Select the "Agent Tab" at the station 
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool selectAgentTab()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSTATIONAGENTTAB, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }


        /// <summary>
        /// Interact with the given agent
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool interactAgent(string name)
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENT, name, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            Thread.Sleep(ran.Next(10, 40));
            m.click(true);
            pm.synchronize(m);
            return true;
        }
        

        /// <summary>
        /// Open the address book
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool openHangar()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETNEOCOMITEMSBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("hangarbutton is null");
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 4, iresp.Y + iresp.Height - 4)));
            Thread.Sleep(ran.Next(200, 300));
            m.click(true);
            pm.synchronize(m);
            return true;
        }


        /// <summary>
        /// Deposits all items in your cargo to the station hangar
        /// </summary>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean depositAll()
        {
            ItemResponse items = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETCARGOLIST, Response.RESPONSES.ITEMRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return false;
            }

            InterfaceResponse stationHangar = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSTATIONHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            if (stationHangar == null)
            {
                Console.WriteLine("stationHangar is null");
                return false;
            }

            foreach (Item it in (List<Item>)items.Data)
            {
                m.move(new Point(ran.Next(it.X, it.X+it.Width), ran.Next(it.Y, it.Y+it.Height)));
                Thread.Sleep(500);
                m.drag(new Point(ran.Next(stationHangar.X, stationHangar.X + stationHangar.Width), ran.Next(stationHangar.Y, stationHangar.Y + stationHangar.Height)));
            }

            pm.synchronize(m);

            return true;
        }

        /// <summary>
        /// Find out if we're docked
        /// </summary>
        /// <returns>True if docked, false otherwise</returns>
        public Boolean isDocked()
        {
            InterfaceResponse dockbutton = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETUNDOCKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (dockbutton == null)
            {
                Console.WriteLine("dockbutton is null");
                return false;
            }

            return true;

        }

        /// <summary>
        /// Undock from station
        /// </summary>
        /// <returns>True if suceeded, false otherwise</returns>
        public Boolean undock()
        {
            InterfaceResponse dockbutton = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETUNDOCKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (dockbutton == null)
            {
                Console.WriteLine("dockbutton is null");
                return false;
            }

            m.move(new Point(ran.Next(dockbutton.X, dockbutton.X + dockbutton.Width), ran.Next(dockbutton.Y, dockbutton.Y+dockbutton.Height)));
            pm.synchronize(m);
            return true;
        }
    }
}
