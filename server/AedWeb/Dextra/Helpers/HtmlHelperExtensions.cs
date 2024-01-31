using Dextra.Toolsspace;
using Xapp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Xapp;

namespace Dextra.Helpers
{
    public static class HtmlHelperExtensions
    {


        public static MvcHtmlString FunctDashRender(this HtmlHelper helper, string Appselector, string param)
        {
            string retval = "";


            return new MvcHtmlString(retval);
        }

        public static MvcHtmlString IndexRender(this HtmlHelper helper, string Appselector,string param)
        {
            string retval = "";

            switch (Appselector)
            {
                case "BM":
                    string path = HttpContext.Current.Server.MapPath(param);
                    if (File.Exists(path))
                    {
                        retval = File.ReadAllText(path);
                    }
                    break;
                case "Default":
                    break;
                case "Riska":
                    break;
                case "Special":
                    break;
                default:
                    break;
            }

            return new MvcHtmlString(retval);

        }
        public static MvcHtmlString ServerSideInclude(this HtmlHelper helper, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);
            try
            {
                string markup = File.ReadAllText(filePath);

                string itemp = "";
                string inclc = "";
                Regex serarchinclude = new Regex(@"include\((.*?)\);");
                var incl = serarchinclude.Matches(markup);
                foreach (Match match in incl)
                {
                    itemp = match.Groups[1].Value;
                    string[] args = itemp.Split('|');
                    filePath = HttpContext.Current.Server.MapPath(args[0].Replace("'", ""));
                    try
                    {
                        inclc = File.ReadAllText(filePath);
                        //inclc = Regex.Replace(inclc, "args", args[1].Trim());
                        for (int i = 1; i < args.Length; i++)
                        {
                            //args\(123,(.*)\)  args(12,'/Alma/Ata')
                            
                            string[] prctemp = args[i].Split(',');
                            if (prctemp.Length == 2)
                            {
                                string xpatt0 = Regex.Replace(args[i].Replace("(", @"\("), @",(.*)\)", @"([^)]+)\)");
                                xpatt0 = xpatt0.Trim();
                               // string xpatt = prctemp[0].Replace("(", @"\(") + @",(.*)\)";
                                inclc = Regex.Replace(inclc, xpatt0, prctemp[1].Replace(")", "").Trim());
                            }
                            
                        }
                        // Regex clear = new Regex(@"args\((.*)\)");
                        Regex clear = new Regex(@"args\(([^)]+)\)"); 
                        var clearmatch = clear.Matches(inclc);
                        foreach (Match cm in clearmatch)
                        {
                            string[] repl = cm.Groups[1].Value.Split(',');
                            inclc = inclc.Replace(cm.Value, repl[1].Replace(")", ""));
                        }

                    }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
                    catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
                    {

                    }

                    markup = markup.Replace(match.Value, inclc);

                }


                string ulang = helper.ViewBag.Preferedlang;
                if (ulang == null) ulang = "hu";



                string temp = "";
                string temp1 = "";
                Regex expression = new Regex(@"§.*?§");
                var results = expression.Matches(markup);
                foreach (Match match in results)
                {
                    temp = match.Value;
                    temp1 = temp.Substring(1, temp.Length - 2);

                    string[] directories = filePath.Split(Path.DirectorySeparatorChar);
                    string selector = directories[directories.Length - 2] + directories[directories.Length - 1];
                    markup = markup.Replace(temp, Langue.Lang(temp, "Js_"+selector + "", ulang));//.Split('.')[0].ToLower()));
                    //markup = markup.Replace(temp, Langue.Lang(temp, Path.GetFileName(filePath) + "_", ulang.Split('.')[0].ToLower()));
                    //markup = markup.Replace(temp, temp);
                }

                return new MvcHtmlString(markup);
            }
            catch
            {
                return new MvcHtmlString("");
            }
        }


        //var matchingKeys = mydict.Keys.Where(x => x.Contains("key1"));
        public static MvcHtmlString GetLangdictionaryToClient(this HtmlHelper helper, string filter)
        {
            string ulang = helper.ViewBag.Preferedlang;
            if (ulang == null) ulang = "hu";
            Dictionary<string, string> ret = new Dictionary<string, string>();
            switch (ulang.Split('.')[0].ToLower())
            {
                case "hu":
                    ret = MvcApplication.LangDictionaryM.Where(x => x.Key.StartsWith(filter)).ToDictionary(mc => mc.Key.ToString(), mc => mc.Value.HuValue.ToString());
                    break;
                case "en":
                    ret = MvcApplication.LangDictionaryM.Where(x => x.Key.StartsWith(filter)).ToDictionary(mc => mc.Key.ToString(), mc => mc.Value.EnValue.ToString()); 
                    break;
                case "oth":
                    ret = MvcApplication.LangDictionaryM.Where(x => x.Key.StartsWith(filter)).ToDictionary(mc => mc.Key.ToString(), mc => mc.Value.OthValue.ToString());
                    break;
                default:
                    ret = MvcApplication.LangDictionaryM.Where(x => x.Key.StartsWith(filter)).ToDictionary(mc => mc.Key.ToString(), mc => mc.Value.HuValue.ToString());
                    break;
            }

            //Dictionary<string, string> ret = MvcApplication.LangDictionary.Where(x => x.Key.StartsWith(filter)).ToDictionary(mc => mc.Key.ToString(),mc => mc.Value.ToString());

            return new MvcHtmlString(JsonConvert.SerializeObject(ret));
        }


        public static MvcHtmlString Lang(this HtmlHelper helper, string value, string Formid = null)
        {
            string userid = helper.ViewBag.CurentUserId;
            string ulang = helper.ViewBag.Preferedlang;
            if (ulang == null) ulang = "hu";

            string temp = Langue.Lang(value, Formid, ulang);//.Split('.')[0].ToLower());
            //if (temp==value)
            //{
            //    if (!string.IsNullOrEmpty(Formid))
            //    {
            //        Formid = Formid.Replace('_', ' ');
            //    }

            //    temp = temp.ReplaceFirstOccurrance(Formid, "");
            //}
            return new MvcHtmlString(temp);
        }

    }
}