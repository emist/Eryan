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


        public eveobjects.functionCall build(string function, string arg)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            builder.Strparameter = arg;
            return builder.Build();
        }


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
