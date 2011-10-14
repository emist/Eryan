using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        /// Builds a solar system from the unparsed response
        /// </summary>
        /// <param name="name">Name of the solar system</param>
        /// <param name="unparsedEntry">The unparsed information string</param>
        public SolarSystem(string name, string unparsedEntry)
        {
            this.name = name;
            Regex reg = new Regex("-?[0-1].[0-9]");
            string entry = reg.Match(unparsedEntry).Value;
            if (entry != "")
            {
                secStatus = Convert.ToDouble(entry);
            }
        }

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
