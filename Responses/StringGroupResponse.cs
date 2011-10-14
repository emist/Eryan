using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;
using Eryan.Wrappers;

namespace Eryan.Responses
{
    public class StringGroupResponse : Response
    {
        List<string> labels;
        stringgroup groupObject;

        public StringGroupResponse(byte[] input)
        {
            labels = new List<string>();
            groupObject = stringgroup.CreateBuilder().MergeFrom(input).Build();
            foreach (eveobjects.stringResponse resp in groupObject.DataList)
            {
                labels.Add(resp.Data);
                Console.WriteLine(resp.Data);
            }
        }

        public List<string> Data
        {
            get
            {
                return labels;
            }
        }

    }
}
