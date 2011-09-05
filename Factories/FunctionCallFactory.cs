using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Factories
{
    /// <summary>
    /// This class builds functionCall objects
    /// </summary>
    public class FunctionCallFactory
    {
        eveobjects.functionCall function;
        
        /// <summary>
        /// Function calls name constant table
        /// </summary>
        public struct CALLS
        {
            /// <summary>
            /// atLogin call
            /// </summary>
            public const string ATLOGIN = "atLogin";
            /// <summary>
            /// Call to find items by name in the login screen
            /// </summary>
            public const string FINDBYNAMELOGIN = "findByNameLogin";
            /// <summary>
            /// Call to find items by text in the login screen
            /// </summary>
            public const string FINDBYTEXTLOGIN = "findByTextLogin";
            /// <summary>
            /// Call to find items by text in a menu
            /// </summary>
            public const string FINDBYTEXTMENU = "findByTextMenu";
            /// <summary>
            /// Call to get the InflightInterface
            /// </summary>
            public const string GETINFLIGHTINTERFACE = "getInflightInterface";
            /// <summary>
            /// Call to check if the menu is open
            /// </summary>
            public const string ISMENUOPEN = "isMenuOpen";
            /// <summary>
            /// Call to retrieve the overview items
            /// </summary>
            public const string GETOVERVIEWITEMS = "getOverViewItems";
            /// <summary>
            /// Call to retrieve the currently selected item
            /// </summary>
            public const string GETSELECTEDITEM = "getSelectedItem";
            /// <summary>
            /// Call to get the list of currently targeted things
            /// </summary>
            public const string GETTARGETLIST = "getTargetList";

            /// <summary>
            /// Call to get the data of the given Highslot
            /// </summary>
            public const string GETHIGHSLOT = "getHighSlot";

            /// <summary>
            /// Get the data associated with the given mid slot
            /// </summary>
            public const string GETMEDSLOT = "getMidSlot";

            /// <summary>
            /// Get the data associated with the given low slot
            /// </summary>
            public const string GETLOWSLOT = "getLowSlot";

            /// <summary>
            /// Call to get the status of the given high slot
            /// </summary>
            public const string ISHIGHSLOTACTIVE = "isHighSlotActive";

            /// <summary>
            /// Retrieve the ship's cargo
            /// </summary>
            public const string GETCARGOLIST = "getCargoList";

            /// <summary>
            /// Retrieve the Undock button's location
            /// </summary>
            public const string GETUNDOCKBUTTON = "getUndockButton";

            /// <summary>
            /// Retrieve the Ship hangar
            /// </summary>
            public const string GETSHIPHANGAR = "getShipHangar";

            /// <summary>
            /// Retrive the Station hangar
            /// </summary>
            public const string GETSTATIONHANGAR = "getStationHangar";

            /// <summary>
            /// Retrieve the items button at the station
            /// </summary>
            public const string GETITEMSBUTTON = "getItemsButton";

            /// <summary>
            /// Retrieve the ship's armor
            /// </summary>
            public const string GETSHIPARMOR = "getShipArmor";

            /// <summary>
            /// Retrieve the ship's shiled
            /// </summary>
            public const string GETSHIPSHIELD = "getShipShield";

            /// <summary>
            /// Retrieve the ship's structure
            /// </summary>
            public const string GETSHIPSTRUCTURE = "getShipStructure";

            /// <summary>
            /// Retrieve the ship's speed
            /// </summary>
            public const string GETSHIPSPEED = "getShipSpeed";

            /// <summary>
            /// Retrieve the ship's capacity
            /// </summary>
            public const string GETSHIPCAPACITY = "getShipCapacity";

            /// <summary>
            /// Retrieve the currently visible menu items
            /// </summary>
            public const string GETMENUITEMS = "getMenuItems";

            /// <summary>
            /// Retrieve the current system
            /// </summary>
            public const string GETSYSTEMINFORMATION = "getSystemInformation";

            /// <summary>
            /// Get the cancel button of a modal window
            /// </summary>
            public const string GETMODALCANCELBUTTON = "getModalCancelButton";

            /// <summary>
            /// Get the OK button of a modal window if present
            /// </summary>
            public const string GETMODALOKBUTTON = "getModalOkButton";

            /// <summary>
            /// Check if the system menu is open
            /// </summary>
            public const string ISSYSTEMMENUOPEN = "isSystemMenuOpen";

            /// <summary>
            /// Check if a loading screen is open
            /// </summary>
            public const string ISLOADING = "isLoading";

            /// <summary>
            /// Get the last server message 
            /// </summary>
            public const string GETSERVERMESSAGE = "getServerMessage";

            /// <summary>
            /// Get the interface windows
            /// </summary>
            public const string GETINTERFACEWINDOWS = "getInterfaceWindows";

            /// <summary>
            /// Get the module's max targeting range
            /// </summary>
            public const string GETTARGETINGRANGE = "getTargetingRange";

            /// <summary>
            /// Check if incursion is undergoing
            /// </summary>
            public const string ISINCURSIONONGOING = "isIncursion";

            /// <summary>
            /// Return the position of the drones in space tab
            /// </summary>
            public const string GETDRONESINSPACETAB = "dronesInSpace";

            /// <summary>
            /// Return the position of the drones in bay tab
            /// </summary>
            public const string GETDRONESINBAYTAB = "dronesInBay";

            /// <summary>
            /// Return true if local is hostile, false otherwise
            /// </summary>
            public const string CHECKLOCAL = "checkLocal";

            /// <summary>
            /// Check if drones are engaged or idle
            /// </summary>
            public const string CHECKDRONESTATUS = "checkDroneStatus";

            /// <summary>
            /// Return the given strip miner's mining amount
            /// </summary>
            public const string GETMININGAMOUNT = "getMiningAmount";

            /// <summary>
            /// Return the given module's cycle duration
            /// </summary>
            public const string GETDURATION = "getDuration";

            /// <summary>
            /// Return the scroll bar for the overview
            /// </summary>
            public const string GETOVERVIEWSCROLL = "overviewGetScrollBar";

            /// <summary>
            /// Return the overview height
            /// </summary>
            public const string GETOVERVIEWHEIGHT = "getOverviewHeight";

            /// <summary>
            /// Get the bottom of the overview scroll area
            /// </summary>
            
            public const string GETOVERVIEWBOTTOM = "getOverviewBottom";

            /// <summary>
            /// Get the top of the overview scroll area
            /// </summary>
            public const string GETOVERVIEWTOP = "getOverviewTop";

            /// <summary>
            /// Return the eve client's version
            /// </summary>
            public const string GETVERSION = "getVersion";

            /// <summary>
            /// Return the bookmark name
            /// </summary>
            public const string GETBOOKMARKFIELDNAME = "getBookMarkFieldName";

            /// <summary>
            /// Get the location of the logoff button
            /// </summary>
            public const string GETLOGOFFBUTTON = "getLogOffButton";

            /// <summary>
            /// Get the Yes button on a modal dialogue
            /// </summary>
            public const string GETMODALYESBUTTON = "getModalYesButton";

            /// <summary>
            /// Get the No button on a modal dialogue
            /// </summary>
            public const string GETMODALNOBUTTON = "getModalNoButton";

            /// <summary>
            /// Find the given player name in local
            /// </summary>
            public const string FINDPLAYERINLOCAL = "findPlayerInLocal";

            /// <summary>
            /// Get the top of the local chat window
            /// </summary>
            public const string GETLOCALCHATTOP = "getLocalChatTop";

            /// <summary>
            /// Get the bottom of the local chat window
            /// </summary>
            public const string GETLOCALCHATBOTTOM = "getLocalChatBottom";

            /// <summary>
            /// Get the local chat scroll bar
            /// </summary>
            public const string GETLOCALCHATSCROLLBAR = "getLocalChatScrollbar";

            /// <summary>
            /// Check if player is fleeted
            /// </summary>
            public const string ISFLEETED = "isFleeted";

            /// <summary>
            /// Get the password entry box
            /// </summary>
            public const string GETPASSWORDBOX = "getPasswordBox";

            /// <summary>
            /// Get the username box
            /// </summary>
            public const string GETUSERNAMEBOX = "getUserNameBox";

            /// <summary>
            /// Get the connect button
            /// </summary>
            public const string GETCONNECTBUTTON = "getConnectButton";

            /// <summary>
            /// Check if we are at the character selection screen
            /// </summary>
            public const string ISATCHARSEL = "isAtCharSel";

            /// <summary>
            /// Get the "Enter Game" button in the character selection screen
            /// </summary>
            public const string GETENTERGAMEBTN = "getEnterButton";

            /// <summary>
            /// Check if the given low slot is active
            /// </summary>
            public const string ISLOWSLOTACTIVE = "isLowSlotActive";

            /// <summary>
            /// Check if the given med slot is active
            /// </summary>
            public const string ISMEDSLOTACTIVE = "isMedSlotActive";

            /// <summary>
            /// Get the items in the hangar
            /// </summary>
            public const string GETHANGARITEMS = "getHangarItems";

            /// <summary>
            /// Get people and places tab in the neocom
            /// </summary>
            public const string GETPEOPLEANDPLACES = "getPeopleAndPlaces";

            /// <summary>
            /// Get the places tab in the addressbook
            /// </summary>
            public const string GETADDRESSBOOKPLACESTAB = "getAddressBookPlacesTab";

            /// <summary>
            /// Get the bookmark button in the addressbook
            /// </summary>
            public const string GETADDRESSBOOKBMBUTTON = "getAddressBookBMButton";

            /// <summary>
            /// Get the addressbook window
            /// </summary>
            public const string GETADDRESSBOOKWINDOW = "getAddressBookWindow";

            /// <summary>
            /// Get the current count in local
            /// </summary>
            public const string GETLOCALCOUNT = "getLocalCount";

            /// <summary>
            /// Get the "items" tab in the neocom
            /// </summary>
            public const string GETNEOCOMITEMSBUTTON = "getNeoComItems";

            /// <summary>
            /// Get the overview selection icon
            /// </summary>
            public const string GETOVERVIEWSELECTION = "getOverViewSelectIcon";

            /// <summary>
            /// Get the name of the currently selected overview
            /// </summary>
            public const string GETOVERVIEWSELECTTEXT = "getOverviewSelectText";
        }


       /// <summary>
       /// Builds a functionCall object with no parameters
       /// </summary>
       /// <param name="function">The function name to be called</param>
       /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            return builder.Build();

        }

        /// <summary>
        /// Builds a functionCall object with one parameter
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arg">The parameter to pass with the functionCall object</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, string arg)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Strparameter = arg;
            return builder.Build();
        }

        /// <summary>
        /// Builds a functionCall object with a variable argument list
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arguments">A List of Strings containing the parameters to pass with the functionCall object</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, List<string> arguments)
        {

            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            

            for (int i = 0; i < arguments.Count; i++)
            {
                builder.Strparameter = builder.Strparameter += arguments[i] + ";";
            }

            builder.Strparameter = builder.Strparameter.Substring(0, builder.Strparameter.Length - 1);

            return builder.Build();
        }
    }
}
