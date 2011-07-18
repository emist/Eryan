using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan.Script
{
    /// <summary>
    /// Scriptable interface must be implemented by all Eryan scripts
    /// </summary>

    public interface Scriptable
    {

        Boolean onStart();

        Boolean onFinish();

        int run();

    }
   
}
