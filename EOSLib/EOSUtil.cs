using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public static class EOSUtil
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime EOSTimeToUTC(Int64 slotTime)
        {
            Int64 interval = 500;
            Int64 epoch = 946684800000;

            var unixEpochTime = (slotTime * interval + epoch) / 1000;
            var UTCTime = FromUnixTime(unixEpochTime);
            return UTCTime;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        public static void updateProxyVotersWithProducerInfo(ref List<EOSVoter_row> voters)
        {

            //logger.Info("Correting proxy voter data");
            int proxyVoterCount = 0;
            // Loop through the full resultset and correct producer list on those that chose to vote via proxy
            foreach (var row in voters)
            {
                //If this voter voted by proxy
                if (!string.IsNullOrEmpty(row.proxy))
                {
                    proxyVoterCount++;
                    //Find the proxy that voted for this voter and link the same producers to this account. 
                    var proxyWhoVoted = voters.Find(x => x.owner.Equals(row.proxy));
                    row.producers = proxyWhoVoted.producers;
                }

            }

            logger.Info("{0} proxy votes updated", proxyVoterCount);

        }
    }
}
