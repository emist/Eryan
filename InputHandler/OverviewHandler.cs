using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Text.RegularExpressions;

using Eryan.Wrappers;
using Eryan.Input;
using Eryan.IPC;
using Eryan.Responses;
using Eryan.Factories;

namespace Eryan.InputHandler
{

    class SortIntDescending : IComparer<OverViewEntry>
    {
        int IComparer<OverViewEntry>.Compare(OverViewEntry b, OverViewEntry a) //implement Compare
        {
            if (a.Distance > b.Distance)
                return -1; //normally greater than = 1
            if (a.Distance < b.Distance)
                return 1; // normally smaller than = -1
            else
                return 0; // equal
        }
    }
 

    /// <summary>
    /// Handles the interaction with the overview
    /// </summary>
    public class OverviewHandler : InputHandler
    {
        List<OverViewEntry> entries;
        Random ran;
        MenuHandler mh;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m">Reference to the bot's mouse</param>
        /// <param name="pm">Reference to the bot's Precise mouse</param>
        /// <param name="com">Reference to the bot's communicator</param>
        public OverviewHandler(MenuHandler mh, Mouse m, PreciseMouse pm, Communicator com)
        {
            this.m = m;
            this.pm = pm;
            this.comm = com;
            this.mh = mh;
            entries = new List<OverViewEntry>();
            ran = new Random();
        }

        /// <summary>
        /// Populate this overview wrapper with the client's current overview
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public bool readOverView()
        {
            entries.Clear();
            OverViewResponse resp = (OverViewResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWITEMS, Response.RESPONSES.OVERVIEWRESPONSE);
            if (resp == null)
            {
                return false;
            }


            foreach (OverViewEntry entry in (List<OverViewEntry>)resp.Data)
            {
                
                entries.Add(entry);
            }

            entries.Sort(new SortIntDescending());
            return true;
        }

        /// <summary>
        /// Open a menu on this row
        /// </summary>
        /// <param name="rowNum">The number of the overview entry</param>
        /// <returns>True if success, false otherwise</returns>
        public bool interactRow(int rowNum)
        {
            readOverView();
            if (entries.Count > rowNum)
            {
                m.moveMouse(new Point(entries[rowNum].X, entries[rowNum].Y));
                Thread.Sleep(200);
                synchronizeMouse(m);
                m.click(false);
                Thread.Sleep(200);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Scroll the overview down
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool scrollDown()
        {
            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSCROLL, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X+3, iresp.X+iresp.Width-3), ran.Next(iresp.Y+3, iresp.Y+iresp.Height-3)));
            Thread.Sleep(300);
            m.drag(new Point(iresp.X, iresp.Y + iresp.Height));
            pm.synchronize(m);

            return true;
        }

        /// <summary>
        /// Scroll the overview up
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool scrollUp()
        {

            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSCROLL, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 3, iresp.X + iresp.Width - 3), ran.Next(iresp.Y + 3, iresp.Y + iresp.Height - 3)));
            Thread.Sleep(300);
            m.drag(new Point(iresp.X, iresp.Y - iresp.Height));
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Read an overview row
        /// </summary>
        /// <param name="rowNum">The row number in the overview</param>
        /// <returns>The contents of the overview row</returns>
        public OverViewEntry readRow(int rowNum)
        {
            readOverView();
            if (entries.Count > rowNum)
                return entries[rowNum];
            return null;
        }

        private InterfaceResponse overviewScrollbar()
        {
            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSCROLL, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return iresp;

        }

        private int overviewBottom()
        {

            StringResponse sresp = (StringResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWBOTTOM, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return -1;
            }

            return Convert.ToInt32((string)sresp.Data);
        }

        /// <summary>
        /// Open a menu on the overview item with the given name
        /// </summary>
        /// <param name="entryName">The name of the overview item to open a menu on</param>
        /// <returns>True on success, false otherwise</returns>
        public bool openMenu(string entryName)
        {
            InterfaceResponse overv = overviewScrollbar();
            int passes = 0;
            bool direction = false;
            bool found = false; 

            OverViewEntry entry = getEntry(entryName);


            while (passes < 2 && entry != null) 
            {

                if (overv != null)
                {
                    if (overv.Y + overv.Height + 20 >= overviewBottom())
                    {
                        passes++;
                        direction = false;
                    }

                    if (overv.Y - 30 <= overv.Y)
                    {
                        passes++;
                        direction = true;
                    }

                    if (!direction)
                    {
                        if (entry.X != 0)
                        {
                            found = true;
                            break;
                        }
                        scrollUp();
                    }
                    else
                    {
                        if (entry.X != 0)
                        {
                            found = true;
                            break;
                        }
                        scrollDown();
                    }

                    Thread.Sleep(500);
                }
                
                entry = getEntry(entryName);
            }
            if(found)
                mh.open(entry);
            return found;
        }

        /// <summary>
        /// Find if the given item is on the overview
        /// </summary>
        /// <param name="labelName">The name of the item to look for</param>
        /// <returns>True if found, false otherwise</returns>
        public bool isInOverView(string labelName)
        {
            readOverView();
            Regex reg = new Regex(labelName);
            foreach (OverViewEntry entry in entries)
            {
                foreach (String section in entry.Sections)
                {
                    if (reg.Match(section).Value != "" && entry.X != 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the overview entry containing the requested label
        /// </summary>
        /// <param name="labelName">Text in the overview row requested</param>
        /// <returns>The overview entry or null if not found</returns>
        public OverViewEntry getEntry(string labelName)
        {
            readOverView();
            Regex reg = new Regex(labelName);
            for (int i = 0; i < entries.Count(); i++)
            {
                foreach (String section in entries[i].Sections)
                {
                    if (reg.Match(section).Value != "")
                    {
                        Console.WriteLine(section);
                        Console.WriteLine(entries[i].Distance);
                        return entries[i];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Interact with the first overview row that contains "content"
        /// </summary>
        /// <param name="content">The string to look for in the overview</param>
        /// <returns>True if interacted, false otherwise</returns>
        public bool interactRow(string content)
        {
            readOverView();
            int i = 0;
            foreach (OverViewEntry entry in entries)
            {
                if (entry.ToString().Contains(content))
                {
                    interactRow(i);
                    return true;
                }
                i++;
            }
            return false;
        }

        /// <summary>
        /// Returns the overview items
        /// </summary>
        public List<OverViewEntry> Items
        {
            get
            {
                return entries;
            }
        }

    }
}
