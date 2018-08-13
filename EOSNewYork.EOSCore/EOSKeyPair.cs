using NLog;
using Cryptography.ECDSA;

namespace EOSNewYork.EOSCore
{
    public class EOSKeyPair
    {
        public string PrivateKey { get; set;}
        
        public string PublicKey { get; set; }

        public EOSKeyPair()
        {
            
        }
    }

}
