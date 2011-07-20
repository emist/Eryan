using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Input;
using Eryan.Responses;
using Eryan.IPC;
using Eryan.Factories;
using System.Drawing;
using System.Threading;


namespace Eryan.InputHandler
{
    /// <summary>
    /// Handles the menu interactions
    /// </summary>
    public class MenuHandler : InputHandler
    {
        Random random;

        public struct MENUITEMS
        {
            public const string RESETCAMERA = "Reset Camera";
            public const string SHOWSOLARSYSTEMINMAPBROWSER = "Show Solar System in Mapbrowser";
            public const string ASTEROIDBELTS = "Asteroid Belts";
            public const string PLANETS = "Planets";
            public const string STARGATES = "Stargates";
            public const string STATIONS = "Stations";
            public const string HIDEROUTE = "Hide Route";
            public const string BELT1 = "Asteroid Belt 1";
            public const string BELT2 = "Asteroid Belt 2";
            public const string BELT3 = "Asteroid Belt 3";
            public const string BELT4 = "Asteroid Belt 4";
            public const string BELT5 = "Asteroid Belt 5";
            public const string BELT6 = "Asteroid Belt 6";
            public const string BELT7 = "Asteroid Belt 7";
            public const string BELT8 = "Asteroid Belt 8";
            public const string BELT9 = "Asteroid Belt 9";
            public const string BELT10 = "Asteroid Belt 10";
            public const string BELT11 = "Asteroid Belt 11";
            public const string BELT12 = "Asteroid Belt 12";
            public const string BELT13 = "Asteroid Belt 13";
            public const string BELT14 = "Asteroid Belt 14";
            public const string APPROACH = "Approach";
            public const string LOOKAT = "Look At";
            public const string LOCKTARGET = "Lock Target";
            public const string SHOWINFO = "Show Info";
            public const string ADDWAYPOINT = "Add Waypoint";
            public const string BOOKMARKLOCATION = "Bookmark Location";

        }
        
        public MenuHandler(Mouse m, PreciseMouse pm, Communicator com)
        {
            this.pm = pm;
            pm.Speed = 20;
            this.m = m;
            this.comm = com;
            random = new Random();
            
        }

        public bool warpToZero()
        {
            return click("warp to within 0 m");
        }

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

            menuOpen.HandleResponse();

            if (!(Boolean)menuOpen.Data)
            {
                InterfaceResponse inflight = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETINFLIGHTINTERFACE, Response.RESPONSES.INTERFACERESPONSE);
                if (inflight == null)
                    return false;

                inflight.HandleResponse();

                m.move(new Point(
                    random.Next(inflight.X, inflight.X + inflight.Width), 
                    random.Next(inflight.Y, inflight.Y + inflight.Width)
                    ));
                pm.x = m.x;
                pm.y = m.y;

                Thread.Sleep(300);
                m.click(false);
                Thread.Sleep(200);
                
            }
            
            InterfaceResponse resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, menuItem.ToUpper(), Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return false;

            resp.HandleResponse();
            pm.MissChance = 100;
            pm.Speed = 5;

            //m.move(new Point(resp.X + random.Next(resp.Height - 2, resp.Height / 2), resp.Y + random.Next(resp.Width, resp.Width / 2)));
            //pm.move(10, random.Next(resp.Width + resp.X - 10, resp.Width + resp.X), resp.Y+5, 0, 0, 0);
           
            pm.move(10, resp.X + 5, pm.getY(), 0, 0, 0);
            pm.move(10, resp.Width/2 + resp.X, resp.Y + 5, 0, 0, 0);

            //Synchronize both mouse devices
            synchronizePreciseMouse(pm);
            
            return true;
        }

        public bool click(string menuItem)
        {

            

            if (!comm.connect())
                return false;


            InterfaceResponse resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, menuItem.ToUpper(), Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return false;

            resp.HandleResponse();
            pm.MissChance = 100;
            pm.Speed = 5;

            Random random = new Random();

           

//            pm.move(10, random.Next(resp.Width + resp.X-10, resp.Width + resp.X), resp.Y+10, 0 , 0, 0);

            
            pm.move(10, resp.X+5, pm.getY(), 0, 0, 0);
            pm.move(10, resp.X + 5, resp.Y + 6, 0, 0,0);


            //Synchronize both mouse devices
            synchronizePreciseMouse(pm);
           
            Thread.Sleep(600);

            pm.click(true);
            return true;
        }
    }
}
