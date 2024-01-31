using Dextra.Database;
using DextraLib.GeneralDao;
using DextraLib.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
namespace Xapp.Db
{

    public static partial class Sqlstore
    {
        public static SqlStrings Vw_User_Orggroup = SqlBuilder<Vw_User_Orggroup>.Build();

    }
    public class Vw_User_Orggroup
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Usedname { get; set; }
        public string Groupname { get; set; }
        public decimal Groupvalue { get; set; }
        public string Flag { get; set; }
        public Vw_User_Orggroup()
        {

        }
        public Vw_User_Orggroup(string iId, string iUsername, string iUsedname, string iGroupname, decimal iGroupvalue, string iFlag)
        {

            Id = iId;
            Username = iUsername;
            Usedname = iUsedname;
            Groupname = iGroupname;
            Groupvalue = iGroupvalue;
            Flag = iFlag;
        }
    }
}