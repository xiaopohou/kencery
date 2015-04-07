// 源文件头信息：
// <copyright file="ValidateCodeHelper.cs">
// Copyright(c)2014-2034 Kencery.All rights reserved.
// 个人博客：http://www.cnblogs.com/hanyinglong
// 创建人：韩迎龙(kencery)
// 创建时间：2014/6/30 10:42
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace KenceryCommonMethod
{
    /// <summary>
    ///  var validateCode = new ValidateCode();
    ///  string code = validateCode.CreateValidateCode(4); 生成验证码的长度
    ///  Session["ValidateCode"] = code;   讲验证码赋值给Session
    ///  byte[] bytes = validateCode.CreateGraphic(code);  创建验证码的图片
    /// -----------------------------------------------------------------------------
    /// 验证码辅助类
    /// <auther>
    ///     <name>Kencery</name>
    ///     <date>2014/6/30</date>
    /// </auther>
    /// 验证码生成修改记录：
    ///     1.  2014/06/30  创建记录，验证生成数字的验证码  by Kencery
    ///     2.  2014/07/03  添加如何使用该验证码的信息
    /// </summary>
    public class ValidateCodeHelper
    {
        #region--------------------------属性--------------------------

        /// <summary>
        /// 验证码的最大长度，限制最长为10
        /// </summary>
        public int MaxLength
        {
            get { return 10; }
        }

        /// <summary>
        /// 验证码的最小长度，不能小于3
        /// </summary>
        public int MinLength
        {
            get { return 3; }
        }

        #endregion

        #region-----------------------私有方法-------------------------

        /// <summary>
        /// 生成验证码的流文件
        /// </summary>
        /// <param name="validateCode">传递过来生成的验证码的随机数</param>
        /// <returns></returns>
        public static MemoryStream CreateStreamInfo(string validateCode)
        {
            Bitmap image = new Bitmap((int) Math.Ceiling(validateCode.Length*12.0), 22);
            Graphics graphics = Graphics.FromImage(image);
            try
            {
                //生成随机数
                Random random = new Random();
                //清空图片的背景色
                graphics.Clear(Color.White);
                //画出来图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    graphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue, Color.DarkRed, 1.2f, true);
                graphics.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片信息
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream;
            }
            finally //最终释放所有的资源
            {
                graphics.Dispose();
                image.Dispose();
            }
        }

        #endregion

        #region-------------------------共有方法-----------------------

        /// <summary>
        /// 返回生成的随机数，生成的随机数全部是数字 
        /// </summary>
        /// <param name="length">生成几个随机数</param>
        /// <returns></returns>
        public string CreateNumValidateCodeInfo(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            //定义生成验证码的字段存放
            string validateNumberStr = "";
            //生成其实序列化值
            int seekSeek = unchecked((int) DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, Int32.MaxValue - length*10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 1000;
                seeks[i] = beginSeek;
            }
            //生成随机数
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1*(int) Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString(CultureInfo.InvariantCulture);
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //抽取随机数生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString(CultureInfo.InvariantCulture);
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 返回生成的随机数，生成的随机数是数字字母混合
        /// </summary>
        /// <param name="length">生成几个随机数</param>
        /// <returns></returns>
        public string CreateBlenddValidateCodeInfo(int length)
        {
            string[] source = ("1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                               "a,b,c,d,e,f,g,h,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z").Split(',');
            //定义生成验证码的字段存放
            string validateNumberStr = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += source[random.Next(0, source.Length)];
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 生成验证码  提供给控制器调用
        /// </summary>
        /// <param name="validateCode">传递进来获取的随机数信息</param>
        /// <returns></returns>
        public byte[] CreateGraphicInfo(string validateCode)
        {
            MemoryStream memory = CreateStreamInfo(validateCode);
            return memory.ToArray();
        }

        #endregion
    }
}