using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public class EOSVoter_row : IEOSTable
    {
        public string owner;
        public string proxy;
        public Int64 staked;
        public double last_vote_weight;
        public double proxied_vote_weight;
        public List<string> producers;
        public double last_vote_weight_for_this_account_only
        {
            get
            {
                return last_vote_weight - proxied_vote_weight;
            }
        }
        public string voterDescription
        {
            get
            {
                string name = owner;
                if (!string.IsNullOrEmpty(proxy))
                    name = name + "(proxyvia:" + proxy + ")";
                return name;
            }
        }

        public EOSTableMetadata getMetadata()
        {
            var meta = new EOSTableMetadata
            {
                primaryKey = "owner",
                contract = "eosio",
                scope = "eosio",
                table = "voters"
            };
            return meta;
        }

    }
}
