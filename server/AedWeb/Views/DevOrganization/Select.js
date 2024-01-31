include('~/Scripts/DextraFlowManParial.js' | args(0, 0));
var CheckPostData = function () {
    WorkData.ClientPartName = 'Org';
    WorkData.model_ClientPart.retval = 'Nincs kivalasztva Szervezet!';
    if (WorkData.Organization_Data.Current.Id != 0) {
        WorkData.model_ClientPart.retval = c_FlowPostpreCheckretOk;
        WorkData.model_ClientPart.io_SelectedOrgId = WorkData.Organization_Data.Current.Id;
    }
    return WorkData.model_ClientPart;
};


//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'DevOrganization' + url;
    }
};

var GetHierarchy_Organization = '/PreGetHierarchy_Organization';
var Get_InProgressDevOrganization = '/Get_InProgressDevOrganization';

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
var Organization_datafields = [
  { name: 'Id_Organization', type: 'decimal' },
  { name: 'Id_Parent', type: 'decimal' },
  { name: 'Id_Ord', type: 'decimal' },
  { name: 'Bid_Organization', type: 'string' },
  { name: 'Name', type: 'string' },
  { name: 'Status', type: 'string' },
  { name: 'Title', type: 'string' },
  { name: 'Shortname', type: 'string' },
  { name: 'Xmldata', type: 'string' },
  { name: 'Recordtype', type: 'string' },
  { name: 'Id_Flows', type: 'decimal' },
  { name: 'Cpyid', type: 'decimal' },
  { name: 'Cpyparrent', type: 'decimal' },
  { name: 'Id_Files', type: 'decimal' },
  { name: 'Id_Scopeofactivity', type: 'decimal' }
];

function OrganizationTree_BeforeLoadcompletEvent(records) {

    var data = new Array();
    for (var i = 0; i < records.length; i++) {
        var org = records[i];
        if (org.Status == 'InDeveloped') {
            org.Name = '<span class="red">' + org.Name + '</span>';
            org.disabled = true;
        } else if (org.Datavalidto != null) {
            org.Name = '<span class="green">' + org.Name + '</span>';
            //org.disabled = true;
        } else {
            org.Name = '<span class="gray">' + org.Name + '</span>';
        }

        org.Rec = org;
        data.push(org);
    }
    return data;
};
var JqxTreeParam_PreselectOrganization = {
    url: urls(GetHierarchy_Organization),
    datafields: Organization_datafields,
    ajax_data: {
        Id: 0,
        level: 10,
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
    beforeLoadComplete: OrganizationTree_BeforeLoadcompletEvent,
    loadComplete: function (data) {
    },
    param: {
        width: 300, height: 220,
        theme: pageVariable.Jqwtheme
    }
};

$("#divOrganizationPreselect").jqxDropDownButton({ width: 300, height: 25 });
$('#PreselectOrganizationTree').DXjqxTree(JqxTreeParam_PreselectOrganization);
$('#PreselectOrganizationTree').on('select', function (event) {
    var args = event.args;
    var item = $('#PreselectOrganizationTree').jqxTree('getItem', args.element);
    WorkData.Organization_Data.Current.Id = item.id;
    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + item.label + '</div>';
    $("#divOrganizationPreselect").jqxDropDownButton('setContent', dropDownContent);
   
    $("#divOrganizationPreselect").jqxDropDownButton("close");
});

AjaxGet(urls(Get_InProgressDevOrganization), {}, function (Data) {
    var hcont='<ul class="list-group">';
    $.each(Data.ret, function (key, value) {
        var pline = '<li class="list-group-item">' + value.Bid_Flow + '-' + value.Flowname + '</li>';
        hcont += pline;
    })
    hcont += '</ul> ';
    

    $("#IdInprogressDev").html( hcont);
});


$("#IdBodyStart").hide();
$("#IdBody").show();
