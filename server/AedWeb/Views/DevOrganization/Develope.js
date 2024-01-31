include('~/Scripts/DextraFlowManParial.js' | args(0, 0));
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

var CheckPostData = function () {
    WorkData.ClientPartName = 'Org';
    WorkData.model_ClientPart.retval = c_FlowPostpreCheckretOk;
    return WorkData.model_ClientPart;
};

function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'DevOrganization' + url;
    }
}

// Organization (Main Entiy) ----------------------------------------------------------------------------------------------------------------------------------
var GetXform_Organization = '/GetXform_Organization';
var InsertXform_Organization = '/InsertXform_Organization';
var SaveXform_Organization = '/SaveXform_Organization';
var DeleteXform_Organization = '/DeleteXform_Organization';
var GetRecords_Organization = '/GetRecords_Organization';
var GetRecords_Organization = '/GetRecords_Organization_Session';
var GetHierarchy_Organization = '/GetHierarchy_Organization';
var MoveHierarchyItem_Organization = '/MoveHierarchyItem_Organization';
var GetPreselect_Organization = '/GetHierarchy_Organization';
// Datastruct
WorkData.Organization_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {}
};
// Columns and record def
var Organization_columns = [{
    text: '§Id_Organization§',
    datafield: 'Id_Organization'
}, {
    text: '§Id_Parent§',
    datafield: 'Id_Parent'
}, {
    text: '§Id_Ord§',
    datafield: 'Id_Ord'
}, {
    text: '§Bid_Organization§',
    datafield: 'Bid_Organization'
}, {
    text: '§Name§',
    datafield: 'Name'
}, {
    text: '§Title§',
    datafield: 'Title'
}, {
    text: '§Shortname§',
    datafield: 'Shortname'
}, {
    text: '§Xmldata§',
    datafield: 'Xmldata'
}, {
    text: '§Recordtype§',
    datafield: 'Recordtype'
}, {
    text: '§Id_Flows§',
    datafield: 'Id_Flows'
}, {
    text: '§Cpyid§',
    datafield: 'Cpyid'
}, {
    text: '§Cpyparrent§',
    datafield: 'Cpyparrent'
}, {
    text: '§Id_Files§',
    datafield: 'Id_Files'
}, {
    name: '§Property§',
    type: 'Property'
}
];
var Organization_datafields = [{
    name: 'Id_Organization',
    type: 'decimal'
}, {
    name: 'Id_Parent',
    type: 'decimal'
}, {
    name: 'Id_Ord',
    type: 'decimal'
}, {
    name: 'Orgtype',
    type: 'string'
}, {
    name: 'Bid_Organization',
    type: 'string'
}, {
    name: 'Name',
    type: 'string'
}, {
    name: 'Title',
    type: 'string'
}, {
    name: 'Shortname',
    type: 'string'
}, {
    name: 'Xmldata',
    type: 'string'
}, {
    name: 'Recordtype',
    type: 'string'
}, {
    name: 'Id_Flows',
    type: 'decimal'
}, {
    name: 'Cpyid',
    type: 'decimal'
}, {
    name: 'Cpyparrent',
    type: 'decimal'
}, {
    name: 'Id_Files',
    type: 'decimal'
}, {
    name: 'Property',
    type: 'string'
}
];
// Database functions
function fnSetXform_Organization(Data, isView) {
    $("#divXformHtml_Organization").html(Data.Xform);
    if (isView)
        fnSetCommonView("Organization", Data.XformView, "text-primary");
    $("#frmXform_Organization .editor").jqte();
    JqwTransformWithRole('frmXform_Organization');
    try {
        $('#frmXform_Organization').jqxValidator({
            onSuccess: function () {
                WorkData.Organization_Data.Valid = true;
            },
            onError: function () {
                WorkData.Organization_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Organizationrules)),
            hintType: "label"
        });
    } catch (err) { }
};
function fnInsertXform_Organization() {
    $("#frmXform_Organization").jqxValidator('validate');
    if (WorkData.Organization_Data.Current.Id_Parent != null && WorkData.Organization_Data.Valid == true) {
        $("#frmXform_Organization").mYPostFormNew(urls(InsertXform_Organization), {
            Id_Parent: WorkData.Organization_Data.Current.Id,
            Id_ParentId_Parent: WorkData.Organization_Data.Current.Id_Parent,
            Recordtype: "recordtype",
            Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow
        }, function (Data) {
            WorkData.Organization_Data.Current.Id = Data.Entity.Id_Organization;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedOrganizationTitle"), Data.Error.Errormessage);
            } else {
                $('#divTree_Organization').DXjqxTreeReload({
                    Id: WorkData.model_CurrentFlowstep.Id_Flow,
                    level: WorkData.Organization_Data.Level,
                    recordtype: 'OrgItem'
                });

                $("#divCreateOrEditModal_Organization").modal('hide');
            }
        });
    } else {
        GlobError("InsertXform Organization ValidateError!", 10);
    }
};
function fnSaveXform_Organization() {
    try {
        $("#frmXform_Organization").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Organization_Data.Valid) {
        $("#frmXform_Organization").mYPostFormNew(urls(SaveXform_Organization), {
            Id_Entity: WorkData.Organization_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedOrganizationTitle"), Data.Error.Errormessage);
            } else {
                $('#divTree_Organization').DXjqxTreeReload({
                    Id: WorkData.model_CurrentFlowstep.Id_Flow,
                    level: WorkData.Organization_Data.Level,
                    recordtype: 'OrgItem'
                });

                $("#divCreateOrEditModal_Organization").modal('hide');
            }
        });
    } else {
        GlobError("SaveXform Organization Error!", 10);
    }
};
function fnDeleteXform_Organization() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Organization), {
                Id_Organization: WorkData.Organization_Data.Current.Id,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedOrganizationTitle"), Data.Error.Errormessage);
                } else {
                    $('#divTree_Organization').DXjqxTreeReload({
                        Id: WorkData.model_CurrentFlowstep.Id_Flow,
                        level: WorkData.Organization_Data.Level,
                        recordtype: 'OrgItem'
                    });
                    $("#divCreateOrEditModal_Organization").modal('hide');
                }
            });
        } else { }
    });
};
function fnGetXform_Organization() {
    WorkData.Organization_Data.Current.ChangedName = WorkData.Organization_Data.Current.Name;
    AjaxGet(urls(GetXform_Organization), {
        Id_Entity: WorkData.Organization_Data.Current.Id,
        Id_Xform: 'frmXform_Organization'
    }, function (Data) {
        fnSetXform_Organization(Data, true);
    });
};
function fnNewXform_Organization() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Organization_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Organization'
    }, function (Data) {
        fnSetXform_Organization(Data, false);
        $.each(WorkData.Organization_Data.TempFormValue, function (key, value) {
            $("#frmXform_Organization input[id*='" + key + "']").val(value);
        });
        WorkData.Organization_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Organization() {
    $("#frmXform_Organization").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Organization',
        NewXformRoot: WorkData.Organization_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Organization(Data);
    });
};
// Xform Selector  combo
function fnBind_cmbXformRootSelect_CangeEvent_Organization() {
    $('#cmbXformRootSelect_Organization').off('change');
    $('#cmbXformRootSelect_Organization').on('change', function (event) {
        if (event.args) {
            var item = event.args.item;
            WorkData.Organization_Data.Current.Root = item.value;
            fnXformChangeRoot_Organization();
        }
    });
};
$("#cmbXformRootSelect_Organization").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'OrgRoots',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Organization_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_Organization();
        } catch (Error) { }
    }
});
// Tree  specific Contextmenu
var TreeContextMenu_Organization = [{
    id: 'Exp',
    html: '§ExpandAll§'
}, {
    id: 'Coll',
    html: '§ColapseAll§'
}
];
function fnTree_ContextMenuClickEvent_Organization(event) {
    var item = event.args.id;
    switch (item) {
        case "Exp":
            $('#divTree_Organization').jqxTree('expandAll');
            break;
        case "Coll":
            $('#divTree_Organization').jqxTree('collapseAll');
            break;
    }
};
// Calbacks
// BeforeLoadCompleteRecord!!
DXjqxTreeBeforeLoadCompleteRecord = function (i, k) {

    if (k.Property == 'InTree') {
        //k.Name = '<span class="text-secondary">' + k.Name + '</span>';
        k.Name = '<span>' + k.Bid_Organization + ' ' +'</span>';
        k.icon = '../Content/icons/famsilkicons/flag_yellow.png';
    }
};
function fnJqxTreeDragStart_Organization(item) {
    return true;
};
function fnJqxTreeDragEnd_Organization(dragItem, dropItem, args, dropPosition, tree) {
    var mreload = dragItem.value.Property;
    JqxTree_DragEnd(dragItem, dropItem, dropPosition, urls(MoveHierarchyItem_Organization), function () {});
    if (mreload == 'InTree') {
        setTimeout(function () { $('#divTree_Organization').DXjqxTreeReload({ Id: WorkData.model_CurrentFlowstep.Id_Flow, level: WorkData.Organization_Data.Level, recordtype: 'OrgItem' }); }, 500);
    }
};
// Tree
var JqxTreeParam_Organization = {
    url: urls(GetHierarchy_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: WorkData.model_CurrentFlowstep.Id_Flow,
        level: WorkData.Organization_Data.Level,
        recordtype: 'OrgItem'
    },
    id_hierarchy: 'Id_Organization',
    id_parent_hierarchy: 'Id_Parent',
    mapp: [{
        name: 'Id_Organization',
        map: 'id'
    }, {
        name: 'Name',
        map: 'html'
    }, {
        name: 'Rec',
        map: 'value'
    }
    ],
    loadComplete: function (data) { },
    contextMenu: $("#divTreeContextmenu_Organization").DxjqxTreeContextmenu(TreeContextMenu_Organization, fnTree_ContextMenuClickEvent_Organization),
    urlforiconxml: "/Ajax/GetXml",
    iconxml: 'Icon-1.1.xml',
    iconref: 'Orgtype',
    param: {
        width: '100%',
        height: 300,
        theme: pageVariable.Jqwtheme,
        allowDrag: true,
        allowDrop: true,
        dragStart: fnJqxTreeDragStart_Organization,
        dragEnd: fnJqxTreeDragEnd_Organization
    }
};
$('#divTree_Organization').DXjqxTree(JqxTreeParam_Organization);
$('#divTree_Organization').on('itemClick', function (event) {
    var item = $(this).jqxTree('getItem', event.args.element);
    if (item != null) {
        WorkData.Organization_Data.Current.Id = item.value.Id_Organization;
        WorkData.Organization_Data.Current.Properties = item.value.Property;
        if (WorkData.Organization_Data.Current.Properties == 'InTree') return;
        fnGetXform_Organization();
        $("#divGrid_Status").DXjqxGridReload({
            id: WorkData.Organization_Data.Current.Id,
            recordtype: 'StatusItem'
        })
    }
});
// Tree Search
$("#btnTreeSearch_Organization").on('click', function (e) {
    $("#divTreeSesrchContainer_Organization").DXjqxTreeSearch("divTree_Organization");
});
// Bind button (new,edit,delete) modal save button !
function fnBind_btnSaveXform_Organization(process_function) {
    $("#btnSaveXform_Organization").off('click');
    $("#btnSaveXform_Organization").on('click', function (e) {
        process_function();
    });
};
function fnBindDefButtonClick_Organization() {
    $("#btnNewXform_Organization").on('click', function (e) {
        if (WorkData.Organization_Data.Current.Properties == 'InTree') return;
        fnNewXform_Organization();
        fnBind_btnSaveXform_Organization(fnInsertXform_Organization);
        $("#divCreateOrEditModal_Organization").modal('show');
    });
    $("#btnEditXform_Organization").on('click', function (e) {
        if (WorkData.Organization_Data.Current.Properties == 'InTree') return;
        fnGetXform_Organization();
        fnBind_btnSaveXform_Organization(fnSaveXform_Organization);
        $("#divCreateOrEditModal_Organization").modal('show');
    });
    $("#btnDeleteXform_Organization").on('click', function (e) {
        if (WorkData.Organization_Data.Current.Properties == 'InTree') return;
        var item = $('#divTree_Organization').jqxTree('getSelectedItem');
        if (item != null) {
            fnDeleteXform_Organization();
        } else {
            GlobError("btnDeleteXform_Organization Error", 11);
        }
    });
};
fnBindDefButtonClick_Organization();


// Status (Slave1 Entiy) ----------------------------------------------------------------------------------------------------------------------------------
var GetXform_Status = '/GetXform_Status';
var InsertXform_Status = '/InsertXform_Status';
var SaveXform_Status = '/SaveXform_Status';
var DeleteXform_Status = '/DeleteXform_Status';
var GetRecords_Status = '/GetRecords_Status';
// Datastruct
WorkData.Status_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {}
};
// Columns and record def
var Status_columns = [
    {
    text: '§Usedname§',
    datafield: 'Usedname'
}, {
    text: '§Bid_Organization§',
    datafield: 'Bid_Organization'
}, {
    text: '§Name§',
    datafield: 'Name'
}, {
    text: '§Title§',
    datafield: 'Title'
}, {
    text: '§Shortname§',
    datafield: 'Shortname'
} ];
var Status_datafields = [{
    name: 'Id_Organization',
    type: 'decimal'
}, {
    name: 'Id_Parent',
    type: 'decimal'
}, {
    name: 'Usedname',
    type: 'string'
}, {
    name: 'Id_Persons',
    type: 'decimal'
}, {
    name: 'Id_Ord',
    type: 'decimal'
}, {
    name: 'Bid_Organization',
    type: 'string'
}, {
    name: 'Name',
    type: 'string'
}, {
    name: 'Title',
    type: 'string'
}, {
    name: 'Shortname',
    type: 'string'
}, {
    name: 'Orgtype',
    type: 'string'
}, {
    name: 'Recordtype',
    type: 'string'
}, {
    name: 'Id_Flows',
    type: 'decimal'
}, {
    name: 'Cpyid',
    type: 'decimal'
}, {
    name: 'Cpyparrent',
    type: 'decimal'
}, {
    name: 'Id_Files',
    type: 'decimal'
}, {
    name: 'Property',
    type: 'string'
}
];
// Database functions
function fnSetXform_Status(Data, isView) {
    $("#divXformHtml_Status").html(Data.Xform);
    if (isView)
        fnSetCommonView("Status", Data.XformView, "text-success");
    $("#frmXform_Status .editor").jqte();
    JqwTransformWithRole('frmXform_Status');
    try {
        $('#frmXform_Status').jqxValidator({
            onSuccess: function () {
                WorkData.Status_Data.Valid = true;
            },
            onError: function () {
                WorkData.Status_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Statusrules)),
            hintType: "label"
        });
    } catch (err) { }
};
function fnInsertXform_Status() {
    $("#frmXform_Status").jqxValidator('validate');
    if (WorkData.Status_Data.Current.Id_Parent != null && WorkData.Status_Data.Valid == true) {
        $("#frmXform_Status").mYPostFormNew(urls(InsertXform_Status), {
            Id_ParentId_Parent: WorkData.Organization_Data.Current.Id,
            Recordtype: "recordtype",
            Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow
        }, function (Data) {
            WorkData.Status_Data.Current.Id = Data.Entity.Id_Status;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedStatusTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Status").DXjqxGridReload({});
                $("#divCreateOrEditModal_Status").modal('hide');
            }
        });
    } else {
        GlobError("InsertXform Status ValidateError!", 10);
    }
};
function fnSaveXform_Status() {
    try {
        $("#frmXform_Status").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Status_Data.Valid) {
        $("#frmXform_Status").mYPostFormNew(urls(SaveXform_Status), {
            Id_Entity: WorkData.Status_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedStatusTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Status").DXjqxGridReload({});
                $("#divCreateOrEditModal_Status").modal('hide');
                fnGetXform_Status();
            }
        });
    } else {
        GlobError("SaveXform Status Error!", 10);
    }
};
function fnDeleteXform_Status() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Status), {
                Id_Status: WorkData.Status_Data.Current.Id,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedStatusTitle"), Data.Error.Errormessage);
                } else {
                    $("#divGrid_Status").DXjqxGridReload({});
                }
            });
        } else { }
    });
};
function fnGetXform_Status() {
    WorkData.Status_Data.Current.ChangedName = WorkData.Status_Data.Current.Name;
    AjaxGet(urls(GetXform_Status), {
        Id_Entity: WorkData.Status_Data.Current.Id,
        Id_Xform: 'frmXform_Status'
    }, function (Data) {
        fnSetXform_Status(Data, true);
    });
};
function fnNewXform_Status() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Status_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Status'
    }, function (Data) {
        fnSetXform_Status(Data, false);
        $.each(WorkData.Status_Data.TempFormValue, function (key, value) {
            $("#frmXform_Status input[id*='" + key + "']").val(value);
        });
        WorkData.Status_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Status() {
    $("#frmXform_Status").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Status',
        NewXformRoot: WorkData.Status_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Status(Data);
    });
};
// Xform Selector  combo
function fnBind_cmbXformRootSelect_CangeEvent_Status() {
    $('#cmbXformRootSelect_Status').off('change');
    $('#cmbXformRootSelect_Status').on('change', function (event) {
        if (event.args) {
            var item = event.args.item;
            WorkData.Status_Data.Current.Root = item.value;
            fnXformChangeRoot_Status();
        }
    });
};
$("#cmbXformRootSelect_Status").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'LayRoots',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Status_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_Status();
        } catch (Error) { }
    }
});
// Grid Specific
var initrowdetailsGrid_Status = function (index, parentElement, gridElement, datarecord) {
    var grid = $($(parentElement).children()[0]);
    if (grid != null) {
        Statusrequirements_NestedSource.data.id = datarecord.Id_Organization;
        nestedAdapter = new $.jqx.dataAdapter(Statusrequirements_NestedSource);
        grid.jqxGrid({
            source: nestedAdapter,
            width: '100%',
            theme: pageVariable.Jqwtheme,
            columns: Statusrequirements_columns
        });
        grid.on('rowclick', function (event) {
            WorkData.Statusrequirements_Data.Current.Id = args.row.bounddata.Id_Statusrequirements;
            fnGetXform_Statusrequirements();
        });
        grid.on('rowselect', function (event) {
            var args = event.args;
            WorkData.Statusrequirements_Data.Current.Id = args.row.Id_Statusrequirements;
        });
    }
};
function fnloadCompleteGrid_Status(data) {
    if (data.length > 0) { }
    else { }
};
var JqxParam_Grid_Status = {
    url: urls(GetRecords_Status),
    datafields: Status_datafields,
    ajax_data: {
        id: WorkData.Organization_Data.Current.Id,
        recordtype: 'StatusItem'
    },
    loadComplete: fnloadCompleteGrid_Status,
    param: {
        columns: Status_columns,
        width: '100%',
        theme: pageVariable.Jqwtheme,
        localization: getLocalization('hu'),
        columnsresize: true,
        pageable: true,
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        showtoolbar: false,
        rowdetails: true,
        rowdetailstemplate: {
            rowdetails: '<div style="margin:5px;"></div>',
            rowdetailsheight: 200
        },
        initrowdetails: initrowdetailsGrid_Status,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Status").DXjqxGrid(JqxParam_Grid_Status);
$('#divGrid_Status').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Status_Data.Current.Id = args.row.Id_Status;
    fnGetXform_Status();
    fnBind_btnSaveXform_Status(fnSaveXform_Status);
});
$('#divGrid_Status').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Status_Data.Current.Id = args.row.bounddata.Id_Organization;
    fnGetXform_Status();
});
$('#divGrid_Status').on('rowselect', function (event) {
    var args = event.args;
    WorkData.Status_Data.Current.Id = args.row.Id_Organization;
});
// Bind button (new,edit,delete) modal save button !
function fnBind_btnSaveXform_Status(process_function) {
    $("#btnSaveXform_Status").off('click');
    $("#btnSaveXform_Status").on('click', function (e) {
        process_function();
    });
};
function fnBindDefButtonClick_Status() {
    $("#btnNewXform_Status").on('click', function (e) {
        if (1 == 1) {
            fnNewXform_Status();
            fnBind_btnSaveXform_Status(fnInsertXform_Status);
            $("#divCreateOrEditModal_Status").modal('show');
        } else {
            GlobError("btnNewXform_Status Error", 11);
        }
    });
    $("#btnEditXform_Status").on('click', function (e) {
        fnGetXform_Status();
        fnBind_btnSaveXform_Status(fnSaveXform_Status);
        $("#divCreateOrEditModal_Status").modal('show');
    });
    $("#btnDeleteXform_Status").on('click', function (e) {
        fnDeleteXform_Status();
    });
};
fnBindDefButtonClick_Status();

// StatusReq (Slave2 Entiy) ----------------------------------------------------------------------------------------------------------------------------------
var GetXform_Statusrequirements = '/GetXform_Statusrequirements';
var InsertXform_Statusrequirements = '/InsertXform_Statusrequirements';
var SaveXform_Statusrequirements = '/SaveXform_Statusrequirements';
var DeleteXform_Statusrequirements = '/DeleteXform_Statusrequirements';
var GetRecords_Statusrequirements = '/GetRecords_Statusrequirements';
var GetXform_AttachedPersons = '/GetXform_AttachedPersons';

var SetStatusInTree = '/SetStatusInTree';
var SetOrphanStatusInTree = '/SetOrphanStatusInTree';
// Datastruct
WorkData.Statusrequirements_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true,
    Disabled: false,
    TempFormValue: {}
}
// Columns and record def
var Statusrequirements_columns = [{
    text: '§Name§',
    datafield: 'Name'
}, {
    text: '§Description§',
    datafield: 'Description'
}, {
    text: '§Recordtype§',
    datafield: 'Recordtype'
}, {
    text: '§Itemtype§',
    datafield: 'Itemtype'
}
];
var Statusrequirements_datafields = [{
    name: 'Id_Statusrequirements',
    type: 'decimal'
}, {
    name: 'Id_Organization',
    type: 'decimal'
}, {
    name: 'Id_Flows',
    type: 'decimal'
}, {
    name: 'Cpyid',
    type: 'decimal'
}, {
    name: 'Recordtype',
    type: 'string'
}, {
    name: 'Itemtype',
    type: 'string'
}, {
    name: 'Name',
    type: 'string'
}, {
    name: 'Description',
    type: 'string'
}
];
// Database functions
function fnSetXform_Statusrequirements(Data, isView) {
    $("#divXformHtml_Statusrequirements").html(Data.Xform);
    if (isView)
        fnSetCommonView("Statusrequirements", Data.XformView, "text-warning");
    $("#frmXform_Statusrequirements .editor").jqte();
    JqwTransformWithRole('frmXform_Statusrequirements');
    try {
        $('#frmXform_Statusrequirements').jqxValidator({
            onSuccess: function () {

                WorkData.Statusrequirements_Data.Valid = true;
            },
            onError: function () {

                WorkData.Statusrequirements_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Statusrequirementsrules)),
            hintType: "label"
        });
    } catch (err) { }
};
function fnInsertXform_Statusrequirements() {
    $("#frmXform_Statusrequirements").jqxValidator('validate');
    WorkData.Statusrequirements_Data.Valid = true;
    if (WorkData.Status_Data.Current.Id != null && WorkData.Statusrequirements_Data.Valid == true) {
        $("#frmXform_Statusrequirements").mYPostFormNew(urls(InsertXform_Statusrequirements), {
            Id_Parent: WorkData.Status_Data.Current.Id,
            Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow
        }, function (Data) {
            WorkData.Statusrequirements_Data.Current.Id = Data.Entity.Id_Statusrequirements;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
            } else {
                nestedAdapter.dataBind();
                $("#divCreateOrEditModal_Statusrequirements").modal('hide');
            }
        });
    } else {
        GlobError("InsertXform Statusrequirements ValidateError!", 10);
    }
};
function fnSaveXform_Statusrequirements() {
    try {
        $("#frmXform_Statusrequirements").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Statusrequirements_Data.Valid) {
        $("#frmXform_Statusrequirements").mYPostFormNew(urls(SaveXform_Statusrequirements), {
            Id_Entity: WorkData.Statusrequirements_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
            } else {
                nestedAdapter.dataBind();
                $("#divCreateOrEditModal_Statusrequirements").modal('hide');
            }
        });
    } else {
        GlobError("SaveXform Statusrequirements Error!", 10);
    }
};
function fnpostDeleteXform_Statusrequirements() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            $("#frmXform_Statusrequirements").mYPostFormNew(urls(DeleteXform_Statusrequirements), {
                Id_Entity: WorkData.Statusrequirements_Data.Current.Id,
                Id_Flows: WorkData.model_ClientPart.Id_Flow
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
                } else {
                    nestedAdapter.dataBind();
                }
            });
        } else { }
    });
};
function fnDeleteXform_Statusrequirements() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            AjaxGet(urls(DeleteXform_Statusrequirements), {
                Id_Statusrequirements: WorkData.Statusrequirements_Data.Current.Id
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
                } else {
                    nestedAdapter.dataBind();
                }
            });
        } else { }
    });
};
function fnGetXform_Statusrequirements() {
    WorkData.Statusrequirements_Data.Current.ChangedName = WorkData.Statusrequirements_Data.Current.Name;
    AjaxGet(urls(GetXform_Statusrequirements), {
        Id_Entity: WorkData.Statusrequirements_Data.Current.Id,
        Id_Xform: 'frmXform_Statusrequirements'
    }, function (Data) {
        fnSetXform_Statusrequirements(Data, true);
    });
};
function fnNewXform_Statusrequirements() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Statusrequirements_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Statusrequirements'
    }, function (Data) {
        fnSetXform_Statusrequirements(Data, false);
        $.each(WorkData.Statusrequirements_Data.TempFormValue, function (key, value) {
            $("#frmXform_Statusrequirements input[id*='" + key + "']").val(value);
        });
        WorkData.Statusrequirements_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Statusrequirements() {
    $("#frmXform_Statusrequirements").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Statusrequirements',
        NewXformRoot: WorkData.Statusrequirements_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Statusrequirements(Data);
    });
};
// Xform Selector  combo
function fnBind_cmbXformRootSelect_CangeEvent_Statusrequirements() {
    $('#cmbXformRootSelect_Statusrequirements').off('change');
    $('#cmbXformRootSelect_Statusrequirements').on('change', function (event) {
        if (event.args) {
            var item = event.args.item;
            WorkData.Statusrequirements_Data.Current.Root = item.value;
            fnXformChangeRoot_Statusrequirements();
        }
    });
};
$("#cmbXformRootSelect_Statusrequirements").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'CheckRoots',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Statusrequirements_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_Statusrequirements();
        } catch (Error) { }
    }
});
// SubGrid Specific
var Statusrequirements_NestedSource = {
    datatype: "json",
    datafields: Statusrequirements_datafields,
    url: urls(GetRecords_Statusrequirements),
    data: {
        id: WorkData.Status_Data.Current.Id,
        recordtype: ''
    },
    async: false
};
// Bind button (new,edit,delete) modal save button !
function fnBind_btnSaveXform_Statusrequirements(process_function) {
    $("#btnSaveXform_Statusrequirements").off('click');
    $("#btnSaveXform_Statusrequirements").on('click', function (e) {
        process_function();
    });
};
function fnBindDefButtonClick_Statusrequirements() {
    $("#btnNewXform_Statusrequirements").on('click', function (e) {
        fnNewXform_Statusrequirements();
        fnBind_btnSaveXform_Statusrequirements(fnInsertXform_Statusrequirements);
        $("#divCreateOrEditModal_Statusrequirements").modal('show');
    });
    $("#btnEditXform_Statusrequirements").on('click', function (e) {
        fnGetXform_Statusrequirements();
        fnBind_btnSaveXform_Statusrequirements(fnSaveXform_Statusrequirements);
        $("#divCreateOrEditModal_Statusrequirements").modal('show');
    });
    $("#btnDeleteXform_Statusrequirements").on('click', function (e) {
        if (WorkData.Statusrequirements_Data.Current.Id != null) {
            fnDeleteXform_Statusrequirements();
        } else {
            GlobError("DeleteXform Statusrequirements Error!", 11);
        }
    });
};
fnBindDefButtonClick_Statusrequirements();
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


function fnBind_OtherButtons() {
    $("#btnAttachedPersons").on('click', function () {
        fnGetXform_AttachedPersons();
    });
    $("#btnMultiplicateStatus").on('click', function () {
        fnOpenMultiplicateStatus();
    });
    $("#btnMultiplicatStatusOk").on('click', function () {
        fnMultiplicateStatus();
    });
}
fnBind_OtherButtons();

// -------------------------------------------------------------------------------------------------------------------------

function fnSetCommonView(label, contetent, bclass) {

    $("#frmView_Organization_XformLabel").removeClass("text-primary text-success text-warning").addClass(bclass);
    $("#frmView_Organization_XformLabel").html(label);
    $("#divXformHtmlView_Organization").html(contetent);
}

// Orpahn status

WorkData.Orphanstatus_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {}
};



var Orphanstatus_datafields = [
{ name: 'Id_Organization', type: 'decimal' },
{ name: 'Id_Parent', type: 'decimal' },
{ name: 'Id_Ord', type: 'decimal' },
{ name: 'Bid_Organization', type: 'string' },
{ name: 'Name', type: 'string' },
{ name: 'Title', type: 'string' },
{ name: 'Shortname', type: 'string' },
{ name: 'Orgtype', type: 'string' },
{ name: 'Recordtype', type: 'string' },
{ name: 'Id_Flows', type: 'decimal' },
{ name: 'Cpyid', type: 'decimal' },
{ name: 'Cpyparrent', type: 'decimal' },
{ name: 'Id_Files', type: 'decimal' },
{ name: 'Id_Scopeofactivity', type: 'decimal' }
];
var Orphanstatus_columns = [
{ text: '§Id_Organization§', datafield: 'Id_Organization' },
{ text: '§Id_Parent§', datafield: 'Id_Parent' },
{ text: '§Id_Ord§', datafield: 'Id_Ord' },
{ text: '§Bid_Organization§', datafield: 'Bid_Organization' },
{ text: '§Name§', datafield: 'Name' },
{ text: '§Title§', datafield: 'Title' },
{ text: '§Shortname§', datafield: 'Shortname' },
{ text: '§Orgtype§', datafield: 'Orgtype' },
{ text: '§Recordtype§', datafield: 'Recordtype' },
{ text: '§Id_Flows§', datafield: 'Id_Flows' },
{ text: '§Cpyid§', datafield: 'Cpyid' },
{ text: '§Cpyparrent§', datafield: 'Cpyparrent' },
{ text: '§Id_Files§', datafield: 'Id_Files' },
{ text: '§Id_Scopeofactivity§', datafield: 'Id_Scopeofactivity' }
];

var GetXform_Orphanstatus = '/GetXform_Status';
var GetRecords_Orphanstatus = '/GetRecords_Orphanstatus';

function fnSetXform_Orphanstatus(Data, isView) {
    fnSetCommonView("OrphanStatus", Data.XformView, "text-warning");
};
function fnGetXform_Orphanstatus() {
    if (WorkData.Orphanstatus_Data.Current.Id > 0) {
        WorkData.Orphanstatus_Data.Current.ChangedName = WorkData.Orphanstatus_Data.Current.Name;
        AjaxGet(urls(GetXform_Orphanstatus), {
            Id_Entity: WorkData.Orphanstatus_Data.Current.Id,
            Id_Xform: 'frmXform_Orphanstatus'
        }, function (Data) {
            fnSetXform_Orphanstatus(Data, true);
        });
    } else {
        fnSetCommonView("-", "-", "text-warning");
    }
};
// Grid 
function fnloadCompleteGrid_Orphanstatus(data) {
    if (data.length > 0) {
    } else {
    }
};

var JqxParam_Grid_Orphanstatus = {
    url: urls(GetRecords_Orphanstatus),
    datafields: Orphanstatus_datafields,
    ajax_data: { Id: WorkData.model_CurrentFlowstep.Id_Flow },
    loadComplete: fnloadCompleteGrid_Orphanstatus,
    param: {
        columns: Orphanstatus_columns,
        width: '100%',
        theme: pageVariable.Jqwtheme,
        localization: getLocalization('hu'),
        columnsresize: true,
        pageable: true,
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        showtoolbar: false,
        rowdetails: false,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Orphanstatus").DXjqxGrid(JqxParam_Grid_Orphanstatus);


var DELAY = 700, clicks = 0, timer = null;

$('#divGrid_Orphanstatus').on('rowdoubleclick', function (event) {
    var args = event.args;
    event.preventDefault();  //cancel system double-click event
    // WorkData.Orphanstatus_Data.Current.Id = args.row.Id_Orphanstatus;
    AjaxGet(urls(MoveHierarchyItem_Organization), {
        Id: args.row.bounddata.Id_Organization,
        NewParentId: WorkData.Organization_Data.Current.Id,
        dropPos: 'inside'
    }, function (Data) {
        if (Data.Error.Errorcode != 0) {
            Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
        } else {
            $("#divGrid_Status").DXjqxGridReload({});
            $("#divGrid_Orphanstatus").DXjqxGridReload({});
            fnSetCommonView("-", "-", "text-warning");
            WorkData.Orphanstatus_Data.Current.Id = 0;
        }
    });
});
$('#divGrid_Orphanstatus').on('rowclick', function (event) {

    var args = event.args;
    clicks++;  //count clicks
    if (clicks === 1) {
        timer = setTimeout(function () {
            WorkData.Orphanstatus_Data.Current.Id = args.row.bounddata.Id_Organization;
            fnGetXform_Orphanstatus();
            //alert("Single Click");  //perform single-click action    
            clicks = 0;             //after action performed, reset counter

        }, DELAY);
    } else {

        clearTimeout(timer);    //prevent single-click action
       // alert("Double Click");  //perform double-click action
        clicks = 0;             //after action performed, reset counter
    }
});
$('#divGrid_Orphanstatus').on('rowselect', function (event) {

    var args = event.args;
    WorkData.Orphanstatus_Data.Current.Id = args.row.Id_Organization;
    //fnGetXform_Orphanstatus();
});


$("#divGrid_Orphanstatus").DXjqxGridReload({ Id: WorkData.model_CurrentFlowstep.Id_Flow });

//-----------------------

//// -+-------------------------------------------------
$("#divColapseStatustable").DXInitStateControl({
    Statusmode: { visible: true },
    Orphanmode: { visible: false },
});
$("#divColapseOrphantable").DXInitStateControl({
    Statusmode: { visible: false },
    Orphanmode: { visible: true },
});
DXSetState('Statusmode');

$("#btnSwitchStatusMode").on('click', function () {
    if ($("#divColapseStatustable").is(":visible")) DXSetState('Orphanmode');
    else DXSetState('Statusmode');
});
//$("#StatusCopyList").jqxTree({ //source:source,
//    allowDrag: true, allowDrop: false, height: '100px', width: '250px',
//    dragEnd: function (item, dropItem, args, dropPosition, tree) {
//        var mydropItem = { id: dropItem.id };
//        var mydragItem = { id: item.value };
//        JqxTree_DragEnd(mydragItem, mydropItem, dropPosition, urls(MoveHierarchyItem_Organization), function () {
//            $('#divTree_Organization').DXjqxTreeReload({
//                Id: WorkData.Organization_Data.Current.Id_Flow,
//                level: WorkData.Organization_Data.Level,
//                recordtype: 'OrgItem'
//            });
//            if ($("#divColapseStatustable").is(":visible")) {
//                $("#divGrid_Status").DXjqxGridReload({});
//            } else {
//                //$('#divGrid_Orphanstatus').DXjqxGridReload({ id: 0, recordtype: '' });
//            }
//        });
//    }
//});

$("#btnCopyStatusToTemp").on('click', function () {
    if ($("#divColapseStatustable").is(":visible")) {
        //$('#StatusCopyList').jqxTree('addTo', { label: WorkData.Status_Data.Current.Name, value: WorkData.Status_Data.Current.Id });
        AjaxGet(urls(SetStatusInTree), {
            Id_Status: WorkData.Status_Data.Current.Id,
            value:'InTree'
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedStatusrequirementsTitle"), Data.Error.Errormessage);
            } else {
                $('#divTree_Organization').DXjqxTreeReload({
                    Id: WorkData.model_CurrentFlowstep.Id_Flow,
                    level: WorkData.Organization_Data.Level,
                    recordtype: 'OrgItem'
                });

            }
        });
    }
    else {
        //$('#StatusCopyList').jqxTree('addTo', { label: WorkData.Orphanstatus_Data.Current.Name, value: WorkData.Orphanstatus_Data.Current.Id });
    }
});
$("#btnClearStatusToTemp").on('click', function () {
  //  $('#StatusCopyList').jqxTree('clear');
});

//$("#divCopyStatus").on('show.bs.collapse', function () {
//    // $('#divTree_Organization').jqxTree({ height: "200px" });
//});
//$("#divCopyStatus").on('hide.bs.collapse', function () {
//    // $('#divTree_Organization').jqxTree({ height: "300px" });
//});



var MultipleStatusrecord = '/MultipleStatusrecord';




//-- Attached Persons
function fnGetXform_AttachedPersons() {

    AjaxGet(urls(GetXform_AttachedPersons), {
        Id: WorkData.Status_Data.Current.Id,
        Id_Xform: 'frmXform_AttachedPersons'
    }, function (Data) {
        if (Data.Error.Errorcode == 0) {
            $('#CaruselInner').empty();
            $('#CarusrlIndicator').empty();
            var HtmlOut1 = "";
            var HtmlOut2 = "";
            var i = 0;
            $.each(Data.Forms, function (key, value) {
                //GetPersonPic/"
                HtmlOut1 += '<li data-target="#carousel-example-generic" data-slide-to="' + i + '"></li>';
                HtmlOut2 += '<div class="carousel-item">' + value + '</div>';

                i++;
            });
            $('#CaruselInner').append(HtmlOut2);
            $('#CarusrlIndicator').append(HtmlOut1);

            $('.carousel-item').first().addClass('active');
            $('.carousel-indicators > li').first().addClass('active');
            $("#carousel-example-generic").carousel();

            $("#divViewModal_AttachedPersons").modal('show');
        }
    });
};
// ---MultiplicateStatus



function fnOpenMultiplicateStatus() {
    $("#divMultiplicatStatusModal").modal('show');
};
function fnMultiplicateStatus() {
    AjaxGet(urls(MultipleStatusrecord), {
        Id: WorkData.Status_Data.Current.Id,
        Copynum: $("#MultipleStatusCount").val()
    }, function (Data) {
        $('#divGrid_Status').DXjqxGridReload({});
        $("#divMultiplicatStatusModal").modal('hide');
    });
};
$("#MultipleStatusCount").InputSpinner({

    // button text/icons
    decrementButton: "<strong>-</strong>", 
    incrementButton: "<strong>+</strong>", 

    // class of input group
    groupClass: "input-group-spinner",

    // button class
    buttonsClass: "btn-outline-secondary",

    // button width
    buttonsWidth: "2.5em",

    // text alignment
    textAlign: "center",

    // delay in milliseconds
    autoDelay: 500, 

    // interval in milliseconds
    autoInterval: 100,

    // boost after these steps
    boostThreshold: 15, 

    // boost multiplier
    boostMultiplier: 2,

    // detects the local from `navigator.language`, if null
    locale: null 
  
});


$("#IdBodyStart").hide();
$("#IdBody").show();
