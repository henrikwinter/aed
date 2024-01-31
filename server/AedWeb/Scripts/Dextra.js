if (typeof Object.assign != 'function') {
    (function () {
        Object.assign = function (target) {
            'use strict';
            // We must check against these specific cases.
            if (target === undefined || target === null) {
                throw new TypeError('Cannot convert undefined or null to object');
            }

            var output = Object(target);
            for (var index = 1; index < arguments.length; index++) {
                var source = arguments[index];
                if (source !== undefined && source !== null) {
                    for (var nextKey in source) {
                        if (source.hasOwnProperty(nextKey)) {
                            output[nextKey] = source[nextKey];
                        }
                    }
                }
            }
            return output;
        };
    })();
};



$.fn.mYPostForm = function (dataUrl, arg, callback) {
    var o = {};
    o = this.serializeObject();
    if (arg) {
        $.each(arg, function (i, value) {
            o[i] = value;
        });
    }
    $.ajax({
        type: "POST",
        url: pageVariable.baseSiteURL + dataUrl,
        content: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        data: o,
        success: function (data, status, xhr) {
            if (callback) {
                callback(data, status, xhr);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            ChecksessionTimeout(xhr);
            GlobError('mYPostForm:' + xhr.responseText, 0);
        }
    });
};
$.fn.mYPostFormNew = function (dataUrl, arg, callback) {
    var o = {};
    o = this.serializeObject();
    if (arg) {
        $.each(arg, function (i, value) {
            o[i] = value;
        });
    }
    $.ajax({
        type: "POST",
        url: dataUrl,
        content: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        context: this,
        data: o,
        success: function (data, status, xhr) {
            if (callback) {
                callback(data, status, xhr);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            ChecksessionTimeout(xhr);
            GlobError('mYPostForm:' + xhr.responseText, 0);
        }
    });
};

$.fn.addHidden = function (name, value) {
    return this.each(function () {
        var input = $("<input>").attr("type", "hidden").attr("name", name).val(value);
        $(this).append($(input));
    });
};

function htmlEncode(value) {
    return $('<div/>').text(value).html();
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
}

function ChecksessionTimeout(xhr) {
    try {
        var obj = JSON.parse(xhr.responseText);
        if (obj.serverError) {
            if (obj.serverError == "SessionExpire")
                window.location.replace("/Home/SessionExpire");
        }
    } catch (err) { }
};
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {

        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
function SchemeRoots(dataUrl, filter, callback) {
    var Ret = {
        Source: {},
        DataAdapter: {},
        MenuRecords: {},
        Loaded: false
    };

    Ret.Source = {
        datatype: "json",
        datafields: [
        { name: 'id', type: 'decimal' },
        { name: 'parentid', type: 'decimal' },
        { name: 'value', type: 'string' },
        { name: 'Label', type: 'string' },
        { name: 'Table', type: 'string' },
        { name: 'RefRoots', type: 'string' }
        ],
        url: dataUrl,
        data: { filter: filter },
        async: true
    };
    Ret.DataAdapter = new $.jqx.dataAdapter(Ret.Source, {
        autoBind: false,
        loadComplete: callback,
        loadError: function (xhr, status, error) {
            ChecksessionTimeout(xhr);
            GlobError('SchemeRoots:' + xhr.responseText, 0);
        }
    });
    //Ret.DataAdapter.dataBind();
    return Ret;
};


$.fn.DXjqxComboBox = function (param) {
    var Ret = {
        Source: {},
        DataAdapter: {},
        Records: {},
        Extvar: param
    };
    if (!param.datafields) {
        param.datafields = [{ name: 'Typename' }, { name: 'Display' }];
        param.param.displayMember = 'Display';
        param.param.valueMember = 'Typename';
    }
    Ret.Source = {
        datatype: "json",
        datafields: param.datafields,
        url: param.url,
        data: param.ajax_data,
        async: false
    };
    if (!param.loadError) {
        param.loadError = function (xhr, status, error) {
            ChecksessionTimeout(xhr);
            GlobError('DXjqxComboBox:' + xhr.responseText, 0);
        };
    }
    if (!param.beforeLoadComplete) {
        //param.beforeLoadComplete = DXjqxTreeBeforeLoadComplete;
    }
    Ret.DataAdapter = new $.jqx.dataAdapter(Ret.Source, {
        autoBind: false,
        loadComplete: param.loadComplete,
        loadError: param.loadError,
        beforeLoadComplete: param.beforeLoadComplete
    });
    //Ret.DataAdapter.dataBind();

    param.param.source = Ret.DataAdapter;
    var value = this.val();
    var strid = '#' + this.attr('id');
    var newElement = '<div id="' + this.attr('id') + '" name="' + this.attr('name') + '" ></div>';
    $(this).replaceWith(newElement);
    $(strid).jqxComboBox(param.param);
    $(strid).jqxComboBox('val', value);
    $(strid).data(Ret);
    return Ret;
};
$.fn.DXjqxComboBoxReload = function (param) {
    var data = $(this).data();
    data.Source.data = param.ajax_data;
    data.DataAdapter.dataBind();
};

$.fn.DXjqxMenuGetSelectedrecord = function (element) {
    var data = $(this).data();
    return Jqx_FindRecordById(data.DataAdapter.originaldata, element.id, 'id');
}

$.fn.DXjqxRootsCombo = function (param) {
    var strid = '#' + this.attr('id');
    var Ret = SchemeRoots(param.url, param.filter, param.ComplettCallback);
    if (param.selector) Ret.Source.data.selector = param.selector;
    param.param.source = Ret.DataAdapter;
    param.param.displayMember = "Label";
    param.param.valueMember = "id";
    $(strid).jqxComboBox(param.param);
    $(strid).data(param);
    $(strid).data(Ret);
    Ret.Loaded = true;
};
$.fn.DXjqxRootsComboReload = function (nparam) {
    var strid = '#' + this.attr('id');
    $(strid).unbind("change");
    var param = $(strid).data();
    param.Source.data.filter = nparam.filter;
    param.DataAdapter._options.loadComplete = nparam.callback;
    param.DataAdapter.dataBind();
};


// -------Tree
function JqxTree_DragEnd(dragItem, dropItem, dropPosition, url, callback) {
    if (dragItem.id == dropItem.id)
        return false;
    if (dropPosition == 'inside') {
        JqxTree_MoveItem({
            url: url,
            success: callback,
            ajax_data: {
                Id: dragItem.id,
                NewParentId: dropItem.id,
                dropPos: dropPosition
            }
        });
    } else {
        JqxTree_MoveItem({
            url: url,
            success: callback,
            ajax_data: {
                Id: dragItem.id,
                NewParentId: dropItem.id,
                dropPos: dropPosition
            }
        });
    }
    return true;
};
function JqxTree_MoveItem(param) {
    var Ret;
    if (!param.error) {
        param.error = function (jqXHR, textStatus, errorThrown) {
            ChecksessionTimeout(jqXHR);
            GlobError("Ajax error:" + jqXHR.responseText, 0);
        };
    }
    $.ajax({
        cache: false,
        dataType: 'json',
        url: param.url,
        data: param.ajax_data,
        type: 'GET',
        async: false,
        success: param.success,
        error: param.error
    });
    return Ret;
}

function DXjqxTreeSearchCalback(id) {
    var newElement = '<div id="' + trim(id, '#') + '" ></div>';
    $(id).replaceWith(newElement);
};
function DXjqxTreeIconSource(param) {

    var Ret = {
        Icon_Source: {},
        iconAdapter: {},
    };
    Ret.Icon_Source = {
        datatype: "xml",
        datafields: [{
            name: 'ref',
            type: 'string'
        }, {
            name: 'fpath',
            type: 'string'
        }
        ],
        url: param.urlforiconxml,
        data: { filename: param.iconxml },
        root: "Icons",
        record: "Icon",
        id: 'IconID',
        async: false
    };
    if (!param.loadError) {
        param.loadError = function (xhr, status, error) {

            ChecksessionTimeout(xhr);
            GlobError('Error :' + xhr.responseText, 0);
        };
    }
    Ret.iconAdapter = new $.jqx.dataAdapter(Ret.Icon_Source, {
        loadComplete: param.loadComplete,
        loadError: param.loadError
    });
    Ret.iconAdapter.dataBind();
    return Ret.iconAdapter;
};
function Old_DXjqxTreeBeforeLoadComplete(records) {

    $.each(records, function (i, k) {
        try {
            var Rec = {};
            $.each(k, function (j, v) {
                Rec[j] = v;
            })
            records[i]['Rec'] = Rec;
            // this.Rec = this;

            DXjqxTreeBeforeLoadCompleteRecord(records, i, k);
        } catch (e) { }
    });
};
function DXjqxTreeBeforeLoadComplete(records) {

    $.each(records, function (i, k) {
        try {

            DXjqxTreeBeforeLoadCompleteRecord(i, k);
            var Rec = {};
            $.each(k, function (j, v) {
                Rec[j] = v;
            })
            records[i]['Rec'] = Rec;
            // this.Rec = this;

            //DXjqxTreeBeforeLoadCompleteRecord(i,k);
        } catch (e) { }
    });
};


function DXjqxTreeBeforeLoadCompleteRecord(records, i, k) {

};

$.fn.DXjqxTree = function (param) {
    var Ret = {
        Source: {},
        DataAdapter: {},
        Records: {},
        Extvar: param
    };
    var iconAdapter;
    Ret.Source = {
        datatype: "json",
        datafields: param.datafields,
        url: param.url,
        data: param.ajax_data,
        async: false
    };
    if (param.urlforiconxml) {
        iconAdapter = DXjqxTreeIconSource(param);
        Ret.Source.datafields.push({ name: 'icon', value: param.iconref, values: { source: iconAdapter.records, value: 'ref', name: 'fpath' } });

    }
    if (!param.loadError) {
        param.loadError = function (xhr, status, error) {
            ChecksessionTimeout(xhr);
            GlobError('Error :' + xhr.responseText, 0);
        };
    }
    if (!param.beforeLoadComplete) {
        param.beforeLoadComplete = function (records) {   //DXjqxTreeBeforeLoadComplete;
            $.each(records, function (i, k) {
                try {
                    if ($.isFunction(param.LoadCompleteRecord)) ret = param.LoadCompleteRecord(i, k);

                    var Rec = {};
                    $.each(k, function (j, v) {
                        Rec[j] = v;
                    })
                    records[i]['Rec'] = Rec;
                } catch (e) { }
            });
        };
    }
    Ret.DataAdapter = new $.jqx.dataAdapter(Ret.Source, {
        loadComplete: param.loadComplete,
        loadError: param.loadError,
        beforeLoadComplete: param.beforeLoadComplete
    });
    Ret.DataAdapter.dataBind();
    Ret.Records = Ret.DataAdapter.getRecordsHierarchy(param.id_hierarchy, param.id_parent_hierarchy, 'items', param.mapp);
    if (Ret.Records == null) {
        Ret.Records = {};
    }
    param.param.source = Ret.Records;
    $(this).data(Ret);
    $(this).jqxTree(param.param);

    if (param.contextMenu) {
        $(this).on('mousedown', function (event) {
            var target = $(event.target).parents('li:first')[0];
            var item = $(this).jqxTree('getItem', target);
            var rightClick = isRightClick(event);
            var found = null;
            if (rightClick && target != null) {

                $(this).jqxTree('selectItem', target);
                var scrollTop = $(window).scrollTop();
                var scrollLeft = $(window).scrollLeft();
                param.contextMenu.jqxMenu('open', parseInt(event.clientX) + 5 + scrollLeft, parseInt(event.clientY) + 5 + scrollTop);
                return false;
            }
        });
    }
    if (param.onClickEvent) {
        $(this).on('itemClick', param.onClickEvent);
    }

    return Ret;
}
$.fn.DXjqxTreeReload = function (ajax_data) {
    var item = $(this).jqxTree('getSelectedItem');
    var data = $(this).data();
    data.Records = {};
    if (ajax_data) data.Extvar.ajax_data = ajax_data;
    $(this).DXjqxTree(data.Extvar);
    if (item) {
        $(this).jqxTree('ensureVisible', $("#" + item.id + "")[0]);
        $(this).jqxTree('selectItem', $("#" + item.id + "")[0]);
    }
};
$.fn.DXjqxTreeSearch = function (treestrid, param) {
    var Reselem = [];
    var container = this.selector;
    var jqxWidget = $("#" + treestrid);
    var offset = jqxWidget.offset();
    if (typeof (param) === 'undefined') {
        param = {
            callback: DXjqxTreeSearchCalback,
            position: { x: offset.left + 50, y: offset.top + 50 },
            //position: { x: 250 + 50, y: 450 + 50 },
            height: 120,
            width: 140,
            label: 'Search',
            label1: 'Search'
        };
    } else {
        if (typeof (param.callback) === 'undefined') param.callback = DXjqxTreeSearchCalback;
        if (typeof (param.position) === 'undefined') param.position = { x: offset.left + 50, y: offset.top + 50 };
        if (typeof (param.height) === 'undefined') param.height = 120;
        if (typeof (param.width) === 'undefined') param.width = 140;
        if (typeof (param.label) === 'undefined') param.label = 'Search';
        if (typeof (param.label1) === 'undefined') param.label1 = 'Search1';
    }


    $(this).append($('<div>', {
        id: treestrid + '_WindowHeader'
    }));
    $("#" + treestrid + "_WindowHeader").append($('<span>', {
        html: ' <span class="glyphicon glyphicon-search" aria-hidden="true"></span>&nbsp;' + param.label
    }));

    $(this).append($('<div>', {
        style: "overflow: hidden;",
        id: treestrid + '_WindowContent'
    }));
    $("#" + treestrid + "_WindowContent").html(
        '<table class="table"><tr><td>' +
        '<input class="form-control input-sm" type="text" id="' + treestrid + '_TreeSearch' + '" value="" />' +
        '</td><td>' +
        '<button type="button" id="' + treestrid + '_StartSearchTree' + '" class="btn btn-success btn-xs"><span class="glyphicon glyphicon-flash" aria-hidden="true"></span></button>' +
        '</td></tr><tr><td colspan="2"><div id="' + treestrid + '_TreeSearchResult"></div>' +
        '</td></tr></table>'
        )
    $(this).jqxWindow({
        position: param.position,
        showCollapseButton: true,
        height: param.height + 105,
        width: param.width + 25,
        initContent: function () {
            $(this).jqxWindow('focus');
            $("#" + treestrid + '_TreeSearchResult').jqxListBox({
                width: param.width,
                height: param.height,
                autoHeight: false
            });
            $("#" + treestrid + '_TreeSearchResult').on('select', function (event) {
                var args = event.args;
                if (args) {
                    var index = args.index;
                    var item = args.item;
                    var originalEvent = args.originalEvent;
                    var label = item.label;
                    var value = item.value;
                    var type = args.type;
                    element = Reselem[index];
                    $("#" + treestrid).jqxTree('expandItem', element);
                    $("#" + treestrid).jqxTree('selectItem', element);
                    var item = $("#" + treestrid).jqxTree('getSelectedItem');
                    try {
                        $("#" + treestrid).jqxTree('ensureVisible', item);
                    } catch (e) { }

                    param.callback(container);
                }
            });
            $("#" + treestrid + '_StartSearchTree').on('click', function (e) {
                e.preventDefault();
                $("#" + treestrid + '_TreeSearchResult').jqxListBox('clear');
                Reselem = [];
                var searched = $("#" + treestrid + '_TreeSearch').val();
                var items = $("#" + treestrid).jqxTree('getItems');
                $.each(items, function () {
                    if (this.label.search(searched) != -1) {
                        $("#" + treestrid + '_TreeSearchResult').jqxListBox('addItem', {
                            label: this.label,
                            value: this.Id
                        });
                        Reselem.push(this);
                    }
                    if (this.items) {
                        $.each(this.items, function (name, value) {
                            if (this.label.search(searched) != -1) {
                                $("#" + treestrid + '_TreeSearchResult').jqxListBox('addItem', {
                                    label: this.label,
                                    value: this.Id
                                });
                                Reselem.push(this);
                            }
                        });
                    }
                });
                $("#" + treestrid + '_TreeSearchResult').show();
            });
        }
    });


    $(this).on('close', function (event) {
        param.callback(container);
    });
};
$.fn.DXjqxTreeGetSelectedrecord = function (compare_propname) {
    var data = $(this).data();
    if (typeof (compare_propname) === 'undefined') compare_propname = data.Extvar.id_hierarchy;
    var item = $(this).jqxTree('getSelectedItem');
    //var item = $(this).jqxTree('getItem', element);
    var ret;
    try {
        ret = Jqx_FindRecordById(data.DataAdapter.originaldata, item.id, compare_propname);
    } catch (e) {
    }
    return ret;
}
$.fn.DxjqxTreeContextmenu = function (srcContextMenu, Tree_ContextMenuClickEvent) {
    $(document).on('contextmenu', function (e) {
        return false;
    });
    var TreeContextMenuParam = {
        width: '220px',
        source: srcContextMenu,
        theme: pageVariable.Jqwtheme,
        autoOpenPopup: false,
        autoSizeMainItems: true,
        mode: 'popup'
    };
    var strid = '#' + this.attr('id');
    var ret = $(strid).jqxMenu(TreeContextMenuParam);
    $(strid).on('itemclick', Tree_ContextMenuClickEvent);
    return ret;
};



$.fn.DXjqxGrid = function (param) {
    var Ret = {
        Source: {},
        DataAdapter: {},
        Extvar: param
    };
    Ret.Source = {
        datatype: "json",
        datafields: param.datafields,
        url: param.url,
        data: param.ajax_data,
        async: false
    };
    if (!param.loadError) {
        param.loadError = function (xhr, status, error) {
            ChecksessionTimeout(xhr);
            GlobError('DXjqxGrid:' + xhr.responseText, 0);
        };
    }
    if (!param.beforeLoadComplete) {
        param.beforeLoadComplete = function (data) { };
    }
    if (!param.loadComplete) {
        param.loadComplete = function (data) { };
    }
    if (!param.autoBind) {
        param.autoBind = false;
    }
    if (!param.async) {
        param.async = false;
    }
    Ret.DataAdapter = new $.jqx.dataAdapter(Ret.Source, {
        loadComplete: param.loadComplete,
        beforeLoadComplete: param.beforeLoadComplete,
        loadError: param.loadError,
        async: param.async,
        autoBind: param.autoBind
    });
    param.param.source = Ret.DataAdapter;
    $(this).data(Ret);
    $(this).jqxGrid(param.param);
    return Ret;
};
$.fn.DXjqxGridReload = function (ajax_data) {
    var Ret = $(this).data();
    Ret.Source.data = ajax_data;
    Ret.DataAdapter.dataBind();
};



function isRightClick(event) {
    var rightclick;
    if (!event)
        var event = window.event;
    if (event.which)
        rightclick = (event.which == 3);
    else if (event.button)
        rightclick = (event.button == 2);
    return rightclick;
};




function Ajax(url, data, type, async, callback) {
    $.ajax({
        cache: false,
        dataType: 'text json',
        url: url,
        data: data,
        async: async,
        type: type,
        success: function (data, status, xhr) {
            if (data.success == false) data = { Error: { Errormessage: 'Server Error', Errorcode: 500 } };
            callback(data);
        },
        //error: function (jqXHR, textStatus, errorThrown) {
        error: function (xhr, status, error) {
            ChecksessionTimeout(xhr);
            GlobError('Ajax:' + xhr.statusText, 0);
        }
    });
};
function AjaxPost(url, data, callback) {
    debugger;
    Ajax(url, data, "POST", true, callback);
};
function AjaxGet(url, data, callback) {
    Ajax(url, data, "GET", true, callback);
};
function AjaxSync(url, data, callback) {
    $.ajax({
        cache: false,
        dataType: 'text json',
        url: url,
        data: data,
        timeout: 100,
        async: false,
        type: "GET",
        success: function (data, status, xhr) { callback(data); },
        error: function (xhr, textStatus, errorThrown) {
            ChecksessionTimeout(xhr);
            GlobError('Ajax:' + xhr.responseText, 0);
        }
    });

}



function XformReorderXformid(strid, xform) {
    var f = strid.match(/_([0-9]*)_/i);
    if (f) {
        strid = strid.substring(0, strid.length - 3);
    }
    var list = [];
    var idx = 2;
    for (i = 2; i < 10; i++) {
        elem = $("#" + xform + " [id=blk_" + strid + "_" + i + "_]");
        if (elem.length == 1) {
            var temp = elem[0].id;
            var myRe = new RegExp('blk_' + strid + '_([0-9])_');
            var n = myRe.exec(temp);
            list.push({
                index: idx,
                id: elem[0].id,
                name: elem[0].name,
                curentidx: n[1]
            });
            idx = idx + 1;
        }
    }
    list.forEach(function (o) {
        if (o.index != o.curentidx) {
            $("#" + xform + " [id^=" + strid + "_" + o.curentidx + "_]").each(function (e) {
                this.id = this.id.replace('' + strid + '_' + o.curentidx + '_', '' + strid + '_' + o.index + '_');
                if (this.name) {
                    this.name = this.name.replace('' + strid + '_' + o.curentidx + '_', '' + strid + '_' + o.index + '_');
                }
            });
            $("#" + xform + " [id^=blk_" + strid + "_" + o.curentidx + "_]").each(function (e) {
                this.id = this.id.replace('blk_' + strid + '_' + o.curentidx + '_', 'blk_' + strid + '_' + o.index + '_');
            });
            $("#" + xform + " [id^=oc_" + strid + "_" + o.curentidx + "_]").each(function (e) {
                this.id = this.id.replace('oc_' + strid + '_' + o.curentidx + '_', 'oc_' + strid + '_' + o.index + '_');
            });
        }
    });
};
function BindXform1(xform) {
    $("#" + xform + " [data-xform='ocplus']").on('click', function (e) {
        var xform = $(this).parents('form').attr('id');
        e.preventDefault();
        var strid = this.id.substring(3);
        var elem = $('#' + xform + ' [id="blk_' + strid + '"]');
        var parent = this.parentElement.parentElement.className;
        var renderblock = '';
        if (parent == 'form-group') {
            renderblock = '<div id="InprogressCopyed" class="form-group" style="display:none;">' + elem[0].innerHTML + '</div>';
        } else {
            renderblock = '<div id="InprogressCopyed" class="col-sm-offset-1"  style="display:none;">' + elem[0].innerHTML + '</div>';
        }
        $("#blk_" + strid).after(renderblock);
        var inprogres = $("#InprogressCopyed");
        inprogres[0].children['0'].innerHTML = inprogres[0].children['0'].innerHTML.replace('ocplus', 'ocminus');
        inprogres[0].children['0'].innerHTML = inprogres[0].children['0'].innerHTML.replace('glyphicon-plus', 'glyphicon-minus');
        $("#InprogressCopyed").show();
        $("#InprogressCopyed [id^=" + strid + "]").each(function (e) {
            this.id = this.id.replace('' + strid, '' + strid + '_9_');
            if (this.name) {
                this.name = this.name.replace('' + strid, '' + strid + '_9_');
            }
        });
        $("#InprogressCopyed [id^=blk_" + strid + "]").each(function (e) {
            this.id = this.id.replace('blk_' + strid, 'blk_' + strid + '_9_');
        });
        $("#InprogressCopyed [id^=oc_" + strid + "]").each(function (e) {
            this.id = this.id.replace('oc_' + strid, 'oc_' + strid + '_9_');
        });
        $("#InprogressCopyed").attr('id', 'blk_' + strid + '_9_');
        XformReorderXformid(strid, "Xfrom1");
    });
}
function XformSetOccurencesRecursive(url, xform, onComplett) {
    $("#" + xform + " [data-xform='ocminus']").off('click');
    $("#" + xform + " [data-xform='ocplus']").off('click');
    $("#" + xform + " [data-xform='ocplus']").on('click', function (e) {
        e.preventDefault();
        var xform = $(this).parents('form').attr('id');
        var strid = this.id.substring(3);
        var idx = 1;
        var elem = $('#' + xform + ' [id="blk_' + strid + '"]');
        idx += elem.length;
        for (i = 2; i < 10; i++) {
            elem = $("#" + xform + " [id=blk_" + strid + "_" + i + "_]");
            idx += elem.length;
        }
        var froot = $("#" + xform + " #XformRoot").val();
        var frefroot = $("#" + xform + " #XformRefRoot").val();

        AjaxSync(pageVariable.baseSiteURL + url, {
            root: froot,
            refroot: frefroot,
            path: strid,
            idn: idx
        }, function (Data) {

            $("#" + xform + " #blk_" + strid).after(Data.ret);
            $('#' + xform).find(':input[type=date]').each(function () {
                ConvInpToJqdate(this.id, xform);
            });
            $('#' + xform).find('textarea').each(function () {
                try {
                    if ($('#' + this.id).data('jflag')) {

                    } else {

                        $('#' + this.id).jqte();
                        $('#' + this.id).data('jflag', true);
                    }
                } catch (err) {
                    alert('Errrrrror');
                }
            });

            if (onComplett) onComplett();
            XformSetOccurencesRecursive(url, xform, onComplett);

        });
    });
    $("#" + xform + " [data-xform='ocminus']").on('click', function (e) {
        e.preventDefault();
        var mstrid = this.id.substring(3);
        $("#" + xform + " #blk_" + mstrid).remove();
        XformReorderXformid(mstrid, xform);
    });
    $('#' + xform).find(':input[type=date]').each(function () {
        ConvInpToJqdate(this.id, xform);
    });

};
function XformSetOccurences(xform, onComplett) {
    XformSetOccurencesRecursive(Xform_RenderPart_Url, xform, onComplett);
};
function XfromComboOptions(xmlfilename, xmldataroot, filter, cValue) {
    var ret;
    AjaxSync(pageVariable.baseSiteURL + 'Ajax/Xform_ComboSource', { xmlfilename: xmlfilename, xmldataroot: xmldataroot, filter: filter, cValue: cValue }, function (Data) {
        ret = Data.ret;
    });
    return ret;
};


var getLocalization = function () {
    var localizationobj = {
        // separator of parts of a date (e.g. '/' in 11/05/1955)
        '/': "/",
        // separator of parts of a time (e.g. ':' in 05:44 PM)
        ':': ":",
        // the first day of the week (0 = Sunday, 1 = Monday, etc)
        firstDay: 0,
        days: {
            // full day names
            names: ["vasárnap", "hétfő", "kedd", "szerda", "csütörtök", "péntek", "szombat"],
            // abbreviated day names
            namesAbbr: ["V", "H", "K", "Sze", "Cs", "P", "Szo"],
            // shortest day names
            namesShort: ["V", "H", "K", "Sze", "Cs", "P", "Szo"]
        },
        months: {
            // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
            names: ["január", "február", "március", "április", "május", "június", "július", "augusztus", "szeptember", "október", "november", "december", ""],
            // abbreviated month names
            namesAbbr: ["jan.", "febr.", "márc.", "ápr.", "máj.", "jún.", "júl.", "aug.", "szept.", "okt.", "nov.", "dec.", ""]
        },
        // AM and PM designators in one of these forms:
        // The usual view, and the upper and lower case versions
        //      [standard,lowercase,uppercase]
        // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
        //      null
        AM: ["DE", "de", "DE"],
        PM: ["DU", "du", "DU"],
        eras: [
        // eras in reverse chronological order.
        // name: the name of the era in this culture (e.g. A.D., C.E.)
        // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
        // offset: offset in years from gregorian calendar
{ "name": "A.D.", "start": null, "offset": 0 }
        ],
        twoDigitYearMax: 2029,
        patterns: {
            d: "yyyy.MM.dd.",
            D: "yyyy. MMMM d.",
            t: "H:mm",
            T: "H:mm:ss",
            f: "yyyy. MMMM d. H:mm",
            F: "yyyy. MMMM d. H:mm:ss",
            M: "MMMM d.",
            Y: "yyyy. MMMM",
            // S is a sortable format that does not vary by culture
            S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss"
        },
        percentsymbol: "%",
        currencysymbol: "Ft",
        currencysymbolposition: "after",
        decimalseparator: '.',
        thousandsseparator: ',',
        pagergotopagestring: "oldalra ugrás:",
        pagershowrowsstring: "Sorok mutatása:",
        pagerrangestring: " / ",
        pagerfirstbuttonstring: "Első",
        pagerlastbuttonstring: "utolsó",
        pagerpreviousbuttonstring: "Előző",
        pagernextbuttonstring: "Következő",
        groupsheaderstring: "Egy adott oszlop szerinti csoportosításhoz fogja a kívánt oszlopot és húzza ide.",
        sortascendingstring: "Növekvő sorrend",
        sortdescendingstring: "Csökkenő sorrend",
        sortremovestring: "Sorrend eltávolítás",
        groupbystring: "Csoportosítás ezen oszlop szerint",
        groupremovestring: "Eltávolítás a csoportosításból",
        filterclearstring: "Mégsem",
        filterstring: "Szűrés",
        filtershowrowstring: "Sorok mutatása, amely:",
        filterorconditionstring: "Vagy",
        filterandconditionstring: "És",
        filterselectallstring: "(Mindet kijelöl)",
        filterchoosestring: "Kérem válasszon:",
        filterstringcomparisonoperators: ['üres', 'nem üres', 'tartalmazza', 'tartalmazza(pontos egyezés)',
'nem tartalmazza', 'nem tartalmazza(pontos egyezés)', 'ezzel kezdődjön', 'ezzel kezdődjön(pontos egyezés)',
'ezzel végződjön', 'ezzel végződjön(pontos egyezés)', 'egyenlő', 'egyenlő(pontos egyezés)', 'nulla', 'nem nulla'],
        filternumericcomparisonoperators: ['egyenlő', 'nem egyenlő', 'kisebb mint', 'kisebb vagy egyenlő', 'nagyobb mint', 'nagyobb vagy egyenlő', 'nulla', 'nem nulla'],
        filterdatecomparisonoperators: ['egyenlő', 'nem egyenlő', 'kisebb mint', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
        filterbooleancomparisonoperators: ['egyenlő', 'nem egyenlő'],
        validationstring: "A megadott érték nem érvényes",
        emptydatastring: "Nincs megjeleníthető adat",
        filterselectstring: "Válasszon szűrőt",
        loadtext: "Betöltés...",
        clearstring: "Törlés",
        todaystring: "Ma"
    };
    return localizationobj;
};


function changeInputToDivToJqw(obj, form) {

    var rs = {
        source: null,
        displayMember: null,
        valueMember: null,
        width: 300, height: 10,
        theme: pageVariable.Jqwtheme,
        formatString: 'yyyy.MM.dd.'
    };
    var newElement = '<div id="' + obj.attr('id') + '" name="' + obj.attr('name') + '" role="' + obj.attr('role') + '"  value="' + obj.attr('value') + '" ></div>';
    var origvalue = obj.val();
    switch (obj.attr('role')) {
        case 'jqxDropDownList':
            obj.trigger('JqwInit', [rs]); //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            obj.replaceWith(newElement);
            obj = $('#' + form + ' div[id=' + obj[0].id + ']');

            //$('#' + obj[0].id).jqxDropDownList({
            obj.jqxDropDownList({
                source: rs.source,
                displayMember: rs.displayMember,
                valueMember: rs.valueMember,
                width: rs.width, height: rs.height,
                theme: rs.theme
            });
            $('#' + obj[0].id).jqxDropDownList('val', obj.val());
            break;
        case 'jqxComboBox':
            obj.trigger('JqwInit', [rs]);  //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            obj.replaceWith(newElement);
            obj = $('#' + form + ' div[id=' + obj[0].id + ']');

            //$('#' + obj[0].id).jqxComboBox({
            obj.jqxComboBox({
                source: rs.source,
                displayMember: rs.displayMember,
                valueMember: rs.valueMember,
                width: rs.width, height: rs.height,
                theme: rs.theme
            });
            //$('#' + obj[0].id).jqxComboBox('val', obj.val());
            obj.jqxComboBox('val', obj.val());
            break;
        case 'jqxRating':
            obj.trigger('JqwInit', [rs]);  //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            obj.replaceWith(newElement);
            obj = $('#' + form + ' div[id=' + obj[0].id + ']');

            //$('#' + obj[0].id).jqxComboBox({
            obj.jqxRating({
                width: rs.width, height: rs.height,
                theme: rs.theme
            });
            //$('#' + obj[0].id).jqxComboBox('val', obj.val());
            obj.jqxRating('val', origvalue);// obj.val());
            break;
            //.jqxSlider({	showTickLabels: true,	tooltip: true,	mode: "fixed",	height: 60,	min: 0,	max: 255,	ticksFrequency: 25.5,	value: 0,	step: 25.5,});
        case 'jqxSlider':
            obj.trigger('JqwInit', [rs]);  //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            obj.replaceWith(newElement);
            obj = $('#' + form + ' div[id=' + obj[0].id + ']');

            //$('#' + obj[0].id).jqxComboBox({
            obj.jqxSlider({
                width: rs.width, height: rs.height,
                theme: rs.theme
            });
            //$('#' + obj[0].id).jqxComboBox('val', obj.val());
            obj.jqxSlider('val', origvalue);// obj.val());
            break;
        case 'DateTimeInput':
            obj.datepicker({
                showOn: "button",
                buttonImageOnly: true,
                buttonImage: "images/calendar.png",
                dateFormat: "yy.mm.dd."
            });
            break;
        case 'jqteTextarea':
            if (obj.data('jflag') === undefined) {
                obj.trigger('JqwInit', [rs]);  //$('#' + obj[0].id).trigger('JqwInit', [rs]);
                obj.jqte({});
                obj.data('jflag', true);
            }
            break;
        case 'jqxDateTimeInput':

            obj.trigger('JqwInit', [rs]); //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            obj.replaceWith(newElement);
            var xdate = inputdate(obj.val());
            obj = $('#' + form + ' div[id=' + obj[0].id + ']');
            //$('#' + obj[0].id)
            obj.jqxDateTimeInput({ theme: rs.theme, formatString: 'yyyy-MM-dd', culture: 'hu-HU', showTimeButton: false, showCalendarButton: false });
            // obj.jqxDateTimeInput({ width: rs.width, theme: rs.theme, formatString: 'yyyy-MM-dd', culture: 'hu-HU' });  formatString: 'yyyy.MM.dd.',culture: 'hu-HU',
            //$('#' + obj[0].id)
            if (xdate != "Invalid Date") obj.jqxDateTimeInput('setDate', xdate);
            break;
        case 'jqxMaskedInput':
            obj.trigger('JqwInit', [rs]); //$('#' + obj[0].id).trigger('JqwInit', [rs]);
            //$('#' + obj[0].id).jqxMaskedInput({

            obj.jqxMaskedInput({
                width: rs.width, //height: rs.height,
                mask: rs.mask, promptChar: rs.promptChar,
                theme: rs.theme
            });

            break;
        default:

    }
}
/* *******************************************************************************************
A form input tagjaira meghívja a JqwInit triggert. Inicializálja  a jqwidgeteket a formban
******************************************************************************************* */
function JqwTransformWithRole(form) {

    var $inputs = $('#' + form + ' :input');
    var ids = {};
    $inputs.each(function (index) {

        changeInputToDivToJqw($(this), form);
    });
};

function inputdate(dateString) {
    try {
        var parts = dateString.split('.');
        var date = new Date(parseInt(parts[0], 10),
                            parseInt(parts[1], 10) - 1,
                            parseInt(parts[2], 10));

    } catch (err) {
        return Date(1900, 00, 01);
    }

    return date;
};

function JqxSetValidatorLabel(id, mode, text) {

    if (mode == 'show') {
        $("#blk_" + id).find('.col-sm-0').append('<label id="' + id + 'Customerrlabel" class="jqx-validator-error-label" style="left: 0px; top: 2px; width: 178px; display: block; position: relative;">' + text + '</label>');
    } else {
        $("#" + id + 'Customerrlabel').remove();
    }

}

function trim(str, chr) {
    var rgxtrim = (!chr) ? new RegExp('^\\s+|\\s+$', 'g') : new RegExp('^' + chr + '+|' + chr + '+$', 'g');
    return str.replace(rgxtrim, '');
};
function rtrim(str, chr) {
    var rgxtrim = (!chr) ? new RegExp('\\s+$') : new RegExp(chr + '+$');
    return str.replace(rgxtrim, '');
};
function ltrim(str, chr) {
    var rgxtrim = (!chr) ? new RegExp('^\\s+') : new RegExp('^' + chr + '+');
    return str.replace(rgxtrim, '');
};


$.fn.dxalign = function (toid, x, y) {
    var p = $("#" + toid);
    var position = p.position();
    this.css("top", position.top + y);
    this.css("left", position.left + x);


}


var LangDictionary = { 'start': 'start' };

function TranslateLang(key, world, LangDictionary) {

    trkey = world;
    try {

        if (LangDictionary[key + '_' + world]) {
            trkey = LangDictionary[key + '_' + world];
        }
    } catch (Error) {

    }
    return trkey;
}


// ------ 
function JClone(obj) {
    return jQuery.extend(true, {}, obj);
};
function JCloneParam(obj, dataadpter, parameter) {
    var p1 = { param: {} };
    p1.param = parameter;
    var cloned = jQuery.extend(true, {}, obj, dataadpter);
    var cloned2 = jQuery.extend(true, cloned, p1);
    return cloned2;
};
function checkStartsWith(str, search) {
    let regex = new RegExp("^" + search),
        check = regex.test(str);
    return check;
}


function FilterColumns(colarr, filterarr) {
    return $.grep(colarr, function (e) { return $.inArray(e.datafield, filterarr) != -1; });
};

$.fn.DXjqxRootsMenu = function (param) {
    var strid = '#' + this.attr('id');
    var Ret = SchemeRoots(param.url, param.filter, function (Res) {
        Ret.MenuRecords = Ret.DataAdapter.getRecordsHierarchy('id', 'parentid', 'items', [{ name: 'Label', map: 'label' }]);
        param.param.source = Ret.MenuRecords;
        $(strid).jqxMenu(param.param);
        $(strid).data(param);
        $(strid).data(Ret);
        if (param.ComplettCallback) param.ComplettCallback({ retv: Ret.MenuRecords });
    });
    Ret.DataAdapter.dataBind();
};
$.fn.DXjqxRootsMenuReload = function (nparam) {
    var param = $(this).data();
    param.rootSelector = nparam.rootSelector;
    param.filter = nparam.filter;
    var Ret = SchemeRoots(param.url, param.filter, function (Res) {
        param.param.source = Ret.MenuRecords;
        $(this).jqxMenu(param.param);
        $(this).data(param);
        $(this).data(Ret);
    });
};

function GetSelectedRootsMenurecord(data, element) {
    id = element.id;
    var record;
    $.each(data.DataAdapter.originaldata, function () {
        if (this['id'] == id) {
            record = this;
            return false;
        }
    });
    return record;
}


function DXSetState(state, v) {
    if (v == undefined) {
        v = 'true';
    }
    $("[state='" + v + "']").each(function (index, element) {
        $(this).DXStateControl(state);
    });
};

$.fn.DXInitStateControl = function (param) {
    $(this).data(param);
};
$.fn.DXStateControl = function (Current) {
    pageVariable.StateMode = Current;
    var id = this.id;
    var data = $(this).data();
    try {
        var work = data[Current];
        if (work) {
            if (work.visible) {
                $(this).show();
            } else {
                $(this).hide();
            }
            $(this).prop("disabled", work.disabled);
            $(this).prop("readonly", work.readonly);
        }
    } catch (e) { }
};


function Titlemess(elem, message) {
    var title = elem.html();
    //var color = elem.css("background-color");
    elem.html(title + " [" + message + "] ");
    //elem.css("background-color", "white");
    setTimeout(function showIt2() {
        elem.html(title);
        //elem.css("background-color", color);
    }, 2000);
};


String.prototype.toProperCase = function () {
    return this.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
};
String.prototype.contains = function (it) { return this.indexOf(it) != -1; };


String.prototype.initCap = function () {
    return this.toLowerCase().replace(/(?:^|\s|_)[a-z]/g, function (m) {
        return m.toUpperCase();
    });
};

//// Picture corp and upload stb...

var UploadPic = {
    views: "views",
    crop_max_width: 400,
    crop_max_height: 400,
    jcrop_api: null,
    canvas: null,
    context: null,
    image: null,
    OrigFilleName: null,
    prefsize: null
};

function loadImage(input) {
    if (input.files && input.files[0]) {
        UploadPic.OrigFilleName = input.files[0].name;
        var reader = new FileReader();
        UploadPic.canvas = null;
        reader.onload = function (e) {
            UploadPic.image = new Image();
            UploadPic.image.onload = validateImage;
            UploadPic.image.src = e.target.result;
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
    if (UploadPic.canvas != null) {
        UploadPic.image = new Image();
        UploadPic.image.onload = restartJcrop;
        UploadPic.image.src = UploadPic.canvas.toDataURL('image/png');
    } else restartJcrop();
}
function restartJcrop() {
    if (UploadPic.jcrop_api != null) {
        UploadPic.jcrop_api.destroy();
    }
    $("#" + UploadPic.views).empty();
    $("#" + UploadPic.views).append("<canvas id=\"canvas\">");
    UploadPic.canvas = $("#canvas")[0];
    UploadPic.context = UploadPic.canvas.getContext("2d");
    UploadPic.canvas.width = UploadPic.image.width;
    UploadPic.canvas.height = UploadPic.image.height;
    UploadPic.context.drawImage(UploadPic.image, 0, 0);
    $("#canvas").Jcrop({
        onSelect: selectcanvas,
        onRelease: clearcanvas,
        boxWidth: UploadPic.crop_max_width,
        boxHeight: UploadPic.crop_max_height
    }, function () {
        UploadPic.jcrop_api = this;
    });
    clearcanvas();
}
function clearcanvas() {
    UploadPic.prefsize = {
        x: 0,
        y: 0,
        x2: Math.round(canvas.width),
        y2: Math.round(canvas.height),
        w: canvas.width,
        h: canvas.height,
    };
}
function selectcanvas(coords) {
    UploadPic.prefsize = {
        x: Math.round(coords.x),
        y: Math.round(coords.y),
        x2: Math.round(coords.x2),
        y2: Math.round(coords.y2),
        w: Math.round(coords.w),
        h: Math.round(coords.h)
    };
}
function applyCrop() {
    UploadPic.canvas.width = UploadPic.prefsize.w;
    UploadPic.canvas.height = UploadPic.prefsize.h;
    UploadPic.context.drawImage(UploadPic.image, UploadPic.prefsize.x, UploadPic.prefsize.y, UploadPic.prefsize.w, UploadPic.prefsize.h, 0, 0, UploadPic.canvas.width, UploadPic.canvas.height);
    validateImage();
}
function applyScale(scale) {
    if (scale == 1) return;
    UploadPic.canvas.width = UploadPic.canvas.width * scale;
    UploadPic.canvas.height = UploadPic.canvas.height * scale;
    UploadPic.context.drawImage(UploadPic.image, 0, 0, UploadPic.canvas.width, UploadPic.canvas.height);
    validateImage();
}
function applyRotate() {
    UploadPic.canvas.width = UploadPic.image.height;
    UploadPic.canvas.height = UploadPic.image.width;
    UploadPic.context.clearRect(0, 0, UploadPic.canvas.width, UploadPic.canvas.height);
    UploadPic.context.translate(UploadPic.image.height / 2, UploadPic.image.width / 2);
    UploadPic.context.rotate(Math.PI / 2);
    UploadPic.context.drawImage(UploadPic.image, -UploadPic.image.width / 2, -UploadPic.image.height / 2);
    validateImage();
}
function applyHflip() {
    UploadPic.context.clearRect(0, 0, UploadPic.canvas.width, UploadPic.canvas.height);
    UploadPic.context.translate(UploadPic.image.width, 0);
    UploadPic.context.scale(-1, 1);
    UploadPic.context.drawImage(UploadPic.image, 0, 0);
    validateImage();
}
function applyVflip() {
    UploadPic.context.clearRect(0, 0, UploadPic.canvas.width, UploadPic.canvas.height);
    UploadPic.context.translate(0, UploadPic.image.height);
    UploadPic.context.scale(1, -1);
    UploadPic.context.drawImage(UploadPic.image, 0, 0);
    validateImage();
}








/// --------------
/*
function replace() {
    var button = document.createElement("input");
    button.type = "button";
    button.id = "btnSaveXform_Orgstatus";
    button.value = "Save22";
    button.setAttribute('class', 'btn btn-primary btn-xs');
    // button.onclick = function () { myFunc(); return false; }
    var el = document.getElementById("btnSaveXform_Orgstatus");
    el.parentNode.replaceChild(button, el);
}
*/
//window.ProcessOccurencePlus = function (ocid) {
function ProcessOccurencePlus(ocid) {

    var maxo = $(ocid).data("xform-maxoccure");
    var blk = $(ocid).data("xform-ocblock");
    var parent = $(ocid).data("xform-ocparent");
    var formid = $('#' + blk).closest('form')[0].id;
    var roulename = formid + "rules";
    var lroule;
    try { lroule = eval(roulename); } catch (err) { }
    var reg = '' + blk + '_[0-9]_\\b';
    var Occurencs = $("div").filter(function () {
        if (this.id)
            return this.id.match(reg);
    });
    var ocidx = Occurencs.length + 2;
    var PartRoot = "" + ocid.id.replace(/oc_/, "") + "_" + ocidx + "_";
    AjaxSync(pageVariable.baseSiteURL + Xform_RenderPart_Url, {
        part: PartRoot
    }, function (Data) {
        $.each(Occurencs, function (index, value) {
            var aid = value.id.replace("blk_", "oc_");
            $("#" + aid).addClass("off");
        })
        if (Occurencs.length == 0) {
            $("#" + blk).after(Data.ret);
            $("#" + ocid.id + "_2_").removeClass("off");
        } else {
            var test = Occurencs[Occurencs.length - 1];
            $("#" + test.id).after(Data.ret);
            $("#" + ocid.id + "_" + ocidx + "_").removeClass("off");
        }
        var rules = JSON.parse('[' + Data.rules + ']');
        if (lroule) {
            lroule = $.merge(lroule, rules);
            $('#' + formid).jqxValidator({
                rules: JSON.parse(JSON.stringify(lroule)),
                hintType: "label"
            });
        }
    });
};
//window.ProcessOccurenceMinus = function (ocid) {
function ProcessOccurenceMinus(ocid) {
    var blk = $(ocid).data("xform-ocblock");
    var formid = $('#' + blk).closest('form')[0].id;
    var ocidx = blk.substring(blk.length - 2).slice(0, -1) - 1;
    var prev = ocid.id.substring(0, ocid.id.length - 3) + '_' + ocidx + '_';
    $("#" + prev).removeClass("off");
    var roulename = formid + "rules";
    var lroule = eval(roulename);
    $("#" + blk).remove();
    lroule = $.map(lroule, function (item, index) {
        if (document.getElementById(item.input.substr(1)) == null)
            return null;
        return item;
    });
    window[roulename] = lroule;
    $('#' + formid).jqxValidator({
        rules: JSON.parse(JSON.stringify(lroule)),
        hintType: "label"
    });
};

var XfromContext = {
    baseSiteURL: '\\',
    newControler: 'Ajax',
    workController: 'Person'
}


$.widget("custom.dxButton", {
    options: {
        name: '',
        context: null,
        frmid: null,

        // Callbacks
        change: null
    },
    _create: function (defunc) {
        this.options.frmid = this.element[0].id;
        this.options.frmid = this.element.context.id;
        this.Setclickporc(defunc);
    },
    _destroy: function () {
    },
    _setOptions: function (options) {
        this._super(options);
    },
    _setOption: function (key, value) {
        this._super(key, value);
    },
    Setclickporc: function (func) {
        $("#" + this.options.frmid).off('click');

        $("#" + this.options.frmid).unbind("click");

        $("#" + this.options.frmid).on('click', function () {
            if ($.isFunction(func)) func();
        });
    }
});

$.widget("custom.dxXform", {
    options: {
        name: '',
        context: null,
        root: '',
        refroot: '',
        template: null,
        selector: null,
        id_viewdiv: null,
        TempFormValue: {},
        Valid: true,
        frmid: null,
        table: null,
        dlgconfirm: null,

        // Callbacks
        change: null,
        SetComplet: null,
        ChangeComplet: null,
        NewComplet: null,
        CreateComplet: null,
        SaveComplet: null,
        InsertComplet: null,
        DeleteComplet: null,
        GetComplet: null,
        Valid: null,
        InValid: null
    },
    _create: function () {
        this.options.frmid = this.element[0].id;
        this.xdiv = $('<div id="divXformHtml_' + name + '"></div>', {}).appendTo(this.element);
        if ($.isFunction(this.options.CreateComplet)) this.options.CreateComplet();
    },
    _destroy: function () {
    },
    _setOptions: function (options) {
        this._super(options);
    },
    _setOption: function (key, value) {
        this._super(key, value);
    },
    New: function (root) {

        var self = this;
        this.options.root = root;
        var url = this.options.context.baseSiteURL + this.options.context.newControler + '/NewXform';
        var sdata = { root: this.options.root, refroot: this.options.refroot, formid: this.options.frmid, template: this.options.template };
        if (this.options.selector != null) sdata.selector = this.options.selector;
        $.ajax({
            cache: false,
            dataType: 'text json',
            url: url,
            data: sdata,
            async: true,
            type: "GET",
            context: self,
            success: function (data) {
                this.Set(data);
                $.each(this.options.TempFormValue, function (key, value) {
                    $("#" + this.options.frmid + " input[id*='" + key + "']").val(value);
                });
                this.options.TempFormValue = {};
                if ($.isFunction(this.options.NewComplet)) this.options.NewComplet();
            },
            error: function (xhr, status, error) {
                debugger;
            }
        });
    },
    Changeroot: function (data) {
        var self = this;
        var convtype = this.options.root;
        this.options.root = data.root;
        this.options.refroot = data.refroot;

        if (data.convtype === undefined) convtype = 'Empty';
        //-- 
        var url = this.options.context.baseSiteURL + this.options.context.newControler + '/Xform_ChangeRootPost';
        data.arg = { formid: this.options.frmid, NewXformRoot: this.options.root, NewXformRefRoot: this.options.refroot, XformRoot: convtype };
        o = {};
        o = this.element.serializeObject();
        if (data.arg) {
            $.each(data.arg, function (i, value) {
                o[i] = value;
            });
        }

        if (data.selector) o.selector = data.selector;
        $.ajax({
            type: "POST",
            url: url,
            content: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            context: self,
            data: o,
            success: function (data, status, xhr) {
                this.Set(data);
                if ($.isFunction(this.options.ChangeComplet)) this.options.ChangeComplet();
            },
            error: function (xhr, textStatus, errorThrown) {
                debugger;
            }
        });
    },
    _insertvalid: function (self, data) {
        var url = self.options.context.baseSiteURL + self.options.context.workController + '/InsertXform_' + self.options.name;
        o = {};
        o = self.element.serializeObject();
        if (data.arg) {
            $.each(data.arg, function (i, value) {
                o[i] = value;
            });
        }
        $.ajax({
            type: "POST",
            url: url,
            content: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            context: self,
            data: o,
            success: function (data, status, xhr) {
                if ($.isFunction(this.options.InsertComplet)) this.options.InsertComplet(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                debugger;
            }
        });
    },
    Insert: function (data) {
        try { fnOnBeforeInsert({}); } catch (err) { }
        var self = this;
        try {
            $("#" + this.options.frmid).on('validationSuccess', self._insertvalid(self, data)); 
            $("#" + this.options.frmid).jqxValidator('validate');
        } catch (err) { }

    },
    _deletevalid: function (self, id, cmd) {
        try { fnOnBeforeDelete({}); } catch (err) { }
        var self = this;
        var url = self.options.context.baseSiteURL + self.options.context.workController + '/DeleteXform_' + self.options.name;
        AjaxGet(url, {
            Id_Persons:id,
            cmd: cmd
        }, function (Data) {
            if ($.isFunction(self.options.DeleteComplet)) self.options.DeleteComplet();
        });
    },
    Delete: function (id) {
        var self = this;
        if (this.options.dlgconfirm) {
            this.options.dlgconfirm(
                function (confirm) {
                    if (confirm) {
                        if (confirm) {
                            var cmd = '';
                            if (confirm == 'close') cmd = 'close';

                            self._deletevalid(self, id, cmd);
                        }
                    } else {

                    }
                });
        } else {
            self._deletevalid(self, id, '');
        }
    },
    Get: function (data) {
        var self = this;
        var url = this.options.context.baseSiteURL + this.options.context.workController + '/GetXform_' + this.options.name;
        var sdata = { Id_Entity: data.id, table: this.options.table, Id_Xform: this.options.frmid };
        $.ajax({
            cache: false,
            dataType: 'text json',
            url: url,
            data: sdata,
            async: true,
            type: "GET",
            context: self,
            success: function (data) {
                this.Set(data);
                if ($.isFunction(this.options.GetComplet)) this.options.GetComplet();
            },
            error: function (xhr, status, error) {
                debugger;
            }
        });
    },
    _savevalid: function (self, data) {
        var url = self.options.context.baseSiteURL + self.options.context.workController + '/SaveXform_' + self.options.name;
        o = {};
        o = self.element.serializeObject();
        if (data.arg) {
            $.each(data.arg, function (i, value) {
                o[i] = value;
            });
        }
        $.ajax({
            type: "POST",
            url: url,
            content: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            context: self,
            data: o,
            success: function (data, status, xhr) {
                if ($.isFunction(this.options.SaveComplet)) this.options.SaveComplet();
            },
            error: function (xhr, textStatus, errorThrown) {
                debugger;
            }
        });
    },
    Save: function (data) {
        var self = this;
        try { fnOnBeforeSave({}); } catch (err) { }
        try {
            $("#" + this.options.frmid).on('validationSuccess', this._savevalid(self, data));
            $("#" + this.options.frmid).jqxValidator('validate');
        } catch (err) { }
    },
    Set: function (data) {
        this.xdiv.html(data.Xform);
        if (this.options.id_viewdiv != null) $("#" + this.options.id_viewdiv).html(data.XformView);
        $("#" + this.options.frmid + " .editor").jqte();
        JqwTransformWithRole(this.options.frmid);
        try { fnOnComplett(); } catch (err) { }
        var crules = {};
        try { crules = eval('' + this.element[0].id + 'rules'); } catch (err) { }
        try {
            $('#' + this.options.frmid).jqxValidator({
                onSuccess: this.options.Valid, // function () {  debugger;  },
                onError: this.options.InValid,     //function () {   debugger; },
                rules: JSON.parse(JSON.stringify(crules)),
                hintType: "label"
            });
        } catch (err) { }
    },
    test: function (id) {
        this.xdiv.html(id);
    },

});

$.widget("custom.dxRootcombo", {
    options: {
        DataAdapter: {},
        Source: {
            datatype: "json",
            datafields: [
            { name: 'id', type: 'decimal' },
            { name: 'parentid', type: 'decimal' },
            { name: 'value', type: 'string' },
            { name: 'Label', type: 'string' },
            { name: 'Table', type: 'string' },
            { name: 'RefRoots', type: 'string' }
            ],
            url: null,
            context: null,
            data: { filter: null }, // selector: null},
            async: true
        },
        CreateComplet: null,
        onChange: null,
        frmid: null,
        filter: null,
        selector: null
    },
    _create: function () {
        var self = this;
        this.options.frmid = this.element[0].id;

        this.options.Source.url = this.options.context.baseSiteURL + Xform_BuildRootSelector;
        this.options.Source.data.filter = this.options.filter;
        if (this.options.selector) this.options.Source.data.selector = this.options.selector;
        this.options.Source.context = self;

        this.options.DataAdapter = new $.jqx.dataAdapter(this.options.Source, {
            autoBind: false,
            loadComplete: function (c) {
                var Root = c.Res[0].value;

                $("#" + self.options.frmid).off('change');
                $("#" + self.options.frmid).on('change', function (event) {
                    var item = event.args.item;
                    Root = item.value;
                    if ($.isFunction(self.options.onChange)) self.options.onChange(Root);
                });
                if ($.isFunction(self.options.CreateComplet)) self.options.CreateComplet(Root);
            },
            loadError: function (xhr, status, error) {
            }
        });
        param = { source: this.options.DataAdapter, displayMember: "Label", valueMember: "id", selectedIndex: 0, height: 25, width: 270 };
        $("#" + this.options.frmid).jqxComboBox(param);


    },
    _destroy: function () {
    },
    _setOptions: function (options) {
        this._super(options);
    },
    _setOption: function (key, value) {
        this._super(key, value);
    }
});