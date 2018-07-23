using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore
{
    public class EOS_Object<T> where T : IEOAPI
    {
        Uri _host;
        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public EOS_Object(Uri host)
        {
            var ObjType = (T)Activator.CreateInstance(typeof(T));
            var meta = ObjType.getMetadata();
            _host = new Uri(host, meta.uri);
        }
        
        public async Task<T> getAllObjectRecordsAsync()
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
