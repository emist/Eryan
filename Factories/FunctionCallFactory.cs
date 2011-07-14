using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Factories
{
    /// <summary>
    /// This class builds functionCall objects
    /// </summary>
    public class FunctionCallFactory
    {
        eveobjects.functionCall function;

        //Function calls this builder builds

        public struct calls
        {
            public const string ATLOGIN = "atLogin";
            public const string FINDBYNAME = "findByName";
        }

       /// <summary>
       /// Builds a functionCall object with no parameters
       /// </summary>
       /// <param name="function">The function name to be called</param>
       /// <param name="responsetype">The type of the expected response as given by the response struct in Responses</param>
       /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, string responsetype)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Responsetype = responsetype;
            return builder.Build();

        }

        /// <summary>
        /// Builds a functionCall object with one parameter
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arg">The parameter to pass with the functionCall object</param>
        /// <param name="responsetype">The type of the expected response as given by the response struct in Responses</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, string arg, string responsetype)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Strparameter = arg;
            builder.Responsetype = responsetype;
            return builder.Build();
        }

        /// <summary>
        /// Builds a functionCall object with a variable argument list
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arguments">A List of Strings containing the parameters to pass with the functionCall object</param>
        /// <param name="responsetype">The type of the expected response as given by he response struct in Responses</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, List<string> arguments, string responsetype)
        {

            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Responsetype = responsetype;

            for (int i = 0; i < arguments.Count; i++)
            {
                builder.Strparameter = builder.Strparameter += arguments[i] + ";";
            }

            builder.Strparameter = builder.Strparameter.Substring(0, builder.Strparameter.Length - 1);

            return builder.Build();
        }
    }
}
