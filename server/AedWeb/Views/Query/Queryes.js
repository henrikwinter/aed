include('~/Scripts/Dextra_Jqentityes.js' | args(0, 0));
$("#tabs").tabs();

var titlearr = [{
    Text: "Id",
    Width: "15%",
    Format: "",
    Hidden: "hideColumn"
}
];
function proceesCols() {
    if (titlearr[0] != null) {
        $("#QeryTable").jqxDataTable('setColumnProperty', 'Id', 'text', titlearr[0].Text);
        $("#QeryTable").jqxDataTable('setColumnProperty', 'Id', 'width', titlearr[0].Width);
        $("#QeryTable").jqxDataTable('setColumnProperty', 'Id', 'cellsFormat', titlearr[0].Format);
        $("#QeryTable").jqxDataTable(titlearr[0].Hidden, 'Id');
    } else {
        $("#QeryTable").jqxDataTable("hideColumn", 'Id');
    }
    for (i = 1; i < 10; i++) {
        var colname = "Col" + i;
        if (titlearr[i] != null) {
            $("#QeryTable").jqxDataTable('setColumnProperty', colname, 'text', titlearr[i].Text);
            $("#QeryTable").jqxDataTable('setColumnProperty', colname, 'width', titlearr[i].Width);
            $("#QeryTable").jqxDataTable('setColumnProperty', colname, 'cellsFormat', titlearr[i].Format);
            $("#QeryTable").jqxDataTable(titlearr[i].Hidden, colname);
        } else {
            $("#QeryTable").jqxDataTable("hideColumn", colname);
        }
    }
};
function getExportServer() {
    return 'ExportData';
}

var Qsource = {
    dataType: 'json',
    type: "POST",
    dataFields: [{
        name: 'Id',
        type: 'string'
    }, {
        name: 'Col1',
        type: 'string'
    }, {
        name: 'Col2',
        type: 'string'
    }, {
        name: 'Col3',
        type: 'string'
    }, {
        name: 'Col4',
        type: 'string'
    }, {
        name: 'Col5',
        type: 'string'
    }, {
        name: 'Col6',
        type: 'string'
    }, {
        name: 'Col7',
        type: 'string'
    }, {
        name: 'Col8',
        type: 'string'
    }, {
        name: 'Col9',
        type: 'string'
    }, ],
    url: pageVariable.baseSiteURL + "Query/RunQuery"
};
var QdataAdapter = new $.jqx.dataAdapter(Qsource, {
    autoBind: false,
    async: true,
    formatData: function (data) {
        var params = {
            QueryRoot: $("#XformRoot").val()
        };
        data = $("#Xmyform").serialize() + "&" + jQuery.param(params);

        return data;
    },
    loadComplete: function () {
        proceesCols();

    },
    loadError: function (xhr, status, error) {
        GlobError("§Ajax error:§" + xhr.responseText, 10);
    }
});
var ifsource = true;
$("#QeryTable").jqxDataTable({
    altRows: true,
    localization: getLocalization(),
    theme: pageVariable.Jqwtheme,
    sortable: true,
    filterable: true,
    height: 400,
    width: '100%',
    filterMode: 'advanced',
    selectionMode: 'singleRow',
    columns: [{
        text: 'id',
        dataField: 'Id',
        width: 80,
        align: 'right',
        cellsAlign: 'right',
        cellsFormat: 'c2'
    }, {
        text: 'col1',
        dataField: 'Col1',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col2',
        dataField: 'Col2',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col3',
        dataField: 'Col3',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col4',
        dataField: 'Col4',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col5',
        dataField: 'Col5',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col6',
        dataField: 'Col6',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col7',
        dataField: 'Col7',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col8',
        dataField: 'Col8',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }, {
        text: 'col9',
        dataField: 'Col9',
        width: '10%',
        align: 'left',
        cellsAlign: 'left',
        cellsFormat: ''
    }
    ]
});
$("#QeryTable").jqxDataTable({
    exportSettings: {
        columnsHeader: true,
        hiddenColumns: false,
        serverURL: 'ExportFile',
        characterSet: null,
        recordsInView: true,
        fileName: "jqxDataTable"
    }
});

$("#excelExport").jqxButton();
$("#csvExport").jqxButton({
    disabled: true
});
$("#htmlExport").jqxButton({
    disabled: true
});
$("#pdfExport").jqxButton();
$("#excelExport").click(function () {
    if (QdataAdapter.records.length > 0) {
        $("#QeryTable").jqxDataTable('exportData', 'xls');
    }
});
$("#csvExport").click(function () {
    $("#QeryTable").jqxDataTable('exportData', 'csv');
});
$("#htmlExport").click(function () {
    $("#QeryTable").jqxDataTable('exportData', 'html');
});
$("#pdfExport").click(function () {
    if (QdataAdapter.records.length > 0) {
        $("#QeryTable").jqxDataTable('exportData', 'pdf');
    }
});


$("#sendQuery").on('click', function () {
    debugger;
    $("#QueryModal").modal('hide');
   // debugger;
    var sw = $("#XformComplexType").val();
    if (sw == "ChartQueryType2" || sw == "ChartQueryType3") sw = "ChartQueryType";
    switch (sw) {
        case "GeneralQueryType":
            $("#tabs").tabs("option", "active", 0);
            if (ifsource) {
                $("#QeryTable").jqxDataTable({
                    source: QdataAdapter
                });
                ifsource = false;
            } else {
                QdataAdapter.dataBind();
            }
            break;
        case "ChartQueryType":
            $("#tabs").tabs("option", "active", 1);

            var settings = JClone(defaultchartSetting);
            try {

                if ($('#chartContainer').jqxChart('getInstance')) {
                    $('#chartContainer').jqxChart(settings);
                    chartInstance = $('#chartContainer').jqxChart('getInstance');
                    SetChartSpec(chartInstance);

                    chartInstance.source = ChartdataAdapter;
                   // chartInstance.refresh();
                    ChartdataAdapter.dataBind();
                } else {
                    SetChartSpec(settings);

                    settings.source = ChartdataAdapter;
                    $('#chartContainer').jqxChart(settings);
                    chartInstance = $('#chartContainer').jqxChart('getInstance');
                }

                ChartI = chartInstance;
            } catch (error) {
                GlobError("§Ajax error:§" + xhr.responseText, 10);
            }


            break;
        case "AltChartQueryType":
            $("#tabs").tabs("option", "active", 2);
            var altsettings = JClone(defaultchartSetting);
            try {

                if ($('#AltchartContainer').jqxChart('getInstance')) {
                    $('#AltchartContainer').jqxChart(altsettings);
                    AltchartInstance = $('#AltchartContainer').jqxChart('getInstance');
                    SetChartSpec(AltchartInstance);

                    AltchartInstance.source = AltChartdataAdapter;
                    //AltchartInstance.refresh();
                    AltChartdataAdapter.dataBind();
                } else {
                    SetChartSpec(altsettings);

                    altsettings.source = AltChartdataAdapter;
                    $('#AltchartContainer').jqxChart(altsettings);
                    AltchartInstance = $('#AltchartContainer').jqxChart('getInstance');
                }

                ChartI = AltchartInstance;
            } catch (error) {
                GlobError("§Ajax error:§" + xhr.responseText, 10);
            }

            break;
        default:
            $("#tabs").tabs("option", "active", 0);
    }

});
// ----------------------------------------------- Chatrs
var defaultchartSetting =
{
    title: "",
    description: "",
    showLegend: false,
    enableAnimations: true,
    //source: ChartdataAdapter,
    colorScheme: 'scheme01',
    xAxis: { dataField: 'Col1', visible: true },
    seriesGroups: [
            {
                type: 'column',
                valueAxis: { visible: false, title: { text: '' }, formatSettings: { decimalSeparator: ".", sufix: '', thousandsSeparator: ',' } },
                series: [
                        { dataField: 'Col2', displayText: '' }
                ]
            }
    ]
};

var Chartsource = {
    dataType: 'json',
    type: "POST",
    dataFields: [{ name: 'Id', type: 'string' }, { name: 'Col1', type: 'string' }, { name: 'Col2', type: 'string' }, { name: 'Col3', type: 'string' }, { name: 'Col4', type: 'string' }, { name: 'Col5', type: 'string' }, { name: 'Col6', type: 'string' }, { name: 'Col7', type: 'string' }, { name: 'Col8', type: 'string' }, { name: 'Col9', type: 'string' }, ],
    url: pageVariable.baseSiteURL + "Query/RunQuery"
};
var ChartdataAdapter = new $.jqx.dataAdapter(Chartsource, {
    async: true,
    autoBind: false,
    formatData: function (data) {

        var params = {
            QueryRoot: $("#QueryRoot").val()
        };
        data = $("#Xmyform").serialize() + "&" + jQuery.param(params);
        return data;
    },
    loadComplete: function () {

    },
    loadError: function (xhr, status, error) {
        GlobError("§Ajax error:§" + xhr.responseText, 10);
    }
});
var chartInstance = $('#chartContainer').jqxChart('getInstance');

$('#QuerySelectMenu').on('itemclick', function (event) {
    var element = event.args;
    var data = $("#QuerySelectMenu").DXjqxMenuGetSelectedrecord(event.args);

    AjaxGet(pageVariable.baseSiteURL + "Query/InitQueryesData", { root: data.value, formid: 'Xmyform'  }, function (ret) {
        titlearr = ret.ta;
        if (ret.root == "GeneralQuery") {
            proceesCols();
        }
        $('#jqxExpander').jqxExpander({ expanded: false });
        $('#jqxExpander').jqxExpander({ disabled: true });

        $("#Qxform").html(ret.xformHtml);


        $("#QueryModal").modal('show');
    })
});
$("#QuerySelectMenu").DXjqxRootsMenu({
    url: pageVariable.baseSiteURL + Xform_BuildRootSelector,
    filter: 'SubQueryType',   //'QueryRoots',
    ComplettCallback: function () {
        $("#IdBodyStart").hide();
        $("#IdBody").show();
        var t = $("#dashnav").children().removeClass('active');
        $(t[5]).addClass('active');

    },
    param: {
        theme: pageVariable.Jqwtheme,
        showTopLevelArrows: true,
        width: '100%'
    }
});





$("#print").click(function () {         //????? vesszok a sosrok vegen ... igy mukodik amugy nem...
    var content = $('#chartContainer')[0].outerHTML;
    var newWindow = window.open('', '', 'width=800, height=500'),
	document = newWindow.document.open(),
	pageContent = '<!DOCTYPE html>' + '<html>' + '<head>' + '<meta charset="utf-8" />' + '<title>jQWidgets Chart</title>' + '</head>' + '<body>' + content + '</body></html>';
    try {
        document.write(pageContent);
        document.close();
        newWindow.print();
        newWindow.close();
    } catch (error) { }

});
$("#print").jqxButton({});
$("#jpegButton").jqxButton({});
$("#pngButton").jqxButton({});
$("#pdfButton").jqxButton({});
$("#jpegButton").click(function () {
    $('#chartContainer').jqxChart('saveAsJPEG', 'myChart.jpeg', getExportServer());
});
$("#pngButton").click(function () {
    $('#chartContainer').jqxChart('saveAsPNG', 'myChart.png', getExportServer());
});
$("#pdfButton").click(function () {
    $('#chartContainer').jqxChart('saveAsPDF', 'myChart.pdf', getExportServer());
});


// -------- AltChart

var AltChartsource = {
    dataType: 'json',
    type: "POST",
    dataFields: [{ name: 'Id', type: 'string' }, { name: 'Col1', type: 'string' }, { name: 'Col2', type: 'string' }, { name: 'Col3', type: 'string' }, { name: 'Col4', type: 'string' }, { name: 'Col5', type: 'string' }, { name: 'Col6', type: 'string' }, { name: 'Col7', type: 'string' }, { name: 'Col8', type: 'string' }, { name: 'Col9', type: 'string' }, ],
    url: pageVariable.baseSiteURL + "Query/RunQuery"
};
var AltChartdataAdapter = new $.jqx.dataAdapter(AltChartsource, {
    async: true,
    autoBind: false,
    formatData: function (data) {

        var params = {
            QueryRoot: $("#QueryRoot").val()
        };
        data = $("#Xmyform").serialize() + "&" + jQuery.param(params);
        return data;
    },
    loadComplete: function () {

    },
    loadError: function (xhr, status, error) {
        GlobError("§Ajax error:§" + xhr.responseText, 10);
    }
});
var AltchartInstance = $('#AltchartContainer').jqxChart('getInstance');
var Altsettings = {
    title: "--",
    description: "--",
    showLegend: true,
    enableAnimations: true,
    showBorderLine: true,
    legendLayout: {
        left: 520,
        top: 170,
        width: 300,
        height: 200,
        flow: 'vertical'
    },
    legendPosition: {
        left: 520,
        top: 140,
        width: 100,
        height: 100
    },
    padding: {
        left: 5,
        top: 5,
        right: 5,
        bottom: 5
    },
    titlePadding: {
        left: 90,
        top: 0,
        right: 0,
        bottom: 10
    },
    source: AltChartdataAdapter,
    colorScheme: 'scheme01',
    columnSeriesOverlap: false,
    seriesGroups: [{
        type: 'pie',
        showLabels: true,
        offsetX: 0,
        offsetY: 0,
        xAxis: {
            formatSettings: {
                decimalSeparator: ".",
                sufix: 'x',
                thousandsSeparator: ','
            }
        },
        series: [{
            dataField: 'Col2',
            displayText: 'Col1',
            labelRadius: 120,
            initialAngle: 15,
            legendFormatSettings: {
                decimalSeparator: ".",
                sufix: 'x',
                thousandsSeparator: ','
            },
            radius: 170,
            centerOffset: 0,
            formatSettings: {
                sufix: '%',
                decimalPlaces: 1
            }
        }
        ]
    }
    ]
};

$("#Altprint").click(function () {
    var content = $('#AltchartContainer')[0].outerHTML;
    var newWindow = window.open('', '', 'width=800, height=500'),
	document = newWindow.document.open(),
	pageContent = '<!DOCTYPE html>' + '<html>' + '<head>' + '<meta charset="utf-8" />' + '<title>jQWidgets Chart</title>' + '</head>' + '<body>' + content + '</body></html>';
    try {
        document.write(pageContent);
        document.close();
        newWindow.print();
        newWindow.close();
    } catch (error) { }

});
$("#Altprint").jqxButton({});
$("#AltjpegButton").jqxButton({});
$("#AltpngButton").jqxButton({});
$("#AltpdfButton").jqxButton({});
$("#AltjpegButton").click(function () {
    $('#AltchartContainer').jqxChart('saveAsJPEG', 'myChart.jpeg', getExportServer());
});
$("#AltpngButton").click(function () {
    $('#AltchartContainer').jqxChart('saveAsPNG', 'myChart.png', getExportServer());
});
$("#AltpdfButton").click(function () {
    $('#AltchartContainer').jqxChart('saveAsPDF', 'myChart.pdf', getExportServer());
});




///------

var spinner_ho;

var Divided_UserFromHupaOrggroup_source = {
    datatype: "json",
    datafields: [{
        name: 'Hupaorggroupname',
        type: "string"
    }, {
        name: 'Hupaorggroupdesc',
        type: "string"
    }, {
        name: 'Id_Mst_Hupaorggroup',
        type: "decimal"
    }
    ],
    url: pageVariable.baseSiteURL + "Query/ListDivided_UserFromHupaOrggroup",
    data: {},
    async: true
};
var Divided_UserFromHupaOrggroup_Adapter = new $.jqx.dataAdapter(Divided_UserFromHupaOrggroup_source, {
    processData: function (data) {
        var target = document.getElementById('Divided_UserFromHupaOrggroup_List');
        //if (target)
        //spinner_ho = new Spinner(opts).spin(target);
    },
    loadComplete: function (data) {
        //spinner_ho.stop();
    }
});
$("#jqxExpander").jqxExpander({
    width: '100%',
    expanded: false,
    toggleMode: 'dblclick',
    initContent: function () {
        $("#Divided_UserFromHupaOrggroup_List").jqxListBox({
            filterable: true,
            allowDrop: true,
            allowDrag: true,
            source: Divided_UserFromHupaOrggroup_Adapter,
            displayMember: "Hupaorggroupname",
            valueMember: "Id_Mst_Hupaorggroup",
            width: '90%',
            height: 120,
            theme: pageVariable.Jqwtheme,
            dragEnd: function (dragItem, dropItem) { },
            renderer: function (index, label, value) {
                try {
                    var datarecord = Divided_UserFromHupaOrggroup_Adapter.originaldata[index];
                    return '<strong class="text-info">' + '' + ' </strong><small>' + '(' + label + ')</small>';
                } catch (e) { }
                return '<strong class="text-info">' + '' + ' </strong><small>' + '(' + label + ')</small>';
            }
        });
        $("#Assigned_UserToHupaOrggroup_List").jqxListBox({
            filterable: true,
            allowDrop: true,
            allowDrag: true,
            width: '90%',
            height: 120,
            theme: pageVariable.Jqwtheme,
            dragEnd: function (dragItem, dropItem) { },
            renderer: function (index, label, value) {
                try {
                    var datarecord = Assigned_UserToHupaOrggroup_Adapter.originaldata[index];
                    return '<strong class="text-info">' + '' + ' </strong><small>' + '(' + label + ')</small>';
                } catch (e) { }
                return '<strong class="text-info">' + '' + ' </strong><small>' + '(' + label + ')</small>';
            }
        });
    }
});
$("#Divided_UserFromHupaOrggroup_List, #Assigned_UserToHupaOrggroup_List").on('dragEnd', function (event) {
    if (event.args.label) {
        var ev = event.args.originalEvent;
        var x = ev.pageX;
        var y = ev.pageY;
        if (event.args.originalEvent && event.args.originalEvent.originalEvent && event.args.originalEvent.originalEvent.touches) {
            var touch = event.args.originalEvent.originalEvent.changedTouches[0];
            x = touch.pageX;
            y = touch.pageY;
        }
    }
    var log = "";
    $("#SelectedOrg").val('');
    var items = $("#Assigned_UserToHupaOrggroup_List").jqxListBox('getItems');
    for (var i = 0; i < items.length; i++) {
        log += items[i].value;
        if (i == 0)
            $("#SelectedOrg").val(items[i].value);
        if (i < items.length - 1)
            log += ', ';
    }
    $("#SelectedOrgs").val(log);
});
$('#jqxExpander').jqxExpander({
    expanded: false
});
$('#jqxExpander').jqxExpander({
    disabled: true
});
