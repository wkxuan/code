﻿/*
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

        if ($.nfinetab) {
            $.nfinetab.addTabM(options.title, options.url);
        }
        else {
            window.open(options.url);
        }


        //window["WindowClose"] = function (data) {
        //    options.callback && options.callback(data);
        //}
        //if (window.parent.navTab) {
        //    window.parent.navTab.openTab('' + options.id, url, { title: options.title, fresh: true, data: {}, external: true })
        //}
        //else {
        //    window.open(url);
        //}
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