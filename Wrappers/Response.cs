using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Wrappers
{
    public abstract class Response
    {
        Object data;

        public virtual void HandleResponse()
        { }

        public virtual Object Data
        {
            get
            {
                return data;
            }
        }
    }

}
