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
        public static SqlStrings Dxxrefuserorggroup = SqlBuilder<Dxxrefuserorggroup>.Build();

    }

    public class Dxxrefuserorggroup
    {
        [PrimaryKeyAttribute]
        public decimal? Id { get; set; }
        public string Username { get; set; }
        public string Groupname { get; set; }
        public string Flag { get; set; }
        public Dxxrefuserorggroup()
        {
                
        }
        public Dxxrefuserorggroup(decimal? iId, string iUsername, string iGroupname, string iFlag)
        {

            Id = iId;
            Username = iUsername;
            Groupname = iGroupname;
            Flag = iFlag;
        }

    }
}