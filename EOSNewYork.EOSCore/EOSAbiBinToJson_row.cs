using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Params;
namespace EOSNewYork.EOSCore
{
    public class EOSAbiBinToJson_row : IEOAPI
    {
        public AbiJsonToBinArgs args { get; set; }
        
        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/chain/abi_bin_to_json"
            };

            return meta;
        }

        public class postData
        {
            public string code { get; set; }
            public string action { get; set; }
            public string binargs { get; set; }
        }
    }
}
