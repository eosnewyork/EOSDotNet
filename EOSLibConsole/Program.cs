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

namespace EOSLibConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //EOSInfo.GetGlobal();
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
            EOSInfo.GetTransaction();
            //EOSInfo.TestTransaction();

            Console.WriteLine("Done");
            //Console.ReadLine();

        }
    }

    public static class EOSInfo
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static string host = "http://dev.cryptolions.io:18888";
        static TableAPI tableAPI = new TableAPI(host);
        static ChainAPI chainAPI = new ChainAPI(host);
        static HistoryAPI historyAPI = new HistoryAPI(host);
        static string privateKeyWIF = "5KENzJxJ6CwnKgi3TZ4sS9Fe6D6JqmZ85JWWtU8H7xNDT6JcbtL";
            
        public static void TestTransaction()
        {
            
            string _accountName = "yatendra1", _permissionName = "active", _code = "eosio.token", _action = "transfer", _memo = "";
            //prepare arguments to be passed to action
            TransferArgs _args = new TransferArgs(){ from = _accountName, to = _accountName, quantity = "1 EOS", memo = _memo };
            //BuyRamArgs _args = new BuyRamArgs(){ payer = _accountName, receiver = _accountName, quant = "0.001 EOS" };
            
            //prepare action object
            Action action = new ActionUtility(host).GetActionObject(_accountName, _action, _permissionName, _code, _args);
            
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
            string blockNumber = "100";
            var block = chainAPI.GetBlock(blockNumber);
            logger.Info("Block recieved for block num {0}", block.block_num);
        }
        public static void GetActions()
        {
            string accountName = "eosio";
            var actions = historyAPI.GetActions(-1, 100, accountName);
            logger.Info("For account {0} recieved actions {1}", accountName, JsonConvert.SerializeObject(actions));
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
            TransferArgs _args = new TransferArgs(){ from = "yatendra1", to = "yatendra1", quantity = "1 EOS", memo = _memo };
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
            var info = chainAPI.GetProducerSchedule();
            foreach (var producer in info.active.producers)
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
                string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", _producer.owner, _producer.total_votes, _producer.is_active, _producer.unpaid_blocks, _producer.url);
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
