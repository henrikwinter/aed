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

//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Persons' + url;
    }
};

WorkData.Vw_Personsproperty_Data = {
    Current: {
        Id: 0,
        Id_Parent: WorkData.model_ClientPart.io_SelectedPersonId,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: '',
        table: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {}
};

// Urls constans
var GetXform_Vw_Personsproperty = '/GetXform_Vw_Personsproperty';
var InsertXform_Vw_Personsproperty = '/InsertXform_Vw_Personsproperty';
var SaveXform_Vw_Personsproperty = '/SaveXform_Vw_Personsproperty';
var DeleteXform_Vw_Personsproperty = '/DeleteXform_Vw_Personsproperty';
var GetRecords_Vw_Personsproperty = '/GetRecords_Vw_Personsproperty';
//var GetRecords_Vw_Personsproperty = '/GetRecords_Vw_Personsproperty_Session';
var GetHierarchy_Vw_Personsproperty = '/GetHierarchy_Vw_Personsproperty';
var MoveHierarchyItem_Vw_Personsproperty = '/MoveHierarchyItem_Vw_Personsproperty';

var Vw_Personsproperty_columns = [
{ text: '§Id_Personsproperty§', datafield: 'Id_Personsproperty' ,width:50},
//{ text: '§Id_Flows§', datafield: 'Id_Flows' },
{ text: '§Id_Persons§', datafield: 'Id_Persons', width: 50 },
{ text: '§Description§', datafield: 'Description' },
{ text: '§Recordtype§', datafield: 'Recordtype' },
{ text: '§Datefrom§', datafield: 'Datefrom' },
{ text: '§Dateto§', datafield: 'Dateto' },
//{ text: '§Ready§', datafield: 'Ready' },
//{ text: '§Conductedyear§', datafield: 'Conductedyear' },
{ text: '§Stable§', datafield: 'Stable' }
//{ text: '§Recordvalidfrom§', datafield: 'Recordvalidfrom' },
//{ text: '§Recordvalidto§', datafield: 'Recordvalidto' },
//{ text: '§Datavalidfrom§', datafield: 'Datavalidfrom' },
//{ text: '§Datavalidto§', datafield: 'Datavalidto' },
//{ text: '§Status§', datafield: 'Status' },
//{ text: '§Creator§', datafield: 'Creator' },
//{ text: '§Modifiers§', datafield: 'Modifiers' },
//{ text: '§Orggroup§', datafield: 'Orggroup' },
//{ text: '§Attributum§', datafield: 'Attributum' },
//{ text: '§Property§', datafield: 'Property' },
//{ text: '§Assignment§', datafield: 'Assignment' }
];
var Vw_Personsproperty_datafields = [
  { name: 'Id_Personsproperty', type: 'decimal' },
  { name: 'Id_Flows', type: 'decimal' },
  { name: 'Id_Persons', type: 'decimal' },
  { name: 'Description', type: 'string' },
  { name: 'Recordtype', type: 'string' },
  { name: 'Datefrom', type: 'DateTime' },
  { name: 'Dateto', type: 'DateTime' },
  { name: 'Ready', type: 'string' },
  { name: 'Conductedyear', type: 'decimal' },
  { name: 'Stable', type: 'string' },
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

function fnSetXform_Vw_Personsproperty(Data, isView) {

    $("#divXformHtml_Vw_Personsproperty").html(Data.Xform);
    if (isView) $("#divXformHtmlView_Vw_Personsproperty").html(Data.XformView); //fnSetCommonView("nnn", Data.XformView, "text-primary");

    $("#frmXform_Vw_Personsproperty .editor").jqte();
    JqwTransformWithRole('frmXform_Vw_Personsproperty');
    try { fnOnComplett(xformcallackdata); } catch (err) { }
    try { $('#frmXform_Vw_Personsproperty').jqxValidator({
            onSuccess: function () {
                WorkData.Vw_Personsproperty_Data.Valid = true;
            },
            onError: function () {
                WorkData.Vw_Personsproperty_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Vw_Personspropertyrules)),
            hintType: "label"
        });     } catch (err) { }

    //$("#_GeneralActivity_Datefrom").jqxDateTimeInput('setDate', new Date(1957, 01, 06));
};
function fnInsertXform_Vw_Personsproperty() {
    try { fnOnBeforeInsert({}); } catch (err) { }        //CXformBeforeSave();
    try {
        $("#frmXform_Vw_Personsproperty").jqxValidator('validate');

    } catch (error) { }
    if (WorkData.Vw_Personsproperty_Data.Current.Id_Parent != null && WorkData.Vw_Personsproperty_Data.Valid == true) {
        $("#frmXform_Vw_Personsproperty").mYPostFormNew(urls(InsertXform_Vw_Personsproperty), {
            Id_Parent: WorkData.Vw_Personsproperty_Data.Current.Id,
            Id_ParentId_Parent: WorkData.Vw_Personsproperty_Data.Current.Id_Parent,
            Recordtype: "recordtype",
            Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow,
            table: WorkData.Vw_Personsproperty_Data.Current.table
        }, function (Data) {
            WorkData.Vw_Personsproperty_Data.Current.Id = Data.Entity.Id_Personsproperty;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_PersonspropertyTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Vw_Personsproperty").DXjqxGridReload({ id: WorkData.model_CurrentFlowstep.Id_Flow, recordtype: '' });
               // fnGetPropertyesInfo();
                $("#divCreateOrEditModal_Vw_Personsproperty").modal('hide');
            }
        });
    } else {
        GlobError("InsertXform Vw_Personsproperty ValidateError!", 10);
    }
};
function fnSaveXform_Vw_Personsproperty() {
    
    try { fnOnBeforeSave({}); } catch (err) { }  //CXformBeforeSave();
    try {
        $("#frmXform_Vw_Personsproperty").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Vw_Personsproperty_Data.Valid) {
  
        $("#frmXform_Vw_Personsproperty").mYPostFormNew(urls(SaveXform_Vw_Personsproperty), {
            Id_Entity: WorkData.Vw_Personsproperty_Data.Current.Id,
            Recordtype: 'Entity',
            table: WorkData.Vw_Personsproperty_Data.Current.table.toProperCase()
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_PersonspropertyTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Vw_Personsproperty").DXjqxGridReload({ id: WorkData.model_CurrentFlowstep.Id_Flow, recordtype: '' });
                //fnGetPropertyesInfo();
                $("#divCreateOrEditModal_Vw_Personsproperty").modal('hide');
            }
        });
    } else {
        GlobError("SaveXform Vw_Personsproperty Error!", 10);
    }
};
function fnDeleteXform_Vw_Personsproperty() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Vw_Personsproperty), {
                Id_Entity: WorkData.Vw_Personsproperty_Data.Current.Id,
                table: WorkData.Vw_Personsproperty_Data.Current.table.toProperCase(),
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedVw_PersonspropertyTitle"), Data.Error.Errormessage);
                } else {
                    $("#divGrid_Vw_Personsproperty").DXjqxGridReload({ id: WorkData.model_CurrentFlowstep.Id_Flow, recordtype: '' });
                   // fnGetPropertyesInfo();
                    $("#divCreateOrEditModal_Vw_Personsproperty").modal('hide');
                }
            });
        } else { }
    });
};
function fnGetXform_Vw_Personsproperty() {
    
    WorkData.Vw_Personsproperty_Data.Current.ChangedName = WorkData.Vw_Personsproperty_Data.Current.Name;
    AjaxGet(urls(GetXform_Vw_Personsproperty), {
        Id_Entity: WorkData.Vw_Personsproperty_Data.Current.Id,
        table: WorkData.Vw_Personsproperty_Data.Current.table.toProperCase(),
        Id_Xform: 'frmXform_Vw_Personsproperty'
    }, function (Data) {

        fnSetXform_Vw_Personsproperty(Data, true);
    });
};
function fnNewXform_Vw_Personsproperty() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Vw_Personsproperty_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Vw_Personsproperty'
    }, function (Data) {
        fnSetXform_Vw_Personsproperty(Data, false);
        $.each(WorkData.Vw_Personsproperty_Data.TempFormValue, function (key, value) {
            $("#frmXform_Vw_Personsproperty input[id*='" + key + "']").val(value);
        });
        WorkData.Vw_Personsproperty_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Vw_Personsproperty() {
    $("#frmXform_Vw_Personsproperty").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Vw_Personsproperty',
        NewXformRoot: WorkData.Vw_Personsproperty_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Vw_Personsproperty(Data);
    });
};

$("#div_PropertyXformRoot_Jqxmenu").DXjqxRootsMenu({
    url: pageVariable.baseSiteURL + "Ajax/BuildRootSelectorMenuWithComplexType",
    filter: 'SubPropType',
    param: {
        theme: pageVariable.JqwthemeMenu,
        showTopLevelArrows: true,
        width: '100%'
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Vw_Personsproperty_Data.Current.Root = c.Res[0].value;
            //fnBind_cmbXformRootSelect_CangeEvent_Vw_Personsproperty();
        } catch (Error) { }
    }
});
$('#div_PropertyXformRoot_Jqxmenu').on('itemclick', function (event) {
    var record = GetSelectedRootsMenurecord($(this).data(), event.args);
    WorkData.Vw_Personsproperty_Data.Current.Root = record.value;
    WorkData.Vw_Personsproperty_Data.Current.table = record.Table;

    fnNewXform_Vw_Personsproperty();
    fnBind_btnSaveXform_Vw_Personsproperty(fnInsertXform_Vw_Personsproperty);

    //$('#chkInsertPosition').prop('checked', false);
    //$("#chkInsertPosition").closest("label").show();
    //$("#BodyLoader").jqxLoader('close');
    $("#divCreateOrEditModal_Vw_Personsproperty").modal('show');

    
});

// Buttons
function fnBind_btnSaveXform_Vw_Personsproperty(process_function) {
    $("#btnSaveXform_Vw_Personsproperty").off('click');
    $("#btnSaveXform_Vw_Personsproperty").on('click', function (e) {
        process_function();
    });
};




// Example !!!
var initrowdetailsXform_Vw_Personsproperty = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        AjaxSync(urls(GetXform_Vw_Personsproperty), {
            Id_Entity: WorkData.Vw_Personsproperty_Data.Current.Id,
            table: WorkData.Vw_Personsproperty_Data.Current.table.toProperCase(),
            Id_Xform: 'frmXform_Vw_Personsproperty'
        }, function (Data) {
            var temstr = '';
            temstr += Data.XformView;
            temstr += '';
            var container = $(temstr);
            $(details).append(container);
        });
    }
};

function fnloadCompleteGrid_Vw_Personsproperty(data) {
    if (data.length > 0) {
    } else {
    }
};
var JqxParam_Grid_Vw_Personsproperty = {
    url: urls(GetRecords_Vw_Personsproperty),
    datafields: Vw_Personsproperty_datafields,
    ajax_data: { id: WorkData.model_CurrentFlowstep.Id_Flow, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Vw_Personsproperty,
    param: {
        columns: Vw_Personsproperty_columns,
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
        rowdetailstemplate: { rowdetails: '<div style="width:98%;min-height:195px;margin:2px;border:1px solid grey;"></div>', rowdetailsheight: 200 },
        initrowdetails: initrowdetailsXform_Vw_Personsproperty,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Vw_Personsproperty").DXjqxGrid(JqxParam_Grid_Vw_Personsproperty);

$('#divGrid_Vw_Personsproperty').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Vw_Personsproperty_Data.Current.Id = args.row.Id_Personsproperty;
   // fnGetXform_Vw_Personsproperty();
    fnBind_btnSaveXform_Vw_Personsproperty(fnSaveXform_Vw_Personsproperty);
});
$('#divGrid_Vw_Personsproperty').on('rowclick', function (event) {
    var args = event.args;
  
    WorkData.Vw_Personsproperty_Data.Current.Id = args.row.bounddata.Id_Personsproperty;
    WorkData.Vw_Personsproperty_Data.Current.table = args.row.bounddata.Stable;
   // fnGetXform_Vw_Personsproperty();
});
$('#divGrid_Vw_Personsproperty').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Vw_Personsproperty_Data.Current.Id = args.row.Id_Personsproperty;
    //$("#SelectedVw_PersonspropertyTitle").html('Selected: ' + args.row.Usedname);	
});
function fnGridToolbarReady_Vw_Personsproperty() {
    fnBindDefButtonClick_Vw_Personsproperty();
};

function fnBind_btnSaveXform_Vw_Personsproperty(process_function) {
    $("#btnSaveXform_Vw_Personsproperty").off('click');
    $("#btnSaveXform_Vw_Personsproperty").on('click', function (e) {
        process_function();
    });
};
function fnBindDefButtonClick_Vw_Personsproperty() {
    $("#btnNewXform_Vw_Personsproperty").on('click', function (e) {
        fnNewXform_Vw_Personsproperty();
        fnBind_btnSaveXform_Vw_Personsproperty(fnInsertXform_Vw_Personsproperty);
        $("#divCreateOrEditModal_Vw_Personsproperty").modal('show');
    });
    $("#btnEditXform_Vw_Personsproperty").on('click', function (e) {
        fnGetXform_Vw_Personsproperty();
        fnBind_btnSaveXform_Vw_Personsproperty(fnSaveXform_Vw_Personsproperty);
        $("#divCreateOrEditModal_Vw_Personsproperty").modal('show');
    });
    $("#btnDeleteXform_Vw_Personsproperty").on('click', function (e) {
        
        fnDeleteXform_Vw_Personsproperty();
    });
};
fnBindDefButtonClick_Vw_Personsproperty();


var GetXform_AttachedPersons = '/GetXform_PersonsCard';
//-- Attached Persons
$("#btnAttachedPersons").on('click', function () {
    fnGetXform_AttachedPersons();
});

function fnGetXform_AttachedPersons() {
    AjaxGet(urls(GetXform_AttachedPersons), {
        Id: WorkData.Vw_Personsproperty_Data.Current.Id_Parent,
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
function fnGetPropertyesInfo(iroot, callaback) {
    AjaxGet(urls('/GetPropertyesInfo'), {
        Id: WorkData.model_CurrentFlowstep.Id_Flow,
        root: iroot,            //'Pin',
        formid: 'formid'
    }, function (Data) {
        if (Data.Error.Errorcode == 0) {
            //$("#divSummhtml").html(Data.Forms[1]);
        }
        callaback(Data.Forms[1]);
    });
};


$("#btnSwitchVw_PersonspropertyMode").on('click', function (e) {
    fnGetPropertyesInfo('Pin', function (sss) {
        $("#divSummhtml").html(sss);
    });
    fnGetPropertyesInfo('Qulai', function (sss) {
        $("#divSummhtml1").html(sss);
    });

});
$("#divCopyVw_Personsproperty").on('show.bs.collapse', function (e) {
    fnGetPropertyesInfo('Pin', function (sss) {
        $("#divSummhtml").html(sss);
    });
    fnGetPropertyesInfo('Qulai', function (sss) {
        $("#divSummhtml1").html(sss);
    });
});



AjaxGet(urls(GetXform_AttachedPersons), {
    Id: WorkData.Vw_Personsproperty_Data.Current.Id_Parent,
    Id_Xform: ''  //'frmXform_AttachedPersons'
}, function (Data) {
    if (Data.Error.Errorcode == 0) {
        
        $("#txtPersonname").html(Data.Personname);
    }
});



$("#IdBodyStart").hide();
$("#IdBody").show();
