using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public class EOSGlobal_row : IEOSTable
    {
        public string total_producer_vote_weight { get; set; }

        public EOSTableMetadata getMetadata()
        {
            var meta = new EOSTableMetadata
            {
                primaryKey = "",
                contract = "eosio",
                scope = "eosio",
                table = "global"
            };
            return meta;
        }

    }
}
