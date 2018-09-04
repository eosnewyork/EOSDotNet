using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Params;
using EOSNewYork.EOSCore.Lib;
using EOSNewYork.EOSCore.Serialization;

namespace EOSNewYork.EOSCore.Response.API
{
    public class Actions : IEOAPI
    {
        public List<OrderedActionResult> actions { get; set; }
        public uint last_irreversible_block { get; set; }
        public bool? time_limit_exeeded_error { get; set; }

        public EOSAPIMetadata GetMetaData()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/history/get_actions"
            };

            return meta;
        }
    }
    public class OrderedActionResult
    {
        public ulong global_action_seq { get; set; }
        public int account_action_seq { get; set; }
        public uint block_num { get; set; }
        public string block_time { get; set; }
        public DateTime block_time_datetime
        {
            get
            {
                return DateTime.SpecifyKind((DateTime.Parse(block_time)), DateTimeKind.Utc);
            }
        }
        public JsonString action_trace { get; set; }
    }
}
