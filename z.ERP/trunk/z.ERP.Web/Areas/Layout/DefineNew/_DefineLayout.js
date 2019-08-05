function _DefineNew() {
    var _this = this;
    this.vueObj;
    this.columnsDef = [];
    this.btnConfig = [];
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
                currentPage: 1,
                toolBtnList: [],
            },
            mounted: function () {
                _this.mountedInit();
                this.initBtn();
            },
            computed: {
                list() {
                    return this.toolBtnList || [];
                }
            },
            methods: {
                //初始化功能按钮
                initBtn() {
                    let _self = this;
                    let baseBtn = [{
                        id: "select",
                        name: "查询",
                        icon: "md-search",
                        fun: function () {
                            _self.search();
                        },
                        enabled: function () {
                            return true;
                        }
                    },{
                        id: "clear",
                        name: "清空",
                        icon: "md-refresh",
                        fun: function () {
                            _self.clear();
                        },
                        enabled: function () {
                            return true;
                        }
                    }, {
                        id: "add",
                        name: "新增",
                        icon: "md-add",
                        fun: function () {
                            _self.add();
                        },
                        enabled: function (a) {
                            return true;
                        }
                    }, {
                        id: "del",
                        name: "删除",
                        icon: "md-trash",
                        fun: function () {
                            _self.del();
                        },
                        enabled: function () {
                            return true;
                        }
                    }];
                    let data = [];
                    for (let j = 0, jlen = _this.btnConfig.length; j < jlen; j++) {
                        for (let i = 0, ilen = baseBtn.length; i < ilen; i++) {
                            if (baseBtn[i].id == _this.btnConfig[j].id) {
                                let loc = {};
                                $.extend(loc, baseBtn[i], _this.btnConfig[j]);
                                data.push(loc);
                            }
                        }
                        if (_this.btnConfig[j].isNewAdd) {
                            data.push(_this.btnConfig[j]);
                        }
                    }
                    _self.toolBtnList = data;
                },
                search: function (event) {
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                },
                clear: function () {
                    let _self = this;
                    _self.searchParam = {};
                    _self.data = [];
                    _self.pagedataCount = 0;
                    _this.newCondition();
                },
                add: function () {
                    _this.add();
                },
                del: function () {
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
