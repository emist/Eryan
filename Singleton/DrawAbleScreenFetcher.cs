using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Eryan.Util;

namespace Eryan.Singleton
{
    /// <summary>
    /// Singleton to keep track of all the drawabale screens
    /// </summary>
    public static class DrawAbleScreenFetcher
    {
        private static List<DrawableScreen> screens = new List<DrawableScreen>();
        private static ReaderWriterLock rwlock = new ReaderWriterLock();


        public static DrawableScreen fetchOpaque(uint pid)
        {
            int timeout = 300;
            rwlock.AcquireReaderLock(timeout);
            try
            {
                foreach (DrawableScreen util in screens)
                {
                    if (util.getPid() == pid)
                        if (util.Opacity == 1.00)
                            return util;
                }
                return null;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }

        public static List<DrawableScreen> fetchAllScreens(uint pid)
        {
            List<DrawableScreen> screenList = new List<DrawableScreen>();
            int timeout = 300;
            rwlock.AcquireReaderLock(timeout);
            try
            {
                foreach (DrawableScreen util in screens)
                {
                    if (util.getPid() == pid)
                        screenList.Add(util);
                }
                return screenList;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Fetch the drawable area attached to an EVE process
        /// </summary>
        /// <param name="pid">The EVE process' pid</param>
        /// <returns>The drawable area if it exists</returns>
        public static DrawableScreen fetch(uint pid)
        {
            int timeout = 300;
            rwlock.AcquireReaderLock(timeout);
            try
            {
                foreach (DrawableScreen util in screens)
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

        /// <summary>
        /// Register a drawable area with the singleton
        /// </summary>
        /// <param name="screen">The drawable area to register</param>
        public static void addScreen(DrawableScreen screen)
        {
            int timeout = 300;
            rwlock.AcquireWriterLock(timeout);
            try
            {
                foreach (DrawableScreen s in screens)
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
