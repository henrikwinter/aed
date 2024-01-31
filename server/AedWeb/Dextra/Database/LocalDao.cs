
using Dextra.Common;
using Dextra.Report;
using Dextra.Xforms;
using DextraLib.GeneralDao;
using DextraLib.MsSql;
using Xapp.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Xapp.Db;

namespace Dextra.Database.Local
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
            dated.SqlSelect("select GETDATE() as SYSDATE  ", new object[] { });
            if (dated.Result.GetSate(0))
            {

                Sysdbdate = (DateTime)dated.DynResult.Sysdate;
            }
            else
            {
                Sysdbdate = DateTime.Now;
            }
            Offset = Sysdbdate.Ticks - Sysdate.Ticks;
            Dbc.Dbsysdate = this;

        }
    }

    public class ProcParam : MsProcParam
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
        public static SqlStrings Build(string tablename = null)
        {
            return MsSqlDaoHelper.WriteColumnMappings<T>(tablename);
        }

        public static Sqlstring Get()
        {
            Type t = typeof(Sqlstore);
            FieldInfo field = t.GetFields(BindingFlags.Static | BindingFlags.Public).ToList().FirstOrDefault(p => p.Name == typeof(T).Name);
            SqlStrings sqls = (SqlStrings)field.GetValue(null);
            Sqlstring ret = new Sqlstring(sqls.Select, sqls.Insert, sqls.Update);
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

    public class DbContext : MsSqlDbContext
    {
        public DbSysDate Dbsysdate { get; set; }

        public DbContext()
        {
        }

        public DbContext(string connectionstring, string Userid = "Gad") : base(connectionstring, Userid = "Gad")
        {
        }


    }

    public class Hierarchy<T>
    {

        public Dao<T> hdao { get; set; }
        public List<T> Hresult { get; set; }
        public PropertyInfo parentcolprop { get; set; }
        public PropertyInfo pkcolprop { get; set; }

        public object GetHierarchy(decimal startid, string parentname)
        {
            parentcolprop = hdao.Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(parentname));
            string pk = SqlBuilder<T>.GetAll().PrKeyColumns;
            pkcolprop = hdao.Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(pk));

            string sql = SqlBuilder<T>.GetAll().Select + " where " + SqlBuilder<T>.GetAll().PrKeyColumns + "=:0  order by Id_Ord";
            hdao.ExecuteReaderToClass(sql, new object[] { startid });
            T r = hdao.Result.GetFirst<T>();
            Hresult.Add(r);
            HWalk(r);
            return Hresult;
        }

        void HWalk(T item)
        {

            object id = pkcolprop.GetValue(item, null);
            string sql = SqlBuilder<T>.GetAll().Select + " where " + parentcolprop.Name + "=:0 order by Id_Ord";
            hdao.SqlSelect(sql, new object[] { id});
            List<T> temp = hdao.Result.GetRes<T>();
            Hresult.AddRange(temp);
            foreach (T rec in temp)
            {
                HWalk(rec);
            }
        }

        public Hierarchy(DbContext db)
        {
            hdao = new Dao<T>(db);
            Hresult = new List<T>();
        }
    }


    public class Dao : MsSqlDataAccess
    {

        public object SqlSelect(string sql, object[] par)
        {
            // sql = MyDaoHelper.ConvertStairsToColumns(sql,null);
            return base.ExecuteReader(sql, par);
        }


        public Dao(MsSqlDbContext db)
            : base(db)    //new DextraLib.OraDao.OraDbContext(db.ConnectString, db.GeneralVariables))
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

    public class Dao<T> : MsSqlDataAccess, IDisposable
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
                Entity = Result.GetFirst<T>();
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
            string lsql = SqlBuilder<T>.Get().Insert;
            e = (T)ExecuteNonQueryFromClass(lsql, e);
            return e;
        }
        public object SqlUpdate(T e)
        {
            string lsql = SqlBuilder<T>.GetAll().UpdatePk;
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
        public string MoveRecord(int id, int newpid, string drp, string foreginkeyname, string reorderproc, decimal? xrowid = null)
        {

            string ret = "";
            PropertyInfo pkkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(SqlBuilder<T>.GetAll().PrKeyColumns));
            PropertyInfo foreginkeyprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert(foreginkeyname));
            PropertyInfo IdOrd_prop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Id_Ord"));
            //PropertyInfo ixrowprop = typeParameterType.GetProperty(DaoHelper.InitCapitalConvert("Xrowid"));


            try
            {
                T t = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T t1 = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                T p = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
                SqlSelectId(id);
                if (Result.GetSate(1))
                {
                    t = Result.GetFirst<T>();
                   // decimal? x = (decimal)ixrowprop.GetValue(t, null);
                   // if (xrowid != null && x != xrowid)
                   // {
                   //     return "ERR_CONCURENCY";
                   // }
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
                    try {
                        reorderparrent = (decimal)foreginkeyprop.GetValue(t, null);
                    } catch { } //t.Id_Parent;

                }
                else
                {
                    foreginkeyprop.SetValue(t, foreginkeyprop.GetValue(t1, null));
                    //t.Id_Parent = t1.Id_Parent;
                }

                if (drp.Equals("before"))
                {
                    IdOrd_prop.SetValue(t, (decimal)IdOrd_prop.GetValue(t1, null) - 0.1m);
                    //t.Id_Ord = t1.Id_Ord - 0.1m;
                }
                else if (drp.Equals("after"))
                {
                    IdOrd_prop.SetValue(t, (decimal)IdOrd_prop.GetValue(t1, null) + 0.1m);
                    //t.Id_Ord = t1.Id_Ord + 0.1m;
                }
                else
                {
                    IdOrd_prop.SetValue(t, (decimal)99999, null);
                    //t.Id_Ord = 999999;
                }
                ClearResult();
                SqlUpdate(t);

                List<ProcParam> param = new List<ProcParam>();
                param.Add(new ProcParam("p", reorderparrent));
                //SqlProcedure("reorder", ref param, this.Dbcontext);
                //SqlProcedure(reorderproc, ref param, this.Dbcontext);
                ret = "Ok";
            }
            catch (Exception e)
            {
                ret = "Error";
            }



            return ret;
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
        public object Id_ParentEntity { get; set; }
        public string Id_Xform { get; set; }


        public PropertyInfo pkkeyprop { get; set; }
        public PropertyInfo foreginkeyprop { get; set; }
        public PropertyInfo xmlcolprop { get; set; }

        public PropertyInfo datavalidfromcolprop { get; set; }
        public PropertyInfo attributumcolprop { get; set; }
        public MethodInfo InitCommonFieldsForAdd { get; set; }

        public MethodInfo CloseCommonFields { get; set; }

        DbSysDate curDate = null;
        DbContext dbcontext = null;

        Dao<T> dao { get; set; }

        public bool Loaded = false;

        public AjaxResultCode Error = new AjaxResultCode();

        public XformProcess()
        {
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
        }

        void Init(DbContext Sdb, string XmlColname = "Xmldata")
        {
            curDate = Sdb.Dbsysdate;
            dbcontext = Sdb;
            Entity = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            try {
                dao = new Dao<T>(Sdb);
                string prkey = ((DextraLib.GeneralDao.SqlStrings)(typeof(Xapp.Db.Sqlstore).GetField(typeof(T).Name).GetValue(dao))).PrKeyColumns;
                pkkeyprop = Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(prkey));
                xmlcolprop = Entity.GetType().GetProperty(DaoHelper.InitCapitalConvert(XmlColname));
                InitCommonFieldsForAdd = typeof(T).GetMethod("InitCommonFieldsForAdd");
                CloseCommonFields = typeof(T).GetMethod("CloseCommonFields");
            }
            catch { }

        }
        public XformProcess(DbContext Sdb, string XmlColname = "Xmldata")
        {
            dao = new Dao<T>(Sdb);
            Init(Sdb, XmlColname);
        }


        // Post ból keres "Id_Entity" és entity->PkName-t vagy Xfromból->   PkName-t és "Id_Parent"-et és "Id_Xform"-ot
        public XformProcess(DbContext Sdb, System.Collections.Specialized.NameValueCollection PostVal, string XmlColname = "Xmldata")
        {
            Init(Sdb, XmlColname);

            if (PostVal["Id_Xform"] != null) Id_Xform = PostVal["Id_Xform"];
            if (PostVal["Id_Entity"] != null) Id_Entity = toDecimal(PostVal["Id_Entity"]);
            try { if (PostVal[pkkeyprop.Name] != null) Id_Entity = toDecimal(PostVal[pkkeyprop.Name]); } catch { }
            if (PostVal["Id_Parent"] != null) Id_ParentEntity = toDecimal(PostVal["Id_Parent"]);
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
            xform.SetEntityClassFromElemnetValue(Entity);
            try { if (pkkeyprop.GetValue(Entity, null) != null) Id_Entity = pkkeyprop.GetValue(Entity, null); } catch { }

            string xml = xform.GetXmlStringFromXform();
            if (xmlcolprop != null) xmlcolprop.SetValue(Entity, xml, null);
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
        public AjaxResultCode Del()
        {

            if (Loaded)
            {
                dao.SqlDelete(Entity);
            }
            else
            {
                pkkeyprop.SetValue(Entity, Id_Entity, null);
                dao.SqlDelete(Entity);
            }
            if (dao.Result.Error)
            {
                Error.Errorcode = dao.Result.ErrorCode;
            }
            return Error;
        }

        public AjaxResultCode Del(decimal id_entity)
        {
            pkkeyprop.SetValue(Entity, id_entity, null);
            dao.SqlDelete(Entity);
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
            xform = new Xform((string)xmlcolprop.GetValue(Entity), null);
            retval[0] = xform.DefRender.Render_simple(Id_Xform);
            XformGenralDocument1 ds = new XformGenralDocument1(xform.ElementAppinfoDocTemplate, xform);
            ds.Render();
            retval[1] = ds.Rendered;
            return retval;
        }

        public string[] Get(decimal id_entity, string id_xform = "formid")
        {
            Id_Entity = id_entity;
            Id_Xform = id_xform;
            return Get();
        }

        public AjaxResultCode Save()
        {
            if (Loaded) dao.SqlUpdate(Entity);
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