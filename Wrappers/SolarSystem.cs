using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    /// <summary>
    /// Wrapper for EVE solar systems
    /// </summary>
    public class SolarSystem
    {
        string name;
        string constellation;
        string sov;
        double secStatus;

        /// <summary>
        /// Build a solar system
        /// </summary>
        /// <param name="name">The name of the system</param>
        /// <param name="constellation">The constellation its on</param>
        /// <param name="sov">The sov of the place</param>
        /// <param name="secStatus">The security status of the system</param>
        public SolarSystem(string name, string constellation, string sov, double secStatus)
        {
            this.name = name;
            this.constellation = constellation;
            this.sov = sov;
            this.secStatus = secStatus;
        }

        /// <summary>
        /// Return the name of the system
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        
        /// <summary>
        /// Return the Constellation the system is in
        /// </summary>
        public string Constellation
        {
            get
            {
                return constellation;
            }
        }

        /// <summary>
        /// Return this system's sovereignity
        /// </summary>
        public string Sovereignty
        {
            get
            {
                return sov;
            }
        }

        /// <summary>
        /// Return the system's security level
        /// </summary>
        public double Security
        {
            get
            {
                return secStatus;
            }
        }

    }
}
