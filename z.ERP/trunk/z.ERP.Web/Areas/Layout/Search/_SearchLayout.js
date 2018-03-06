function _Search() {

    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    //查询必须存在的条件,主要是一些赋值
    this.newCondition = function () { }

    //是否可以查询的判断
    this.IsValidSrch = function (mess) {
        return true;
    }

    //是否可以修改
    this.canEdit = function (mess) {
        return true;
    }

    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                    screenParam: _this.screenParam,
                    searchParam: _this.searchParam,
                    windowParam: _this.windowParam,
                    panelName: 'condition',
                    disabled: _this.enabled(true),
                    
                    //鉴于功能已经开始做了,故而将table的数据绑定的对象重新命名一个对象，进而刷新使用
                    screenParamData: {
                        dataDef: []
                    }
            },

            mounted: function () {
            },
            methods: {
                //查询OK
                seach: function (event) {
                    event.stopPropagation();
                    var mess = this;

                    if (!_this.IsValidSrch(mess))
                        return;
                    //先清空结果
                    Vue.set(ve.screenParamData, "dataDef", []);
                    //查询
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });
                },
                //清空OK
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.screenParamData.dataDef = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要导出的数据!");
                    } else {
                        this.$Message.error("尚未提供导出方法!");
                    }
                    //继续
                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要打印的数据!");
                    } else {
                        this.$Message.error("尚未提供打印方法!");
                    }
                    //继续
                },
                //新增
                add: function (event) {
                    event.stopPropagation();
                    _this.addHref();
                },
                browse: function (row, index) {
                    _this.browseHref(row, index);
                },
                //删除
                del: function (event) {
                    event.stopPropagation();
                    var _self = this;
                    console.log(this);
                    var selectton = this.$refs.selectData.getSelection();
                    if (selectton.length == 0) {
                        this.$Message.info("请选中要删除的数据!");
                    }
                    else {
                        this.$Modal.confirm({
                            title: '提示',
                            content: '是否删除',
                            onOk: function () {
                                _.Ajax('Delete', {
                                    DeleteData: selectton
                                }, function (data){ 
                                    showList(function (data){ 
                                        // ve.$set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                                        Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                                        _self.$Message.info("删除成功");
                                    });
                                });
                            },
                            onCancel: function () {
                                this.id = "关闭"
                            }
                        });
                    }
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showList(callback) {
            ve.searchParam = _this.searchParam;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                Success: function (data) {
                    _this.screenParam.dataDef = data.rows;
                    callback && callback();
                }
            })
        };
        function notExistsData() {
            return (!ve.screenParamData.dataDef) || (ve.screenParamData.dataDef.length == 0)
        }
    }
    //新增链接的地址
    this.addHref = function () {
    }
    //修改链接的地址
    this.modHref = function (row, index) {
    }
    //查看链接的地址
    this.browseHref = function (row, index) {

    }


    this.colDefInit = function () {
        _this.colMul = [{
            type: 'selection',
            width: 60,
            align: 'center',
            fixed: 'left',
        }];

        _this.colOperate = [{
            title: '操作',
            key: 'action',
            width: 80,
            align: 'center',
            fixed: 'right',
            render: function (h, params) {
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled: false },

                        style: { marginRight: '5px' },
                        on: {
                            click: function (event) {
                                _this.modHref(params.row, params.index);
                            }
                        },
                    }, '修改'),
                    ]);
            }
        }]
    }

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.windowParam = {};
        _this.service = "";
        _this.method = "";
    };

    setTimeout(function () {
        _this.colDefInit();
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var search = new _Search();