using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Input;
using Eryan.Wrappers;
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
        
        public MenuHandler(PreciseMouse m, Communicator com)
        {
            this.pm = m;
            pm.Speed = 20;
            this.comm = com;
            
        }

        public bool select(string menuItem)
        {

            Thread.Sleep(300);

            if (!comm.connect())
                return false;

           
            InterfaceResponse resp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.FINDBYTEXTMENU, menuItem.ToUpper(), Response.RESPONSES.INTERFACERESPONSE);
            if (resp == null)
                return false;

            resp.HandleResponse();
            pm.MissChance = 100;

            Random random = new Random();

            pm.Speed = 5;

            //m.move(new Point(resp.X + random.Next(resp.Height - 2, resp.Height / 2), resp.Y + random.Next(resp.Width, resp.Width / 2)));
            //pm.move(10, random.Next(resp.Width + resp.X - 10, resp.Width + resp.X), resp.Y+5, 0, 0, 0);
           
            pm.move(10, resp.X + 5, pm.getY(), 0, 0, 0);
            pm.move(10, resp.Width/2 + resp.X, resp.Y + 5, 0, 0, 0);

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
            Thread.Sleep(600);

            pm.click(true);


            return true;
        }

    }
}
