var dlgAreYouSoure = function (id, callback) {
    $("#"+id).modal('show');
    $("#"+id+"CloseRecord").off("click");
    $("#"+id+"Yes").off("click");
    $("#"+id+"No").off("click");
    $("#" + id + "CloseRecord").on("click", function () {
        $("#"+id).modal('hide');
        callback('close');
    });
    $("#" + id + "Yes").on("click", function () {
        $("#"+id).modal('hide');
        callback(true);
    });
    $("#" + id + "No").on("click", function () {
        $("#"+id).modal('hide');
        callback(false);
    });
};

function xfindpk(darr) {
    var ret = '';
    $.each(darr, function (i, n) {

        if ( n.pk ===undefined) {
        } else {
            ret = n.pk;
            return false;
        }
    });
    return ret;
}