function _Define() {
    var _this = this;

    //vue操作之前的方法
    this.search = function () { }
    this.beforeVue = function () { }

    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                isShowBRANCHID: _this.isShowBRANCHID,
            },
            mounted: function () {
                _this.search();
            },
            methods: {
                add: function (event) {
                    ve.dataParam = {};
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

    this.vueInit = function () {
        _this.dataParam = {};
        _this.isShowBRANCHID = false;
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}

var define = new _Define();