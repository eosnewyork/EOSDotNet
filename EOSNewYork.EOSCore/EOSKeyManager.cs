using NLog;
using Cryptography.ECDSA;
using Base58 = EOSNewYork.EOSCore.Utilities.Base58;

namespace EOSNewYork.EOSCore
{
    public class EOSKeyManager
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public static EOSKeyPair GenerateKeyPair()
        {
            EOSKeyPair keyPair = new EOSKeyPair();
            byte[] privateKey = Secp256K1Manager.GenerateRandomKey();
            keyPair.PrivateKey = Base58.EncodePrivateWif(privateKey);
            byte[] publicKey = Secp256K1Manager.GetPublicKey(privateKey, true);
            keyPair.PublicKey = Base58.EncodePublicWif(publicKey, "EOS");
            return keyPair;
        }
    }
}
