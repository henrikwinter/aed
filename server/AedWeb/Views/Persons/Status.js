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
    WorkData.model_ClientPart.io_SelectedOrgId = WorkData.Status_Data.Current.Id;
    return WorkData.model_ClientPart;
};
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

function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Organization' + url;
    }
}

function Purls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Persons' + url;
    }
}



// --- Preselect
var GetPreselect_Organization = '/GetHierarchy_Organization';
var JqxTreeParam_PreselectOrganization = {
    url: urls(GetPreselect_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: 0,
        level: 3,
        recordtype: 'OrgItem'
    },
    id_hierarchy: 'Id_Organization',  ///<-- Specific :Id_Organization
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
    loadComplete: function (data) {
    },
    param: {
        width: 300, height: 220,
        theme: pageVariable.Jqwtheme
    }
};
$("#divOrgPreselect").jqxDropDownButton({ width: 300, height: 25 });
$('#PreselectOrgTree').DXjqxTree(JqxTreeParam_PreselectOrganization);
$('#PreselectOrgTree').on('select', function (event) {
    var args = event.args;
    var item = $('#PreselectOrgTree').jqxTree('getItem', args.element);
    WorkData.Organization_Data.Current.Id = item.id;
    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
    $("#divOrgPreselect").jqxDropDownButton('setContent', dropDownContent);
    $('#divTree_Organization').DXjqxTreeReload({ Id: WorkData.Organization_Data.Current.Id, level: WorkData.Organization_Data.Level, recordtype: 'OrgItem' });
    $("#divOrgPreselect").jqxDropDownButton("close");
});


// --- OrgChanged dates
var GetValidDates = '/GetValidDates';
var sourceValidDates = {
    datatype: "json",
    datafields: [{
        name: 'Datavalidfrom'
    }
    ],
    url: urls(GetValidDates),
    async: true
};
var dataAdapterValidDates = new $.jqx.dataAdapter(sourceValidDates, {
    loadComplete: function (data) {
    }
});
$("#divOrgDateList").jqxDropDownList({
    selectedIndex: 0,
    source: dataAdapterValidDates,
    displayMember: "Datavalidfrom",
    valueMember: "Datavalidfrom",
    width: 200,
    height: 30,
});
$("#divOrgDateList").on('select', function (event) {
    if (event.args) {
        var item = event.args.item;
        if (item) {
            var test = item.value;
            WorkData.Organization_Data.Current.selectdate = test;
            WorkData.Organization_Data.Current.Id = 0;
            WorkData.Organization_Data.Level = 20;
            $('#divTree_Organization').DXjqxTreeReload({
                Id: WorkData.Organization_Data.Current.Id,
                level: WorkData.Organization_Data.Level,
                recordtype: 'OrgItem',
                selecdate: WorkData.Organization_Data.Current.selectdate
            }
                );
        }
    }
});



// Organization (Main Entiy) ----------------------------------------------------------------------------------------------------------------------------------
var GetXform_Organization = '/GetXform_Organization';
var GetRecords_Organization = '/GetRecords_Organization';
var GetRecords_Organization = '/GetRecords_Organization_Session';
var GetHierarchy_Organization = '/GetHierarchy_Organization';
var GetPreselect_Organization = '/GetHierarchy_Organization';
// Datastruct
WorkData.Organization_Data = {
    Current: {
        Id: 0,
        //  Id_Flow: WorkData.model_CurrentFlowstep.Id_Flow,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: '',
        selectdate: ''
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
    text: '§Id_Scopeofactivity§',
    datafield: 'Id_Scopeofactivity'
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
}, { name: 'Status', type: 'string' },
{
    name: 'Cpyid',
    type: 'decimal'
}, {
    name: 'Cpyparrent',
    type: 'decimal'
}, {
    name: 'Id_Files',
    type: 'decimal'
}, {
    name: 'Id_Scopeofactivity',
    type: 'decimal'
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
                $('#divTree_Organization').DXjqxTreeReload();
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
                $('#divTree_Organization').DXjqxTreeReload({});
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
                        Id: 100,
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
    if (k.Status == 'InDeveloped') { k.Name = '<span class="text-secondary">' + k.Name + '</span>'; }
};

// Tree
var JqxTreeParam_Organization = {
    url: urls(GetHierarchy_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: WorkData.Organization_Data.Current.Id,
        level: WorkData.Organization_Data.Level,
        recordtype: 'OrgItem',
        selecdate: WorkData.Organization_Data.Current.selectdate
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
        theme: pageVariable.Jqwtheme
    }
};
$('#divTree_Organization').DXjqxTree(JqxTreeParam_Organization);
$('#divTree_Organization').on('itemClick', function (event) {
    var item = $(this).jqxTree('getItem', event.args.element);
    if (item != null) {
        WorkData.Organization_Data.Current.Id = item.value.Id_Organization;
        WorkData.Organization_Data.Current.Properties = item.value.Properties;
        fnGetXform_Organization();
        $("#divGrid_Status").DXjqxGridReload({
            id: WorkData.Organization_Data.Current.Id,
            recordtype: 'StatusItem'
        })
        if (item.value.Status == 'InDeveloped') { WorkData.Organization_Data.Current.Id = null; WorkData.Organization_Data.Disabled = true; fnSetDisable(true); } else { WorkData.Organization_Data.Disabled = false; fnSetDisable(false); }
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
        fnNewXform_Organization();
        fnBind_btnSaveXform_Organization(fnInsertXform_Organization);
        $("#divCreateOrEditModal_Organization").modal('show');
    });
    $("#btnEditXform_Organization").on('click', function (e) {
        fnGetXform_Organization();
        fnBind_btnSaveXform_Organization(fnSaveXform_Organization);
        $("#divCreateOrEditModal_Organization").modal('show');
    });
    $("#btnDeleteXform_Organization").on('click', function (e) {
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
var GetXform_Status = '/GetXform_Organization';
var InsertXform_Status = '/InsertXform_Orgstatus';
var SaveXform_Status = '/SaveXform_Orgstatus';
var DeleteXform_Status = '/DeleteXform_Orgstatus';
var GetRecords_Status = '/GetRecords_Organization';
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
var Status_columns = [{
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
}, ];
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
    name: 'Id_Scopeofactivity',
    type: 'decimal'
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
function fnGetXform_Status() {
    WorkData.Status_Data.Current.ChangedName = WorkData.Status_Data.Current.Name;
    AjaxGet(urls(GetXform_Status), {
        Id_Entity: WorkData.Status_Data.Current.Id,
        Id_Xform: 'frmXform_Status'
    }, function (Data) {
        fnSetXform_Status(Data, true);
    });
};


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
        rowdetails: false,
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

    $("#txtStatusname").html( args.row.Bid_Organization );


});


function fnSetCommonView(label, contetent, bclass) {

    $("#frmView_Organization_XformLabel").removeClass("text-primary text-success text-warning").addClass(bclass);
    $("#frmView_Organization_XformLabel").html(label);
    $("#divXformHtmlView_Organization").html(contetent);
}

function fnSetDisable(flag) {
    //$('button').prop("disabled", flag);
    //$("button").find("[state='sensdisable']").prop("disabled", flag);
    $('[state="sensdisable"]').prop("disabled", flag);
};



var GetXform_AttachedPersons = '/GetXform_PersonsCard';
AjaxGet(Purls(GetXform_AttachedPersons), {
    Id: WorkData.model_PersonClientPart.io_SelectedPersonId,
    Id_Xform: ''  //'frmXform_AttachedPersons'
}, function (Data) {
    if (Data.Error.Errorcode == 0) {
        $("#txtPersonname").html(Data.Personname);
    }
});




$("#IdBodyStart").hide();
$("#IdBody").show();
