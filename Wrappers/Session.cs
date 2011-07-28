using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        public bool IsLoading()
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
        public bool IsSystemMenuOpen()
        {
            BooleanResponse bresp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISSYSTEMMENUOPEN, Response.RESPONSES.BOOLEANRESPONSE);
            if (bresp == null)
                return false;
            return (Boolean)bresp.Data;
        }

        public Button GetOkButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALOKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("OK", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

        public Button GetCancelButton()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMODALCANCELBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return new Button("CANCEL", iresp.X, iresp.Y, iresp.Height, iresp.Width);
        }

    }
}
