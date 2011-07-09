using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    public interface Observer
    {
        void update(Event e);
    }
}
