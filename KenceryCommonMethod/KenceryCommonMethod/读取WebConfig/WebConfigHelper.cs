// 源文件头信息：
// <copyright file="WebConfigHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/12/10 16:17
// </copyright>

using System.Configuration;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 读取WebConfig中的连接的字符串和其它的链接信息
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/12/10</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.
    /// </summary>
    public static class WebConfigHelper
    {
        /// <summary>
        ///  获取应用程序配置项(路劲存放)
        /// </summary>
        /// <param name="key">传递的key</param>
        /// <returns></returns>
        public static string GetAppSettingsInfo(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取连接数据库的连接字符串
        /// </summary>
        /// <param name="key">传递的key</param>
        /// <returns></returns>
        public static string GetConnectionStringsInfo(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}