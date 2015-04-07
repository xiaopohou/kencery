// 源文件头信息：
// <copyright file="DropDownListHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/7/3 10:17
// </copyright>

using System.Collections.Generic;
using System.ComponentModel;

namespace KenceryCommonMethod
{
    /// <summary>
    /// 下拉框得到枚举的类型对象
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/7/3</date>
    /// </auther>
    /// 修改记录：时间  内容  姓名
    ///     1.
    /// </summary>
    public  class DropDownListHelper
    {
        #region-----共有方法-----

        /// <summary>
        /// 前台下拉框得到单选框的选择信息(无/请选择/全部)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ExtendIdName> ListItemIdNameInfo(int id = 0)
        {
            var list = new List<ExtendIdName>();
            switch (id)
            {
                case 0: //没有全部或者请选择信息
                    break;
                case 1: //全部
                    list.Add(new ExtendIdName {Id = -1, Name = "---全部----"});
                    break;
                default: //请选择
                    list.Add(new ExtendIdName {Id = -1, Name = "---请选择----"});
                    break;
            }
            return list;
        }

        /// <summary>
        /// 获取枚举的状态(启用/禁用)
        /// </summary>
        /// <param name="id">传递的参数Id</param>
        /// <returns></returns>
        public List<ExtendIdName> GetBindIsEnableStatusInfo(int id = 0)
        {
            //控制器中的调用枚举的方法是：
            //var list = ListItemIdName(id);
            //(from n in typeof(IsEnabled).GetEnumList().OrderByDescending(c => c.Id)
            // select new { n.Id, n.Name }).ForEach(c => list.Add(new ExtendIdName
            // {
            //     Id = c.Id,
            //     Name = c.Name
            // }));
            //return JsonResult(list);
            return null;
        }

        #endregion
    }

    /// <summary>
    /// 定义类来存放下拉框的信息
    /// </summary>
    public class ExtendIdName
    {
        /// <summary>
        /// 下拉框Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 下拉框名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 枚举状态(启用/禁用)
    /// </summary>
    public enum IsEnabled
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")] Enabled = 1,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")] Disabled = 0
    }
}