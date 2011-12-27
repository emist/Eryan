using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using Eryan.Input;
using Eryan.Responses;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.Wrappers;
using Eryan.UI;

namespace Eryan.InputHandler
{
    /// <summary>
    /// Handles the menu interactions
    /// </summary>
    public class MenuHandler : InputHandler
    {
        Random random;

        /// <summary>
        /// Constants for the implemented MENU items.
        /// </summary>
        public struct MENUITEMS
        {
            /// <summary>
            /// Reset the camera
            /// </summary>
            public const string RESETCAMERA = "Reset Camera";
            /// <summary>
            /// Show Solar System in map browser
            /// </summary>
            public const string SHOWSOLARSYSTEMINMAPBROWSER = "Show Solar System in Mapbrowser";
            /// <summary>
            /// Asteroid Belts
            /// </summary>
            public const string ASTEROIDBELTS = "Asteroid Belts";
            /// <summary>
            /// Planets
            /// </summary>
            public const string PLANETS = "Planets";
            /// <summary>
            /// Stargates
            /// </summary>
            public const string STARGATES = "Stargates";
            /// <summary>
            /// Stations
            /// </summary>
            public const string STATIONS = "Stations";
            /// <summary>
            /// Hide Route
            /// </summary>
            public const string HIDEROUTE = "Hide Route";
            /// <summary>
            /// Asteroid Belt 1
            /// </summary>
            public const string BELT1 = "Asteroid Belt 1";
            /// <summary>
            /// Asteroid Belt 2
            /// </summary>
            public const string BELT2 = "Asteroid Belt 2";
            /// <summary>
            /// Asteroid Belt 3
            /// </summary>
            public const string BELT3 = "Asteroid Belt 3";
            /// <summary>
            /// Asteroid Belt 4
            /// </summary>
            public const string BELT4 = "Asteroid Belt 4";
            /// <summary>
            /// Asteroid Belt 5
            /// </summary>
            public const string BELT5 = "Asteroid Belt 5";
            /// <summary>
            /// Asteroid Belt 6
            /// </summary>
            public const string BELT6 = "Asteroid Belt 6";
            /// <summary>
            /// Asteroid Belt 7
            /// </summary>
            public const string BELT7 = "Asteroid Belt 7";
            /// <summary>
            /// Asteroid Belt 8
            /// </summary>
            public const string BELT8 = "Asteroid Belt 8";
            /// <summary>
            /// Asteroid Belt 9
            /// </summary>
            public const string BELT9 = "Asteroid Belt 9";
            /// <summary>
            /// Asteroid Belt 10
            /// </summary>
            public const string BELT10 = "Asteroid Belt 10";
            /// <summary>
            /// Asteroid Belt 11
            /// </summary>
            public const string BELT11 = "Asteroid Belt 11";
            /// <summary>
            /// Asteroid Belt 12
            /// </summary>
            public const string BELT12 = "Asteroid Belt 12";
            /// <summary>
            /// Asteroid Belt 13
            /// </summary>
            public const string BELT13 = "Asteroid Belt 13";
            /// <summary>
            /// Asteroid Belt 14
            /// </summary>
            public const string BELT14 = "Asteroid Belt 14";
            /// <summary>
            /// Approach
            /// </summary>
            public const string APPROACH = "Approach";
            /// <summary>
            /// Orbit
            /// </summary>
            public const string ORBIT = "Orbit";
            /// <summary>
            /// Look At
            /// </summary>
            public const string LOOKAT = "Look At";
            /// <summary>
            /// Lock Target
            /// </summary>
            public const string LOCKTARGET = "Lock Target";
            /// <summary>
            /// Show Info
            /// </summary>
            public const string SHOWINFO = "Show Info";
            /// <summary>
            /// Add Waypoint
            /// </summary>
            public const string ADDWAYPOINT = "Add Waypoint";
            /// <summary>
            /// Bookmark Location
            /// </summary>
            public const string BOOKMARKLOCATION = "Save Location";

            /// <summary>
            /// Dock
            /// </summary>
            public const string DOCK = "Dock";

            /// <summary>
            /// Warp to zero element
            /// </summary>
            public const string WARPTOZERO = "Warp to Within 0 m";

            /// <summary>
            /// Warp to location
            /// </summary>
            public const string WARPTOLOCATION = "Warp to Location";

            /// <summary>
            /// Warp to within 10
            /// </summary>
            public const string WARPWITHIN10 = "Within 10 km";

            public const string WARPWITHIN20 = "Within 20 km";

            public const string WARPWITHIN30 = "Within 30 km";

            public const string WARPWITHIN50 = "Within 50 km";

            public const string WARPWITHIN70 = "Within 70 km";

            public const string WARPWITHIN100 = "Within 100 km";

            /// <summary>
            /// Unlock Target
            /// </summary>
            public const string UNLOCKTARGET = "Unlock Target";

            /// <summary>
            /// Launch Drones
            /// </summary>
            public const string LAUNCHDRONES = "Launch Drones";

            /// <summary>
            /// Return to drone bay
            /// </summary>
            public const string RETURNTODRONEBAY = "Return To Drone Bay";

            /// <summary>
            /// Engage target
            /// </summary>
            public const string ENGAGETARGET = "Engage Target";

            /// <summary>
            /// Remove location
            /// </summary>
            public const string REMOVELOCATION = "Remove Location";

            /// <summary>
            /// Form Fleet With
            /// </summary>
            public const string FORMFLEETWITH = "Form Fleet With...";

            /// <summary>
            /// Stack all
            /// </summary>
            public const string STACKALL = "Stack All";

            /// <summary>
            /// Load Default
            /// </summary>
            public const string LOADDEFAULT = "Load Default";

            /// <summary>
            /// Unload To Cargohold
            /// </summary>
            public const string UNLOADTO = "Unload To";

            /// <summary>
            /// Reload All
            /// </summary>
            public const string RELOADALL = "Reload All";

            /// <summary>
            /// Reload
            /// </summary>
            public const string Reload = "Reload";

            /// <summary>
            /// Scan Range
            /// </summary>
            public const string SCANRANGE = "Scan Range";

            /// <summary>
            /// Select all
            /// </summary>
            public const string SELECTALL = "Select All";
        }
        
        /// <summary>
        /// Build a menuhandler with the given mouse, precisemouse and communicator
        /// </summary>
        /// <param name="m">The bot's Mouse</param>
        /// <param name="pm">The bot's PreciseMouse</param>
        /// <param name="com">The bot's communicator</param>
        public MenuHandler(Mouse m, PreciseMouse pm, Communicator com, KeyBoard kb)
        {
            this.pm = pm;
            pm.Speed = 20;
            this.m = m;
            this.comm = com;
            this.kb = kb;
            random = new Random();
            
        }

        public MenuHandler(WindowHandler wh)
        {
            this.pm = wh.PMOUSE;
            pm.Speed = 20;
            this.m = wh.MOUSE;
            this.comm = wh.COMMUNICATOR;
            this.kb = wh.KEYBOARD;
            random = new Random();
        }


        /// <summary>
        /// Warps to Zero on the selected item(if possible)
        /// </summary>
        /// <returns>True if successs, false otherwise</returns>
        public bool warpToZero()
        {
            return click("warp to within 0 m");
        }

        /// <summary>
        /// Open a menu at the given interface entry 
        /// </summary>
        /// <param name="entry">The interface entry to open the menu on</param>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean open(InterfaceEntry entry)
        {
            
            Point pt = new Point(random.Next(entry.X, entry.X + entry.Width), random.Next(entry.Y, entry.Y + entry.Height - 5));
            m.move(pt);
            pm.synchronize(m);
            Thread.Sleep(random.Next(300,400));
            m.click(false);
            Thread.Sleep(random.Next(1500, 1600));
            return true;
        }


        /// <summary>
        /// Select the given menuview entry if not null
        /// </summary>
        /// <param name="entry">The menu entry to select</param>
        /// <returns>true if success, false otherwise</returns>
        public bool select(MenuEntry entry)
        {
            Console.WriteLine("Selecting " + entry.Text + " at " + entry.X + " , " + entry.Y);
            pm.MissChance = 100;
            pm.Speed = 5;

            if (entry == null)
            {
                return false;
            }

            pm.move(10, random.Next(entry.X + 2, entry.X + 5), random.Next(pm.getY() - 3, pm.getY()), 0, 0, 0);
            pm.move(10, random.Next(entry.Width / 3 + entry.X, entry.Width / 2 + entry.X), random.Next(entry.Y + 1, entry.Y + 5), 0, 0, 0);

            Thread.Sleep(300);
            m.synchronize(pm);
            return true;
        }

        /// <summary>
        /// Click the given menu entry
        /// </summary>
        /// <param name="entry">The menu entry to click</param>
        /// <returns>true on sucess, false otherwise</returns>
        public bool click(MenuEntry entry)
        {
            pm.MissChance = 100;
            pm.Speed = 5;

            if (entry == null)
            {
                return false;
            }


            pm.move(10,random.Next(entry.X+2, entry.X + 5), random.Next(pm.getY()-3, pm.getY()), 0, 0, 0);
            pm.move(10,random.Next(entry.Width/3 + entry.X, entry.Width / 2 + entry.X), random.Next(entry.Y+ 1, entry.Y + 5), 0, 0, 0);
            Thread.Sleep(300);
            pm.click(true);
            m.synchronize(pm);
            return true;
        }

        /// <summary>
        /// Returns the asteroid belts in the currently open menu
        /// </summary>
        /// <returns>List of menu entries representing the asteroid belts</returns>
        public List<MenuEntry> getBelts()
        {
            List<MenuEntry> belts = new List<MenuEntry>();
            BooleanResponse menuOpen = (BooleanResponse)comm.sendCall(FunctionCallFactory.CALLS.ISMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (menuOpen == null)
            {
                return null;
            }

            if (!(Boolean)menuOpen.Data)
            {
                return null;
            }
            
            MenuResponse resp = (MenuResponse)comm.sendCall(FunctionCallFactory.CALLS.GETMENUITEMS, Response.RESPONSES.MENURESPONSE);
            if (resp == null)
            {
                Console.WriteLine("couldn't get menu items");
                return null;
            }

            foreach (MenuEntry entry in (List<MenuEntry>)resp.Data)
            {
                if (entry.Text.ToLower().Contains("belt "))
                {
                    belts.Add(entry);
                }
            }

            return belts;
        }

        /// <summary>
        /// Check if there is a currently open menu
        /// </summary>
        /// <returns>Return true if there is, false otherwise</returns>
        public bool isMenuOpen()
        {
            BooleanResponse menuOpen = (BooleanResponse)comm.sendCall(FunctionCallFactory.CALLS.ISMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (menuOpen == null)
            {
                return false;
            }

            return (Boolean)menuOpen.Data;
        }

        public bool isEmpty(List<Rectangle> recs, Point pt)
        {
            foreach (Rectangle rec in recs)
                if (rec.Contains(pt))
                    return false;
            return true;
        }

        /// <summary>
        /// Open a menu on the given point
        /// </summary>
        /// <param name="pt">The client point in which to open the menu</param>
        public void open(Point pt)
        {
            m.move(pt);
            Thread.Sleep(random.Next(200, 300));
            pm.synchronize(m);
            m.click(false);
            Thread.Sleep(random.Next(300, 600));
        }

        /// <summary>
        /// Find an open area in the inflight interface
        /// </summary>
        /// <returns>An empty point in space</returns>
        public Point getOpenSpace()
        {
            List<Rectangle> recs = new List<Rectangle>();
            OverViewResponse items = (OverViewResponse)comm.sendCall(FunctionCallFactory.CALLS.GETINTERFACEWINDOWS, Response.RESPONSES.OVERVIEWRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return new Point(-1, -1);
            }

            InterfaceResponse inflight = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETINFLIGHTINTERFACE, Response.RESPONSES.INTERFACERESPONSE);
            if (inflight == null)
            {
                Console.WriteLine("inflight is null");
                return new Point(-1, -1);
            }

            Point pt = new Point(random.Next(inflight.X, inflight.X + inflight.Width), random.Next(inflight.Y, inflight.Y + inflight.Height));

            foreach (OverViewEntry it in (List<OverViewEntry>)items.Data)
            {
                recs.Add(new Rectangle(it.X, it.Y, it.Width, it.Height));
            }

            while (!isEmpty(recs, pt))
                pt = new Point(random.Next(inflight.X, inflight.X + inflight.Width), random.Next(inflight.Y, inflight.Y + inflight.Height));

            return pt;
        }

        /// <summary>
        /// Select the given string in the currently open menu
        /// </summary>
        /// <param name="menuItem">The text in the menu to select</param>
        /// <returns>True if success, false otherwise</returns>
        public bool select(string menuItem)
        {

            Thread.Sleep(300);

            if (!comm.connect())
                return false;
            
            BooleanResponse menuOpen = (BooleanResponse)comm.sendCall(FunctionCallFactory.CALLS.ISMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (menuOpen == null)
            {
                return false;
            }

            if (!(Boolean)menuOpen.Data)
            {
                InterfaceResponse inflight = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETINFLIGHTINTERFACE, Response.RESPONSES.INTERFACERESPONSE);
                if (inflight == null)
                {
                    return false;
                }

                Point pt = getOpenSpace();
                if (pt.X == -1 || pt.Y == -1)
                    return false;

                m.move(pt);
                pm.x = m.x;
                pm.y = m.y;

                Thread.Sleep(300);
                m.click(false);
                Thread.Sleep(600);
                
            }
            
            InterfaceResponse resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, menuItem, Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
            {
                if(isMenuOpen())
                    kb.sendChar((char)KeyBoard.VKeys.VK_ESCAPE);
                return false;
            }

            pm.MissChance = 100;
            pm.Speed = 5;
 
            pm.move(10, resp.X + 5, pm.getY(), 0, 0, 0);
            pm.move(10, resp.Width/2 + resp.X, resp.Y + 5, 0, 0, 0);

            //Synchronize both mouse devices
            m.synchronize(pm);
            
            return true;
        }


        /// <summary>
        /// Clicks the given menu item
        /// </summary>
        /// <param name="menuItem">The text of the menu item to click</param>
        /// <returns>True if success, false otherwise</returns>
        public bool click(string menuItem)
        {

            

            if (!comm.connect())
                return false;


            InterfaceResponse resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, menuItem, Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return false;


            pm.MissChance = 100;
            pm.Speed = 5;

            Random random = new Random();

            
            pm.move(10, resp.X+5, pm.getY(), 0, 0, 0);
            pm.move(10, resp.X + 5, resp.Y + 6, 0, 0,0);


            //Synchronize both mouse devices
            m.synchronize(pm);
           
            Thread.Sleep(600);

            pm.click(true);
            return true;
        }
    }
}
