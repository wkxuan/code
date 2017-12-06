function _Define() {
    var _this = this;

    //vue操作之前的方法
    this.beforeVue = function () { }

    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                isShowBRANCHID: _this.isShowBRANCHID,
            },
            methods: {
                add: function (event) {
                    ve.dataParam = {};
                },
                save: function (event) {
                    _.Ajax('Save', {
                        data: ve.dataParam
                    }, function (a, b, c) {
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
    }, 10);
}

var define = new _Define();