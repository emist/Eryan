using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;

namespace Eryan.Wrappers
{   
    /// <summary>
    /// Concrete Response for boolean replies
    /// </summary>
    public class BooleanResponse : Response
    {
       
        Boolean data = true;
        BooleanObject booleanObject;

        /// <summary>
        /// Creates a BooleanResponse object from its byte representation
        /// </summary>
        /// <param name="input">Byte representation of the object</param>
        public BooleanResponse(byte[] input)
        {
            booleanObject = BooleanObject.CreateBuilder().MergeFrom(input).Build();
            //Console.WriteLine(booleanObject.Istrue);
        }

        /// <summary>
        /// Initializes the response variables
        /// </summary>
        public override void HandleResponse()
        {
            data = booleanObject.Istrue;
        }

        /// <summary>
        /// Accessor for the data private variable
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
