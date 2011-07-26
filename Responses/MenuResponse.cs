using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;
using Eryan.Wrappers;

namespace Eryan.Responses
{
    /// <summary>
    /// Wraps an overview Response from the client
    /// </summary>
    public class MenuResponse : Response
    {
        List<MenuEntry> data;
        overview OverViewObject;


        /// <summary>
        /// Concrete response for Interface replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public MenuResponse(byte[] input)
        {
            OverViewObject = overview.CreateBuilder().MergeFrom(input).Build();
            data = new List<MenuEntry>();
            foreach (label lab in OverViewObject.OverviewEntryList)
            {
                Console.WriteLine(lab.Text);
                Console.WriteLine(lab.TopLeftX);
                Console.WriteLine(lab.TopLeftY);
                  
                data.Add(new MenuEntry(lab.Text, lab.TopLeftY, lab.TopLeftX, lab.Height, lab.Width));
            }
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
