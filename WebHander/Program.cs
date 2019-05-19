using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
namespace WebHander
{
    class Program
    {
        private static Regex regexJs = new Regex("(?<=<script(.)*?>)([\\s\\S](?!<script))*?(?=</script>)", RegexOptions.IgnoreCase);
        static void Main(string[] args)
        {
            string url = "https://www.wikifx.com/au_en/";

            string htmlSstring = WebHelper.GetWebContent(url);
            var styleReg = @"/ (< style.*?<\/ style >)/ g";
            ///var linkReg =@"/ (< link.*\s + href = (?: "[^"]*"|'[^']*')[^<]*>)/g";
            //var scriptReg =@ "/< script.*?>.*?<\/ script >/ g";
            //var repJs = Regex.Replace(htmlSstring, @" /<script\b.*?(?:\bsrc\s?=\s?([^>]*))?>(.*?)<\/script>/ig", "https://www.wikifx.com//Contentgj", RegexOptions.None);
            //var c = regexJs.Matches(htmlSstring, 0);
            List<string> listJs = WebHelper.GetHtmlAttr(htmlSstring, "script", "src");
            List<string> listCs = WebHelper.GetHtmlAttr(htmlSstring, "link", "href");
            List<string> listImg = WebHelper.GetHtmlAttr(htmlSstring, "img", "src");
            //List<string> listA = WebHelper.GetHtmlAttr(htmlSstring, "a", "href");
            List<string> Img=new List<string>();
            List<string> a = new List<string>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string item in listImg)
            {
                if (!item.Contains("https") && item!= "/Contentgj/images/default.png")
                {
                    Img.Add(item);
                }
            }
            //foreach (string item in listA)
            //{
            //    if (!item.Contains("https") && !item.Contains("javascript"))
            //    {
            //        a.Add(item);
            //    }
            //}
            int i = 0;
            StreamWriter sw = null;//写入流对象
            foreach (string item in listJs)
            {
                i++;
                dic.Add(i.ToString(), item + ")" + "https://www.wikifx.com" + item);
            }
            foreach (string item in listCs)
            {
                i++;
                dic.Add(i.ToString(), item + ")" + "https://www.wikifx.com" + item);
            }
            foreach (string item in Img)
            {
                if (!item.Contains("https"))
                {
                    i++;
                    if (!item.Contains("eimgjys.wikifx.com") &&!item.Contains("//img.wikifx.com"))
                    {
                        dic.Add(i.ToString(), item + ")" + "https://www.wikifx.com" + item);
                    }
                    else
                    {
                        dic.Add(i.ToString(), item + ")" + "https:" + item);
                    }
                }
            }
           // foreach (string item in a)
            //{
            //    i++;
            //    if (!item.Contains("//www.wikifx.com") && !item.Contains("//live.wikifx.com") && !item.Contains("//tools.wikifx.com") && !item.Contains("//survey.wikifx.com"))
            //    {
            //        dic.Add(i.ToString(), item + ")" + "https://www.wikifx.com" + item);
            //    }
            //    else
            //    {
            //        dic.Add(i.ToString(), item + ")" + "https:" + item);
            //    }

            //}
            foreach (var item in dic)
            {
                string[] arry = item.Value.Split(')');
                htmlSstring = htmlSstring.Replace(arry[0], arry[1]);
            }
            htmlSstring = htmlSstring.Replace("https://www.wikifx.comhttps://www.wikifx.com/Contentgj/js/loadsh.js", "https://www.wikifx.com/Contentgj/js/loadsh.js");
            htmlSstring = htmlSstring.Replace("https:https:", "https:");
            htmlSstring = htmlSstring.Replace(" https: https:", "https:");
            htmlSstring = htmlSstring.Replace("https:https:", "https");
            htmlSstring = htmlSstring.Replace("href=/au_en", "href=https://www.wikifx.com/au_en");
            htmlSstring = htmlSstring.Replace("https//eimgjys.wikifx.com/logo/0001698019/FXT0001698019_907528.png_wiki-template-global", "https://eimgjys.wikifx.com/logo/0001698019/FXT0001698019_907528.png_wiki-template-global");
            try
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());//随机函数
                int indexRandom = random.Next(100, 999);
                Encoding code = Encoding.GetEncoding("UTF-8"); //声明文件编码

                string htmlfilename = DateTime.Now.ToString("yyyyMMddHHmmss") + indexRandom + ".html";
                string createfilename = DateTime.Now.ToString("yyyyMM");

                //创建文件夹               
                string FilePath = AppDomain.CurrentDomain.BaseDirectory + ("~/htmlmessage/") + createfilename;


                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }


                sw = new StreamWriter(FilePath + "\\" + htmlfilename, false, code);
                sw.Write(htmlSstring);
                sw.Flush();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sw.Close();

            }

            Console.WriteLine(htmlSstring);
        }
    }
}
