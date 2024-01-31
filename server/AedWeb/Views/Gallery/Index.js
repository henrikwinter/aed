
var Api;
var ACurdata;

//$("#btn" + Currentgallery.Gname).css({ "opacity": "0.4", "border": "3px solid black" });

Api = $("#gallery").unitegallery({
    gallery_theme: "video"
});


$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');


