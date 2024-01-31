

$("#btnProcEntityDesc").on('click', function () {
    $("#frmProcEntity").mYPostFormNew(pageVariable.baseSiteURL + 'Dev/prcGenerator', { test: 1 }, function (Data) {

    });
});


$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');