using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dextra.Database;
using DextraLib.GeneralDao;



namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Qualifications = SqlBuilder<Qualifications>.Build();


    }


    public class Qualifications : Commonfields
    {
        public decimal? Id_Qualifications { get; set; }
        public decimal? Id_Persons { get; set; }
        public decimal? Id_Flows { get; set; }
        public string Description { get; set; }
        public string Recordtype { get; set; }
        public string Itemtype { get; set; }
        public string Xmldata { get; set; }
        public string Category { get; set; }
        public string Qlevel { get; set; }
        public string Rank { get; set; }
        public string Score { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Ready { get; set; }
        public decimal Conductedyear { get; set; }
        public decimal Toexpert { get; set; }
        public Qualifications()
        {
        }
        public Qualifications ShallowCopy()
        {
            return (Qualifications)this.MemberwiseClone();
        }
        public Qualifications(HttpRequestBase rRequest)
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