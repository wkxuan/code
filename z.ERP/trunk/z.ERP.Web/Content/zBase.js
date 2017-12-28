////js的基础帮助类,这只是一个入口
(function (window, undefined) {
    var
    zQuery = function (selector, context) {
        return new zQuery.fn.init(selector);
    };
    //原型方法
    zQuery.fn = zQuery.prototype = {
        init: function (selector) {
            return $(selector);
        }
    }
    zQuery.fn.init.prototype = zQuery.fn;
    //扩展方法的入口
    zQuery.extend = zQuery.fn.extend = function () {
        var src, copyIsArray, copy, name, options, clone,
            target = arguments[0] || {},
            i = 1,
            length = arguments.length,
            deep = false;
        if (typeof target === "boolean") {
            deep = target;
            target = arguments[1] || {};
            i = 2;
        }
        if (typeof target !== "object" && !jQuery.isFunction(target)) {
            target = {};
        }
        if (length === i) {
            target = this;
            --i;
        }

        for (; i < length; i++) {
            if ((options = arguments[i]) != null) {
                for (name in options) {
                    src = target[name];
                    copy = options[name];
                    if (target === copy) {
                        continue;
                    }
                    if (deep && copy && (jQuery.isPlainObject(copy) || (copyIsArray = jQuery.isArray(copy)))) {
                        if (copyIsArray) {
                            copyIsArray = false;
                            clone = src && jQuery.isArray(src) ? src : [];

                        } else {
                            clone = src && jQuery.isPlainObject(src) ? src : {};
                        }
                        target[name] = jQuery.extend(deep, clone, copy);
                    } else if (copy !== undefined) {
                        target[name] = copy;
                    }
                }
            }
        }
        return target;
    };
    //这里兼容模块化js
    if (typeof module === "object" && module && typeof module.exports === "object") {
        module.exports = zQuery;
    } else {
        window.zQuery = window._ = zQuery;
        if (typeof define === "function" && define.amd) {
            define("zQuery", [], function () { return zQuery; });
        }
    }
})(window);

