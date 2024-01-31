using Dextra.Database;
using DextraLib.GeneralDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Agreement_Pays = SqlBuilder<Agreement_Pays>.Build();
    }


    public class Agreement_Pays : Commonfields
    {
       public decimal? Id_Agreement_Pays { get; set; }
        public decimal Id_Agreement { get; set; }
        public decimal Id_Persons { get; set; }
        public decimal? Id_Flows { get; set; }
        public string Root { get; set; }
        public string Itemtype { get; set; }
        public string Elementtype { get; set; }
        public string Complextype { get; set; }
        public string Claim { get; set; }
        public string Feetype { get; set; }
        public decimal? Feebaseamount { get; set; }
        public decimal? Feefactor { get; set; }
        public decimal? Minfeefactor { get; set; }
        public decimal? Maxfeefator { get; set; }
        public decimal? Classificationfeeamount { get; set; }
        public decimal? Biaspercent { get; set; }
        public decimal? Basefeeamount { get; set; }
        public decimal? Extendtominimalfeeamount { get; set; }
        public decimal? Hsztcutamount { get; set; }
        public decimal? Considerationfeeamount { get; set; }
        public string Feecomment { get; set; }
        public string Xmldata { get; set; }
        public string Description { get; set; }

        public Agreement_Pays()
        {
        }

        public Agreement_Pays(HttpRequestBase rRequest)
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


        public Agreement_Pays(decimal? id_agreement_pays, decimal id_agreement, string root, string elementtype, string xmldata)
        {

            Id_Agreement_Pays = id_agreement_pays;
            Id_Agreement = id_agreement;
            Itemtype = root;
            Elementtype = elementtype;
            Xmldata = xmldata;

        }


    }
}