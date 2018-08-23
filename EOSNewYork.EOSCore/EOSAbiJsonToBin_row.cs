using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Params;
namespace EOSNewYork.EOSCore
{
    public class EOSAbiJsonToBin_row : IEOAPI
    {
        public string binargs { get; set; }
        
        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/chain/abi_json_to_bin"
            };

            return meta;
        }

        public class postData
        {
            public string code { get; set; }
            public string action { get; set; }
            public object args { get; set; }
        }
    }
}
