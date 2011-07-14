using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Factories
{
    public class FunctionCallFactory
    {
        eveobjects.functionCall function;

        //Function calls this builder builds

        public const string ATLOGIN = "atLogin";
        public const string FINDBYNAME = "findByName";

       
        public eveobjects.functionCall build(string function)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            return builder.Build();
        }

        public eveobjects.functionCall build(string function, List<string> arguments)
        {

            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();

            for (int i = 0; i < arguments.Count; i++)
            {
                builder.Strparameter = builder.Strparameter += arguments[i] + ";";
            }

            return builder.Build();
        }
    }
}
