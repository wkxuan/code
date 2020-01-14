﻿function _Search() {

    var _this = this;
    
    this.vueObj;
    this.service = "";
    this.method = "";
    this.beforeVue = function () { }
    this.newCondition = function () { }
    this.IsValidSrch = function () {
        return true;
    }
    this.initSearchParam = function () { };
    this.popCallBack = function (data) { };
    this.mountedInit = function () { }
    this.vue = function VueOperate() {
        var options = {
            el: '#popSearch',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: ["condition", "result"],
                disabled: true,
                columns: [],
                data: [],
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100],
                pagedataCount: 0,
                pageSize: 10,
                currentPage: 1
            },
            mounted: function () {
                _this.mountedInit();
                this.initParam();
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

                    if (!_this.IsValidSrch())
                        return;
                    this.pageSize = 10;
                    this.currentPage = 1;
                    showList();
                },
                initParam: function () {
                    let param;
                    if (window.parent.editDetail != undefined)
                        param = window.parent.editDetail.screenParam.popParam;
                    else if (window.parent.search != undefined)
                        param = window.parent.search.screenParam.popParam;
                    else if (window.parent.define != undefined)
                        param = window.parent.define.screenParam.popParam;
                    else if (window.parent.defineDetail != undefined)
                        param = window.parent.defineDetail.screenParam.popParam;

                    for (let i in param) {
                        this.searchParam[i] = param[i];
                    }
                },
                callBack: function (data) {
                    if (window.parent.editDetail != undefined)
                        window.parent.editDetail.popCallBack(data)
                    else if (window.parent.search != undefined)
                        window.parent.search.popCallBack(data)
                    else if (window.parent.define != undefined)
                        window.parent.define.popCallBack(data);
                    else if (window.parent.splc != undefined)
                        window.parent.splc.popCallBack(data);                        
                    else if (window.parent.defineDetail != undefined)
                        window.parent.defineDetail.popCallBack(data);
                    else if (window.parent.DataView != undefined)
                        window.parent.DataView.popCallBack(data);
                    this.data = [];
                },
                qr: function (event) {
                    event.stopPropagation();
                    let data = {};
                    data.sj = this.$refs.selectData.getSelection();
                    if (!data.sj.length) {
                        this.$Message.info("请选择数据!");
                        return;
                    }
                    this.callBack(data);
                },
                rowdblclick: function (row, index) {
                    let data = {};
                    data.sj = [row];
                    this.callBack(data);
                },
                clear: function (event) {
                    event.stopPropagation();
                    this.data = [];
                    this.pagedataCount = 0;
                    _this.initSearchParam();
                    _this.newCondition();
                    this.initParam();
                    this.panelName = ["condition", "result"];
                },
                changePageCount: function (index) {
                    this.currentPage = index;
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                },
                changePageSizer: function (value) {
                    this.pageSize = value;
                    this.currentPage = 1;
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        this.vueObj = new Vue(options);

        function showList() {
            let ve = _this.vueObj;
            ve.initParam();
            let param = {};
            for (let item in ve.searchParam) {
                let p = ve.searchParam[item];
                if (Array.isArray(p)) {
                    if (p.length) {
                        param[item] = p.join(',');
                    }                    
                } else {
                    if (p) {
                        param[item] = p;
                    }
                }
            }
            ve.data = [];
            ve.pagedataCount = 0;
            ve.tbLoading = true;
            
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: param,
                PageInfo: {
                    PageSize: ve.pageSize,
                    PageIndex: ve.currentPage - 1
                },
                Success: function (data) {
                    ve.tbLoading = false;
                    if (data.rows.length > 0) {
                        ve.panelName = 'result';
                        ve.data = data.rows;
                        ve.pagedataCount = data.total;
                    }
                    else {
                        iview.Message.info("没有满足当前查询条件的结果!");
                    }
                },
                Error: function () {
                    ve.tbLoading = false;
                }
            })
        };
    }
    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
    };

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.initSearchParam();
        _this.vue();
    }, 100);
}
var search = new _Search();