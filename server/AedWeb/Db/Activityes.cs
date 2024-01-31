using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dextra.Database;
using DextraLib.GeneralDao;
using Xapp.Db;



namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Activityes = SqlBuilder<Activityes>.Build();
    }

    public class Activityes : Commonfields
    {

        public decimal? Id_Activityes { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal? Id_Persons { get; set; }
        public string Description { get; set; }

        public string Recordtype { get; set; }
        public string Xmldata { get; set; }
        public string Roledescription { get; set; }
        public string Category { get; set; }
        public string Itemtype { get; set; }
        public string Alevel { get; set; }
        public string Rank { get; set; }
        public string Score { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Ready { get; set; }
        public decimal? Conductedyear { get; set; }
        public decimal? Toexpert { get; set; }
        public string Invokedlist { get; set; }


        public Activityes()
        {
            Id_Activityes = 0;
        }

        public Persons ShallowCopy()
        {
            return (Persons)this.MemberwiseClone();
        }

        public Activityes(HttpRequestBase rRequest)
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