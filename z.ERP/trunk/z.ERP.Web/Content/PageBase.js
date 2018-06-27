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
        
        $.nfinetab.addTabM(options.title, url);
    },
    Close: function (data) {
        if (window.opener && window.opener.WindowClose) {
            window.opener.WindowClose(data);
            window.close();
        }
    }
});
