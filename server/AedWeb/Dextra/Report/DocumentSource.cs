using System;
using Xapp.Db;
using Dextra.Toolsspace;
using Dextra.Xforms;
using DextraLib.Report;
using Xapp;
using Dextra.Database;
using System.Collections.Generic;
using Xapp.FlowDatas;
using DextraLib.GeneralDao;
using System.Globalization;
using System.Dynamic;

namespace Dextra.Report
{

    public class RepDocumnet : DocumentSourceOld
    {

        public RepDocumnet()
        {

        }

        public RepDocumnet(string template) : base(template)
        {

        }
        public RepDocumnet(string file, string process, string submiturl, string basurl) : base(file, process, submiturl, basurl)
        {

        }

        public override string GetDictionary(string group, string name, string param, string value)
        {
            value = MvcApplication.Gdict.GetValue(group, name, param, value);
            value = "D_" + value;
            return value;
        }
        public override string Translate(string key, string value)
        {
            value = Langue.Translate(value, key);
            //value = "T_" + value;
            return value;
        }
        public string SaveTempReport()
        {
            string fileName = "Rep" + Guid.NewGuid().ToString() + "";
            string html = "";
            html = RenderdedReport;
            string hfilename = fileName + ".html";
            string houtputFile = Toolsspace.Tools.Getpath(hfilename, C.c_Temp);
            System.IO.File.WriteAllText(houtputFile, html);
            string url = Toolsspace.Tools.GetBaseUrl() + C.c_Tempu + hfilename; ;
            return url;
        }
        public string SaveTempPdf(byte[] data)
        {
            string fileName = "Rep" + Guid.NewGuid().ToString() + "";
            string html = "";
            html = RenderdedReport;
            string hfilename = fileName + ".pdf";
            string houtputFile = Toolsspace.Tools.Getpath(hfilename, C.c_Temp);
            System.IO.File.WriteAllBytes(houtputFile, data);
            string url = Toolsspace.Tools.GetBaseUrl() + C.c_Tempu + hfilename; ;
            return url;
        }
    }

    public class XformDataRow
    {
        public string Xpath { get; set; }
        public string  Value { get; set; }

        public XformDataRow()
        {
           
        }

        public XformDataRow(string x,string v)
        {
            Xpath = x;
            Value = v;
        }
    }

    public class XformGenralDocument: RepDocumnet
    {
        public DocumentRow<XformDataRow> XformDatarowOne { get; set; }
        public DocumentRow<XformDataRow> XformDatarowTwo { get; set; }
        public DocumentRow<XformDataRow> XformDatarowTree { get; set; }
        public DocumentRow<XformDataRow> XformDatarowFour { get; set; }
        public DocumentRow<XformDataRow> XformDatarowFive { get; set; }

        public Xform Xform { get; set; }
        public XformGenralDocument(string template, Xform x,int count=0) : base(template)
        {
            Apppath = System.Web.HttpContext.Current.Server.MapPath(C.c_XmlCodesFiles); 
            Xform = x;
            XformDatarowOne = new DocumentRow<XformDataRow>(x);
            XformDatarowOne.DbResultcount = count;
            XformDatarowTwo = new DocumentRow<XformDataRow>(x);
            XformDatarowTwo.DbResultcount = count;
            XformDatarowTree = new DocumentRow<XformDataRow>(x);
            XformDatarowTree.DbResultcount = count;
            XformDatarowFour = new DocumentRow<XformDataRow>(x);
            XformDatarowFour.DbResultcount = count;
            XformDatarowFive = new DocumentRow<XformDataRow>(x);
            XformDatarowFive.DbResultcount = count;
        }
        public XformGenralDocument(Xform x,string file, int count = 0) : base(file,"","","")
        {
            Apppath = System.Web.HttpContext.Current.Server.MapPath(C.c_XmlCodesFiles);
            Xform = x;
            XformDatarowOne = new DocumentRow<XformDataRow>(x);
            XformDatarowOne.DbResultcount = count;
            XformDatarowTwo = new DocumentRow<XformDataRow>(x);
            XformDatarowTwo.DbResultcount = count;
            XformDatarowTree = new DocumentRow<XformDataRow>(x);
            XformDatarowTree.DbResultcount = count;
            XformDatarowFour = new DocumentRow<XformDataRow>(x);
            XformDatarowFour.DbResultcount = count;
            XformDatarowFive = new DocumentRow<XformDataRow>(x);
            XformDatarowFive.DbResultcount = count;
        }


    }

    public class XformGenralDocument1 : DocumentSource
    {
        public DocumentRow<XformDataRow> XformDatarowOne { get; set; }
        public DocumentRow<XformDataRow> XformDatarowTwo { get; set; }
        public DocumentRow<XformDataRow> XformDatarowTree { get; set; }
        public DocumentRow<XformDataRow> XformDatarowFour { get; set; }
        public DocumentRow<XformDataRow> XformDatarowFive { get; set; }

        public string CurLang { get; set; }

        public override string EnumTranslate(string key, string value)
        {
            return Langue.EnumTranslateXform( key,  value, CurLang, "DsXform" + Xform.RootName);
        }


        public override string Translate(string key, string value)
        {

            return Langue.Translate(key, "DsXform"+Xform.RootName, CurLang);     //Preflang.Split('.')[0].ToLower());
            //return base.Translate(key, value);
        }

        public Xform Xform { get; set; }
        public XformGenralDocument1(string template, Xform x, int count = 0) : base()
        {
            Init(template,null);

            Apppath = System.Web.HttpContext.Current.Server.MapPath(C.c_XmlCodesFiles);
            Xform = x;
            XformDatarowOne = new DocumentRow<XformDataRow>(x);
            XformDatarowOne.DbResultcount = count;
            XformDatarowTwo = new DocumentRow<XformDataRow>(x);
            XformDatarowTwo.DbResultcount = count;
            XformDatarowTree = new DocumentRow<XformDataRow>(x);
            XformDatarowTree.DbResultcount = count;
            XformDatarowFour = new DocumentRow<XformDataRow>(x);
            XformDatarowFour.DbResultcount = count;
            XformDatarowFive = new DocumentRow<XformDataRow>(x);
            XformDatarowFive.DbResultcount = count;
        }
        public XformGenralDocument1(Xform x, string file, int count = 0) : base(file)
        {
            Apppath = System.Web.HttpContext.Current.Server.MapPath(C.c_XmlCodesFiles);
            Xform = x;
            XformDatarowOne = new DocumentRow<XformDataRow>(x);
            XformDatarowOne.DbResultcount = count;
            XformDatarowTwo = new DocumentRow<XformDataRow>(x);
            XformDatarowTwo.DbResultcount = count;
            XformDatarowTree = new DocumentRow<XformDataRow>(x);
            XformDatarowTree.DbResultcount = count;
            XformDatarowFour = new DocumentRow<XformDataRow>(x);
            XformDatarowFour.DbResultcount = count;
            XformDatarowFive = new DocumentRow<XformDataRow>(x);
            XformDatarowFive.DbResultcount = count;
        }


    }


    /// Teszt
    /// 

    public class Workplaces
    {
        public Workplaces()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Taxno { get; set; }
        public string Pkwp { get; set; }
        public Workplaces(int id, string pk, string name, string taxno)
        {

            Id = id;
            Pkwp = pk;
            Name = name;
            Taxno = taxno;
        }
    }
    public class Status
    {
        public Status()
        {

        }
        public int Id { get; set; }
        public string Pkstat { get; set; }
        public string Name { get; set; }
        public string Feor { get; set; }

        public Status(int id, string pk, string name, string feor)
        {

            Id = id;
            Pkstat = pk;
            Name = name;
            Feor = feor;
        }
    }
    public class Pays
    {
        public Pays()
        {

        }
        public int Id { get; set; }
        public string Pkpay { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Pays(int id, string pk, string name, string value)
        {

            Id = id;
            Pkpay = pk;
            Name = name;
            Value = value;
        }
    }
    public class TestDocument : DocumentSource  // RepDocumnet
    {
        // Sample 
        public DocumentRow<Person> Persons { get; set; }
        public DocumentRow<Workplaces> Workplaces { get; set; }
        public DocumentRow<Status> Status { get; set; }
        public DocumentRow<Pays> Pays { get; set; }

        public string TesztData { get; set; }

        public Xform Xtest { get; set; }

        public Person Marcsi { get; set; }

        public TestDocument(string file, string process, string submiturl, string basurl) : base(file)// base(file, process, submiturl, basurl)
        {

            Xtest = new Xform("Entity");
            //Xform xform = new Xform(testx,true);
            Xtest.InitDefaults();
            Xtest.SetXformelement("/Entity/Name", "Makkos Peter");

            Marcsi = new Person(5, "Marok Marcsi", "444");

            TesztData = "XXXXXXXXXXX";
            Persons = new DocumentRow<Person>();
            Workplaces = new DocumentRow<Workplaces>();
            Status = new DocumentRow<Status>();
            Pays = new DocumentRow<Pays>();

            Persons.Add(new Person(1, "Peti", "Huszt"));
            Workplaces.Add(new Workplaces(1, "Peti", "PetiWplaceElso", "a123"));
            Status.Add(new Status(1, "Peti", "PetiStatusElso", "axx"));
            Pays.Add(new Pays(1, "PetiStatusElso", "PetiPayelso", "a123"));
            Pays.Add(new Pays(1, "PetiStatusElso", "PetiPaymasodik", "a123"));
            Status.Add(new Status(1, "Peti", "PetiStatusMasodik", "axx"));

            
                        Persons.Add(new Person(2, "Pisti", "Nyuszt"));
                        Workplaces.Add(new Workplaces(1, "Pisti", "PistiWplaceElso", "Aaaaaaa"));
                        Status.Add(new Status(1, "Pisti", "PistiStatusElso", "11111111111"));
                        Pays.Add(new Pays(1, "PistiStatusElso", "PistiPayelso", "a123"));
                        Pays.Add(new Pays(1, "PistiStatusElso", "PistiPaymasodik", "a123"));
                        Status.Add(new Status(1, "Pisti", "PistiStatusMasodik", "11111111111"));
                        Pays.Add(new Pays(1, "PistiStatusMasodik", "PistiPayelso", "a123"));
                        Pays.Add(new Pays(1, "PistiStatusMasodik", "PistiPaymasodik", "a123"));

                        Persons.Add(new Person(2, "Mari", "Qgli"));
                        Status.Add(new Status(1, "Mari", "MariStatusElso", "11111111111"));
                        Pays.Add(new Pays(1, "MariStatusElso", "PayMarielso", "a123"));
            



            //Persons.Add(new Person(1, "Teszt Elek", "Huszt"));
            //Persons.Add(new Person(2, "Kakas Elek", "Huszt"));
            //Persons.Add(new Person(3, "Nyakas Elek", "Huszt"));
            //Persons.Add(new Person(4, "Balog Elek", "Huszt"));

            //Workplaces.Add(new Workplaces(1, "Teszt Elek", "Mom1", "a123"));
            //Workplaces.Add(new Workplaces(2, "Teszt Elek", "Mom2", "b123"));
            //Workplaces.Add(new Workplaces(3, "Kakas Elek", "Mom3", "c123"));
            //Workplaces.Add(new Workplaces(4, "Kakas Elek", "Mom4", "d123"));
            //Workplaces.Add(new Workplaces(5, "Kakas Elek", "Mom5", "e123"));
            //Workplaces.Add(new Workplaces(6, "Balog Elek", "Mom6", "f123"));
            //Workplaces.Add(new Workplaces(7, "Balog Elek", "Mom7", "g123"));

            // Status.Add(new Status(1, "Teszt Elek", "Eloado1", "axx"));
            //Status.Add(new Status(2, "Teszt Elek", "Eloado2", "bxx"));
            //Status.Add(new Status(3, "Kakas Elek", "Eloado3", "cxx"));
            //Status.Add(new Status(4, "Kakas Elek", "Eloado4", "dxx"));
            //Status.Add(new Status(5, "Kakas Elek", "Eloado5", "exx"));
            //Status.Add(new Status(6, "Balog Elek", "Eloado6", "fxx"));
            //Status.Add(new Status(7, "Balog Elek", "Eloado7", "gxx"));

            //Pays.Add(new Pays(1, "Eloado1", "Alap11", "a123"));
            //Pays.Add(new Pays(1, "Eloado1", "Alap12", "a123"));
            //Pays.Add(new Pays(1, "Eloado1", "Alap13", "a123"));
            //Pays.Add(new Pays(1, "Eloado1", "Alap14", "a123"));
            //Pays.Add(new Pays(2, "Eloado2", "Alap21", "b123"));
            //Pays.Add(new Pays(3, "Eloado3", "Alap31", "c123"));
            //Pays.Add(new Pays(4, "Eloado4", "Alap41", "d123"));
            //Pays.Add(new Pays(5, "Eloado5", "Alap51", "e123"));
            //Pays.Add(new Pays(5, "Eloado5", "Alap52", "e123"));
            //Pays.Add(new Pays(5, "Eloado5", "Alap53", "e123"));
            //Pays.Add(new Pays(5, "Eloado5", "Alap54", "e123"));
            //Pays.Add(new Pays(6, "Eloado6", "Alap61", "f123"));
            //Pays.Add(new Pays(7, "Eloado7", "Alap71", "g123"));

        }

    }

    public class SimpleDocument :DocumentSource
    {
        public dynamic Glob { get; set; }

        public SimpleDocument()
        {
            Glob = new ExpandoObject();
            Glob.Curentdat = DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss.fff", CultureInfo.InvariantCulture);

        }

        public SimpleDocument(DbContext Sdb, string templatefilename,  Dictionary<string, string> hiddens = null) : base( Dextra.Toolsspace.Tools.Getpath(templatefilename, C.c_HtmlTemplate), hiddens)
        {
            Glob = new ExpandoObject();
            Glob.Curentdat = DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Baseurl = Dextra.Toolsspace.Tools.GetBaseUrl();
        }
    }


    public class AgreementDocument : DocumentSource  // RepDocumnet
    {

        public Agreement docAgreement { get; set; }
        public Organization Status { get; set; }

        public Persons docPerson { get; set; }

        // Sample 
        public DocumentRow<Agreement_Pays> docAgreement_Pays { get; set; }
        public DocumentRow<Agreement_Elements> docAgreement_Elements { get; set; }

        public string TesztData { get; set; }

        public Xform Xtest { get; set; }

        public AgreementDocument(DbContext Sdb, string templatefilename,decimal Id_Flows,Dictionary<string,string> hiddens=null) : base( Dextra.Toolsspace.Tools.Getpath(templatefilename, C.c_HtmlTemplate), hiddens)
        {
            Baseurl = Dextra.Toolsspace.Tools.GetBaseUrl();

            docAgreement_Pays = new DocumentRow<Agreement_Pays>();
            docAgreement_Elements = new DocumentRow<Agreement_Elements>();

                        OrgPersonViewModel ViewModel = new OrgPersonViewModel();
            Dao<Flow> fdao = new Dao<Flow>(Sdb);
            fdao.SqlSelectId(Id_Flows);
            if (fdao.Result.GetSate(DaoResult.ResCountOne))
            {
                Flow f = fdao.Result.GetFirst<Flow>();
                
                ViewModel = (OrgPersonViewModel)Dextra.Toolsspace.Tools.Deserialize(ViewModel, f.Pvariables);
            }

            Dao<Agreement> adao = new Dao<Agreement>(Sdb);
            adao.SqlSelectId(ViewModel.Person.ClientPart.io_AgreemetId);
            if (adao.Result.GetSate(DaoResult.ResCountOne))
            {
                docAgreement = adao.Result.GetFirst<Agreement>();
            }

            Dao<Organization> odao = new Dao<Organization>(Sdb);
            odao.SqlSelectId(ViewModel.Org.ClientPart.io_SelectedStatusId);
            if (odao.Result.GetSate(DaoResult.ResCountOne))
            {
                Status = odao.Result.GetFirst<Organization>();
            }

            Dao<Persons> pdao = new Dao<Persons>(Sdb);
            pdao.SqlSelectId(ViewModel.Person.ClientPart.io_SelectedPersonId);
            if (pdao.Result.GetSate(DaoResult.ResCountOne))
            {
                docPerson = pdao.Result.GetFirst<Persons>();
            }


            Dao<Agreement_Pays> apdao = new Dao<Xapp.Db.Agreement_Pays>(Sdb);
            apdao.SqlSelect("select * from Agreement_Pays where id_Agreement=:0 ", new object[] { ViewModel.Person.ClientPart.io_AgreemetId });
            if (apdao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach(Agreement_Pays ap in apdao.Result.GetRes<Agreement_Pays>())
                {
                    docAgreement_Pays.Add(ap);
                }
            }

            Dao<Agreement_Elements> aedao = new Dao<Xapp.Db.Agreement_Elements>(Sdb);
            aedao.SqlSelect("select * from Agreement_Elements where id_Agreement=:0 ", new object[] { ViewModel.Person.ClientPart.io_AgreemetId });
            if (aedao.Result.GetSate(DaoResult.ResCountOneOrMore))
            {
                foreach (Agreement_Elements ae in aedao.Result.GetRes<Agreement_Elements>())
                {
                    docAgreement_Elements.Add(ae);
                }
            }



        }


        public AgreementDocument(string file, string process, string submiturl, string basurl) : base(file)
        {


            


        }

    }

}
