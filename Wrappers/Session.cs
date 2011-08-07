using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

using Eryan.Responses;
using Eryan.Factories;
using Eryan.IPC;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Hold eve session information
    /// </summary>
    public class Session
    {
        Communicator com;
        Random ran = new Random();

        /// <summary>
        /// Builds the session object with the given communicator
        /// </summary>
        /// <param name="com">The reference to the bot's communicator</param>
        public Session(Communicator com)
        {
            this.com = com;
        }


        /// <summary>
        /// Check if we are loading something
        /// </summary>
        /// <returns>True if there is a progress dialog open, false otherwise</returns>
        public bool isLoading()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISLOADING, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;
            return (Boolean)bresp.Data;
        }


        /// <summary>
        /// Check if the system menu is open
        /// </summary>
        /// <returns>True if it is, false otherwise</returns>
        public bool isSystemMenuOpen()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISSYSTEMMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;
            return (Boolean)bresp.Data;
        }

        /// <summary>
        /// Get the OK button of an interface if it exists
        /// </summary>
        /// <returns>The ok button of the interface, null if it doesn't exist</returns>
        public Button getOkButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALOKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("OK", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the cancel button of an interface if it exists
        /// </summary>
        /// <returns>The cancel button of the interface, null if it doesn't exist</returns>
        public Button getCancelButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALCANCELBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("CANCEL", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        /// <summary>
        /// Get the last server message
        /// </summary>
        /// <returns>The message or null if none exists</returns>
        public string getServerMessage()
        {

            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSERVERMESSAGE, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return null;
            }


            Regex reg = new Regex("<center>");
            string[] split = reg.Split((string)sresp.Data);
            if (split.Count() > 1)
            {
                return split[1];
            }
            return null;

        }

        /// <summary>
        /// Check if current system is undergoing an incursion
        /// </summary>
        /// <returns>Returns true if there is an incursion, false otherwise</returns>
        public Boolean isIncursionOngoing()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISINCURSIONONGOING, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
            {
                return false;
            }

            return (Boolean)bresp.Data;
        }

        /// <summary>
        /// Get the current solar system
        /// </summary>
        /// <returns>Returns a solarsystem object on success, null on failure</returns>
        public SolarSystem getSolarSystem()
        {
            SystemResponse sresp = (SystemResponse)com.sendCall(FunctionCallFactory.CALLS.GETSYSTEMINFORMATION, Response.RESPONSES.SOLARYSYSTEMRESPONSE);
            if (sresp == null)
            {
                return null;
            }

            return new SolarSystem(sresp.Name, sresp.Info);
           
        }

    }
}
