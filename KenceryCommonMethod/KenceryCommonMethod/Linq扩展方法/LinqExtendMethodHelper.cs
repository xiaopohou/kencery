// 源文件头信息：
// <copyright file="LinqExtendMethodHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/12/18
// </copyright>

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Aspose.Words.Properties;

namespace KenceryCommonMethod
{
    /// <summary>
    ///  Linq OrderBy封装的传递查询，由于本质Linq To EF支持OrderBy，所以只要模拟个keySelector后，
    ///     将剩余的工作交还给Linq To EF Provider来处理
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/12/18</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.  
    /// </summary>
    public static class LinqExtendMethodHelper
    {
        /// <summary>
        /// 扩展Linq的OrderBy方法，实现根据属性和顺序(倒序)进行排序，调用和linq的方法一致
        /// </summary>
        /// <typeparam name="TEntity">需要排序的实体对象</typeparam>
        /// <param name="source">结果集信息</param>
        /// <param name="propertyStr">动态排序的属性名(从前台获取)</param>
        /// <param name="isDesc">排序方式，不传递表示顺序，默认true，false表示倒序</param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyStr,
            bool isDesc = true) where TEntity : class
        {
            //以下四句用来建立c>c.propertyStr的Expression对象，实现Lambda表达式的状态
            ParameterExpression parameterExpression = Expression.Parameter(typeof (TEntity), "c");
            PropertyInfo propertyInfo = typeof (TEntity).GetProperty(propertyStr);
            Expression expression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            LambdaExpression lambdaExpression = Expression.Lambda(expression, parameterExpression);
            Type type = typeof (TEntity);

            //读取排序的顺序信息，如果传递的参数(isDesc)是true，则为顺序排序，否则为倒序排序
            string ascDesc = isDesc ? "OrderBy" : "OrderByDescending";

            //Expression.Call跟上面的信息一样，这里采用重载的形式，上面的GetCurrentMethod结果也是ascDesc
            //Expression.Call方法会利用typeof(Queryable),ascDesc,new Type[]{type,property,PropertyType}三个参数
            //合成跟MethodInfo等同的消息
            MethodCallExpression methodCallExpression = Expression.Call(typeof (Queryable), ascDesc,
                new Type[] {type, propertyInfo.PropertyType}, source.Expression, Expression.Quote(lambdaExpression));

            //返回成功
            return (IOrderedQueryable<TEntity>) source.Provider.CreateQuery<TEntity>(methodCallExpression);

        }
    }
}