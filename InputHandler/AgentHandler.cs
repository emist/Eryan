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
    public class AgentHandler : InputHandler
    {
        Communicator com;
        Mouse m;
        PreciseMouse pm;
        Random ran;
        MenuHandler mh;
        KeyBoard kb;
        Session sess;

        /// <summary>
        /// Handles the agent interaction
        /// </summary>
        /// <param name="wh"></param>
        public AgentHandler(WindowHandler wh)
        {
            com = wh.COMMUNICATOR;
            m = wh.MOUSE;
            pm = wh.PMOUSE;
            ran = new Random();
            mh = wh.MENU;
            kb = wh.KEYBOARD;
            sess = wh.SESSION;
        }

        /// <summary>
        /// Accept the currently open mission
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool acceptMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENTMISSIONACCEPTBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }
        
        /// <summary>
        /// Requets a mission from the agent we are conversing with
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public bool requestMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENTREQMISSIONBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Delay the currently offered mission
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool delayMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENTDELAYMISSIONBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Close the currently viewing mission
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool closeMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENTMISSIONCLOSEBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Decline the currently offered mission
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool declineMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall("getAgentMissionDeclineBtn", Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);

            Button yes = sess.getYesButton();
            if (yes != null)
                sess.click(yes);

            return true;
        }

        /// <summary>
        /// Quit the currently active mission
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool quitMission()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETAGENTMISSIONQUITBTN, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return false;

            m.move(new Point(ran.Next(iresp.X + 10, iresp.X + iresp.Width - 10), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            Thread.Sleep(ran.Next(300, 400));
            pm.synchronize(m);

            Button yes = sess.getYesButton();
            if (yes != null)
                sess.click(yes);
            return true;
        }

        /// <summary>
        /// Get the mission text from the agent
        /// </summary>
        /// <returns>The mission text or null otherwise</returns>
        public string getMissionText()
        {
            StringResponse sresp = (StringResponse)com.sendCall("getAgentMissionText", Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
                return null;

            return (string)sresp.Data;
        }


    }
}
