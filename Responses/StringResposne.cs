using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eveobjects;

namespace Eryan.Responses
{
    /// <summary>
    /// Concrete Response for single field replies
    /// </summary>
    public class StringResponse : Response
    {

        String data;
        stringResponse stringObject;

        /// <summary>
        /// Creates a StringResponse object from its byte representation
        /// </summary>
        /// <param name="input">Byte representation of the object</param>
        public StringResponse(byte[] input)
        {
            stringObject = stringResponse.CreateBuilder().MergeFrom(input).Build();
            Console.WriteLine(stringObject.Data);
            data = stringObject.Data;
        }

        /// <summary>
        /// Initializes the response variables
        /// </summary>
        public override void HandleResponse()
        {

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
