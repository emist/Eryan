using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using Eryan.Wrappers;
using Eryan.Input;
using Eryan.IPC;
using Eryan.Responses;
using Eryan.Factories;

namespace Eryan.InputHandler
{
    /// <summary>
    /// Handles the interaction with the overview
    /// </summary>
    public class OverviewHandler
    {
        List<OverViewEntry> entries;
        Mouse m;
        PreciseMouse pm;
        Communicator com;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m">Reference to the bot's mouse</param>
        /// <param name="pm">Reference to the bot's Precise mouse</param>
        /// <param name="com">Reference to the bot's communicator</param>
        public OverviewHandler(Mouse m, PreciseMouse pm, Communicator com)
        {
            this.m = m;
            this.pm = pm;
            this.com = com;
        }

        /// <summary>
        /// Populate this overview wrapper with the client's current overview
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public bool readOverView()
        {
            OverViewResponse resp = (OverViewResponse)com.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWITEMS, Response.RESPONSES.OVERVIEWRESPONSE);
            if (resp == null)
            {
                return false;
            }
            resp.HandleResponse();

            entries = (List<OverViewEntry>)resp.Data;
            return true;
        }

        /// <summary>
        /// Open a menu on this row
        /// </summary>
        /// <param name="rowNum">The number of the overview entry</param>
        /// <returns>True if success, false otherwise</returns>
        public bool interactRow(int rowNum)
        {
            if (entries.Count > rowNum)
            {
                m.moveMouse(new Point(entries[rowNum].X, entries[rowNum].Y));
                Thread.Sleep(200);
                m.click(false);
                return true;
            }
            return false;
        }


    }
}
