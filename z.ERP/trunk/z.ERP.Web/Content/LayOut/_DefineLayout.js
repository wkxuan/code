function _Define() {
    var _this = this;

    //vue操作之前的方法
    this.beforeVue = function () { }

    //数据保存之前
    this.beforeSave = function () { }

    //数据维护之后的方法
    this.afterSave = function () { }

    this.afterAdd = function () { }


    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                editData: {}
            },
            //mounted: function () {
            //},
            methods: {
                add: function (event) {
                    ve.editData = {};
                    _this.afterAdd();
                },
                save: function (event) {
                    _this.beforeSave();
                    _.Ajax('Save', {
                        saveParam: ve.editData
                    }, function (callback) {
                        _this.afterSave(callback);
                        ve.$Message.info({
                            title: '结果',
                            content: '保存成功'
                        });
                    });
                }
            }
        });
    }

    setTimeout(function () {
        _this.beforeVue();
        _this.vue();
    }, 10);
}
var define = new _Define();