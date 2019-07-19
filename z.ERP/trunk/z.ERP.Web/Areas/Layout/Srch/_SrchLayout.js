function _Srch() {

    var _this = this;
    this.vueObj;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.newCondition = function () { }

    this.IsValidSrch = function () {
        return true;
    }
    this.afterResult = function (data) { }
    //echart初始化函数
    this.echartInit = function (data) { }
    
    this.canEdit = function (mess) {
        return true;
    }

    this.mountedInit = function () { }
    //是否显示可视化数据折叠面板
    this.echartResult = false;

    this.vue = function VueOperate() {
        var options = {
            el: '#srch',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo: _this.pageInfo,
                panelName: ["condition", "echart", "result"],
                disabled: _this.enabled(true),
                columns: [],
                data: [],
                tbLoading: false,
                arrPageSize: [10, 20, 50, 100],
                pagedataCount: 0,
                pageSize: 10,
                currentPage: 1,
                echartResult: _this.echartResult
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
                    var mess = this;
                    if (!_this.IsValidSrch())
                        return;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = 0;
                    showList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    _this.vueObj.searchParam = _this.searchParam;
                    this.data = [];
                    _this.vueObj.pagedataCount = 0;
                    _this.vueObj.panelName = 'condition';
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

                    /*   if (notExistsData()) {
                        this.$Message.error("没有要导出的数据!");
                    } else {
                        // this.$Message.error("尚未提供导出方法!");
                               this.$refs.selectData.exportCsv({
                                   filename: (new Date()).toString()
                            });
                    } */
                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要打印的数据!");
                    } else {
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
                    }
                },
                changePageCount: function (index) {
                    let mess = this;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = index - 1;
                    mess.currentPage = index;
                    showList();
                },
                changePageSizer: function (value) {
                    let mess = this;
                    this.pageSize = value;
                    _this.pageInfo.PageSize = value;
                    mess.currentPage = 1;
                    showList();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.vueObj = new Vue(options);
        function showList() {
            _this.vueObj.searchParam = _this.searchParam;
            _this.vueObj.pageInfo = _this.pageInfo;
            _this.vueObj.data = [];
            _this.vueObj.pagedataCount = 0;
            _this.vueObj.tbLoading = true;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: _this.vueObj.searchParam,
                PageInfo: _this.vueObj.pageInfo,
                Success: function (data) {
                    _this.vueObj.tbLoading = false;
                    if (data.rows.length > 0) {
                        _this.afterResult(data.rows);
                        _this.vueObj.panelName = ["echart", "result"];
                        _this.vueObj.pagedataCount = data.total;
                        _this.vueObj.data = data.rows;    
                    }
                    else {
                        _this.vueObj.$Message.info("没有满足当前查询条件的结果!");
                    }
                    if (_this.echartResult) {
                        _this.echartInit(_this.vueObj.data);
                    }
                },
                Error: function () {
                    _this.vueObj.tbLoading = false;
                }
            })
        };
        function notExistsData() {
            return (!_this.vueObj.data) || (_this.vueObj.data.length == 0)
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