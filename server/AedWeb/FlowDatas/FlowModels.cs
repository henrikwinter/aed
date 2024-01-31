using Dextra.Database;
using Dextra.Flowbase;
using Dextra.Toolsspace;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Xapp.Db;

namespace Xapp.FlowDatas
{



    public class BaseFlowModel<T>
    {
        public DxFlowManager FlowModel { get; set; }

        public T ViewModel { get; set; }

        public BaseFlowModel()
        {

        }

        public void InitViewdata()
        {
            //ViewModel = (T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
        }


        public void Getflowstep(string flowname, string stepname)
        {
            FlowModel._Getflowstep(flowname, stepname);
        }
        public void Getflowstep(decimal flowid)
        {
            FlowModel._Getflowstep(flowid);
            try
            {
                ViewModel = (T)Tools.Deserialize(ViewModel, FlowModel.dbflow.Pvariables);
            } catch { }
            
        }
        public void Setflowstep(string flowname, string stepname)
        {
            FlowModel.dbflow.Pvariables = Tools.Serialize(ViewModel);
            FlowModel._Setflowstep(flowname, stepname);
        }
        public void Makeflowstep(string flowname, string stepname, string descibre = null)
        {
            FlowModel.dbflow.Pvariables = Tools.Serialize(ViewModel);
            FlowModel._Makeflowstep(flowname, stepname, descibre);
        }
        public ActionResult Set(string Backurl, ActionResult Backtowiew, string flowdesc = null, DbContext db = null)
        {
            ActionResult ret;
            if (!FlowModel.Complett)
            {
                if (db != null) db.RollBack();
                return Backtowiew;
            }

            FlowModel.dbflow.Pvariables = Tools.Serialize(ViewModel);
            ret = FlowModel._Set(Backurl, flowdesc);
            if (FlowModel.Erroroccure)
            {
                FlowModel.PostedFlowData.Error = true;
                FlowModel.PostedFlowData.Errormsgs = "Error for set!";
                if (db != null) db.RollBack();
                return Backtowiew;
            }
            if (db != null) db.Commit();
            return ret;
        }
        public void End()
        {
            FlowModel.dbflow.Pvariables = Tools.Serialize(ViewModel);
            FlowModel.End();
        }


    }


    // ---------------------------------------------------------------------------------
    [Serializable]
    public class PersonClientPart
    {
        public int? io_SelectedPersonId { get; set; }

        public decimal? io_AgreemetId { get; set; }
        public string o_Usedname  { get; set; }

        public PersonClientPart()
        {
            io_SelectedPersonId = null;
            o_Usedname = null;
        }

    }
    [Serializable]
    public class PersonsModeldata
    {

        public PersonClientPart ClientPart { get; set; }
        public PersonsModeldata()
        {
            ClientPart = new PersonClientPart();
        }

    }

    // ---------------------------------------------------------------------------------


    // ---------------------------------------------------------------------------------    


    [Serializable]
    public class OrgClientPart
    {
        public int o_OrgHiercurentCount { get; set; }
        public int? io_SelectedOrgId { get; set; }
        public int? io_SelectedStatusId { get; set; }

        public DateTime? Activate_Date { get; set; }


        public OrgClientPart()
        {

        }

    }

    [Serializable]
    public class OrgModeldata
    {
        public OrgClientPart ClientPart { get; set; }
        public OrgModeldata()
        {
            ClientPart = new OrgClientPart();
        }
    }

    [Serializable]
    public class OrgPersonViewModel
    {

        public byte[] PdfAgrement { get; set; }
        public PersonsModeldata Person { get; set; }
        public OrgModeldata Org { get; set; }


        public OrgPersonViewModel()
        {
            //  Flow = new FlowModeldata();
            Person = new PersonsModeldata();
            Org = new OrgModeldata();
            PdfAgrement = new byte[] { 0 };

        }

    }
    public class OrgPersonModel :BaseFlowModel<OrgPersonViewModel>
    {

        public OrgPersonModel()
        {
            ViewModel = new OrgPersonViewModel();
        }
        public OrgPersonModel(DbContext db, List<Vw_User> roles, string curentUser)
        {
            FlowModel = new DxFlowManager(db, roles, curentUser);
            ViewModel = new OrgPersonViewModel();

        }

    }

    public class EmptyViewModel
    {
        public EmptyViewModel()
        {

        }
    }

    public class SimpleFlowModel : BaseFlowModel<EmptyViewModel>
    {

        public SimpleFlowModel()
        {
            
        }
        public SimpleFlowModel(DbContext db, List<Vw_User> roles, string curentUser)
        {
            FlowModel = new DxFlowManager(db, roles, curentUser);
            

        }

    }


    //-----------------------------------

    [Serializable]
    public class AnimalClientPart
    {
        public int? io_SelectedAnimalId { get; set; }


        public string o_Usedname { get; set; }

        public AnimalClientPart()
        {
            io_SelectedAnimalId = null;
            o_Usedname = null;
        }

    }


    public class AnimalModeldata
    {

        public AnimalClientPart ClientPart { get; set; }
        public AnimalModeldata()
        {
            ClientPart = new AnimalClientPart();
        }

    }


    [Serializable]
    public class AnimalViewModel
    {

        public byte[] PdfAgrement { get; set; }
        public AnimalModeldata Animal { get; set; }
        public OrgModeldata Org { get; set; }


        public AnimalViewModel()
        {
            //  Flow = new FlowModeldata();
            Animal = new AnimalModeldata();

            PdfAgrement = new byte[] { 0 };

        }

    }


    public class AnimalModel : BaseFlowModel<AnimalViewModel>
    {

        public AnimalModel()
        {
            ViewModel = new AnimalViewModel();
        }
        public AnimalModel(DbContext db, List<Vw_User> roles, string curentUser)
        {
            FlowModel = new DxFlowManager(db, roles, curentUser);
            ViewModel = new AnimalViewModel();

        }

    }

    //-----------------------------------

    [Serializable]
    public class AedClientPart
    {
        public AedClientPart()
        {
        }

    }



    public class AedModeldata
    {

        public AedClientPart ClientPart { get; set; }
        public AedModeldata()
        {
            ClientPart = new AedClientPart();
        }

    }



    [Serializable]
    public class AedViewModel
    {
        public AedModeldata Aed { get; set; }
        public AedViewModel()
        {
            Aed = new AedModeldata();
        }

    }



    public class AedModel : BaseFlowModel<AnimalViewModel>
    {

        public AedModel()
        {
            ViewModel = new AnimalViewModel();
        }
        public AedModel(DbContext db, List<Vw_User> roles, string curentUser)
        {
            FlowModel = new DxFlowManager(db, roles, curentUser);
            ViewModel = new AnimalViewModel();

        }
    }

}
