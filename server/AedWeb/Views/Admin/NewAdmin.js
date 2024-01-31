
// Search logic Begin -----------------------------------------------------------
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
    try { $("#frmXform_SearchPersons").jqxValidator('validate'); } catch (err) { }
    if (WorkData.SearchPersons_Data.Valid) {
        $("#frmXform_SearchPersons").mYPostFormNew(urls(SaveXform_SearchPersons), {
            Recordtype: 'Entity', selector: 'Aschema',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedSearchPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Searchperson').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Searchperson").modal('hide');
                //DXSetState('Uploadmode');
                $("#SelectedPersonsTitle").html('§Selected§:');
                $("#divGrid_Persons").DXjqxGridReload({ opt: '' });

            }
        });
    } else {
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
        } catch (Error) { }
    }
});
$("#btnSaveXform_SearchPersons").on('click', function (e) {
    fnSaveXform_SearchPersons();
    WorkData.Persons_Data.Current.Id = null;
});
// Search logic End -------------------------------------------------------------


// Urls constans
var GetXform_Persons = '/GetXform_Persons';
var InsertXform_Persons = '/InsertXform_Persons';
var SaveXform_Persons = '/SaveXform_Persons';
var DeleteXform_Persons = '/DeleteXform_Persons';
//var GetRecords_Persons = '/GetRecords_Persons';
var GetRecords_Persons = '/GetRecords_Persons_Session';
var GetHierarchy_Persons = '/GetHierarchy_Persons';
var MoveHierarchyItem_Persons = '/MoveHierarchyItem_Persons';
//Url builder function
function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Admin' + url;
    }
}
// --------------------------------------------------------------------------------------------
// Entity and Xform functions 
function fnSetXform_Persons(Data, isView) {
    $("#divXformHtml_Persons").html(Data.Xform);
    if (isView) $("#divXformHtmlView_Persons").html(Data.XformView);
    $("#frmXform_Persons .editor").jqte();
    JqwTransformWithRole('frmXform_Persons');
    try {
        $('#frmXform_Persons').jqxValidator({
            onSuccess: function () { WorkData.Persons_Data.Valid = true; },
            onError: function () { WorkData.Persons_Data.Valid = false; },
            rules: JSON.parse(JSON.stringify(frmXform_Personsrules)), hintType: "label"
        });
    } catch (err) { }
};
function fnInsertXform_Persons() {
    $("#frmXform_Persons").jqxValidator('validate');
    if (WorkData.Persons_Data.Current.Id_Parent != null && WorkData.Persons_Data.Valid == true) {
        $("#frmXform_Persons").mYPostFormNew(urls(InsertXform_Persons), {
            Id_Parent: WorkData.Persons_Data.Current.Id
        }, function (Data) {
            WorkData.Persons_Data.Current.Id = Data.Entity.Id_Persons;
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Persons').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Persons").modal('hide');
                DXSetState('Uploadmode');
            }
        });
    } else {
        GlobError("InsertXform Persons ValidateError!", 10);
    }
};
function fnSaveXform_Persons() {
    try { $("#frmXform_Persons").jqxValidator('validate'); } catch (err) { }
    if (WorkData.Persons_Data.Valid) {
        $("#frmXform_Persons").mYPostFormNew(urls(SaveXform_Persons), {
            Id_Entity: WorkData.Persons_Data.Current.Id,
            Recordtype: 'Entity',
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Persons').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Persons").modal('hide');

                var urli = pageVariable.baseSiteURL + "Ajax/GetPersonPic/" + WorkData.Persons_Data.Current.Id + "?version=" + Math.random();
                var imageObj = new Image();
                imageObj.onload = function () {
                    $("#views").empty();
                    $("#views").append("<canvas id=\"canvas\">");
                    UploadPic.canvas = $("#canvas")[0];
                    UploadPic.canvas.width = imageObj.width;
                    UploadPic.canvas.height = imageObj.height;
                    UploadPic.context = UploadPic.canvas.getContext("2d");
                    UploadPic.context.drawImage(imageObj, 0, 0);
                };
                imageObj.src = urli;

                DXSetState('Uploadmode');

            }
        });
    } else {
        GlobError("SaveXform Persons Error!", 10);
    }
};
function fnpostDeleteXform_Persons() {
    if (dlgAreYouSoure) {
        $("#frmXform_Persons").mYPostFormNew(urls(DeleteXform_Persons), {
            Id_Entity: WorkData.Persons_Data.Current.Id,
            Id_Flows: WorkData.model_ClientPart.Id_Flow
        }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Persons').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Persons").modal('hide');
                //DXSetState('Uploadmode');
            }
        });
    } else {
        //
    }
};
function fnDeleteXform_Persons() {
    if (dlgAreYouSoure) {
        AjaxGet(urls(DeleteXform_Persons), { Id_Entity: WorkData.Persons_Data.Current.Id }, function (Data) {
            if (Data.Error.Errorcode != 0) {
                Titlemess($("#SelectedPersonsTitle"), Data.Error.Errormessage);
            } else {
                //$('#divTree_Persons').DXjqxTreeReload({Id: 0,recordtype: ''});
                //$("#divCreateOrEditModal_Persons").modal('hide');
                //DXSetState('Uploadmode');
            }
        });
    } else {
        //
    }
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
        fnSetXform_Persons(Data, false);
    });
};
// Tree special functions
function fnJqxTreeDragStart_Persons(item) {
    return true;
};
function fnJqxTreeDragEnd_Persons(dragItem, dropItem, args, dropPosition, tree) {
    JqxTree_DragEnd(dragItem, dropItem, dropPosition, urls(MoveHierarchyItem_Persons));
};
// Html element binding functions
function fnXformChangeRoot_Persons() {
    $("#frmXform_Persons").mYPostFormNew(urls(Xform_ChangeRootPost_Url), {
        formid: 'frmXform_Persons',
        NewXformRoot: WorkData.Persons_Data.Current.Root,
        NewXformRefRoot: ''
    }, function (Data) {
        fnSetXform_Persons(Data);
    });
};
function fnBind_btnSaveXform_Persons(process_function) {
    $("#btnSaveXform_Persons").off('click');
    $("#btnSaveXform_Persons").on('click', function (e) {
        process_function();
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

// --------------------------------------------------------------------------------------------
// Html Elemet Initialize

// Root select
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
        } catch (Error) { }
    }
});


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
var Persons_columns = [
//{ text: '§Id_Persons§', datafield: 'Id_Persons' },
{ text: '§Bid_Persons§', datafield: 'Bid_Persons' },
{ text: '§Userid§', datafield: 'Userid' },
{ text: '§Usedname§', datafield: 'Usedname' },
{ text: '§Email§', datafield: 'Email' },
//{ text: '§Birthfirstname§', datafield: 'Birthfirstname' },
//{ text: '§Birthlastname§', datafield: 'Birthlastname' },
{ text: '§Birthdate§', datafield: 'Birthdate' },
{ text: '§Placeofbirth§', datafield: 'Placeofbirth' }
//{ text: '§Motherfirstname§', datafield: 'Motherfirstname' },
//{ text: '§Motherlastname§', datafield: 'Motherlastname' },
//{ text: '§Xmldata§', datafield: 'Xmldata' },
//{ text: '§Id_Parent§', datafield: 'Id_Parent' },

];
// Grid 
function fnloadCompleteGrid_Persons(data) {
    if (data.length > 0) {
        DXSetState('SearcGridhmode');
    } else {
        Titlemess($("#SelectedPersonsTitle"), "§NotFound§!");
        DXSetState('Searchmode');
    }
};
function rdRender(title, value, percent) {
    var retval = '<div class="d-flex">';
    retval += '<div class="p-2 font-italic" style="width:' + percent + '%;">' + title + ':</div>';
    retval += '<div class="p-2 font-weight-bold">' + value + '</div>';
    retval += '</div>';
    return retval;
};
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

// Grid 
var JqxParam_Grid_Persons = {
    url: urls(GetRecords_Persons),
    datafields: Persons_datafields,
    ajax_data: { id: 0, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Persons,
    param: {
        columns: Persons_columns,
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
        initrowdetails: initrowdetails_Persons,
        rendertoolbar: function (toolbar) {
            var container = '<ul class="nav">';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnNewXform_Persons"><i class="glyphicon glyphicon glyphicon-plus pull-left"></i> NewXform_Persons</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnEditXform_Persons"><i class="glyphicon glyphicon-hand-right pull-left"></i>EditXform_Persons</a></li>';
            container += '<li class="nav-item"><a class="nav-link"href="#" id="btnDeleteXform_Persons"><i class="glyphicon glyphicon glyphicon-minus pull-left"></i> DeleteXform_Persons</a></li>';
            container += '</ul>';
            toolbar.append(container);
            fnGridToolbarReady_Persons();
        },
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Persons").DXjqxGrid(JqxParam_Grid_Persons);

$('#divGrid_Persons').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Persons_Data.Current.Id = args.row.Id_Persons;
    $("#SelectedPersonsTitle").html('Selected: ' + args.row.Usedname);
});
function fnGridToolbarReady_Persons() {
    $("#btnNewXform_Persons").on('click', function (e) {
        if (1 == 1) {
            //$("#BodyLoader").jqxLoader('open');
            //$('#cmbXformRootSelect_Persons').jqxComboBox('clearSelection');

            fnNewXform_Persons();
            fnBind_btnSaveXform_Persons(fnInsertXform_Persons);

            //$("#BodyLoader").jqxLoader('close');
            //$("#divCreateOrEditModal_Persons").modal('show');
        } else {
            //
        }
    });
    $("#btnEditXform_Persons").on('click', function (e) {
        //$('#cmbXformRootSelect_Persons').jqxComboBox('clearSelection');
        //$("#BodyLoader").jqxLoader('open');

        fnGetXform_Persons();
        fnBind_btnSaveXform_Persons(fnSaveXform_Persons);

        //$("#BodyLoader").jqxLoader('close');
        //$("#divCreateOrEditModal_Persons").modal('show');
    });
    $("#btnDeleteXform_Persons").on('click', function (e) {
        var item = $('#divTree_Persons').jqxTree('getSelectedItem');
        if (item != null) {

            fnDeleteXform_Persons();

        } else {
            GlobError("DeleteXform Persons Error!", 11);
        }
    });

};

// --------------------------------------------------------------------

$("#btnSwitchSearc,#btnSwitchSearcFromPic").on('click', function () {
    DXSetState('Searchmode');
    WorkData.Persons_Data.Current.Id = null;
});
$("#btnSwitchNew").on('click', function () {
    DXSetState('Insertmode');
    fnNewXform_Persons();
    fnBind_btnSaveXform_Persons(fnInsertXform_Persons);
});
$("#btnSwitchEdit").on('click', function () {
    DXSetState('Insertmode');
    fnGetXform_Persons();
    fnBind_btnSaveXform_Persons(fnSaveXform_Persons);
});
$("#btnSwitchUpload").on('click', function () {
    $("#views").empty();
    DXSetState('Uploadmode');
});

$("#divColapseNewPersons").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: true },
    Uploadmode: { visible: false }
});
$("#divColapsePersonsGrid").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: true },
    Insertmode: { visible: false },
    Uploadmode: { visible: false }
});
$("#divCollapsePersonsPicture").DXInitStateControl({
    Searchmode: { visible: false },
    SearcGridhmode: { visible: false },
    Insertmode: { visible: false },
    Uploadmode: { visible: true }
});
$("#divColapseSearchPersons").DXInitStateControl({
    Searchmode: { visible: true },
    SearcGridhmode: { visible: true },
    Insertmode: { visible: false },
    Uploadmode: { visible: false }
});

$("#FindPersonsModal").on('shown.bs.modal', function (e) {
    DXSetState('Searchmode');
    $("#FindPersonsModalTitle").html("");
    fnNewXform_SearchPersons();
});
$("#FindPersonsModal").on('hidden.bs.modal', function () {
    DXSetState('Searchmode');

});

