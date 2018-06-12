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

        var url = __BaseUrl + "/" + options.url;

        if (parent.layui.element) {

            if ($(".layui-tab-title li[lay-id]").length <= 0) {
                parent.layui.element.tabAdd('yxadmin', {
                    id: options.id,
                    title: options.title,
                    content: '<iframe data-frameid="' + options.id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:900px;"></iframe>',
                });
            } else {
                var isData = false;
                $.each($(".layui-tab-title li[lay-id]"), function () {
                    if ($(this).attr("lay-id") == options.id) {
                        isData = true;
                    }
                })
                if (isData == false) {
                    parent.layui.element.tabAdd('yxadmin', {
                        id: options.id,
                        title: options.title,
                        content: '<iframe data-frameid="' + options.id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:900px;"></iframe>',
                    });
                }
            }
            parent.layui.element.tabChange('yxadmin', options.id);
        }
        else {
            window.open(url);
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