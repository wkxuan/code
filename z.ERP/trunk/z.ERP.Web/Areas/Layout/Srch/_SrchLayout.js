function _Srch() {

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
    this.mountedInit = function () { }
    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo:_this.pageInfo,
                panelName: 'condition',
                disabled: _this.enabled(true),

                screenParamData: {
                    dataDef: []
                },
                pagedataCount: 0,
                pageSize: 10,
            },

            mounted: function () {
                _this.mountedInit();        
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    var mess = this;
                    if (!_this.IsValidSrch())
                        return;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = 0;
                    Vue.set(ve.screenParamData, "dataDef", []);
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

                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.screenParamData.dataDef = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要导出的数据!");
                    } else {
                        this.$Message.error("尚未提供导出方法!");
                    }

                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要打印的数据!");
                    } else {
                        this.$Message.error("尚未提供打印方法!");
                    }

                },
                changePageCount: function (index) {
                    let mess = this;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = (index - 1);

                    Vue.set(ve.screenParamData, "dataDef", []);
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });

                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showList(callback) {
            ve.searchParam = _this.searchParam;
            ve.pageInfo=_this.pageInfo;

            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                PageInfo:ve.pageInfo,
                Success: function (data) {
                    _this.screenParam.dataDef = data.rows;
                    ve.pagedataCount = data.total;
                    callback && callback();
                }
            })
        };
        function notExistsData() {
            return (!ve.screenParamData.dataDef) || (ve.screenParamData.dataDef.length == 0)
        }
    }






    this.vueInit = function () {
        _this.pageInfo = {};
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
    };

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var srch = new _Srch();