using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;

using Eryan.UI;
using Eryan.Wrappers;
using Eryan.Responses;
using Eryan.InputHandler;
using Eryan.IPC;
using Eryan.Factories;
using Eryan.Input;


namespace Eryan.Wrappers
{
    /// <summary>
    /// Player's ship handler
    /// </summary>
    public class Ship
    {
        OverviewHandler overviewhandler;
        Communicator com;
        MenuHandler menu;
        PreciseMouse pm;
        Mouse m;
        Random ran = new Random();  

        /// <summary>
        /// Constant to signify warpspeed = really really fast
        /// </summary>
        public const int WARPSPEED = 300000000;
        

        public Ship(MenuHandler mh, OverviewHandler oh, Communicator com, PreciseMouse pm, Mouse m)
        {
            overviewhandler = oh;
            this.com=com;
            menu = mh;
            this.pm = pm;
            this.m = m;
        }

        /// <summary>
        /// Returns the overview entries
        /// </summary>
        /// <returns>List of overview entries</returns>
        public List<OverViewEntry> getOverView()
        {
            if (overviewhandler.readOverView())
            {
                return overviewhandler.Items;
            }

            return null;

        }

        /// <summary>
        /// Warps to zero on the given asteroid belt
        /// </summary>
        /// <param name="beltname">The name of the belt</param>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean warpToZeroAsteroidBelt(string beltname)
        {
            bool success = menu.select(MenuHandler.MENUITEMS.ASTEROIDBELTS);
            if (!success)
                return false;
            success = menu.select(beltname);
            if (!success)
                return false;
            return warpToZero();
        }

        /// <summary>
        /// Target the given overview entry
        /// </summary>
        /// <param name="entry">The overview entry to target</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean target(OverViewEntry entry)
        {
            menu.open(entry);
            menu.click(MenuHandler.MENUITEMS.LOCKTARGET);
            return true;
        }

        /// <summary>
        /// Approach the overview item
        /// </summary>
        /// <param name="entry">Name of the item as it appears on the overview</param>
        /// <returns>true on sucess, false otherwise</returns>
        public Boolean approach(OverViewEntry entry)
        {
            bool success = menu.open(entry);
            menu.click(MenuHandler.MENUITEMS.APPROACH);
            return success;
        }


        /// <summary>
        /// Get the list of our ship's currently active targets
        /// </summary>
        /// <returns>The list of the currently targeted ships</returns>
        public List<TargetEntry> getTargetList()
        {
            TargetListResponse tresp = (TargetListResponse)com.sendCall(FunctionCallFactory.CALLS.GETTARGETLIST, Response.RESPONSES.TARGETRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve targetlist");
                return new List<TargetEntry>();
            }
            return (List<TargetEntry>)tresp.Data;
        }

        /// <summary>
        /// Return the selected item
        /// </summary>
        /// <returns>Selected item</returns>
        public SelectedItem getSelectedItem()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSELECTEDITEM, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve selected item");
                return null;
            }

            Regex reg = new Regex("<br>Distance: ");
            string[] result = reg.Split((string)tresp.Data);

            if(result.Count() > 1)
            {
                reg = new Regex("[0-9]+");
                String nums;
                int distance = 0;
                if ((nums = reg.Match(result[1]).Value) != "")
                {
                    reg = new Regex("km");
                    if (reg.Match(result[1]).Value != "")
                    {
                        distance = Convert.ToInt32(nums) * 1000;
                        Console.WriteLine("Distance is " + distance);
                    }
                    else
                    {
                        distance = Convert.ToInt32(nums);
                        Console.WriteLine("Distance is " + distance);
                    }
                }
                
                return new SelectedItem(result[0], distance);
            }

            return null;
            
        }

        /// <summary>
        /// Returns the percentage of structure the ship currently has
        /// </summary>
        /// <returns>Ship Structure percentage or -1 on failure</returns>
        public int GetStructurePercentage()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPSTRUCTURE, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve armor percentage");
                return -1;
            }

            Regex reg = new Regex("[0-9]+%");
            string result = reg.Match((string)tresp.Data).Value;
            result = result.Substring(0, result.Length - 1);
            return Convert.ToInt32(result);
        }


        /// <summary>
        /// Returns the percentage of shield the ship currently has
        /// </summary>
        /// <returns>Ship Shiled percentage or -1 on failure</returns>
        public int GetShiledPercentage()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPSHIELD, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve armor percentage");
                return -1;
            }

            Regex reg = new Regex("[0-9]+%");
            string result = reg.Match((string)tresp.Data).Value;
            result = result.Substring(0, result.Length - 1);
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Returns the percentage of armor the ship currently has
        /// </summary>
        /// <returns>Ship Armor percentage or -1 on failure</returns>
        public int GetArmorPercentage()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPARMOR, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve armor percentage");
                return -1;
            }

            Regex reg = new Regex("[0-9]+%");
            string result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
                return Convert.ToInt32(result);
            }
            else
                return -1;
        }

        /// <summary>
        /// Returns the amount of cargo space that has been used
        /// </summary>
        /// <returns>Volume used or -1 on error</returns>
        public double GetCargoFilled()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve selected item");
                return -1;
            }


            Regex reg = new Regex("[0-9]+" + @"." + "[0-9]+" + @"/");
            string result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                try
                {
                    result = result.Substring(0, result.Length - 1);
                    return Convert.ToDouble(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }
            return -1;
        }


        /// <summary>
        /// Returns the ship's current available cargo volume
        /// </summary>
        /// <returns>The ammount of volume available or -1 on error</returns>
        public double GetCargoSpaceRemaining()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve capacity");
                return -1;
            }

            double capacity = 0, used = 0;

            Regex reg = new Regex(@"/" + "[0-9]+" + @"." + "[0-9]+");
            string result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                result = result.Substring(1, result.Length - 1);
                try
                {
                    capacity = Convert.ToDouble(result);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }

            reg = new Regex("[0-9]+" + @"." + "[0-9]+" + @"/");
            result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
                try
                {
                    used = Convert.ToDouble(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }
            if(capacity != 0)
                return capacity - used;
            
            return -1;
        }



        /// <summary>
        /// Returns the cargo capacity of your ship
        /// </summary>
        /// <returns>Capacity or -1 on error</returns>
        public int GetShipCapacity()
        {
                  
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve selected item");
                return -1;
            }

              
            Regex reg = new Regex(@"/" + "[0-9]+" + @"." + "[0-9]+");
            string result = reg.Match((string)tresp.Data).Value;
            if (result.Length > 0)
            {
                try
                {
                    result = result.Substring(1, result.Length - 1);
                    return Convert.ToInt32(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }
            }
            return -1;
        }


        /// <summary>
        /// Activate the high slot located at position num
        /// </summary>
        /// <param name="num">The position of the high slot to activate</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean activateHighPowerSlot(int num)
        {
            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETHIGHSLOT, num + "", Response.RESPONSES.INTERFACERESPONSE);
            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X+5, activateResp.X + activateResp.Width - 5), ran.Next(activateResp.Y+5, activateResp.Y+activateResp.Height-5)));
            Thread.Sleep(200);
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Check if the highslot at position num is active
        /// </summary>
        /// <param name="num">The position of the highslot to check</param>
        /// <returns>True if its active, false otherwise</returns>
        public Boolean isHighSlotActive(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISHIGHSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
            return (Boolean)activeResp.Data;
        }

        /// <summary>
        /// Get our ship's current speed
        /// </summary>
        /// <returns>The speed of the ship in m/s</returns>
        public int getSpeed()
        {
            int speed = -1;
            StringResponse iresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPSPEED, Response.RESPONSES.STRINGRESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("Response is null");
                return speed;
            }

            Regex regex = new Regex("Warping");
            if (regex.Match((String)iresp.Data).Value != "")
            {
                Console.WriteLine("We're warping");
                speed = WARPSPEED;
            }
            else
            {
                regex = new Regex("[0-9]+" + @"\." + "*[0-9]*");
                String match = regex.Match((String)iresp.Data).Value;
                try
                {
                    speed = (int)Convert.ToDouble(match);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return speed;
        }

        /// <summary>
        /// Warps to zero
        /// </summary>
        /// <returns>True if sucess, false otherwise</returns>
        public Boolean warpToZero()
        {
            return menu.click(MenuHandler.MENUITEMS.WARPTOZERO);
        }

        /// <summary>
        /// Check if we are docked
        /// </summary>
        /// <returns>True if docked, false otherwise</returns>
        public Boolean isDocked()
        {
            return getSpeed() == -1;
        }

        /// <summary>
        /// Undock if docked
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        public Boolean unDock()
        {

            InterfaceResponse dockbutton = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETUNDOCKBUTTON, Response.RESPONSES.INTERFACERESPONSE);
            if (dockbutton == null)
            {
                Console.WriteLine("dockbutton is null");
                return false;
            }

            m.move(new Point(ran.Next(dockbutton.X, dockbutton.X + dockbutton.Width), ran.Next(dockbutton.Y, dockbutton.Y + dockbutton.Height)));
            pm.synchronize(m);
            m.click(true);
            return true;
        }

        /// <summary>
        /// Docks ship in the given station name
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public Boolean dock(String stationName)
        {
            bool success = menu.select(MenuHandler.MENUITEMS.STATIONS);
            if (!success)
                return false;
            success = menu.select(stationName);
            if (!success)
                return false;
            success = menu.click(MenuHandler.MENUITEMS.DOCK);
            return success;
        }

        /// <summary>
        /// Return the list of items in the cargo
        /// </summary>
        /// <returns>Item entries representing cargo items</returns>
        public List<Item> getCargo()
        {
            ItemResponse resp = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETCARGOLIST, Response.RESPONSES.ITEMRESPONSE);
            if (resp == null)
            {
                Console.WriteLine("Response is null");
                return null;
            }
            return (List<Item>)resp.Data;
        }
    }
}
