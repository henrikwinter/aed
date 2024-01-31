
var dlgAreYouSoure = function (callback) {
    $("#dlgAreYouSoure").modal('show');
    $("#dlgAreYouSoureCloseRecord").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback('close');
    });
    $("#dlgAreYouSoureYes").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback(true);
    });
    $("#dlgAreYouSoureNo").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback(false);
    });
};


WorkData.Persons_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true
};
WorkData.SearchPersons_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true
};
WorkData.Vw_Users_Data = {
    TempFormValue: {},
    Current: {
        Id: '',
        Id_Parent: 0,
        Id_Persons: 0,
        LangLayot:'',
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true
};
WorkData.Acceslist_Data = {
    Current: {
        Id: 0,
        User: '',
        UserGroup: '',
        Valid: ''
    }
};


var Postdata = {};
// funct
var urlf = pageVariable.baseSiteURL + "Admin/GetFunctionhier";
var sourcef = {
    dataType: "json",
    dataFields: [{
        name: 'Id',
        type: 'string'
    }, {
        name: 'Parent',
        type: 'string'
    }, {
        name: 'Type',
        type: 'string'
    }, {
        name: 'ItemName',
        type: 'string'
    }, {
        name: 'Desc',
        type: 'string'
    }, {
        name: 'GroupName',
        type: 'string'
    }, {
        name: 'Flag',
        type: 'string'
    }, {
        name: 'Param',
        type: 'string'
    }, {
        name: 'Template',
        type: 'string'
    }, {
        name: 'icon',
        type: 'string'
    }
    ],
    id: 'Id',
    hierarchy: {
        keyDataField: {
            name: 'Id'
        },
        parentDataField: {
            name: 'Parent'
        }
    },
    url: urlf
};
var Selectedgrowf = null;
var dataAdapterf = new $.jqx.dataAdapter(sourcef, {
    loadComplete: function () {
        var row = Selectedgrowf;
        if (row) {
            var i = 0;
            while (i < 10) {
                $("#Functgrid").jqxTreeGrid('expandRow', row.Id);
                if (row.Parent == 0)
                    break;
                row = row.parent;
                i++;
            }
        }
    }
});
$("#Functgrid").jqxTreeGrid({
    source: dataAdapterf,
    columnsResize: true,
    width: '100%',
    height: 600,
    theme: pageVariable.Jqwtheme,
    altRows: true,
    icons: true,
    ready: function () { },
    columns: [{
        text: 'Id',
        dataField: 'Id',
        hidden: true
    }, {
        text: 'Parent',
        dataField: 'Parent',
        hidden: true
    }, {
        text: 'Type',
        dataField: 'Type',
        width: 150
    }, {
        text: 'ItemName',
        dataField: 'ItemName',
        cellsRenderer: function (rowKey, dataField, value, data) {
            if (data.Type == 'Transition' || data.Type == 'Role') {
                return value + ' (' + data.GroupName + ')';
            } else {
                return value;
            }
        }
    }, {
        text: 'Desc',
        dataField: 'Desc',
        hidden: true
    }, {
        text: 'GroupName',
        dataField: 'GroupName',
        hidden: true
    }, {
        text: 'Flag',
        dataField: 'Flag',
        hidden: true
    }, {
        text: 'Param',
        dataField: 'Param',
        hidden: true
    }, {
        text: 'Template',
        dataField: 'Template',
        hidden: true
    }
    ],
    columnGroups: [{
        text: 'ItemName',
        name: 'ItemName'
    }
    ]
});
$("#EditFunction").on('click', function () {
    var row = $("#Functgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Funct') {
        $("#Functionform #OrigFunctionName").val(row.ItemName);
        $("#FunctionName").val(row.ItemName);
        $("#FunctionDesc").val(row.Desc);
        $("#FunctionGroup").val(row.GroupName);
        $("#Functionparam").val(row.Param);
        $("#FunctionModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveFunctiondata").on('click', function () {
    $("#Functionform").mYPostForm("Admin/ModifyFNode", {
        Oper: "EditFunction"
    }, function (Data) {
        var row = $("#Functgrid").jqxTreeGrid('getSelection')[0];
        row.ItemName = $("#FunctionName").val();
        row.Desc = $("#FunctionDesc").val();
        row.GroupName = $("#FunctionGroup").val();
        row.Template = $("#Functionparam").val();
        $("#Functgrid").jqxTreeGrid('updateRow', row.uid, row);
        $("#FunctionModal").modal('hide');
    });
});
$("#AddFrole").on('click', function () {
    var row = $("#Functgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Funct') {
        $("#RoleFunctform #OrigFunctionName").val(row.ItemName);
        $("#RoleFunctform #Rolefname").DXjqxComboBoxReload({
            ajax_data: {
                curfunct: row.ItemName
            }
        });
        $("#RoleFModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveRoleFdata").on('click', function () {
    Selectedgrowf = $("#Functgrid").jqxTreeGrid('getSelection')[0];
    $("#RoleFunctform").mYPostForm("Admin/ModifyFNode", {
        Oper: "AddRole"
    }, function (Data) {
        $("#Functgrid").jqxTreeGrid('updateBoundData');
        $("#RoleFModal").modal('hide');
    });
});
$("#DelFrole").on('click', function () {
    var row = $("#Functgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Role') {
        $("#RoleFunctform").mYPostForm("Admin/ModifyFNode", {
            Oper: "DelRole",
            OrigFunctionName: row.parent.ItemName,
            Rolefname: row.ItemName
        }, function (Data) {
            $("#Functgrid").jqxTreeGrid('deleteRow', row.uid);
        });
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
//GetcombosourceFRoles
var RoleFCombo = {
    url: pageVariable.baseSiteURL + 'Admin/GetcombosourceFRoles',
    ajax_data: {
        curfunct: ''
    },
    param: {
        theme: pageVariable.Jqwtheme
    }
};
$("#RoleFunctform #Rolefname").DXjqxComboBox(RoleFCombo);
// flows
var url = pageVariable.baseSiteURL + "Admin/GetFlowshier";
var sourcexx = {
    dataType: "json",
    dataFields: [{
        name: 'Id',
        type: 'string'
    }, {
        name: 'Parent',
        type: 'string'
    }, {
        name: 'Type',
        type: 'string'
    }, {
        name: 'ItemName',
        type: 'string'
    }, {
        name: 'Desc',
        type: 'string'
    }, {
        name: 'GroupName',
        type: 'string'
    }, {
        name: 'Flag',
        type: 'string'
    }, {
        name: 'Template',
        type: 'string'
    }, {
        name: 'icon',
        type: 'string'
    }
    ],
    id: 'Id',
    hierarchy: {
        keyDataField: {
            name: 'Id'
        },
        parentDataField: {
            name: 'Parent'
        }
    },
    url: url
};
var Selectedgrow = null;
var dataAdapterxx = new $.jqx.dataAdapter(sourcexx, {
    loadComplete: function () {
        var row = Selectedgrow;
        if (row) {
            var i = 0;
            while (i < 10) {
                $("#Flowstgrid").jqxTreeGrid('expandRow', row.Id);
                if (row.Parent == 0)
                    break;
                row = row.parent;
                i++;
            }
        }
    }
});
$("#Flowstgrid").jqxTreeGrid({
    source: dataAdapterxx,
    columnsResize: true,
    width: '100%',
    height: 600,
    theme: pageVariable.Jqwtheme,
    altRows: true,
    icons: true,
    ready: function () { },
    columns: [{
        text: 'Id',
        dataField: 'Id',
        hidden: true
    }, {
        text: 'Parent',
        dataField: 'Parent',
        hidden: true
    }, {
        text: 'Type',
        dataField: 'Type',
        width: 150
    }, {
        text: 'ItemName',
        dataField: 'ItemName',
        cellsRenderer: function (rowKey, dataField, value, data) {
            if (data.Type == 'Transition' || data.Type == 'Role') {
                return value + ' (' + data.GroupName + ')';
            } else {
                return value;
            }
        }
    }, {
        text: 'Desc',
        dataField: 'Desc',
        hidden: true
    }, {
        text: 'GroupName',
        dataField: 'GroupName',
        hidden: true
    }, {
        text: 'Flag',
        dataField: 'Flag',
        hidden: true
    }, {
        text: 'Template',
        dataField: 'Template',
        hidden: true
    }
    ],
    columnGroups: [{
        text: 'ItemName',
        name: 'ItemName'
    }
    ]
});
$("#Editflow").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Flow') {
        $("#Flowform #OrigFlowName").val(row.ItemName);
        $("#FlowName").val(row.ItemName);
        $("#FlowDesc").val(row.Desc);
        $("#FlowGroup").val(row.GroupName);
        $("#FlowTemplate").val(row.Template);
        $("#FlowModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveFlowdata").on('click', function () {
    $("#Flowform").mYPostForm("Admin/ModifyNode", {
        Oper: "EditFlow"
    }, function (Data) {
        var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
        row.ItemName = $("#FlowName").val();
        row.Desc = $("#FlowDesc").val();
        row.GroupName = $("#FlowGroup").val();
        row.Template = $("#FlowTemplate").val();
        $("#Flowstgrid").jqxTreeGrid('updateRow', row.uid, row);
        $("#FlowModal").modal('hide');
    });
});
$("#Editstep").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Step') {
        
        $("#Stepform #OrigFlowName").val(row.parent.ItemName);
        $("#Stepform #OrigStepName").val(row.ItemName);
        $("#StepName").val(row.ItemName);
        $("#StepDesc").val(row.Desc);
        $("#Flag").val(row.Flag);
        $("#StepTemplate").val(row.Template);
        $("#StepModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveStepdata").on('click', function () {

    $("#Stepform").mYPostForm("Admin/ModifyNode", {
        Oper: "EditStep"
    }, function (Data) {

        var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
        row.ItemName = $("#StepName").val();
        row.Desc = $("#StepDesc").val();
        row.Flag = $("#Flag").val();
        row.Template = $("#StepTemplate").val();
        $("#Flowstgrid").jqxTreeGrid('updateRow', row.uid, row);
        $("#StepModal").modal('hide');
    });
});
$("#Addtransition").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'tHead') {
        $("#Transform #OrigFlowName").val(row.parent.parent.ItemName);
        $("#Transform #OrigStepName").val(row.parent.ItemName);
        $("#Transform #ToStep").DXjqxComboBoxReload({
            ajax_data: {
                filter: row.parent.parent.ItemName,
                curstep: row.parent.ItemName
            }
        });
        $("#TransitionModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveTransitiondata").on('click', function () {
    Selectedgrow = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    $("#Transform").mYPostForm("Admin/ModifyNode", {
        Oper: "AddTransaction"
    }, function (Data) {
        $("#Flowstgrid").jqxTreeGrid('updateBoundData');
        $("#TransitionModal").modal('hide');
    });
});
$("#Deltransition").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Transition') {
        $("#Transform").mYPostForm("Admin/ModifyNode", {
            Oper: "DelTransaction",
            OrigFlowName: row.parent.parent.parent.ItemName,
            OrigStepName: row.parent.parent.ItemName,
            ToStep: row.ItemName
        }, function (Data) {
            $("#Flowstgrid").jqxTreeGrid('deleteRow', row.uid);
        });
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
var StepsCombo = {
    url: pageVariable.baseSiteURL + 'Admin/GetcombosourceCursteps',
    ajax_data: {
        filter: '',
        curstep: ''
    },
    param: {
        theme: pageVariable.Jqwtheme
    }
};
$("#Transform #ToStep").DXjqxComboBox(StepsCombo);
$("#Addrole").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'rHead') {
        $("#Roleform #OrigFlowName").val(row.parent.parent.ItemName);
        $("#Roleform #OrigStepName").val(row.parent.ItemName);
        $("#Roleform #Rolename").DXjqxComboBoxReload({
            ajax_data: {
                filter: row.parent.parent.ItemName,
                curstep: row.parent.ItemName
            }
        });
        $("#RoleModal").modal('show');
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
$("#btnSaveRoledata").on('click', function () {
    Selectedgrow = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    $("#Roleform").mYPostForm("Admin/ModifyNode", {
        Oper: "AddRole"
    }, function (Data) {
        $("#Flowstgrid").jqxTreeGrid('updateBoundData');
        $("#RoleModal").modal('hide');
    });
});
$("#Delrole").on('click', function () {
    var row = $("#Flowstgrid").jqxTreeGrid('getSelection')[0];
    if (row && row.Type == 'Role') {
        $("#Roleform").mYPostForm("Admin/ModifyNode", {
            Oper: "DelRole",
            OrigFlowName: row.parent.parent.parent.ItemName,
            OrigStepName: row.parent.parent.ItemName,
            Rolename: row.ItemName
        }, function (Data) {
            $("#Flowstgrid").jqxTreeGrid('deleteRow', row.uid);
        });
    } else {
        GlobError("Nincs kivalasztva", 20);
    }
});
var RoleCombo = {
    url: pageVariable.baseSiteURL + 'Admin/GetcombosourceRoles',
    ajax_data: {
        filter: '',
        curstep: ''
    },
    param: {
        theme: pageVariable.Jqwtheme
    }
};
$("#Roleform #Rolename").DXjqxComboBox(RoleCombo);
// ----------------------------------------------------------------------------------------------------

// ----------------------------------------------------------------------------------------------------

var Vw_Users_columns = [
//{ text: '§Id§', datafield: 'Id' },
//{ text: '§Id_Vw_Users§', datafield: 'Id_Vw_Users' },
{ text: '§Username§', datafield: 'Username', editable:false },
{ text: '§Usedname§', datafield: 'Usedname', editable: false },
{ text: '§Email§', datafield: 'Email', editable: false },
{
    text: '§Preferedlang§', datafield: 'Preferedlang', columntype: 'dropdownlist',
    createeditor: function (row, column, editor) {
        var list = ['Hu.Default.Default', 'Hu.Bm.Bm', 'Hu.Aed.Aed', 'Hu.Personal.Personal', 'Hu.Special.Special', 'Hu.Riska.Riska'];
        editor.jqxDropDownList({
            autoDropDownHeight: true,
            source: list
        });
        editor[0].onchange = function (event) {
            var args = event.args;
            // index represents the item's index.                      
            var index = args.index;
            var item = args.item;
            // get item's label and value.
            var label = item.label;
            var value = item.value;
            WorkData.Vw_Users_Data.Current.LangLayot = label;
        };
    },
    cellvaluechanging: function (row, column, columntype, oldvalue, newvalue) {
        if (newvalue == "") return oldvalue;
        WorkData.Vw_Users_Data.Current.LangLayot = newvalue;
    }
}
];
var Vw_Users_datafields = [
  { name: 'Id', type: 'string'},
  { name: 'Id_Vw_Users', type: 'decimal'},
  { name: 'Username', type: 'string'},
  { name: 'Usedname', type: 'string'},
  { name: 'Email', type: 'string'},
  { name: 'Preferedlang', type: 'string'}
];
// Urls constans
var InsertXform_Vw_Users = '/InsertXform_Vw_Users';
var DeleteXform_Vw_Users = '/DeleteXform_Vw_Users';
var GetRecords_Vw_Users = '/GetRecords_Vw_Users';
//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Admin' + url;
    }
}
function fnSetXform_Vw_Users(Data, isView) {
    $("#divXformHtml_Vw_Users").html(Data.Xform);
    if (isView) $("#divXformHtmlView_Vw_Users").html(Data.XformView);
    $("#frmXform_Vw_Users .editor").jqte();
    JqwTransformWithRole('frmXform_Vw_Users');
    try {
        $('#frmXform_Vw_Users').jqxValidator({
            onSuccess: function () { WorkData.Vw_Users_Data.Valid = true; },
            onError: function () { WorkData.Vw_Users_Data.Valid = false; },
            rules: JSON.parse(JSON.stringify(frmXform_Vw_Usersrules)), hintType: "label"
        });
    } catch (err) { }
};
function fnInsertXform_Vw_Users() {
    $("#frmXform_Vw_Users").jqxValidator('validate');
    if (WorkData.Vw_Users_Data.Current.Id_Parent != null && WorkData.Vw_Users_Data.Valid == true) {
        $("#frmXform_Vw_Users").mYPostFormNew(urls(InsertXform_Vw_Users), {
            Id_Parent: WorkData.Vw_Users_Data.Current.Id, selector: 'Aschema'
        }, function (Data) {
            WorkData.Vw_Users_Data.Current.Id = Data.Entity.Username;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_UsersTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Vw_Users').DXjqxTreeReload({Id: 0,recordtype: ''});
                $("#divGrid_Vw_Users").DXjqxGridReload({ filter: $("#inpVw_Users_Presearch").val() });


                $("#divCreateOrEditModal_Vw_Users").modal('hide');
                //DXSetState('Uploadmode');
            }
        });
    } else {
        GlobError("InsertXform Vw_Users ValidateError!", 10);
        Titlemess($("#SelectedVw_UsersTitle"), "ValidateError!");
    }
};
function fnDeleteXform_Vw_Users() {
    if (dlgAreYouSoure) {
        AjaxGet(urls(DeleteXform_Vw_Users), { Id_Entity: WorkData.Vw_Users_Data.Current.Id }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_UsersTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Vw_Users').DXjqxTreeReload({Id: 0,recordtype: ''});
                $("#divGrid_Vw_Users").DXjqxGridReload({ filter: $("#inpVw_Users_Presearch").val() });
                $("#SelectedVw_UsersTitle1").html('Selected: -');
                WorkData.Vw_Users_Data.Current.Id = null;
                //$("#divCreateOrEditModal_Vw_Users").modal('hide');
                //DXSetState('Uploadmode');
            }
        });
    } else {
        //
    }
};
function fnNewXform_Vw_Users() {
    AjaxGet(urls(Xform_NewXformRoot_Url),{                  //+'Id'), { !!!! option
        root: WorkData.Vw_Users_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Vw_Users', selector: 'Aschema'
    }, function (Data) {
        fnSetXform_Vw_Users(Data, false);
        $.each(WorkData.Vw_Users_Data.TempFormValue, function (key, value) {
            $("#frmXform_Vw_Users input[id*='" + key + "']").val(value);
        });
        WorkData.Vw_Users_Data.TempFormValue = {}; 
    });
};

function fnPasswordReset() {
    try { $("#frmXform_Vw_Users").jqxValidator('validate'); } catch (err) { }
    if (WorkData.Vw_Users_Data.Valid) {
        $("#frmXform_Vw_Users").mYPostFormNew(urls(PasswordReset), {
            Recordtype: 'Entity', selector: 'Aschema', Username:WorkData.Vw_Users_Data.Current.Id
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_UsersTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Vw_Users').DXjqxTreeReload({Id: 0,recordtype: ''});
                $("#divCreateOrEditModal_Vw_Users").modal('hide');
                //DXSetState('Uploadmode');
            }
        });
    } else {
        GlobError("SaveXform Vw_Users Error!", 10);
    }

};

function fnBind_btnSaveXform_Vw_Users(process_function) {
    $("#btnSaveXform_Vw_Users").off('click');
    $("#btnSaveXform_Vw_Users").on('click', function (e) {
        process_function();
    });
};
// Html element binding functions
// --------------------------------------------------------------------------------------------

// Grid 
function fnloadCompleteGrid_Vw_Users(data) {
    if (data.length > 0) {

    } else {
        Titlemess($("#SelectedVw_UsersTitle"), "§NotFound§!");
    }
};
var JqxParam_Grid_Vw_Users = {
    url: urls(GetRecords_Vw_Users),
    datafields: Vw_Users_datafields,
    ajax_data: { filter: '' },
    loadComplete: fnloadCompleteGrid_Vw_Users,
    param: {
        columns: Vw_Users_columns,
        width: '100%',
        theme: pageVariable.Jqwtheme,
        localization: getLocalization('hu'),
        columnsresize: true,
        pageable: true,
        editable: true,
        //selectionmode: 'singlecell',
        //editmode: 'click',
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        showtoolbar: true,
        rowdetails: false,
        rendertoolbar: function (toolbar) {
            var container = '<ul class="nav">';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnNewXform_Vw_Users"><i class="glyphicon glyphicon glyphicon-plus pull-left"></i> NewUsers</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnUnbindPersons_Vw_Users"><i class="glyphicon glyphicon glyphicon-plus pull-left"></i> UnbindPersons</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnDeleteXform_Vw_Users"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> DeleteUsers</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnPasswordreset_Vw_Users"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> Passwordreset</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnSetApp_Vw_Users"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> SetApp</a></li>';
            container += '</ul>';
            toolbar.append(container);
            fnGridToolbarReady_Vw_Users();
        },
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Vw_Users").DXjqxGrid(JqxParam_Grid_Vw_Users);
$('#divGrid_Vw_Users').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Vw_Users_Data.Current.Id = args.row.bounddata.Username;
    WorkData.Vw_Users_Data.Current.LangLayot = args.row.bounddata.Preferedlang;
    if (args.row.bounddata.Id_Vw_Users <= 0) {
        $("#FindPersonsModal").modal('show');
    }
});
$('#divGrid_Vw_Users').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Vw_Users_Data.Current.Id = args.row.Username;
    WorkData.Vw_Users_Data.Current.LangLayot = args.row.Preferedlang;
});
$('#divGrid_Vw_Users').on('rowselect', function (event) {
    // event arguments.
    $("#jqxList_Systemroles").jqxListBox('clear');
    var args = event.args;
    WorkData.Vw_Users_Data.Current.Id = args.row.Username; //Id_Vw_Users;

    WorkData.Vw_Users_Data.Current.Id_Persons = args.row.Id_Vw_Users;

    WorkData.Vw_Users_Data.Current.LangLayot = args.row.Preferedlang;
    $("#SelectedVw_UsersTitle1").html('Selected: ' + args.row.Username);

    listboxLoaded = 0;
    $("#BodyLoader").jqxLoader('open');
    //$("[id^='jqxList']").hide();
    $("[id^='jqxList']").show();
    $("[id^='jqxList']").fadeTo(100, 0.2);
    setTimeout(fnListcomplett, 500);

    SystemrolesdataAdapter._source.data = { Userid: WorkData.Vw_Users_Data.Current.Id };
    SystemrolesdataAdapter.dataBind();

    UserOrggroupdataAdapter._source.data = { Userid: WorkData.Vw_Users_Data.Current.Id };
    UserOrggroupdataAdapter.dataBind();

    UserUsergroupdataAdapter._source.data = { Userid: WorkData.Vw_Users_Data.Current.Id };
    UserUsergroupdataAdapter.dataBind();


});
function fnGridToolbarReady_Vw_Users() {
    $("#btnNewXform_Vw_Users").on('click', function (e) {
        if (1 == 1) {
            //$("#BodyLoader").jqxLoader('open');
            //$('#cmbXformRootSelect_Vw_Users').jqxComboBox('clearSelection');
            WorkData.Vw_Users_Data.Current.Root = 'Vw_Users';
            fnNewXform_Vw_Users();
            fnBind_btnSaveXform_Vw_Users(fnInsertXform_Vw_Users);

            //$("#BodyLoader").jqxLoader('close');
            $("#divCreateOrEditModal_Vw_Users").modal('show');
        } else {
            //
        }
    });
    $("#btnDeleteXform_Vw_Users").on('click', function (e) {
        dlgAreYouSoure(function (confirm) {
            if (confirm) {
                //var item = $('#divTree_Vw_Users').jqxTree('getSelectedItem');
                var rowindex = $('#divGrid_Vw_Users').jqxGrid('getselectedrowindex');
                if (rowindex != -1) {
                    fnDeleteXform_Vw_Users();
                } else {
                    GlobError("DeleteXform Vw_Users Error!", 11);
                }
            } else {
            }
        });


    });
    $("#btnUnbindPersons_Vw_Users").on('click', function (e) {
        var rowindex = $('#divGrid_Vw_Users').jqxGrid('getselectedrowindex');
        if (rowindex != -1) {
            var data = $('#divGrid_Vw_Users').jqxGrid('getrowdata', rowindex);
            AjaxGet(urls(Update_PersonsUserid), {
                Id_Entity: data.Id_Vw_Users,
                Userid: null,
                LangLayout:null
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedVw_UsersTitle1"), Data.Error.Errormessage);
                } else {
                    $("#divGrid_Vw_Users").DXjqxGridReload({ filter: $("#inpVw_Users_Presearch").val() });
                }
            });
        } else {
            GlobError("DeleteXform Vw_Users Error!", 11);
        }
    });
    $("#btnPasswordreset_Vw_Users").on('click', function (e) {
        //var item = $('#divTree_Vw_Users').jqxTree('getSelectedItem');
        var rowindex = $('#divGrid_Vw_Users').jqxGrid('getselectedrowindex');
        if (rowindex != -1) {
            WorkData.Vw_Users_Data.Current.Root = 'Vw_PasswordReset';
            fnNewXform_Vw_Users();
            fnBind_btnSaveXform_Vw_Users(fnPasswordReset);
            WorkData.Vw_Users_Data.TempFormValue = { 'Username': WorkData.Vw_Users_Data.Current.Id };
            //$("input[id*=Username]").val(WorkData.Vw_Users_Data.Current.Id);
            //$("#BodyLoader").jqxLoader('close');
            $("#divCreateOrEditModal_Vw_Users").modal('show');

        } else {
            GlobError("DeleteXform Vw_Users Error!", 11);
        }
    });
    //btnSetApp_Vw_Users
    $("#btnSetApp_Vw_Users").on('click', function (e) {
        //var item = $('#divTree_Vw_Users').jqxTree('getSelectedItem');
        var rowindex = $('#divGrid_Vw_Users').jqxGrid('getselectedrowindex');
        if (rowindex != -1) {
            var id = WorkData.Vw_Users_Data.Current.Id_Persons;
            //var data = $('#divGrid_Vw_Users').jqxGrid('getrowdata', rowindex);
            AjaxGet(pageVariable.baseSiteURL + "Admin/Update_Appselector", { id_person: id, appselector: WorkData.Vw_Users_Data.Current.LangLayot }, function (Data) {

            });
        } else {
            GlobError("DeleteXform Vw_Users Error!", 11);
        }
    });
};

$("#divGrid_Vw_Users").on('cellendedit', function (event) {
    var args = event.args;
    
    //WorkData.Vw_Users_Data.Current.LangLayot = newvalue;
    //$("#cellendeditevent").text("Event Type: cellendedit, Column: " + args.datafield + ", Row: " + (1 + args.rowindex) + ", Value: " + args.value);
});

var Update_PersonsUserid = '/Update_PersonsUserid';
var PasswordReset = '/PasswordReset';

$("#btnFindPersonsModalSave").on('click', function () {
    if (WorkData.Persons_Data.Current.Id > 0) {

        AjaxGet(urls(Update_PersonsUserid), {
            Id_Entity: WorkData.Persons_Data.Current.Id,
            Userid: WorkData.Vw_Users_Data.Current.Id,
            LangLayout:WorkData.Vw_Users_Data.Current.LangLayot
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_UsersTitle1"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Vw_Users").DXjqxGridReload({ filter: $("#inpVw_Users_Presearch").val() });
                $("#FindPersonsModal").modal('hide');
            }
        });


    } else {
        Titlemess($("#SelectedPersonsTitle"), "Pleaas Select befor close!");
    }
});
$("#btnVw_Users_Presearch").on('click', function () {
       $("#divGrid_Vw_Users").DXjqxGridReload({ filter: $("#inpVw_Users_Presearch").val() });
});

//-----------------------------------------------------------------------------------------------------
var listboxLoaded = 0;
$("[id^='jqxList']").hide();
function fnListcomplett() {
    if (listboxLoaded >= 3) {
        //$("[id^='jqxList']").show();
        $("[id^='jqxList']").fadeTo(100, 1);
        $("#BodyLoader").jqxLoader('close');
        return;
    } else {
        setTimeout(fnListcomplett, 500);
    }
}

function fnListboxBindingComplett(listbox, url, data,setfunct) {
    var adapter = new $.jqx.dataAdapter({ autoBind: false, datatype: "json", datafields: [{ name: 'Key' }, { name: 'Value' }, { name: 'Checked' }], id: 'Key', url: urls(url), data: data});// { Userid: '' } });
    listbox.jqxListBox({ source: adapter, displayMember: "Value", valueMember: "Key", width: 200, height: 200, checkboxes: true, theme: pageVariable.Jqwtheme });

    listbox.on('bindingComplete', function (event) {
        listbox.jqxListBox('beginUpdate');
        var items = listbox.jqxListBox('getItems');
        $(items).each(function (index, item) {
            item.checked = (item.originalItem.Checked);
            if (item.originalItem.Checked)
                listbox.jqxListBox('checkIndex', index);
        });
        listbox.jqxListBox('endUpdate');

        listbox.off('checkChange');
        listbox.on('checkChange', function (event) {
            listbox.jqxListBox('beginUpdate');
            var args = event.args;
            var cmd = '';
            if (args.checked) {
                cmd = 'Add';
            } else {
                cmd = 'Del';
            }
            setfunct(listbox, args, cmd);
        });
        listboxLoaded++;
    });
    return adapter;
};

// -----------------------------------------------------------------
function fnSetSystemRole(listbox, args, cmd) {
    AjaxGet(urls('/SetSystemRole'), { Userid: WorkData.Vw_Users_Data.Current.Id, role: args.label, roleid: args.item.value, cmd: cmd }, function (Data) {
        listbox.jqxListBox('endUpdate');
    });
};
var SystemrolesdataAdapter = fnListboxBindingComplett($("#jqxList_Systemroles"), '/GetSystemRoles', { Userid: '' }, fnSetSystemRole);

function fnSetUserOrggroup(listbox, args, cmd) {
    AjaxGet(urls('/SetUserOrggroup'), { Userid: WorkData.Vw_Users_Data.Current.Id, role: args.label, roleid: args.item.value, cmd: cmd }, function (Data) {
        listbox.jqxListBox('endUpdate');
    });
};
var UserOrggroupdataAdapter = fnListboxBindingComplett($("#jqxList_UserOrggroup"), '/GetUserOrggroup', { Userid: '' }, fnSetUserOrggroup);

function fnSetUserUsergroup(listbox, args, cmd) {
    AjaxGet(urls('/SetUserUsergroup'), { Userid: WorkData.Vw_Users_Data.Current.Id, role: args.label, roleid: args.item.value, cmd: cmd }, function (Data) {
        listbox.jqxListBox('endUpdate');
    });
};
var UserUsergroupdataAdapter = fnListboxBindingComplett($("#jqxList_UserUsergroup"), '/GetUserUsergroup', { Userid: '' }, fnSetUserUsergroup);

// Usergroup -X- Roles    -----------------------------------------------------------------
var UsergroupdataAdapter = new $.jqx.dataAdapter({  datatype: "json", datafields: [{ name: 'Id' }, { name: 'Groupname' }], id: 'Id', url: urls('/GetUsergroup'), data: {} });
$("#jqxxList_Usergroup").jqxListBox({ source: UsergroupdataAdapter, displayMember: "Groupname", valueMember: "Id", width: 200, height: 360, theme: pageVariable.Jqwtheme, filterable: true });
$('#jqxxList_Usergroup').on('select', function (event) {
    var args = event.args;
    if (args) {
        var item = args.item;
        var value = item.value;
        WorkData.Acceslist_Data.Current.UserGroup = item.label;

        UsergroupRolesdataAdapter._source.data = { Usergroup: WorkData.Acceslist_Data.Current.UserGroup };
        UsergroupRolesdataAdapter.dataBind();
    }
});


function fnSetUsergroupRoles(listbox, args, cmd) {
    AjaxGet(urls('/SetUserGroupRoles'), { Usergroup: WorkData.Acceslist_Data.Current.UserGroup, role: args.label, roleid: args.item.value, cmd: cmd }, function (Data) {
        listbox.jqxListBox('endUpdate');
    });
};
var UsergroupRolesdataAdapter = fnListboxBindingComplett($("#jqxxList_UsergroupRoles"), '/GetUserGroupRoles', { Usergroup: '' }, fnSetUsergroupRoles);

// ----------------------------------------------------------------------------------------------------
include('~/Views/Admin/NewAdmin.js' | args(0, 0));
include('~/Views/Base/NUpload.js' | args(0, 0));

// Show view
$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[6]).addClass('active');
