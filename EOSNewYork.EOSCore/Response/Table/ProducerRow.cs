using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore.Response.Table
{
    public class ProducerRow : IEOSTable
    {
        public string owner { get; set; }
        public string total_votes { get; set; }
        public string producer_key { get; set; }
        public bool is_active { get; set; }
        public long unpaid_blocks { get; set; }
        public string url { get; set; }

        public double total_votes_long
        {
            get
            {
                return double.Parse(total_votes);
            }
        }
        public EOSTableMetadata GetMetaData()
        {

            var meta = new EOSTableMetadata
            {
                primaryKey = "owner",
                contract = "eosio",
                scope = "eosio",
                table = "producers",
                key_type = "name"
            };

            return meta;
        }

    }
}
