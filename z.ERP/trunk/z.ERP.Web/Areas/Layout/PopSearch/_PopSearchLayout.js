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

    this.mountedInit = function () { }

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: 'condition',
                disabled: _this.enabled(true),
                columns: [],
                data: [],
                maxHeight: 240,
            },
            mounted: function () {
                _this.mountedInit();
            },
            watch: {
                "screenParam.colDef": {
                    handler: function (nv, ov) {
                        this.columns = nv;
                    },
                    immediate: true,
                    deep: true
                }
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    let _self = this;

                    if (!_this.IsValidSrch())
                        return;
                    this.data = [];
                    //父页面是单据
                    if (window.parent.editDetail != undefined)
                        _this.popInitParam(window.parent.editDetail.screenParam.popParam);
                        //父页面是报表
                    else if (window.parent.srch != undefined)
                        _this.popInitParam(window.parent.srch.screenParam.popParam);
                        //父页面是查询
                    else if (window.parent.search != undefined)
                        _this.popInitParam(window.parent.search.screenParam.popParam);
                        //父页面是简单定义
                    else if (window.parent.define != undefined)
                        _this.popInitParam(window.parent.define.screenParam.popParam);

                    _self.searchParam = _this.searchParam;
                    _.Search({
                        Service: _this.service,
                        Method: _this.method,
                        Data: _self.searchParam,
                        Success: function (data) {
                            if (data.rows.length > 0) {
                                _self.panelName = 'result';
                                _self.data = data.rows;
                            }
                            else {
                                _self.$Message.info("没有满足当前查询条件的结果!");
                            }
                        }
                    });
                },
                qr: function (event) {
                    event.stopPropagation();
                    let data = {};
                    data.sj = this.$refs.selectData.getSelection();
                    if (!data.sj.length) {
                        this.$Message.info("请选择数据!");
                        return;
                    }
                    if (window.parent.editDetail != undefined)
                        window.parent.editDetail.popCallBack(data)
                    else if (window.parent.srch != undefined)
                        window.parent.srch.popCallBack(data)
                    else if (window.parent.search != undefined)
                        window.parent.search.popCallBack(data)
                    else if (window.parent.define != undefined)
                        window.parent.define.popCallBack(data);
                    else if (window.parent.splc != undefined)
                        window.parent.splc.popCallBack(data);
                    this.data = [];
                },
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    this.searchParam = _this.searchParam;
                    this.data = [];
                    this.panelName = 'condition';
                    _this.newCondition();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function notExistsData() {
            return (!ve.data) || (ve.data.length == 0)
        }
    }

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