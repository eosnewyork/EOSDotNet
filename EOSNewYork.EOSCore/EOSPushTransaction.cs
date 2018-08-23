using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Params;
namespace EOSNewYork.EOSCore
{
    public class EOSPushTransaction : IEOAPI
    {
        public string transaction_id { get; set; }
        
        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/chain/push_transaction"
            };

            return meta;
        }

        public class postData
        {
            public string packed_trx { get; set; }
            public string packed_context_free_data { get; set; }
            public string compression { get; set; }
            public string[] signatures { get; set; }
        }
    }
}
