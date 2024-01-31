var PageAsyncvar = "01";
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

//pageVariable.Jqwtheme = 'orange';
//pageVariable.JqwthemeAlt = 'orange';
//pageVariable.JqwthemeMenu = 'orange';


//$("#ModalLoaderStart").jqxLoader({ text: "", width: 100, height: 60, imagePosition: 'center', theme: pageVariable.Jqwtheme });
//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Animal' + url;
    }
};


$("#btnPropertyes_Animal").on('click', function (event) {
    event.preventDefault();
    window.location.href = pageVariable.baseSiteURL + 'Animal/Propertyes'+ "?id_animal=" + WorkData.Animals_Data.Current.Id;

});


// Search logic Begin -----------------------------------------------------------
WorkData.SearchAnimals_Data = {
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
var SaveXform_SearchAnimals = '/SaveXform_SearchAnimals';
function fnSetXform_SearchAnimals(Data, isView) {
    $("#divXformHtml_SearchAnimals").html(Data.Xform);
    if (isView) $("#divXformHtmlView_SearchAnimals").html(Data.XformView);
    $("#frmXform_SearchAnimals .editor").jqte();
    JqwTransformWithRole('frmXform_SearchAnimals');
    try {
        $('#frmXform_SearchAnimals').jqxValidator({
            onSuccess: function () { WorkData.SearchAnimals_Data.Valid = true; },
            onError: function () { WorkData.SearchAnimals_Data.Valid = false; },
            rules: JSON.parse(JSON.stringify(frmXform_SearchAnimalsrules)), hintType: "label"
        });
    } catch (err) { }
};
function fnSaveXform_SearchAnimals() {
    try { $("#frmXform_SearchAnimals").jqxValidator('validate'); } catch (err) { }
    if (WorkData.SearchAnimals_Data.Valid) {
        $("#frmXform_SearchAnimals").mYPostFormNew(urls(SaveXform_SearchAnimals), {
            Recordtype: 'Entity', selector: 'Aschema',
            selecdate: WorkData.SearchAnimals_Data.Current.selectdate
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedSearchAnimalsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Searchperson').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Searchperson").modal('hide');
                //DXSetState('Uploadmode');
                //$("#SelectedAnimalsTitle").html('§Selected§:');
                $("#divGrid_Animals").DXjqxGridReload({ opt: '' });
                $("#BodyLoader").jqxLoader('close');
            }
        });
    } else {
        GlobError("SaveXform SearchAnimals Error!", 10);
    }
};
function fnNewXform_SearchAnimals() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.SearchAnimals_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_SearchAnimals',
        selector: 'Aschema',
    }, function (Data) {

        fnSetXform_SearchAnimals(Data, false);
    });
};
function fnXformChangeRoot_SearchAnimals() {
    $("#frmXform_SearchAnimals").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_SearchAnimals',
        NewXformRoot: WorkData.SearchAnimals_Data.Current.Root,
        NewXformRefRoot: '', selector: 'Aschema',
    }, function (Data) {
        fnSetXform_SearchAnimals(Data);
    });
};
function fnBind_btnSaveXform_SearchAnimals(process_function) {
    $("#btnSaveXform_SearchAnimals").off('click');
    $("#btnSaveXform_SearchAnimals").on('click', function (e) {
        process_function();
    });
};
function fnBind_cmbXformRootSelect_CangeEvent_SearchAnimals() {
    $('#cmbXformRootSelect_SearchAnimals').off('change');
    $('#cmbXformRootSelect_SearchAnimals').on('change', function (event) {
        if (event.args) {

            var item = event.args.item;
            WorkData.SearchAnimals_Data.Current.Root = item.value;
            fnXformChangeRoot_SearchAnimals();
        }
    });
};
$("#cmbXformRootSelect_SearchAnimals").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'AnimalFilter', selector: 'Aschema',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {

            WorkData.SearchAnimals_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_SearchAnimals();
            fnNewXform_SearchAnimals();
        } catch (Error) { }
    }
});
$("#btnSaveXform_SearchAnimals").on('click', function (e) {
    $("#BodyLoader").jqxLoader('open');
    fnSaveXform_SearchAnimals();
    WorkData.Animals_Data.Current.Id = null;
    $("#divSelectedTxt").html('');

});
// Search logic End -------------------------------------------------------------


WorkData.Animals_Data = {
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
var Animals_columns = [
//{ text: '§Id_Animal§', datafield: 'Id_Animal' },
{ text: '§Enar§', datafield: 'Enar' },
{ text: '§Birthdate§', datafield: 'Birthdate' },
{ text: '§Name§', datafield: 'Name' },
{ text: '§Root§', datafield: 'Root' },
//{ text: '§Recordtype§', datafield: 'Recordtype' },
//{ text: '§Xmldata§', datafield: 'Xmldata' },
//{ text: '§Id_Parent_Org§', datafield: 'Id_Parent_Org' },
{ text: '§Gender§', datafield: 'Gender' },
{ text: '§Placeofbirth§', datafield: 'Placeofbirth' },
//{ text: '§Enar_Parent_Male§', datafield: 'Enar_Parent_Male' },
//{ text: '§Enar_Parent_Female§', datafield: 'Enar_Parent_Female' },
//{ text: '§Item§', datafield: 'Item' }
];
var Animals_datafields = [
  { name: 'Id_Animal', type: 'decimal' },
  { name: 'Enar', type: 'string' },
  { name: 'Birthdate', type: 'DateTime' },
  { name: 'Name', type: 'string' },
  { name: 'Root', type: 'string' },
  { name: 'Recordtype', type: 'string' },
  { name: 'Xmldata', type: 'string' },
  { name: 'Id_Parent_Org', type: 'decimal' },
  { name: 'Gender', type: 'string' },
  { name: 'Placeofbirth', type: 'string' },
  { name: 'Enar_Parentmale', type: 'string' },
  { name: 'Enar_Parentfemale', type: 'string' },
  { name: 'Item', type: 'string' }
];
// Urls constans
var GetXform_Animals = '/GetXform_Animals';
var InsertXform_Animals = '/InsertXform_Animals';
var SaveXform_Animals = '/SaveXform_Animals';
var DeleteXform_Animals = '/DeleteXform_Animals';
var GetRecords_AnimalsId = '/GetRecords_Animals';
var GetRecords_Animals = '/GetRecords_Animals_Session';
var GetHierarchy_Animals = '/GetHierarchy_Animals';
var MoveHierarchyItem_Animals = '/MoveHierarchyItem_Animals';


function fnSetXform_Animals(Data, isView) {
    $("#ModalLoaderStart").jqxLoader('close');
    $("#divXformHtml_Animals").html(Data.Xform);
    fnGetPicture(WorkData.Animals_Data.Current.Id, 200, 200);
    //if (isView) $("#divXformHtmlView_Animals").html(Data.XformView); //fnSetCommonView("nnn", Data.XformView, "text-primary");

    $("#frmXform_Animals .editor").jqte();
    JqwTransformWithRole('frmXform_Animals');
    try {
        $('#frmXform_Animals').jqxValidator({
            onSuccess: function () {
                WorkData.Animals_Data.Valid = true;
            },
            onError: function () {
                WorkData.Animals_Data.Valid = false;
            },
            rules: JSON.parse(JSON.stringify(frmXform_Animalsrules)),
            hintType: "label"
        });
    } catch (err) { }

};
function fnInsertXform_Animals() {
    $("#frmXform_Animals").jqxValidator('validate');
    if (WorkData.Animals_Data.Current.Id_Parent != null && WorkData.Animals_Data.Valid == true) {
        $("#frmXform_Animals").mYPostFormNew(urls(InsertXform_Animals), {
            Recordtype: "recordtype"
        }, function (Data) {
            WorkData.Animals_Data.Current.Id = Data.Entity.Id_Animal;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedAnimalsTitle"), Data.Error.Errormessage);
            } else {
                DXSetState('Uploadmode');
                $("#btnSwitchUpload_Animals").prop('disabled', false);
                //$("#divCreateOrEditModal_Animals").modal('hide');

                AjaxGet(urls(GetRecords_AnimalsId), {
                    Id: WorkData.Animals_Data.Current.Id,
                    recordtype: ''
                }, function (Data) {
                    $("#divGrid_Animals").DXjqxGridReload({ opt: '' });
                });

            }
        });
    } else {
        GlobError("InsertXform Animals ValidateError!", 10);
    }
};
function fnSaveXform_Animals() {
    try {
        $("#frmXform_Animals").jqxValidator('validate');
    } catch (err) { }
    if (WorkData.Animals_Data.Valid) {
        $("#frmXform_Animals").mYPostFormNew(urls(SaveXform_Animals), {
            Id_Entity: WorkData.Animals_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedAnimalsTitle"), Data.Error.Errormessage);
            } else {

                //  $("#divCreateOrEditModal_Animals").modal('hide');
                //Uploadmode
                AjaxGet(urls(GetRecords_AnimalsId), {
                    Id: WorkData.Animals_Data.Current.Id,
                    recordtype: ''
                }, function (Data) {
                    
                });

                DXSetState('Uploadmode');
            }
        });
    } else {
        GlobError("SaveXform Animals Error!", 10);
    }
};
function fnDeleteXform_Animals() {
    dlgAreYouSoure(function (confirm) {
        if (confirm) {
            var cmd = '';
            if (confirm == 'close')
                cmd = 'close';
            AjaxGet(urls(DeleteXform_Animals), {
                Id_Entity: WorkData.Animals_Data.Current.Id,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    Titlemess($("#SelectedAnimalsTitle"), Data.Error.Errormessage);
                } else {
                    fnSaveXform_SearchAnimals();

                    //$('#divTree_Animals').DXjqxTreeReload({});
                    //$("#divCreateOrEditModal_Animals").modal('hide');
                }
            });
        } else { }
    });
};
function fnGetXform_Animals() {

    WorkData.Animals_Data.Current.ChangedName = WorkData.Animals_Data.Current.Name;
    AjaxGet(urls(GetXform_Animals), {
        Id_Entity: WorkData.Animals_Data.Current.Id,
        Id_Xform: 'frmXform_Animals'
    }, function (Data) {
        fnSetXform_Animals(Data, true);
    });
};
function fnNewXform_Animals() {
    AjaxGet(urls(Xform_NewXformRoot_Url), {
        root: WorkData.Animals_Data.Current.Root,
        refroot: '',
        formid: 'frmXform_Animals'
    }, function (Data) {

        fnSetXform_Animals(Data, true);
        $.each(WorkData.Animals_Data.TempFormValue, function (key, value) {
            $("#frmXform_Animals input[id*='" + key + "']").val(value);
        });
        WorkData.Animals_Data.TempFormValue = {};
    });
};
function fnXformChangeRoot_Animals() {
    $("#frmXform_Animals").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Animals',
        NewXformRoot: WorkData.Animals_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Animals(Data);
    });
};

function fnBind_cmbXformRootSelect_CangeEvent_Animals() {
    $('#cmbXformRootSelect_Animals').off('change');
    $('#cmbXformRootSelect_Animals').on('change', function (event) {
        if (event.args) {
            var item = event.args.item;
            WorkData.Animals_Data.Current.Root = item.value;
            fnXformChangeRoot_Animals();
        }
    });
    PageAsync("0");
};
$("#cmbXformRootSelect_Animals").DXjqxRootsCombo({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'AnimalType',
    param: {
        theme: pageVariable.Jqwtheme,
        selectedIndex: 0,
        height: 25,
        width: 270
    },
    ComplettCallback: function (c) {
        try {

            WorkData.Animals_Data.Current.Root = c.Res[0].value;
            fnBind_cmbXformRootSelect_CangeEvent_Animals();
          //  fnNewXform_Animals();
        } catch (Error) { }
    }
});


// Buttons
function fnBind_btnSaveXform_Animals(process_function) {
    $("#btnSaveXform_Animals").off('click');
    $("#btnSaveXform_Animals").on('click', function (e) {
        process_function();
    });
    $("#btnSwitchUpload_Animals").off('click');
    $("#btnSwitchUpload_Animals").on('click', function (e) {
        //$("#divCreateOrEditModal_Animals").modal('hide');
        DXSetState('Uploadmode');
    });

    $("#btnSwitchData_Animals").off('click');
    $("#btnSwitchData_Animals").on('click', function (e) {
        //$("#divCreateOrEditModal_Animals").modal('hide');
        DXSetState('Insertmode');
    });

};
function fnBindDefButtonClick_Animals() {
    $("#btnNewXform_Animals").on('click', function (e) {
        if (1 == 1) {
            fnNewXform_Animals();
            fnBind_btnSaveXform_Animals(fnInsertXform_Animals);
            DXSetState('Insertmode');
            $("#btnSwitchUpload_Animals").prop('disabled', true);
            $("#divCreateOrEditModal_Animals").modal('show');
        } else {
            GlobError("btnNewXform_Animals Error", 11);
        }
    });
    $("#btnEditXform_Animals").on('click', function (e) {
        if (WorkData.Animals_Data.Current.Id > 0) {
            $("#ModalLoaderStart").jqxLoader('open');


            fnGetXform_Animals();
            fnBind_btnSaveXform_Animals(fnSaveXform_Animals);
            DXSetState('Insertmode');
            $("#btnSwitchUpload_Animals").prop('disabled', false);
            $("#divCreateOrEditModal_Animals").modal('show');


        } else {
            GlobError('No selected!', 11);
        }
    });
    $("#btnDeleteXform_Animals").on('click', function (e) {
        if (WorkData.Animals_Data.Current.Id > 0) {
            fnDeleteXform_Animals();
        } else {
            GlobError('No selected!', 11);
        }

    });
};
fnBindDefButtonClick_Animals();

// Grid
var initrowdetails_Animals = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        var temstr = '<div class="row small" style="background-color:#f9f9f9;">';
        temstr += '<div class="col-md-2" >';
        temstr += '<p>§P_PHOTO§</p><p><img height="100" src="' + pageVariable.baseSiteURL + 'Ajax/GetAnimalPic/' + datarecord.Id_Animal + '?version=' + Math.random() + '" /></p>';
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += rdRender('§Enar§', datarecord.Enar, 30);
        temstr += rdRender('§NAME§', datarecord.Name, 30);
        temstr += rdRender('§Gender§', datarecord.Gender, 30);
        temstr += rdRender('§Enar_Parent_Female§', datarecord.Enar_Parentfemale, 30);
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += rdRender('§Enar_Parent_Male§', datarecord.Enar_Parentmale + '' + '', 40);
        temstr += rdRender('§Birthdate§', datarecord.Birthdate, 40);
        temstr += rdRender('§Placeofbirth§', datarecord.Placeofbirth, 40);
        temstr += '</div>';
        var container = $(temstr);
        $(details).append(container);
    }
};
function fnloadCompleteGrid_Animals(data) {
    if (data.length > 0) {
    } else {
        if (WorkData.Animals_Data.First) {
            WorkData.Animals_Data.First = false;
        } else {
            GlobError('No result!', 11);
        }
    }
    PageAsync("1");


   
};
function rdRender(title, value, percent) {
    var retval = '<div class="d-flex">';
    retval += '<div class="p-2 font-italic" style="width:' + percent + '%;">' + title + ':</div>';
    retval += '<div class="p-2 font-weight-bold">' + value + '</div>';
    retval += '</div>';
    return retval;
};
var JqxParam_Grid_Animals = {
    url: urls(GetRecords_Animals),
    datafields: Animals_datafields,
    async: true,
    ajax_data: { id: WorkData.Animals_Data.Current.Id, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Animals,
    param: {
        columns: Animals_columns,
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
        initrowdetails: initrowdetails_Animals,
        rendertoolbar: function (toolbar) {
            var container = '<div class="btn-group btn-group-xs" role="group" aria-label="...">';
            container += '<button type="button" id="btnNewXform_Animals" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon glyphicon-plus pull-left"></i> New_Animals</button>';
            container += '<button type="button" id="btnEditXform_Animals" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon-hand-right pull-left"></i> Edit_Animals</button>';
            container += '<button type="button" id="btnDeleteXform_Animals" class="btn btn-outline-info btn-xs"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> Delete_Animals</button>';
            container += '</div>';
            toolbar.append(container);
            fnGridToolbarReady_Animals();
        },
        altrows: true,
        ready: function () {


        }
    }
};

AjaxGet(urls(GetRecords_AnimalsId), {
    Id: WorkData.Animals_Data.Current.Id,
    recordtype: 'first'
}, function (Data) {
    //$("#divGrid_Animals").DXjqxGridReload({ opt: '' });
    $("#divGrid_Animals").DXjqxGrid(JqxParam_Grid_Animals);
});



$('#divGrid_Animals').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Animals_Data.Current.Id = args.row.Id_Animal;
    //    fnGetXform_Animals();
    //    fnBind_btnSaveXform_Animals(fnSaveXform_Animals);
});
$('#divGrid_Animals').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Animals_Data.Current.Id = args.row.bounddata.Id_Animal;
    $("#divSelectedTxt").html('' + args.row.bounddata.Name + '');
    $("#Temp_selectedAnimal").val(args.row.Id_Animal);
    //fnGetXform_Animals();
});
$('#divGrid_Animals').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Animals_Data.Current.Id = args.row.Id_Animal;
    $("#Temp_selectedAnimal").val(args.row.Id_Animal);
    //$("#SelectedAnimalsTitle").html('Selected: ' + args.row.Usedname);	
});
function fnGridToolbarReady_Animals() {
    //  fnBindDefButtonClick_Animals();
};



$("#divColapseNewAnimals").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: true },
    Uploadmode: { visible: false }
});
$("#divCollapseAnimalsPicture").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: false },
    Uploadmode: { visible: true }
});

function fnUploadComplette() {
   // fnSaveXform_SearchAnimals();
    //  $("#divCreateOrEditModal_Animals").modal('hide');

    AjaxGet(urls(GetRecords_AnimalsId), {
        Id: WorkData.Animals_Data.Current.Id,
        recordtype: ''
    }, function (Data) {
        $("#divGrid_Animals").DXjqxGridReload({ opt: '' });
        $("#divCreateOrEditModal_Animals").modal('hide');
    });


};
include('~/Views/Base/NUploadAnimal.js' | args(0, 0));
// A vegere egyszer kell!
//$("#IdBodyStart").hide();
//$("#IdBody").show();




function PageAsync(v) {

    PageAsyncvar = PageAsyncvar.replace(new RegExp(v, 'g'), '');
    if (PageAsyncvar.length == 0) {
        LoadComplettCallback();
    }

};
