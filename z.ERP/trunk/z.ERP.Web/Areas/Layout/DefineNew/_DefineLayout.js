function _DefineNew() {
    var _this = this;
    this.myvue;
    this.columnsDef = [];
    this.beforeVue = function () { };
    this.mountedInit = function () { };
    this.add = function () { };
    this.newCondition = function () { };

    this.vue = function VueOperate() {
        var options = {
            el: '#def_Main',
            data: {
                panelName: ["condition", "result"],
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                data: [],
                columns: _this.columnsDef,
                tbLoading: false
            },
            mounted: function () {
                _this.mountedInit();
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    this.searchList();
                },
                clear: function (event) {
                    event.stopPropagation();
                    let _self = this;
                    _self.searchParam = {};
                    _self.data = [];
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
                searchList:function () {
                    let _self = this;
                    _self.tbLoading = true;
                    _self.data = [];
                    _.Search({
                        Service: _this.service,
                        Method: _this.method,
                        Data: _this.searchParam,
                        Success: function (data) {
                            _self.tbLoading = false;
                            if (data.rows.length) {
                                _self.data = data.rows;
                            } else {
                                iview.Message.info("没有满足当前查询条件的结果!");
                            }
                        },
                        Error: function () {
                            _self.tbLoading = false;
                        }
                    })
                }
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.myvue = new Vue(options);
    }

    this.searchList = function () {
        _this.myvue.searchList();
    };

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.myvue = null;
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var defineNew = new _DefineNew();
