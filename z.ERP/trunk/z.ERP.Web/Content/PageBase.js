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
            self: false,
            callback: null
        }
        options = $.extend(options_default, options);//处理参数

        var isAbUrl = function (str) {
            return str.match(/http:\/\/.+/) != null;
        }

        var url = isAbUrl(options.url) ? options.url : (__BaseUrl + "/" + options.url);
        options.url = url;

        if (!options.self && parent && parent._ && parent._.OpenPage) {
            options.self = true;
            parent._.OpenPage(options);
        }
        else {
            window.open(options.url);
        }
    },
    Close: function (data) {
        if (window.opener && window.opener.WindowClose) {
            window.opener.WindowClose(data);
            window.close();
        }
    }
});
