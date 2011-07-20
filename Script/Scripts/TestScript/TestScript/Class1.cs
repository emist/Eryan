using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eryan.Script;
using Eryan;
using Eryan.Factories;
using Eryan.Responses;

namespace Script
{
    public class Script : Scriptable
    {
        public override Boolean onStart()
        {
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        public override int run()
        {
            comm.connect();
            OverViewResponse resp = (OverViewResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWITEMS, Response.RESPONSES.OVERVIEWRESPONSE);
            if(resp == null)
            {
                Console.WriteLine("Response is null");
                return 1000;
            }
            resp.HandleResponse();
            //menuHandler.select("planets");
            //menuHandler.select("bourynes III");
            //menuHandler.click("warp to within 0 m");
            Console.WriteLine("Script is running!!");
            return 2000;
        }

    }
}
