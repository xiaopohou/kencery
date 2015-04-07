// 源文件头信息：
// <copyright file="EncryptInfo.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2015/1/12 10:12
// </copyright>

using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 通用工具操作类[加密，解密],对字符串的操作
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2015/1/12</date>
    /// </auther>
    /// 修改记录：
    ///     1.
    /// </summary>
    public class EncryptHelper
    {
        #region-------------------------对称加密(向量)---------------------

        private SymmetricAlgorithm symmetricAlgorithm; //表示对称算法的实现都必须从中集成的抽象基类
        private readonly string _key;

        //加密密钥
        public const string SKey = "ihlih*0037JOHT*)(PIJY*(()JI^)IO%";

        public EncryptHelper()
        {
            symmetricAlgorithm = new RijndaelManaged();
            _key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7"; //自定义密钥
        }

        /// <summary>
        /// 获得密钥信息—得到密钥的信息
        /// </summary>
        /// <returns>返回密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = _key;
            symmetricAlgorithm.GenerateKey(); //当在派生类中重写时，生成用于该算法的随机密钥
            byte[] bytTemp = symmetricAlgorithm.Key; //获取或设置对称算法的密钥
            int keyLength = bytTemp.Length;
            sTemp = sTemp.Length > keyLength ? sTemp.Substring(0, keyLength) : sTemp.PadRight(keyLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始化向量IV—得到初始化向量IV的信息
        /// </summary>
        /// <returns></returns>
        private byte[] GetLegalIv()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            symmetricAlgorithm.GenerateIV();
            byte[] byteTemp = symmetricAlgorithm.IV; //获取或设置对称算法的初始化向量
            int ivLength = byteTemp.Length; //获得一个32位整数，表示System.Array的所有维数中元素的总数
            sTemp = sTemp.Length > ivLength ? sTemp.Substring(0, ivLength) : sTemp.PadRight(ivLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 向量对称加密方法(请先实例化Encrypt类)
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <returns>返回经过加密的字符串</returns>
        public string EncryptStrByIvInfo(string source)
        {
            byte[] byteIn = Encoding.UTF8.GetBytes(source); //将一个字符串转换成字节
            var memoryStream = new MemoryStream();
            symmetricAlgorithm.Key = GetLegalKey();
            symmetricAlgorithm.IV = GetLegalIv();
            ICryptoTransform iCryptoTransform = symmetricAlgorithm.CreateEncryptor();
            var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(byteIn, 0, byteIn.Length);
            cryptoStream.FlushFinalBlock(); //用缓冲区的当前状态更新基础数据源或储存库，随后清除缓冲区
            memoryStream.Close();
            byte[] byteOut = memoryStream.ToArray();
            return Convert.ToBase64String(byteOut);
        }

        /// <summary>
        /// 向量对称解密方法(请先实例化Encrypt类)
        /// </summary>
        /// <param name="source">已加密的字符串</param>
        /// <returns>返回解密后的字符串信息</returns>
        public string DecryptStrByIvInfo(string source)
        {
            byte[] byteIn = Convert.FromBase64String(source);
            var memoryStream = new MemoryStream(byteIn, 0, byteIn.Length);
            symmetricAlgorithm.Key = GetLegalKey();
            symmetricAlgorithm.IV = GetLegalIv();
            ICryptoTransform iCryptoTransform = symmetricAlgorithm.CreateDecryptor();
            var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        #endregion

        #region-------------------------DES加密和解密----------------------

        /// <summary>
        /// Des实现加密
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <param name="sKey">加密密钥</param>
        /// <returns>返回加密后的字符串</returns>
        public static string DesEncryptInfo(string source, string sKey)
        {
            var desCrypto = new DESCryptoServiceProvider(); //初始化对象
            byte[] byteInputByteArray = Encoding.Default.GetBytes(source);

            desCrypto.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                .Substring(0, 8))
            ;
            desCrypto.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                .Substring(0, 8));
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, desCrypto.CreateEncryptor(), CryptoStreamMode.Write);
            //目标数据流转化为新实例
            cryptoStream.Write(byteInputByteArray, 0, byteInputByteArray.Length);
            cryptoStream.FlushFinalBlock();

            var stringBuilder = new StringBuilder();
            foreach (var memoryS in memoryStream.ToArray())
            {
                stringBuilder.AppendFormat("{0:X2}", memoryS);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Des实现解密
        /// </summary>
        /// <param name="source">待解密的字符串信息</param>
        /// <param name="sKey">解密密钥</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DesDecryptInfo(string source, string sKey)
        {
            var desCrypto = new DESCryptoServiceProvider(); //初始化对象
            int length = source.Length/2;
            byte[] byteInputByteArray = new byte[length];

            //循环数据流
            int i;
            for (int x = 0; x < length; x++)
            {
                i = Convert.ToInt32(source.Substring(x*2, 2), 16);
                byteInputByteArray[x] = (byte) i;
            }
            //组织返回解密后的字符串
            desCrypto.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                .Substring(0, 8));
            desCrypto.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5")
                .Substring(0, 8));
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, desCrypto.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(byteInputByteArray, 0, byteInputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(memoryStream.ToArray());
        }

        #endregion

        #region-------------------------MD5加密----------------------------

        /// <summary>
        /// 实现Md5加密算法，对传递进来的字符串进行加密然后可以获取加密后的内容（MD5CryptoServiceProvider）
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <returns>返回加密后的字符串信息</returns>
        public static string Md5EncryptInfo(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider(); //实例化MD5加密对象
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(source));
            return Encoding.Default.GetString(result);
        }

        /// <summary>
        /// 实现Md5加密算法，对传递进来的字符串进行加密然后可以获取加密后的内容（HashPasswordForStoringInConfigFile）
        /// 方法调用：EncryptHelper.Md5EncryptInfo("字符串",EncryptHelper.SKey);
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <param name="sKey">加密密钥</param>
        /// <returns>返回加密后的字符串</returns>
        public static string Md5EncryptInfo(string source, string sKey)
        {
            string tempStr = FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5");
            //返回加密后统一转换成小写展示
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source + tempStr, "md5").ToLower();
        }

        #endregion

        #region-------------------------倒序加1加密解密--------------------

        /// <summary>
        /// 倒序加1加密实现
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string OrderEncryptStrInfo(string source)
        {
            byte[] bytes = new byte[source.Length];
            for (int i = 0; i <= source.Length - 1; i++)
            {
                bytes[i] = (byte) ((byte) source[i] + 1);
            }
            source = "";
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                source += ((char) bytes[i]).ToString(CultureInfo.InvariantCulture);
            }
            return source;
        }

        /// <summary>
        /// 顺序减1解密实现
        /// </summary>
        /// <param name="source">待解密的字符串</param>
        /// <returns>返回解密后的字符串信息</returns>
        public static string OrderDecryptStrInfo(string source)
        {
            byte[] bytes = new byte[source.Length];
            for (int i = 0; i <= source.Length - 1; i++)
            {
                bytes[i] = (byte) ((byte) source[i] - 1);
            }
            source = "";
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                source += ((char) bytes[i]).ToString(CultureInfo.InvariantCulture);
            }
            return source;
        }

        #endregion

        #region-------------------------Escape加密解密,处理全角字符有问题--

        /// <summary>
        /// Escape实现加密
        /// </summary>
        /// <param name="source">待加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string EscapeEncryptInfo(string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            int length = source.Length;
            for (int i = 0; i < length; i++)
            {
                char chars = source[i];
                if (Char.IsLetterOrDigit(chars) || chars == '-' || chars == '_' || chars == '/' || chars == '\\' ||
                    chars == '.')
                {
                    stringBuilder.Append(chars);
                }
                else
                {
                    stringBuilder.Append(Uri.HexEscape(chars));
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///  Escape实现解密
        /// </summary>
        /// <param name="source">待解密的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public static string EscapeDecryptInfo(string source)
        {
            if (source == null)
            {
                return String.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            int length = source.Length;
            int i = 0;
            while (i != length)
            {
                stringBuilder.Append(Uri.IsHexEncoding(source, i) ? Uri.HexUnescape(source, ref i) : source[i++]);
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region-------------------------Base64编码解码---------------------

        /// <summary>
        /// Base64实现编码
        /// </summary>
        /// <param name="code_type">编码类型</param>
        /// <param name="code">待编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string Base64EncodeInfo(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        ///  Base64实现解码
        /// </summary>
        /// <param name="code_type">编码类型</param>
        /// <param name="code">编码后的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string Base64DecodeInfo(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        #endregion
    }
}