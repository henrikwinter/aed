
function remove(array, element) {
    //const index = array.indexOf(element);
    for (var i = 0; i < array.length; i++) {
        if (array[i].toString() === element.toString()) {
            array.splice(i, 1);
            return;
        }
    }
};
function uploadedOnClick() {
    $('.dxselector_1').off('click');
    $('.dxselector_1').on('click', function (e) {
        var id = this.dataset.dextraId;
        var y = this;
        if (id == 0) {
            $("#UploadFile").modal('show');
        } else {
            AjaxGet(pageVariable.baseSiteURL + "Ajax/DelUploadedFile", { id: id }, function (data) {
                $("#liid" + id).remove();
                remove(UploadData.fids, id);
            });
        }
    });
    $('.dxselector_2').off('click');
    $('.dxselector_2').on('click', function (e) {
        var id = this.dataset.dextraId;
        var filepath = pageVariable.baseSiteURL + 'Ajax/GetFile?id=' + id;
        top.location.href = filepath;
    });
};

var extraObj = $("#extraupload").uploadFile({
    url: pageVariable.baseSiteURL + 'Ajax/UploadFile',
    fileName: "MyFile",
    dragDrop: false,
    showStatusAfterSuccess: false,
    autoSubmit: false,
    showFileCounter: false,
    showFileSize: false,
    showCancel: false,
    showProgress: false,
    uploadStr: "Browse",
    customProgressBar: function (obj, s) {
        this.statusbar = $('<div style="padding-bottom:2px;"></div>');
        this.preview = $("<img class='ajax-file-upload-preview' />").width(s.previewWidth).height(s.previewHeight).appendTo(this.statusbar).hide();
        //this.filename = $("<div class='form-control alert-warning' disabled></div>").appendTo(this.statusbar);
        this.filename = $('<div style="display:none;"></div>').appendTo(this.statusbar);
        this.progressDiv = $("<div class='ajax-file-upload-progress'>").appendTo(this.statusbar).hide();
        this.progressbar = $("<div class='ajax-file-upload-bar'></div>").appendTo(this.progressDiv);
        this.abort = $("<div>" + s.abortStr + "</div>").appendTo(this.statusbar).hide();
        this.cancel = $("<div>" + s.cancelStr + "</div>").appendTo(this.statusbar).hide();
        this.done = $("<div>" + s.doneStr + "</div>").appendTo(this.statusbar).hide();
        this.download = $("<div>" + s.downloadStr + "</div>").appendTo(this.statusbar).hide();
        this.del = $("<div>" + s.deleteStr + "</div>").appendTo(this.statusbar).hide();
        return this;
    },
    extraHTML: function () {
        var html = '<div class="form-inline"><label class="sr-only">Name</label>'
        html += '<select class="custom-select mb-2 mr-sm-2 mb-sm-0" name="category" style="width:150px;">'
        html += '<option selected>Choose...</option>'
        html += '<option value="DOCUMENT">Document</option>'
        html += '<option value="CV">CV</option>'
        html += '<option value="CERT">Certificate</option>'
        html += '</select>'

        html += '<label class="sr-only">Username</label>'
        html += '<div class="input-group mb-2 mr-sm-2 mb-sm-0">'
        html += '<div class="input-group-addon"></div>'
        html += '<input type="text" class="form-control" name="tags" style="width:450px;">'
        html += '</div></div>'

        return html;
    },
    afterUploadAll: function (obj) {
        var t = obj.responses;
        for (var id in t) {

            try {
                var uid = JSON.parse(t[id]);
                if (uid.isUploaded) {
                    AjaxSync(pageVariable.baseSiteURL + "Ajax/GetFileInfo", { id: uid.newid }, function (data) {
                        UploadData.fids.push(uid.newid);
                        $("#listuploadedfiles").append(data.ret);
                    });
                }
            } catch (err) { GlobError(' Hiba', 11); }
        }
        extraObj.existingFileNames = [];
        extraObj.responses = [];
        extraObj.fileCounter = 0
        extraObj.selectedFiles = 0;
        extraObj.reset();
        uploadedOnClick();
        $("#UploadFile").modal('hide');
    },
    onError: function (files, status, errMsg, pd) {

        GlobError(errMsg + ' ' + files, 11);
        extraObj.existingFileNames = [];
        extraObj.responses = [];
        extraObj.fileCounter = 0
        extraObj.selectedFiles = 0;
        extraObj.reset();
        $("#UploadFile").modal('hide');
    },
    onSelect: function (files) {
        files[0].name;
        files[0].size;
        this.extraHTML = function () {
            var html = '<div class="form-inline"><label class="sr-only">Name</label>'
            html += '<select class="custom-select mb-2 mr-sm-2 mb-sm-0" name="category" style="width:150px;">'
            html += '<option selected>Choose...</option>'
            html += '<option value="DOCUMENT">Document</option>'
            html += '<option value="CV">CV</option>'
            html += '<option value="CERT">Certificate</option>'
            html += '</select>'

            html += '<label class="sr-only">Username</label>'
            html += '<div class="input-group mb-2 mr-sm-2 mb-sm-0">'
            html += '<div class="input-group-addon"></div>'
            html += '<input type="text" class="form-control"  name="tags" value="' + files[0].name + '" style="width:450px;">'
            html += '</div></div>'
            return html;
        }
        return true; //to allow file submission.
    },
    dynamicFormData: function()
    {
        return setuploadfromdata();
    }
});

$("#inpFile").fileupload({
    url: pageVariable.baseSiteURL + 'Ajax/UploadImg',
    dataType: 'json',
    add: function (e, data) {
        $("#ImgCont").empty();
        $("#ImgCont").append('<img src="" alt="" id="Img1" width="200" />');
        $('#Img1').attr('src', data.fileInput[0].value + "?version=" + Math.random());
        $('#Img1').Jcrop({
            //setSelect: [10, 10, $("#Img1").width()-10, $("#Img1").height()-10],
            //aspectRatio: 1 / 1,
            onSelect: function (cdata) {
                data.formData = cdata;
                data.formData.ow = $("#Img1").width();
                data.formData.oh = $("#Img1").height();
                CorpData = data;
            }
        });
    },
    done: function (event, data) {
        var id = data.jqXHR.responseJSON.newid;
        var urli = pageVariable.baseSiteURL + "Ajax/GetImg/" + id + "?version=" + Math.random();
        UploadData.faceid = id;
        $("#imgFace").attr('src', urli);
        $('#imgFace').data('data-dextra-id', id);
        $("#UploadPic").modal('hide');
    },
    fail: function (event, data) {
        GlobError("Image error", 10);
    }
});




/*

$('#jqxFileUpload').jqxFileUpload({
    browseTemplate: 'success',
    uploadTemplate: 'primary',
    cancelTemplate: 'danger',
    width: '100%', 
    uploadUrl: pageVariable.baseSiteURL + 'Ajax/UploadFile',
    fileInputName: 'MyFile'
});


$('#jqxFileUpload').on('uploadEnd', function (event) {
    var args = event.args;
    var fileName = args.file;
    
    var serverResponce = JSON.parse(args.response);
    if (serverResponce.isUploaded) {
        // Your code here.
        var lid = serverResponce.newid;
        UploadData.fids.push(lid);

        //var stringhtml = '<button type="button" class="list-group-item list-group-item-action dxselector_1" data-dextra-id="' + lid + '" > <i class="glyphicon glyphicon-list-alt" aria-hidden="true"></i> ' + fileName + '</button>';
        //$("#listuploadedfiles").append(stringhtml);

        //$("#listuploadedfiles").append('<img src="" alt="" id="UImg' + lid + '" width="20" />');
        //$('#UImg'+lid).attr('src', pageVariable.baseSiteURL + "Ajax/GetImg/"+lid + "?version=" + Math.random());
        UploadData.count--;
    } else {
        GlobError("Upload error", 12);
    }
    if (UploadData.count <= 0) {
        $("#UploadFile").modal('hide');
        for (var id in UploadData.fids) {
            var uid=UploadData.fids[id];
            AjaxSync(pageVariable.baseSiteURL + "Ajax/GetFileInfo", { id: uid }, function (data) {
                $("#listuploadedfiles").append(data.ret);
            });
        }
    }
});
$('#jqxFileUpload').on('select', function (event) {
    var args = event.args;
    var fileName = args.file;
    var fileSize = args.size; // Note: Internet Explorer 9 and earlier do not support getting the file size.
    // Your code here.
    UploadData.count++;
});
$('#jqxFileUpload').on('remove', function (event) {
    var fileName = event.args.file;
    // Your code here.
    UploadData.count--;
});






*/