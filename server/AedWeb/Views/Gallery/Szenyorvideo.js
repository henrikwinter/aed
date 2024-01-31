$('.modal').on('hidden.bs.modal', function (e) {
    // do something...
    try {
        var id = this.id;
        //var v1src = $("#v" + id)[0].attr("src");
        $("#v" + id)[0].pause();
        //$('#'+id+' video').attr("src",'');
    } catch (Error) {}
});


$('.modal').on('show.bs.modal', function (e) {
    // do something...
    try {
        var id = this.id;
        var vid = document.getElementById("v" + id);
        vid.play();
    } catch (Error) {}
});


try {
    Api = $("#gallery").unitegallery({
        gallery_theme: "video",
        theme_skin: "right-no-thumb"
    });
} catch (Error) {

}


$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');


