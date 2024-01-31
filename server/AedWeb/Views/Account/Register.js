


var teste = $("#formselectore").val();
var testg = $("#formselectorg").val();
var testp = $("#formselectorp").val();

if (teste == "frmEmploy") $('.nav-tabs a[href="#home"]').tab('show');
if (testg == "frmGuest") $('.nav-tabs a[href="#profile"]').tab('show');
if (testp == "frmPerson") $('.nav-tabs a[href="#person"]').tab('show');


$('#divreglayer div').click(function () {
    $("#listErrors").empty();
    $("#ContributionErrortxt").hide();
    $('#frmGuest').jqxValidator('hide');
    $('#frmEmploy').jqxValidator('hide');
    $("#ContributionTxt").show();
});
$('#chkbContribution').change(function () {
    if ($(this).is(":checked")) {

    } else {

    }
    $("#ContributionErrortxt").hide();
});
function postvalidate() {
    if ($('#chkbContribution').prop('checked')) {
        return true;
    }
    return false;
}



$('#frmEmploy').jqxValidator({
    closeOnClick: true,
    hintType: 'label',
    animation: 'fade',
    animationDuration: 300,
    rules: [
    { input: '#FirstName', message: '§Error_Username_Required§', action: 'change', rule: 'required' },
    { input: '#LastName', message: '§Error_Username_Required§', action: 'change', rule: 'required' },
    { input: '#Email', message: '§Error_Bademail§', action: 'change', rule: 'email' },
    { input: '#Email', message: '§Error_User_Chars§', action: 'change', rule: 'required' },
    { input: '#Password', message: '§Error_Password_Required§', action: 'blur', rule: 'required' },
    { input: '#Password', message: '§Error_Pw_Chars§', action: 'blur', rule: 'length=4,12' },
    { input: '#ConfirmPassword', message: '§Error_Password_Required§', action: 'change', rule: 'required' },
    { input: '#ConfirmPassword', message: '§Error_Pw_Chars§', action: 'change', rule: 'length=4,12' },
    { input: '#Phonenum', message: '§Error_Phonenum_Required§', action: 'change', rule: 'required' },
    { input: '#Answer', message: '§Error_Answer_Required§', action: 'change', rule: 'required' },
    {
        input: '#Question', message: '§Error_Question_Required§', action: 'select', rule: function (input) {
            if (input[0].value == "Select you want to use Function!") return false;
            return true;
        }
    }
    ]
});

$('#frmGuest').jqxValidator({
    closeOnClick: true,
    hintType: 'label',
    animation: 'fade',
    animationDuration: 300,
    rules: [
    { input: '#NickName', message: '§Error_Nickname_Required§', action: 'change', rule: 'required' },
    { input: '#Email_g', message: '§Error_Bademail§', action: 'change', rule: 'email' },
    { input: '#Email_g', message: '§Error_User_Chars§', action: 'change', rule: 'required' },
    { input: '#Password_g', message: '§Error_Password_Required§', action: 'blur', rule: 'required' },
    { input: '#Password_g', message: '§Error_Pw_Chars§', action: 'blur', rule: 'length=4,12' },
    { input: '#ConfirmPassword_g', message: '§Error_Password_Required§', action: 'change', rule: 'required' },
    { input: '#ConfirmPassword_g', message: '§Error_Pw_Chars§', action: 'change', rule: 'length=4,12' }
    ]
});

$('#frmPerson').jqxValidator({
    closeOnClick: true,
    hintType: 'label',
    animation: 'fade',
    animationDuration: 300,
    rules: [
    { input: '#PersonName', message: '§Error_Personname_Required§', action: 'change', rule: 'required' },
    { input: '#Email_p', message: '§Error_Bademail§', action: 'change', rule: 'email' },
    { input: '#Email_p', message: '§Error_User_Chars§', action: 'change', rule: 'required' },
    { input: '#Password_p', message: '§Error_Password_Required§', action: 'blur', rule: 'required' },
    { input: '#Password_p', message: '§Error_Pw_Chars§', action: 'blur', rule: 'length=4,12' },
    { input: '#ConfirmPassword_p', message: '§Error_Password_Required§', action: 'change', rule: 'required' },
    { input: '#ConfirmPassword_p', message: '§Error_Pw_Chars§', action: 'change', rule: 'length=4,12' }
    ]
});


$('#frmEmploy').on('validationError', function (event) { // Some code here. 
    return false;
});
$('#frmEmploy').on('validationSuccess', function (event) { // Some code here.
    $('#frmEmploy').submit();
});
$('#btnSendEmploy').on('click', function () {
    if (!postvalidate()) {
        $("#ContributionErrortxt").show();
        return false;
    }
    $("#formselectore").val('frmEmploy');
    $("#formselectorg").val('frmEmploy');
    $("#formselectorp").val('frmEmploy');
    $('#frmEmploy').jqxValidator('validate');
    return false;
});

$('#frmGuest').on('validationError', function (event) { // Some code here. 
    return false;
});
$('#frmGuest').on('validationSuccess', function (event) { // Some code here.
    $('#frmGuest').submit();
});
$('#btnSendGuest').on('click', function () {
    if (!postvalidate()) {
        $("#ContributionErrortxt").show();
        return false;
    }
    $("#formselectore").val('frmGuest');
    $("#formselectorg").val('frmGuest');
    $("#formselectorp").val('frmGuest');
    $('#frmGuest').jqxValidator('validate');
    return false;
});

$('#frmPerson').on('validationError', function (event) { // Some code here. 
    return false;
});
$('#frmPerson').on('validationSuccess', function (event) { // Some code here.
     $('#frmPerson').submit();
});
$('#btnSendPerson').on('click', function () {
    if (!postvalidate()) {
        $("#ContributionErrortxt").show();
        return false;
    }
    $("#formselectore").val('frmPerson');
    $("#formselectorg").val('frmPerson');
    $("#formselectorp").val('frmPerson');
    $('#frmPerson').jqxValidator('validate');
    return false;
});


$("#IdBodyStart").hide();
$("#IdBody").show();
