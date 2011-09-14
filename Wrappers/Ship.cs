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
        KeyBoard kb;
        Random ran = new Random();  

        /// <summary>
        /// Constant to signify warpspeed = really really fast
        /// </summary>
        public const int WARPSPEED = 300000000;
        
        public Ship(WindowHandler wh)
        {
            overviewhandler = wh.OVERVIEW;
            com = wh.COMMUNICATOR;
            menu = wh.MENU;
            pm = wh.PMOUSE;
            m = wh.MOUSE;
            kb = wh.KEYBOARD;
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
        /// Open the cargo
        /// </summary>
        /// <returns>Return true on success, false otherwise</returns>
        public Boolean openCargo()
        {

            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp != null)
            {
                return true;
            }
            
            kb.sendAltCharacter('c');
            return true;
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
        /// Target the entry with the given name
        /// </summary>
        /// <param name="entry">The overview name of the entry</param>
        /// <returns>True</returns>
        public Boolean target(string entry)
        {
            overviewhandler.openMenu(entry);
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

        public Boolean approach(string entry)
        {
            bool success = overviewhandler.openMenu(entry);
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
        /// Get the cycle duration for the given high slot
        /// </summary>
        /// <param name="i">The high slot number</param>
        /// <returns>The cycle duration in seconds, or -1 on failure</returns>
        public double getHighSlotCycleDuration(int i)
        {
            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETDURATION, "" + i, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
                return -1;

            return (double)sresp.Data / 1000;
        }

        /// <summary>
        /// Get the yield amount of the given strip miner
        /// </summary>
        /// <param name="i">The high slot number</param>
        /// <returns>The yield amount in m^3 or -1 on failure</returns>
        public double getMiningAmount(int i)
        {
            StringResponse sresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETMININGAMOUNT, ""+i, Response.RESPONSES.STRINGRESPONSE);
            if (sresp == null)
                return -1;

            

            return  Convert.ToDouble((string)sresp.Data);
        }

        /// <summary>
        /// Find out if the drones are fighting something
        /// </summary>
        /// <returns>1 if fighting, 0 if idle, -1 on failure</returns>
        public int areDronesEngaged()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.CHECKDRONESTATUS, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
                return -1;

            if (((string)iresp.Name).Contains("Fighting"))
                return 1;
            else
                return 0;
        }



        /// <summary>
        /// Stack all items in the cargo hold
        /// </summary>
        /// <returns>Returns true on success, false on failure</returns>
        public bool stackCargo()
        {

            List<Rectangle> recs = new List<Rectangle>();

            ItemResponse items = (ItemResponse)com.sendCall(FunctionCallFactory.CALLS.GETCARGOLIST, Response.RESPONSES.ITEMRESPONSE);
            if (items == null)
            {
                Console.WriteLine("cargolist is null");
                return false;
            }

            foreach (Item it in (List<Item>)items.Data)
            {
                recs.Add(new Rectangle(it.X, it.Y, it.Width, it.Height));
            }

            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            if (iresp == null)
            {
                Console.WriteLine("hangar is null");
                return false;
            }

            Point pt = new Point(ran.Next(iresp.X, iresp.X + iresp.Width), ran.Next(iresp.Y + 30, iresp.Y + iresp.Height));

            

            while (!menu.isEmpty(recs, pt))
                pt = new Point(ran.Next(iresp.X, iresp.X + iresp.Width), ran.Next(iresp.Y, iresp.Y + iresp.Height));

            menu.open(pt);
            Thread.Sleep(ran.Next(200, 300));
            menu.select(MenuHandler.MENUITEMS.STACKALL);
            Thread.Sleep(ran.Next(200, 300));
            menu.click(MenuHandler.MENUITEMS.STACKALL);

            return true;

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
        public int getStructurePercentage()
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
        /// Returns the percentage of capacitor the ship currently has
        /// </summary>
        /// <returns>Ship Capacitor percentage or -1 on failure</returns>
        public int getCapacitorPercentage()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITOR, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve capacitor percentage");
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
        public int getShieldPercentage()
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
        public int getArmorPercentage()
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
        public double getCargoFilled()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
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
        /// Launch drones
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        public bool launchDrones()
        {
            InterfaceResponse tresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETDRONESINBAYTAB, Response.RESPONSES.INTERFACERESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve drone tab");
                return false;
            }
            InterfaceEntry ientry = new InterfaceEntry(tresp.X, tresp.Y, tresp.Width, tresp.Height);
            menu.open(ientry);
            Thread.Sleep(400);
            return menu.click(MenuHandler.MENUITEMS.LAUNCHDRONES);
        }

        /// <summary>
        /// Return drones to drone bay 
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool retrieveDrones()
        {
            InterfaceResponse tresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETDRONESINSPACETAB, Response.RESPONSES.INTERFACERESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve drone tab");
                return false;
            }
            InterfaceEntry ientry = new InterfaceEntry(tresp.X, tresp.Y, tresp.Width, tresp.Height);
            menu.open(ientry);
            Thread.Sleep(400);
            return menu.click(MenuHandler.MENUITEMS.RETURNTODRONEBAY);
        }

        /// <summary>
        /// Check if we have drones in space
        /// </summary>
        /// <returns>True if we do, false otherwise</returns>
        public bool hasDronesInSpace()
        {
            InterfaceResponse tresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETDRONESINSPACETAB, Response.RESPONSES.INTERFACERESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve drone tab");
                return false;
            }

            Regex reg = new Regex("[0-9]");
            string nums = reg.Match(tresp.Name).Value;
            int digit = 0;
            if (nums != "")
            {
                digit = Convert.ToInt32(nums);
                if (digit == 0)
                    return false;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if we have drones available to launch
        /// </summary>
        /// <returns>True if we do, false otherwise</returns>
        public bool hasAvailableDrones()
        {
            InterfaceResponse tresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETDRONESINSPACETAB, Response.RESPONSES.INTERFACERESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve drone tab");
                return false;
            }

            Regex reg = new Regex("[0-9]");
            string nums = reg.Match(tresp.Name).Value;
            int digit = 0;
            if(nums != "")
            {
                digit = Convert.ToInt32(nums);
                if (digit == 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Set active target on the current target
        /// </summary>
        /// <param name="target">The target entry to set active</param>
        /// <returns>True on success, false otherwise</returns>
        public bool setActiveTarget(TargetEntry target)
        {
            m.move(new Point(ran.Next(target.X + 10, target.X + target.Width - 10), ran.Next(target.Y + 10, target.Y + target.Height - 10)));
            Thread.Sleep(400);
            m.click(true);
            pm.synchronize(m);
            return true;
        }
        /// <summary>
        /// Engage active target with drones in space
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public bool engageDrones()
        {
            InterfaceResponse tresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETDRONESINSPACETAB, Response.RESPONSES.INTERFACERESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve drone tab");
                return false;
            }
            InterfaceEntry ientry = new InterfaceEntry(tresp.X, tresp.Y, tresp.Width, tresp.Height);
            menu.open(ientry);
            Thread.Sleep(400);
            return menu.click(MenuHandler.MENUITEMS.ENGAGETARGET);
        }

        /// <summary>
        /// Returns the ship's current available cargo volume
        /// </summary>
        /// <returns>The ammount of volume available or -1 on error</returns>
        public double getCargoSpaceRemaining()
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve capacity");
                return -1;
            }

            double capacity = 0, used = 0;

            Regex reg = new Regex(@"/" + "[0-9]*,*[0-9]+" + @"." + "[0-9]+");
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

            reg = new Regex("[0-9]*,*[0-9]+" + @"." + "[0-9]+" + @"/");
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
        public int getShipCapacity()
        {
                  
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPCAPACITY, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve selected item");
                return -1;
            }

              
            Regex reg = new Regex(@"/" + "[0-9]*,*[0-9]+" + @"." + "[0-9]+");
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
        /// Stop the ship
        /// </summary>
        /// <returns></returns>
        public bool stop()
        {
            menu.KEYBOARD.sendCtrlCharacter((char)0x20);
            return true;
        }


        /// <summary>
        /// Load the ammo with the given name in the given highslot
        /// </summary>
        /// <param name="i">The number of the highslot to load the ammo into</param>
        /// <param name="ammoName">The name of the ammo to load</param>
        /// <returns></returns>
        public Boolean loadAmmo(int i, string ammoName)
        {
            if (!hasHighSlot(i))
                return false;

            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETHIGHSLOT, i + "", Response.RESPONSES.INTERFACERESPONSE);
           
            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X + 10, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y + 20, activateResp.Y + activateResp.Height - 10)));
            Thread.Sleep(200);
            m.click(false);
            Thread.Sleep(ran.Next(500, 700));
            pm.synchronize(m);
            if (!menu.click(ammoName))
                return menu.click(MenuHandler.MENUITEMS.Reload + " (" + ammoName);
            
            return true;
        }

        /// <summary>
        /// Unload the ammo from the given high slot
        /// </summary>
        /// <param name="i">The high slot to unload</param>
        /// <returns>True on success, false otherwise</returns>
        public Boolean unloadAmmo(int i)
        {
            if (!hasHighSlot(i))
                return false;

            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETHIGHSLOT, i + "", Response.RESPONSES.INTERFACERESPONSE);

            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X + 10, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y + 20, activateResp.Y + activateResp.Height - 10)));
            Thread.Sleep(200);
            m.click(false);
            Thread.Sleep(ran.Next(500, 700));
            pm.synchronize(m);
            return menu.click(MenuHandler.MENUITEMS.UNLOADTO);

        }


        /// <summary>
        /// Activate the high slot located at position num
        /// </summary>
        /// <param name="num">The position of the high slot to activate</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean activateHighPowerSlot(int num)
        {
            if (!hasHighSlot(num))
                return false;

            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETHIGHSLOT, num + "", Response.RESPONSES.INTERFACERESPONSE);

            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X+10, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y+20, activateResp.Y+activateResp.Height-10)));
            Thread.Sleep(200);
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Activate the med slot located at position num
        /// </summary>
        /// <param name="num">The position of the med slot to activate</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean activateMedPowerSlot(int num)
        {
            if (!hasMedSlot(num))
                return false;

            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETMEDSLOT, num + "", Response.RESPONSES.INTERFACERESPONSE);
    
            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X + 20, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y + 20, activateResp.Y + activateResp.Height - 10)));
            Thread.Sleep(200);
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Activate the low slot located at position num
        /// </summary>
        /// <param name="num">The position of the low slot to activate</param>
        /// <returns>True on sucess, false otherwise</returns>
        public Boolean activateLowPowerSlot(int num)
        {
            if (!hasLowSlot(num))
                return false;

            InterfaceResponse activateResp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETLOWSLOT, num + "", Response.RESPONSES.INTERFACERESPONSE);

            if (activateResp == null)
            {
                Console.WriteLine("Can't find module item");
                return false;
            }
            m.move(new Point(ran.Next(activateResp.X + 10, activateResp.X + activateResp.Width - 10), ran.Next(activateResp.Y + 20, activateResp.Y + activateResp.Height - 10)));
            Thread.Sleep(200);
            m.click(true);
            pm.synchronize(m);
            return true;
        }

        /// <summary>
        /// Returns the max targeting range of the given high slot
        /// </summary>
        /// <param name="num">The high slot number</param>
        /// <returns>The max targeting range l;in meters</returns>
        public double getHighSlotTargetingRange(int num)
        {
            StringResponse tresp = (StringResponse)com.sendCall(FunctionCallFactory.CALLS.GETTARGETINGRANGE, "" + num, Response.RESPONSES.STRINGRESPONSE);
            if (tresp == null)
            {
                Console.WriteLine("Couldn't retrieve highslot range");
                return -1.0;
            }

            return Convert.ToDouble((string)tresp.Data);

        }

        /// <summary>
        /// Check if the ship has the given highslot
        /// </summary>
        /// <param name="num">highslot position</param>
        /// <returns>True if it does, false otherwise</returns>
        public Boolean hasHighSlot(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISHIGHSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
            return true;
        }



        /// <summary>
        /// Check if the ship has the given med slot
        /// </summary>
        /// <param name="num">The slot number from 1-9</param>
        /// <returns>True if it does, false otherwise</returns>
        public Boolean hasMedSlot(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISMEDSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the ship has the given low slot
        /// </summary>
        /// <param name="num">The slot number from 1-9</param>
        /// <returns>True if it does, false otherwise</returns>
        public Boolean hasLowSlot(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISLOWSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
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
        /// Check if the given med slot is active
        /// </summary>
        /// <param name="num">The slot number from 1-9</param>
        /// <returns>True if active, false otherwise</returns>
        public Boolean isMedSlotActive(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISMEDSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
            if (activeResp == null)
            {
                Console.WriteLine("Can't get activity status");
                return false;
            }
            return (Boolean)activeResp.Data;
        }

        /// <summary>
        /// Check if the given low slot is active
        /// </summary>
        /// <param name="num">The slot number from 1-9</param>
        /// <returns>True if active, false otherwise</returns>
        public Boolean isLowSlotActive(int num)
        {
            BooleanResponse activeResp = (BooleanResponse)com.sendCall(FunctionCallFactory.CALLS.ISLOWSLOTACTIVE, "" + num, Response.RESPONSES.BOOLEANRESPONSE);
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

        /// <summary>
        /// Check if the cargo window is open
        /// </summary>
        /// <returns>True if its open, false otherwise</returns>
        public bool isCargoOpen()
        {
            InterfaceResponse iresp = (InterfaceResponse)com.sendCall(FunctionCallFactory.CALLS.GETSHIPHANGAR, Response.RESPONSES.INTERFACERESPONSE);
            return iresp != null;
            
        }
    }
}
