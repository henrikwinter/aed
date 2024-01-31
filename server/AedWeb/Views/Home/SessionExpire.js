
$("#dashnav").html('');
var navlink = '<li class="nav-item"><a href="/Home/Index" class="nav-link">Home</a></li>';
$("#dashnav").append(navlink);
navlink = '<li class="nav-item"><a href="/Account/Login" class="nav-link">Login</a></li>';
$("#dashnav").append(navlink);

$("#IdBodyStart").hide();
$("#IdBody").show();