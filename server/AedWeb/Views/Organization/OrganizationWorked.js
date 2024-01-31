$("#RootSelector").dxRootcombo({
	context: XCt,
	filter: 'OrgRoots',
	CreateComplet: function (Root) {},
	onChange: function (Root) {
		$("#Xform").dxXform('Changeroot', {
			root: Root
		});
	}
});


$("#Xform").dxXform({
	context: XCt,
	name: 'Organization',
	id_viewdiv: 'CommonView',
	dlgconfirm: dlgAreYouSoure,
	GetComplet: function () {
		$("#Save").dxButton('Setclickporc', function () {
			$("#Xform").dxXform('Save', {
				arg: {
					Id_Entity: Id_Entity
				}
			});
		});
		$("#Save").prop('disabled', false);
		$("#CommonView").html("Organization")
	},
	SaveComplet: function () {
		$("#EditModal").modal('hide');
		$("#Xform").dxXform('Get', {
			id: Id_Entity
		});
		ReloadAll();
	},
	NewComplet: function () {
		$("#New").prop("disabled", true);
		$("#Save").prop('disabled', false);
		$("#EditModal").modal('show');
		var id_parent = $("#chkInsertPosition").is(':checked') ? Id_Parent : Id_Entity;
		$("#Save").dxButton('Setclickporc', function () {
			$("#Xform").dxXform('Insert', {
				arg: {
					Id_Parent: id_parent,
					Recordtype: 'OrgItem'
				}
			});
		});
	},
	InsertComplet: function () {
		$("#EditModal").modal('hide');
		ReloadAll();
	},
	DeleteComplet: function () {
		ReloadAll();
	},
	InValid: function () {}
});


$("#Save").dxButton();

$("#New").on('click', function (e) {
	$("#Xform").dxXform('New', 'GeneralOrg');
});

$("#Edit").on('click', function (e) {
	if (Id_Entity > 0) {
		$("#Xform").dxXform('Get', {
			id: Id_Entity
		});
		$("#EditModal").modal('show');
	} else {
		GlobError('No selected!', 11);
	}
});

$("#Delete").on('click', function (e) {});
