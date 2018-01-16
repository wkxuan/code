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
            mounted: function () {
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
                browse: function (row,index) {
                    _this.browseHref(row, index);
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
    }
    //修改链接的地址
    this.modHref = function (row, index) {
    }
    //查看链接的地址
    this.browseHref = function (row, index) {

    }
    //删除对应的操作
    this.deleteData = function (row, index) {
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
            width: 130,
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

                    h('Button', {
                        props: { type: 'error', size: 'small', disabled: false },

                        style: { marginRight: '5px' },
                        on: {
                            click: function (event) {
                                _this.deleteData(params.row, params.index);
                            }
                        },
                    }, '删除')
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