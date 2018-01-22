function _EditDetail() {

    var _this = this;

    this.beforeVue = function () { }

    //校验保存前数据是否合法,前端验证，非空的可以加上
    this.IsValidSave = function (_self) {
        return true;
    }

    //控件是否可用，扩展函数,待完善
    this.enabled = function (val) { return val; }

    //为了清空非BILLID主键表的主键,或者可以根据需要清空想清空的内容,节间可以实现复制功能
    this.clearKey = function () { }

    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#EditDetail',
            data: {
                dataParam: _this.dataParam,
                windowParam: _this.windowParam,
                screenParam: _this.screenParam,
                panelName: 'base',
                branchid: _this.branchid,
                others: _this.others,
                disabled: _this.enabled(true),
            },
            methods: {
                add: function (event) {
                    //新增暂时先将单号清空
                    _this.dataParam.BILLID = null;
                    _this.clearKey();
                    ve.dataParam = _this.dataParam;
                },
                //保存
                save: function (event) {
                    var _self = this;
                    if (!_this.IsValidSave(_self))
                        return;
                    save(function (data) {
                        _this.showOne(data, function () {
                            ve.dataParam = _this.dataParam;
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
    }

    this.Id = "";

    this.showOne = function (data, callback) {
    }

    this.vueInit = function () {
        _this.dataParam = { NAME: '' };
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.branchid = true;
        _this.others = true;
    };

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
        if (editDetail.Id) {
            editDetail.showOne(editDetail.Id);
        }
    }, 100);
}
var editDetail = new _EditDetail();