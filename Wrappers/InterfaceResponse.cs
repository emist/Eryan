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


        /// <summary>
        /// Concrete response for Interface replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public InterfaceResponse(byte[] input)
        {
            interfaceObject = Interface.CreateBuilder().MergeFrom(input).Build();
            Console.WriteLine(interfaceObject.Name);
            Console.WriteLine(interfaceObject.TopleftX);
            Console.WriteLine(interfaceObject.TopleftY);
        }


        /// <summary>
        /// Initializes the data elements
        /// </summary>
        public override void HandleResponse()
        {
            name = interfaceObject.Name;
            x = interfaceObject.TopleftY;
            y = interfaceObject.TopleftX;
            data = new List<string>();
            data.Add(name);
            data.Add(x + "");
            data.Add(y + "");
        }
        

        /// <summary>
        /// List represenation of the data
        /// </summary>
        public override Object Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Getter for the name of this interface
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Getter for the X position of this interface
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Getter for the Y position of this interface
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }
    }
}
