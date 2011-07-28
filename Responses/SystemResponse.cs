using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;
using Eryan.Wrappers;

namespace Eryan.Responses
{
    /// <summary>
    /// Wraps an System Response from the client
    /// </summary>
    public class SystemResponse : Response
    {
        List<string> data;

        systemObject SystemObject;

        /// <summary>
        /// Concrete response for Interface replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public SystemResponse(byte[] input)
        {
            SystemObject = systemObject.CreateBuilder().MergeFrom(input).Build();
            //data = new List<OverViewEntry>();

            //data.Add(new OverViewEntry(lab.Text, lab.TopLeftY, lab.TopLeftX, lab.Height, lab.Width));
            Console.WriteLine(SystemObject.Name);
            Console.WriteLine(SystemObject.Info);
                //     Console.WriteLine(lab.TopLeftY);
                //     Console.WriteLine(lab.Width);
                //     Console.WriteLine(lab.Height);
            
        }


        /// <summary>
        /// Initializes the data elements
        /// </summary>
        public override void HandleResponse()
        {

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
    }
}
