

var $,window;

$(window).on('load', function () {
    
    
    $(".trigger_popup_fricc").click(function(){
       $('.hover_bkgr_fricc').show();
    });
    $('.hover_bkgr_fricc').click(function(){
        $('.hover_bkgr_fricc').hide();
    });
    $('.popupCloseButton').click(function(){
        $('.hover_bkgr_fricc').hide();
    });
});



$(window).on('load', function () {
    
    
    $(".trigger_popup_fricc").click(function(){
       $('.hover_bkgr_fricc-01').show();
    });
    $('.hover_bkgr_fricc').click(function(){
        $('.hover_bkgr_fricc-01').hide();
    });
    $('.popupCloseButton').click(function(){
        $('.hover_bkgr_fricc-01').hide();
    });
});



$(window).on('load', function () {
    
    
    $(".trigger_popup_fricc").click(function(){
       $('.hover_bkgr_fricc-02').show();
    });
    $('.hover_bkgr_fricc').click(function(){
        $('.hover_bkgr_fricc-02').hide();
    });
    $('.popupCloseButton').click(function(){
        $('.hover_bkgr_fricc-02').hide();
    });
});


$(window).on('load', function () {
    
    
    $(".trigger_popup_fricc").click(function(){
       $('.hover_bkgr_fricc-03').show();
    });
    $('.hover_bkgr_fricc').click(function(){
        $('.hover_bkgr_fricc-03').hide();
    });
    $('.popupCloseButton').click(function(){
        $('.hover_bkgr_fricc-03').hide();
    });
});

$(window).on('load', function () {
    
    
    $(".trigger_popup_fricc").click(function(){
       $('.hover_bkgr_fricc-04').show();
    });
    $('.hover_bkgr_fricc').click(function(){
        $('.hover_bkgr_fricc-04').hide();
    });
    $('.popupCloseButton').click(function(){
        $('.hover_bkgr_fricc-04').hide();
    });
});

$(".toggle-password").click(function() {

  $(this).toggleClass("fa-eye fa-eye-slash");
  var input = $($(this).attr("toggle"));
  if (input.attr("type") == "password") {
    input.attr("type", "text");
  } else {
    input.attr("type", "password");
  }
});

$(function () {
    showHidenav();
    
    $(window).scroll(function(){
        
        showHidenav();
        
    });
    function showHidenav() {
        if( $(window).scrollTop() > 50 ) {
            $("nav").addClass("white-nav-top");
            $(".navbar-brand img").attr("src", "img/pre-login/logo.png");
        }
        else{
            $("nav").removeClass("white-nav-top");
            $(".navbar-brand img").attr("src", "img/pre-login/top-logo.png");
        }
        
        
    }
});