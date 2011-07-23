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
    public class ItemResponse : Response
    {
        List<Item> data;
        itemgroup itemGroupObject;


        /// <summary>
        /// Concrete response for Interface replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public ItemResponse(byte[] input)
        {
            itemGroupObject = itemgroup.CreateBuilder().MergeFrom(input).Build();
            data = new List<Item>();
        }


        /// <summary>
        /// Initializes the data elements
        /// </summary>
        public override void HandleResponse()
        {
            foreach (item lab in itemGroupObject.ItemsList)
            {
               
//                data.Add(new Item(item.Text, item.TopLeftY, item.TopLeftX, item.Height, item.Width));
                
                
                data.Add(new Item(lab.Quantity, lab.Volume, lab.Data.Text, lab.Meta, lab.Data.Width, lab.Data.Height, lab.Data.TopLeftX, lab.Data.TopLeftY));



                Console.WriteLine(lab.Data.Text);
                Console.WriteLine(lab.Quantity);
                Console.WriteLine(lab.Volume);
                Console.WriteLine(lab.Meta);
                Console.WriteLine(lab.Data.TopLeftX);
                Console.WriteLine(lab.Data.TopLeftY);
                Console.WriteLine(lab.Data.Width);
                Console.WriteLine(lab.Data.Height);
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