using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xapp.Db
{
    public static partial class Sqlstore
    {
        public static SqlStrings Vw_Users = SqlBuilder<Vw_Users>.Build();
        public static SqlStrings Users = SqlBuilder<Users>.Build();

    }



    public class Vw_Users
    {
        public string Id { get; set; }
        public decimal Id_Vw_Users { get; set; }
        public string Username { get; set; }
        public string Usedname { get; set; }
        public string Email { get; set; }
        public string Preferedlang { get; set; }
        public string Userpreferences { get; set; }
        public Vw_Users()
        {
        }
        public Vw_Users ShallowCopy()
        {
            return (Vw_Users)this.MemberwiseClone();
        }
        public Vw_Users(HttpRequestBase rRequest)
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
    public class Users
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Users()
        {

        }
        public Users(string iId,  string iUsername, string iEmail)
        {

            Id = iId;
            Username = iUsername;
            Email = iEmail;
        }
        public Users(HttpRequestBase rRequest)
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