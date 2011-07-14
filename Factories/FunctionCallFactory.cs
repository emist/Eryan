using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Factories
{
    public class FunctionCallFactory
    {
        eveobjects.functionCall function;

       
        public eveobjects.functionCall build(string function)
        {
            this.function = new eveobjects.functionCall();
            eveobjects.functionCall.Builder builder = this.function.ToBuilder();
            builder.Name = function;
            return builder.Build();
        }

        public eveobjects.functionCall.Builder build(string function, List<string> arguments)
        {
            return null;
        }
    }
}
