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
        public static SqlStrings Checklistelement = SqlBuilder<Checklistelement>.Build();
    }

    public class Checklistelement : Commonfields
    {

        public decimal? Id_Checklistelement { get; set; }
        public decimal Id_Organization { get; set; }
        public decimal Id_Flows { get; set; }
        public decimal? Cpyid { get; set; }
        public string Recordtype { get; set; }

        //  [DbColumn("Elementtype", ExtraInfo = "{0}", ExtraInfoBind = "{0}")]
        public string Itemtype { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Xmldata { get; set; }



        public Checklistelement()
        {
            Id_Checklistelement = 0;
        }

        public Checklistelement ShallowCopy()
        {
            return (Checklistelement)this.MemberwiseClone();
        }

        public Checklistelement(HttpRequestBase rRequest)
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


        public Checklistelement(decimal id_checklistelement, decimal id_organization, string name, string description, string xmldata)
        {
            Id_Checklistelement = id_checklistelement;
            Id_Organization = id_organization;
            Name = name;
            Description = description;
            Xmldata = xmldata;
        }


    }

}