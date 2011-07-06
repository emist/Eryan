using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eryan
{
    public static class DrawAbleScreenFetcher
    {
        private static List<Utils> screens = new List<Utils>();

        public static Utils fetch(uint pid)
        {
            foreach (Utils util in screens)
            {
                if (util.getPid() == pid)
                    return util;
            }
            return null;
        }

        public static void addScreen(Utils screen)
        {
            lock (screens)
            {
                screens.Add(screen);
            }
        }
    }
}
