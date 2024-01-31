

//$(function () { $("[data-toggle='tooltip']").tooltip(); });

//$('.form-control').jqxInput({});

if (!Appselector ) {
    $("#GmessageNotificationContainer").hide();
}

$("#Email,#Password").keypress(function () {
       
    if ($("#errorLogin").length ) {
        $("#errorLogin").remove();
    }
});

$("#Email,#Password").on('click', function (e) {
    if ($("#errorLogin").length) {
        $("#errorLogin").remove();
    }
})

$('#loginForm').jqxValidator({
    closeOnClick: true,
    hintType: 'label',
    animation: 'fade',
    animationDuration: 300,
    rules: [
    { input: '#Email', message: '§Error_Username_Required§', action: 'change', rule: 'required' },
    { input: '#Email', message: '§Error_User_Chars§', action: 'change', rule: 'length=2,42' },
    { input: '#Password', message: '§Error_Password_Required§', action: 'change', rule: 'required' },
    { input: '#Password', message: '§Error_Pw_Chars§', action: 'change', rule: 'length=4,22' }
    ]
});

$('#loginForm').on('validationError', function (event) { // Some code here. 
    return false;
});
$('#loginForm').on('validationSuccess', function (event) { // Some code here.
    $('#loginForm').submit();
});
$('#sendButton').on('click', function () {
    $('#loginForm').jqxValidator('validate');
});

$("#IdBodyStart").hide();
$("#IdBody").show();
