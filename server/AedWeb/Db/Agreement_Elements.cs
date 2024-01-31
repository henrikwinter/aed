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
        public static SqlStrings Agreement_Elements = SqlBuilder<Agreement_Elements>.Build();
    }


    public class Agreement_Elements : Commonfields
    {

        public decimal? Id_Agreement_Elements { get; set; }
        public decimal Id_Agreement { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal Id_Persons { get; set; }

        public string Root { get; set; }
        public string Itemtype { get; set; }
        public string Elementtype { get; set; }
        public string Complextype { get; set; }
        public string Xmldata { get; set; }
        public string Description { get; set; }

        public Agreement_Elements()
        {
        }

        public Agreement_Elements(HttpRequestBase rRequest)
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


        public Agreement_Elements(decimal? id_agreement_elements, decimal id_agreement, string root, string elementtype, string xmldata)
        {

            Id_Agreement_Elements = id_agreement_elements;
            Id_Agreement = id_agreement;
            Itemtype = root;
            Elementtype = elementtype;
            Xmldata = xmldata;

        }


    }
}