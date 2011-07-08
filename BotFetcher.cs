using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eryan
{
    public static class BotFetcher
    {
        private static List<Bot> bots = new List<Bot>();
        private static ReaderWriterLock rwlock = new ReaderWriterLock();


        public static Bot fetch(uint pid)
        {
            int timeout = 300;
            rwlock.AcquireReaderLock(timeout);
            try
            {
                foreach (Bot bot in bots)
                {
                    if (bot.getPid() == pid)
                        return bot;
                }
                return null;
            }
            finally
            {
                rwlock.ReleaseReaderLock();
            }

        }

        public static void addBot(Bot b)
        {
            int timeout = 300;
            rwlock.AcquireWriterLock(timeout);
            try
            {
                foreach (Bot bot in bots)
                {
                    if (b.Equals(bot))
                        return;
                }
                bots.Add(b);
            }
            finally
            {
                rwlock.ReleaseWriterLock();
            }
        }
    }
}
