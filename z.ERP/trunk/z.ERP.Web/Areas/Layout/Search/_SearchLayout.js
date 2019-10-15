function _Search() {

    var _this = this;
    this.vueObj;
    this.service = "";
    this.method = "";
    //默认功能按钮
    this.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }, {
        id: "del",
        authority: ""
    }];
    //默认弹窗设置
    this.popConfig = {
        title: "弹窗",
        src: "",
        width: 800,
        height: 500,
        open: false
    };
    //是否显示中间折叠面板
    this.panelTwoShow = false;
    //中间折叠面板的Header title
    this.panelTwoName = "可视化数据";
    //table是否显示序号
    this.indexShow = false;
    //table是否显示checkbox
    this.selectionShow = true;

    this.beforeVue = function () { }

    this.newCondition = function () { }

    this.enabled = function (val) { return val; }

    this.IsValidSrch = function () {
        return true;
    }

    this.addHref = function () { }
   
    this.mountedInit = function () { }

    this.searchDataAfter = function (data) { };

    this.searchList = function () {
        _this.vueObj.searchList();
    };

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo: _this.pageInfo,
                disabled: _this.enabled(true),
                panelName: ["panelOne", "panelTwo", "panelThree"],
                columns: [],
                data: [],
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100, 200],
                pagedataCount: 0,
                pageSize: 10,
                currentPage: 1,
                importOpen: false,
                toolBtnList: [],
                panelTwoShow: _this.panelTwoShow,
                panelTwoName: _this.panelTwoName,
                indexShow: _this.indexShow,
                selectionShow: _this.selectionShow,
                popConfig: _this.popConfig
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
                this.initBtn();
            },
            methods: {
                //初始化功能按钮
                initBtn: function () {
                    let _self = this;
                    let baseBtn = [{
                        id: "search",
                        name: "查询",
                        icon: "md-search",
                        fun: function () {
                            _self.search();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "clear",
                        name: "清空",
                        icon: "md-refresh",
                        fun: function () {
                            _self.clear();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "add",
                        name: "新增",
                        icon: "md-add",
                        fun: function () {
                            _self.add();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "del",
                        name: "删除",
                        icon: "md-trash",
                        fun: function () {
                            _self.del();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "upload",
                        name: "导入",
                        icon: "md-cloud-upload",
                        fun: function () {
                            _self.upload();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "export",
                        name: "导出",
                        icon: "md-download",
                        fun: function () {
                            _self.exp();
                        },
                        enabled: function (disabled, data) {
                            return true;
                        }
                    }, {
                        id: "print",
                        name: "打印",
                        icon: "md-print",
                        fun: function () {
                            _self.print();
                        },
                        enabled: function (disabled, data) {
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
                //查询
                search: function () {
                    if (!_this.IsValidSrch())
                        return;
                    this.pageSize = 10;
                    this.currentPage = 1;
                    showList();
                },
                //清空
                clear: function () {
                    let _self = this;
                    _this.searchParam = {};
                    _self.searchParam = _this.searchParam;
                    _self.data = [];
                    _self.pagedataCount = 0;
                    _self.panelName = ["panelOne", "panelTwo", "panelThree"];
                    _this.newCondition();
                },               
                //新增
                add: function () {
                    _this.addHref();
                },
                //删除
                del: function () {
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
                            _self.pageSize = 10;
                            _self.currentPage = 1;
                            showList();
                        });
                    });
                },
                //导入
                upload: function () {
                    this.importOpen = true;
                },
                //导出待完善
                exp: function () {
                    let _self = this;
                    let param = {};
                    for (let item in _self.searchParam) {
                        if (_self.searchParam[item]) {
                            if (Array.isArray(_self.searchParam[item])) {
                                param[item] = _self.searchParam[item].join(',');
                            } else {
                                param[item] = _self.searchParam[item];
                            }
                        }
                    }
                    let cols = {};
                    for (let i = 0; i < _self.columns.length; i++) {
                        cols[_self.columns[i].key] = _self.columns[i].title
                    }

                    _.Ajax('Output', {
                        Name: window.document.title,
                        Cols: cols,
                        Values: param
                    }, function (data) {
                        if (data) {
                            window.open(__BaseUrl + data);
                        }
                    });
                },
                //打印待完善
                print: function () {
                    if (!this.data.length) {
                        iview.Message.error("没有要打印的数据!");
                        return;
                    }
                    // 获取原来的窗口界面body的html内容，并保存起来
                    var oldhtml = window.document.body.innerHTML;
                    //根据div标签ID拿到div中的局部内容
                    var TableData = window.document.getElementById("TableData").innerHTML;
                    //把获取的 局部div内容赋给body标签
                    window.document.body.innerHTML = TableData;
                    window.print();
                    // 将原来窗口body的html值回填展示
                    window.document.body.innerHTML = oldhtml;
                    //刷新页面,否则无法点击
                    window.location.reload();
                },
                //切换页数change
                changePageCount: function (index) {
                    this.currentPage = index;
                    showList();
                },
                //改变页的大小change
                changePageSizer: function (value) {
                    this.pageSize = value;
                    this.currentPage = 1;
                    showList();
                },
                searchList: function () {
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
                        ve.panelName = ["panelTwo", "panelThree"];
                        ve.data = data.rows;
                        ve.pagedataCount = data.total;
                    }
                    else {
                        iview.Message.info("没有满足当前查询条件的结果!");
                    }
                    if (_this.panelTwoShow) {
                        _this.searchDataAfter(ve.data);
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
        _this.vue();
    }, 100);
}
var search = new _Search();