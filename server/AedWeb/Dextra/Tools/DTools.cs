using Dextra.Database;
using Dextra.Xforms;
using DextraLib.XForm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Dextra.Toolsspace
{

    //string hashed_password = SecurePasswordHasherHelper.Hash("12345");
    // if (SecurePasswordHasher.Verify("12345",hashed_password)) {      return "Password is Valid"; }



    public class SecurePasswordHasherHelper
    {
        /// <summary>
        /// Size of salt
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Creates a hash from a password
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="iterations">number of iterations</param>
        /// <returns>the hash</returns>
        public static string Hash(string password, int iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }
        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">the password</param>
        /// <returns>the hash</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Check if hash is supported
        /// </summary>
        /// <param name="hashString">the hash</param>
        /// <returns>is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="hashedPassword">the hash</param>
        /// <returns>could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }



    public static class Tools
    {
        public static LangResume Lang(string iindex)
        {
            LangResume ret = new LangResume();
            ret.Succes = true;
            ret.Translated = iindex;
            return ret;
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;


            if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";


            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            return baseUrl;
        }

        public static string Getpath(string file, string dir = null)
        {
            if (dir == null) dir = P.LangDir;
            return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(dir), file);
        }

        public static string GetXformXmlComboPath(string file)
        {
            string dir = P.c_XmlCodes;
            return Path.Combine(System.Web.HttpContext.Current.Server.MapPath(dir), file);
        }

        public static string ReplaceFirstOccurrance(this string original, string oldValue, string newValue)
        {
            if (String.IsNullOrEmpty(original))
                return String.Empty;
            if (String.IsNullOrEmpty(oldValue))
                return original;
            if (String.IsNullOrEmpty(newValue))
                newValue = String.Empty;
            int loc = original.IndexOf(oldValue);
            if (loc < 0) return original;
            return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
        }

        public static bool isNumericTypeOld(Type dataType)
        {
            return (dataType == typeof(int)
                    || dataType == typeof(double)
                    || dataType == typeof(long)
                    || dataType == typeof(short)
                    || dataType == typeof(float)
                    || dataType == typeof(Int16)
                    || dataType == typeof(Int32)
                    || dataType == typeof(Int64)
                    || dataType == typeof(uint)
                    || dataType == typeof(UInt16)
                    || dataType == typeof(UInt32)
                    || dataType == typeof(UInt64)
                    || dataType == typeof(sbyte)
                    || dataType == typeof(Single)
                   );

        }

        public static bool isNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //return Nullable.GetUnderlyingType(type).IsNumeric();
                        return isNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static string Serialize(object model)
        {
            XmlSerializer xmlserializer = new XmlSerializer(model.GetType());

            //using (StringWriterUTF8 stringWriter = new StringWriterUTF8())

            using (StringWriter stringWriter = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, model);
                return stringWriter.ToString();
            }
            return null;
        }

        public static object Deserialize(object model, string serxml)
        {
            XmlSerializer xmlserializer1 = new XmlSerializer(model.GetType());

            using (StringReader stringReader = new StringReader(serxml))
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                if (xmlserializer1.CanDeserialize(reader))
                {
                    return model = (xmlserializer1.Deserialize(reader));

                }
            }
            return null;
        }

        // daohelper copy
        public static string InitCapitalConvert(string input_string)
        {
            input_string = ' ' + input_string.ToLower();

            string[] AlphabatList = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            foreach (string alpha in AlphabatList)
            {
                input_string = input_string.Replace(' ' + alpha, ' ' + alpha.ToUpper());
                input_string = input_string.Replace('_' + alpha, '_' + alpha.ToUpper());
            }
            return input_string.Trim();
        }

        public static object[] BuildparamfromClass(ref string sql, object e)
        {
            object[] retval = new object[] { };
            int i = 0;
            Regex rgx = new Regex(@":\w*%|:\w*");
            foreach (Match match in rgx.Matches(sql))
            {
                try
                {
                    Array.Resize(ref retval, retval.Length + 1);
                    string pname = match.Value.TrimStart(':');
                    if (pname.EndsWith("%"))
                    {
                        pname = pname.TrimEnd('%');
                        retval[retval.Length - 1] = e.GetType().GetProperty(pname).GetValue(e, null) + "%";
                    }
                    else
                    {
                        retval[retval.Length - 1] = e.GetType().GetProperty(pname).GetValue(e, null);
                    }
                    sql = sql.Replace(match.Value, ":" + i.ToString()); i++;
                }
                catch { }
            }
            return retval;
        }

        public static object[] BuildparamfromXform(ref string sql, XForm x)
        {
            object[] retval = new object[] { };
            int i = 0;
            Regex rgx = new Regex(@":\w*%|:\w*");
            foreach (Match match in rgx.Matches(sql))
            {
                try
                {
                    Array.Resize(ref retval, retval.Length + 1);
                    string pname = match.Value.TrimStart(':');
                    if (pname.EndsWith("%"))
                    {
                        pname = pname.TrimEnd('%');


                        retval[retval.Length - 1] = x.GetElementValue(XformUtil.fromName(pname)) + "%";   //x.GetElementValueById(pname,"")+"%";     //e.GetType().GetProperty(pname).GetValue(e, null) + "%";
                    }
                    else
                    {
                        retval[retval.Length - 1] = x.GetElementValue(XformUtil.fromName(pname)); // x.GetElementValueById(pname, "");       // e.GetType().GetProperty(pname).GetValue(e, null);
                    }
                    sql = sql.Replace(match.Value, ":" + i.ToString()); i++;
                }
                catch { }
            }
            return retval;
        }

        public static int SetXformelementFromDaoresult(ref Xform work, Dao qd)
        {
            int cik = 1;

            if (1 == 11)
            {
                foreach (var property in (IDictionary<String, Object>)qd.DynResult)
                {
                    work.SetXformelement(XformUtil.fromName("/" + property.Key), property.Value.ToString());
                }
            }
            else
            {
                cik = 1;
                string lkey = "";
                foreach (System.Dynamic.ExpandoObject o in qd.Result.Resultset)
                {
                    foreach (var property in (IDictionary<String, Object>)o)
                    {
                        if (property.Key.Contains("_X_"))
                        {
                            if (cik == 1)
                            {
                                lkey = property.Key.Replace("_X_", "");
                            }
                            else
                            {
                                lkey = property.Key.Replace("_X_", "_" + cik.ToString() + "_");
                            }
                        }
                        else
                        {
                            lkey = property.Key;
                        }

                        work.SetXformelement(XformUtil.fromName("/" + lkey), property.Value.ToString());

                    }
                    cik++;
                    if (cik > 20) break;
                    //dynamic sampleObject = o;
                    //var t = sampleObject.Birthfirstname;
                }
            }
            return cik;
        }

        public static DateTime? ToDate(string date, string format = "yyyy.mm.dd.")
        {
            DateTime? dateTime = null;
            try
            {
                dateTime = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
            }
            catch
            {

            }
            return dateTime;
        }

        public static string GetTwinFile(string file, string twin, string dirconst)
        {
            string ret = file;
            string twinfilename = Path.GetFileNameWithoutExtension(file) + twin + Path.GetExtension(file);
            if (System.IO.File.Exists(Tools.Getpath(twinfilename, dirconst)))
            {
                ret = twinfilename;
            }
            return ret;
        }


        public static string GetEncoding(string filename)
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                return cdet.Charset;
            }
        }

        // Report

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncodings(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }
        public static String readFileAsUtf8(string fileName)
        {
            Encoding encoding = Encoding.Default;
            String original = String.Empty;

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            if (encoding == Encoding.UTF8)
                return original;

            byte[] encBytes = encoding.GetBytes(original);
            byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

    }
}