using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GST.KPI.Utils
{
    public static class DESEncrypt
    {
        private static string KEY;

        static DESEncrypt()
        {
            KEY = AppConfig.DESEncryptKey;
        }

        public static string Encrypt(string str)
        {
            return Encrypt(str, KEY);
        }

        public static string Decrypt(string str)
        {
            return Decrypt(str, KEY);
        }

        private static string Encrypt(string str, string sKey)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            des.Key = ASCIIEncoding.ASCII.GetBytes(generateKey(sKey));
            des.IV = ASCIIEncoding.ASCII.GetBytes(generateKey(sKey));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var retB = Convert.ToBase64String(ms.ToArray());
            return retB;
        }

        private static string generateKey(string sKey)
        {
            MD5 md5 = MD5.Create();
            byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(sKey));
            var keyStr = BitConverter.ToString(key);
            keyStr = keyStr.Replace("-", "");
            return keyStr.Substring(0, 8);
        }
        private static string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(generateKey(sKey));
            des.IV = ASCIIEncoding.ASCII.GetBytes(generateKey(sKey));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
