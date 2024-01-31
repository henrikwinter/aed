using Dextra.Common;
using Dextra.Report;
using Dextra.Xforms;
using DextraLib.GeneralDao;
using DextraLib.Oracle;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xapp.Db;
using System.Web;
using Xapp;

namespace Dextra.Database
{

    public class DbSysDate
    {
        public DateTime Sysdbdate { get; set; }
        public DateTime Sysdate { get; set; }
        public long Offset { get; set; }

        public DateTime GetDbSydate()
        {
            DateTime ret = DateTime.Now;
            ret.AddTicks(Offset);
            return ret;
        }

        public DbSysDate(DbContext Dbc)
        {
            Sysdate = DateTime.Now;
            Dao dated = new Dao(Dbc);
            dated.SqlSelect("select sysdate from dual", new object[] { });
            if (dated.Result.GetSate(0))
            {
                Sysdbdate = dated.DynResult.Sysdate;
            }
            else
            {
                Sysdbdate = DateTime.Now;
            }
            Offset = Sysdbdate.Ticks - Sysdate.Ticks;
            Dbc.Dbsysdate = this;

        }

        public static explicit operator DateTime(DbSysDate v)
        {
            throw new NotImplementedException();
        }
    }

    public class ProcParam : OraProcParam
    {
        public ProcParam() : base()
        {

        }

        public ProcParam(string name, object value, string dir = "I") : base(name, value, dir)
        {

        }

    }

    public class Sqlstring
    {
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }

        public Sqlstring()
        {

        }

        public Sqlstring(string select, string insert, string update)
        {
            Select = select;
            Insert = insert;
            Update = update;
        }
    }
    public static class SqlBuilder<T>
    {
        public static SqlStrings Build(string tablename=null)
        {
            return OraDaoHelper.WriteColumnMappings<T>(tablename);
        }
      
        public static Sqlstring Get()
        {
            Type t = typeof(Sqlstore);
            FieldInfo field = t.GetFields(BindingFlags.Static | BindingFlags.Public).ToList().FirstOrDefault(p => p.Name == typeof(T).Name);
            SqlStrings sqls = (SqlStrings)field.GetValue(null);
            Sqlstring ret = new Sqlstring(sqls.Select,sqls.Insert, sqls.Update);
            return ret;
        }
        public static SqlStrings GetAll()
        {
            Type t = typeof(Sqlstore);
            FieldInfo field = t.GetFields(BindingFlags.Static | BindingFlags.Public).ToList().FirstOrDefault(p => p.Name == typeof(T).Name);
            SqlStrings sqls = (SqlStrings)field.GetValue(null);
           
            return sqls;
        }

    }

    public class DbContext : OraDbContext
    {
        public void AddGeneralVariable(string name,object value)
        {
            if (GeneralVariables.ContainsKey(name))
            {
                GeneralVariables[name]= value;
            } else
            {
                GeneralVariables.Add(name, value);
            }
        }
        public void RemoveGeneralVariable(string name)
        {
            try
            {
                GeneralVariables.Remove(name);
            } catch { }
        }

        public void AddRowSelectFilters(string name, string value)
        {
            if (RowSelectFilters.ContainsKey(name))
            {
                RowSelectFilters[name] = value;
            } else
            {
                RowSelectFilters.Add(name, value);
            }

        }

        public DbSysDate Dbsysdate { get; set; }

        public string CurrentLang { get; set; }

        public void RemoveRowSelectFilters(string name)
        {
            try
            {
                RowSelectFilters.Remove(name);
            }
            catch { }
        }

        public DbContext()
        {

        }


        public DbContext(string connectionstring, DbSysDate dbdate=null) : base(connectionstring)
        {

            Dbsysdate = dbdate;
            GeneralVariables = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) {
                    { "userid", "Gad" },
                    { "dtfrom", new ExplicitParameter(typeof(DateTime)) },
                    { "dtto", new ExplicitParameter(null, typeof(DateTime)) },
                    { "lastid", null }
            };
            RowSelectFilters.Add(
                    "default",
                    @"TRUNC(Datavalidfrom)<= trunc(NVL(:dtfrom, SYSDATE)) and (Datavalidto is null or TRUNC(Datavalidto)>trunc(NVL(:dtfrom, SYSDATE))) and ( CheckUserorggroup(:userid,Orggroup) >0  or Creator=:userid)");
            RowSelectFilters.Add(
                    "OwnJoj",
                    @"TRUNC(T.Datavalidfrom)<= trunc(NVL(:dtfrom, SYSDATE)) and (T.Datavalidto is null or TRUNC(T.Datavalidto)>trunc(NVL(:dtfrom, SYSDATE))) and ( CheckUserorggroup(:userid,T.Orggroup) >0  or T.Creator=:userid)");
            RowSelectFilters.Add(
                    "USGr",
                    @" ( CheckUserorggroup(:userid,T.Orggroup) >0  or T.Creator=:userid) ");
            RowSelectFilters.Add(
                    "Hupa",
                    @" ( 1=1 ) ");


        }

    }

    public class Hierarchy<T>
    {

        public Dao<T> hdao { get; set; }
        public List<T> Hresult { get; set; }
        public PropertyInfo parentcolprop { get; set; }
        public PropertyInfo pkcolprop { get; set; }

        public object GetHierarchy(decimal startid, string parentname,string filter=null)
        {
            parentcolprop = hdao.Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(parentname));
            string pk = SqlBuilder<T>.GetAll().PrKeyColumns;
            pkcolprop = hdao.Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(pk));
            string sql = "";
            if (!string.IsNullOrEmpty(filter))
            {
                sql = SqlBuilder<T>.GetAll().Select + " where " + SqlBuilder<T>.GetAll().PrKeyColumns + "=:0  "+filter +" order by Id_Ord";
            } else
            {
                sql = SqlBuilder<T>.GetAll().Select + " where " + SqlBuilder<T>.GetAll().PrKeyColumns + "=:0  order by Id_Ord";
            }

            hdao.ExecuteReaderToClass(sql, new object[] { startid });
            T r = hdao.Result.GetFirst<T>();
            Hresult.Add(r);
            HWalk(r,filter);
            return Hresult;
        }

        void HWalk(T item,string filter=null)
        {

            object id = pkcolprop.GetValue(item, null);
            string sql = "";
            if (!string.IsNullOrEmpty(filter))
            {
                sql = SqlBuilder<T>.GetAll().Select + " where " + parentcolprop.Name + "=:0  " + filter + " order by Id_Ord";
            }
            else
            {
                sql = SqlBuilder<T>.GetAll().Select + " where " + parentcolprop.Name + "=:0 order by Id_Ord";
            }

            
            hdao.SqlSelect(sql, new object[] { id });
            List<T> temp = hdao.Result.GetRes<T>();
            Hresult.AddRange(temp);
            foreach (T rec in temp)
            {
                HWalk(rec,filter);
            }
        }

        public Hierarchy(DbContext db)
        {
            hdao = new Dao<T>(db);
            Hresult = new List<T>();
        }
    }
    
    public class Dao : OracleDataAccess, IDisposable
    {
        public object SqlSelect(string sql, object[] par)
        {
            sql = OraDaoHelper.ConvertStairsToColumns(sql, Dbcontext.Connection);
            return base.ExecuteReader(sql, par);
        }

        public object SqlUpdate(string sql, object[] par)
        {
            return base.ExecuteNonQuery(sql, par);
        }

        public Dao(DbContext db)
                 : base(db)    
        {

        }


        #region dispose
        bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Dao()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {

            }
            _disposed = true;
        }
        #endregion
    }

    public class Dao<T> : OracleDataAccess, IDisposable  
    {

        public T Entity { get; set; }
        public decimal? PkVal { get; set; }
        public AjaxResultCode Error { get; set; }

        public Xform Entxform;

        PropertyInfo Pk;
        PropertyInfo Xmldata;

        public void SetEntityFromPost(System.Collections.Specialized.NameValueCollection PostVal)
        {
            Type t = Entity.GetType();
            Type proptype = null;
            System.TypeCode typeCode;

            foreach (PropertyInfo info in t.GetProperties())
            {
                if (info.CanWrite)
                {
                    string value = null;
                    try
                    {
                        if (PostVal.AllKeys.Contains(info.Name))
                        {
                            value = PostVal[info.Name];
                            if (Nullable.GetUnderlyingType(info.PropertyType) != null)
                            {
                                proptype = Nullable.GetUnderlyingType(info.PropertyType);
                            }
                            else
                            {
                                proptype = info.PropertyType;
                            }
                            typeCode = Type.GetTypeCode(proptype);

                            try
                            {
                                switch (typeCode)
                                {
                                    case TypeCode.Decimal:
                                        info.SetValue(Entity, decimal.Parse(value), null);
                                        break;
                                    case TypeCode.Int32:
                                        info.SetValue(Entity, int.Parse(value), null);
                                        break;
                                    case TypeCode.String:
                                        info.SetValue(Entity, value, null);
                                        break;
                                    case TypeCode.DateTime:
                                        try
                                        {
                                            DateTime testdate;
                                            if (string.IsNullOrEmpty(value))
                                            {
                                                info.SetValue(Entity, null, null);
                                            }
                                            else
                                            {
                                                testdate = DateTime.ParseExact(value, "yyyy.MM.dd.", CultureInfo.InvariantCulture);
                                                info.SetValue(Entity, testdate, null);
                                            }
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                DateTime testdate;
                                                testdate = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                                info.SetValue(Entity, testdate, null);
                                            }
                                            catch { }
                                        }
                                        break;
                                    case TypeCode.Boolean:
                                        info.SetValue(Entity, Boolean.Parse(value), null);
                                        break;
                                    default:
                                        info.SetValue(Entity, value, null);
                                        break;
                                }

                            }
                            catch { }

                        }
                    }
                    catch { }

                }
            }
        }



        public Dao(DbContext db) : base(db)
        {
            typeParameterType = typeof(T);
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            Entxform = new Xform();

        }

        public Dao(DbContext db, System.Collections.Specialized.NameValueCollection PostVal) : base(db) 
        {
            typeParameterType = typeof(T);
            Error = new AjaxResultCode();
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            Xmldata = Entity.GetType().GetProperty("Xmldata");
            Pk = Entity.GetType().GetProperty(SqlBuilder<T>.GetAll().PrKeyColumns);

            try { PkVal = toDecimal(PostVal[SqlBuilder<T>.GetAll().PrKeyColumns]); } catch { PkVal = null; }

                SqlSelectId(PkVal);
                if (Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity =Result.GetFirst<T>();
                }
            
            Entxform = new Xform(PostVal);
            Entxform.SetEntityClassFromElemnetValue(Entity);
            SetEntityFromPost(PostVal);
            Xmldata.SetValue(Entity, Entxform.GetXmlStringFromXform(), null);
        }

        public Dao(DbContext db, System.Collections.Specialized.NameValueCollection PostVal, decimal? Updateid) : base(db)
        {
            typeParameterType = typeof(T);
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            Xmldata = Entity.GetType().GetProperty("Xmldata");
            SqlSelectId(Updateid);
            if (Result.GetSate(DaoResult.ResCountOne))
            {
                Entity = Result.GetFirst<T>();
                Entxform = new Xform(PostVal);
                Entxform.SetEntityClassFromElemnetValue(Entity);
                SetEntityFromPost(PostVal);
                Xmldata.SetValue(Entity, Entxform.GetXmlStringFromXform(), null);
                //SqlUpdate(Entity);
            }
        }



        public object SqlSelect(string sql, object[] par, string cmd = "")
        {
            return base.ExecuteReaderToClass(sql, par);
        }

        public object SqlSelectId(object id)
        {
            string sql = SqlBuilder<T>.GetAll().Select + " where " + SqlBuilder<T>.GetAll().PrKeyColumns + "=:0 ";
            return base.ExecuteReaderToClass(sql, new object[] { id });
        }

        public object SqlInsert(T e)
        {
            string lsql =  SqlBuilder<T>.Get().Insert;  
            e = (T)ExecuteNonQueryFromClass(lsql, e);
            return e;
        }
        public object SqlUpdate(T e,bool rowid=false)
        {
            string lsql = SqlBuilder<T>.GetAll().UpdatePk;
            if (rowid)
            {
                lsql += " and ora_rowscn=:Xrowid ";
            }
            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }
        public object SqlUpdate(T e, string Where, object[] par, string cmd = "Update")
        {
            string lsql = SqlBuilder<T>.GetAll().Update;
            lsql = Regex.Replace(lsql, "WHERE", "", RegexOptions.IgnoreCase);
            lsql = lsql + " where " + Regex.Replace(Where, "WHERE", "", RegexOptions.IgnoreCase);

            ExecuteNonQueryFromClass(lsql, e, par);
            return Result;
        }

        public object SqlDelete(T e)
        {
            string lsql = SqlBuilder<T>.GetAll().DeletePk;
            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }
        public object SqlDelete(T e, string Where, object[] par)
        {
            string lsql = SqlBuilder<T>.GetAll().Delete;
            lsql = Regex.Replace(lsql, "WHERE", "", RegexOptions.IgnoreCase);
            lsql = lsql + " where " + Regex.Replace(Where, "WHERE", "", RegexOptions.IgnoreCase);

            ExecuteNonQueryFromClass(lsql, e);
            return Result;
        }



        public string MoveRecord(int id, int newpid, string drp, string foreginkeyname,string reorderproc, decimal? xrowid = null)
        {

            string ret = "";
            PropertyInfo pkkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(SqlBuilder<T>.GetAll().PrKeyColumns));
            PropertyInfo foreginkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(foreginkeyname));
            PropertyInfo idordprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Id_Ord"));
            PropertyInfo ixrowprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Xrowid"));


            try
            {
                T t = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T t1 = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T p = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                SqlSelectId(id);
                if (Result.GetSate(1))
                {
                    t = Result.GetFirst<T>();
                    if (ixrowprop != null)
                    {
                        decimal? x = (decimal)ixrowprop.GetValue(t, null);
                        if (xrowid != null && x != xrowid)
                        {
                            return "ERR_CONCURENCY";
                        }
                    }
                }
                ClearResult();
                SqlSelectId(newpid);
                if (Result.GetSate(1))
                {
                    t1 = Result.GetFirst<T>();
                }
                decimal? reorderparrent = null;
                try
                {
                    reorderparrent = (decimal)foreginkeyprop.GetValue(t1, null);   //   t1.Id_Parent;
                }
                catch { }
                if (drp.Equals("inside"))
                {
                    foreginkeyprop.SetValue(t, pkkeyprop.GetValue(t1, null));
                    //t.Id_Parent = (decimal)t1.Id_Organization;
                    try { reorderparrent = (decimal)foreginkeyprop.GetValue(t, null); } catch { } //t.Id_Parent;

                }
                else
                {
                    foreginkeyprop.SetValue(t, foreginkeyprop.GetValue(t1, null));
                    //t.Id_Parent = t1.Id_Parent;
                }

                if (drp.Equals("before"))
                {
                    idordprop.SetValue(t, (decimal)idordprop.GetValue(t1, null) - 0.1m);
                    //t.Id_Ord = t1.Id_Ord - 0.1m;
                }
                else if (drp.Equals("after"))
                {
                    idordprop.SetValue(t, (decimal)idordprop.GetValue(t1, null) + 0.1m);
                    //t.Id_Ord = t1.Id_Ord + 0.1m;
                }
                else
                {
                    idordprop.SetValue(t, (decimal)99999, null);
                    //t.Id_Ord = 999999;
                }
                ClearResult();
                SqlUpdate(t);

                List<OraProcParam> param = new List<OraProcParam>();
                param.Add(new OraProcParam("p", reorderparrent));
                //SqlProcedure("reorder", ref param, this.Dbcontext);
                SqlProcedure(reorderproc, ref param, this.Dbcontext); 
                ret = "Ok";
            }
            catch (Exception e)
            {
                ret = "Error";
            }



            return ret;
        }

        public void ReorderOrg(decimal? reorderparrent,string orgtype)
        {
            List<OraProcParam> param = new List<OraProcParam>();
            param.Add(new OraProcParam("p", reorderparrent));
            param.Add(new OraProcParam("rectype", orgtype));
            SqlProcedure("reorder", ref param, this.Dbcontext);

        }


        public decimal? MoveRecordNew(int id, int newpid, string drp, string foreginkeyname,  decimal? xrowid = null)
        {
            decimal? reorderparrent = null;
            string ret = "";
            PropertyInfo pkkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(SqlBuilder<T>.GetAll().PrKeyColumns));
            PropertyInfo foreginkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(foreginkeyname));
            PropertyInfo idordprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Id_Ord"));
            PropertyInfo ixrowprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Xrowid"));


            try
            {
                T t = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T t1 = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T p = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                SqlSelectId(id);
                if (Result.GetSate(1))
                {
                    t = Result.GetFirst<T>();
                    if (ixrowprop != null)
                    {
                        decimal? x = (decimal)ixrowprop.GetValue(t, null);
                        if (xrowid != null && x != xrowid)
                        {
                            return -1;// "ERR_CONCURENCY";
                        }
                    }
                }
                ClearResult();
                SqlSelectId(newpid);
                if (Result.GetSate(1))
                {
                    t1 = Result.GetFirst<T>();
                }
                //decimal? reorderparrent = null;
                try
                {
                    reorderparrent = (decimal)foreginkeyprop.GetValue(t1, null);   //   t1.Id_Parent;
                }
                catch { }
                if (drp.Equals("inside"))
                {
                    foreginkeyprop.SetValue(t, pkkeyprop.GetValue(t1, null));
                    //t.Id_Parent = (decimal)t1.Id_Organization;
                    try { reorderparrent = (decimal)foreginkeyprop.GetValue(t, null); } catch { } //t.Id_Parent;

                }
                else
                {
                    foreginkeyprop.SetValue(t, foreginkeyprop.GetValue(t1, null));
                    //t.Id_Parent = t1.Id_Parent;
                }

                if (drp.Equals("before"))
                {
                    idordprop.SetValue(t, (decimal)idordprop.GetValue(t1, null) - 0.1m);
                    //t.Id_Ord = t1.Id_Ord - 0.1m;
                }
                else if (drp.Equals("after"))
                {
                    idordprop.SetValue(t, (decimal)idordprop.GetValue(t1, null) + 0.1m);
                    //t.Id_Ord = t1.Id_Ord + 0.1m;
                }
                else
                {
                    idordprop.SetValue(t, (decimal)99999, null);
                    //t.Id_Ord = 999999;
                }
                ClearResult();
                SqlUpdate(t);

                //List<OraProcParam> param = new List<OraProcParam>();
                //param.Add(new OraProcParam("p", reorderparrent));
                //SqlProcedure("reorder", ref param, this.Dbcontext);
                //SqlProcedure(reorderproc, ref param, this.Dbcontext);
                //ret = "Ok";
            }
            catch (Exception e)
            {
                //ret =  "Error";
                return -1;
            }


            return reorderparrent;
            //return ret;
        }





        public void ClearResult()
        {
            Result.Clear();
        }


        private decimal toDecimal(string value)
        {
            decimal ret = 0;
            try
            {
                ret = decimal.Parse(value);
            }
            catch
            {
                Error.Errorcode = -10;
            }
            return ret;
        }
        private decimal? toNDecimal(string value)
        {
            decimal? ret = null;
            try
            {
                ret = decimal.Parse(value);
            }
            catch
            {
                Error.Errorcode = -10;
            }
            return ret;
        }


        #region dispose
        bool _disposed;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Dao()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {

            }
            _disposed = true;
        }
        #endregion
    }

    public class XformProcess<T>
    {
        public T Entity { get; set; }
        public Xform xform = new Xform();

        public object Id_Entity { get; set; }
        public object Id_Parent { get; set; }
        public string Id_Xform { get; set; }

        public string Curlang { get; set; }


        public PropertyInfo pkkeyprop { get; set; }
        public PropertyInfo foreginkeyprop { get; set; }
        public PropertyInfo xmlcolprop { get; set; }

        public PropertyInfo datavalidfromcolprop { get; set; }
        public PropertyInfo attributumcolprop { get; set; }

        public PropertyInfo xrowidprop { get; set; }
        public MethodInfo InitCommonFieldsForAdd { get; set; }

        public MethodInfo CloseCommonFields { get; set; }

        Dextra.Database.DbSysDate curDate = null;

        DbContext dbcontext = null;

        public Dextra.Database.Dao<T> dao = null;

        public bool Loaded = false;

        public AjaxResultCode Error = new AjaxResultCode();

        public XformProcess()
        {
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
        }

        void Init(Dextra.Database.DbContext Sdb, string XmlColname = "Xmldata")
        {
            dbcontext = Sdb;
            curDate = Sdb.Dbsysdate;
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            try
            {
                dao = new Dextra.Database.Dao<T>(Sdb);
                string prkey = ((DextraLib.GeneralDao.SqlStrings)(typeof(Xapp.Db.Sqlstore).GetField(typeof(T).Name).GetValue(dao))).PrKeyColumns; 
                //string prkey = ((DextraLib.GeneralDao.SqlStrings)(typeof(NewApp.Db.Sqlstore).GetField(typeof(T).Name).GetValue(dao))).PkeyPropName;  //???
                pkkeyprop = Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(prkey));
                xmlcolprop = Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(XmlColname));
                try { xrowidprop = Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert("Xrowid")); } catch { }
                InitCommonFieldsForAdd = typeof(T).GetMethod("InitCommonFieldsForAdd");
                CloseCommonFields = typeof(T).GetMethod("CloseCommonFields");
            }
            catch { }

        }
        public XformProcess(Dextra.Database.DbContext Sdb, string XmlColname = "Xmldata",string curlang="hu")
        {
            //Curlang = curlang;
            Curlang = Sdb.CurrentLang;
            dao = new Dextra.Database.Dao<T>(Sdb);
            Init(Sdb, XmlColname);
        }

        public void FakeXformProcess(HttpRequestBase PostVal)
        {
            //Init(Sdb);
            if (PostVal["Id_Entity"] != null) Id_Entity = toDecimal(PostVal["Id_Entity"]);
            try { if (PostVal[pkkeyprop.Name] != null) Id_Entity = toDecimal(PostVal[pkkeyprop.Name]); } catch { }
            if (PostVal["Id_Parent"] != null) Id_Parent = toDecimal(PostVal["Id_Parent"]);


   
            if (Id_Entity != null)
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity = dao.Result.GetFirst<T>();
                    Loaded = true;
                }
                else Loaded = false;
            }
            Type[] ArgumentTypes = new Type[] { typeof(HttpRequestBase) };
            ConstructorInfo Constructor = typeof(T).GetConstructor(ArgumentTypes);
            Entity = (T)Constructor.Invoke(new object[] { PostVal });

        }

        // Post ból keres "Id_Entity" és entity->PkName-t vagy Xfromból->   PkName-t és "Id_Parent"-et és "Id_Xform"-ot
        public XformProcess(Dextra.Database.DbContext Sdb, System.Collections.Specialized.NameValueCollection PostVal, string XmlColname = "Xmldata", string curlang = "hu")
        {
            //Curlang = curlang;
            Curlang = Sdb.CurrentLang;
            Init(Sdb, XmlColname);
            string XRowId = null;
            if (PostVal["XRowId"] != null) XRowId = PostVal["XRowId"];
            if (PostVal["Id_Xform"] != null) Id_Xform = PostVal["Id_Xform"];
            if (PostVal["Id_Entity"] != null) Id_Entity = toDecimal(PostVal["Id_Entity"]);
            try { if (PostVal[pkkeyprop.Name] != null) Id_Entity = toDecimal(PostVal[pkkeyprop.Name]); } catch { }
            if (PostVal["Id_Parent"] != null) Id_Parent = toDecimal(PostVal["Id_Parent"]);
            string selector = string.IsNullOrEmpty(PostVal["selector"]) ? "Gschema" : PostVal["selector"];

            if (Id_Entity != null)
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity = dao.Result.GetFirst<T>();
                    Loaded = true;
                }
                else Loaded = false;
            }

            xform = new Xform(PostVal,selector);
            xform.XRowId = XRowId;
            xform.SetEntityClassFromElemnetValue(Entity);
            try { xrowidprop.SetValue(Entity,decimal.Parse(XRowId), null); } catch { }
            try { if (pkkeyprop.GetValue(Entity, null) != null) Id_Entity = pkkeyprop.GetValue(Entity, null); } catch { }

            string xml = xform.GetXmlStringFromXform();
            if(xmlcolprop!=null) xmlcolprop.SetValue(Entity, xml, null);
        }
        public AjaxResultCode Insert()
        {
            pkkeyprop.SetValue(Entity, null, null);
            if (InitCommonFieldsForAdd != null) InitCommonFieldsForAdd.Invoke(Entity, new object[] { dbcontext });
            dao.SqlInsert(Entity);
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            else
            {
                pkkeyprop.SetValue(Entity, dao.Result.Lastid, null);
            }
            return Error;
        }
        public AjaxResultCode Del(decimal? id_entity=null)
        {

            if (Loaded && id_entity==null)
            {
                dao.SqlDelete(Entity);
            }
            else if(id_entity!=null)
            {
                pkkeyprop.SetValue(Entity, id_entity, null);
                dao.SqlDelete(Entity);
            }
            else
            {
                Error.Errorcode = 9;
            }
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }
        public AjaxResultCode Close(decimal? id_entity=null)
        {
            if (Loaded && id_entity == null) 
            {
                if (CloseCommonFields != null) CloseCommonFields.Invoke(Entity, new object[] { (DateTime?)curDate.Sysdbdate, (DateTime?)curDate.Sysdbdate,null,null,null });
                dao.SqlUpdate(Entity);
            }
            else if (id_entity != null)
            {
    
                dao.SqlSelectId(id_entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    SetEntity(dao.Result.GetFirst<T>());
                    if (CloseCommonFields != null) CloseCommonFields.Invoke(Entity, new object[] { (DateTime?)curDate.Sysdbdate, (DateTime?)curDate.Sysdbdate, null, null, null });
                    dao.SqlUpdate(Entity);
                }
            } else
            {
                Error.Errorcode = 9;
            }
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }

        public string[] Get()
        {
            string[] retval = new string[] { "", "" };
            if (!Loaded)
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity = dao.Result.GetFirst<T>();
                    Loaded = true;
                }

            }
            //if(xmlcolprop!=null)
            if (xmlcolprop != null) xform = new Xform((string)xmlcolprop.GetValue(Entity), null);
            try { if (xrowidprop.GetValue(Entity, null) != null) xform.XRowId = (string)xrowidprop.GetValue(Entity, null).ToString(); } catch { }
            retval[0] = xform.DefRender.Render_simple(Id_Xform,Curlang);
            string currenttemplate = xform.ElementAppinfoDocTemplate;
            // 
            if (Id_Xform.StartsWith("template")) {
                switch (Id_Xform.Split('_')[1])
                {
                    case DextraLib.XForm.Xconstans.AppinfoViewTemplate:
                        currenttemplate = xform.ElementAppinfoTemplateView;
                        break;
                    case DextraLib.XForm.Xconstans.AppinfoAltTemplate:
                        currenttemplate = xform.ElementAppinfoTemplateAlt;
                        break;
                    default:
                        break;
                }
            }
            //
            retval[1] = "";
            if (currenttemplate != null)
            {
                XformGenralDocument1 ds = new XformGenralDocument1(currenttemplate, xform);
                ds.CurLang = Curlang;
                ds.Render();
                retval[1] = ds.Rendered; //RenderdedReport;
            }
            return retval;
        }


        string drender(string itemplate)
        {
            string currenttemplate = null;
            if (string.IsNullOrEmpty(itemplate))
            {
                currenttemplate = xform.ElementAppinfoDocTemplate;
            } else
            {
                if (itemplate.StartsWith("template"))
                {
                    switch (Id_Xform.Split('_')[1])
                    {
                        case DextraLib.XForm.Xconstans.AppinfoViewTemplate:
                            currenttemplate = xform.ElementAppinfoTemplateView;
                            break;
                        case DextraLib.XForm.Xconstans.AppinfoAltTemplate:
                            currenttemplate = xform.ElementAppinfoTemplateAlt;
                            break;
                        default:
                            break;
                    }
                }
            }

            //
            string retval = "";
            if (currenttemplate != null)
            {
                XformGenralDocument1 ds = new XformGenralDocument1(currenttemplate, xform);
                ds.Render();
                retval = ds.Rendered; //RenderdedReport;
            }
            return retval;
        }
        string ddrender(string itemplate)
        {
            string retval = "";
            if (string.IsNullOrEmpty(itemplate))
            {
                retval = xform.DefRender.Render_simple(Id_Xform, Curlang);
            } else
            {
                retval = xform.DefRender.Render(Id_Xform,itemplate, Curlang);
            }


            return retval;
        }

        public string[] GetNewVer(decimal id_entity, string mode,string templatename,string id_xform = "formid")
        {
            string[] retval = new string[] { "", "" };
            Id_Entity = id_entity;
            Id_Xform = id_xform;
            if (!Loaded)
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity = dao.Result.GetFirst<T>();
                    Loaded = true;
                }
            }
            if (xmlcolprop != null) xform = new Xform((string)xmlcolprop.GetValue(Entity), null);
            try { if (xrowidprop.GetValue(Entity, null) != null) xform.XRowId = (string)xrowidprop.GetValue(Entity, null).ToString(); } catch { }
            switch (mode)
            {
                case "NoRender":
                    break;
                case "AllRender":
                    retval[0] = ddrender(templatename);// xform.DefRender.Render(Id_Xform);
                    retval[1] = drender(templatename);
                    break;
                case "DefRender":
                    retval[0] = ddrender(templatename);// xform.DefRender.Render(Id_Xform);
                    break;
                case "DocRender":
                    retval[1] = drender(templatename);
                    break;
                default:
                    break;
            }
            return retval;
        }


        public string[] Get(decimal id_entity, string id_xform = "formid")
        {
            Id_Entity = id_entity;
            Id_Xform = id_xform;
            return Get();
        }

        public string[] FakeGet(decimal id_entity,string root, string id_xform = "formid")
        {
            Id_Entity = id_entity;
            Id_Xform = id_xform;
            string[] retval = new string[] { "", "" };
            if (!Loaded)
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    Entity = dao.Result.GetFirst<T>();
                    Loaded = true;
                }

            }
            xform = new Xform(root);
            xform.SetElementValueFromEntityClass(Entity);
            retval[0] = xform.DefRender.Render_simple(Id_Xform, Curlang);
            XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
            ds.Render();
            retval[1] = ds.Rendered;
            return retval;
        }


        public AjaxResultCode Save()
        {
            bool chkxrowid = (xrowidprop != null);
            if (Loaded) dao.SqlUpdate(Entity, chkxrowid);
            if (dao.Result.Error || dao.Result.Lastid==0)
            {
                if (dao.Result.Lastid == 0 && chkxrowid) { Error.Errorcode = 55; Error.Errormessage = "Concurency Error!!"; }
                else Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }

        public AjaxResultCode CloseAndSave()
        {
            bool chkxrowid = (xrowidprop != null);
            dao.SqlSelectId(Id_Entity);
            if (dao.Result.GetSate(DaoResult.ResCountOne))
            {
                T tempentity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                tempentity = dao.Result.GetFirst<T>();
                if (chkxrowid)
                {
                    decimal xr1=(decimal)xrowidprop.GetValue(Entity, null);
                    decimal xr2 = (decimal)xrowidprop.GetValue(tempentity, null);
                    if (xr1!=xr2)
                    {
                        Error.Errorcode = 55; Error.Errormessage = "Concurency Error!!";
                        return Error;
                    }
                }
                if (CloseCommonFields != null) CloseCommonFields.Invoke(tempentity, new object[] { curDate, curDate });
                dao.SqlInsert(tempentity);
                dao.SqlUpdate(Entity);
            }
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }

        public AjaxResultCode SaveSpec()
        {

            if (Loaded) dao.SqlUpdate(Entity);
            else
            {
                dao.SqlSelectId(Id_Entity);
                if (dao.Result.GetSate(DaoResult.ResCountOne))
                {
                    SetEntity(dao.Result.GetFirst<T>());
                    dao.SqlUpdate(Entity);
                }
            }
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }

        private void SetEntity(T e)
        {
            object en = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);

            Type t = e.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (info.GetValue(Entity, null) == null) info.SetValue(Entity, info.GetValue(e, null));
            }


        }
        private decimal toDecimal(string value)
        {
            decimal ret = 0;
            try
            {
                ret = decimal.Parse(value);
            }
            catch
            {
                Error.Errorcode = -10;
            }
            return ret;
        }

    }

}