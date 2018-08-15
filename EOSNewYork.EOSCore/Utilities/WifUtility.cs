using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cryptography.ECDSA;

namespace EOSNewYork.EOSCore.Utilities
{
    public class WifUtility : Base58
    {
        public static string GetPrivateWif(byte[] source)
        {
            var updatedSource = AddFirstBytes(source, 1);
            updatedSource[0] = 0x80;
            var doubleHash = DoubleHash(updatedSource);
            updatedSource = AddLastBytes(updatedSource, CheckSumSizeInBytes);
            Array.Copy(doubleHash, 0, updatedSource, updatedSource.Length - CheckSumSizeInBytes, CheckSumSizeInBytes);
            return Encode(updatedSource);
        }
        public static string GetPublicWif(byte[] publicKey, string prefix)
        {
            var hash = Ripemd160Manager.GetHash(publicKey);
            var updatedPublicKey = AddLastBytes(publicKey, CheckSumSizeInBytes);
            Array.Copy(hash, 0, updatedPublicKey, updatedPublicKey.Length - CheckSumSizeInBytes, CheckSumSizeInBytes);
            var encodedHash = Encode(updatedPublicKey);
            return prefix + encodedHash;
        }
    }
}