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
using Eryan.UI;

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

        public OverviewHandler(WindowHandler wh)
        {
            this.m = wh.MOUSE;
            this.pm = wh.PMOUSE;
            this.comm = wh.COMMUNICATOR;
            this.mh = wh.MENU;
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

        private bool openOverviewSelect()
        {
            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSELECTION, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return false;
            }

            m.move(new Point(ran.Next(iresp.X + 2, iresp.X + iresp.Width - 2), ran.Next(iresp.Y + 2, iresp.Y + iresp.Height - 2)));
            pm.synchronize(m);
            pm.move(new Point(ran.Next(iresp.X + 2, iresp.X + iresp.Width - 2), ran.Next(iresp.Y + 2, iresp.Y + iresp.Height - 2)));
            m.synchronize(pm);
            Thread.Sleep(ran.Next(200, 300));
            pm.click(true);
            return true;
        }

        /// <summary>
        /// Check if the overview is sorted by distance in ascending order
        /// </summary>
        /// <returns>True if it is, false otherwise</returns>
        public bool isSortedByDistanceAsc()
        {
 
            readOverView();

            foreach (OverViewEntry tmp in Items)
            {
                foreach (OverViewEntry entry in Items)
                {
                    if (tmp.Y == 0 || entry.Y == 0)
                    {
                        continue;
                    }

                    if (tmp.Distance > entry.Distance)
                        if (tmp.Y < entry.Y)
                            return false;
                }
            }

            return true;
        }

        public bool toggleDistanceSort()
        {
            InterfaceResponse activateResp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWDISTANCEHEADER, Response.RESPONSES.INTERFACERESPONSE);
            if (activateResp == null)
                return false;

            m.move(new Point(ran.Next(activateResp.X + 10, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y + 3, activateResp.Y + activateResp.Height - 3)));
            Thread.Sleep(ran.Next(300, 400));
            m.click(true);
            pm.synchronize(m);
            return true;
        }



        /// <summary>
        /// Select the given overview profile
        /// </summary>
        /// <param name="profile">The name of the overview profile to select</param>
        /// <returns>True on success, false otherwise</returns>
        public bool selectProfile(string profile)
        {
            openOverviewSelect();
            Thread.Sleep(ran.Next(200, 400));

            if (!mh.isMenuOpen())
                return false;

            mh.select(MenuHandler.MENUITEMS.LOADDEFAULT);
            Thread.Sleep(ran.Next(200, 400));
            mh.click(profile);
            return true;
        }

        /// <summary>
        /// Select the given overview profile
        /// </summary>
        /// <param name="profile">The name of the overview profile to select</param>
        /// <param name="defaultProfile">True if the profile is a default one, false if its custom</param>
        /// <returns>true on success, fale otherwise</returns>
        public bool selectProfile(string profile, bool defaultProfile)
        {
            if (defaultProfile)
                return selectProfile(profile);
            openOverviewSelect();
            Thread.Sleep(ran.Next(200, 400));

            if (!mh.isMenuOpen())
                return false;

            mh.click("Load " + profile);
            return true;
        }

        /// <summary>
        /// Get the currently selected overview profile
        /// </summary>
        /// <returns>The name of the profile or null on failure</returns>
        public string getSelectedProfile()
        {
            StringResponse sresp = (StringResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSELECTTEXT, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
                return null;
            int posStart = -1, posEnd = -1;
            posStart = ((String)sresp.Data).IndexOf('(');
            posEnd = ((String)sresp.Data).IndexOf(')');

            if (posStart == -1 || posEnd == -1)
                return null;

            return ((String)sresp.Data).Substring(posStart + 1, posEnd - posStart - 1);
        }

        /// <summary>
        /// Check if the given profile is currently selected
        /// </summary>
        /// <param name="profile">The name of the profile to check for</param>
        /// <returns>True if selected, false otherwise</returns>
        public bool isProfileSelected(string profile)
        {
            string selected = getSelectedProfile();
            if (selected == null)
                return false;

            if (selected.ToLower().Equals(profile.ToLower()))
                return true;

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

        /// <summary>
        /// Get the coordinates of the overview scrollbar
        /// </summary>
        /// <returns>An interface response object with the scrollbar information or null on failure</returns>
        public InterfaceResponse overviewScrollbar()
        {
            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWSCROLL, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                return null;
            }

            return iresp;

        }

        /// <summary>
        /// Get the y coordinate of the overview scroll area bottom
        /// </summary>
        /// <returns>A coordinate between 0 and size on success, -1 on failure</returns>
        public int overviewBottom()
        {

            StringResponse sresp = (StringResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWBOTTOM, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return -1;
            }

            return Convert.ToInt32((string)sresp.Data);
        }

        /// <summary>
        /// Get the y coordinate of the top of the overview scrolling area
        /// </summary>
        /// <returns>A coordinate between 0 and size on success, -1 on failure</returns>
        public int overviewTop()
        {
            StringResponse sresp = (StringResponse)comm.sendCall(FunctionCallFactory.CALLS.GETOVERVIEWTOP, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
            {
                return -1;
            }

            return Convert.ToInt32((string)sresp.Data);
        }


        /// <summary>
        /// Bookmark the given overview entry by name
        /// </summary>
        /// <param name="entryName">The name of the entry to bookmark</param>
        /// <returns>True on success, false otherwise</returns>
        public string bookmark(string entryName)
        {
            InterfaceResponse iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETMODALSUBMITBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp != null)
                return null;

            openMenu(entryName);
            Thread.Sleep(400);

            mh.click(MenuHandler.MENUITEMS.BOOKMARKLOCATION);
            Thread.Sleep(ran.Next(1000, 1500));

            
            iresp = (InterfaceResponse)comm.sendCall(FunctionCallFactory.CALLS.GETMODALSUBMITBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return null;
            

            StringResponse bmname = (StringResponse)comm.sendCall(FunctionCallFactory.CALLS.GETBOOKMARKFIELDNAME, Response.RESPONSES.STRINGRESPONSE);
            if (bmname == null)
                return null;
            

            string name = (string)bmname.Data;
            

            m.move(new Point(ran.Next(iresp.X + 5, iresp.X + iresp.Width - 5), ran.Next(iresp.Y + 5, iresp.Y + iresp.Height - 5)));
            Thread.Sleep(300);
            m.click(true);
            pm.synchronize(m);
            Thread.Sleep(300);
            
            return name;
        }

        /// <summary>
        /// Open a menu on the overview item with the given name
        /// </summary>
        /// <param name="entryName">The name of the overview item to open a menu on</param>
        /// <returns>True on success, false otherwise</returns>
        public bool openMenu(string entryName)
        {
            int passes = 0;
            bool direction = false;
            bool found = false;
            OverViewEntry entry = getEntry(entryName);

            while (passes < 2 && entry != null)
            {

                InterfaceResponse overv = overviewScrollbar();
                if (overv != null)
                {

                    if (entry.X != 0)
                    {
                        found = true;
                        break;
                    }

                    //Console.WriteLine("Overview Bottom: " + overviewBottom());
                    //Console.WriteLine("O.Y+O.Height+50: " + (overv.Y + overv.Height + 50));
                    if (overv.Y + overv.Height + 30 >= overviewBottom())
                    {
                      //  Console.WriteLine("GOING UP!");
                      //  Console.WriteLine("PASSES INCREMENTED");
                        passes++;
                        direction = false;
                    }

                    //Console.WriteLine("Overview Top: " + EOverViewHandler.overviewTop());
                    //Console.WriteLine("Overview.Y-3: " + (overv.Y - 30));
                    if (overv.Y - 30 <= overviewTop())
                    {
                       // Console.WriteLine("GOING DOWN!!!!");
                       // Console.WriteLine("PASSES INCREMENTED");
                        passes++;
                        direction = true;
                    }

                    if (!direction)
                        scrollUp();

                    else
                        scrollDown();

                    entry = getEntry(entryName);
                    Thread.Sleep(200);
                }


            }
            if (found)
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
