using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOSLib
{
    public class EOSInfo_row
    {
        public string server_version;
        public string chain_id;
        public Int64 head_block_num;
        public Int64 last_irreversible_block_num;
        public string last_irreversible_block_id;
        public string head_block_id;
        public string head_block_time;
        public string head_block_producer;
        public Int64 virtual_block_cpu_limit;
        public Int64 virtual_block_net_limit;
        public Int64 block_cpu_limit;
        public Int64 block_net_limit;
    }
}
