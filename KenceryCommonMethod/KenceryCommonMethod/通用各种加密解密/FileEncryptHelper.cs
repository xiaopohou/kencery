// 源文件头信息：
// <copyright file="FileEncryptHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2015/1/13 12:12
// </copyright>

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 文件操作帮助类:提供对文件进行加密和解密
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2015/1/13</date>
    /// </auther>
    /// 修改记录：
    ///     1.
    /// </summary>
    public class FileEncryptHelper
    {
        public const string FileKey = "ihlih*0037JOHT*)(PIJY*(()JI^)IO%"; //加密密钥

        /// <summary>
        /// 对文件进行加密
        /// 调用:FileEncryptHelper.FileEncryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
        /// </summary>
        /// <param name="fileOriginalPath">需要加密的文件路径</param>
        /// <param name="fileFinshPath">加密完成后存放的文件路径</param>
        /// <param name="fileKey">文件密钥</param>
        public static void FileEncryptInfo(string fileOriginalPath, string fileFinshPath, string fileKey)
        {
            //分组加密算法的实现
            using (var fileStream = new FileStream(fileOriginalPath, FileMode.Open))
            {
                var buffer = new Byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length); //得到需要加密的字节数组
                //设置密钥，密钥向量，两个一样，都是16个字节byte
                var rDel = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(fileKey),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cryptoTransform = rDel.CreateEncryptor();
                byte[] cipherBytes = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
                using (var fileSEncrypt = new FileStream(fileFinshPath, FileMode.Create, FileAccess.Write))
                {
                    fileSEncrypt.Write(cipherBytes, 0, cipherBytes.Length);
                }
            }
        }

        /// <summary>
        /// 对文件进行解密
        /// 调用:FileEncryptHelper.FileDecryptInfo(Server.MapPath("~" +路径), Server.MapPath("~" +路径), FileHelper.FileEncrityKey)
        /// </summary>
        /// <param name="fileFinshPath">传递需要解密的文件路径</param>
        /// <param name="fileOriginalPath">解密后文件存放的路径</param>
        /// <param name="fileKey">密钥</param>
        public static void FileDecryptInfo(string fileFinshPath, string fileOriginalPath, string fileKey)
        {
            using (var fileStreamIn = new FileStream(fileFinshPath, FileMode.Open, FileAccess.Read))
            {
                using (var fileStreamOut = new FileStream(fileOriginalPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var rDel = new RijndaelManaged
                    {
                        Key = Encoding.UTF8.GetBytes(fileKey),
                        Mode = CipherMode.ECB,
                        Padding = PaddingMode.PKCS7
                    };
                    using (var cryptoStream = new CryptoStream(fileStreamOut, rDel.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        var bufferLen = 4096;
                        var buffer = new byte[bufferLen];
                        int bytesRead;
                        do
                        {
                            bytesRead = fileStreamIn.Read(buffer, 0, bufferLen);
                            cryptoStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }
            }
        }
    }
}