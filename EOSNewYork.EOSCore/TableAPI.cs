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
            return await GetTableRowsAsync<GlobalRow>();
        }
        public List<GlobalRow> GetGlobalRows()
        {
            return GetGlobalRowsAsync().Result;
        }
        public async Task<List<NameBidsRow>> GetNameBidRowsAsync()
        {
            return await GetTableRowsAsync<NameBidsRow>();
        }
        public List<NameBidsRow> GetNameBidRows()
        {
            return GetNameBidRowsAsync().Result;
        }
        public async Task<List<ProducerRow>> GetProducerRowsAsync()
        {
            return await GetTableRowsAsync<ProducerRow>();
        }
        public List<ProducerRow> GetProducerRows()
        {
            return GetProducerRowsAsync().Result;
        }
        public async Task<List<VoterRow>> GetVoterRowsAsync()
        {
            return await GetTableRowsAsync<VoterRow>();
        }
        public List<VoterRow> GetVoterRows()
        {
            return GetVoterRowsAsync().Result;
        }
        public async Task<List<T>> GetTableRowsAsync<T>() where T : IEOSTable
        {
            return await new EOS_Table<T>(HOST).GetRowsFromAPIAsync();
        }
        public List<T> GetTableRows<T>() where T : IEOSTable
        {
            return GetTableRowsAsync<T>().Result;
        }

        public async Task<List<RammarketRow>> GetRammarketRowsAsync()
        {
            return await GetTableRowsAsync<RammarketRow>();
        }
        public List<RammarketRow> GetRammarketRows()
        {
            return GetRammarketRowsAsync().Result;
        }

    }
}
