var JqxParam_GridDefault = {
    url: null,
    datafields: null,
    ajax_data: null,
    loadComplete: null,
    param: {
        columns: null,
        width: '100%',
        localization: getLocalization('hu'),
        columnsresize: true,
        pageable: true,
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        showtoolbar: false
    }
};
var JqxParam_TreeDefault = {
    url: null,
    datafields: null,
    mapp: null,
    ajax_data: null,
    id_hierarchy: 'id',
    id_parent_hierarchy: 'idparent',
    loadComplete: null,
    contextMenu: null,
    urlforiconxml: pageVariable.baseSiteURL + "Files/Xml/Codes/Icon-1.1.xml",
    iconref: 'Recordtype',
    param: {
        width: '100%',
        theme: pageVariable.Jqwtheme,
        allowDrag: false,
        allowDrop: false,
        dragStart: null,
        dragEnd: null
    }
};

var JobsSelectTree_jqxTree = {
    url: pageVariable.baseSiteURL + 'AjaxData/GetJobsHierarchy',
    datafields: Jobs_datafields,
    mapp: [{ name: 'Id_Jobs', map: 'id' }, { name: 'Name', map: 'html' }, { name: 'Rec', map: 'value' }],
    ajax_data: { id: null, date: null, level: 10 },
    id_hierarchy: 'Id_Jobs',
    id_parent_hierarchy: 'Id_Parent',
    loadComplete: null,
    contextMenu: null,
    urlforiconxml: pageVariable.baseSiteURL + "Files/Xml/Codes/Icon-1.1.xml",
    iconref: 'Jobscategory',
    param: {
        width: '100%',
        theme: pageVariable.Jqwtheme,
        allowDrag: true,
        allowDrop: true,
        dragStart: null,
        dragEnd: null
    }
};




//------------------
// Personpropertyes
try {
    var PersonPropertyes_jqxGrid = JCloneParam(JqxParam_GridDefault, {
        url: pageVariable.baseSiteURL + "AjaxData/GetPersonpropertyesrecords",
        datafields: Personpropertyesforview_datafields,
        ajax_data: {
            id_persons: model_ClientPart.io_SelectedPersonId
            //id_persons: args(PersonPropertyes_jqxGrid_id, '0')
        }
    }, {
        theme: pageVariable.JqwthemeAlt,
        altrows: true,
        columns: Personpropertyesforview_columns,
    });
} catch (err) { };

//-------------------------
// OrganizationSelectTree
var OrganizationSelectTree_jqxTree = JCloneParam(JqxParam_TreeDefault, {
    url: pageVariable.baseSiteURL + "AjaxData/GetOrgHierarchy",
    datafields: Organization_datafields_forTree,
    id_hierarchy : 'Id_Organization',
    id_parent_hierarchy: 'Id_Parent',
    iconref:'Orgtype',
    ajax_data: { id: null, date: null, level: 10 },
    mapp: [
    { name: 'Id_Organization', map: 'id' },
    { name: 'Name', map: 'html' },
    { name: 'Rec', map: 'value' }
    ]
},{
    theme: pageVariable.JqwthemeAlt
    //height : 350
});




// ------- Teszt -----------
var jqxUsersGrid = JClone(JqxParam_GridDefault);
jqxUsersGrid.url = pageVariable.baseSiteURL + "Account/GetUsers";
jqxUsersGrid.datafields = Users_datafields,
jqxUsersGrid.param.columns = FilterColumns(Users_columns, ["Email", "Username", "Id"]);
jqxUsersGrid.param.theme = pageVariable.JqwthemeAlt;

