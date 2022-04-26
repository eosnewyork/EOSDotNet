using System.Threading.Tasks;

using EOSNewYork.EOSCore.Params;
using EOSNewYork.EOSCore.Response.Trace;
using EOSNewYork.EOSCore.Lib;

namespace EOSNewYork.EOSCore
{
    public class TraceAPI : BaseAPI
    {
        public TraceAPI() {}
        public TraceAPI(string host) : base(host) {}

        public async Task<Block> GetBlockAsync(uint blockNumber)
        {
            return await new EOS_Object<Block>(HOST).GetObjectsFromAPIAsync(new TraceBlockParam { block_num = blockNumber });
        }
        public Block GetBlock(uint blockNumber)
        {
            return GetBlockAsync(blockNumber).Result;
        }
    }
}
