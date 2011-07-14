using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eryan
{
    /// <summary>
    /// Singleton to keep track of all running bots
    /// </summary>
    public static class BotFetcher
    {
        private static List<Bot> bots = new List<Bot>();
        private static ReaderWriterLock rwlock = new ReaderWriterLock();

        /// <summary>
        /// Fetches a bot based on PID
        /// </summary>
        /// <param name="pid">The PID of the EVE process the bot is attached to</param>
        /// <returns>A bot reference if a bot is found, null if not</returns>
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

        /// <summary>
        /// Registers a bot with the singleton
        /// </summary>
        /// <param name="b">The reference to the bot to be registered</param>
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
