function _Search() {
    var _this = this;
    this.beforeVue = function () { }
    this.enabled = function (val) { return val; }
    this.newCondition = function () { }
    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: 'condition',
                disabled: _this.enabled(true),
            },
            methods: {


                //查询
                seach: function (event) {
                    _this.newCondition();
                    ve.panelName = 'result';
                },
                //清空
                clear: function (event) {
                },
                //导出
                exp: function (event) {
                },
                //取消
                print: function (event) {
                },
            }
        });
    }
    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
    }
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var search = new _Search();