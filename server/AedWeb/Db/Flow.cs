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
        public static SqlStrings Flow = SqlBuilder<Flow>.Build("Flows");

    }


    public class Flow : Commonfields
    {
        [PrimaryKeyAttribute]
        public decimal? Id_Flow { get; set; }
        public decimal? Id_Parrentflow { get; set; }
        public string Bid_Flow { get; set; }
        public string Flowname { get; set; }
        public string Stepname { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Title { get; set; }
        public string Flowhistory { get; set; }
        public string Pvariables { get; set; }
        public DateTime? Deadline_Day  { get; set; }

        public Flow()
        {

        }
        public Flow(decimal iId_Flow, decimal iId_Parrentflow, string iBid_Flow, string iFlowname, string iStepname, string iContorller, string iAction, string iTitle, string iFlowhistory, string iPvariables, DateTime iRecordvalidfrom, DateTime iRecordvalidto, DateTime iDatavalidfrom, DateTime iDatavalidto, string iStatus, string iCreator, string iModifiers, decimal iOrggroup, string iAttributum, string iProperty, string iAssignment)
        {

            Id_Flow = iId_Flow;
            Id_Parrentflow = iId_Parrentflow;
            Bid_Flow = iBid_Flow;
            Flowname = iFlowname;
            Stepname = iStepname;
            Controller = iContorller;
            Action = iAction;
            Title = iTitle;
            Flowhistory = iFlowhistory;
            Pvariables = iPvariables;
            Recordvalidfrom = iRecordvalidfrom;
            Recordvalidto = iRecordvalidto;
            Datavalidfrom = iDatavalidfrom;
            Datavalidto = iDatavalidto;
            Status = iStatus;
            Creator = iCreator;
            Modifiers = iModifiers;
            Orggroup = iOrggroup;
            Attributum = iAttributum;
            Property = iProperty;
            Assignment = iAssignment;
        }
    }
}