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
        public static SqlStrings Sy_Hupaszemely2 = SqlBuilder<Sy_Hupaszemely2>.Build();


    }

    public class Sy_Hupaszemely2
    {
        [PrimaryKeyAttribute]
        public decimal Szemely_Az { get; set; }
        public string Nev { get; set; }
        public string Lnev { get; set; }
        public decimal Szuletesidatum { get; set; }
        public string Szuletesihely { get; set; }
        public string Rendfokozat { get; set; }
        public decimal Rfokkod { get; set; }

        public string Szervnev { get; set; }
        public string Neme { get; set; }

        public string Xmldata { get; set; }

        public Sy_Hupaszemely2()
        {
        }
        public Sy_Hupaszemely2 ShallowCopy()
        {
            return (Sy_Hupaszemely2)this.MemberwiseClone();
        }
        public Sy_Hupaszemely2(HttpRequestBase rRequest)
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