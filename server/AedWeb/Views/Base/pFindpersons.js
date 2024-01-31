
var source = {
    localData: [],
    dataType: "array",
    dataFields: Persons_datafields
};
var dataAdapter = new $.jqx.dataAdapter(source);
var formatcolumn = function (row, dataField, cellText, rowData) {

    if (rowData.Userid && dataField == 'Usedname') {
        return 'Personsgridred';
    }
    return '';
};
$("#MorepersonExpander").jqxExpander({
    expanded: false,
    theme: pageVariable.Jqwtheme
});


$("#Personsgrid").jqxDataTable({
    source: dataAdapter,
    width: '100%',
    height: 240,
    altRows: true,
    showHeader: false,
    theme: pageVariable.Jqwtheme,
    filterable: true,
    sortable: true,
    selectionMode: 'singleRow',
    columns: [{
        text: '§Id_Persons§',
        datafield: 'Id_Persons',
        hidden: true
    }, {
        text: '§Usedname§',
        datafield: 'Usedname',
        width: '200px',
        cellClassName: formatcolumn
    }, {
        text: '§Birthfirstname§',
        datafield: 'Birthfirstname',
        hidden: false
    }, {
        text: '§Birthlastname§',
        datafield: 'Birthlastname',
        hidden: false
    }, {
        text: '§Birthdate§',
        datafield: 'Birthdate',
        width: '100px'
    }, {
        text: '§Placeofbirth§',
        datafield: 'Placeofbirth'
    }, {
        text: '§Motherfirstname§',
        datafield: 'Motherfirstname',
        width: '150px'
    }, {
        text: '§Motherlastname§',
        datafield: 'Motherlastname'
    }, {
        text: '§Userid§',
        datafield: 'Userid',
        hidden: true
    }
    ]
});