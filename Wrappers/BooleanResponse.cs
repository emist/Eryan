using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;

namespace Eryan.Wrappers
{
    public class BooleanResponse : Response
    {
        Boolean data = true;
        eveobjects.BooleanObject booleanObject;

        public BooleanResponse(byte[] input)
        {
            booleanObject = eveobjects.BooleanObject.CreateBuilder().MergeFrom(input).Build();
            Console.WriteLine(booleanObject.Istrue);
        }
        public override void HandleResponse()
        {
            data = booleanObject.Istrue;
        }
        public override Object Data
        {
            get
            {
                return data;
            }
        }

    }
}
