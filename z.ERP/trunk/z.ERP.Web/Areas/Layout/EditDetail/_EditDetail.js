function _EditDetail() {

    var _this = this;

    this.beforeVue = function () { }

    //校验保存前数据是否合法,前端验证，非空的可以加上
    this.IsValidSave = function (_self) {
        return true;
    }

    //控件是否可用，扩展函数,待完善
    this.enabled = function (val) { return val; }

    //得到单号
    this.Key = undefined

    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#EditDetail',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                stepParam: _this.stepParam,
                panelName: 'base',
                branchid: _this.branchid,
                others: _this.others,
                stepsCurrent: _this.stepsCurrent,
                disabled: _this.enabled(true),
            },
            methods: {
                //保存
                save: function (event) {
                    var _self = this;
                    if (!_this.IsValidSave(_self))
                        return;
                    save(function (data) {
                        showone(data, function () {
                            _self.$Message.info("保存成功");
                        });
                    })
                },
            }
        });
        function save(callback) {
            _.Ajax('Save', {
                SaveData: ve.dataParam
            }, function (data) {
                callback && callback(data);
            });
        }
        function showone(key, callback) {
            if (key) {
                var v = {};
                v[_this.Key] = key;
                _.Search({
                    Service: _this.service,
                    Method: _this.method,
                    Data: v,
                    Success: function (data) {
                        _this.dataParam = data.rows[0];
                        ve.dataParam = _this.dataParam;
                        callback && callback();
                    }
                });
            }
        }
    }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.stepParam = [];
        _this.service = "";
        _this.method = "";
        _this.branchid = true;
        _this.others = true;
        _this.stepsCurrent = 0;
    };

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var editDetail = new _EditDetail();