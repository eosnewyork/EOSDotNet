using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSNewYork.EOSCore
{
    public class EOSProducerSchedule_row : IEOAPI
    {
        public EOSProducerSchedule active { get; set; }
        public EOSProducerSchedule pending { get; set; }
        public EOSProducerSchedule proposed { get; set; }

        public EOSAPIMetadata getMetadata()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "v1/chain/get_producer_schedule"
            };

            return meta;
        }
    }

    public class EOSProducerSchedule
    {
        public int version;
        public List<EOSProducerSchedule_ProducerInfo> producers;
    }

    public class EOSProducerSchedule_ProducerInfo
    {
        public string producer_name;
        public string block_signing_key;
    }


}
