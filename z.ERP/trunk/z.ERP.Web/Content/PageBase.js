/*
页面基础内容
*/
zQuery.extend({
    OpenPage: function (options) {
        if (arguments.length == 1 && typeof (options) == "string") {
            options = {
                url: options
            }
        }
        if (arguments.length > 1) {
            options = {
                url: arguments[0],
                callback: arguments[1]
            }
        }
        var options_default = {
            id: '',
            title: '',
            url: "",
            callback: null
        }
        options = $.extend(options_default, options);//处理参数

        var url = __BaseUrl + "/" + options.url;

        if (parent.layui.element) {

            //var data = $(window.parent.document.getElementsByClassName("layui-tab-title"));
            //var datali = data[0].innerHTML;
            //console.log(datali);
            //var layid = [];
            //for (var i = 0; i <= datali.length-1; i++) {
            //    var layidFirst = datali.indexOf("lay-id");
            //    console.log(layidFirst);
            //    datali = datali.substr(layidFirst, datali.length - 1);
            //    console.log(datali);
            //    //将得到的值push给数组id
            //    if (layidFirst == 0) {
            //        break;
            //    }
            //}

            //if (layid.length <= 0) {
            //    parent.layui.element.tabAdd('yxadmin', {
            //        id: options.id,
            //        title: options.title,
            //        content: '<iframe data-frameid="' + options.id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:900px;"></iframe>',
            //    });
            //} else {
            //    var isData = false;
            //    $.each(layid, function () {
            //        if (layid.id == options.id) {
            //            isData = true;
            //        }
            //    })
            //    if (isData == false) {
            //        parent.layui.element.tabAdd('yxadmin', {
            //            id: options.id,
            //            title: options.title,
            //            content: '<iframe data-frameid="' + options.id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:900px;"></iframe>',
            //        });
            //    }
            //}



            parent.layui.element.tabAdd('yxadmin', {
                id: options.id,
                title: options.title,
                content: '<iframe data-frameid="' + options.id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:900px;"></iframe>',
            });
            parent.layui.element.tabChange('yxadmin', options.id);
        }
        else {
            window.open(url);
        }

    },
    Close: function (data) {
        if (window.opener && window.opener.WindowClose) {
            window.opener.WindowClose(data);
            window.close();
        }
    }
});

//$(window).bind('unload', function () {
//    _.Close();
//});