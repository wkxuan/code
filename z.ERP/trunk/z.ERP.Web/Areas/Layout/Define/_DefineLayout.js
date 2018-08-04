function _Define() {
    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }
    this.isvisible = function (val) { return val; }
    this.btnChkvisible = false;
    this.alwaysenabled = true;
    this.IsValidSave = function () {
        return true;
    }

    //添加后初始化数据信息
    this.newRecord = function () { }
    this.beforeDel = function () {
        return true;
    }

    //得到主键
    this.Key = undefined;
    this.mountedInit = function () { };


    this.vue = function VueOperate() {
        var options = {
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                disabled: _this.enabled(true),
                topbtnModVisible: _this.isvisible(true),
                topbtnChkVisible: _this.btnChkvisible,
                alwaysdisabled: _this.alwaysenabled,
                _key: undefined,
                tableH: 500
            },
            mounted: function () {                
                _.Search({
                    Service: _this.service,
                    Method: _this.methodList,
                    Data: _this.searchParam,//增加查询条件
                    Success: function (data) {
                        define.screenParam.dataDef = data.rows;
                    }
                })
                _this.mountedInit();
            },
            methods: {
                add: function (event) {
                    ve._key = define.dataParam[_this.Key],
                    _this.dataParam = {};
                    _this.newRecord();
                    ve.dataParam = _this.dataParam;
                    ve.disabled = _this.enabled(false);
                },
                mod: function (event) {
                    if (!ve._key) {
                        this.$Message.error("请选择数据");
                        return;
                    }
                    ve._key = define.dataParam[_this.Key];
                    ve.disabled = _this.enabled(false);
                },
                save: function (event) {
                    var _self = this;
                    if (!_this.IsValidSave())
                        return;
                    save(function (data) {
                        showlist(function () {
                            _this.showone(data, function () {
                                ve.disabled = _this.enabled(true);
                                ve.dataParam = _this.dataParam;
                                _self.$Message.info("保存成功");
                            });
                        });
                    })
                },
                quit: function (event) {
                    if (ve._key) {
                        this.$Modal.confirm({
                            title: '提示',
                            content: '是否取消',
                            onOk: function () {
                                _this.dataParam = {};
                                ve.dataParam = _this.dataParam;
                                _this.showone(ve._key, function () {
                                    ve.dataParam = _this.dataParam;
                                });
                                ve.disabled = _this.enabled(true);
                            },
                            onCancel: function () {
                                ve.disabled = _this.enabled(false);
                                this.id = "关闭"
                            }
                        });
                    }
                    else {
                        ve.disabled = _this.enabled(true);
                    }

                },
                del: function (event) {
                    var _self = this;
                    if (!ve._key) {
                        _self.$Message.error("请选择数据");
                        return;
                    };

                    if (!_this.beforeDel())
                        return;

                    this.$Modal.confirm({
                        title: '提示',
                        content: '是否删除',
                        onOk: function () {
                            deleteone(ve.dataParam, function () {
                                showlist();
                                _this.dataParam = {};
                                ve.dataParam = _this.dataParam;
                                _self.$Message.info("删除成功");
                            });
                        },
                        onCancel: function () {
                            this.id = "关闭"
                        }
                    });
                    ve.disabled = _this.enabled(true);
                },
                showlist: function (currentRow, oldCurrentRow) {
                    $.extend(_this.dataParam , currentRow);
                    ve._key = define.dataParam[_this.Key];
                    _this.showone(ve._key, function () {
                        ve.dataParam = _this.dataParam;
                    });
                },
                seachList: function (event) {
                    showlist();
                },
                chk: function (event) {
                    var _self = this;
                    if (!ve._key) {
                        _self.$Message.error("请选择数据");
                        return;
                    };
                    if (!_this.IsValidSave())
                        return;
                    check(function (data) {
                        showlist(function () {
                            _this.showone(data, function () {
                                //ve.disabled = _this.enabled(false);
                                ve.dataParam = _this.dataParam;
                                _self.$Message.info("审核成功");
                            });
                        });
                    })
                },
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        _this.myve = ve;

        function showlist(callback) {
            _.Search({
                Service: _this.service,
                Method: _this.methodList,
                Data: _this.searchParam,
                Success: function (data) {
                    define.screenParam.dataDef = data.rows;
                    callback && callback();
                }
            })
        }

        function save(callback) {
            _.Ajax('Save', {
                DefineSave: ve.dataParam
            }, function (data) {
                callback && callback(data);
            });
        }

        function deleteone(data, callback) {
            _.Ajax('Delete', {
                DefineDelete: data
            }, function (data) {
                callback && callback();
            });
        }

        function check(callback) {
            _.Ajax('Check', {
                DefineSave: ve.dataParam
            }, function (data) {
                callback && callback(data);
            });
        }
    }

    this.showlist = function (callback) {
        _.Search({
            Service: _this.service,
            Method: _this.methodList,
            Data: _this.searchParam,
            Success: function (data) {
                define.screenParam.dataDef = data.rows;
                callback && callback();
            }
        })
    }
    this.showone=function (key, callback) {
        if (key) {
            var v = {};
            v[_this.Key] = key;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: v,
                Success: function (data) {
                    _this.dataParam = data.rows[0];
                    callback && callback();
                }
            });
        }
    }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.methodList = "";
        _this.myve = null;
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();         
    }, 100);
}
var define = new _Define();
