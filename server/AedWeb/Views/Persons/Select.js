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
    WorkData.model_ClientPart.io_SelectedPersonId = WorkData.Persons_Data.Current.Id;
    return WorkData.model_ClientPart;
};


$("#ModalLoaderStart").jqxLoader({ text: "", width: 100, height: 60, imagePosition: 'center', theme: pageVariable.Jqwtheme });
//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Persons' + url;
    }
};


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
$("#divPersonDateList").jqxDropDownList({
    selectedIndex: 0,
    source: dataAdapterValidDates,
    displayMember: "Datavalidfrom",
    valueMember: "Datavalidfrom",
    width: 200,
    height: 30,
    theme: pageVariable.JqwthemeAlt
});
$("#divPersonDateList").on('select', function (event) {
    if (event.args) {
        var item = event.args.item;
        if (item) {
            var test = item.value;
            WorkData.SearchPersons_Data.Current.selectdate = test;

        }
    }
});



// Search logic Begin -----------------------------------------------------------
WorkData.SearchPersons_Data = {
    Current: {
        Id: 0,
        Id_Parent: 0,
        Name: '',
        ChangedName: '',
        Root: '',
        Properties: '',
        selectdate: ''
    },
    Valid: true
};
var SaveXform_SearchPersons = '/SaveXform_SearchPersons';
function fnSetXform_SearchPersons(Data, isView) {
    $("#divXformHtml_SearchPersons").html(Data.Xform);
    if (isView) $("#divXformHtmlView_SearchPersons").html(Data.XformView);
    $("#frmXform_SearchPersons .editor").jqte();
    JqwTransformWithRole('frmXform_SearchPersons');
    try {
        $('#frmXform_SearchPersons').jqxValidator({
            onSuccess: function () { WorkData.SearchPersons_Data.Valid = true; },
            onError: function () { WorkData.SearchPersons_Data.Valid = false; },
            rules: JSON.parse(JSON.stringify(frmXform_SearchPersonsrules)), hintType: "label"
        });
    } catch (err) { }
};
function fnSaveXform_SearchPersons() {
    try { $("#frmXform_SearchPersons").jqxValidator('validate'); } catch (err) { $("#BodyLoader").jqxLoader('close'); }
    if (WorkData.SearchPersons_Data.Valid) {
        $("#frmXform_SearchPersons").mYPostFormNew(urls(SaveXform_SearchPersons), {
            Recordtype: 'Entity', selector: 'Aschema',
            selecdate: WorkData.SearchPersons_Data.Current.selectdate
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedSearchPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Searchperson').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Searchperson").modal('hide');
                //DXSetState('Uploadmode');
                //$("#SelectedPersonsTitle").html('§Selected§:');
                $("#divGrid_Persons").DXjqxGridReload({ opt: '' });
                $("#BodyLoader").jqxLoader('close');
            }
        });
    } else {
        $("#BodyLoader").jqxLoader('close');
        GlobError("SaveXform SearchPersons Error!", 10);
    }
};
function fnNewXform_SearchPersons() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.SearchPersons_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_SearchPersons',
        selector: 'Aschema',
    }, function (Data) {
        fnSetXform_SearchPersons(Data, false);
    });
};
function fnXformChangeRoot_SearchPersons() {
    $("#frmXform_SearchPersons").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_SearchPersons',
        NewXformRoot: WorkData.SearchPersons_Data.Current.Root,
        NewXformRefRoot: '', selector: 'Aschema',
    }, function (Data) {
        fnSetXform_SearchPersons(Data);
    });
};
function fnBind_btnSaveXform_SearchPersons(process_function) {
    $("#btnSaveXform_SearchPersons").off('click');
    $("#btnSaveXform_SearchPersons").on('click', function (e) {
        process_function();
    });
};
function fnBind_cmbXformRootSelect_CangeEvent_SearchPersons() {
    $('#cmbXformRootSelect_SearchPersons').off('change');
    $('#cmbXformRootSelect_SearchPersons').on('change', function (event) {
        if (event.args) {

            var item = event.args.item;
            WorkData.SearchPersons_Data.Current.Root = item.value;
            fnXformChangeRoot_SearchPersons();
        }
    });
};
$("#cmbXformRootSelect_SearchPersons").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'PersonsearchFilter', selector: 'Aschema',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {
            WorkData.SearchPersons_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_SearchPersons();
            fnNewXform_SearchPersons();
        } catch (Error) { }
    }
});
$("#btnSaveXform_SearchPersons").on('click', function (e) {
    $("#BodyLoader").jqxLoader('open');
    fnSaveXform_SearchPersons();
    WorkData.Persons_Data.Current.Id = null;
    $("#divSelectedTxt").html('');

});
// Search logic End -------------------------------------------------------------


WorkData.Persons_Data = {
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
    First: true,
    TempFormValue: {}
};
var Persons_columns = [
//{ text: '§Id_Persons§', datafield: 'Id_Persons' },
{ text: '§Bid_Persons§', datafield: 'Bid_Persons' },
{ text: '§Usedname§', datafield: 'Usedname' },
//{ text: '§Email§', datafield: 'Email' },
//{ text: '§Birthfirstname§', datafield: 'Birthfirstname' },
//{ text: '§Birthlastname§', datafield: 'Birthlastname' },
{ text: '§Birthdate§', datafield: 'Birthdate' },
{ text: '§Placeofbirth§', datafield: 'Placeofbirth' },
{ text: '§Motherfirstname§', datafield: 'Motherfirstname' },
{ text: '§Motherlastname§', datafield: 'Motherlastname' },
//{ text: '§Xmldata§', datafield: 'Xmldata' },
//{ text: '§Id_Parent§', datafield: 'Id_Parent' },
//{ text: '§Userid§', datafield: 'Userid' }
];
var Persons_datafields = [
  { name: 'Id_Persons', type: 'decimal' },
  { name: 'Bid_Persons', type: 'string' },
  { name: 'Usedname', type: 'string' },
  { name: 'Email', type: 'string' },
  { name: 'Birthfirstname', type: 'string' },
  { name: 'Birthlastname', type: 'string' },
  { name: 'Birthdate', type: 'DateTime' },
  { name: 'Placeofbirth', type: 'string' },
  { name: 'Motherfirstname', type: 'string' },
  { name: 'Motherlastname', type: 'string' },
  { name: 'Xmldata', type: 'string' },
  { name: 'Id_Parent', type: 'decimal' },
  { name: 'Userid', type: 'string' }
];
// Urls constans
var GetXform_Persons = '/GetXform_Persons';
var InsertXform_Persons = '/InsertXform_Persons';
var SaveXform_Persons = '/SaveXform_Persons';
var DeleteXform_Persons = '/DeleteXform_Persons';
var GetRecords_PersonsId = '/GetRecords_Persons';
var GetRecords_Persons = '/GetRecords_Persons_Session';
var GetHierarchy_Persons = '/GetHierarchy_Persons';
var MoveHierarchyItem_Persons = '/MoveHierarchyItem_Persons';

function fnSetXform_Persons(Data, isView) {
    $("#ModalLoaderStart").jqxLoader('close');
    $("#divXformHtml_Persons").html(Data.Xform);
    fnGetPicture(WorkData.Persons_Data.Current.Id, 200, 200);
    //if (isView) $("#divXformHtmlView_Persons").html(Data.XformView); //fnSetCommonView("nnn", Data.XformView, "text-primary");

    $("#frmXform_Persons .editor").jqte();
    JqwTransformWithRole('frmXform_Persons');
    try {
        $('#frmXform_Persons').jqxValidator({
            onSuccess: function () {
                WorkData.Persons_Data.Valid = true;
            },
            onError: function () {
                WorkData.Persons_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Personsrules)),
            hintType: "label"
        });
    } catch (err) { }

};
function fnInsertXform_Persons() {
    $("#frmXform_Persons").jqxValidator('validate');
    if (WorkData.Persons_Data.Current.Id_Parent != null && WorkData.Persons_Data.Valid == true) {
        $("#frmXform_Persons").mYPostFormNew(urls(InsertXform_Persons), {
            Recordtype: "recordtype"
        }, function (Data) {
            WorkData.Persons_Data.Current.Id = Data.Entity.Id_Persons;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {
                DXSetState('Uploadmode');
                $("#btnSwitchUpload_Person").prop('disabled', false);
                //$("#divCreateOrEditModal_Persons").modal('hide');

                AjaxGet(urls(GetRecords_PersonsId), {
                    Id: WorkData.Persons_Data.Current.Id,
                    recordtype: ''
                }, function (Data) {
                    $("#divGrid_Persons").DXjqxGridReload({ opt: '' });
                });

            }
        });
    } else {
        GlobError("InsertXform Persons ValidateError!", 10);
    }
};
function fnSaveXform_Persons() {
    try {
        $("#frmXform_Persons").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Persons_Data.Valid) {
        $("#frmXform_Persons").mYPostFormNew(urls(SaveXform_Persons), {
            Id_Entity: WorkData.Persons_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {

                //  $("#divCreateOrEditModal_Persons").modal('hide');
                //Uploadmode
                AjaxGet(urls(GetRecords_PersonsId), {
                    Id: WorkData.Persons_Data.Current.Id,
                    recordtype: ''
                }, function (Data) {
                    $("#divGrid_Persons").DXjqxGridReload({ opt: '' });
                });

                DXSetState('Uploadmode');
            }
        });
    } else {
        GlobError("SaveXform Persons Error!", 10);
    }
};
function fnDeleteXform_Persons() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Persons), {
                Id_Persons: WorkData.Persons_Data.Current.Id,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
                } else {
                    //$('#divTree_Persons').DXjqxTreeReload({});
                    //$("#divCreateOrEditModal_Persons").modal('hide');
                }
            });
        } else { }
    });
};
function fnGetXform_Persons() {

    WorkData.Persons_Data.Current.ChangedName = WorkData.Persons_Data.Current.Name;
    AjaxGet(urls(GetXform_Persons), {
        Id_Entity: WorkData.Persons_Data.Current.Id,
        Id_Xform: 'frmXform_Persons'
    }, function (Data) {
        fnSetXform_Persons(Data, true);
    });
};
function fnNewXform_Persons() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Persons_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Persons'
    }, function (Data) {

        fnSetXform_Persons(Data, true);
        $.each(WorkData.Persons_Data.TempFormValue, function (key, value) {
            $("#frmXform_Persons input[id*='" + key + "']").val(value);
        });
        WorkData.Persons_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Persons() {
    $("#frmXform_Persons").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Persons',
        NewXformRoot: WorkData.Persons_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Persons(Data);
    });
};

function fnBind_cmbXformRootSelect_CangeEvent_Persons() {
    $('#cmbXformRootSelect_Persons').off('change');
    $('#cmbXformRootSelect_Persons').on('change', function (event) {
        if (event.args) {
            var item = event.args.item;
            WorkData.Persons_Data.Current.Root = item.value;
            fnXformChangeRoot_Persons();
        }
    });
};
$("#cmbXformRootSelect_Persons").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'PersonRoots',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {
            WorkData.Persons_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_Persons();
            fnNewXform_Persons();
        } catch (Error) { }
    }
});




// Buttons
function fnBind_btnSaveXform_Persons(process_function) {
    $("#btnSaveXform_Persons").off('click');
    $("#btnSaveXform_Persons").on('click', function (e) {
        process_function();
    });
    $("#btnSwitchUpload_Person").off('click');
    $("#btnSwitchUpload_Person").on('click', function (e) {
        //$("#divCreateOrEditModal_Persons").modal('hide');
        DXSetState('Uploadmode');
    });

    $("#btnSwitchData_Person").off('click');
    $("#btnSwitchData_Person").on('click', function (e) {
        //$("#divCreateOrEditModal_Persons").modal('hide');
        DXSetState('Insertmode');
    });

};
function fnBindDefButtonClick_Persons() {
    $("#btnNewXform_Persons").on('click', function (e) {
        if (1 == 1) {
            fnNewXform_Persons();
            fnBind_btnSaveXform_Persons(fnInsertXform_Persons);
            DXSetState('Insertmode');
            $("#btnSwitchUpload_Person").prop('disabled', true);
            $("#divCreateOrEditModal_Persons").modal('show');
        } else {
            GlobError("btnNewXform_Persons Error", 11);
        }
    });
    $("#btnEditXform_Persons").on('click', function (e) {
        if (WorkData.Persons_Data.Current.Id > 0) {
            $("#ModalLoaderStart").jqxLoader('open');


            fnGetXform_Persons();
            fnBind_btnSaveXform_Persons(fnSaveXform_Persons);
            DXSetState('Insertmode');
            $("#btnSwitchUpload_Person").prop('disabled', false);
            $("#divCreateOrEditModal_Persons").modal('show');


        } else {
            GlobError('No selected!', 11);
        }
    });
    $("#btnDeleteXform_Persons").on('click', function (e) {
        if (WorkData.Persons_Data.Current.Id > 0) {
            fnDeleteXform_Persons();
        } else {
            GlobError('No selected!', 11);
        }

    });
};
fnBindDefButtonClick_Persons();

// Grid
var initrowdetails_Persons = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        var temstr = '<div class="row small" style="background-color:#f9f9f9;">';
        temstr += '<div class="col-md-2" >';
        temstr += '<p>§P_PHOTO§</p><p><img height="100" src="' + pageVariable.baseSiteURL + 'Ajax/GetPersonPic/' + datarecord.Id_Persons + '?version=' + Math.random() + '" /></p>';
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += rdRender('§Bid_Persons§', datarecord.Bid_Persons, 30);
        temstr += rdRender('§Usedname§', datarecord.Usedname, 30);
        temstr += rdRender('§Firstname§', datarecord.Birthfirstname, 30);
        temstr += rdRender('§Lastname§', datarecord.Birthlastname, 30);
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += rdRender('§Mothersname§', datarecord.Motherfirstname + '' + datarecord.Motherlastname, 40);
        temstr += rdRender('§Birthdate§', datarecord.Birthdate, 40);
        temstr += rdRender('§Placeofbirth§', datarecord.Placeofbirth, 40);
        temstr += '</div>';
        var container = $(temstr);
        $(details).append(container);
    }
};
function fnloadCompleteGrid_Persons(data) {
    if (data.length > 0) {
    } else {
        if (WorkData.Persons_Data.First) {
            WorkData.Persons_Data.First = false;
        } else {
            GlobError('No result!', 11);
        }
    }
};
function rdRender(title, value, percent) {
    var retval = '<div class="d-flex">';
    retval += '<div class="p-2 font-italic" style="width:' + percent + '%;">' + title + ':</div>';
    retval += '<div class="p-2 font-weight-bold">' + value + '</div>';
    retval += '</div>';
    return retval;
};
var JqxParam_Grid_Persons = {
    url: urls(GetRecords_Persons),
    datafields: Persons_datafields,
    ajax_data: { id: WorkData.Persons_Data.Current.Id, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Persons,
    param: {
        columns: Persons_columns,
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
        rowdetailstemplate: { rowdetails: '<div style="margin:5px;"></div>', rowdetailsheight: 200 },
        initrowdetails: initrowdetails_Persons,
        rendertoolbar: function (toolbar) {
            var container = '<div class="btn-group btn-group-xs" role="group" aria-label="...">';
            container += '<button type="button" id="btnNewXform_Persons" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon glyphicon-plus pull-left"></i> New_Persons</button>';
            container += '<button type="button" id="btnEditXform_Persons" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon-hand-right pull-left"></i> Edit_Persons</button>';
            container += '<button type="button" id="btnDeleteXform_Persons" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> Delete_Persons</button>';
            container += '</div>';
            toolbar.append(container);
            fnGridToolbarReady_Persons();
        },
        altrows: true,
        ready: function () {
            AjaxGet(urls(GetRecords_PersonsId), {
                Id: WorkData.Persons_Data.Current.Id,
                recordtype: 'first'
            }, function (Data) {
                $("#divGrid_Persons").DXjqxGridReload({ opt: '' });
            });

        }
    }
};
$("#divGrid_Persons").DXjqxGrid(JqxParam_Grid_Persons);

$('#divGrid_Persons').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Persons_Data.Current.Id = args.row.Id_Persons;
    //    fnGetXform_Persons();
    //    fnBind_btnSaveXform_Persons(fnSaveXform_Persons);
});
$('#divGrid_Persons').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Persons_Data.Current.Id = args.row.bounddata.Id_Persons;
    $("#divSelectedTxt").html('' + args.row.bounddata.Usedname + '');
    //fnGetXform_Persons();
});
$('#divGrid_Persons').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Persons_Data.Current.Id = args.row.Id_Persons;
    //$("#SelectedPersonsTitle").html('Selected: ' + args.row.Usedname);	
});
function fnGridToolbarReady_Persons() {
    //  fnBindDefButtonClick_Persons();
};



$("#divColapseNewPersons").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: true },
    Uploadmode: { visible: false }
});
$("#divCollapsePersonsPicture").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: false },
    Uploadmode: { visible: true }
});

function fnUploadComplette() {
    var a = tesGrid;
    //tesGrid.DataAdapter.dataBind();
    AjaxGet(urls(GetRecords_PersonsId), {
        Id: WorkData.Persons_Data.Current.Id,
        recordtype: ''
    }, function (Data) {
        $("#divGrid_Persons").DXjqxGridReload({ opt: '' });
        $("#divCreateOrEditModal_Persons").modal('hide');
    });

};
include('~/Views/Base/NUpload.js' | args(0, 0));



$("#IdBodyStart").hide();
$("#IdBody").show();
