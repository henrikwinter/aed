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
$("#ModalLoaderStart").jqxLoader({ text: "", width: 100, height: 60, imagePosition: 'center', theme: pageVariable.Jqwtheme });


var XCt = {
    baseSiteURL: pageVariable.baseSiteURL,
    newControler: 'Ajax',
    workController: 'Persons'
};
var Id = 2;
var Selectdate = null;

// --- OrgChanged dates
var sourceValidDates = {
    datatype: "json",
    datafields: [{
        name: 'Datavalidfrom'
    }
    ],
    url: XCt.baseSiteURL + XCt.workController + '/GetValidDates',
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
            Selectdate = test;

        }
    }
});


$("#frmXform_SearchPersons").dxXform({
    context: XCt,
    name: 'SearchPersons',
    selector: 'Aschema',
    SaveComplet: function () {
        $("#divGrid_Persons").DXjqxGridReload({
            opt: ''
        });
        $("#BodyLoader").jqxLoader('close');
    },
    InValid: function () {

    }
});
$("#cmbXformRootSelect_SearchPersons").dxRootcombo({
    context: XCt,
    selector: 'Aschema',
    filter: 'PersonsearchFilter',
    CreateComplet: function (Root) {
        $("#frmXform_SearchPersons").dxXform('New', Root);
    },
    onChange: function (Root) {
        $("#frmXform_SearchPersons").dxXform('Changeroot', {
            root: Root,
            selector: 'Aschema'
        });
    }
});
$("#btnSaveXform_SearchPersons").on('click', function (e) {
    $("#BodyLoader").jqxLoader('open');
    $("#frmXform_SearchPersons").dxXform('Save', {
        arg: {
            selector: 'Aschema'
        }
    });
});
var Persons_columns = [{
    text: '§Bid_Persons§',
    datafield: 'Bid_Persons',
    width: 100
}, {
    text: '§Usedname§',
    datafield: 'Usedname'
}, {
    text: '§Birthdate§',
    datafield: 'Birthdate'
}, {
    text: '§Placeofbirth§',
    datafield: 'Placeofbirth'
}, {
    text: '§Motherfirstname§',
    datafield: 'Motherfirstname'
}, {
    text: '§Motherlastname§',
    datafield: 'Motherlastname'
}, ];
var Persons_datafields = [{
    name: 'Id_Persons',
    type: 'decimal'
}, {
    name: 'Bid_Persons',
    type: 'string'
}, {
    name: 'Usedname',
    type: 'string'
}, {
    name: 'Email',
    type: 'string'
}, {
    name: 'Birthfirstname',
    type: 'string'
}, {
    name: 'Birthlastname',
    type: 'string'
}, {
    name: 'Birthdate',
    type: 'DateTime'
}, {
    name: 'Placeofbirth',
    type: 'string'
}, {
    name: 'Motherfirstname',
    type: 'string'
}, {
    name: 'Motherlastname',
    type: 'string'
}, {
    name: 'Xmldata',
    type: 'string'
}, {
    name: 'Id_Parent',
    type: 'decimal'
}, {
    name: 'Userid',
    type: 'string'
}
];
function rdRender(title, value, percent) {
    var retval = '<div class="d-flex">';
    retval += '<div class="p-2 font-italic" style="width:' + percent + '%;">' + title + ':</div>';
    retval += '<div class="p-2 font-weight-bold">' + value + '</div>';
    retval += '</div>';
    return retval;
};
function PreloadGrid() {
    AjaxGet(XCt.baseSiteURL + XCt.workController + '/GetRecords_Persons', {
        Id: Id,
        recordtype: 'first'
    }, function (Data) {
        $("#divGrid_Persons").DXjqxGridReload({
            opt: ''
        });
    });
}
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
var JqxParam_Grid_Persons = {
    url: XCt.baseSiteURL + XCt.workController + '/GetRecords_Persons_Session',
    datafields: Persons_datafields,
    ajax_data: {
        opt: ''
    },
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
        rowdetails: true,
        rowdetailstemplate: {
            rowdetails: '<div style="margin:5px;"></div>',
            rowdetailsheight: 200
        },
        initrowdetails: initrowdetails_Persons,
        altrows: true,
        ready: function () {
            PreloadGrid();
        }
    }
};
$("#divGrid_Persons").DXjqxGrid(JqxParam_Grid_Persons);
$('#divGrid_Persons').on('rowclick', function (event) {
    var args = event.args;
    Id = args.row.bounddata.Id_Persons;
    $("#divSelectedTxt").html('' + args.row.bounddata.Usedname + '');
});
$("#cmbXformRootSelect_Persons").dxRootcombo({
    context: XCt,
    filter: 'PersonRoots',
    CreateComplet: function (Root) { },
    onChange: function (Root) {
        $("#frmXform_Persons").dxXform('Changeroot', {
            root: Root
        });
    }
});
$("#frmXform_Persons").dxXform({
    context: XCt,
    name: 'Persons',
    dlgconfirm: dlgAreYouSoure,
    GetComplet: function () {
        DXSetState('Insertmode');
        $("#ModalLoaderStart").jqxLoader('close');
        $("#btnSwitchUpload_Person").prop('disabled', false);
        $("#divCreateOrEditModal_Persons").modal('show');
        $("#btnSaveXform_Persons").dxButton('Setclickporc', function () {
            $("#frmXform_Persons").dxXform('Save', {
                arg: {
                    Id_Entity: Id
                }
            });
        });
    },
    SaveComplet: function () {
        $("#ModalLoaderStart").jqxLoader('close');
        $("#divCreateOrEditModal_Persons").modal('hide');
        PreloadGrid();
    },
    NewComplet: function () {
        $("#ModalLoaderStart").jqxLoader('close');
        DXSetState('Insertmode');
        $("#btnSwitchUpload_Person").prop('disabled', true);
        $("#divCreateOrEditModal_Persons").modal('show');
        $("#btnSaveXform_Persons").dxButton('Setclickporc', function () {
            $("#frmXform_Persons").dxXform('Insert', {
                arg: {
                    Id_Parent: 2,
                    Recordtype: 'Proba'
                }
            });
        });
    },
    InsertComplet: function () {
        $("#ModalLoaderStart").jqxLoader('close');
        $("#divCreateOrEditModal_Persons").modal('hide');
        PreloadGrid();
    },
    DeleteComplet: function () {
        $("#ModalLoaderStart").jqxLoader('close');
        PreloadGrid();
    },
    InValid: function () {
        debugger;
    }
});
$("#btnSaveXform_Persons").dxButton();
$("#btnEditXform_Persons").on('click', function (e) {
    if (Id > 0) {
        $("#ModalLoaderStart").jqxLoader('open');
        $("#frmXform_Persons").dxXform('Get', {
            id: Id
        });
    } else {
        GlobError('No selected!', 11);
    }
});
$("#btnNewXform_Persons").on('click', function (e) {
    $("#ModalLoaderStart").jqxLoader('open');
    $("#frmXform_Persons").dxXform('New', 'Person');
});
$("#btnDeleteXform_Persons").on('click', function (e) {
    $("#frmXform_Persons").dxXform('Delete', Id);
});


$("#btnSwitchUpload_Person").on('click', function (e) {
    DXSetState('Uploadmode');
});
$("#btnSwitchData_Person").on('click', function (e) {
    DXSetState('Insertmode');
});



$("#divColapseNewPersons").DXInitStateControl({
    Searchmode: {
        visible: false
    },
    SearcGridhmode: {
        visible: false
    },
    Insertmode: {
        visible: true
    },
    Uploadmode: {
        visible: false
    }
});
$("#divCollapsePersonsPicture").DXInitStateControl({
    Searchmode: {
        visible: false
    },
    SearcGridhmode: {
        visible: false
    },
    Insertmode: {
        visible: false
    },
    Uploadmode: {
        visible: true
    }
});


function fnUploadStart() {
    $("#ModalLoaderStart").jqxLoader('open');
};
function fnUploadComplette() {
    $("#ModalLoaderStart").jqxLoader('close');
    DXSetState('Insertmode');
};
include('~/Views/Base/NUpload.js' | args(1, Id));

$("#IdBodyStart").hide();
$("#IdBody").show();
