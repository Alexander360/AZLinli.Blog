using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wchl.WMBlog.Common.ToolsHelper
{
    /// <summary>
    /// 公共实用工具
    /// </summary>
    public class Tools
    {
        #region 计算两个时间差值的函数，返回时间差的绝对值
        /// <summary>
        /// 计算两个时间差值的函数，返回时间差的绝对值：
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天"
                        + ts.Hours.ToString() + "小时"
                        + ts.Minutes.ToString() + "分钟"
                        + ts.Seconds.ToString() + "秒";
            }
            catch
            {
            }
            return dateDiff;
        }
        #endregion

        #region 去除富文本中的HTML标签
        /// <summary>
        /// 去除富文本中的HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        #endregion

        /// <summary>上传文件方法，默认生成日期文件夹,MVC使用
        /// 返回文件名
        /// </summary>
        /// <param name="PostedFile">mvc后台获取的</param>
        /// <param name="allowExtensions">允许上传的扩展文件名类型,如：string[] allowExtensions = { ".doc", ".xls", ".ppt", ".jpg", ".gif" };</param>
        /// <param name="maxLength">允许上传的最大大小，以M为单位</param>
        /// <param name="savePath">保存文件的目录，注意是绝对路径,如：Server.MapPath("~/upload/");</param>
        /// <param name="gendatedir">是否生成日期文件夹</param>
		/// <param name="guidname">是否是GUID名字，默认true</param>
        /// <param name="savename">保存文件名（不带后缀），如果非空则以此文件名保存</param>
        public static string Upload_MVC(HttpPostedFileBase PostedFile, string[] allowExtensions, int maxLength, string savePath, bool gendatedir = true, bool guidname = true, string savename = "")
        {


            // 文件格式是否允许上传
            bool fileAllow = false;


            // 检查文件大小, ContentLength获取的是字节，转成M的时候要除以2次1024
            if (PostedFile.ContentLength / 1024 / 1024 >= maxLength)
            {
                throw new Exception("只能上传小于" + maxLength + "M的文件！");
            }

            //取得上传文件之扩展文件名，并转换成小写字母
            string fileExtension = System.IO.Path.GetExtension(PostedFile.FileName).ToLower();
            string tmp = "";   // 存储允许上传的文件后缀名
                               //检查扩展文件名是否符合限定类型
            for (int i = 0; i < allowExtensions.Length; i++)
            {
                tmp += i == allowExtensions.Length - 1 ? allowExtensions[i] : allowExtensions[i] + ",";
                if (fileExtension == allowExtensions[i])
                {
                    fileAllow = true;
                }
            }

            if (fileAllow)
            {
                try
                {
                    string datedir = DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(savePath + datedir) && gendatedir)
                    {
                        Directory.CreateDirectory(savePath + datedir);
                    }
                    string saveName = guidname ? Guid.NewGuid() + fileExtension : PostedFile.FileName;
                    if (!string.IsNullOrEmpty(savename))
                    {
                        saveName = savename + fileExtension;
                    }
                    string path = gendatedir ? savePath + datedir + "/" + saveName : savePath + saveName;
                    //存储文件到文件夹
                    PostedFile.SaveAs(path);
                    return gendatedir ? datedir + "/" + saveName : saveName;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception("文件格式（" + fileExtension + "）不符，可以上传的文件格式为：" + tmp);
            }
        }
    }
}
