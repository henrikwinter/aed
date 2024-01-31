include('~/Scripts/Dextra_Jqentityes.js' | args(0, 0));
include('~/Scripts/Dextra_Jqparameters.js' | args(0, 0));


$("#tblUsers").DXjqxGrid(jqxUsersGrid);
$("#tblUsers").on("cellclick", function (event) {
    var column = event.args.column.datafield;
    var rowData = event.args.row.bounddata;
    AjaxGet(pageVariable.baseSiteURL + "Account/GetUser", { id: rowData.Id },
        function (data) {
            $("#xfrmUser").html(data.form);
            $('#myModal').modal('show');
        });
    return true;
});

$("#btnPaswordReset").on('click', function () {
    $("#frmUser").mYPostForm("Account/PaswordReset", {}, function (Data) { });
});


$("#IdBodyStart").hide();
$("#IdBody").show();