using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Wrapper for a Module
    /// </summary>
    public class Module
    {
        string name;
        string status, chargename, overload, shortcut;
        int chargeqty;

        /// <summary>
        /// Build a module
        /// </summary>
        /// <param name="name">Name of the item in the slot</param>
        /// <param name="status">The status of the item</param>
        /// <param name="chargeqty">The ammount of ammo in the mod if any</param>
        /// <param name="chargename">The name of the ammo if any</param>
        /// <param name="overload">Overload status of the module</param>
        /// <param name="shortcut">The shortcut to activate the mod</param>
        public Module(string name, string status, int chargeqty, string chargename, string overload, string shortcut)
        {
            this.name = name;
            this.status = status;
            this.chargename = chargename;
            this.overload = overload;
            this.shortcut = shortcut;
        }

        /// <summary>
        /// The name of the selected item
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The status of the item
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// The ammount of charge in the module if any
        /// </summary>
        public int ChargeQTY
        {
            get
            {
                return chargeqty;
            }
        }

        /// <summary>
        /// The name of the charge in the mod if any
        /// </summary>
        public string ChargeName
        {
            get
            {
                return chargename;
            }
        }

        /// <summary>
        /// The overload status of the module
        /// </summary>
        public string Overload
        {
            get
            {
                return overload;
            }
        }

        /// <summary>
        /// The shortcut to activate the module
        /// </summary>
        public string Shortcut
        {
            get
            {
                return shortcut;
            }
        }

    }
}
