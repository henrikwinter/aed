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
        public static SqlStrings Dxxrefusergrouprole = SqlBuilder<Dxxrefusergrouprole>.Build();

    }

    public class Dxxrefusergrouprole
    {
        [PrimaryKeyAttribute]
        public decimal? Id { get; set; }
        public string Rolename { get; set; }
        public string Groupname { get; set; }
        public Dxxrefusergrouprole()
        {

        }
        public Dxxrefusergrouprole(decimal? iId, string iRolename, string iGroupname)
        {

            Id = iId;
            Rolename = iRolename;
            Groupname = iGroupname;
        }
    }
}