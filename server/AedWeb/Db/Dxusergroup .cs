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
        public static SqlStrings Dxusergroup = SqlBuilder<Dxusergroup>.Build();
        public static SqlStrings Dxroles = SqlBuilder<Dxroles>.Build();
        public static SqlStrings Dxorggroup = SqlBuilder<Dxorggroup>.Build();

    }


    public class Dxorggroup
    {

        [PrimaryKeyAttribute]
        public decimal Id { get; set; }
        public string Groupname { get; set; }
        public decimal Groupvalue { get; set; }
        public string Descript { get; set; }

        public Dxorggroup()
        {

        }
    }

    public class Dxusergroup
    {
        [PrimaryKeyAttribute]
        public decimal Id { get; set; }
        public string Groupname { get; set; }
        public Dxusergroup()
        {

        }
        public Dxusergroup(decimal iId, string iGroupname)
        {

            Id = iId;
            Groupname = iGroupname;
        }

    }


    public class Dxroles
    {
        [PrimaryKeyAttribute]
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Descript { get; set; }
        public Dxroles()
        {

        }
        public Dxroles(decimal iId, string iname)
        {

            Id = iId;
            Name = iname;
        }

    }


}