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
