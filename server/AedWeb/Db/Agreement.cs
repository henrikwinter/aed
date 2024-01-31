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
        public static SqlStrings Agreement = SqlBuilder<Agreement>.Build();
    }


    public class Agreement : Commonfields
    {
        public decimal? Id_Agreement { get; set; }
        public decimal? Id_Persons { get; set; }

        public decimal? Id_Flows { get; set; }
        public string Bid_Agreement { get; set; }
        public decimal? Id_Mxrf_Persons_Org { get; set; }
        //public string Root { get; set; }
        public string Itemtype { get; set; }
        public string Agreementtype { get; set; }
        public string Xmldata { get; set; }
        public string Description { get; set; }

        public Agreement()
        {
        }

        public Agreement(HttpRequestBase rRequest)
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


        public Agreement(decimal? id_agreement, decimal id_persons, string bid_Agreement, decimal? id_mxrf_persons_org, string root, string agreementtype, string xmldata)
        {
            Id_Agreement = id_agreement;
            Id_Persons = id_persons;
            Bid_Agreement = bid_Agreement;
            Id_Mxrf_Persons_Org = id_mxrf_persons_org;
            Itemtype = root;
            Agreementtype = agreementtype;
            Xmldata = xmldata;
        }

    }
}