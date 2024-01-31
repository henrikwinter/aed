var dlgAreYouSoure = function (callback) {
    $("#dlgAreYouSoure").modal('show');
    $("#dlgAreYouSoureCloseRecord").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback('close');
    });
    $("#dlgAreYouSoureYes").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback(true);
    });
    $("#dlgAreYouSoureNo").on("click", function () {
        $("#dlgAreYouSoure").modal('hide');
        callback(false);
    });
};

var PressDeleteButton = false;

var Api;
var ACurdata;

if (color == "Video") {

    try {
        Api = $("#gallery").unitegallery({
            gallery_theme: "video"
        });

    } catch (Error) {

    }

} else {
    var galoptions = {
        gallery_theme: "tiles",
        tiles_justified_space_between: 10,
        tiles_type: "nested",

       // tiles_type: "justified",
        gallery_width: 900,							//gallery width		
        //gallery_width: "100%",				//gallery width
        gallery_height: 650,							//gallery height
        gallery_min_width: 900,						//gallery minimal width when resizing
        gallery_min_height: 650,
        gallery_background_color: "#32a89e",		//set custom background color. If not set it will be taken from css.


        //tiles_justified_row_height: 150,	//base row height of the justified type
        tiles_justified_space_between: 3,	//space between the tiles justified type
        tiles_set_initial_height: true,		//columns type related only
        tiles_enable_transition: true,
        tiles_nested_optimal_tile_width: 250,	// tiles optimal width
        //thumb_fixed_size: true,
        
        //tiles_col_width: 450,					//column width - exact or base according the settings

        //tile design options:

        tile_enable_border: false,			//enable border of the tile
        tile_border_width: 3,				//tile border width
        tile_border_color: "#F0F0F0",		//tile border color
        tile_border_radius: 0,				//tile border radius (applied to border only, not to outline)

        tile_enable_outline: false,			//enable outline of the tile (works only together with the border)
        tile_outline_color: "#8B8B8B",		//tile outline color

        tile_enable_shadow: true,			//enable shadow of the tile
        tile_shadow_h: 1,					//position of horizontal shadow
        tile_shadow_v: 1,					//position of vertical shadow
        tile_shadow_blur: 3,					//shadow blur
        tile_shadow_spread: 2,				//shadow spread
        tile_shadow_color: "#8B8B8B",		//shadow color

        tile_enable_action: true,			//enable tile action on click like lightbox
        tile_as_link: false,				//act the tile as link, no lightbox will appear
        tile_link_newpage: true,			//open the tile link in new page

        tile_enable_overlay: true,			//enable tile color overlay (on mouseover)
        tile_overlay_opacity: 0.4,			//tile overlay opacity
        tile_overlay_color: "#000000",		//tile overlay color

        tile_enable_icons: true,			//enable icons in mouseover mode
        tile_show_link_icon: false,			//show link icon (if the tile has a link). In case of tile_as_link this option not enabled
        tile_space_between_icons: 26,		//initial space between icons, (on small tiles it may change)

        tile_enable_image_effect: false,		//enable tile image effect
        tile_image_effect_type: "bw",		//bw, blur, sepia - tile effect type
        tile_image_effect_reverse: false,	//reverce the image, set only on mouseover state



    }


    $("#theme").val(galerytheme);



    if (galerytheme == "tiles") {
        galoptions.gallery_theme = "tiles";
    } 
    if (galerytheme == "default") {
        galoptions.gallery_theme = "default";
    }
    

    Api = $("#gallery").unitegallery(galoptions);



    $("#Deleteimg").on('click', function (e) {
        PressDeleteButton = true;
    });

    
};

$("#btnthemedefault").on('click', function (e) {
    $("#theme").val("default");
    $("#frmrefresh").submit();
});

$("#btnthemetiles").on('click', function (e) {
    $("#theme").val("tiles");
    $("#frmrefresh").submit();
});


Api.on("item_change", function (num, data) {
    //on item change, get item number and item data
    ACurdata = data;
    if (PressDeleteButton) {
        PressDeleteButton = false;
        try {
            var delimg = ACurdata.urlImage.substring(3);
            $("#delimgid").html(delimg);
            dlgAreYouSoure(function (confirm) {
                if (confirm) {
                    AjaxGet(pageVariable.baseSiteURL + "Gallery/DelGalleryPicture", { path: delimg }, function (Data) {
                        $("#frmrefresh").submit();

                    });
                } else {

                }
            });

        } catch (e) { }
    }

});




/// -------------------------------------------------------------------------------------------
$("#imgFace").on('click', function (e) {
    $("#UploadPic").modal('show');
});


var crop_max_width = 400;
var crop_max_height = 400;
var jcrop_api;
var canvas;
var context;
var image;
var OrigFilleName;
var prefsize;

$("#inpFile").change(function () {
    loadImage(this);
});
function loadImage(input) {
    if (input.files && input.files[0]) {
        OrigFilleName = input.files[0].name;
        var reader = new FileReader();
        canvas = null;
        reader.onload = function (e) {
            image = new Image();
            image.onload = validateImage;
            image.src = e.target.result;
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function dataURLtoBlob(dataURL) {
    var BASE64_MARKER = ';base64,';
    if (dataURL.indexOf(BASE64_MARKER) == -1) {
        var parts = dataURL.split(',');
        var contentType = parts[0].split(':')[1];
        var raw = decodeURIComponent(parts[1]);

        return new Blob([raw], {
            type: contentType
        });
    }
    var parts = dataURL.split(BASE64_MARKER);
    var contentType = parts[0].split(':')[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;
    var uInt8Array = new Uint8Array(rawLength);
    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], {
        type: contentType
    });
}
function validateImage() {
    if (canvas != null) {
        image = new Image();
        image.onload = restartJcrop;
        image.src = canvas.toDataURL('image/png');
    } else restartJcrop();
}
function restartJcrop() {
    if (jcrop_api != null) {
        jcrop_api.destroy();
    }
    $("#views").empty();
    $("#views").append("<canvas id=\"canvas\">");
    canvas = $("#canvas")[0];
    context = canvas.getContext("2d");
    canvas.width = image.width;
    canvas.height = image.height;
    context.drawImage(image, 0, 0);
    $("#canvas").Jcrop({
        onSelect: selectcanvas,
        onRelease: clearcanvas,
        boxWidth: crop_max_width,
        boxHeight: crop_max_height
    }, function () {
        jcrop_api = this;
    });
    clearcanvas();
}
function clearcanvas() {
    prefsize = {
        x: 0,
        y: 0,
        x2: Math.round(canvas.width),
        y2: Math.round(canvas.height),
        w: canvas.width,
        h: canvas.height,
    };
}
function selectcanvas(coords) {
    prefsize = {
        x: Math.round(coords.x),
        y: Math.round(coords.y),
        x2: Math.round(coords.x2),
        y2: Math.round(coords.y2),
        w: Math.round(coords.w),
        h: Math.round(coords.h)
    };
}
function applyCrop() {
    canvas.width = prefsize.w;
    canvas.height = prefsize.h;
    context.drawImage(image, prefsize.x, prefsize.y, prefsize.w, prefsize.h, 0, 0, canvas.width, canvas.height);
    validateImage();
}
function applyScale(scale) {
    if (scale == 1) return;
    canvas.width = canvas.width * scale;
    canvas.height = canvas.height * scale;
    context.drawImage(image, 0, 0, canvas.width, canvas.height);
    validateImage();
}
function applyRotate() {
    canvas.width = image.height;
    canvas.height = image.width;
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.translate(image.height / 2, image.width / 2);
    context.rotate(Math.PI / 2);
    context.drawImage(image, -image.width / 2, -image.height / 2);
    validateImage();
}
function applyHflip() {
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.translate(image.width, 0);
    context.scale(-1, 1);
    context.drawImage(image, 0, 0);
    validateImage();
}
function applyVflip() {
    context.clearRect(0, 0, canvas.width, canvas.height);
    context.translate(0, image.height);
    context.scale(1, -1);
    context.drawImage(image, 0, 0);
    validateImage();
}

$("#cropbutton").click(function (e) {
    applyCrop();
});
$("#scalebutton").click(function (e) {
    var scale = prompt("Scale Factor:", "1");
    applyScale(scale);
});
$("#rotatebutton").click(function (e) {
    applyRotate();
});
$("#hflipbutton").click(function (e) {
    applyHflip();
});
$("#vflipbutton").click(function (e) {
    applyVflip();
});


$("#btnUpload").click(function (e) {


    try {
        formData = new FormData($("#UploadForm"));
        var blob = dataURLtoBlob(canvas.toDataURL('image/png'));
        //---Add file blob to the form data
        formData.append("img", blob);
        formData.append("origFileName", OrigFilleName);
        formData.append("Id_Ufiles", 'Idufile');
        formData.append("Titles", $("#Picturetitle").val());
        formData.append("galerycolor", color);
        formData.append("galleryname", folder);
        formData.append("x", prefsize.x);
        formData.append("y", prefsize.y);
        formData.append("x2", prefsize.x2);
        formData.append("y2", prefsize.y2);

        formData.append("w", prefsize.w);
        formData.append("h", prefsize.h);
        formData.append("ow", prefsize.w);
        formData.append("oh", prefsize.h);

        $.ajax({
            url: pageVariable.baseSiteURL + 'Gallery/UploadImg',
            type: "POST",
            data: formData,
            dataType: 'json',
            contentType: false,
            cache: false,
            processData: false,
            success: function (data) {
                //alert("Success");
                $("#UploadPic").modal('hide');
                $("#frmrefresh").submit();
            },
            error: function (data) {
               
                alert("Error");
            },
            complete: function (data) { }
        });

    } catch (e) {


    }




});


$("#IdBodyStart").hide();
$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');


