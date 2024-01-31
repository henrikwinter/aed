using Dextra.Flowbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Xapp.Models
{


    [Serializable]
    public class FlowViewModelData
    {
        public decimal Id_a { get; set; }
        public FlowViewModelData()
        {
            Id_a = 0;
        }

    }


    public class FlowViewModel
    {
        public FlowViewModelData Data { get; set; }
        public int CurrentFlowstep { get; set; }
        public List<RenderSartableFlows> Startableflows { get; set; }
        public FlowViewModel()
        {
            Startableflows = new List<RenderSartableFlows>();
        }
    }

    public class FunctionViewModel
    {
        public MvcHtmlString Rendered { get; set; }
        public List<RenderSartableFunctions> Startableflows { get; set; }
        public FunctionViewModel()
        {
            Startableflows = new List<RenderSartableFunctions>();
        }

    }
}