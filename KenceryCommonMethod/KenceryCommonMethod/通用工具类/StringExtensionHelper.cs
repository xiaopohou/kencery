// 源文件头信息：
// <copyright file="StringExtensionHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2015/01/14
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 针对字符串的扩展方法
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2015/01/14</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.
    /// </summary>
    public static class StringExtensionHelper
    {
        /// <summary>
        /// 指定的正则表达式在传递过来的字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">搜索匹配项的字符串</param>
        /// <param name="pattern">匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为true,否则为false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            return value != null && Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要配的匹正则表达式对象</param>
        /// <returns>返回一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            return value == null ? null : Regex.Match(value, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>返回一个集合，</returns>
        public static IEnumerable<string> Matchs(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] {};
            }
            MatchCollection matchCollection = Regex.Matches(value, pattern);
            //使用Linq返回集合
            var linqCollection = from Match n in matchCollection
                select n.Value;
            return linqCollection;
        }
    }
}