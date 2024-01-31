/*
var sampleData = [{
    a: 0.35,
    b: 14.5
}, {
    a: 1,
    b: 2.5
}, {
    a: 10,
    b: 0.2
}, {
    a: 100,
    b: 205
}, {
    a: 1,
    b: 100
}, {
    a: 5.11,
    b: 10.13
}, {
    a: 20.13,
    b: 10.13
}, {
    a: 600,
    b: 300
}];
var settingsxx = {
    title: "Logarithmic Scale Axis Example",
    description: "Sample logarithmic scale axis with base 2",
    padding: {
        left: 5,
        top: 5,
        right: 5,
        bottom: 5
    },
    titlePadding: {
        left: 0,
        top: 0,
        right: 0,
        bottom: 10
    },
    source: sampleData,
    enableAnimations: true,
    categoryAxis: {
        dataField: '',
        description: '',
        showGridLines: true,
        showTickMarks: true
    },
    seriesGroups: [{
        type: 'column',
        valueAxis: {
            description: 'Value',
            logarithmicScale: true,
            logarithmicScaleBase: 2,
            unitInterval: 1,
            tickMarksInterval: 1,
            gridLinesInterval: 1,
            formatSettings: {
                decimalPlaces: 3
            },

            horizontalTextAlignment: 'right'
        },
        series: [{
            dataField: 'a',
            displayText: 'A'
        }, {
            dataField: 'b',
            displayText: 'B'
        }]
    }]
};



$('#jqxChart').jqxChart(settingsxx);


var source = {
    datatype: "tsv",
    datafields: [{
        name: 'Year'
    }, {
        name: 'HPI'
    }, {
        name: 'BuildCost'
    }, {
        name: 'Population'
    }, {
        name: 'Rate'
    }
    ],
    url: "../../Gallery/detailed.txt",
};
var dataAdapter = new $.jqx.dataAdapter(source, {
    async: false,
    autoBind: true,
    loadComplete:function(data){
        //debugger;
    },
    loadError: function (xhr, status, error) {
        alert('Error loading "' + source.url + '" : ' + error);
    }
});
var settingsee = {
    title: "U.S. History Home Prices (1950-2017)",
    description: "Source: http://www.econ.yale.edu/~shiller/data.htm",
    enableAnimations: true,
    showLegend: true,
    padding: {
        left: 15,
        top: 5,
        right: 20,
        bottom: 5
    },
    titlePadding: {
        left: 10,
        top: 0,
        right: 0,
        bottom: 10
    },
    source: dataAdapter,
    xAxis: {
        dataField: 'Year',
        minValue: 1950,
        maxValue: 2017,
        unitInterval: 5,
        valuesOnTicks: true
    },
    colorScheme: 'scheme05',
    seriesGroups: [{
        alignEndPointsWithIntervals: false,
        type: 'splinearea',
        valueAxis: {
            visible: true,
            unitInterval: 20,
            title: {
                text: 'Index Value'
            },
            labels: {
                horizontalAlignment: 'right',
                formatSettings: {
                    decimalPlaces: 0
                }
            }
        },
        series: [{
            dataField: 'HPI',
            displayText: 'Real Home Price Index',
            opacity: 0.7
        }, {
            dataField: 'BuildCost',
            displayText: 'Building Cost Index',
            opacity: 0.9
        }
        ]
    }, {
        type: 'spline',
        alignEndPointsWithIntervals: false,
        valueAxis: {
            title: {
                text: 'Interest Rate'
            },
            position: 'right',
            unitInterval: 0.01,
            maxValue: 0.2,
            labels: {
                formatSettings: {
                    decimalPlaces: 2
                }
            },
            tickMarks: {
                visible: true,
                interval: 0.005,
            },
            gridLines: {
                visible: false,
                interval: 0.01
            }
        },
        series: [{
            dataField: 'Rate',
            displayText: 'Interest Rate',
            opacity: 1.0,
            lineWidth: 4,
            dashStyle: '4,4'
        }
        ]
    }
    ]
};
//var deby = dataAdapter;
//debugger;
$('#chartContainer').jqxChart(settingsee);
*/




// ---- ----------------------------------------------

var map;
function initMap(xlat, xlon) {
    map = new google.maps.Map(document.getElementById('map'), {
        //center: { lat: 47.416386, lng: 20.555044 },
        center: { lat: parseFloat(xlat), lng: parseFloat(xlon) },
        zoom: 7
    });
}
initMap(47.416386, 20.555044);

function setmarker(xlat, xlon) {
    //var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';
    var image = pageVariable.baseSiteURL + 'Content/images/aed/flag3.gif'
    var beachMarker = new google.maps.Marker({
        position: { lat: parseFloat(xlat), lng: parseFloat(xlon) },
        map: map,
        icon: image
    });
}


function urls(url) {
    if (checkStartsWith(url, 'Ajax')) {
        return pageVariable.baseSiteURL + '' + url;
    } else {
        return pageVariable.baseSiteURL + 'Aed' + url;
    }
};

var GetRecords_Aed_Activityes1 = '/GetRecords_Aed_Activityes';
var GetRecords_Aed_Activityes = '/GetRecords_Aed_VwActivityes';

var Aed_Activityes_columns = [
{ text: '§Id_Device§', datafield: 'Id_Device', width: 100 },
{ text: '§Pdate§', datafield: 'Pdate', width: 120 },
{ text: '§Gaccount§', datafield: 'Gaccount', width: 200 },
{ text: '§Ip§', datafield: 'Ip', width: 120 },
{ text: '§Coord§', datafield: 'Coord', width: 200 },
{ text: '§Category§', datafield: 'Category', width: 120 },
//{ text: '§Recordtype§', datafield: 'Recordtype', width: 200 },
{ text: '§Id_Persons§', datafield: 'Id_Persons'}
];
var Aed_Activityes_datafields = [
 
  { name: 'Id_Device', type: 'string' },
  { name: 'Gaccount', type: 'string' },
  { name: 'Category', type: 'string' },
  { name: 'Recordtype', type: 'string' },
  { name: 'Pdate', type: 'date' },
  { name: 'Id_Persons', type: 'decimal' },
  { name: 'Coord', type: 'string' },
  { name: 'Ip', type: 'string' }
];

function fnloadCompleteGrid_Aed_Activityes(data) {
    if (data.length > 0) {
    } else {
        if (1==1) {

        } else {
            //GlobError('No result!', 11);
        }
    }
    //PageAsync("1");

};

var JqxParam_Grid_Aed_Activityes = {
    url: urls(GetRecords_Aed_Activityes),
    datafields: Aed_Activityes_datafields,
    async: true,
    ajax_data: { id: 1, recordtype: '' },
    loadComplete: fnloadCompleteGrid_Aed_Activityes,
    param: {
        columns: Aed_Activityes_columns,
        width: '100%',
        theme: pageVariable.JqwthemeAlt,
        localization: getLocalization('hu'),
        columnsresize: true,
        pageable: true,
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        altrows: true,
        ready: function () {
        }
    }
};

$("#divGrid_Aed_Activityes").DXjqxGrid(JqxParam_Grid_Aed_Activityes);

$('#divGrid_Aed_Activityes').on('rowclick', function (event) {
    var args = event.args;
    coord = args.row.bounddata.Coord;
    var array = coord.split(";");
    initMap(array[0], array[1]);
    setmarker(array[0], array[1]);
    

});

LoadComplettCallback();


// -------------------------------------------------------

var Chartsource = {
    dataType: 'json',
    dataFields: [
        { name: 'Actdate', type: 'date' },
        { name: 'Connect_Val', type: 'decimal' },
        { name: 'Mesurement_Val', type: 'decimal' },
        { name: 'Pulse_Val', type: 'decimal' },
        { name: 'Resume_Val', type: 'decimal' }
    ],
    url: pageVariable.baseSiteURL + 'Aed/Getchart1Data'
};

var ChartdataAdapter = new $.jqx.dataAdapter(Chartsource, {
    async: true,
    //autoBind: false,
    formatData: function (data) {
        return data;
    },
    loadComplete: function () {
    },
    loadError: function (xhr, status, error) {
    }
});


var settings = {
    title: "Használat havi bontásban",
    description: "összes aktivity",
    showLegend: true,
    enableAnimations: true,
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
    source: ChartdataAdapter,
    xAxis: {
        dataField: 'Actdate', formatFunction: function (value) {
            var date = moment(value).format("YYYY.MM.DD.");
            return date; 
        }, type: 'date', baseUnit: 'month', valuesOnTicks: true, minValue: '01-01-2019', maxValue: '01-01-2020', tickMarks: {
            visible: true,
            interval: 1,
            color: '#BCBCBC'
        }, unitInterval: 1, gridLines: {
            visible: true,
            interval: 1,
            color: '#BCBCBC'
        }, labels: {
            angle: -45,
            rotationPoint: 'topright',
            offset: {
                x: 0,
                y: -25
            }
        }
    },
    colorScheme: 'scheme01',
    //columnSeriesOverlap: false,
    seriesGroups: [{
        type: 'stackedcolumn',
        columnsGapPercent: 50,
        seriesGapPercent: 0,
        series: [{
            dataField: 'Connect_Val',
            displayText: 'Connect_Val'
        }, {
            dataField: 'Mesurement_Val',
            displayText: 'Mesurement_Val'
        }, {
            dataField: 'Pulse_Val',
            displayText: 'Pulse_Val'
        },
        {
            dataField: 'Resume_Val',
            displayText: 'Resume_Val'
        }
        ]
    }
    ]
   
};
$('#chartContainer').jqxChart(settings);

