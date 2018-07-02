using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public class EOS_Object
    {
        Uri _host;
        Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public EOS_Object(Uri host)
        {
            _host = new Uri(host, "v1/chain/get_info");
        }

        public async Task<T> getAllObjectRecordsAsync<T>()
        {
            object result = null;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;

            response = await client.GetAsync(_host);
            var responseString = await response.Content.ReadAsStringAsync();
            T m = JsonConvert.DeserializeObject<T>(responseString);

            result = m;
            return (T)result;
        }

    }

}
