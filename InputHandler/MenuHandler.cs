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
    public class MenuHandler
    {
        Mouse m;
        Communicator comm;
        PreciseMouse pm;
        Random random;
        
        public MenuHandler(Mouse m, PreciseMouse pm, Communicator com)
        {
            this.pm = pm;
            pm.Speed = 20;
            this.m = m;
            this.comm = com;
            random = new Random();
            
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

                m.click(false);
                Thread.Sleep(300);
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
            m.x = pm.x;
            m.y = m.y;
            
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
            m.x = pm.x;
            m.y = m.y;

            Thread.Sleep(600);

            pm.click(true);


            return true;
        }

    }
}
