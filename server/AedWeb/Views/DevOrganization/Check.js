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

// Status (Slave1 Entiy) ----------------------------------------------------------------------------------------------------------------------------------
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

var GetXform_Status = '/GetXform_Status';
var GetRecords_Status = '/GetCheckList';
var UpdateCheckAction = '/UpdateCheckAction';


var cellclassname1 = function (row, columnfield, value, data) {
    if (value == '-na-') return '';
    return 'gray';
};
var cellclassname2 = function (row, columnfield, value, data) {
    if (value == 'Original') return '';
    return 'gray';
};

var cellclassname3 = function (row, columnfield, value, data) {
    if (data.Checkstate == 'Original' && data.Attributum == '-na-') return '';
    return 'gray';
};

var Status_columns = [
    {
        text: '§Id_Organization§',
        datafield: 'Id_Organization',editable: false ,width:50 
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
        text: '§NewOrglongname§',
        datafield: 'Neworglongname', editable: false
    }, {
        text: '§OrigOrglongname§',
        datafield: 'Origorglongname', editable: false
    }, {
        text: '§Attributum§',
        datafield: 'Attributum', editable: false, width: 80 //, cellclassname: cellclassname1
    }, {
        text: '§Checkstate§',
        datafield: 'Checkstate', editable: false, width: 80 //, cellclassname: cellclassname2
    }, {
        text: 'Action', datafield: 'Assignment', width: 150, cellclassname: cellclassname3,
        columntype: 'dropdownlist',
        createeditor: function (row, column, editor) {
         // assign a new data source to the dropdownlist.
         var list = ['Continoue', 'Modify'];
         editor.jqxDropDownList({ autoDropDownHeight: true, source: list });
     },
     // update the editor's value before saving it.
        cellvaluechanging: function (row, column, columntype, oldvalue, newvalue) {
         // return the old value, if the new value is empty.
         //debugger;
         var data = $('#divGrid_Status').jqxGrid('getrowdata', row);
         AjaxGet(urls(UpdateCheckAction), { Id_Status: data.Id_Organization, value: newvalue }, function (Data) { });
         if (newvalue == "") return oldvalue;
     }
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
        mode:0
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
$('#divGrid_Status').on('rowdoubleclick', function (event) {
    var args = event.args;
});
$('#divGrid_Status').on('rowclick', function (event) {
    var args = event.args;
});
$('#divGrid_Status').on('rowselect', function (event) {
    var args = event.args;
    WorkData.Status_Data.Current.Id = args.row.Id_Organization;
});


$("#ActivatteDate").jqxCalendar({
    width: '200px',
    height: '200px',
    theme: pageVariable.Jqwtheme
});
//$('#ActivatteDate').jqxCalendar('setMinDate', new Date());
$("#ActivatteDate").jqxCalendar({ culture: 'hu-HU' })
$('#ActivatteDate').on('change', function (event) {
    var date = moment(event.args.date).format("YYYY.MM.DD.");
    $("#btnActivate").html("Activate :" + date);
    WorkData.model_ClientPart.Activate_Date = date;
});



$("#IdBodyStart").hide();
$("#IdBody").show();