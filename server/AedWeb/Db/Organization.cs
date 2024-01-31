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
        public static SqlStrings Organization = SqlBuilder<Organization>.Build();
    }


    public class Status : Organizationclient
    {

    }

    public class Organizationclient
    {
        
        public decimal? Id_Organization { get; set; }
        public decimal? Id_Parent { get; set; }
        public decimal? Id_Ord { get; set; }
        public string Bid_Organization { get; set; }
        public decimal? Id_Persons { get; set; }
        public string Usedname { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Shortname { get; set; }
        public string Orgtype { get; set; }
        public string Recordtype { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal? Cpyid { get; set; }
        public decimal? Cpyparrent { get; set; }
        public decimal Id_Files { get; set; }
        public decimal Id_Scopeofactivity { get; set; }
        public string Status { get; set; }
        public string Property { get; set; }
        public Organizationclient()
        {

        }

        public Organizationclient( Organization o)
        {
            Id_Organization = o.Id_Organization;
            Id_Parent = o.Id_Parent;
            Id_Ord = o.Id_Ord;
            Bid_Organization = o.Bid_Organization;
            Name = o.Name;
            Title = o.Title;
            Shortname = o.Shortname;
            Recordtype = o.Recordtype;
            Id_Flows = o.Id_Flows;
            Cpyid = o.Cpyid;
            Cpyparrent = o.Cpyparrent;
            Id_Files = o.Id_Files;
            Status = o.Status;
            Property = o.Property;

        }
    }
    public class Organization : Commonfields
    {
        public decimal? Id_Organization { get; set; }
        public decimal? Id_Parent { get; set; }
        public decimal? Id_Ord { get; set; }
        public string Bid_Organization { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Shortname { get; set; }
        public string Xmldata { get; set; }
        public string Recordtype { get; set; }
        public decimal? Id_Flows { get; set; }
        public decimal? Cpyid { get; set; }
        public decimal? Cpyparrent { get; set; }
        public decimal Id_Files { get; set; }
        public decimal Id_Scopeofactivity { get; set; }
        public Organization()
        {
        }
        public Organization ShallowCopy()
        {
            return (Organization)this.MemberwiseClone();
        }
        public Organization(HttpRequestBase rRequest)
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