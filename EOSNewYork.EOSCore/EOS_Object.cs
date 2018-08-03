using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore
{
    public class EOS_Object<T> where T : IEOAPI
    {
        // Best to use a global HTTP Client
        // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient Client = new HttpClient();
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

            //HttpClient client = new HttpClient();
            HttpResponseMessage response = null;

            logger.Debug("HTTP GET: {0}", _host);

            response = await Client.GetAsync(_host);
            var responseString = await response.Content.ReadAsStringAsync();
            T m = JsonConvert.DeserializeObject<T>(responseString);

            result = m;
            return (T)result;
        }

        public async Task<T> getAllObjectRecordsAsync(object postData)
        {
            
            object result = null;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;

            string json = JsonConvert.SerializeObject(postData);
            StringContent x = new StringContent(json);

            logger.Debug("HTTP POST: {0}, DATA: {1}", _host, json);

            response = await client.PostAsync(_host,x);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("API Call did not respond with 200 OK");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            T m = JsonConvert.DeserializeObject<T>(responseString);

            result = m;
            return (T)result;
        }

    }

}
