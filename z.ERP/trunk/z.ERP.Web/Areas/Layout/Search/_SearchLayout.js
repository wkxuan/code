function _Search() {

    var _this = this;
    this.vueObj;

    this.beforeVue = function () { }

    this.newCondition = function () { }

    this.enabled = function (val) { return val; }

    this.IsValidSrch = function () {
        return true;
    }
    this.service = "";
    this.method = "";
    this.mountedInit = function () { }

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo: _this.pageInfo,
                disabled: _this.enabled(true),
                panelName: ["condition", "result"],
                columns: [],
                data: [],
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100],
                pagedataCount: 0,
                pageSize: 10,
                currentPage: 1,
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
            mounted: function () {
                _this.mountedInit();
            },
            methods: {
                search: function (event) {
                    event.stopPropagation();
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    _this.searchParam = {};
                    _self.searchParam = _this.searchParam;
                    _self.data = [];
                    _self.pagedataCount = 0;
                    _self.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    _.Ajax('Output', {
                        Values: _self.searchParam
                    }, function (data) {
                        window.open(__BaseUrl + data);
                    });
                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (!ve.data.length) {
                        iview.Message.error("没有要打印的数据!");
                    } else {
                        iview.Message.error("尚未提供打印方法!");
                    }
                },
                add: function (event) {
                    event.stopPropagation();
                    _this.addHref();
                },
                del: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    let selectton = this.$refs.selectData.getSelection();
                    if (selectton.length == 0) {
                        iview.Message.info("请选中要删除的数据!");
                        return;
                    }
                    _.MessageBox("是否删除？", function () {
                        _.Ajax('Delete', {
                            DeleteData: selectton
                        }, function (data) {
                            iview.Message.info("删除成功");
                            showList();
                        });
                    });
                },
                changePageCount: function (index) {
                    this.currentPage = index;
                    showList();
                },
                changePageSizer: function (value) {
                    this.pageSize = value;
                    this.currentPage = 1;
                    showList();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.vueObj = new Vue(options);

        function showList() {
            let ve = _this.vueObj;
            let param = {};
            for (let item in ve.searchParam) {
                if (ve.searchParam[item]) {
                    if (Array.isArray(ve.searchParam[item])) {
                        param[item] = ve.searchParam[item].join(',');
                    } else {
                        param[item] = ve.searchParam[item];
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

    this.addHref = function () { }

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};   
    };

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var search = new _Search();