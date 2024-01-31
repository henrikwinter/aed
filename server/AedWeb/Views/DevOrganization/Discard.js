include('~/Scripts/DextraFlowManParial.js' | args(0, 0));
var CheckPostData = function () {
    WorkData.ClientPartName = 'Org';
    WorkData.model_ClientPart.retval = c_FlowPostpreCheckretOk;
    return WorkData.model_ClientPart;
};




$("#IdBodyStart").hide();
$("#IdBody").show();
