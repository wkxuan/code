/*
ajax相关的内容



*/
zQuery.extend({
    AjaxUrl: __ControllerUrl,
    CommonAjaxUrl: __CommonControllerUrl,
    ShareAjaxUrl: __BaseUrl + 'Areas.Share/',
    doResult: function (retdata, success, error) {
        if (retdata && retdata.Flag != undefined) {
            if (retdata.Flag >= 0) {
                switch (retdata.Flag) {
                    case 0: {
                        success && success(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    case 100: {//没有登陆
                        LoginOut();
                        error && error(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    case 101: { //数据库异常
                        alert(retdata.Msg);
                        error && error(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    case 102: {//逻辑异常
                        alert(retdata.Msg);
                        error && error(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    case 103: {//没有登陆
                        alert(retdata.Msg);
                        error && error(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    case 104: {//没有权限
                        alert(retdata.Msg);
                        error && error(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                    default: {
                        success && success(retdata.Obj, retdata.Flag, retdata.Msg);
                        break;
                    }
                }
            }
            else {
                alert(retdata.Msg);
                error && error(retdata.Obj, retdata.Flag, retdata.Msg);
            }
        }
        else {
            success && success(retdata, 0, '');
        }
    },
    Ajax: function (options) {
        if (arguments.length == 1) {
            if (!options.action && !options.url) {
                alert("请求没有action");
                debugger;
                return;
            }
        }
        if (arguments.length > 1) {
            options = {
                action: arguments[0],
                data: arguments[1],
                success: arguments.length >= 2 ? arguments[2] : null,
                error: arguments.length >= 3 ? arguments[3] : null
            }
        }
        //if (typeof options.data == 'object')
        //    $.each(options.data, function (inx, obj) {
        //        if (typeof obj == 'object')
        //            options.data[inx] = JSON.stringify(obj);
        //    });
        var options_default = {
            type: "Post",
            url: _.AjaxUrl + options.action,
            data: null,
            async: true,
            cache: false,
            dataType: "json",
            success: function (data) {

            },
            error: function () {

            }
        }
        options = $.extend(options_default, options);//处理参数
        $.ajax({
            type: options.type,
            url: options.url,
            data: options.data,
            async: options.async,
            cache: options.cache,
            dataType: options.dataType,
            success: function (retdata) {
                _.doResult(retdata, function (obj, flag, msg) {
                    options.success && options.success(obj, flag, msg);
                }, function (obj, flag, msg) {
                    options.error && options.error(obj, flag, msg);
                });
            },
            error: function (retdata) {
                alert("请求出错:" + retdata.status);
                options.error && options.error(retdata, -1, '');
            }
        });
    },
    Search: function (Options) {
        if (!Options.Service || !Options.Method) {
            alert("必要的参数Service,Method");
            return;
        }
        if (typeof Options.Data == 'object')
            $.each(Options.Data, function (inx, obj) {
                if (typeof obj == 'object')
                    Options.Data[inx] = JSON.stringify(obj);
            });
        this.Ajax({
            url: _.CommonAjaxUrl + "Search",
            data: {
                Service: Options.Service,
                Method: Options.Method,
                Data: { Values: Options.Data }
            },
            success: function (data) {
                Options.Success && Options.Success(data);
            }
        });
    },
    SearchNoQuery: function (Options) {
        if (!Options.Service || !Options.Method) {
            alert("必要的参数Service,Method");
            return;
        }
        this.Ajax({
            url: _.CommonAjaxUrl + "SearchNoQuery",
            data: {
                Service: Options.Service,
                Method: Options.Method
            },
            success: function (data) {
                Options.Success && Options.Success(data);
            }
        });
    }
});