// 源文件头信息：
// <copyright file="DropDownListHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/7/3 10:17
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 对象转换器的帮助类,将查询出来同名的内容调用下面的方法转换成实体对象
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/12/11</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.
    /// </summary>
    public static class ObjectConvertHelper
    {
        /// <summary>
        /// 使用此方法：
        ///     var list = ObjectConverter.ConvertTo<实体类>(使用Linq查询出来的结果);
        /// </summary>
        /// <typeparam name="TResult">目标实体对象</typeparam>
        /// <param name="sources">源目标</param>
        /// <returns></returns>
        public static List<TResult> ConvertToInfo<TResult>(IEnumerable sources) where TResult : new()
        {
            /* 第一步：判断传递的source对象是否为空，如果为空，则不能进行任何转换
               第二步：判断传递的对象和需要转换的目标类型是否一致，如果一致，则可以直接进行转换
               第三步：如果不满足上面两个条件，则直接琢个属性的去对应进行转换 */
            if (sources == null)
            {
                return null;
            }
            if (sources is IEnumerable<TResult>)
            {
                return sources.Cast<TResult>().ToList();
            }

            //第三步：执行源程序和对象实体的转换
            var result = new List<TResult>();
            bool hasGetElementType = false;
            IEnumerable<CommonProperty> commonProperties = null; //公共属性(按属性名称进行匹配)
            foreach (var source in sources)
            {
                if (!hasGetElementType) //访问第一个元素的时候，取得属性的对应关系，后续的元素就不用在重新计算了
                {
                    if (source is TResult) //如果源类型是目标类型的子类，可以直接使用Cast<T>扩展方法
                    {
                        return sources.Cast<TResult>().ToList();
                    }
                    commonProperties = GetCommonProperties(source.GetType(), typeof (TResult));
                    hasGetElementType = true;
                }
                //转换第二种行为
                var tResult = new TResult();
                foreach (CommonProperty commonProperty in commonProperties)
                {
                    object value = commonProperty.SourceProperty.GetValue(source, null);
                    commonProperty.TargetProperty.SetValue(tResult, value, null);
                }
                result.Add(tResult);
            }
            return result;
        }

        /// <summary>
        /// 将源目标特性和实体特性转化成实体类对象
        /// </summary>
        /// <param name="sourceType">源目标特性</param>
        /// <param name="targetType">实体目标特性</param>
        /// <returns></returns>
        private static IEnumerable<CommonProperty> GetCommonProperties(Type sourceType, Type targetType)
        {
            PropertyInfo[] sourceTypeProperties = sourceType.GetProperties(); //获取源对象的所有属性
            PropertyInfo[] targetTypeProperties = targetType.GetProperties(); //获取目标对象的所有属性

            //使用Linq将源对象和目标对象查询出实体对象,根据属性名进行对应(不区分大小写)
            var commonPropertyInfo = from sourceP in sourceTypeProperties
                join targetP in targetTypeProperties on sourceP.Name.ToLower() equals targetP.Name.ToLower()
                select new CommonProperty
                {
                    SourceProperty = sourceP,
                    TargetProperty = targetP
                };
            return commonPropertyInfo;
        }

        private class CommonProperty
        {
            public PropertyInfo SourceProperty { get; set; }

            public PropertyInfo TargetProperty { get; set; }

        }
    }
}