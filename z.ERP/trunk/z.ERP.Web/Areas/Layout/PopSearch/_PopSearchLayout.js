function _Search() {

    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.newCondition = function () { }

    this.IsValidSrch = function () {
        return true;
    }
    this.canEdit = function (mess) {
        return true;
    }
    this.popInitParam = function (data) { }

    this.popCallBack = function (data) { };

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: 'condition',
                disabled: _this.enabled(true),

                screenParamData: {
                    dataDef: []
                }
            },
            mounted: function () {
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    var mess = this;

                    if (!_this.IsValidSrch())
                        return;
                    Vue.set(ve.screenParamData, "dataDef", []);
                    //父页面是单据
                    if (window.parent.editDetail != undefined)
                        _this.popInitParam(window.parent.editDetail.screenParam.popParam);
                        //父页面是查询
                    else if (window.parent.search != undefined)
                        _this.popInitParam(window.parent.search.screenParam.popParam);
                        //父页面是简单定义
                    else if (window.parent.define != undefined)
                        _this.popInitParam(window.parent.define.screenParam.popParam);
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });
                },
                qr: function (event) {
                    event.stopPropagation();
                    var data = {};
                    data.sj = this.$refs.selectData.getSelection();
                    this.Data = [];
                    this.$refs.selectData.makeObjData();
                    //this.$emit('setdialog', data)
                    if (window.parent.editDetail != undefined)
                        window.parent.editDetail.popCallBack(data)
                    else if (window.parent.search != undefined)
                        window.parent.search.popCallBack(data)
                    else if (window.parent.define != undefined)
                        window.parent.define.popCallBack(data);
                    //清空查询结果
                    Vue.set(ve.screenParamData, "dataDef",[]);
                    //localStorage.setItem("relt", data);
                    //var site = localStorage.getItem("relt");
                },
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.screenParamData.dataDef = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showList(callback) {
            ve.searchParam = _this.searchParam;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                Success: function (data) {
                    _this.screenParam.dataDef = data.rows;
                    callback && callback();
                }
            })
        };
        function notExistsData() {
            return (!ve.screenParamData.dataDef) || (ve.screenParamData.dataDef.length == 0)
        }
    }

    this.addHref = function () { }

    this.modHref = function (row, index) { }

    this.browseHref = function (row, index) { }

    this.colDefInit = function () {
        _this.colMul = [{
            type: 'selection',
            width: 60,
            align: 'center',
            fixed: 'left',
        }];
    }

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
    };

    setTimeout(function () {
        _this.colDefInit();
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var search = new _Search();