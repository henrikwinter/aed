$.widget("dx.Xform", {
    Wvar:{ mode:null ,ajaxdata:null},
    options: {
        controller: '',
        actionpart: '',
        root: '',
        Table: '',
        refroot:'',
        formid: '',
        viewformid: '',
        savebuttonid: null,
        selector: 'Gschema',
        xformcallbackdata: null,
        savebuttoncallback: null,
        Valid: false,
        typecombo: null,
        typemenu: null,
        typedata: {}
    },
    _create: function () {
        var widget = this;
        if (this.options.typecombo!=null) {
            widget._SetTypeCombo();
        }
        if (this.options.typemenu != null) {
            widget._SetTypeMenu();
        }
        if (this.options.savebuttonid != null) {
            $("#" + widget.options.savebuttonid).off('click');
            $("#" + widget.options.savebuttonid).on('click', function (event) {
                var value = null;
                if ($.isFunction(widget.options.savebuttoncallback)) widget.Wvar.ajaxdata = widget.options.savebuttoncallback(widget.Wvar.mode);
                widget.ValidateAndSave(widget.Wvar.ajaxdata);

            });
        }
        this._inerdiv = $("<div>");
        this._inerdiv.addClass("form-horizontal");
        this._inerdiv.addClass("xblck0");
        $(this.element).append(this._inerdiv);
        widget._trigger("Complete", null, { mode: 'Create', value:null });
    },
    newform: function (root, refroot, formid, selector) {
        var widget = this;
        if (root === undefined) root = this.options.root;
        if (refroot === undefined) refroot = this.options.refroot;
        if (formid === undefined) formid = this.options.formid;
        if (selector === undefined) selector = this.options.selector;
        AjaxGet(pageVariable.baseSiteURL + 'Ajax' + const_ActionXform_New, {
            root: root,
            refroot: refroot,
            formid: formid,
            selector: selector,
        }, function (Data) {
            $(widget._inerdiv).html(Data.Xform);
            widget._processhtml();
            widget.Wvar.mode = 'insert'
            //widget._trigger("NewComplete", null, { value: Data });
            widget._trigger("Complete", null, { mode: 'New', value: Data });

        });
    },
    get: function (ajaxdata) {
        var widget = this;
        if (ajaxdata.Id_Xform === undefined) ajaxdata.Id_Xform = this.options.formid;
        AjaxGet(pageVariable.baseSiteURL + this.options.controller + const_ActionXform_Get + this.options.actionpart, ajaxdata, function (Data) {
            $(widget._inerdiv).html(Data.Xform);
            $("#" + widget.options.viewformid).html(Data.XformView);
            widget._processhtml();
            widget.Wvar.mode = 'save'
            //widget._trigger("GetComplete", null, { value: Data });
            widget._trigger("Complete", null, { mode: 'Get', value: Data });
        });
    },
    ValidateAndSave: function (ajaxdata) {
        this.Wvar.ajaxdata = ajaxdata;
        $("#" + this.element[0].id).jqxValidator('validate');
    },
    SaveForm: function (ajaxdata) {
        this.Wvar.ajaxdata = ajaxdata;
        if (this.Wvar.mode == 'insert') {
            this.insert();
        } else if (this.Wvar.mode == 'save') {
            this.save();
        } else {

        }

    },
    insert: function (ajaxdata) {
        var widget = this;
        try { fnOnBeforeInsert(); } catch (err) { }
        if (ajaxdata === undefined) ajaxdata = this.Wvar.ajaxdata;
        $("#" + this.element[0].id).mYPostFormNew(
            pageVariable.baseSiteURL + this.options.controller + const_ActionXform_Ins + this.options.actionpart,
            ajaxdata, function (Data) {
                    if (Data.Error.Errorcode == 0) {
                        widget.Wvar.mode = 'save';
                        widget._trigger("Complete", null, { mode: 'Insert', value: Data });
                    } else {
                        widget._trigger("Error", null, { mode: 'Insert', value: Data });
                    }
            }
        );
    },
    save: function (ajaxdata) {
        var widget = this;
        try { fnOnBeforeSave(); } catch (err) { }
        if (ajaxdata === undefined) ajaxdata = this.Wvar.ajaxdata;
        $("#" + this.element[0].id).mYPostFormNew(
            pageVariable.baseSiteURL + this.options.controller + const_ActionXform_Save + this.options.actionpart,
            ajaxdata, function (Data) {
                if (Data.success == false) widget._trigger("Error", null, { mode: 'Save', value: { Data: { Error: { Errormessage: 'Server Error' } } } });
                else {
                    if (Data.Error.Errorcode == 0) {
                        widget._trigger("Complete", null, { mode: 'Save', value: Data });
                    } else {
                        widget._trigger("Error", null, { mode: 'Save', value: Data });
                    }
                }
            }
        );
    },
    _SetTypeMenu: function () {
        var widget = this;
        if (this.options.typemenu.width === undefined) this.options.typemenu.width = 250;
        $("#" + this.options.typemenu.Id).DXjqxRootsMenu({
            url: pageVariable.baseSiteURL + const_TypemenuGet,
            filter:widget.options.typemenu.filter,
            param: {
                theme: pageVariable.Jqwtheme,
                showTopLevelArrows: true,
                width: this.options.typemenu.width
            },
            ComplettCallback: function (c) {
                
                widget.options.typedata = c;
                widget.options.root = c.Res[0].value;
                widget.options.Table = c.Res[0].Table;
                $("#" + widget.options.typemenu.Id).on('itemclick', function (event) {
                    widget.options.typemenu.selected = GetSelectedRootsMenurecord($(this).data(), event.args);
                    widget.newform(widget.options.typemenu.selected.value);
                });
                widget._trigger("Complete", null, { mode: 'TypeMenu', value: c });
            }
        });
    },
    _SetTypeCombo: function () {
        var widget = this;

        if (this.options.typecombo.width === undefined) this.options.typecombo.width = 250;
        $("#" + this.options.typecombo.Id).DXjqxRootsCombo({
            url: pageVariable.baseSiteURL + const_TypecomboGet,
            filter:this.options.typecombo.filter, 
            param: {
                theme: pageVariable.Jqwtheme,
                selectedIndex: 0,
                width: this.options.typecombo.width
            },
            ComplettCallback: function (c) {
                widget.options.typedata = c;
                //widget.options.root = c.Res[0].value;
                widget.options.root = (c.Res[0] === undefined) ? '' : c.Res[0].value;
                $("#" + widget.options.typecombo.Id).off('change');
                $("#" + widget.options.typecombo.Id).on('change', function (event) {
                    if (event.args) {
                        widget.options.typecombo.selected = event.args.item;
                        widget.newform(widget.options.typecombo.selected.value);
                    }
                });
                widget._trigger("Complete", null, { mode: 'TypeCombo', value: c });
            }
        });
    },
    _processhtml: function () {
        var widget = this;
        $("#" + this.element[0].id + " .editor").jqte();
        JqwTransformWithRole(this.element[0].id);
        try { fnOnComplett(this.options.xformcallbackdata); } catch (err) { }
        try {
            var lroule;
            try { lroule = eval(this.element[0].id + 'rules'); } catch (err) { }
            $("#" + this.element[0].id).jqxValidator({
                onSuccess: function () {
                    widget.options.Valid = true;
                    if (widget.Wvar.mode == 'insert') {
                        widget.insert();
                    } else {
                        widget.save();
                    }
                },
                onError: function () {
                    widget.options.Valid = false;
                },
                rules: JSON.parse(JSON.stringify(lroule)),
                hintType: "label"
            });
        } catch (err) {  }
    }
});