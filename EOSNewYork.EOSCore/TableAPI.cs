using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EOSNewYork.EOSCore.Response.Table;
using EOSNewYork.EOSCore.Lib;

namespace EOSNewYork.EOSCore
{
    public class TableAPI : BaseAPI
    {
        public TableAPI() {}

        public TableAPI(string host) : base(host) {}

        public async Task<List<GlobalRow>> GetGlobalRowsAsync()
        {
            return await new EOS_Table<GlobalRow>(HOST).GetRowsFromAPIAsync();
        }
        public List<GlobalRow> GetGlobalRows()
        {
            return GetGlobalRowsAsync().Result;
        }
        public async Task<List<NameBidsRow>> GetNameBidRowsAsync()
        {
            return await new EOS_Table<NameBidsRow>(HOST).GetRowsFromAPIAsync();
        }
        public List<NameBidsRow> GetNameBidRows()
        {
            return GetNameBidRowsAsync().Result;
        }
        public async Task<List<ProducerRow>> GetProducerRowsAsync()
        {
            return await new EOS_Table<ProducerRow>(HOST).GetRowsFromAPIAsync();
        }
        public List<ProducerRow> GetProducerRows()
        {
            return GetProducerRowsAsync().Result;
        }
        public async Task<List<VoterRow>> GetVoterRowsAsync()
        {
            return await new EOS_Table<VoterRow>(HOST).GetRowsFromAPIAsync();
        }
        public List<VoterRow> GetVoterRows()
        {
            return GetVoterRowsAsync().Result;
        }
    }
}
