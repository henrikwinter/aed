
var XCt = {
    baseSiteURL: pageVariable.baseSiteURL,
    newControler: 'Ajax',
    workController: 'Persons'
}

$("#TPerson").dxXform({ context: XCt, name: 'Persons', id_viewdiv: 'TPersonview',
    ChangeComplet: function () {
        debugger;
    }
});

$("#TPerson").dxXform({ ChangeComplet: function () { debugger; } });

$("#TPerson").dxXform('New', 'Person');

$("#btnTPersonTest3").on('click', function () {
    $("#TPerson").dxXform('Changeroot', { root: 'Candidate' });
});

$("#btnTPersonTest").on('click', function () {
    $("#TPerson").dxXform('Get', { id: 8373 });
});

$("#btnTPersonTest1").on('click', function () {
    $("#TPerson").dxXform('Save', { arg: { Id_Entity: 8373 } });
});

$("#btnTPersonTest2").on('click', function () {
    $("#TPerson").dxXform('Insert', { arg: { Id_Entity: 1, Id_Parent: 2, Recordtype: 'Proba', Id_Flows: 5 } });
});

