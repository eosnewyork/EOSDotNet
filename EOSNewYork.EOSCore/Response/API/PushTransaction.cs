using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Params;

namespace EOSNewYork.EOSCore.Response.API
{
    public class PushTransaction : IEOAPI
    {
        public string transaction_id { get; set; }
        
        public EOSAPIMetadata GetMetaData()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/chain/push_transaction"
            };

            return meta;
        }
    }
}
