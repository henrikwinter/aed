UploadPic.views = "views";
$("#inpFile").change(function () {
    loadImage(this);
    $("#btnUpload").prop('disabled', false);
});

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

    formData = new FormData($("#UploadForm"));
    var blob = dataURLtoBlob(canvas.toDataURL('image/png'));

    //---Add file blob to the form data
    formData.append("img", blob);
    formData.append("origFileName", UploadPic.OrigFilleName);
    formData.append("Id_Ufiles", 'Idufile');
    formData.append("Id_Animal", WorkData.Animals_Data.Current.Id);
    formData.append("Titles", $("#Picturetitle").val());
    formData.append("Picturetype", "FACEPIC");

    //  formData.append("galerycolor", color);
    //  formData.append("galleryname", folder);
    formData.append("x", UploadPic.prefsize.x);
    formData.append("y", UploadPic.prefsize.y);
    formData.append("x2", UploadPic.prefsize.x2);
    formData.append("y2", UploadPic.prefsize.y2);

    formData.append("w", UploadPic.prefsize.w);
    formData.append("h", UploadPic.prefsize.h);
    formData.append("ow", UploadPic.prefsize.w);
    formData.append("oh", UploadPic.prefsize.h);

    $.ajax({
        url: pageVariable.baseSiteURL + 'Ajax/UploadAnimalPic',
        type: "POST",
        data: formData,
        dataType: 'json',
        contentType: false,
        cache: false,
        processData: false,
        success: function (data) {
            try { fnUploadComplette(); } catch (err) { }

            //$("#btnUpload").prop('disabled', true);
            //DXSetState('Searchmode');
        },
        error: function (data) {
            //debugger;
            alert("Error");
        },
        complete: function (data) {

        }
    });


});
$("#btnUpload").prop('disabled', true);

function fnGetPicture(Id, iwith, iheight) {
    var urli = pageVariable.baseSiteURL + "Ajax/GetAnimalPic/" + Id + "?version=" + Math.random();
    var imageObj = new Image();
    imageObj.onload = function () {
        $("#views").empty();
        $("#views").append("<canvas id=\"canvas\">");
        UploadPic.canvas = $("#canvas")[0];
        UploadPic.canvas.width = iwith;  //imageObj.width;
        UploadPic.canvas.height = iheight;// imageObj.height;
        UploadPic.context = UploadPic.canvas.getContext("2d");
        //  UploadPic.context.drawImage(imageObj, 0, 0);
        fitImageOn(UploadPic.canvas, imageObj);
    };
    imageObj.src = urli;

}

var fitImageOn = function (canvas, imageObj) {
    var imageAspectRatio = imageObj.width / imageObj.height;
    var canvasAspectRatio = canvas.width / canvas.height;
    var renderableHeight, renderableWidth, xStart, yStart;

    // If image's aspect ratio is less than canvas's we fit on height
    // and place the image centrally along width
    if (imageAspectRatio < canvasAspectRatio) {
        renderableHeight = canvas.height;
        renderableWidth = imageObj.width * (renderableHeight / imageObj.height);
        xStart = (canvas.width - renderableWidth) / 2;
        yStart = 0;
    }

        // If image's aspect ratio is greater than canvas's we fit on width
        // and place the image centrally along height
    else if (imageAspectRatio > canvasAspectRatio) {
        renderableWidth = canvas.width
        renderableHeight = imageObj.height * (renderableWidth / imageObj.width);
        xStart = 0;
        yStart = (canvas.height - renderableHeight) / 2;
    }

        // Happy path - keep aspect ratio
    else {
        renderableHeight = canvas.height;
        renderableWidth = canvas.width;
        xStart = 0;
        yStart = 0;
    }
    UploadPic.context.drawImage(imageObj, xStart, yStart, renderableWidth, renderableHeight);
};