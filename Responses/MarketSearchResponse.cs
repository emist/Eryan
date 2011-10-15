using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using eveobjects;
using Eryan.Wrappers;


namespace Eryan.Responses
{
    public class MarketSearchResponse : Response
    {
        String text;
        List<MarketSearchEntry> data;
        TargetList TargetObject;


        /// <summary>
        /// Concrete response for target list replies
        /// </summary>
        /// <param name="input">The byte representation of the reply</param>
        public MarketSearchResponse(byte[] input)
        {
            TargetObject = TargetList.CreateBuilder().MergeFrom(input).Build();
            data = new List<MarketSearchEntry>();
            foreach (targetentry entry in TargetObject.ThistargetList)
            {
                data.Add(new MarketSearchEntry(entry.Text.Text, entry.Text.TopLeftY, entry.Text.TopLeftX, entry.Text.Height, entry.Text.Width));
                Console.WriteLine(entry.Text.Text);
                Console.WriteLine(entry.Text.TopLeftX);
                Console.WriteLine(entry.Text.TopLeftY);
                Console.WriteLine(entry.Text.Width);
                Console.WriteLine(entry.Text.Height);
            }
        }

        public List<MarketSearchEntry> Data
        {
            get
            {
                return data;
            }
        }

    }
}
