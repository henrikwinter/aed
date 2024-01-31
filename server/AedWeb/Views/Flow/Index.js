

$("#FlowBar").jqxNavigationBar({ theme: pageVariable.Jqwtheme, height: 300 });


//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Flow' + url;
    }
};


var Flows_datafields = [
  { name: 'Id_Flow', type: 'decimal' },
  { name: 'Id_Parrentflow', type: 'decimal' },
  { name: 'Bid_Flow', type: 'string' },
  { name: 'Flowname', type: 'string' },
  { name: 'Stepname', type: 'string' },
  { name: 'Controller', type: 'string' },
  { name: 'Action', type: 'string' },
  { name: 'Title', type: 'string' },
  { name: 'Flowhistory', type: 'string' },
  { name: 'Pvariables', type: 'string' },
  { name: 'Recordvalidfrom', type: 'DateTime' },
  { name: 'Recordvalidto', type: 'DateTime' },
  { name: 'Datavalidfrom', type: 'DateTime' },
  { name: 'Datavalidto', type: 'DateTime' },
  { name: 'Status', type: 'string' },
  { name: 'Creator', type: 'string' },
  { name: 'Modifiers', type: 'string' },
  { name: 'Orggroup', type: 'decimal' },
  { name: 'Attributum', type: 'string' },
  { name: 'Property', type: 'string' },
  { name: 'Assignment', type: 'string' }
];
var Flows_columns = [
 //{ text: '§Id_Flow§', datafield: 'Id_Flow', width: 30 },
 //{ text: '§Id_Parrentflow§', datafield: 'Id_Parrentflow' },
 { text: '§Bid_Flow§', datafield: 'Bid_Flow', columntype: 'textbox', filtertype: 'textbox', width: 100 },
 { text: '§Title§', datafield: 'Title' },
 { text: '§Flowname§', datafield: 'Flowname', width: 120 },
 { text: '§Stepname§', datafield: 'Stepname', width: 120 },
 //{ text: '§Controller§', datafield: 'Controller' },
 //{ text: '§Action§', datafield: 'Action' },
  //{ text: '§Flowhistory§', datafield: 'Flowhistory' },
 //{ text: '§Pvariables§', datafield: 'Pvariables' },
 { text: '§Recordvalidfrom§', datafield: 'Recordvalidfrom', cellsalign: 'right', width: 130 },
 //{ text: '§Recordvalidto§', datafield: 'Recordvalidto' },
 //{ text: '§Datavalidfrom§', datafield: 'Datavalidfrom' },
 //{ text: '§Datavalidto§', datafield: 'Datavalidto' },
 //{ text: '§Status§', datafield: 'Status' },
 //{ text: '§Creator§', datafield: 'Creator' },
 //{ text: '§Modifiers§', datafield: 'Modifiers' },
 //{ text: '§Orggroup§', datafield: 'Orggroup' },
 { text: '§Attributum§', datafield: 'Attributum', width: 100 }
 //{ text: '§Property§', datafield: 'Property' },
 //{ text: '§Assignment§', datafield: 'Assignment' }
];
WorkData.Flows_Data = {
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

// Urls constans
var GetXform_Flows = '/GetRowdetails';
var GetRecords_Flows = '/GetFlows';

var initrowdetailsXform_Flows = function (index, parentElement, gridElement, idatarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        if (idatarecord == null) {
            $("#FlowsTable").jqxGrid('hiderowdetails', index);
        } else {
            AjaxGet(urls(GetXform_Flows), { id_flow: idatarecord.Id_Flow }, function (Data) {
                var temstr = '<div class="row small" style="background-color:#f9f9f9;">';
                temstr += Data.rendered;
                temstr += '</div>';
                var container = $(temstr);
                $(details).append(container);
            })
        }
    }
};
function fnloadCompleteGrid_Flows(data) {
    if (data.length > 0) {
    } else {
    }
};
var JqxParam_Grid_Flows = {
    url: urls(GetRecords_Flows),
    datafields: Flows_datafields,
    ajax_data: { id: 0, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Flows,
    param: {
        columns: Flows_columns,
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
        rowdetailstemplate: { rowdetails: '<div style="margin:5px;"></div>', rowdetailsheight: 200 },
        initrowdetails: initrowdetailsXform_Flows,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Flows").DXjqxGrid(JqxParam_Grid_Flows);
$('#divGrid_Flows').on('rowdoubleclick', function (event) {
    var data = event.args.row.bounddata;
    url = pageVariable.baseSiteURL + data.Controller + "/" + data.Action + "?Id_Flow=" + data.Id_Flow;
    $(location).attr("href", url);
});




// A vegere egyszer kell!
$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[4]).addClass('active');