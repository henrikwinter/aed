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
        return pageVariable.baseSiteURL + 'Animal' + url;
    }
};

WorkData.Vw_Animalproperty_Data = {
    Current: {
        Id: 0,
        Id_Parent: WorkData.model_ClientPart.io_SelectedAnimalId,
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





// FAKE FLOW ID !!!!
WorkData.model_CurrentFlowstep = { Id_Flow: 11 };

// Urls constans
var GetXform_Vw_Animalproperty = '/GetXform_Vw_Animalproperty';
var InsertXform_Vw_Animalproperty = '/InsertXform_Vw_Animalproperty';
var SaveXform_Vw_Animalproperty = '/SaveXform_Vw_Animalproperty';
var DeleteXform_Vw_Animalproperty = '/DeleteXform_Vw_Animalproperty';
var GetRecords_Vw_Animalproperty = '/GetRecords_Vw_Animalproperty';
//var GetRecords_Vw_Animalproperty = '/GetRecords_Vw_Animalproperty_Session';
var GetHierarchy_Vw_Animalproperty = '/GetHierarchy_Vw_Animalproperty';
var MoveHierarchyItem_Vw_Animalproperty = '/MoveHierarchyItem_Vw_Animalproperty';

var Vw_Animalproperty_columns = [
{ text: '§Id_Animalproperty§', datafield: 'Id_Animalproperty' ,width:50},
//{ text: '§Id_Flows§', datafield: 'Id_Flows' },
{ text: '§Id_Animal§', datafield: 'Id_Animal', width: 50 },
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
var Vw_Animalproperty_datafields = [
  { name: 'Id_Animalproperty', type: 'decimal' },
  { name: 'Id_Flows', type: 'decimal' },
  { name: 'Id_Animal', type: 'decimal' },
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

function fnSetXform_Vw_Animalproperty(Data, isView) {

    $("#divXformHtml_Vw_Animalproperty").html(Data.Xform);
    if (isView) $("#divXformHtmlView_Vw_Animalproperty").html(Data.XformView); //fnSetCommonView("nnn", Data.XformView, "text-primary");

    $("#frmXform_Vw_Animalproperty .editor").jqte();
    JqwTransformWithRole('frmXform_Vw_Animalproperty');
    try { fnOnComplett(xformcallackdata); } catch (err) { }
    try { $('#frmXform_Vw_Animalproperty').jqxValidator({
            onSuccess: function () {
                WorkData.Vw_Animalproperty_Data.Valid = true;
            },
            onError: function () {
                WorkData.Vw_Animalproperty_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Vw_Animalpropertyrules)),
            hintType: "label"
        });     } catch (err) { }

    //$("#_GeneralActivity_Datefrom").jqxDateTimeInput('setDate', new Date(1957, 01, 06));
};
function fnInsertXform_Vw_Animalproperty() {
    try { fnOnBeforeInsert({}); } catch (err) { }        //CXformBeforeSave();
    try {
        $("#frmXform_Vw_Animalproperty").jqxValidator('validate');

    } catch (error) { }
    if (WorkData.Vw_Animalproperty_Data.Current.Id_Parent != null && WorkData.Vw_Animalproperty_Data.Valid == true) {
        $("#frmXform_Vw_Animalproperty").mYPostFormNew(urls(InsertXform_Vw_Animalproperty), {
            Id_Parent: WorkData.Vw_Animalproperty_Data.Current.Id,
            Id_ParentId_Parent: WorkData.Vw_Animalproperty_Data.Current.Id_Parent,
            Recordtype: "recordtype",
            Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow,
            table: WorkData.Vw_Animalproperty_Data.Current.table
        }, function (Data) {
            WorkData.Vw_Animalproperty_Data.Current.Id = Data.Entity.Id_Animalproperty;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_AnimalpropertyTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Vw_Animalproperty").DXjqxGridReload({ id: WorkData.model_ClientPart.io_SelectedAnimalId, recordtype: '' });
               // fnGetPropertyesInfo();
                $("#divCreateOrEditModal_Vw_Animalproperty").modal('hide');
            }
        });
    } else {
        GlobError("InsertXform Vw_Animalproperty ValidateError!", 10);
    }
};
function fnSaveXform_Vw_Animalproperty() {
    
    try { fnOnBeforeSave({}); } catch (err) { }  //CXformBeforeSave();
    try {
        $("#frmXform_Vw_Animalproperty").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Vw_Animalproperty_Data.Valid) {
  
        $("#frmXform_Vw_Animalproperty").mYPostFormNew(urls(SaveXform_Vw_Animalproperty), {
            Id_Entity: WorkData.Vw_Animalproperty_Data.Current.Id,
            Recordtype: 'Entity',
            table: WorkData.Vw_Animalproperty_Data.Current.table.toProperCase()
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedVw_AnimalpropertyTitle"), Data.Error.Errormessage);
            } else {
                $("#divGrid_Vw_Animalproperty").DXjqxGridReload({ id: WorkData.model_ClientPart.io_SelectedAnimalId, recordtype: '' });
                //fnGetPropertyesInfo();
                $("#divCreateOrEditModal_Vw_Animalproperty").modal('hide');
            }
        });
    } else {
        GlobError("SaveXform Vw_Animalproperty Error!", 10);
    }
};
function fnDeleteXform_Vw_Animalproperty() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Vw_Animalproperty), {
                Id_Entity: WorkData.Vw_Animalproperty_Data.Current.Id,
                table: WorkData.Vw_Animalproperty_Data.Current.table.toProperCase(),
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedVw_AnimalpropertyTitle"), Data.Error.Errormessage);
                } else {
                    $("#divGrid_Vw_Animalproperty").DXjqxGridReload({ id: WorkData.model_ClientPart.io_SelectedAnimalId, recordtype: '' });
                   // fnGetPropertyesInfo();
                    $("#divCreateOrEditModal_Vw_Animalproperty").modal('hide');
                }
            });
        } else { }
    });
};
function fnGetXform_Vw_Animalproperty() {
    
    WorkData.Vw_Animalproperty_Data.Current.ChangedName = WorkData.Vw_Animalproperty_Data.Current.Name;
    AjaxGet(urls(GetXform_Vw_Animalproperty), {
        Id_Entity: WorkData.Vw_Animalproperty_Data.Current.Id,
        table: WorkData.Vw_Animalproperty_Data.Current.table.toProperCase(),
        Id_Xform: 'frmXform_Vw_Animalproperty'
    }, function (Data) {

        fnSetXform_Vw_Animalproperty(Data, true);
    });
};
function fnNewXform_Vw_Animalproperty() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Vw_Animalproperty_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Vw_Animalproperty'
    }, function (Data) {
        fnSetXform_Vw_Animalproperty(Data, false);
        $.each(WorkData.Vw_Animalproperty_Data.TempFormValue, function (key, value) {
            $("#frmXform_Vw_Animalproperty input[id*='" + key + "']").val(value);
        });
        WorkData.Vw_Animalproperty_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Vw_Animalproperty() {
    $("#frmXform_Vw_Animalproperty").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Vw_Animalproperty',
        NewXformRoot: WorkData.Vw_Animalproperty_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Vw_Animalproperty(Data);
    });
};

$("#div_PropertyXformRoot_Jqxmenu").DXjqxRootsMenu({
    url: pageVariable.baseSiteURL +const_TypemenuGet, // "Ajax/BuildRootSelectorMenuWithComplexType",
    //url: pageVariable.baseSiteURL + "Ajax/BuildRootSelectorWithAllElement",
    filter: 'AnimalPropType',
    param: {
        theme: pageVariable.JqwthemeMenu,
        showTopLevelArrows: true,
        width: '100%'
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Vw_Animalproperty_Data.Current.Root = c.Res[0].value;
            //fnBind_cmbXformRootSelect_CangeEvent_Vw_Animalproperty();
        } catch (Error) { }
    }
});
$('#div_PropertyXformRoot_Jqxmenu').on('itemclick', function (event) {
    var record = GetSelectedRootsMenurecord($(this).data(), event.args);
    WorkData.Vw_Animalproperty_Data.Current.Root = record.value;
    WorkData.Vw_Animalproperty_Data.Current.table = record.Table;
    fnNewXform_Vw_Animalproperty();
    fnBind_btnSaveXform_Vw_Animalproperty(fnInsertXform_Vw_Animalproperty);

    //$('#chkInsertPosition').prop('checked', false);
    //$("#chkInsertPosition").closest("label").show();
    //$("#BodyLoader").jqxLoader('close');
    $("#divCreateOrEditModal_Vw_Animalproperty").modal('show');

    
});



// Buttons
function fnBind_btnSaveXform_Vw_Animalproperty(process_function) {
    $("#btnSaveXform_Vw_Animalproperty").off('click');
    $("#btnSaveXform_Vw_Animalproperty").on('click', function (e) {
        process_function();
    });
};




// Example !!!
var initrowdetailsXform_Vw_Animalproperty = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        AjaxSync(urls(GetXform_Vw_Animalproperty), {
            Id_Entity: WorkData.Vw_Animalproperty_Data.Current.Id,
            table: WorkData.Vw_Animalproperty_Data.Current.table.toProperCase(),
            Id_Xform: 'frmXform_Vw_Animalproperty'
        }, function (Data) {
            var temstr = '';
            temstr += Data.XformView;
            temstr += '';
            var container = $(temstr);
            $(details).append(container);
        });
    }
};

function fnloadCompleteGrid_Vw_Animalproperty(data) {
    if (data.length > 0) {
    } else {
    }
};
var JqxParam_Grid_Vw_Animalproperty = {
    url: urls(GetRecords_Vw_Animalproperty),
    datafields: Vw_Animalproperty_datafields,
    ajax_data: { id: WorkData.model_ClientPart.io_SelectedAnimalId, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Vw_Animalproperty,
    param: {
        columns: Vw_Animalproperty_columns,
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
        initrowdetails: initrowdetailsXform_Vw_Animalproperty,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Vw_Animalproperty").DXjqxGrid(JqxParam_Grid_Vw_Animalproperty);

$('#divGrid_Vw_Animalproperty').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Vw_Animalproperty_Data.Current.Id = args.row.Id_Animalproperty;
   // fnGetXform_Vw_Animalproperty();
    fnBind_btnSaveXform_Vw_Animalproperty(fnSaveXform_Vw_Animalproperty);
});
$('#divGrid_Vw_Animalproperty').on('rowclick', function (event) {
    var args = event.args;
  
    WorkData.Vw_Animalproperty_Data.Current.Id = args.row.bounddata.Id_Animalproperty;
    WorkData.Vw_Animalproperty_Data.Current.table = args.row.bounddata.Stable;
   // fnGetXform_Vw_Animalproperty();
});
$('#divGrid_Vw_Animalproperty').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Vw_Animalproperty_Data.Current.Id = args.row.Id_Animalproperty;
    //$("#SelectedVw_AnimalpropertyTitle").html('Selected: ' + args.row.Usedname);	
});
function fnGridToolbarReady_Vw_Animalproperty() {
    fnBindDefButtonClick_Vw_Animalproperty();
};

function fnBind_btnSaveXform_Vw_Animalproperty(process_function) {
    $("#btnSaveXform_Vw_Animalproperty").off('click');
    $("#btnSaveXform_Vw_Animalproperty").on('click', function (e) {
        process_function();
    });
};
function fnBindDefButtonClick_Vw_Animalproperty() {
    $("#btnNewXform_Vw_Animalproperty").on('click', function (e) {
        fnNewXform_Vw_Animalproperty();
        fnBind_btnSaveXform_Vw_Animalproperty(fnInsertXform_Vw_Animalproperty);
        $("#divCreateOrEditModal_Vw_Animalproperty").modal('show');
    });
    $("#btnEditXform_Vw_Animalproperty").on('click', function (e) {
        fnGetXform_Vw_Animalproperty();
        fnBind_btnSaveXform_Vw_Animalproperty(fnSaveXform_Vw_Animalproperty);
        $("#divCreateOrEditModal_Vw_Animalproperty").modal('show');
    });
    $("#btnDeleteXform_Vw_Animalproperty").on('click', function (e) {
        
        fnDeleteXform_Vw_Animalproperty();
    });
};
fnBindDefButtonClick_Vw_Animalproperty();


var GetXform_AttachedAnimal = '/GetXform_AnimalCard';
//-- Attached Animal
$("#btnAttachedAnimal").on('click', function () {
    fnGetXform_AttachedAnimal();
});

function fnGetXform_AttachedAnimal() {
    AjaxGet(urls(GetXform_AttachedAnimal), {
        Id: WorkData.Vw_Animalproperty_Data.Current.Id_Parent,
        Id_Xform: 'frmXform_AttachedAnimal'
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

            $("#divViewModal_AttachedAnimal").modal('show');
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


$("#btnSwitchVw_AnimalpropertyMode").on('click', function (e) {
    fnGetPropertyesInfo('Pin', function (sss) {
        $("#divSummhtml").html(sss);
    });
    fnGetPropertyesInfo('Qulai', function (sss) {
        $("#divSummhtml1").html(sss);
    });

});
$("#divCopyVw_Animalproperty").on('show.bs.collapse', function (e) {
    fnGetPropertyesInfo('Pin', function (sss) {
        $("#divSummhtml").html(sss);
    });
    fnGetPropertyesInfo('Qulai', function (sss) {
        $("#divSummhtml1").html(sss);
    });
});



AjaxGet(urls(GetXform_AttachedAnimal), {
    Id: WorkData.Vw_Animalproperty_Data.Current.Id_Parent,
    Id_Xform: ''  //'frmXform_AttachedAnimal'
}, function (Data) {
    if (Data.Error.Errorcode == 0) {
        
        $("#txtAnimalname").html(Data.Animalname);
    }
});



$("#IdBodyStart").hide();
$("#IdBody").show();
