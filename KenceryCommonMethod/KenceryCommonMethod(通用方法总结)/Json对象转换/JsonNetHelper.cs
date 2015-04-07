// 源文件头信息：
// <copyright file="JsonNetHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/7/7 09:30
// </copyright>

using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace KenceryCommonMethod
{
    /// <summary>
    /// Json.NET,JavaScript两种类型对Json串的操作
    /// 使用： var Demo = JsonNet.JsonNetDeserializeToString<Demo />(string inserted);
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/7/7</date>
    /// </auther>
    /// Json转换对象的修改记录：
    ///     1.  2014/7/7  创建Json转换对象的信息  by Kencery
    /// </summary>
    public static class JsonNetHelper
    {
        #region-----共用静态方法  Json.NET进行转换Json对象-----

        /// <summary>
        /// 将实体对象转换为Json对象，生成字符串
        /// </summary>
        /// <param name="item">实体对象</param>
        /// <returns></returns>
        public static string JsonNetSerializeToEntityInfo(object item)
        {
            try
            {
                return JsonConvert.SerializeObject(item);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将Json串对象解析成固定的对象信息
        /// </summary>
        /// <typeparam name="T">需要得到解析后对象的实体类型</typeparam>
        /// <param name="jsonStr">json串</param>
        /// <returns></returns>
        public static T JsonNetDeserializeToStringInfo<T>(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region-----共用静态方法  JavaScript进行转换Json对象-----

        /// <summary>
        /// 将需要转换成json串的对象转换为Json字符串新消息
        /// </summary>
        /// <param name="item">需要实例化的对象</param>
        /// <returns></returns>
        public static string JavaScriptSerializeToEntityInfo(object item)
        {
            try
            {
                var javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(item);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将指定的Json字符串转换为T类型的对象，JavaScript序列化
        /// </summary>
        /// <typeparam name="T">需要得到解析后的对象的实体类型</typeparam>
        /// <param name="jsonStr">进行反序列化的Json字符串</param>
        /// <returns></returns>
        public static T JavaScriptSerializeToStringInfo<T>(string jsonStr)
        {
            try
            {
                var javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Deserialize<T>(jsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}