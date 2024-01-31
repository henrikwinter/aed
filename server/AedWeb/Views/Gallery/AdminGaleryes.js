
var Currentgallery = "Gallery";
var datafields_forGallery = [
 { name: 'Id', type: 'decimal' },
 { name: 'Id_Parent', type: 'decimal' },
 { name: 'Name', type: 'string' },
 { name: 'Title', type: 'string' }
];
var GalleryTree = {
    url: pageVariable.baseSiteURL + "Gallery/Get_Gallery_Hierarchy",
    datafields: datafields_forGallery,
    ajax_data: {},
    mapp: [{ name: 'Name', map: 'label' }, { name: 'Rec', map: 'value' }],
    iconref: 'Name',
    id_hierarchy: 'Id',
    id_parent_hierarchy: 'Id_Parent',
    loadComplete: function (data) {

    },
    param: {
        width: '100%',
        height:300,
        theme: pageVariable.Jqwtheme
    }
};
function Jqx_FindRecordById(data, id, compare_propname) {
    var record = {};
    $.each(data, function () {
        if (this[compare_propname] == id) {
            record = this;
            return false;
        }
    });
    return record;
};

function GalleryTree_Item_ClickEvent(event) {
    var item = $(this).jqxTree('getItem', event.args.element); //.value.Rec.Name;
    if (item != null) {
        
        $('#div_Gallerydata_Jqxgrid').jqxGrid('clear');
        $("#DelGallery").prop("disabled", true);
        $("#AddGallery").prop("disabled", true);
        var path = "Gallery";
        if (item.value.Name == "Gallery") {
            $("#Currentpat").text("Gallery");
        } else {
            if (item.value.Id_Parent == 0) {
                path = path + "\\" + item.value.Name;
                $("#Currentpat").text(path);
                $("#AddGallery").prop("disabled", false);
            } else {
                var ttt = Jqx_FindRecordById(tri.DataAdapter.originaldata, item.value.Id_Parent, "Id");
                path = path + "\\" + ttt.Name + "\\" + item.value.Name;
                Currentgallery = path;
                $("#div_Gallerydata_Jqxgrid").DXjqxGridReload({ path: path });
                $("#Currentpat").text(Currentgallery);
                $("#DelGallery").prop("disabled", false);
                
            }
        }
    }
};
var tri=$('#div_Gallery_Jqxtree').DXjqxTree(GalleryTree);
$('#div_Gallery_Jqxtree').on('itemClick', GalleryTree_Item_ClickEvent);


var datafields_forGalleryelemets = [
 { name: 'Id', type: 'decimal' },
 { name: 'Filename', type: 'string' },
 { name: 'Title', type: 'string' },
 { name: 'ThumbnailFielname', type: 'string' },
 { name: 'Description', type: 'string' },
 { name: 'UsersAccesDescribe', type: 'string' },
 { name: 'Property', type: 'string' },
 { name: 'Flag', type: 'string' }
];
var Gallerydata_columns = [
 { text: '§Id§', datafield: 'Id', width: '30', editable :false},
 { text: '§Filename§', datafield: 'Filename', width: '150', editable: false },
 { text: '§Title§', datafield: 'Title', width: '200'},
 { text: '§Description§', datafield: 'Description', width: '200' },
 { text: '§UsersAccesDescribe§', datafield: 'UsersAccesDescribe' },
 { text: '§Property§', datafield: 'Property' },
 { text: '§Flag§', datafield: 'Flag', width: '40' }
]
var Gallerydata = {
    url: pageVariable.baseSiteURL + "Gallery/Get_Gallerydata",
    datafields: datafields_forGalleryelemets,
    ajax_data: { path: 'Gallery' },
    loadComplete: null,
    param: {
        columns: Gallerydata_columns,
        width: '100%',
        localization: getLocalization('hu'),
        columnsresize: true,
        editable: true,
        pageable: true,
        pagermode: 'simple',
        sortable: true,
        sorttogglestates: 1,
        filterable: true,
        showtoolbar: false,
        theme: pageVariable.JqwthemeAlt,
        altrows: true
    }
};
$("#div_Gallerydata_Jqxgrid").DXjqxGrid(Gallerydata);


$("#btnSave").on('click', function (data) {
    var rows = $('#div_Gallerydata_Jqxgrid').jqxGrid('getrows');
    //$.each(rows, function (index, value) {    });
    $.ajax({
        url: pageVariable.baseSiteURL + "Gallery/Set_Gallerydata",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ data: rows, path: Currentgallery }),
        success: function (response) {
            // $("#div_Gallerydata_Jqxgrid").DXjqxGridReload({ path: Currentgallery });
        },
        error: function (request, status, error) {
            alert(request.responseText);
        }

    });

})
$("#SetAcces").on('click', function (a) {
    var cp = $("#Currentpat").text();
    var oper = $("#Userstroper").val();
    AjaxGet(pageVariable.baseSiteURL + "Gallery/SetAcces", { path: cp, ustr: $("#Userstr").val(),oper:oper }, function (Data) {
    });

});

$("#AddGallery").on('click', function (a) {
    var cp = $("#Currentpat").text();
    AjaxGet(pageVariable.baseSiteURL + "Gallery/AddGalleryfolder", { path: cp, name: $("#Directoryname").val() }, function (Data) {
    });

});
$("#DelGallery").on('click', function (a) {
    var cp = $("#Currentpat").text();
    AjaxGet(pageVariable.baseSiteURL + "Gallery/DelGalleryfolder", { path: cp }, function (Data) {
    });


});

$("#btnNewUser").on('click', function (a) {
    AjaxGet(pageVariable.baseSiteURL + "Gallery/NewUser", { userid: $("#Username").val(), password: $("#Password").val() }, function (Data) {

    });
});

$("#btnDelUser").on('click', function (a) {
    AjaxGet(pageVariable.baseSiteURL + "Gallery/DeletUser", { userid: $("#Userstr").val() }, function (Data) {

    });
});

$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');


