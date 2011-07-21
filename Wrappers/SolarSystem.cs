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
        int belts;
        List<string> gates;
        List<string> stations;

        /// <summary>
        /// List of all EVE Solary Systems
        /// </summary>
        public struct names
        {
            public const string BOURYNES = "Bourynes";
        }

        /// <summary>
        /// SolarSystem constructor
        /// </summary>
        /// <param name="name">The solarsystem name</param>
        /// <param name="belts">Number of belts in the solar system</param>
        /// <param name="gates">List of gates in the system</param>
        /// <param name="stations">List of stations in the system</param>
        public SolarSystem(string name, int belts, List<string> gates, List<string> stations)
        {
            this.name = name;         
            this.belts = belts;
            this.gates = gates;
            this.stations = stations;
        }

    }
}
