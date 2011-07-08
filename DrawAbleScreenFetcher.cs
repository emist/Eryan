using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eryan
{
    public static class DrawAbleScreenFetcher
    {
        private static List<Utils> screens = new List<Utils>();
        private static ReaderWriterLock rwlock = new ReaderWriterLock();
        

        public static Utils fetch(uint pid)
        {
            int timeout = 300;
            rwlock.AcquireReaderLock(timeout);
            try
            {
                foreach (Utils util in screens)
                {
                    if (util.getPid() == pid)
                        return util;
                }
                return null;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
            
        }

        public static void addScreen(Utils screen)
        {
            int timeout = 300;
            rwlock.AcquireWriterLock(timeout);
            try
            {
                foreach (Utils s in screens)
                {
                    if (s.Equals(screen))
                        return;
                }
                screens.Add(screen);
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
        }
    }
}
