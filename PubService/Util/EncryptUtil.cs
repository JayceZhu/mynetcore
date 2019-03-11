using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PubService
{
    public class EncryptUtil
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="data">输入参数</param>
        /// <returns></returns>
        public static string MD5Encrypt(string data)
        {
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            var inputBye = Encoding.UTF8.GetBytes(data);
            var outputBye = m5.ComputeHash(inputBye);

            var retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "");

            return retStr;
        }

        /// <summary>
        /// Sha1哈加密
        /// </summary>
        /// <param name="str">输入参数</param>
        /// <returns></returns>
        public static string Sha1Encrypt(String str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }
    }
}
