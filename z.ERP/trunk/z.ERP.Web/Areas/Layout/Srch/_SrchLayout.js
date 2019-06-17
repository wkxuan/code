function _Srch() {

    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.newCondition = function () { }

    this.IsValidSrch = function () {
        return true;
    }
    this.afterResult = function (data) { }

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
                options:{
                    columns: [],
                    data:[],
                },
                arrPageSize:[10,20,50,100],
                pagedataCount: 0,
                pageSize: 10
            },
            mounted: function () {
                _this.mountedInit();        
            },
            watch: {
                "screenParam.colDef": {
                    handler: function (nv, ov) {
                        this.options.columns = nv;
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
                    ve.searchParam = _this.searchParam;
                    this.options.data = [];
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
                        window.open(__BaseUrl+data);
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
                        window.document.body.innerHTML=TableData; 
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
        function showList(callback) {
            ve.searchParam = _this.searchParam;
            ve.pageInfo=_this.pageInfo;
            ve.options.data = [];
            ve.$Spin.show();
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                PageInfo:ve.pageInfo,
                Success: function (data) {
                    ve.$Spin.hide();
                    if (data.rows.length > 0) {
                        _this.afterResult(data);
                        ve.panelName = 'result';
                        ve.pagedataCount = data.total;
                        ve.options.data = data.rows;
                    }
                    else {
                        mess.$Message.info("没有满足当前查询条件的结果!");
                    }
                },
                Error: function () {
                   ve.$Spin.hide();
                }
            })
        };
        function notExistsData() {
            return (!ve.options.data) || (ve.options.data.length == 0)
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