
$(function () {
    //滑动验证
    jigsaw.init(document.getElementById('captcha'), imagesuccess, imagefail);
    function imagesuccess() {
        var username = $('.username').val();
        var password = $('.password').val();
        $("#msg").css("color","green");
        document.getElementById('msg').innerHTML = "验证成功！";
        login(username, password);
        return false;
    };
    function imagefail() {
        document.getElementById('msg').innerHTML = "验证错误，请重试！";
        cleartext();
    };
    function cleartext(){
        window.setTimeout(function () {

            document.getElementById('msg').innerHTML = " &nbsp;";;

        }, 1500);
    }
    //$('.page-container form').submit(function () {
    //    var username = $(this).find('.username').val();
    //    var password = $(this).find('.password').val();
    //    if (username == '') {
    //        $(this).find('.error').fadeOut('fast', function () {
    //            $(this).css('top', '27px');
    //        });
    //        $(this).find('.error').fadeIn('fast', function () {
    //            $(this).parent().find('.username').focus();
    //        });
    //        return false;
    //    }
    //    if (password == '') {
    //        $(this).find('.error').fadeOut('fast', function () {
    //            $(this).css('top', '96px');
    //        });
    //        $(this).find('.error').fadeIn('fast', function () {
    //            $(this).parent().find('.password').focus();
    //        });
    //        return false;
    //    }
    //    login(username, password);
    //    return false;
    //});
    //$('.page-container form .username, .page-container form .password').keyup(function () {
    //    $(this).parent().find('.error').fadeOut('fast');
    //});

    $.supersized({

        // Functionality
        slide_interval: 4000,    // Length between transitions
        transition: 1,    // 0-None, 1-Fade, 2-Slide Top, 3-Slide Right, 4-Slide Bottom, 5-Slide Left, 6-Carousel Right, 7-Carousel Left
        transition_speed: 2000,    // Speed of transition
        performance: 1,    // 0-Normal, 1-Hybrid speed/quality, 2-Optimizes image quality, 3-Optimizes transition speed // (Only works for Firefox/IE, not Webkit)

        // Size & Position
        min_width: 0,    // Min width allowed (in pixels)
        min_height: 0,    // Min height allowed (in pixels)
        vertical_center: 1,    // Vertically center background
        horizontal_center: 1,    // Horizontally center background
        fit_always: 0,    // Image will never exceed browser width or height (Ignores min. dimensions)
        fit_portrait: 1,    // Portrait images will not exceed browser height
        fit_landscape: 0,    // Landscape images will not exceed browser width

        // Components
        slide_links: 'blank',    // Individual links for each slide (Options: false, 'num', 'name', 'blank')
        slides: [    // Slideshow Images
                                 { image: __PortalBaseUrl + "/Content/Login/img/backgrounds/loginimg.jpg" },
                                 //{ image: _.buildUrl("Themes/default/Login/img/backgrounds/2.jpg") },
                                 //{ image: _.buildUrl("Themes/default/Login/img/backgrounds/3.jpg") },
                                 //{ image: _.buildUrl("Themes/default/Login/img/backgrounds/4.jpg") }
        ]

    });

    //登陆
    function login(username, password) {
        _.Ajax("Starts", {
            LoginName: username,
            PassWord: password
        },
        function (data) {
            switch (data) {
                case "0": window.location.href = __PortalBaseUrl + "/HOME/Index/Index"; break;
                case "1": document.getElementById('msg').innerHTML = "账号错误，请重试！"; $("#msg").css("color", "red"); cleartext(); $('.refreshIcon').click(); break;
                case "2": document.getElementById('msg').innerHTML = "密码错误，请重试！"; $("#msg").css("color", "red"); cleartext(); $('.refreshIcon').click(); break;
                case "3": document.getElementById('msg').innerHTML = "该账号已停用！"; $("#msg").css("color", "red"); cleartext(); $('.refreshIcon').click(); break;
                default: document.getElementById('msg').innerHTML = "请联系管理员！"; $("#msg").css("color", "red"); cleartext(); $('.refreshIcon').click();
            }
        });
    }
});
