function _DefineNew() {
    var _this = this;
    this.vueObj;
    this.columnsDef = [];
    this.beforeVue = function () { };
    this.mountedInit = function () { };
    this.add = function () { };
    this.newCondition = function () { };
    this.IsValidSrch = function () {
        return true;
    }
    this.vue = function VueOperate() {
        var options = {
            el: '#defineNew',
            data: {
                panelName: ["condition", "result"],
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                data: [],
                columns: _this.columnsDef,
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100],
                pagedataCount: 0,
                pageSize: 10,
                currentPage: 1
            },
            mounted: function () {
                _this.mountedInit();
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    _self.searchParam = {};
                    _self.data = [];
                    _self.pagedataCount = 0;
                    _this.newCondition();
                },
                add: function (event) {
                    event.stopPropagation();
                    _this.add();
                },
                del: function (event) {
                    event.stopPropagation();
                    let selectton = this.$refs.selectData.getSelection();
                    if (selectton.length == 0) {
                        iview.Message.info("请选中要删除的数据!");
                        return;
                    } else {
                        _.MessageBox("是否删除？", function () {
                            _.Ajax('Delete', {
                                DefineDelete: selectton
                            }, function (data) {
                                iview.Message.info("删除成功");
                                _this.searchList();
                            });
                        });
                    }
                },
                changePageCount: function (index) {
                    this.currentPage = index;
                    showList();
                },
                changePageSizer: function (value) {
                    this.pageSize = value;
                    this.currentPage = 1;
                    showList();
                },
                searchList: function () {
                    showList();
                }
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.vueObj = new Vue(options);

        function showList() {
            let ve = _this.vueObj;
            ve.data = [];
            ve.pagedataCount = 0;
            ve.tbLoading = true;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                PageInfo: {
                    PageSize: ve.pageSize,
                    PageIndex: ve.currentPage - 1
                },
                Success: function (data) {
                    ve.tbLoading = false;
                    if (data.rows.length > 0) {
                        ve.pagedataCount = data.total;
                        ve.data = data.rows;
                        ve.panelName = ["result"];
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

    this.searchList = function () {
        _this.vueObj.searchList();
    };

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var defineNew = new _DefineNew();
