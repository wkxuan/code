function _Define() {
    //评估这个全局变量是否合适
    var _this = this;
    //界面打开的查询以及保存完之后调用的查询
    this.search = function () { }

    //vue之前的操作
    this.beforeVue = function () { }

    //vue操作
    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                isShowBRANCHID: _this.isShowBRANCHID,
                screenParam: _this.screenParam
            },
            mounted: function () {
                _this.search();
            },
            methods: {
                add: function (event) {
                    _this.dataParam = {};
                    ve.dataParam = _this.dataParam
                },
                save: function (event) {
                    _.Ajax('save', {
                        DefineSave: ve.dataParam
                    }, function (data) {
                        _this.search();
                        alert("成功");
                    });
                },
            }
        });
    }

    //初始化vue绑定的对象
    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.isShowBRANCHID = false;
    }

    //延时
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}

var define = new _Define();