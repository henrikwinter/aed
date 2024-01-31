var Flowtransitionxref = {
    'SelectDevelopedOrganization': 'glyphicon-trash',
    'DevelopeSelected': 'glyphicon-trash',
    'CheckDeveloped': 'glyphicon-home',
    'FinalizeDevelope': 'glyphicon-time'
};
function recursiveFunction(key, val, pre) {
    var value = val;
    if (value instanceof Object) {
        pre += key + ".";
        $.each(value, function (key, val) {
            recursiveFunction(key, val, pre)
        });
    } else {
        actualFunction(key, val, pre);
    }
}
function actualFunction(key, val, pre) {
    var t = pre + key;
    $("#Transitionform").addHidden(t, val);
}
var FlowName;
var ToStepname;
var UserHasContinue;
var Id_Flow;
function TransitionSubmit(xml) {
    var PostData = {};
    if (xml != '<h>Cancel</h>') {
        if (typeof CheckPostData == 'function') {
            var m = CheckPostData();
            if (m.retval != c_FlowPostpreCheckretOk) {
                GlobError(m.retval, 2);
                return false;
            }
            $.each(m, function (key, val) {
                recursiveFunction(key, val, "ViewModel." + WorkData.ClientPartName + ".")
            });
            $.each(m, function (key, val) {
                $("#Transitionform").addHidden(key, val);
            });
        }
    }
    $("#Transitionform").addHidden('ToFlowname', FlowName);
    $("#Transitionform").addHidden('ToStepname', ToStepname);
    $("#Transitionform").addHidden('History', htmlEncode(xml));
    $("#Transitionform").addHidden('Continoue', UserHasContinue);
    $("#Transitionform").submit();
};
try {
    if (WorkData.model_CurrentFlowstep.Error == true)
        GlobError(WorkData.model_CurrentFlowstep.ErrorMessage, 2);
    $("#historybtn").on('click', function (e) {
        AjaxGet(pageVariable.baseSiteURL + "Ajax/GetProcFlowHist", {
            id_flow: WorkData.model_CurrentFlowstep.Id_Flow
        }, function (data) {
            $('#HistoryTree').html(data.frm);
            $("#FlowHistoryWindow").modal('show');
        });
    });
    $("#Transitionform").find("button").each(function (index) {
        if (this.id == "historybtn")
            return false;
        value = $(this).attr('data-bind');
        var parts = value.split('.');
        key = parts[0];
        var value = Flowtransitionxref[key];
        if (value != undefined) {
            $(this).find('.glyphicon-hand-right').removeClass("glyphicon-hand-right").addClass(value);
        }
    });
    Id_Flow = $("#Transitionform").attr('data1');
    FlowName = $("#Transitionform").attr('data');
    $("#Transitionform").find("button").click(function () {
        if (this.id == "historybtn")
            return false;
        value = $(this).attr('data-bind');
        var parts = value.split('.');
        ToStepname = parts[0];
        UserHasContinue = parts[1];
        var Postback = parts[2];
        if (ToStepname == "Cancel") {
            TransitionSubmit('<h>Cancel</h>');
        } else {
            AjaxPost(pageVariable.baseSiteURL + "Ajax/GetProcFlowext", {
                Flowname: FlowName,
                Stepname: ToStepname,
                Id_Flow: Id_Flow
            }, function (data) {
                if (UserHasContinue != 'True') {
                    $("#BtnFlowContinue").hide();
                } else {
                    $("#BtnFlowContinue").show();
                }
                $("#ProcFlowDiv").html(data.ret);
                $("#FlowWindow").modal('show');
            });
        }
        return false;
    });
    $("#BtnFlowContinue").click(function () {
        UserHasContinue = 'true';
        $("#FlowWindow").modal('hide');
        $("#ProcflowFrmForm").mYPostForm("Ajax/GenerateHistoryxml", {}, function (Data) {
            TransitionSubmit(Data.xml);
        });
    });
    $("#BtnFlowDone").click(function () {
        UserHasContinue = 'false';
        $("#FlowWindow").modal('hide');
        $("#ProcflowFrmForm").mYPostForm("Ajax/GenerateHistoryxml", {}, function (Data) {
            TransitionSubmit(Data.xml);
        });
    });
    $("#Transitionform").submit(function () {
        return true;
    });
} catch (err) { }
