using NLog;
using System;

namespace EOSLib
{
    public class Class1
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string test;

        public void testp()
        {
            logger.Info("Logged via NLOG");
        }
    }
}
