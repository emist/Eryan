using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;

namespace Eryan.Wrappers
{
    public class BooleanResponse : Response
    {
        bool data = false;
        eveobjects.Interaface eveobject;



        public BooleanResponse(byte[] input)
        {
            eveobject = eveobjects.Interaface.CreateBuilder().MergeFrom(input).Build();
            //Console.WriteLine(eveobject.Name);
        }
        public override void HandleResponse()
        {

            data = eveobject.Name.Contains("proto");
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
