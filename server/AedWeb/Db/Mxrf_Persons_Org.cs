using Dextra.Database;
using DextraLib.GeneralDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xapp.Db;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Mxrf_Persons_Org = SqlBuilder<Mxrf_Persons_Org>.Build();
    }


    public class Mxrf_Persons_Org : Commonfields
    {
        public decimal? Id_Mxrf_Persons_Org { get; set; }
        public decimal? Id_Person { get; set; }
        public decimal? Cid_Person { get; set; }
        public decimal? Id_Organization { get; set; }
        public decimal? Cid_Organization { get; set; }
        public string Xmldata { get; set; }
        public string Root { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal? Cpyid { get; set; }
        public decimal? Cpyparrent { get; set; }

        public Mxrf_Persons_Org() { }
        public Mxrf_Persons_Org ShallowCopy()
        {
            return (Mxrf_Persons_Org)this.MemberwiseClone();
        }
        public Mxrf_Persons_Org(HttpRequestBase rRequest)
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


    };


}