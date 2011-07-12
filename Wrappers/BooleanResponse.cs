using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    public class BooleanResponse : Response
    {
        bool data = false;
        byte[] input;
        public BooleanResponse(byte[] input)
        {
            this.input = input;
        }
        public override void HandleResponse()
        {
            string output = Encoding.ASCII.GetString(input, 0, Array.IndexOf(input, (byte)0));
            output =output.Trim();
            //Console.WriteLine("HandleResponse: " + output);
            data = output.Equals("True");
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
