using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using Eryan.UI;
using Eryan.Wrappers;
using Eryan.Responses;
using Eryan.InputHandler;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.Input;


namespace Eryan.Wrappers
{
    /// <summary>
    /// Player's ship handler
    /// </summary>
    public class Ship
    {
        OverviewHandler overviewhandler;
        Communicator com;
        MenuHandler menu;
        PreciseMouse pm;
        Mouse m;
        Random ran = new Random();
        

        public Ship(MenuHandler mh, OverviewHandler oh, Communicator com, PreciseMouse pm, Mouse m)
        {
            overviewhandler = oh;
            this.com=com;
            menu = mh;
            this.pm = pm;
            this.m = m;
        }

        /// <summary>
        /// Returns the overview entries
        /// </summary>
        /// <returns>List of overview entries</returns>
        public List<OverViewEntry> getOverView()
        {
            if (overviewhandler.readOverView())
            {
                return overviewhandler.Items;
            }

            return null;

        }

        /// <summary>
        /// Warps to zero on the given asteroid belt
        /// </summary>
        /// <param name="beltname">The name of the belt</param>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean warpToZeroAsteroidBelt(string beltname)
        {
            bool success = menu.select(MenuHandler.MENUITEMS.ASTEROIDBELTS);
            if (!success)
                return false;
            success = menu.select(beltname);
            if (!success)
                return false;
            return warpToZero();
        }

        /// <summary>
        /// Target the given overview entry
        /// </summary>
        /// <param name="entry">The overview entry to target</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean target(OverViewEntry entry)
        {
            menu.open(entry);
            menu.click(MenuHandler.MENUITEMS.LOCKTARGET);
            return true;
        }

        /// <summary>
        /// Approach the overview item
        /// </summary>
        /// <param name="entry">Name of the item as it appears on the overview</param>
        /// <returns>true on sucess, false otherwise</returns>
        public Boolean approach(OverViewEntry entry)
        {
            bool success = menu.open(entry);
            menu.click(MenuHandler.MENUITEMS.APPROACH);
            return success;
        }

        public List<TargetEntry> getTargetList()
        {
            TargetListResponse tresp = (TargetListResponse)com.sendCall(FunctionCallFactory.CALLS.GETTARGETLIST, Response.RESPONSES.TARGETRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve targetlist");
                return null;
            }
            return (List<TargetEntry>)tresp.Data;
        }

        public Boolean activateHighPowerSlot(int num)
        {
            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETHIGHSLOT, num + "", Response.RESPONSES.INTERFACERESPONSE);
            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X, activateResp.X + activateResp.Width), ran.Next(activateResp.Y, activateResp.Y+activateResp.Height)));
            Thread.Sleep(200);
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        public Boolean isHighSlotActive(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISHIGHSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
            return (Boolean)activeResp.Data;
        }

        /// <summary>
        /// Warps to zero
        /// </summary>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean warpToZero()
        {
            return menu.click(MenuHandler.MENUITEMS.WARPTOZERO);
        }

        /// <summary>
        /// Check if we are docked
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

        public Boolean unDock()
        {

            InterfaceResponse dockbutton = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETUNDOCKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (dockbutton == null)
            {
                Console.WriteLine("dockbutton is null");
                return false;
            }

            m.move(new Point(ran.Next(dockbutton.X, dockbutton.X + dockbutton.Width), ran.Next(dockbutton.Y, dockbutton.Y + dockbutton.Height)));
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Docks ship in the given station name
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public Boolean dock(String stationName)
        {
            bool success = menu.select(MenuHandler.MENUITEMS.STATIONS);
            if (!success)
                return false;
            success = menu.select(stationName);
            if (!success)
                return false;
            success = menu.click(MenuHandler.MENUITEMS.DOCK);
            return success;
        }

        /// <summary>
        /// Return the list of items in the cargo
        /// </summary>
        /// <returns>Item entries representing cargo items</returns>
        public List<Item> getCargo()
        {
            ItemResponse resp = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETCARGOLIST, Response.RESPONSES.ITEMRESPONSE);
            if (resp == null)
            {
                Console.WriteLine("Response is null");
                return null;
            }
            return (List<Item>)resp.Data;
        }
    }
}
