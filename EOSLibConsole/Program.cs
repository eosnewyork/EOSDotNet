using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;
using EOSNewYork.EOSCore;
using EOSNewYork.EOSCore.ActionArgs;
using EOSNewYork.EOSCore.Params;
using EOSNewYork.EOSCore.Response.API;
using EOSNewYork.EOSCore.Response.Table;
using EOSNewYork.EOSCore.Serialization;
using EOSNewYork.EOSCore.Utilities;
using Action = EOSNewYork.EOSCore.Params.Action;
using Newtonsoft.Json;
using Cryptography.ECDSA;
using System.Text.RegularExpressions;

namespace EOSLibConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //EOSInfo.GetGlobal();
            EOSInfo.GetRammarket();
            //EOSInfo.GetNameVotes();
            //EOSInfo.GetProducers();
            //EOSInfo.GetVoters();
            //EOSInfo.GetInfo();
            //EOSInfo.GetProduerSchedule();
            //EOSInfo.GetAccountInfo();
            //EOSInfo.GetAccountBalance();
            //EOSInfo.GetNewKeyPair();
            //EOSInfo.GetAbiJsonToBin();
            //EOSInfo.GetBlock();
            //EOSInfo.GetAbi();
            //EOSInfo.GetCode();
            //EOSInfo.GetRawCodeAndAbi();
            //EOSInfo.GetActions();
            //EOSInfo.GetTransaction();
            //EOSInfo.TestTransaction();
            //EOSInfo.GetTableRows();

            Console.WriteLine("Done");
            Console.ReadLine();

        }
    }

    public static class EOSInfo
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        //static string host = "http://dev.cryptolions.io:18888";
        static string host = "http://api.eosnewyork.io";
        static TableAPI tableAPI = new TableAPI(host);
        static ChainAPI chainAPI = new ChainAPI(host);
        static HistoryAPI historyAPI = new HistoryAPI(host);
        static string privateKeyWIF = "";
            
        public static void TestTransaction()
        {
            
            string _accountName = "yatendra1", _accountNameTo = "yatendra1234",_permissionName = "active", _code = "eosio.token", _action = "transfer", _memo = "";
            //prepare arguments to be passed to action
            TransferArgs _args = new TransferArgs(){ from = _accountName, to = _accountNameTo, quantity = "1.0000 EOS", memo = _memo };
            //BuyRamArgs _args = new BuyRamArgs(){ payer = _accountName, receiver = _accountName, quant = "0.001 EOS" };
            
            //prepare action object
            Action action = new ActionUtility(host).GetActionObject(_action, _accountName, _permissionName, _code, _args);
            
            List<string> privateKeysInWIF = new List<string> { privateKeyWIF };

            //push transaction
            var transactionResult = chainAPI.PushTransaction(new [] { action }, privateKeysInWIF);
            logger.Info(transactionResult.transaction_id);
        }
        public static void GetNewKeyPair()
        {
            var keypair = KeyManager.GenerateKeyPair();
            logger.Info("New keypair generated. Private key: {0}, Public key: {1}", keypair.PrivateKey, keypair.PublicKey);
            logger.Info(keypair.PrivateKey.Length);
            logger.Info(keypair.PublicKey.Length);
        }
        public static void GetBlock()
        {
            string blockNumber = "0107b7ae3c9845ecdbdb66afb8e2be2af3d515e0c01e8082c0cb17127610af49";
            var block = chainAPI.GetBlock(blockNumber);
            logger.Info("For block num {0} recieved block {1}", blockNumber, JsonConvert.SerializeObject(block));
        }
        public static void GetTableRows()
        {
            string code = "eosio", scope = "eosio", table = "global", json = "true";
            int lowerBound = 0, upperBound = -1, limit = 10;
            var tableRows = chainAPI.GetTableRows(scope, code, table, json, lowerBound, upperBound, limit);
            logger.Info("Recieved rows {0}", JsonConvert.SerializeObject(tableRows));
        }

        public static void GetActions()
        {
            string accountName = "eosnewyorkio";
            var actions = historyAPI.GetActions(-1, -10, accountName);
            //logger.Info("For account {0} recieved actions {1}", accountName, JsonConvert.SerializeObject(actions));

            // List all actions
            foreach (var action in actions.actions)
            {
                string singleLineData = Regex.Replace(action.action_trace.act.data.ToString(), @"\t|\n|\r", "");
                Console.WriteLine(string.Format("# {0}\t{1}\t{2} => {3}\t{4}", action.account_action_seq, action.block_time_datetime, action.action_trace.act.account+"::"+ action.action_trace.act.name, action.action_trace.receipt.receiver, singleLineData));
            }

            Console.WriteLine("----------");

            //List specific actions
            var res = actions.actions.Where(pr => pr.action_trace.act.name == "transfer");
            foreach (var action in res)
            {
                string singleLineData = Regex.Replace(action.action_trace.act.data.ToString(), @"\t|\n|\r", "");

                if (action.action_trace.act.data.from == "eosio.vpay" || action.action_trace.act.data.from == "eosio.bpay")
                    Console.WriteLine(string.Format("# {0}\t{1}\t{2} => {3}\t{4}", action.account_action_seq, action.block_time_datetime, action.action_trace.act.account + "::" + action.action_trace.act.name, action.action_trace.receipt.receiver, singleLineData));
            }


        }

        public static void GetAbi()
        {
            string accountName = "eosio";
            var abi = chainAPI.GetAbi(accountName);
            logger.Info("For account {0} recieved abi {1}", accountName, JsonConvert.SerializeObject(abi));
        }
        public static void GetTransaction()
        {
            string id = "ebe3435b22e302c6e3021b97756cdd900099eeac9060db3dbd1b116c7bbeee69";
            var transaction = historyAPI.GetTransaction(id, 11371727);
            logger.Info("For transaction id {0} recieved transaction {1}", id, JsonConvert.SerializeObject(transaction));
        }
        public static void GetCode()
        {
            string accountName = "eosio";
            var code = chainAPI.GetCode(accountName, true);
            logger.Info("For account {0} recieved code {1}", accountName, JsonConvert.SerializeObject(code));
        }
        public static void GetRawCodeAndAbi()
        {
            string accountName = "eosio";
            var rawCodeAndAbi = chainAPI.GetRawCodeAndAbi(accountName);
            logger.Info("For account {0} recieved code and abi {1}", accountName, JsonConvert.SerializeObject(rawCodeAndAbi));
        }
        public static void GetAbiJsonToBin()
        {
            string _code = "eosio.token", _action = "transfer", _memo = "";
            TransferArgs _args = new TransferArgs(){ from = "yatendra1", to = "yatendra1", quantity = "1.0000 EOS", memo = _memo };
            var abiJsonToBin = chainAPI.GetAbiJsonToBin(_code, _action, _args);
            logger.Info("For code {0}, action {1}, args {2} and memo {3} recieved bin {4}", _code, _action, _args, _memo, abiJsonToBin.binargs);
       
            var abiBinToJson = chainAPI.GetAbiBinToJson(_code, _action, abiJsonToBin.binargs);
            logger.Info("Received args json {0}", JsonConvert.SerializeObject(abiBinToJson.args));
        }
        public static void GetAccountBalance()
        {
            var currencyBalance = chainAPI.GetCurrencyBalance("yatendra1", "eosio.token", "EOS");
            logger.Info("The account had {0} balance records. The 1st (and probably the only balance) is {1}", currencyBalance.balances.Count, currencyBalance.balances.First());
        }


        public static void GetAccountInfo()
        {
            var account = chainAPI.GetAccount("yatendra1");
            logger.Info("{0} is currently the returned account name", account.account_name);
            string json = JsonConvert.SerializeObject(account);
            logger.Info("{0}", json);
        }


        public static void GetInfo()
        {
            var info = chainAPI.GetInfo();
            logger.Info("{0} is currently the head block producer", info.head_block_producer);
        }


        public static void GetProduerSchedule()
        {
            var producerSchedule = chainAPI.GetProducerSchedule();
            foreach (var producer in producerSchedule.active.producers)
            {
                logger.Info("{0}\t{1}", producer.producer_name, producer.block_signing_key);
            }
        }

        public static void GetGlobal()
        {
            var globalInfo = tableAPI.GetGlobalRows();

            foreach (var global in globalInfo)
            {
                logger.Debug("total_producer_vote_weight : {0}", global.total_producer_vote_weight);
            }
        }

        public static void GetRammarket()
        {
            var rammarketInfo = tableAPI.GetRammarketRows();

            foreach (var ramrow in rammarketInfo)
            {
                logger.Debug("supply : {0}", ramrow.supply);
                logger.Debug("base : {0}, {1}", ramrow.base_.balance_long, ramrow.base_.weight);
                logger.Debug("quote : {0}, {1}", ramrow.quote.balance_double, ramrow.quote.weight);
            }
        }

        public static void GetNameVotes()
        {
            StringBuilder tsvnamebids = new StringBuilder();
            var namebids = tableAPI.GetNameBidRows();

            foreach (var namebid in namebids)
            {
                tsvnamebids.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", namebid.newname, namebid.high_bid, namebid.high_bidder, namebid.last_bid_time, namebid.last_bid_time_utc));
            }

            File.WriteAllText("namebids.txt", tsvnamebids.ToString());
        }

        public static void GetProducers()
        {
            StringBuilder tsvproducers = new StringBuilder();

            var producers = tableAPI.GetProducerRows();
            foreach (var _producer in producers)
            {
                string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", _producer.owner, _producer.total_votes, _producer.is_active, _producer.unpaid_blocks, _producer.url, _producer.last_claim_time_DateTime);
                tsvproducers.AppendLine(line);
            }
            logger.Debug("Write {0} records to disk", producers.Count);
            File.WriteAllText("producerReport.txt", tsvproducers.ToString());

            IEnumerable<ProducerRow> query = producers.OrderByDescending(producer => producer.total_votes_long);

            StringBuilder tsvTop21producers = new StringBuilder();
            StringBuilder tsvTop21producersNameOnly = new StringBuilder();
            int countTop21 = 0;
            foreach (var _producer in query)
            {
                countTop21++;
                tsvTop21producers.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", _producer.owner, _producer.total_votes, _producer.is_active, _producer.unpaid_blocks, _producer.url));
                tsvTop21producersNameOnly.AppendLine(string.Format("{0}", _producer.owner));
                if (countTop21 == 21)
                    break;
            }

            File.WriteAllText("Top21ProducerReport.txt", tsvTop21producers.ToString());
            File.WriteAllText("Top21ProducerReportNameOnly.txt", tsvTop21producersNameOnly.ToString());
        }

        public static void GetVoters()
        {
            StringBuilder tsvoutput = new StringBuilder();

            var voters = tableAPI.GetVoterRows();
            EOSUtility.UpdateProxyVotersWithProducerInfo(ref voters);

            int voted = 0;
            int producerMatchCount = 0;
            long producerstake = 0;
            long totalstake = 0;
            foreach (var voter in voters)
            {
                if (voter.producers.Count > 0)
                {
                    voted++;
                    totalstake = totalstake + voter.staked;
                    foreach (var producervote in voter.producers)
                    {
                        producerMatchCount++;
                        producerstake = producerstake + voter.staked;
                        tsvoutput.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", producervote, voter.voterDescription, voter.staked / 10000, voter.last_vote_weight_for_this_account_only, voter.last_vote_weight, voter.proxied_vote_weight, String.Join(",", voter.producers.Select(x => x.ToString()).ToArray())));
                    }
                }
            }


            Console.WriteLine(voters.Count + " Records returned");
            if (voters.Count > 0)
            {
                Console.WriteLine(voters[0].owner + " = 1st record");
                Console.WriteLine(voted + " have voted");
                Console.WriteLine(totalstake + " tokens staked ");
            }

            File.WriteAllText("report.txt", tsvoutput.ToString());
        }

    }
}
