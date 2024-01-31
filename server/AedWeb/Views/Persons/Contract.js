include('~/Scripts/DextraFlowManParial.js' | args(0, 0));

var CheckPostData = function () {
    WorkData.ClientPartName = 'Org';
    WorkData.model_ClientPart.retval = c_FlowPostpreCheckretOk;
    return WorkData.model_ClientPart;
};

function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Persons' + url;
    }
};
var GetRecords_Vw_Agreementproperty = '/GetRecords_Vw_Agreementproperty';
var DeleteXform_Vw_Agreementproperty = '/DeleteXform_Vw_Agreementproperty';

// Work datas
WorkData.Organization_Data = {
    Current: {
        Id_Entity: WorkData.model_ClientPart.io_SelectedOrgId,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
   // FkKey: xfindpk(Organization_datafields)
};
WorkData.Persons_Data = {
    Current: {
        Id_Entity: WorkData.model_PersonClientPart.io_SelectedPersonId,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    //FkKey: xfindpk(Persons_datafields)
};
WorkData.Agreement_Data = {
    Current: {
        Id_Entity: WorkData.model_PersonClientPart.io_AgreemetId,
        Id_Parent: 0,
        Root: '',
        Name: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    //FkKey: xfindpk(Agreement_datafields)
};


// Agrement open 
var CommonXformViewPanel = "divXformHtmlCommonView";
$("#frmXform_Agreement").Xform({
    controller: 'Persons',			// <Setting> 
    actionpart: 'Agreement',			// <Setting> 
    refroot: '',
    formid: 'frmXform_Agreement',
    viewformid: CommonXformViewPanel,
    typecombo: { Id: 'cmbXformRootSelect_Agreement', filter: 'Agreement', width:350 },
    //typemenu: { Id: 'cmbXformRootSelect_Agreement', filter: 'AgreementRoots'  },
    savebuttonid: 'btnSaveXform_Agreement',
    savebuttoncallback: function (mode) {
        if (mode == 'insert') {
            var id_parent = WorkData.Agreement_Data.Current.Id_Entity;
            return { Id_Parent: id_parent, Recordtype: "Agreement" };
        } else {
            return { Id_Entity: WorkData.Agreement_Data.Current.Id_Entity };
        }
    },
    Complete: function (e, eData) {
        if (eData.mode == 'Save' || eData.mode == 'Insert') {
            //if ($.isFunction(fnAgreement_Reload)) fnAgreement_Reload();
            //$("#divCreateOrEditModal_Agreement").modal('hide');
        } else if (eData.mode == 'Get') {
            if (eData.value.Loaded) {
                $("#" + CommonXformViewPanel + "_XformLabel").removeClass("text-primary text-success text-warning").addClass("text-primary");
                $("#" + CommonXformViewPanel + "_XformLabel").html('Agreement');
            } else {
                Titlemess($("#SelectedAgreementTitle"), "Errr");
            }
        }
        try { eval('OnComplete_' + eData.mode + '(eData);'); } catch (err) { }
    },
    Error: function (e, eData) {
        Titlemess($("#SelectedAgreementTitle"), eData.value.Error.Errormessage);
    }
});
function OnComplete_TypeCombo() {

    $("#frmXform_Agreement").Xform('get', { Id_Entity: WorkData.Agreement_Data.Current.Id_Entity });
};

function OnComplete_Create() {

    //$("#frmXform_Agreement").Xform('get', { Id_Entity: WorkData.Agreement_Data.Current.Id_Entity });
};

function OnComplete_Get() {

    LoadedComplett();
};




// Propertyes xform
function fnDeleteXform_Vw_Agreementproperty() {
    dlgAreYouSoure("dlgAreYouSoure", function (confirm) {
        if (confirm) {
            var cmd = (confirm == 'close') ? 'close' : '';
            AjaxGet(urls(DeleteXform_Vw_Agreementproperty), {
                Id_Entity: WorkData.Vw_Agreementproperty_Data.Current.Id_Entity,
                table: WorkData.Vw_Agreementproperty_Data.Current.Table,
                cmd: cmd
            }, function (Data) {
                if (Data.Error.Errorcode != 0) {
                    GlobError(Data.Error.Errormessage, 55);
                } else {
                    if ($.isFunction(fnVw_Agreementproperty_Reload)) fnVw_Agreementproperty_Reload();
                }
            });
        } else { }
    });
};
function fnVw_Agreementproperty_Reload() {
    $('#divGrid_Vw_Agreementproperty').DXjqxGridReload({ id: WorkData.Agreement_Data.Current.Id_Entity, recordtype: 'Vw_AgreementpropertyItem' });
};
$("#frmXform_Vw_Agreementproperty").Xform({
    controller: 'Persons',			// <Setting> 
    actionpart: 'Vw_Agreementproperty',			// <Setting> 
    refroot: '',
    formid: 'frmXform_Vw_Agreementproperty',
    viewformid: CommonXformViewPanel,
    //typecombo: { Id: 'cmbXformRootSelect_Vw_Agreementproperty', filter: 'SubContractType' },
    typemenu: { Id: 'cmbXformRootSelect_Vw_AgreementpropertyMenu', filter: 'SubContractType' ,width:450},
    savebuttonid: 'btnSaveXform_Vw_Agreementproperty',
    savebuttoncallback: function (mode) {
        if (mode == 'insert') {
            return { Id_Parent: WorkData.Agreement_Data.Current.Id_Entity, Recordtype: "Vw_Agreementproperty", table: this.typemenu.selected.Table, Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow };
        } else {
            return { Id_Entity: WorkData.Vw_Agreementproperty_Data.Current.Id_Entity, table: WorkData.Vw_Agreementproperty_Data.Current.Table };
        }
    },
    Complete: function (e, eData) {
        if (eData.mode == 'Save' || eData.mode == 'Insert') {
            if ($.isFunction(fnVw_Agreementproperty_Reload)) fnVw_Agreementproperty_Reload();
            $("#divCreateOrEditModal_Vw_Agreementproperty").modal('hide');
        } else if (eData.mode == 'Get') {
            $("#" + CommonXformViewPanel + "_XformLabel").removeClass("text-primary text-success text-warning").addClass("text-primary");
            $("#" + CommonXformViewPanel + "_XformLabel").html('Vw_Agreementproperty');
        }

        try { eval('OnComplete1_' + eData.mode + '(eData);'); } catch (err) { }
    },
    Error: function (e, eData) {
        Titlemess($("#SelectedVw_AgreementpropertyTitle"), eData.value.Error.Errormessage);
    }
});
function OnComplete1_New(eData) {
    $("#divCreateOrEditModal_Vw_Agreementproperty").modal('show');
};


$("#btnEditXform_Vw_Agreementproperty").on('click', function (e) {
    if (WorkData.Vw_Agreementproperty_Data.Current.Id_Entity > 0) {
        $("#divCreateOrEditModal_Vw_Agreementproperty").modal('show');
    } else {
        GlobError("btnDeleteXform_Vw_Agreementproperty Error", 11);
    }
});
$("#btnDeleteXform_Vw_Agreementproperty").on('click', function (e) {
        if (WorkData.Vw_Agreementproperty_Data.Current.Id_Entity > 0) {
        fnDeleteXform_Vw_Agreementproperty();
    } else {
        GlobError("btnDeleteXform_Vw_Agreementproperty Error", 11);
    }
});



// Propertys grid

var cellsrendererRectype = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
    if (rowdata.Stable == 'AGREEMENT_PAYS') {
        return '<span style="margin: 4px; margin-top:8px; float: ' + columnproperties.cellsalign + '; color: #ff0000;">' + rowdata.Amount + '</span>';
    } else if (rowdata.Stable == 'AGREEMENT_ELEMENTS') {
        return '<span style="margin: 4px; margin-top:8px; float: ' + columnproperties.cellsalign + '; color: #008000;">' + rowdata.Amount + '</span>';
    } else {

    }
}

var GetRecords_Vw_Agreementproperty = '/GetRecords_Vw_Agreementproperty';
// -> Columns!!!!
var Vw_Agreementproperty_columns = [
//{ text: '§Id_Agreementproperty§', datafield: 'Id_Agreementproperty' },
//{ text: '§Id_Agreement§', datafield: 'Id_Agreement' },
//{ text: '§Id_Flows§', datafield: 'Id_Flows' },
{ text: '§Description§', datafield: 'Description' },
{ text: '§Recordtype§', datafield: 'Recordtype', cellsrenderer: cellsrendererRectype },
{ text: '§Amount§', datafield: 'Amount' },
{ text: '§Stable§', datafield: 'Stable' }
];
// -> Datafields!!!!
var Vw_Agreementproperty_datafields = [
  { name: 'Id_Agreementproperty', type: 'decimal', pk: 'Id_Agreementproperty' },
  { name: 'Id_Agreement', type: 'decimal' },
  { name: 'Id_Flows', type: 'decimal' },
  { name: 'Description', type: 'string' },
  { name: 'Recordtype', type: 'string'},
  { name: 'Amount', type: 'decimal' },
  { name: 'Stable', type: 'string' }
];
WorkData.Vw_Agreementproperty_Data = {
    Current: {
        Id_Entity: 0,
        Id_Parent: 0,
        Root: '',
        Name: '',
        Table: '',
        ChangedName: '',
        Properties: ''
    },
    Valid: true,
    Level: 20,
    TempFormValue: {},
    FkKey: xfindpk(Vw_Agreementproperty_datafields)
};




var initrowdetailsXform_Vw_Agreementproperty = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        AjaxGet(urls('/GetXform_Vw_Agreementproperty'), {
            Id_Entity: datarecord.Id_Agreementproperty,
            table: datarecord.Stable.initCap(),
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
var initrowdetailsSimple_Vw_Agreementproperty = function (index, parentElement, gridElement, datarecord) {
    var details = $($(parentElement).children()[0]);
    if (details != null) {
        // Example !!!
        var temstr = '<div class="row small" style="background-color:#f9f9f9;">';
        temstr += '<div class="col-md-2" >';
        temstr += '<p>§P_PHOTO§</p><p><img height="100" src="' + pageVariable.baseSiteURL + 'Ajax/GetPersonPic/' + datarecord.Id_Persons + '?version=' + Math.random() + '" /></p>';
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += '';
        temstr += '</div>';
        temstr += '<div class="col-md-5">';
        temstr += '';
        temstr += '</div>';
        var container = $(temstr);
        $(details).append(container);
    }
};



function fnloadCompleteGrid_Vw_Agreementproperty(data) {
    if (data.length > 0) {
    } else {
    }
};
var JqxParam_Grid_Vw_Agreementproperty = {
    url: urls(GetRecords_Vw_Agreementproperty),
    datafields: Vw_Agreementproperty_datafields,
    ajax_data: { id: WorkData.Agreement_Data.Current.Id_Entity, recordtype: 'Vw_AgreementpropertyItem' },
    loadComplete: fnloadCompleteGrid_Vw_Agreementproperty,
    param: {
        columns: Vw_Agreementproperty_columns,
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
        initrowdetails: initrowdetailsXform_Vw_Agreementproperty,
        altrows: true,
        ready: function () { }
    }
};
$("#divGrid_Vw_Agreementproperty").DXjqxGrid(JqxParam_Grid_Vw_Agreementproperty);

$('#divGrid_Vw_Agreementproperty').on('rowdoubleclick', function (event) {
    var args = event.args;
    WorkData.Vw_Agreementproperty_Data.Current.Id_Entity = args.row[WorkData.Vw_Agreementproperty_Data.FkKey];

});
$('#divGrid_Vw_Agreementproperty').on('rowclick', function (event) {
    var args = event.args;
    WorkData.Vw_Agreementproperty_Data.Current.Table = args.row.bounddata.Stable.initCap();
    WorkData.Vw_Agreementproperty_Data.Current.Id_Entity = args.row.bounddata[WorkData.Vw_Agreementproperty_Data.FkKey];
    $("#frmXform_Vw_Agreementproperty").Xform('get', { Id_Entity: WorkData.Vw_Agreementproperty_Data.Current.Id_Entity, table: args.row.bounddata.Stable.initCap() });
});
$('#divGrid_Vw_Agreementproperty').on('rowselect', function (event) {
    // event arguments.
    var args = event.args;
    WorkData.Vw_Agreementproperty_Data.Current.Id_Entity = args.row[WorkData.Vw_Agreementproperty_Data.FkKey];
    WorkData.Vw_Agreementproperty_Data.Current.Table = args.row.Stable.initCap();
});


// Sub grid req
var Vw_Agreementproperty_NestedSource = {
    datatype: "json",
    datafields: Vw_Agreementproperty_datafields,
    url: urls(GetRecords_Vw_Agreementproperty),
    data: { id: 0, recordtype: '' },
    async: false
};





/// Info status and persons gets
var GetXform_Organization = pageVariable.baseSiteURL +'Organization/GetXform_Organization';
var GetXform_Persons = '/GetXform_Persons';
var ohtml = "";
function fnGetXform_Organization() {
    AjaxGet(GetXform_Organization, {
        Id_Entity: WorkData.Organization_Data.Current.Id_Entity,
        Id_Xform: ''
    }, function (Data) {
        //$("#divXformHtmlView_Organization").html(Data.XformView);        
        ohtml += Data.XformView;
        fnGetStatusrequirements();
    });
};
function fnGetXform_Persons() {
    AjaxGet(urls(GetXform_Persons), {
        Id_Entity: WorkData.Persons_Data.Current.Id_Entity,
        Id_Xform: 'template_templateview'
    }, function (Data) {
        $("#divXformHtmlView_Persons").html(Data.XformView);
    });
};
fnGetXform_Organization();
fnGetXform_Persons();

var winPop = false;
$("#btnGetDocument_Agreement").on('click', function (e) {
    AjaxGet(pageVariable.baseSiteURL + 'Persons/GetAgrementDoc', {
        Id_Flows: WorkData.model_CurrentFlowstep.Id_Flow
    }, function (Data) {
        if (winPop && !winPop.closed) {  //checks to see if window is open
            winPop.close();
        }

        //winPop = window.open('', '_blank', 'width=' + (parseInt(window.innerWidth) * 0.7) + ',height=' + (parseInt(window.innerHeight) * 0.8) + ',toolbar=0,menubar=0,location=0,status=0,scrollbars=1,resizable=0,left=10,top=10');
        //winPop = window.open('', '_blank', 'width=900,height=1200,toolbar=0,menubar=0,location=0,status=0,scrollbars=1,resizable=1,left=10,top=10');
        winPop = window.open('', '_blank');
        var html = Data.Agrdoc;
        
        winPop.document.write(html);
        winPop.document.close()
        winPop.focus();
    });

});


function fnGetStatusrequirements() {

    var StatusrequirementsSource = {
        datatype: "json",
        datafields: [{name: 'Id_Statusrequirements',type: 'decimal'}],
        url: pageVariable.baseSiteURL +'Organization/GetRecords_Statusrequirements',
        data: {
            id: WorkData.Organization_Data.Current.Id_Entity,
            recordtype: ''
        },
        async: false,
        autoBind:false
    };
    nestedAdapter = new $.jqx.dataAdapter(StatusrequirementsSource, {
        loadComplete: function (records) {
            $.each(records, function (key, value) {
                AjaxSync(pageVariable.baseSiteURL + 'Organization/GetXform_Statusrequirements', {
                    Id_Entity: value.Id_Statusrequirements,
                    Id_Xform: ''
                }, function (Data) {
                    ohtml += Data.XformView;
                });
            });
            $("#divXformHtmlView_Organization").html(ohtml);        
        }
    });
    nestedAdapter.dataBind();
};


function LoadedComplett() {

    $("#IdBodyStart").hide();
    $("#IdBody").show();

    //var temp = $("#IdBody").html();
    //$("#IdBody").remove();
    //$("#bcontainer").html(temp);
};

