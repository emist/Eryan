using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;

namespace Eryan.Wrappers
{
    class InterfaceResponse : Response
    {
        List<string> data;
        string name;
        int x, y;
        Interface interfaceObject;

        public InterfaceResponse(byte[] input)
        {
            interfaceObject = Interface.CreateBuilder().MergeFrom(input).Build();
            Console.WriteLine(interfaceObject.Name);
            Console.WriteLine(interfaceObject.TopleftX);
            Console.WriteLine(interfaceObject.TopleftY);
        }

        public override void HandleResponse()
        {
            name = interfaceObject.Name;
            x = interfaceObject.TopleftX;
            y = interfaceObject.TopleftY;
            data = new List<string>();
            data.Add(name);
            data.Add(x + "");
            data.Add(y + "");
        }
        
        public override Object Data
        {
            get
            {
                return data;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public int X
        {
            get
            {
                return x;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
        }
    }
}
