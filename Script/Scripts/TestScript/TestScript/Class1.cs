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
        double maxRange = -1.0;
        int dist = 0;

        private bool approaching = false;

        public enum State { MINING, TOMINING, TOSTATION, DEPOSITING, LOADING, INCURSION, ERROR, DEFENDING };

        public override Boolean onStart()
        {
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        public State getState()
        {

            if (ESession.isLoading())
                return State.LOADING;

            if (ESession.isIncursionOngoing())
                return State.INCURSION;

            //If we're fucked up dock up
            if (!MyShip.isDocked())
            {
                if (MyShip.getShiledPercentage() < 50)
                    return State.TOSTATION;
            }

            OverViewEntry gurista = EOverViewHandler.getEntry("Gurista");
            if (gurista != null)
            {
                if (gurista.Distance < 3000)
                    return State.DEFENDING;
            }

            if (MyShip.getCargoFilled() > 0 && MyShip.isDocked())
                return State.DEPOSITING;

            //If we got space in the cargo and we're not docked we must mine or go to mine
            if (MyShip.getCargoSpaceRemaining() > 10)
            {
                if (!MyShip.isDocked())
                {
                    //if no asteroids go mine, else we're mining
                    if (!EOverViewHandler.isInOverView("Asteroid"))
                    {
                        Console.WriteLine("TOMINING");
                        return State.TOMINING;
                    }
                    else
                    {
                        Console.WriteLine("MINING");
                        return State.MINING;
                    }
                }
                if (MyShip.isDocked())
                {
                    return State.TOMINING;
                }
            }

            //If we're out of space if we're at station we deposit, otherwise we go to station
            if (MyShip.getCargoSpaceRemaining() < 10)
            {
                if (MyShip.isDocked())
                {
                    Console.WriteLine("DEPOSITING");
                    return State.DEPOSITING;
                }
                else
                {
                    Console.WriteLine("TOSTATION");
                    return State.TOSTATION;
                }
            }

            //For undefined behavior
            return State.ERROR;
        }


        public override int run()
        {
            ECommunicator.connect();
            EMouse.Speed = 30;
            EPreciseMouse.Speed = 20;

            if (maxRange < 0)
                maxRange = MyShip.getHighSlotTargetingRange(1);

            switch (getState())
            {
                
                case State.LOADING:
                    return 1000;

                case State.INCURSION:
                    if (!MyShip.isDocked())
                    {
                        if (MyShip.getSpeed() != Ship.WARPSPEED)
                        {
                            if (MyShip.hasDronesInSpace())
                            {
                                MyShip.retrieveDrones();
                                return 600;
                            }
                            EMenuHandler.select(MenuHandler.MENUITEMS.STATIONS);
                            Thread.Sleep(600);
                            EMenuHandler.select("VII");
                            Thread.Sleep(600);
                            EMenuHandler.click(MenuHandler.MENUITEMS.DOCK);
                            return 3000;
                        }
                        return 1000;
                    }
                    else
                    {
                        Console.WriteLine("INCURSION");
                        return 2000;
                    }
                case State.TOSTATION:
                    if (MyShip.getSpeed() != Ship.WARPSPEED)
                    {
                        if (MyShip.hasDronesInSpace())
                        {
                            MyShip.retrieveDrones();
                            return 600;
                        }

                        EMenuHandler.select(MenuHandler.MENUITEMS.STATIONS);
                        Thread.Sleep(600);
                        EMenuHandler.select("VII");
                        Thread.Sleep(600);
                        EMenuHandler.click(MenuHandler.MENUITEMS.DOCK);
                        return 3000;
                    }
                    return 300;
                    
                case State.TOMINING:
                    if (MyShip.isDocked())
                    {
                        MyShip.unDock();
                        return 30000;
                    }
                    if (MyShip.getSpeed() != Ship.WARPSPEED)
                    {
                        if (MyShip.hasDronesInSpace())
                        {
                            MyShip.retrieveDrones();
                            return 600;
                        }
                        EMenuHandler.select(MenuHandler.MENUITEMS.ASTEROIDBELTS);
                        Thread.Sleep(600);
                        //EMenuHandler.select(MenuHandler.MENUITEMS.BELT1);
                        Random rand = new Random();
                        List<MenuEntry> belts = EMenuHandler.getBelts();
                        Console.WriteLine(belts.Count);
                        MenuEntry belt = belts[rand.Next(0, belts.Count - 1)];
                        EMenuHandler.select(belts[rand.Next(0, belts.Count - 1)]);
                        Thread.Sleep(600);
                        EMenuHandler.click(MenuHandler.MENUITEMS.WARPTOZERO);
                        return 1000;
                    }
                    return 300;    

                case State.DEFENDING:
                    OverViewEntry gurista = EOverViewHandler.getEntry("Gurista");
                    if (gurista != null)
                    {
                        List<TargetEntry> mytargets = MyShip.getTargetList();
                        TargetEntry guristaEntry = null;

                        foreach(TargetEntry entry in mytargets)
                        {
                            if(entry.Name.Contains("Gurista"))
                                guristaEntry = entry;
                        }

                        if (guristaEntry == null)
                        {
                            MyShip.target(gurista);
                            return 5000;
                        }
                        else
                        {
                            foreach (TargetEntry entry in mytargets)
                            {
                                if (entry.Name.Contains("Gurista"))
                                {
                                    if (entry.Distance < 30000)
                                    {
                                        SelectedItem sitem = MyShip.getSelectedItem();
                                        
                                        if (sitem != null)
                                        {
                                            if (!MyShip.getSelectedItem().Name.Contains("Gurista"))
                                            {
                                                MyShip.setActiveTarget(entry);
                                                Thread.Sleep(300);
                                            }
                                        }
                                        else
                                        {
                                            MyShip.setActiveTarget(entry);
                                            Thread.Sleep(300);
                                        }

                                        if (MyShip.hasDronesInSpace())
                                        {
                                            MyShip.engageDrones();
                                            return 2000;
                                        }
                                        else
                                        {
                                            MyShip.launchDrones();
                                            return 600;
                                        }
                                    }
                                }
                            }

                            return 600;                
                        }
                        
                    }
                    return 600;
                case State.MINING:
                    OverViewEntry asteroid;
                    if ( (asteroid = EOverViewHandler.getEntry("Asteroid")) != null)
                    {
                        asteroid = EOverViewHandler.getEntry("Asteroid");
                        if (asteroid.Distance >= maxRange)
                        {
                            Console.WriteLine("Distance greater than " + maxRange);
                            if (MyShip.getSpeed() < 10)
                            {
                                MyShip.approach(asteroid);
                                dist = asteroid.Distance;
                                EMouse.click(true);
                            }
                            
                            return 4000;
                        }
                        List<TargetEntry> mytargets = MyShip.getTargetList();
                        if (mytargets.Count < 1)
                        {
                            Console.WriteLine("Targeting");
                            MyShip.target(asteroid);
                            return 5000;
                        }
                        else
                        {

                            Console.WriteLine(mytargets[0].Distance);
                            if (mytargets[0].Distance >= maxRange)
                            {
                                if (MyShip.getSpeed() < 50 || dist > asteroid.Distance)
                                {
                                    EMenuHandler.open(mytargets[0]);
                                    Thread.Sleep(500);
                                    EMenuHandler.click(MenuHandler.MENUITEMS.APPROACH);
                                    dist = mytargets[0].Distance;
                                }
                                return 300;
                            }
                            else
                            {
                                if(MyShip.getSpeed() > 50)
                                    MyShip.stop();
                            }
                            if (!MyShip.isHighSlotActive(2))
                                MyShip.activateHighPowerSlot(2);
                            if (!MyShip.isHighSlotActive(1))
                                MyShip.activateHighPowerSlot(1);
                            return 600;
                        }
                    
                    }
                    return 300;
                case State.DEPOSITING:
                    EStationHandler.depositAll();
                    return 1000;
                case State.ERROR:
                    Console.WriteLine("ERROR state");
                    return 1000;
            }

            return 300;           
        }

    }
}
