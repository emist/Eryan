using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Responses
{
    /// <summary>
    /// Abstract response, all Responses inherit from this
    /// </summary>
    public abstract class Response
    {
        Object data;


        public struct RESPONSES
        {
            public const string INTERFACERESPONSE = "InterfaceResponse";
            public const string BOOLEANRESPONSE = "BooleanResponse";
        }

        /// <summary>
        /// Implement in subclass
        /// </summary>
        public virtual void HandleResponse()
        { }

        /// <summary>
        /// Returns the representation of all the Response's data
        /// </summary>
        public virtual Object Data
        {
            get
            {
                return data;
            }
        }
    }

}
