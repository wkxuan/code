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
        var ve = new Vue({
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: 'condition',
                disabled: _this.enabled(true),
            },
            methods: {
                //查询OK
                seach: function (event) {
                    event.stopPropagation();
                    var mess = this;

                    if (!_this.IsValidSrch(mess))
                        return;
                    showList(function (data) {
                        if (ve.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
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
                    ve.screenParam.dataDef = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    if (notExistsData) {
                        this.$Message.error("没有要导出的数据!");
                    };
                    //继续
                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (notExistsData) {
                        this.$Message.error("没有要打印的数据!");
                    };
                    //继续
                },
                //新增
                add: function (event) {
                    event.stopPropagation();
                    _this.addHref();
                },
                //修改
                mod: function (event) {
                    event.stopPropagation();
                    var _self = this;
                    var selectton = this.$refs.selectData.getSelection();
                    if (selectton.length != 1) {
                        this.$Message.error("只能选择一条记录修改!");
                    }
                    else {
                        //这里就根据状态等信息判断
                        if (!_this.canEdit(_self))
                            return;
                        else
                             //链接过去编辑页面之后在保存的后端在进行实际的判断
                            _this.modHref();
                    }
                },
                del: function (event) {
                    //删除可以批量删除
                    event.stopPropagation();
                    var _self = this;
                    var selectton = this.$refs.selectData.getSelection();
                    if (selectton.length == 0) {
                        this.$Message.error("请选中要删除的数据!");
                    }
                    else {

                        this.$Modal.confirm({
                            title: '提示',
                            content: '是否删除',
                            onOk: function () {
                                deleteList(selectton, function () {
                                    showList(function () {
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
        });

        function showList(callback) {
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: _this.searchParam,
                Success: function (data) {
                    ve.screenParam.dataDef = [];
                    ve.screenParam.dataDef = data.rows;
                    callback && callback();
                }
            })
        }
        function notExistsData() {
            return (!ve.screenParam.dataDef) || (ve.screenParam.dataDef.length == 0)
        }

        function deleteList(data, callback) {
            _.Ajax('Delete', {
                DeleteData: data
            }, function (data) {
                callback && callback();
            });
        }

    }
    //新增链接的地址
    this.addHref = function () {
        return " ";
    }
    //修改链接的地址
    this.modHref = function () {
        return " ";
    }
    //查看链接的地址
    this.browseHref = function () {
        return " ";
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
            width: 60,
            align: 'center',
            fixed: 'right',
            render: function (h, params) {
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled:false },

                        style: { marginRight: '50px' },
                        on: {
                            click: function (event) {
                                _this.browseHref();
                            }
                        },
                    }, '浏览')
                    ]);
            }
        }]
    }

    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
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