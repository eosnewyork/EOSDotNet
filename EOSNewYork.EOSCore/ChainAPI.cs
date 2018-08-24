using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptography.ECDSA;
using EOSNewYork.EOSCore.Params;
using EOSNewYork.EOSCore.Response.API;
using EOSNewYork.EOSCore.Serialization;
using EOSNewYork.EOSCore.Utilities;
using Action = EOSNewYork.EOSCore.Params.Action;
using EOSNewYork.EOSCore.Lib;

namespace EOSNewYork.EOSCore
{
    public class ChainAPI : BaseAPI
    {
        public ChainAPI(){}

        public ChainAPI(string host) : base(host) {}

        public async Task<AbiBinToJson> GetAbiBinToJsonAsync(string code, string action, string binargs)
        {
            return await new EOS_Object<AbiBinToJson>(HOST).GetObjectsFromAPIAsync(new AbiBinToJsonParam { code = code, action = action, binargs = binargs });
        }
        public AbiBinToJson GetAbiBinToJson(string code, string action, string binargs)
        {
            return GetAbiBinToJsonAsync(code, action, binargs).Result;
        }        
        public async Task<AbiJsonToBin> GetAbiJsonToBinAsync(string code, string action, object args)
        {
            return await new EOS_Object<AbiJsonToBin>(HOST).GetObjectsFromAPIAsync(new AbiJsonToBinParam { code = code, action = action, args = args });
        }
        public AbiJsonToBin GetAbiJsonToBin(string code, string action, object args)
        {
            return GetAbiJsonToBinAsync(code, action, args).Result;
        }        
        public async Task<Account> GetAccountAsync(string accountName)
        {
            return await new EOS_Object<Account>(HOST).GetObjectsFromAPIAsync(new AccountParam { account_name = accountName });
        }
        public Account GetAccount(string accountName)
        {
            return GetAccountAsync(accountName).Result;
        }        
        public async Task<Block> GetBlockAsync(string blockNumOrId)
        {
            return await new EOS_Object<Block>(HOST).GetObjectsFromAPIAsync(new BlockParam { block_num_or_id = blockNumOrId });
        }
        public Block GetBlock(string blockNumOrId)
        {
            return GetBlockAsync(blockNumOrId).Result;
        }
        public async Task<CurrencyBalance> GetCurrencyBalanceAsync(string account, string code, string symbol)
        {
            return await new EOS_Object<CurrencyBalance>(HOST).GetObjectsFromAPIAsync(new CurrencyBalanceParam { account = account, code = code, symbol = symbol });
        }
        public CurrencyBalance GetCurrencyBalance(string account, string code, string symbol)
        {
            return GetCurrencyBalanceAsync(account, code, symbol).Result;
        }
        public async Task<Info> GetInfoAsync()
        {
            return await new EOS_Object<Info>(HOST).GetObjectsFromAPIAsync();
        }
        public Info GetInfo()
        {
            return GetInfoAsync().Result;
        }
        public async Task<ProducerSchedule> GetProducerScheduleAsync()
        {
            return await new EOS_Object<ProducerSchedule>(HOST).GetObjectsFromAPIAsync();
        }
        public ProducerSchedule GetProducerSchedule()
        {
            return GetProducerScheduleAsync().Result;
        }
        public async Task<PushTransaction> PushTransactionAsync(Action[] actions, List<string> privateKeysInWIF)
        {
            //get info
            var info = await GetInfoAsync();
            
            //get head block
            var block = await GetBlockAsync(info.head_block_id);
            
            //prepare transaction object
            var transaction = new EOSNewYork.EOSCore.Params.Transaction {
                actions = actions,
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
            List<byte[]> privateKeys = new List<byte[]>();
            for(int i = 0; i < privateKeysInWIF.Count; i++)
            {
                privateKeys.Add(WifUtility.DecodePrivateWif(privateKeysInWIF[i]));
            }
            
            //get signatures for each provate key by signing message hash with private key
            string[] signatures = new string[privateKeys.Count];
            for(int i = 0; i< privateKeys.Count; i++)
            {
                signatures[i] = WifUtility.EncodeSignature(Secp256K1Manager.SignCompressedCompact(messageHash, privateKeys[i]));
            }

            //push transaction
            return await new EOS_Object<PushTransaction>(HOST).GetObjectsFromAPIAsync(new PushTransactionParam { packed_trx = Hex.ToString(packedTransaction), signatures = signatures, packed_context_free_data = string.Empty, compression = "none" });
        }
        public PushTransaction PushTransaction(Action[] actions, List<string> privateKeysInWIF)
        {
            return PushTransactionAsync(actions, privateKeysInWIF).Result;
        }
    }
}
