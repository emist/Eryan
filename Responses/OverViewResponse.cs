using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;
using Eryan.Wrappers;

namespace Eryan.Responses
{
    public class OverViewResponse : Response
    {
        List<OverViewEntry> data;
        overview OverViewObject;


        /// <summary>
        /// Concrete response for Interface replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public OverViewResponse(byte[] input)
        {
            OverViewObject = overview.CreateBuilder().MergeFrom(input).Build();
            data = new List<OverViewEntry>();
        }


        /// <summary>
        /// Initializes the data elements
        /// </summary>
        public override void HandleResponse()
        {
            foreach (label lab in OverViewObject.OverviewEntryList)
            {
                
                data.Add(new OverViewEntry(lab.Text, lab.TopLeftY, lab.TopLeftX, lab.Height, lab.Width));
                Console.WriteLine(lab.Text);
                Console.WriteLine(lab.TopLeftX);
                Console.WriteLine(lab.TopLeftY);
                Console.WriteLine(lab.Width);
                Console.WriteLine(lab.Height);
            }
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
