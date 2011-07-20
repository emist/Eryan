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
        
        /// <summary>
        /// Function calls name constant table
        /// </summary>
        public struct CALLS
        {
            public const string ATLOGIN = "atLogin";
            public const string FINDBYNAMELOGIN = "findByNameLogin";
            public const string FINDBYTEXTLOGIN = "findByTextLogin";
            public const string FINDBYTEXTMENU = "findByTextMenu";
            public const string GETINFLIGHTINTERFACE = "getInflightInterface";
            public const string ISMENUOPEN = "isMenuOpen";
            public const string GETOVERVIEWITEMS = "getOverViewItems";
        }

       /// <summary>
       /// Builds a functionCall object with no parameters
       /// </summary>
       /// <param name="function">The function name to be called</param>
       /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            return builder.Build();

        }

        /// <summary>
        /// Builds a functionCall object with one parameter
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arg">The parameter to pass with the functionCall object</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, string arg)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Strparameter = arg;
            return builder.Build();
        }

        /// <summary>
        /// Builds a functionCall object with a variable argument list
        /// </summary>
        /// <param name="function">The function name to be called</param>
        /// <param name="arguments">A List of Strings containing the parameters to pass with the functionCall object</param>
        /// <returns>The serializeable functionCall object</returns>
        public eveobjects.functionCall build(string function, List<string> arguments)
        {

            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            

            for (int i = 0; i < arguments.Count; i++)
            {
                builder.Strparameter = builder.Strparameter += arguments[i] + ";";
            }

            builder.Strparameter = builder.Strparameter.Substring(0, builder.Strparameter.Length - 1);

            return builder.Build();
        }
    }
}
