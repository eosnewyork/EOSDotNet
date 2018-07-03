using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public class EOSNamebids_row : IEOSTable
    {
        public string newname { get; set; }
        public string high_bidder { get; set; }
        public long high_bid { get; set; }
        public string last_bid_time { get; set; }
        public string last_bid_time_utc
        {
            get
            {
                return EOSUtil.FromUnixTime(long.Parse(last_bid_time) / 1000000).ToString();
            }
        }
        public EOSTableMetadata getMetadata()
        {

            var meta = new EOSTableMetadata
            {
                primaryKey = "newname",
                contract = "eosio",
                scope = "eosio",
                table = "namebids"
            };

            return meta;
        }

    }
}
