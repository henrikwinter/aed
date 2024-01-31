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

var GetRecords_Status = '/GetCheckList';

var Status_columns = [
    {
        text: '§Id_Organization§',
        datafield: 'Id_Organization', editable: false, width: 50
    }, {
        text: '§Id_Persons§',
        datafield: 'Id_Persons', editable: false, width: 50
    }, {
        text: '§Id_Mxrf_Persons_Org§',
        datafield: 'Id_Mxrf_Persons_Org', editable: false, width: 50
    }, {
        text: '§Shortname§',
        datafield: 'Shortname', editable: false
    }, {
        text: '§Usedname§',
        datafield: 'Usedname', editable: false
    }, {
        text: '§OrigOrglongname§',
        datafield: 'Origorglongname', editable: false
    }
];
var Status_datafields = [{
    name: 'Id_Organization',
    type: 'decimal'
}, {
    name: 'Id_Persons',
    type: 'decimal'
}, {
    name: 'Id_Mxrf_Persons_Org',
    type: 'decimal'
}, {
    name: 'Usedname',
    type: 'string'
}, {
    name: 'Attributum',
    type: 'string'
}, {
    name: 'Assignment',
    type: 'string'
}, {
    name: 'Checkstate',
    type: 'string'
}, {
    name: 'Neworglongname',
    type: 'string'
}, {
    name: 'Shortname',
    type: 'string'
}, {
    name: 'Origorglongname',
    type: 'string'
}
];
var initrowdetailsGrid_Status = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        AjaxSync(urls(GetXform_Status), {
            Id_Entity: datarecord.Id_Organization,
            Id_Xform: ''
        }, function (Data) {

            var temstr = '<div class="row small" style="background-color:#f9f9f9;">';
            temstr += Data.XformView;
            temstr += '</div>';
            var container = $(temstr);
            $(details).append(container);
        });
    }
};
var JqxParam_Grid_Status = {
    url: urls(GetRecords_Status),
    datafields: Status_datafields,
    ajax_data: {
        Id_Flow: WorkData.model_CurrentFlowstep.Id_Flow,
        mode:1
    },
    //loadComplete: fnloadCompleteGrid_Status,
    param: {
        columns: Status_columns,
        width: '100%',
        theme: pageVariable.Jqwtheme,
        localization: getLocalization('hu'),
        columnsresize: true,
        editable: true,
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


$("#IdBodyStart").hide();
$("#IdBody").show();
