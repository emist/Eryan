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

        /// <summary>
        /// Response types constants
        /// </summary>
        public struct RESPONSES
        {
            /// <summary>
            /// The InterfaceResponse type const
            /// </summary>
            public const string INTERFACERESPONSE = "InterfaceResponse";

            /// <summary>
            /// Boolean Response type const
            /// </summary>
            public const string BOOLEANRESPONSE = "BooleanResponse";

            /// <summary>
            /// OverviewResponse type const
            /// </summary>
            public const string OVERVIEWRESPONSE = "OverViewResponse";

            /// <summary>
            /// TargetResponse type const
            /// </summary>
            public const string TARGETRESPONSE = "TargetResponse";

            /// <summary>
            /// ItemResponse type const
            /// </summary>
            public const string ITEMRESPONSE = "ItemResponse";

            /// <summary>
            /// StringResponse type const
            /// </summary>
            public const string STRINGRESPONSE = "StringResponse";
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
