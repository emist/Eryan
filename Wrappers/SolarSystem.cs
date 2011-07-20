using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    public class SolarSystem
    {
        string name;
        List<string> belts;
        List<string> gates;
        List<string> stations;

        public struct names
        {
            public const string BOURYNES = "Bourynes";
        }

        public SolarSystem(string name, List<string> belts, List<string> gates, List<string> stations)
        {
            this.name = name;         
            this.belts = belts;
            this.gates = gates;
            this.stations = stations;
        }

    }
}
