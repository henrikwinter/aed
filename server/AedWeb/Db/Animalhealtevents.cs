using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.MsSql;
using System;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Animalhealtevents = SqlBuilder<Animalhealtevents>.Build();


    }
    public class Animalhealtevents : Commonfields
    {


        public decimal? Id_Animalhealtevents { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal? Id_Animal { get; set; }
       // public decimal Cid_Persons { get; set; }

        public string Description { get; set; }
        public string Recordtype { get; set; }
        public string Xmldata { get; set; }
        public string Category { get; set; }
        public string Itemtype { get; set; }
        public string Olevel { get; set; }
        public string Rank { get; set; }
        public string Score { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Ready { get; set; }
        public decimal? Conductedyear { get; set; }
       // public string Root { get; set; }
        //public string Complextype { get; set; }

        

        public Animalhealtevents()
        {
            Id_Animalhealtevents = 0;
        }

        public Othersdata ShallowCopy()
        {
            return (Othersdata)this.MemberwiseClone();
        }

        public Animalhealtevents(HttpRequestBase rRequest)
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