using DextraLib.GeneralDao;
using Dextra.Database.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Project = SqlBuilder<Project>.Build();
    }

    public class Project
    {
        [IdentyKeyAttribute]
        public decimal Id_Project { get; set; }
        public decimal? Id_Parent { get; set; }
        public string Itemtype { get; set; }
        public string Name { get; set; }
        public string Xmldata { get; set; }
        public decimal? Id_Ord { get; set; }
        public string Properties { get; set; }
        public Project()
        {
        }
        public Project ShallowCopy()
        {
            return (Project)this.MemberwiseClone();
        }
        public Project(HttpRequestBase rRequest)
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


    /*
        public class Project
        {
            [IdentyKeyAttribute]
            public decimal Id_Project { get; set; }
            public decimal? Id_Parent { get; set; }
            public string Itemtype { get; set; }
            public string Name { get; set; }
            public string Xmldata { get; set; }
            public decimal? Id_Ord { get; set; }
            public string Properties { get; set; }
            public Project()
            {
            }
            public Project ShallowCopy()
            {
                return (Project)this.MemberwiseClone();
            }
            public Project(HttpRequestBase rRequest)
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
    */
    public class ProjectOrig
    {
        [IdentyKeyAttribute]
        public decimal? Id_Project { get; set; }
        public decimal? Id_Parent { get; set; }

        public string Itemtype { get; set; }
        public string Name { get; set; }
        public string Xmldata { get; set; }
        public decimal? Id_Ord { get; set; }
        public string Properties { get; set; }
        
        public ProjectOrig()
        {

        }
        public ProjectOrig ShallowCopy()
        {
            return (ProjectOrig)this.MemberwiseClone();
        }

        public ProjectOrig(HttpRequestBase rRequest)
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