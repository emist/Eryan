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
using Eryan.Input;
using jabber;
using jabber.client;

//Artic monkeys-Dancing shoes

namespace Script
{
    public class Script : Scriptable
    {
        Random ran = new Random();
        Queue<string> commandQ = new Queue<string>();
        jabber.client.JabberClient jc = new jabber.client.JabberClient();
        const string TARGET = "USERNAME@jabber.org";
        const bool VERBOSE = true;

        static ManualResetEvent done = new ManualResetEvent(false);

        public override Boolean onStart()
        {
            Name = "UnitTest";
            ESession.disableAutoLogin();

            try
            {

                JID j = new JID("eryanbot@jabber.org");
                jc.User = j.User;
                jc.Server = j.Server;
                jc.Password = "hellohello";
                jc.AutoPresence = true;
                jc.AutoRoster = false;
                jc.AutoReconnect = -1;

                jc.OnError += new bedrock.ExceptionHandler(j_OnError);
                jc.OnMessage += new MessageHandler(jc_OnMessage);
                bedrock.net.AsyncSocket.UntrustedRootOK = true;

                jc.Connect();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR");
            }
            return true;
        }

        public override Boolean onFinish()
        {
            return true;
        }

        static void j_OnError(object sender, Exception ex)
        {
            // There was an error!
            Console.WriteLine("Error: " + ex.ToString());

            // Shut down.
            done.Set();
        }


        void jc_OnMessage(object sender, jabber.protocol.client.Message msg)
        {
            Console.WriteLine(msg.Body);

            commandQ.Enqueue(msg.Body);

            /*
            if (msg.Body.Equals("LOL"))
                MyShip.unDock();

            if (msg.Body.Equals("WARP"))
            {
                commandQ.Add("WARP");
            }
            if (msg.Body.Equals("TARGET"))
            {
                commandQ.Add("TARGET");
            }
            */


        }

        public override int run()
        {
            ECommunicator.connect();
            EMouse.Speed = 30;
            EPreciseMouse.Speed = 30;
            ESession.disableAutoLogin();
            

            if (commandQ.Count > 0)
            {

                string command = commandQ.Dequeue();

                if (command.Equals(""))
                    return 500;

                Console.WriteLine(command);

                if(command.Equals("getOverView"))
                {
                    MyShip.getOverView();
                    Console.WriteLine("overview");
                }

                if (command.Equals("warpToZeroAsteroidBelt"))
                {
                    MyShip.warpToZeroAsteroidBelt("I");
                }

                if (command.Equals("openCargo"))
                {
                    MyShip.openCargo();
                }

                if (command.Equals("probeScan"))
                {
                    MyShip.probeScan();
                }

                if (command.Equals("recoverProbe"))
                {
                    MyShip.recoverProbe();
                }

                if (command.Equals("openScanner"))
                {
                    MyShip.openScanner();
                }

                if (command.Equals("target"))
                {
                    MyShip.target("Asteroid");
                }

                if (command.Equals("approach"))
                {
                    MyShip.approach("Asteroid");
                }

                if (command.Equals("getProbeEntry"))
                {
                    MyShip.getProbeEntry("heaven");
                }

                if (command.Equals("getProbeDistance"))
                {
                    MyShip.setProbeDistance(1, 0.25);
                }

                if (command.Equals("getProbeResult"))
                {
                    MyShip.getProbeResult("");
                }

                if (command.Equals("warpToZeroProbeResult"))
                {
                    MyShip.warpToZeroProbeResult("");
                }

                if (command.Equals("getTargetList"))
                {
                    MyShip.getTargetList();
                }

                if (command.Equals("getHighSlotCycleDuration"))
                {
                    MyShip.getHighSlotCycleDuration(1);
                }

                if (command.Equals("getMiningAmount"))
                {
                    MyShip.getMiningAmount(1);
                }

                if (command.Equals("getInjuredDrone"))
                {
                    MyShip.getInjuredDrone();
                }

                if (command.Equals("areDronesEngaged"))
                {
                    MyShip.areDronesEngaged();
                }

                if (command.Equals("stackCargo"))
                {
                    MyShip.stackCargo();
                }

                if (command.Equals("getSelectedItem"))
                {
                    MyShip.getSelectedItem();
                }

                if (command.Equals("getStructurePercentage"))
                {
                    MyShip.getStructurePercentage();
                }

                if (command.Equals("getCapacitorPercentage"))
                {
                    MyShip.getCapacitorPercentage();
                }

                if (command.Equals("getArmorPercentage"))
                {
                    MyShip.getArmorPercentage();
                }

                if (command.Equals("getCargoFilled"))
                {
                    MyShip.getCargoFilled();
                }

                if (command.Equals("launchDrones"))
                {
                    MyShip.launchDrones();
                }

                if (command.Equals("retrieveDrones"))
                {
                    MyShip.retrieveDrones();
                }
                if (command.Equals("hasDronesInSpace"))
                {
                    MyShip.hasDronesInSpace();
                }

                if (command.Equals("hasAvailableDrones"))
                {
                    MyShip.hasAvailableDrones();
                }

                if (command.Equals("setActiveTarget"))
                {
                    //MyShip.setActiveTarget(
                }

                if (command.Equals("openTargetMenu"))
                {
                    //MyShip.openTargetMenu(
                }

                if (command.Equals("engageDrones"))
                {
                    MyShip.engageDrones();
                }

                if (command.Equals("toggleLocalDrones"))
                {
                    MyShip.toggleLocalDrones();
                }

                if (command.Equals("getCargoSpaceRemaining"))
                {
                    MyShip.getCargoSpaceRemaining();
                }

                if (command.Equals("getShipCapacity"))
                {
                    MyShip.getShipCapacity();
                }

                if (command.Equals("stop"))
                {
                    MyShip.stop();
                }

                if (command.Equals("loadAmmo"))
                {
                    MyShip.loadAmmo(1, "lead");
                }

                if(command.Equals("unloadAmmo"))
                {
                    MyShip.unloadAmmo(1);
                }

                if(command.Equals("toggleHighPowerSlot"))
                {
                    MyShip.toggleHighPowerSlot(1);
                }

                if (command.Equals("toggleMedPowerSlot"))
                {
                    MyShip.toggleMedPowerSlot(1);
                }

                if (command.Equals("toggleLowPowerSlot"))
                {
                    MyShip.toggleLowPowerSlot(1);
                }

                if (command.Equals("activateHighPowerSlot"))
                {
                    MyShip.activateHighPowerSlot(1);
                }

                if (command.Equals("deactivateHighPowerSlot"))
                {
                    MyShip.deactivateHighPowerSlot(1);
                }

                if (command.Equals("activateMedPowerSlot"))
                {
                    MyShip.activateMedPowerSlot(1);
                }

                if (command.Equals("deactivateMedPowerSlot"))
                {
                    MyShip.deactivateLowPowerSlot(1);
                }

                if (command.Equals("activateLowPowerSlot"))
                {
                    MyShip.activateLowPowerSlot(1);
                }

                if (command.Equals("deactivateLowPowerSlot"))
                {
                    MyShip.deactivateLowPowerSlot(1);
                }

                if (command.Equals("getHighSlotTargetingRange"))
                {
                    MyShip.getHighSlotTargetingRange(1);
                }

                if (command.Equals("hasHighSlot"))
                {
                    MyShip.hasHighSlot(1);
                }

                if (command.Equals("getHighSlotAttributes"))
                {
                    MyShip.getHighSlotAttributes(1);
                }

                if (command.Equals("getHighSlotModuleInfo"))
                {
                    MyShip.getHighSlotModuleInfo(1);
                }

                if (command.Equals("hasMedSlot"))
                {
                    MyShip.hasMedSlot(1);
                }

                if (command.Equals("hasLowSlot"))
                {
                    MyShip.hasLowSlot(1);
                }

                if (command.Equals("isHighSlotActive"))
                {
                    MyShip.isHighSlotActive(1);
                }

                if (command.Equals("isMedSlotActive"))
                {
                    MyShip.isMedSlotActive(1);
                }

                if (command.Equals("isLowSlotActive"))
                {
                    MyShip.isLowSlotActive(1);
                }

                if (command.Equals("getSpeed"))
                {
                    MyShip.getSpeed();
                }

                if (command.Equals("warpToZero"))
                {
                    MyShip.warpToZero();
                }

                if (command.Equals("isDocked"))
                {
                    MyShip.isDocked();
                }

                if (command.Equals("unDock"))
                {
                    MyShip.unDock();
                }

                if (command.Equals("dock"))
                {
                    MyShip.dock("I");
                }

                if (command.Equals("getCargo"))
                {
                    MyShip.getCargo();
                }

                
                //station
                if (command.Equals("isItemInHangar"))
                {
                    EStationHandler.isItemInHangar("tritanium");
                }
                if (command.Equals("withdrawItem"))
                {
                    EStationHandler.withdrawItem("Tritanium");
                }

                if (command.Equals("stackHangarItems"))
                {
                    EStationHandler.stackHangarItems();
                }
                if (command.Equals("isHangarOpen"))
                {
                    EStationHandler.isHangarOpen();
                }
                if (command.Equals("selectAgentTab"))
                {
                    EStationHandler.selectAgentTab();
                }
                if (command.Equals("openHangar"))
                {
                    EStationHandler.openHangar();
                }

                if (command.Equals("selectAllCargo"))
                {
                    EStationHandler.selectAllCargo();
                }

                if (command.Equals("depositAll"))
                {
                    EStationHandler.depositAll();
                }

                if (command.Equals("isDocked"))
                {
                    EStationHandler.isDocked();
                }

                if (command.Equals("unDock"))
                {
                    EStationHandler.undock();
                }

                if (command.Equals("isLoading"))
                {
                    ESession.isLoading();
                }

                if (command.Equals("isSystemMenuOpen"))
                {
                    ESession.isSystemMenuOpen();
                }

                if (command.Equals("getNoButton"))
                {
                    ESession.getNoButton();
                }

                if (command.Equals("atLogin"))
                {
                    ESession.atLogin();
                }

                if (command.Equals("atCharSel"))
                {
                    ESession.atCharSel();
                }
                if (command.Equals("bookmarkInSpace"))
                {
                    ESession.bookMarkInSpace();
                }

                if (command.Equals("getEnterButton"))
                {
                    ESession.getEnterButton();
                }
                if (command.Equals("getConnectButton"))
                {
                    ESession.getConnectButton();
                }

                if (command.Equals("logout"))
                {
                    ESession.logout();
                }

                if (command.Equals("openSystemMenu"))
                {
                    ESession.openSystemMenu();
                }

                if (command.Equals("getLocalCount"))
                {
                    ESession.getLocalCount();
                }
                if (command.Equals("initializeLocal"))
                {
                    ESession.initializeLocal();
                }

                if (command.Equals("getSolarSystem"))
                {
                    ESession.getSolarSystem();
                }

                

   
                command = "";

                /*
                if (commandQ[0].Equals("WARP"))
                {
                    EMenuHandler.select(MenuHandler.MENUITEMS.ASTEROIDBELTS);
                    Thread.Sleep(300);
                    EMenuHandler.select(MenuHandler.MENUITEMS.BELT1);
                    Thread.Sleep(300);
                    EMenuHandler.click(MenuHandler.MENUITEMS.WARPTOZERO);
                    commandQ.Remove("WARP");
                }
                if (commandQ[0].Equals("TARGET"))
                {
                    MyShip.target("Scordite");
                    commandQ.Remove("TARGET");
                }
                */
                
            }

            return 2000;

        }
    }
}