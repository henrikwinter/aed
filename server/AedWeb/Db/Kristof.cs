//using DextraLib.Oracle;
using Dextra.Database;
using Dextra.Toolsspace;
using DextraLib.GeneralDao;
using DextraLib.MsSql;
using System;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Kristof = SqlBuilder<Kristof>.Build();


    }

    public class Kristof
    {
        public decimal Id_Kristof { get; set; }
        public String Neve { get; set; }
        public String Megjegyzes { get; set; }
        public DateTime Datum { get; set; }


        public Kristof()
        {
        }
        public Kristof ShallowCopy()
        {
            return (Kristof)this.MemberwiseClone();
        }
        public Kristof(HttpRequestBase rRequest)
        {
            var type = this.GetType();
            foreach (string key in rRequest.Form.Keys)
            {
                var property = type.GetProperty(key);
                if (property != null)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(rRequest.Form[key], property.PropertyType);
                        property.SetValue(this, convertedValue);
                    }
                    catch
                    {
                    }
                }
            }
        }

    }
}