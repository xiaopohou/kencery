// 源文件头信息：
// <copyright file="ZipHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/12/15
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ZipFile = Ionic.Zip.ZipFile;
using ICSharpCode.SharpZipLib.Zip;

namespace KenceryCommonMethod
{
    /// <summary>
    /// Zip压缩文件的实现
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/12/15</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.  
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// 压缩文件(传递参数源文件路径和压缩后的文件路径)
        /// </summary>
        /// <param name="srcPath">源文件全路径</param>
        /// <param name="destPath">保存为Zip包的目录</param>
        public static void ZipFileInfo(string srcPath, string destPath)
        {
            using (var zip = new ZipFile())
            {
                Ionic.Zip.ZipEntry zipEntry = zip.AddFile(srcPath, @"\");
                zipEntry.Comment = "Added by Cheeso's CreateZip utility.";
                zip.Comment =
                    string.Format("This zip archive was created by the CreateZip example application on machine '{0}'",
                        Dns.GetHostName());
                zip.Save(destPath);
            }
        }

        /// <summary>
        /// 压缩目录下的所有文件
        /// </summary>
        /// <param name="filePath">源文件根目录</param>
        /// <param name="savePath">保存为Zip包的目录</param>
        public static void ZipDirFileInfo(string filePath, string savePath)
        {
            using (var zip = new ZipFile())
            {
                //读取文件夹下面的所有文件
                string[] fileNames = Directory.GetFiles(filePath);
                foreach (string fileName in fileNames)
                {
                    Ionic.Zip.ZipEntry zipEntry = zip.AddFile(fileName, @"\");
                    zipEntry.Comment = "Added by Cheeso's CreateZip utility.";
                }
                zip.Comment = string.Format(
                    "This zip archive was created by the CreateZip example application on machine '{0}'",
                    Dns.GetHostName());
                zip.Save(savePath);
            }
        }

        /// <summary>
        /// 压缩文件夹，文件架下面包含文件夹
        /// </summary>
        /// <param name="filePath">源文件夹路径</param>
        /// <param name="savePath">保存为Zip包的目录</param>
        public static void ZipDirInfo(string filePath, string savePath)
        {
            using (var zip = new ZipFile())
            {
                zip.StatusMessageTextWriter = Console.Out;
                zip.AddDirectory(filePath, @"\");
                zip.Save(savePath);
            }
        }

        /// <summary>
        /// 将一个压缩文件集合返回压缩后的字节数
        /// </summary>
        /// <param name="filesPath">源文件集合</param>
        /// <returns></returns>
        public static byte[] ZipFilesInfo(IEnumerable<string> filesPath)
        {
            using (var zipFile = new ZipFile())
            {
                foreach (var filePath in filesPath)
                {
                    Ionic.Zip.ZipEntry zipEntry = zipFile.AddFile(filePath, @"\");
                }
                zipFile.Comment =
                    string.Format("This zip archive was created by the CreateZip example application on machine '{0}'",
                        Dns.GetHostName());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zipFile.Save(memoryStream);
                    return memoryStream.ToArray(); //将流内容写入字节数组
                }
            }
        }

        /// <summary>
        /// 压缩一个文件返回流信息
        /// </summary>
        /// <param name="filePath">源文件服务器路径</param>
        /// <param name="fileName">压缩文件名</param>
        /// <returns></returns>
        public static byte[] ZipFileOneInfo(string filePath, string fileName)
        {
            using (var zipFile = new ZipFile(Encoding.UTF8))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    zipFile.AddEntry(fileName, fileStream);
                    using (var memoryStream = new MemoryStream())
                    {
                        zipFile.Save(memoryStream);
                        return memoryStream.ToArray();
                    }

                }
            }
        }

        /// <summary>
        /// 解压缩(将压缩的文件解压到制定的文件夹下面)—解压到指定文件夹下面
        /// </summary>
        /// <param name="zipFilePath">源压缩的文件路径</param>
        /// <param name="savePath">解压后保存文件到指定的文件夹</param>
        public static void ZipUnFileInfo(string zipFilePath, string savePath)
        {
            try
            {
                using (var zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry zipEntry;
                    while ((zipEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(zipEntry.Name);
                        string fileName = Path.GetFileName(zipEntry.Name);
                        string serverFolder = savePath;
                        //创建一个文件目录信息
                        Directory.CreateDirectory(serverFolder + "/" + directoryName);
                        //如果解压的文件不等于空，则执行以下步骤
                        if (fileName != string.Empty)
                        {
                            using (FileStream fileStream = File.Create((serverFolder + "/" + zipEntry.Name)))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048]; //初始化字节数为2兆，后面根据需要解压的内容扩展字节数
                                while (true)
                                {
                                    size = zipInputStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        fileStream.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                fileStream.Close();
                            }
                        }
                    }
                    zipInputStream.Close();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("出现错误了，不能解压缩,错误原因：" + exception.Message);
            }
        }

        /// <summary>
        /// 解压缩—结果不包含文件夹
        /// </summary>
        /// <param name="zipFilePath">源压缩的文件路径</param>
        /// <param name="savePath">解压后的文件保存路径</param>
        public static void ZipUnFileWithOutFolderInfo(string zipFilePath, string savePath)
        {
            try
            {
                using (var zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry zipEntry;
                    while ((zipEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        string fileName = Path.GetFileName(zipEntry.Name);
                        if (fileName != string.Empty)
                        {
                            using (var fileStream = File.Create(savePath))
                            {
                                int size = 2084; //初始化字节数为2兆，后面根据需要解压的内容扩展字节数
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = zipInputStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        fileStream.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                fileStream.Close();
                            }
                        }
                    }
                    zipInputStream.Close();
                }
            }
            catch (Exception exception)
            {

                throw new Exception("解压缩出现错误了，错误原因：" + exception.Message);
            }
        }
    }
}