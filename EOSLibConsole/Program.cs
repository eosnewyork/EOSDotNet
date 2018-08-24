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
            EOSInfo.GetBlock();
            //EOSInfo.TestTransaction();

            Console.WriteLine("Done");
            //Console.ReadLine();

        }
    }

    public static class EOSInfo
    {
        static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static Uri HOST = new Uri("http://dev.cryptolions.io:18888");
        static Uri pennStationHOST = new Uri("http://pennstation.eosdocs.io:7001");
        static string privateKeyWIF = "";
            
        public static void TestTransaction()
        {
            string _accountName = "yatendra1", _permissionName = "active", _code = "eosio.token", _action = "transfer", _memo = "";
            //prepare action object
            Action action = new Action(){ 
                account = new AccountName(_accountName),
                name = new ActionName(_action),
                authorization = new[]{
                    new EOSNewYork.EOSCore.Params.Authorization{
                        actor = new AccountName(_accountName),
                        permission = new PermissionName(_permissionName)
                    }
                }
            };
            
            //prepare arguments to be passed to action
            TransferArgs _args = new TransferArgs(){ from = _accountName, to = _accountName, quantity = "1 EOS", memo = _memo };
            //BuyRamArgs _args = new BuyRamArgs(){ payer = _accountName, receiver = _accountName, quant = "0.001 EOS" };
            
            //convert action arguments to binary and save it in action.data
            var abiJsonToBin = new EOS_Object<AbiJsonToBin>(HOST).GetObjectsFromAPIAsync(new AbiJsonToBinParam { code = _code, action = _action, args = _args }).Result;
            action.data = new BinaryString(abiJsonToBin.binargs);
            
            //get info
            var info = new EOS_Object<Info>(HOST).GetObjectsFromAPIAsync().Result;
            
            //get head block
            var block = new EOS_Object<Block>(HOST).GetObjectsFromAPIAsync(new BlockParam { block_num_or_id = info.head_block_id }).Result;
            
            //prepare transaction object
            var transaction = new EOSNewYork.EOSCore.Params.Transaction {
                actions = new [] { action },
                ref_block_num = (ushort)(block.block_num & 0xffff),
                ref_block_prefix = block.ref_block_prefix,
                expiration = new TimePointSec(block.timestamp_datetime.AddSeconds(120))
            };
            
            //pack the transaction
            var packedTransaction = new PackingSerializer().Serialize<EOSNewYork.EOSCore.Params.Transaction>(transaction);
            
            //get chain id
            var chainId = Hex.HexToBytes(info.chain_id);
            
            //combine chainId, packed transaction and 32 empty bytes
            var message = new byte[chainId.Length + packedTransaction.Length + 32];
            Array.Copy(chainId, message, chainId.Length);
            Array.Copy(packedTransaction, 0, message, chainId.Length, packedTransaction.Length);
            
            //calculate message hash
            var messageHash = Sha256Manager.GetHash(message);

            //get private keys in WIF format
            List<byte[]> privateKeys = new List<byte[]> { WifUtility.DecodePrivateWif(privateKeyWIF) };
            
            //get signatures for each provate key by signing message hash with private key
            string[] signatures = new string[privateKeys.Count];
            for(int i = 0; i< privateKeys.Count; i++)
            {
                signatures[i] = WifUtility.EncodeSignature(Secp256K1Manager.SignCompressedCompact(messageHash, privateKeys[i]));
            }

            //push transaction
            var transactionResult = new EOS_Object<PushTransaction>(HOST).GetObjectsFromAPIAsync(new PushTransactionParam { packed_trx = Hex.ToString(packedTransaction), signatures = signatures, packed_context_free_data = string.Empty, compression = "none" }).Result;
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
            var block = new EOS_Object<Block>(HOST).GetObjectsFromAPIAsync(new BlockParam { block_num_or_id = blockNumber }).Result;
            logger.Info("Block recieved for block num {0}", block.block_num);
        }
        public static void GetAbiJsonToBin()
        {
            string _code = "eosio.token", _action = "transfer", _memo = "";
            TransferArgs _args = new TransferArgs(){ from = "yatendra1", to = "yatendra1", quantity = "1 EOS", memo = _memo };
            var abiJsonToBin = new EOS_Object<AbiJsonToBin>(HOST).GetObjectsFromAPIAsync(new AbiJsonToBinParam { code = _code, action = _action, args = _args }).Result;
            logger.Info("For code {0}, action {1}, args {2} and memo {3} recieved bin {4}", _code, _action, _args, _memo, abiJsonToBin.binargs);
       
            var abiBinToJson = new EOS_Object<AbiBinToJson>(HOST).GetObjectsFromAPIAsync(new AbiBinToJsonParam { code = _code, action = _action, binargs = abiJsonToBin.binargs }).Result;
            logger.Info("Received args json {0}", JsonConvert.SerializeObject(abiBinToJson.args));
        }
        public static void GetAccountBalance()
        {
            var currencyBalance = new EOS_StringArray<CurrencyBalance>(HOST).GetObjectsFromAPIAsync(new CurrencyBalanceParam { account = "yatendra1", code = "eosio.token", symbol = "EOS" }).Result;
            logger.Info("The account had {0} balance records. The 1st (and probably the only balance) is {1}", currencyBalance.balances.Count, currencyBalance.balances.First());
        }


        public static void GetAccountInfo()
        {
            var account = new EOS_Object<Account>(HOST).GetObjectsFromAPIAsync(new AccountParam { account_name = "yatendra1" }).Result;
            logger.Info("{0} is currently the returned account name", account.account_name);
            string json = JsonConvert.SerializeObject(account);
            logger.Info("{0}", json);
        }


        public static void GetInfo()
        {
            var info = new EOS_Object<Info>(HOST).GetObjectsFromAPIAsync().Result;
            logger.Info("{0} is currently the head block producer", info.head_block_producer);
        }

        public static void GetProduerSchedule()
        {
            var info = new EOS_Object<ProducerSchedule>(HOST).GetObjectsFromAPIAsync().Result;
            foreach (var producer in info.active.producers)
            {
                logger.Info("{0}\t{1}", producer.producer_name, producer.block_signing_key);
            }
        }

        public static void GetGlobal()
        {
            var globalInfo = new EOS_Table<GlobalRow>(HOST).GetRowsFromAPIAsync().Result;

            foreach (var global in globalInfo)
            {
                logger.Debug("total_producer_vote_weight : {0}", global.total_producer_vote_weight);
            }
        }

        public static void GetNameVotes()
        {
            StringBuilder tsvnamebids = new StringBuilder();
            var namebids = new EOS_Table<NameBidsRow>(HOST).GetRowsFromAPIAsync().Result;

            foreach (var namebid in namebids)
            {
                tsvnamebids.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", namebid.newname, namebid.high_bid, namebid.high_bidder, namebid.last_bid_time, namebid.last_bid_time_utc));
            }

            File.WriteAllText("namebids.txt", tsvnamebids.ToString());
        }

        public static void GetProducers()
        {
            StringBuilder tsvproducers = new StringBuilder();

            var producers = new EOS_Table<ProducerRow>(HOST).GetRowsFromAPIAsync().Result;
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

            var voters = new EOS_Table<VoterRow>(HOST).GetRowsFromAPIAsync().Result;
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
