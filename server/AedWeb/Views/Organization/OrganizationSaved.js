function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Organization' + url;
    }
};
//urls
var GetRecords_Organization = '/GetRecords_Organization';
var DeleteXform_Organization = '/DeleteXform_Organization';
var GetHierarchy_Organization = '/GetHierarchy_Organization';
var MoveHierarchyItem_Organization = '/MoveHierarchyItem_Organization';
var GetRecords_Orgstatus = '/GetRecords_Organization';
var DeleteXform_Orgstatus = '/DeleteXform_Orgstatus';
var GetRecords_Statusrequirements = '/GetRecords_Statusrequirements';
var DeleteXform_Statusrequirements = '/DeleteXform_Statusrequirements';
var GetPreselect_Organization = '/GetHierarchy_Organization';
var GetValidDates = '/GetValidDates';
// Columns  Datafields
var Organization_datafields = [{
    name: 'Id_Organization',
    type: 'decimal', pk: 'Id_Organization'
}, {
    name: 'Id_Parent',
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
    name: 'Id_Scopeofactivity',
    type: 'decimal'
}, { name: 'Orgtype', type: 'string' }
];
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
var Orgstatus_datafields = [{
    name: 'Id_Organization',
    type: 'decimal', pk: 'Id_Organization'
}, {
    name: 'Id_Parent',
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
var Orgstatus_columns = [
 {
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
    text: '§Orgtype§',
    datafield: 'Orgtype'
}, {
    text: '§Recordtype§',
    datafield: 'Recordtype'
}
];
var Statusrequirements_datafields = [{
    name: 'Id_Statusrequirements',
    type: 'decimal', pk: 'Id_Statusrequirements'
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
var Statusrequirements_columns = [{
    text: '§Id_Statusrequirements§',
    datafield: 'Id_Statusrequirements'
}, {
    text: '§Id_Organization§',
    datafield: 'Id_Organization'
}, {
    text: '§Id_Flows§',
    datafield: 'Id_Flows'
}, {
    text: '§Cpyid§',
    datafield: 'Cpyid'
}, {
    text: '§Recordtype§',
    datafield: 'Recordtype'
}, {
    text: '§Itemtype§',
    datafield: 'Itemtype'
}, {
    text: '§Name§',
    datafield: 'Name'
}, {
    text: '§Description§',
    datafield: 'Description'
}
];

// Work datas
WorkData.Organization_Data = {
    Current: {
        Id_Entity: 0,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    FkKey: xfindpk(Organization_datafields)
};
WorkData.Orgstatus_Data = {
    Current: {
        Id_Entity: 0,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    FkKey: xfindpk(Orgstatus_datafields)
};
WorkData.Statusrequirements_Data = {
    Current: {
        Id_Entity: 0,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    FkKey: xfindpk(Statusrequirements_datafields)
};
// Xforms
var CommonXformViewPanel = "divXformHtmlCommonView";
function fnDeleteXform_Organization() {
    dlgAreYouSoure("dlgAreYouSoure", function (confirm) {
        if (confirm) {
            var cmd = (confirm == 'close') ? 'close' : '';
            AjaxGet(urls(DeleteXform_Organization), {
                Id_Entity: WorkData.Organization_Data.Current.Id_Entity,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    GlobError(Data.Error.Errormessage, 55);
                } else {
                    if ($.isFunction(fnOrganization_Reload)) fnOrganization_Reload();
                }
            });
        } else {}
    });
};
function fnOrganization_Reload() {
    $('#divTree_Organization').DXjqxTreeReload();
};
$("#frmXform_Organization").Xform({
    controller: 'Organization',
    actionpart: 'Organization',
    refroot: '',
    formid: 'frmXform_Organization',
    viewformid: CommonXformViewPanel,
    typecombo: {
        Id: 'cmbXformRootSelect_Organization',
        filter: 'OrgRoots'
    },
    savebuttonid: 'btnSaveXform_Organization',
    savebuttoncallback: function (mode) {
        if (mode == 'insert') {
            var id_parent = $("#chkInsertPosition").is(':checked') ? WorkData.Organization_Data.Current.Id_Parent : WorkData.Organization_Data.Current.Id_Entity;
            return {
                Id_Parent: id_parent,
                Recordtype: "OrgItem"
            };
        } else {
            return {
                Id_Entity: WorkData.Organization_Data.Current.Id_Entity
            };
        }
    },
    Complete: function (e, eData) {
        if (eData.mode == 'Save' || eData.mode == 'Insert') {
            if ($.isFunction(fnOrganization_Reload)) fnOrganization_Reload();
            $("#divCreateOrEditModal_Organization").modal('hide');
        } else if (eData.mode == 'Get') {
            
            $("#" + CommonXformViewPanel + "_XformLabel").removeClass("text-primary text-success text-warning").addClass("text-primary");
            $("#" + CommonXformViewPanel + "_XformLabel").html('§Organization§ (' + TranslateLang('Fid_OrganizationOrganization', eData.value.Root, LangDictionary) + ')');
        }
        try {
            eval('OnComplete_' + eData.mode + '(eData);');
        } catch (err) {}
    },
    Error: function (e, eData) {
        Titlemess($("#SelectedOrganizationTitle"), eData.value.Error.Errormessage);
    }
});
function fnDeleteXform_Orgstatus() {
    dlgAreYouSoure("dlgAreYouSoure", function (confirm) {
        if (confirm) {
            var cmd = (confirm == 'close') ? 'close' : '';
            AjaxGet(urls(DeleteXform_Orgstatus), {
                Id_Entity: WorkData.Orgstatus_Data.Current.Id_Entity,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    GlobError(Data.Error.Errormessage, 55);
                } else {
                    if ($.isFunction(fnOrgstatus_Reload)) fnOrgstatus_Reload();
                }
            });
        } else { }
    });
};
function fnOrgstatus_Reload() {
    $('#divGrid_Orgstatus').DXjqxGridReload();
}
$("#frmXform_Orgstatus").Xform({
    controller: 'Organization',
    actionpart: 'Orgstatus',
    refroot: '',
    formid: 'frmXform_Orgstatus',
    viewformid: CommonXformViewPanel,
    typecombo: {
        Id: 'cmbXformRootSelect_Orgstatus',
        filter: 'LayRoots'
    },
    savebuttonid: 'btnSaveXform_Orgstatus',
    savebuttoncallback: function (mode) {
        if (mode == 'insert') {
            //var id_parent = $("#chkInsertPosition").is(':checked') ? WorkData.Orgstatus_Data.Current.Id_Parent : WorkData.Orgstatus_Data.Current.Id_Entity;
            return {
                Id_Parent: WorkData.Organization_Data.Current.Id_Entity,
                Recordtype: "StatusItem"
            };
        } else {
            return {
                Id_Entity: WorkData.Orgstatus_Data.Current.Id_Entity
            };
        }
    },
    Complete: function (e, eData) {
        if (eData.mode == 'Save' || eData.mode == 'Insert') {
            if ($.isFunction(fnOrgstatus_Reload)) fnOrgstatus_Reload();
            $("#divCreateOrEditModal_Orgstatus").modal('hide');
        } else if (eData.mode == 'Get') {
            $("#" + CommonXformViewPanel + "_XformLabel").removeClass("text-primary text-success text-warning").addClass("text-primary");
            $("#" + CommonXformViewPanel + "_XformLabel").html('§Orgstatus§  (' + TranslateLang('Fid_OrganizationOrganization', eData.value.Root, LangDictionary) + ')');
        }
        try {
            eval('OnComplete_' + eData.mode + '(eData);');
        } catch (err) {}
    },
    Error: function (e, eData) {
        Titlemess($("#SelectedOrgstatusTitle"), eData.value.Error.Errormessage);
    }
});
function fnDeleteXform_Statusrequirements() {
    dlgAreYouSoure("dlgAreYouSoure", function (confirm) {
        if (confirm) {
            var cmd = (confirm == 'close') ? 'close' : '';
            AjaxGet(urls(DeleteXform_Statusrequirements), {
                Id_Entity: WorkData.Statusrequirements_Data.Current.Id_Entity,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    GlobError(Data.Error.Errormessage, 55);
                } else {
                    if ($.isFunction(fnOrgstatus_Statusrequirements)) fnOrgstatus_Statusrequirements();
                }
            });
        } else { }
    });
};
function fnStatusrequirements_Reload() {
    // $('#divGrid_Statusrequirements').DXjqxGridReload();     
    nestedAdapter.dataBind();
}
$("#frmXform_Statusrequirements").Xform({
    controller: 'Organization',
    actionpart: 'Statusrequirements',
    refroot: '',
    formid: 'frmXform_Statusrequirements',
    viewformid: CommonXformViewPanel,
    typecombo: {
        Id: 'cmbXformRootSelect_Statusrequirements',
        filter: 'CheckRoots'
    },
    savebuttonid: 'btnSaveXform_Statusrequirements',
    savebuttoncallback: function (mode) {
        if (mode == 'insert') {
            return {
                Id_Parent: WorkData.Orgstatus_Data.Current.Id_Entity,
                Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow
            };
        } else {
            return {
                Id_Entity: WorkData.Statusrequirements_Data.Current.Id_Entity
            };
        }
    },
    Complete: function (e, eData) {
        if (eData.mode == 'Save' || eData.mode == 'Insert') {
            if ($.isFunction(fnStatusrequirements_Reload)) fnStatusrequirements_Reload();
            $("#divCreateOrEditModal_Statusrequirements").modal('hide');
        } else if (eData.mode == 'Get') {
            $("#" + CommonXformViewPanel + "_XformLabel").removeClass("text-primary text-success text-warning").addClass("text-primary");
            $("#" + CommonXformViewPanel + "_XformLabel").html('§Statusrequirements§  (' + TranslateLang('Fid_OrganizationOrganization', eData.value.Root, LangDictionary) + ')');
        }
        try {
            eval('OnComplete_' + eData.mode + '(eData);');
        } catch (err) {}
    },
    Error: function (e, eData) {
        Titlemess($("#SelectedStatusrequirementsTitle"), eData.value.Error.Errormessage);
    }
});

// New,Edit,Delete Buttons
$("#btnNewXform_Organization").on('click', function (e) {

    if (WorkData.Organization_Data.Current.Id_Parent >= 0 && WorkData.Organization_Data.Current.Id_Entity > 0) {
        $("#frmXform_Organization").Xform('newform');
        $("#divCreateOrEditModal_Organization").modal('show');
    } else if (WorkData.Organization_Data.Current.Id_Parent == null && WorkData.Organization_Data.Current.Id_Entity > 0) {
        $("#frmXform_Organization").Xform('newform');
        $("#divCreateOrEditModal_Organization").modal('show');

    } else {
        GlobError("btnNewXform_Organization Error", 11);
    }
});
$("#btnEditXform_Organization").on('click', function (e) {
    if (WorkData.Organization_Data.Current.Id_Entity > 0) {
        $("#divCreateOrEditModal_Organization").modal('show');
    } else {
        GlobError("btnDeleteXform_Organization Error", 11);
    }
});
$("#btnDeleteXform_Organization").on('click', function (e) {
    if (WorkData.Organization_Data.Current.Id_Entity > 0) {
        fnDeleteXform_Organization();
    } else {
        GlobError("btnDeleteXform_Organization Error", 11);
    }
});
$("#btnNewXform_Orgstatus").on('click', function (e) {
    if (WorkData.Organization_Data.Current.Id_Entity > 0) {
        $("#frmXform_Orgstatus").Xform('newform');
        $("#divCreateOrEditModal_Orgstatus").modal('show');
    } else {
        GlobError("btnNewXform_Orgstatus Error", 11);
    }
});
$("#btnEditXform_Orgstatus").on('click', function (e) {
    if (WorkData.Orgstatus_Data.Current.Id_Entity > 0) {
        $("#divCreateOrEditModal_Orgstatus").modal('show');
    } else {
        GlobError("btnDeleteXform_Orgstatus Error", 11);
    }
});
$("#btnDeleteXform_Orgstatus").on('click', function (e) {
    if (WorkData.Orgstatus_Data.Current.Id_Entity > 0) {
        fnDeleteXform_Orgstatus();
    } else {
        GlobError("btnDeleteXform_Orgstatus Error", 11);
    }
});
$("#btnNewXform_Statusrequirements").on('click', function (e) {
    if (WorkData.Orgstatus_Data.Current.Id_Entity > 0) {
        $("#frmXform_Statusrequirements").Xform('newform');
        $("#divCreateOrEditModal_Statusrequirements").modal('show');
    } else {
        GlobError("btnNewXform_Statusrequirements Error", 11);
    }
});
$("#btnEditXform_Statusrequirements").on('click', function (e) {
    if (WorkData.Statusrequirements_Data.Current.Id_Entity > 0) {
        $("#divCreateOrEditModal_Statusrequirements").modal('show');
    } else {
        GlobError("btnDeleteXform_Statusrequirements Error", 11);
    }
});
$("#btnDeleteXform_Statusrequirements").on('click', function (e) {
    if (WorkData.Statusrequirements_Data.Current.Id_Entity > 0) {
        fnDeleteXform_Statusrequirements();
    } else {
        GlobError("btnDeleteXform_Statusrequirements Error", 11);
    }
});
// Tree Organization
var TreeContextMenu_Organization = [{
    id: 'Exp',
    html: '§ExpandAll§'
}, {
    id: 'Coll',
    html: '§COLLAPSE§'
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
function fnJqxTreeCompleteRecord_Organization(i, k) {
    if (k.Status == 'InDeveloped') {
        k.Name = '<span class="text-secondary">' + k.Name + '</span>';
    }
};
function fnJqxTreeDragStart_Organization(item) {
    return true;
};
function fnJqxTreeDragEnd_Organization(dragItem, dropItem, args, dropPosition, tree) {
    JqxTree_DragEnd(dragItem, dropItem, dropPosition, urls(MoveHierarchyItem_Organization));
};
var JqxTreeParam_Organization = {
    url: urls(GetHierarchy_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: WorkData.Organization_Data.Current.Id_Entity,
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
    loadComplete: function (data) {},
    LoadCompleteRecord: fnJqxTreeCompleteRecord_Organization,
    contextMenu: $("#divTreeContextmenu_Organization").DxjqxTreeContextmenu(TreeContextMenu_Organization, fnTree_ContextMenuClickEvent_Organization),
    urlforiconxml: pageVariable.baseSiteURL + "Ajax/GetXml",
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
    if (item != null || item.value != null) {
        WorkData.Organization_Data.Current.Id_Entity = item.value[WorkData.Organization_Data.FkKey];   // item.value.Id_Organization;  //item.value[WorkData.Organization_Data.FkKey]
        WorkData.Organization_Data.Current.Id_Parent = item.value.Id_Parent;
        $("#frmXform_Organization").Xform('get', {
            Id_Entity: WorkData.Organization_Data.Current.Id_Entity
        });
        $("#divGrid_Orgstatus").DXjqxGridReload({
            id: WorkData.Organization_Data.Current.Id_Entity,
            recordtype: 'StatusItem'
        });
        if (item.value.Status == 'InDeveloped') {
            WorkData.Organization_Data.Current.Id_Entity = null;
            WorkData.Organization_Data.Disabled = true;
            fnSetDisable(true);
        } else {
            WorkData.Organization_Data.Disabled = false;
            fnSetDisable(false);
        }
    }
});
$("#btnTreeSearch_Organization").on('click', function (e) {
    $("#divTreeSesrchContainer_Organization").DXjqxTreeSearch("divTree_Organization");
});
// Preselect Tree
var JqxTreeParam_PreselectOrganization = {
    url: urls(GetPreselect_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: 0,
        level: 3,
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
    LoadCompleteRecord: fnJqxTreeCompleteRecord_Organization,
    param: {
        width: 300,
        height: 220,
        theme: pageVariable.Jqwtheme
    }
};
$("#divOrganizationPreselect").jqxDropDownButton({
    width: 300,
    height: 25
});
$('#PreselectOrganizationTree').DXjqxTree(JqxTreeParam_PreselectOrganization);
$('#PreselectOrganizationTree').on('select', function (event) {
    var args = event.args;
    var item = $('#PreselectOrganizationTree').jqxTree('getItem', args.element);
    WorkData.Organization_Data.Current.Id_Entity = item.id;
    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
    $("#divOrganizationPreselect").jqxDropDownButton('setContent', dropDownContent);
    $('#divTree_Organization').DXjqxTreeReload({
        Id: WorkData.Organization_Data.Current.Id_Entity,
        level: WorkData.Organization_Data.Level,
        recordtype: 'OrgItem'
    });
    $("#divOrganizationPreselect").jqxDropDownButton("close");
});
// Grid Slave Status
var initrowdetailsGrid_Orgstatus = function (index, parentElement, gridElement, datarecord) {
    var grid = $($(parentElement).children()[0]);
    if (grid != null) {
        Statusrequirements_NestedSource.data.id = datarecord.Id_Organization;
        nestedAdapter = new $.jqx.dataAdapter(Statusrequirements_NestedSource);
        grid.jqxGrid({
            source: nestedAdapter,
            width: '98%',
            height:'95%',
            theme: 'ui-redmond',//'ui-redmond',// pageVariable.Jqwtheme,
            columns: Statusrequirements_columns
        });
    grid.on('rowclick', function (event) {
            WorkData.Statusrequirements_Data.Current.Id_Entity= args.row.bounddata.Id_Statusrequirements;
            $("#frmXform_Statusrequirements").Xform('get', {Id_Entity: WorkData.Statusrequirements_Data.Current.Id_Entity
            });
    });
    grid.on('rowselect', function (event) {
        var args = event.args;
        WorkData.Statusrequirements_Data.Current.Id_Entity = args.row[WorkData.Statusrequirements_Data.FkKey];
    });
}
};
function fnloadCompleteGrid_Orgstatus(data) {
    if (data.length > 0) {}
    else {}
};
var JqxParam_Grid_Orgstatus = {
    url: urls(GetRecords_Orgstatus),
    datafields: Orgstatus_datafields,
    ajax_data: {
        id: WorkData.Organization_Data.Current.Id_Entity,
        recordtype: 'StatusItem'
    },
    loadComplete: fnloadCompleteGrid_Orgstatus,
    param: {
        columns: Orgstatus_columns,
        width: '100%',
        theme: pageVariable.JqwthemeAlt,
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
            rowdetails: '<div style="margin:5px;border:1px solid grey;"></div>',
            rowdetailsheight: 200
        },
        initrowdetails: initrowdetailsGrid_Orgstatus,
        altrows: true,
        ready: function () {}
    }
};
$("#divGrid_Orgstatus").DXjqxGrid(JqxParam_Grid_Orgstatus);
$('#divGrid_Orgstatus').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Orgstatus_Data.Current.Id_Entity = args.row[WorkData.Orgstatus_Data.FkKey];
});
$('#divGrid_Orgstatus').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Orgstatus_Data.Current.Id_Entity = args.row.bounddata[WorkData.Orgstatus_Data.FkKey];
    $("#frmXform_Orgstatus").Xform('get', {
        Id_Entity: WorkData.Orgstatus_Data.Current.Id_Entity
    });
});
$('#divGrid_Orgstatus').on('rowselect', function (event) {
    var args = event.args;
    WorkData.Orgstatus_Data.Current.Id_Entity = args.row[WorkData.Orgstatus_Data.FkKey];
});
// Sub grid req
var Statusrequirements_NestedSource = {
    datatype: "json",
    datafields: Statusrequirements_datafields,
    url: urls(GetRecords_Statusrequirements),
    data: {
        id: 0,
        recordtype: ''
    },
    async: false
};

// --- Custom code OrgChanged dates
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
            WorkData.Organization_Data.Current.Id_Entity = 0;
            WorkData.Organization_Data.Level = 20;
            $('#divTree_Organization').DXjqxTreeReload({
                Id: WorkData.Organization_Data.Current.Id_Entity,
                level: WorkData.Organization_Data.Level,
                recordtype: 'OrgItem',
                selecdate: WorkData.Organization_Data.Current.selectdate
            }
                );
        }
    }
});

// --- Custom code Colapse div
$("#divColapseStatustable").DXInitStateControl({
    Statusmode: { visible: true },
    Orphanmode: { visible: false },
});
$("#divColapseOrphantable").DXInitStateControl({
    Statusmode: { visible: false },
    Orphanmode: { visible: true },
});

function fnSetDisable(flag) {
    $('[state="sensdisable"]').prop("disabled", flag);
};
fnSetDisable(true);
DXSetState('Statusmode');


LoadComplettCallback();
//$("#IdBodyStart").hide();
//$("#IdBody").show();
