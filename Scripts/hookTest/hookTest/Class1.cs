using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Text.RegularExpressions;

using Eryan;
using Eryan.Script;
using Eryan.Factories;
using Eryan.Responses;
using Eryan.Wrappers;
using Eryan.InputHandler;

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
            ECommunicator.connect();
            EMouse.Speed = 30;
            EPreciseMouse.Speed = 20;
            
            //EOverViewHandler.readOverView();
            //Console.WriteLine(EOverViewHandler.getEntry("Asteroid").Sections[0]);

            //MyShip.openCargo();

            StringResponse tresp = (StringResponse)ECommunicator.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve selected item");
                return -1;
            }


            Regex reg = new Regex("[0-9]*,*[0-9]+" + @"." + "[0-9]+" + @"/");
            string result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                try
                {
                    result = result.Substring(0, result.Length - 1);
                    Console.WriteLine(Convert.ToDouble(result));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }
            return 1000;


           // Console.WriteLine(MyShip.GetCargoSpaceRemaining());
           // Console.WriteLine(MyShip.GetCargoFilled());

            /*
            SystemResponse sresp = (SystemResponse)ECommunicator.sendCall(FunctionCallFactory.CALLS.GETSYSTEMINFORMATION, Response.RESPONSES.SOLARYSYSTEMRESPONSE);
            if (sresp == null)
            {
            }
           

            Regex reg = new Regex("[0-1].[0-9]");

            //Console.WriteLine(sresp.Info);

            Console.WriteLine(reg.Match((string)sresp.Info).Value);

            /*
            <b>Jita</b>
            Docked in<t>&gt;<t><b>Jita IV - Moon 4 - Caldari Navy Assembly Plant</b><br><t>
            &gt;<t><b>Caldari State</b><br>Constellation<t>&gt;<t>Kimotoro<br>Region<t>&gt;
            <t>The Forge<br>Security Level<t>&gt;<t><color=0xff4dffccL>0.9</color>

            /*
            Regex reg = new Regex("<center>");
            StringResponse sresp = (StringResponse)ECommunicator.sendCall("getServerMessage", Response.RESPONSES.STRINGRESPONSE);
            if(sresp == null)
            {
            }

            string[] split = reg.Split((string)sresp.Data);
            Console.WriteLine(split[0]);
            Console.WriteLine(split[1]);
            */
            //Console.WriteLine((Boolean)iresp.Data);
            

            //MyShip.stop();
            //return 4000;

            /*
            TargetListResponse tresp = (TargetListResponse)ECommunicator.sendCall(FunctionCallFactory.CALLS.GETTARGETLIST, Response.RESPONSES.TARGETRESPONSE);
            if (tresp == null)
            {
                return 300;
            }

            
            String text = ((List<TargetEntry>)tresp.Data)[0].Name;

            Regex reg = new Regex("<br>");

            string[] split = reg.Split(text);


            reg = new Regex("[0-9]+");
            int nums;
            int distance = 0;
            string num = "";

            foreach (Capture capture in reg.Matches(split[1]))
            {
                num += capture.Value;
            }

            if (num == "")
                return 300;

            nums = Convert.ToInt32(num);

            reg = new Regex("km");
            if (reg.Match(split[1]).Value != "")
            {
                distance = nums * 1000;
                Console.WriteLine("Distance is " + distance);
            }

            
            Console.WriteLine("Distance is " + nums);

            Console.WriteLine(split[0]);
            Console.WriteLine(split[1]);

            Console.WriteLine(text);
            */

            return 2000;

        }
    }
}
