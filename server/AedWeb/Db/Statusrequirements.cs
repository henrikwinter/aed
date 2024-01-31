using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xapp.Db;
// svn test

namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Statusrequirements = SqlBuilder<Statusrequirements>.Build();
    }
    public class Statusrequirements : Commonfields
    {
        public decimal? Id_Statusrequirements { get; set; }
        public decimal? Id_Organization { get; set; }
        public decimal Id_Flows { get; set; }
        public decimal? Cpyid { get; set; }
        public string Recordtype { get; set; }
        public string Itemtype { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Xmldata { get; set; }
        public Statusrequirements()
        {
        }
        public Statusrequirements ShallowCopy()
        {
            return (Statusrequirements)this.MemberwiseClone();
        }
        public Statusrequirements(HttpRequestBase rRequest)
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