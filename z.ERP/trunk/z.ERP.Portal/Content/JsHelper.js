////js的帮助类
(function (window, undefined) {
    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, '');
    }
})(window);

