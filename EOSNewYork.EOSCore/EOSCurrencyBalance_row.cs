using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore
{
    public class EOSCurrencyBalance_row : IEOAPI, IEOStringArray
    {
        public List<String> balances { get; set; }
        
        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/chain/get_currency_balance"
            };

            return meta;
        }

        public void setStringArray(List<String> array)
        {
            balances = array;
        }

        public class postData
        {
            public string account { get; set; }
            public string code { get; set; }
            public string symbol { get; set; }
        }
    }
}
