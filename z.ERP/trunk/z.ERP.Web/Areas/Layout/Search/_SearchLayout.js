function _Search() {

    var _this = this;

    this.beforeVue = function () { }

    this.newCondition = function () { }

    this.enabled = function (val) { return val; }

    this.IsValidSrch = function () {
        return true;
    }

    this.mountedInit = function () { }

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo: _this.pageInfo,
                disabled: _this.enabled(true),
                panelName: 'condition',
                columns: [],
                data: [],
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100],
                pagedataCount: 0,
                pageSize: 10
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
                seach: function (event) {
                    event.stopPropagation();
                    let mess = this;
                    if (!_this.IsValidSrch())
                        return;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = 0;
                    showList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.data = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    var _self = this;
                    _.Ajax('Output', {
                        Values: _this.searchParam
                    }, function (data) {
                        window.open(__BaseUrl + data);
                    });
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
                    } else {
                        _self.$Modal.confirm(_.MessageBox("是否删除？", () => {
                            _.Ajax('Delete', {
                                DeleteData: selectton
                            }, function (data) {
                                iview.Message.info("删除成功");
                                showList();
                            });
                        }));
                    }
                },
                changePageCount: function (index) {
                    let mess = this;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = (index - 1);
                    showList();
                },
                changePageSizer: function (value) {
                    let mess = this;
                    this.pageSize = value;
                    _this.pageInfo.PageSize = value;
                    showList();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showList() {
            ve.searchParam = _this.searchParam;
            ve.pageInfo = _this.pageInfo;
            ve.data = [];
            ve.tbLoading = true;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                PageInfo: ve.pageInfo,
                Success: function (data) {
                    ve.tbLoading = false;
                    if (data.rows.length > 0) {
                        ve.panelName = 'result';
                        ve.data = data.rows;
                        ve.pagedataCount = data.total;
                    }
                    else {
                        ve.$Message.info("没有满足当前查询条件的结果!");
                    }
                },
                Error: function () {
                    ve.tbLoading = false;
                }
            })
        };
        function notExistsData() {
            return (!ve.data) || (ve.data.length == 0)
        }
    }

    this.addHref = function () { }

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
var search = new _Search();