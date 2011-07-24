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
