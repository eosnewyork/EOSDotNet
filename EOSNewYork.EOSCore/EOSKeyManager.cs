using NLog;
using Cryptography.ECDSA;
using EOSNewYork.EOSCore.Utilities;

namespace EOSNewYork.EOSCore
{
    public class EOSKeyManager
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public static EOSKeyPair GenerateKeyPair()
        {
            EOSKeyPair keyPair = new EOSKeyPair();
            byte[] privateKey = Secp256K1Manager.GenerateRandomKey();
            keyPair.PrivateKey = WifUtility.GetPrivateWif(privateKey);
            byte[] publicKey = Secp256K1Manager.GetPublicKey(privateKey, true);
            keyPair.PublicKey = WifUtility.GetPublicWif(publicKey, "EOS");
            return keyPair;
        }
    }
}
