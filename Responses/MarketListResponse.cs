using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


using eveobjects;
using Eryan.Wrappers;



namespace Eryan.Responses
{
    /// <summary>
    /// Wraps a target list Response from the client
    /// </summary>
    public class MarketListResponse : Response
    {
        List<MarketEntry> data;
        TargetList TargetObject;


        /// <summary>
        /// Concrete response for target list replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public MarketListResponse(byte[] input)
        {
            TargetObject = TargetList.CreateBuilder().MergeFrom(input).Build();
            data = new List<MarketEntry>();
            foreach (targetentry entry in TargetObject.ThistargetList)
            {
                data.Add(new MarketEntry(entry.Text.Text, entry.Text.TopLeftY, entry.Text.TopLeftX, entry.Text.Height, entry.Text.Width));
                Console.WriteLine(entry.Text.Text);
                Console.WriteLine(entry.Text.TopLeftX);
                Console.WriteLine(entry.Text.TopLeftY);
                Console.WriteLine(entry.Text.Width);
                Console.WriteLine(entry.Text.Height);
            }
        }


        /// <summary>
        /// Initializes the data elements
        /// </summary>
        public override void HandleResponse()
        {

            Console.WriteLine("handling response");
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
