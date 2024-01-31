using System.IO;
using Xapp;
using Xapp;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Text;

namespace Dextra.Toolsspace
{
    public class LangResume
    {
        public string Translated { get; set; }
        public bool Succes { get; set; }
        public LangResume()
        {
            Translated = "";
            Succes = true;
        }
    }
    public static class Langue
    {

        public static string Lang(string word, string GroupSelect = null, string Lang = "hu")
        {
            return Translate(word, GroupSelect, Lang);
        }

        public static string TranslateXform(string word, string lang, string GroupSelect = null)
        {
            return Translate(word, "XForm" + GroupSelect, lang);
        }

        public static string Translate(string word, string GroupSelect = null, string Lang = "hu")
        {
            if (Lang == null) Lang = "hu";  //Lang = "hu.Default.Default";
            LangContent vals = new LangContent();
            string ret = "";
            if (word == null) return ret;
            word = word.Trim('§');
            string key = GroupSelect == null ? word : GroupSelect + "_" + word;
            ret = word;
            //ret = MvcApplication.LangDictionary.ContainsKey(key) ? MvcApplication.LangDictionary[key] : word;
            if (MvcApplication.LangDictionaryM.ContainsKey(key))
            {
                vals = MvcApplication.LangDictionaryM[key];
            }
            else
            {
                if (MvcApplication.LangDictionaryM.ContainsKey(word))
                {
                    vals = MvcApplication.LangDictionaryM[word];
                    //vals = MvcApplication.LangDictionaryM.ContainsKey(key) ? MvcApplication.LangDictionaryM[key] : new LangContent(word, word, word);
                }
                else
                {
                    vals = new LangContent(word + "", word + "", word + "");
                    MvcApplication.LangDictionaryM.Add(key, vals);
                }

            }

            //switch (Lang.Split('.')[0].ToLower())
            switch (Lang.ToLower())
            {
                case "hu":
                    ret = vals.HuValue;
                    break;
                case "en":
                    ret = vals.EnValue;
                    break;
                case "oth":
                    ret = vals.OthValue;
                    break;
                default:
                    ret = vals.HuValue;
                    break;
            }
            if (string.IsNullOrEmpty(ret)) ret = word;
            ret = ret.Replace("_", " ");
            return ret;
        }


        public static string EnumTranslateXform(string key, string value, string lang, string GroupSelect = null)
        {
            return EnumTranslate(key, value, "XF_" + GroupSelect, lang);
        }


        public static string EnumTranslate(string key, string value, string GroupSelect = null, string Lang = "hu")
        {
            if (Lang == null) Lang = "hu";
            LangContent vals = new LangContent();
            string ret = "";
            if (key == null) return ret;
            key = key.Trim('§');
            string fkey = GroupSelect == null ? key : GroupSelect + ":" + key;
            ret = key;

            if (MvcApplication.EnumDictionaryM.ContainsKey(fkey))
            {
                vals = MvcApplication.EnumDictionaryM[fkey];
            }
            else
            {
                if (MvcApplication.EnumDictionaryM.ContainsKey(key))
                {
                    vals = MvcApplication.EnumDictionaryM[key];
                    // vals = MvcApplication.EnumDictionaryM.ContainsKey(key) ? MvcApplication.EnumDictionaryM[key] : new LangContent(word, word, word);
                }
                else
                {
                    vals = new LangContent(value + "", value + "", value + "");
                    MvcApplication.EnumDictionaryM.Add(fkey, vals);
                }

            }

            switch (Lang.Split('.')[0].ToLower())
            {
                case "hu":
                    ret = vals.HuValue;
                    break;
                case "en":
                    ret = vals.EnValue;
                    break;
                case "oth":
                    ret = vals.OthValue;
                    break;
                default:
                    ret = vals.HuValue;
                    break;
            }
            if (string.IsNullOrEmpty(ret)) ret = key;
            ret = ret.Replace("_", " ");
            return ret;
        }

        public static string escapecsv(string data)
        {
            if (data.Contains("\""))
            {
                data = data.Replace("\"", "\"\"");
            }

            if (data.Contains(","))
            {
                data = String.Format("\"{0}\"", data);
            }

            if (data.Contains(System.Environment.NewLine))
            {
                data = String.Format("\"{0}\"", data);
            }

            return data;
        }


        public static void processlang()
        {

            Dictionary<string, string> test = new Dictionary<string, string>();
            Dictionary<string, string> test1 = new Dictionary<string, string>();

            string filepath = Tools.Getpath("Langwork.csv", C.LangDir);
            File.Delete(filepath);
            using (StreamWriter outputFile = new StreamWriter(filepath, false, Encoding.UTF8))
            {

                List<string> files = Directory.GetFiles("C:\\Work\\DotNetDevelope\\Dextra\\Xapp\\Xapp\\Views", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".js") || s.EndsWith(".cshtml")).ToList<string>();
                foreach (string file in files)
                {

                    string filename = Path.GetFileNameWithoutExtension(file);
                    string ext = Path.GetExtension(file);
                    string path = Path.GetDirectoryName(file);
                    path = path.Replace("C:\\Work\\DotNetDevelope\\Dextra\\Xapp\\Xapp", "");
                    string formid = "";
                    string text = System.IO.File.ReadAllText(file);
                    string line = "";

                    string key = "";
                    string val = "";


                    Regex regex = new Regex(@"ViewBag.FormId(.*)""(.*)""", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                    Match matchfid = regex.Match(text);
                    if (matchfid.Success)
                    {
                        formid = matchfid.Groups[2].Value;
                    }
                    string[] sp = path.Split('\\');
                    string lastpath = sp[sp.Length - 1];

                    if (ext == ".js")
                    {

                        MatchCollection matches = Regex.Matches(text, @"§(.*?)§", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                        foreach (Match match in matches)
                        {
                            var x = match.Groups[0].Value;
                            x = x.Replace("§", "");

                            key = escapecsv("Js_" + lastpath + filename + ".js_" + x);
                            val = escapecsv(x);


                            line = escapecsv(path) + "," + escapecsv(filename) + "," + escapecsv(ext) + ",Js_" + lastpath + filename + "," + escapecsv(x) + "," + key + "," + escapecsv(x);


                            outputFile.WriteLine(line);
                            try
                            {

                                test.Add(key, val);

                            }
                            catch { }
                            try
                            {
                                test1.Add(val, val);
                            }
                            catch { }

                        }


                    }
                    else
                    {
                        MatchCollection matches = Regex.Matches(text, @"(?<=@L\("")(.*?)(?=""\))|(?<=@Html\.Lang\()(.*?)(?=\))", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
                        foreach (Match match in matches)
                        {
                            var x = match.Groups[0].Value;
                            if (x != "title, (string")
                            {

                                key = escapecsv(formid + "_" + x);
                                val = escapecsv(x);

                                line = escapecsv(path) + "," + escapecsv(filename) + "," + escapecsv(ext) + "," + escapecsv(formid) + "," + escapecsv(x) + "," + escapecsv(formid + "_" + x) + "," + escapecsv(x);
                                outputFile.WriteLine(line);
                                try
                                {

                                    test.Add(key, val);

                                }
                                catch { }
                                try
                                {
                                    test1.Add(val, val);
                                }
                                catch { }
                            }





                        }

                    }

 

                }



            }

            string filepath1 = Tools.Getpath("Langwork1.csv", C.LangDir);
            File.Delete(filepath1);
            using (StreamWriter outputFile = new StreamWriter(filepath1, false, Encoding.UTF8))
            {
                foreach(var k in test1)
                {
                    outputFile.WriteLine(k.Key+","+k.Value+"," + k.Value + "," + k.Value );

                }
            }


        }



    }
}