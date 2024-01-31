var XCt = {
    baseSiteURL: pageVariable.baseSiteURL,
    newControler: 'Ajax',
    workController: 'Organization'
};

var Id_Entity = 0;
var Id_Parent = 0;
var Level = 5;
var Selectdate = null;

var Id_Status = 0;
var Id_Requriement = 0;

function ReloadAll() {
    $('#divTree_Organization').DXjqxTreeReload({
        Id:0,
        level: Level,
        recordtype: 'OrgItem'
    });
    $('#divGrid_Orgstatus').DXjqxGridReload({
        id: Id_Entity,
        recordtype: 'StatusItem'
    });
}


// ------- Organization Tree Xform
$("#cmbXformRootSelect_Organization").dxRootcombo({
    context: XCt,
    filter: 'OrgRoots',
    CreateComplet: function (Root) { },
    onChange: function (Root) {
        $("#frmXform_Organization").dxXform('Changeroot', {
            root: Root
        });
    }
});
$("#frmXform_Organization").dxXform({
    context: XCt,
    name: 'Organization',
    id_viewdiv: 'divXformHtmlCommonView',
    dlgconfirm: dlgAreYouSoure,
    GetComplet: function () {
        $("#btnSaveXform_Organization").dxButton('Setclickporc', function () {
            $("#frmXform_Organization").dxXform('Save', { arg: { Id_Entity: Id_Entity } });
        });
        $("#btnSaveXform_Organization").prop('disabled', false);
        $("#divXformHtmlCommonView_XformLabel").html("Organization")
    },
    SaveComplet: function () {
        $("#divCreateOrEditModal_Organization").modal('hide');
        $("#frmXform_Organization").dxXform('Get', { id: Id_Entity });
        ReloadAll();
    },
    NewComplet: function () {
        $("#btnNewXform_Organization").prop("disabled", true);
        $("#btnSaveXform_Organization").prop('disabled', false);
        $("#divCreateOrEditModal_Organization").modal('show');
        var id_parent = $("#chkInsertPosition").is(':checked') ? Id_Parent : Id_Entity;
        $("#btnSaveXform_Organization").dxButton('Setclickporc', function () {
            $("#frmXform_Organization").dxXform('Insert', { arg: { Id_Parent: id_parent, Recordtype: 'OrgItem' } });
        });
    },
    InsertComplet: function () {
        $("#divCreateOrEditModal_Organization").modal('hide');
        ReloadAll();
    },
    DeleteComplet: function () {
        ReloadAll();
    },
    InValid: function () {

    }
});

// New,Edit,Delete Buttons
$("#btnSaveXform_Organization").dxButton();
$("#btnNewXform_Organization").on('click', function (e) {
    $("#frmXform_Organization").dxXform('New', 'GeneralOrg');
});
$("#btnEditXform_Organization").on('click', function (e) {
    if (Id_Entity > 0) {
        
        $("#frmXform_Organization").dxXform('Get', {
            id: Id_Entity
        });
        $("#divCreateOrEditModal_Organization").modal('show');
    } else {
        GlobError('No selected!', 11);
    }
});
$("#btnDeleteXform_Organization").on('click', function (e) {

});


// ------- Organization Tree
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
    JqxTree_DragEnd(dragItem, dropItem, dropPosition, XCt.baseSiteURL + XCt.workController + '/MoveHierarchyItem_Organization');
};
var JqxTreeParam_Organization = {
    url: XCt.baseSiteURL + XCt.workController + '/GetHierarchy_Organization',
    datafields: Organization_datafields,
    ajax_data: {
        Id: Id_Entity,
        level: Level,
        recordtype: 'OrgItem',
        selecdate: Selectdate
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
        Id_Entity = item.value.Id_Organization;
        Id_Parent = item.value.Id_Parent;
        $("#frmXform_Organization").dxXform('Get', { id: Id_Entity });
        $("#btnNewXform_Organization").prop("disabled", false);
        $("#btnNewXform_Orgstatus").prop("disabled", false);
        $('#divGrid_Orgstatus').DXjqxGridReload({
            id: Id_Entity,
            recordtype: 'StatusItem'
        });
    }
});
$("#btnTreeSearch_Organization").on('click', function (e) {
    $("#divTreeSesrchContainer_Organization").DXjqxTreeSearch("divTree_Organization");
});

// -------- Staus Grid
// ------- Status grid Xform
$("#cmbXformRootSelect_Orgstatus").dxRootcombo({
    context: XCt,
    filter: 'LayRoots',
    CreateComplet: function (Root) { },
    onChange: function (Root) {
        $("#frmXform_Orgstatus").dxXform('Changeroot', {
            root: Root
        });
    }
});
$("#frmXform_Orgstatus").dxXform({
    context: XCt,
    name: 'Orgstatus',
    id_viewdiv: 'divXformHtmlCommonView',
    dlgconfirm: dlgAreYouSoure,
    GetComplet: function () {
        $("#divXformHtmlCommonView_XformLabel").html("Status");
        $("#btnSaveXform_Orgstatus").dxButton('Setclickporc', function () {
            $("#frmXform_Orgstatus").dxXform('Save', { arg: { Id_Entity: Id_Status } });
        });
    },
    SaveComplet: function () {
        $("#divCreateOrEditModal_Orgstatus").modal('hide');
        $("#frmXform_Orgstatus").dxXform('Get', { id: Id_Status });
        $('#divGrid_Orgstatus').DXjqxGridReload({
            id: Id_Entity,
            recordtype: 'StatusItem'
        });
    },
    NewComplet: function (p) {
        $("#btnNewXform_Orgstatus").prop("disabled", true);
        $("#divCreateOrEditModal_Orgstatus").modal('show');
        $("#btnSaveXform_Orgstatus").dxButton('Setclickporc', function () {
            $("#frmXform_Orgstatus").dxXform('Insert', { arg: { Id_Parent: Id_Entity, Recordtype: 'StatusItem' } });
        });
    },
    InsertComplet: function (data) {
        $("#btnEditXform_Orgstatus").prop("disabled", true);
        Id_Status = data.Entity.Id_Organization;
        $("#divCreateOrEditModal_Orgstatus").modal('hide');
        $("#frmXform_Orgstatus").dxXform('Get', {
            id: Id_Status
        });
        ReloadAll();
    },
    DeleteComplet: function () {
        ReloadAll();
    },
    InValid: function () {

    }
});
// New,Edit,Delete Buttons
$("#btnSaveXform_Orgstatus").dxButton();
$("#btnNewXform_Orgstatus").on('click', function (e) {
    $("#frmXform_Orgstatus").dxXform('New', 'GeneralLay');
});
$("#btnEditXform_Orgstatus").on('click', function (e) {
    if (Id_Entity > 0) {

        $("#frmXform_Orgstatus").dxXform('Get', {
            id: Id_Status
        });
        $("#divCreateOrEditModal_Orgstatus").modal('show');
    } else {
        GlobError('No selected!', 11);
    }
});
$("#btnDeleteXform_Orgstatus").on('click', function (e) {
    
});
$("#btnAttachedPersons").on('click', function (e) {
});

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

var initrowdetailsGrid_Orgstatus = function (index, parentElement, gridElement, datarecord) {
    var grid = $($(parentElement).children()[0]);
    if (grid != null) {
        Statusrequirements_NestedSource.data.id = datarecord.Id_Organization;
        nestedAdapter = new $.jqx.dataAdapter(Statusrequirements_NestedSource);
        grid.jqxGrid({
            source: nestedAdapter,
            width: '98%',
            height: '95%',
            theme: 'ui-redmond',//'ui-redmond',// pageVariable.Jqwtheme,
            columns: Statusrequirements_columns
        });
        grid.on('rowclick', function (event) {
            Id_Entity = args.row.bounddata.Id_Statusrequirements;
        });
        grid.on('rowselect', function (event) {
            var args = event.args;
            Id_Entity = args.row['Id_Organization'];
        });
    }
};
function fnloadCompleteGrid_Orgstatus(data) {
};
var JqxParam_Grid_Orgstatus = {
    url: XCt.baseSiteURL + XCt.workController + '/GetRecords_Organization',
    datafields: Orgstatus_datafields,
    ajax_data: {
        id: Id_Entity,
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
        ready: function () { }
    }
};
$("#divGrid_Orgstatus").DXjqxGrid(JqxParam_Grid_Orgstatus);
$('#divGrid_Orgstatus').on('rowselect', function (event) {
    var args = event.args;
    Id_Status = args.row['Id_Organization'];
    $("#btnEditXform_Orgstatus").prop("disabled", false);
    $("#frmXform_Orgstatus").dxXform('Get', {
        id: Id_Status
    });
});


// Sub grid req
// Grid Slave Status
// Grid Slave Status
// -------- Requriement Grid
$("#btnNewXform_Statusrequirements").on('click', function (e) {

});
$("#btnEditXform_Statusrequirements").on('click', function (e) {

});
$("#btnDeleteXform_Statusrequirements").on('click', function (e) {

});

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

var Statusrequirements_NestedSource = {
    datatype: "json",
    datafields: Statusrequirements_datafields,
    url: XCt.baseSiteURL + XCt.workController + '/GetRecords_Statusrequirements',
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
    url: XCt.baseSiteURL + XCt.workController + '/GetValidDates',
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
            Selectdate = test;
            Id_Entity = 0;
            Level = 20;
            $('#divTree_Organization').DXjqxTreeReload({
                Id: Id_Entity,
                level:Level,
                recordtype: 'OrgItem',
                selecdate: Selectdate
            }
                );
        }
    }
})
// ------- Control
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
//fnSetDisable(true);
DXSetState('Statusmode');


LoadComplettCallback();
//$("#IdBodyStart").hide();
//$("#IdBody").show();
