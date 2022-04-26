using System;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using EOSNewYork.EOSCore.Lib;

namespace EOSNewYork.EOSCore.Response.Trace
{
    public partial class Block : IEOAPI
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("previous_id")]
        public string PreviousId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("producer")]
        public string Producer { get; set; }

        [JsonProperty("transaction_mroot")]
        public string TransactionMroot { get; set; }

        [JsonProperty("action_mroot")]
        public string ActionMroot { get; set; }

        [JsonProperty("schedule_version")]
        public long ScheduleVersion { get; set; }

        [JsonProperty("transactions")]
        public List<Transaction> Transactions { get; set; }

        public EOSAPIMetadata GetMetaData()
        {
            var meta = new EOSAPIMetadata
            {
                uri = "/v1/trace_api/get_block"
            };

            return meta;
        }
    }

    public partial class Transaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("actions")]
        public List<Action> Actions { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cpu_usage_us")]
        public long CpuUsageUs { get; set; }

        [JsonProperty("net_usage_words")]
        public long NetUsageWords { get; set; }

        [JsonProperty("signatures")]
        public List<object> Signatures { get; set; }

        [JsonProperty("transaction_header")]
        public TransactionHeader TransactionHeader { get; set; }
    }

    public partial class Action
    {
        [JsonProperty("global_sequence")]
        public long GlobalSequence { get; set; }

        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("action")]
        public string ActionAction { get; set; }

        [JsonProperty("authorization")]
        public List<Authorization> Authorization { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public partial class Authorization
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }
    }

    public partial class TransactionHeader
    {
        [JsonProperty("expiration")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty("ref_block_num")]
        public long RefBlockNum { get; set; }

        [JsonProperty("ref_block_prefix")]
        public long RefBlockPrefix { get; set; }

        [JsonProperty("max_net_usage_words")]
        public long MaxNetUsageWords { get; set; }

        [JsonProperty("max_cpu_usage_ms")]
        public long MaxCpuUsageMs { get; set; }

        [JsonProperty("delay_sec")]
        public long DelaySec { get; set; }
    }
}
