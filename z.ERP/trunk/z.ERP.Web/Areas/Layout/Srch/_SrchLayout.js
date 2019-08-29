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
    this.service = "";
    this.method = "";
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
                    if (!_this.IsValidSrch())
                        return;
                    showList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    _this.beforeVue();
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
            ve.data = [];
            ve.pagedataCount = 0;
            ve.tbLoading = true;
            let param = {};
            for (let item in ve.searchParam) {
                if (Array.isArray(ve.searchParam[item])) {
                    param[item] = ve.searchParam[item].join(',');
                } else {
                    param[item] = ve.searchParam[item];
                }
            }
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
                        _this.afterResult(data.rows);
                        ve.panelName = ["echart", "result"];
                        ve.pagedataCount = data.total;
                        ve.data = data.rows;
                    }
                    else {
                        iview.Message.info("没有满足当前查询条件的结果!");
                    }
                    if (_this.echartResult) {
                        _this.echartInit(ve.data);
                    }
                },
                Error: function () {
                    ve.tbLoading = false;
                }
            });
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
var srch = new _Srch();