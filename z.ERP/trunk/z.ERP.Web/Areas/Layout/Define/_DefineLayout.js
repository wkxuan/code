function _Define() {
    var _this = this;
    //得到主键
    this.Key;
    this.vueObj;
    this.service = "";
    this.method = "";
    this.methodList = "";
    this.splitVal = 0.3; //左右split分割比
    this.backData = {};
    this.btnConfig = [{
        id: "search"
    }, {
        id: "add"
    }, {
        id: "edit"
    }, {
        id: "save"
    }, {
        id: "abandon"
    }];
    //默认弹窗设置
    this.popConfig = {
        title: "弹窗",
        src: "",
        width: 800,
        height: 550,
        open: false
    };

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }
    //保存前验证函数
    this.IsValidSave = function () {
        return true;
    }
    //修改前验证函数
    this.IsValidMod = function () {
        return true;
    }

    //添加后初始化数据信息
    this.newRecord = function () { }
    //删除前验证函数
    this.beforeDel = function () {
        return true;
    }
    //审核前验证函数
    this.IsValidChk = function () {
        return true;
    }
    this.cancelAfter = function () { }
    //初始化vue data.dataParam
    this.initDataParam = function () { };

    this.mountedInit = function () { };

    this.vue = function VueOperate() {
        var options = {
            el: '#define',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                disabled: _this.enabled(true),
                data: [],
                columns: [],
                splitVal: _this.splitVal,
                toolBtnList: [],
                popConfig: _this.popConfig,
                tbLoading: false
            },
            mounted: function () {
                _this.mountedInit();
                this.seachList();
                this.initBtn();
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
            computed: {
                highlightRow() {
                    return this.disabled;
                }
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
                            _self.seachList();
                        },
                        enabled: function (disabled, data) {
                            return disabled;
                        }
                    }, {
                        id: "add",
                        name: "新增",
                        icon: "md-add",
                        fun: function () {
                            _self.add();
                        },
                        enabled: function (disabled, data) {
                            return disabled;
                        }
                    }, {
                        id: "edit",
                        name: "编辑",
                        icon: "md-create",
                        fun: function () {
                            _self.edit();
                        },
                        enabled: function (disabled, data) {
                            if (disabled && data && data[_this.Key]) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                        //}, {
                        //    id: "del",
                        //    name: "删除",
                        //    icon: "md-trash",
                        //    fun: function () {
                        //        _self.del();
                        //    },
                        //    enabled: function (disabled, data) {
                        //        if (!disabled && data && data[_this.Key]) {
                        //            return true;
                        //        } else {
                        //            return false;
                        //        }
                        //    }
                    }, {
                        id: "save",
                        name: "存档",
                        icon: "md-checkmark-circle",
                        fun: function () {
                            _self.save();
                        },
                        enabled: function (disabled, data) {
                            return !disabled;
                        }
                    }, {
                        id: "abandon",
                        name: "放弃",
                        icon: "md-refresh",
                        fun: function () {
                            _self.quit();
                        },
                        enabled: function (disabled, data) {
                            return !disabled;
                        }
                    }, {
                        id: "confirm",
                        name: "审核",
                        icon: "md-star",
                        fun: function () {
                            _self.chk();
                        },
                        enabled: function (disabled, data) {
                            return !disabled;
                        }
                    }];
                    let data = [];
                    for (let j = 0, jlen = _this.btnConfig.length; j < jlen; j++) {
                        for (let i = 0, ilen = baseBtn.length; i < ilen; i++) {
                            if (baseBtn[i].id == _this.btnConfig[j].id) {
                                let loc = {
                                };
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
                add: function () {
                    this.disabled = false;
                    _this.backData = DeepClone(this.dataParam);
                    this.dataParam = ClearObject(this.dataParam);
                    _this.newRecord();
                },
                edit: function () {
                    if (!this.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }
                    if (!_this.IsValidMod())
                        return;

                    this.disabled = false;
                    _this.backData = DeepClone(this.dataParam);

                },
                save: function () {
                    var _self = this;
                    if (!_this.IsValidSave())
                        return;
                    _.Ajax('Save', {
                        DefineSave: _self.dataParam
                    }, function (data) {
                        _self.disabled = true;
                        _this.showOne(data, function () {
                            _self.seachList();
                            iview.Message.info("保存成功!");
                        });
                    });
                },
                quit: function () {
                    var _self = this;
                    _.MessageBox("是否取消？", function () {
                        let flag = false;
                        for (let item in _this.backData) {
                            flag = true;
                            _self.dataParam[item] = _this.backData[item];

                            _this.cancelAfter();
                        }
                        if (!flag) {
                            _this.initDataParam();
                        }
                        _self.disabled = true;
                    });
                },
                del: function () {
                    var _self = this;
                    if (!_self.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }

                    if (!_this.beforeDel())
                        return;

                    _.MessageBox("是否删除？", function () {
                        _.Ajax('Delete', {
                            DefineDelete: _self.dataParam
                        }, function (data) {
                            _this.initDataParam();
                            this.seachList();
                            iview.Message.info("删除成功");
                        });
                    });
                },
                chk: function () {
                    var _self = this;
                    if (!_self.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }
                    if (!_this.IsValidChk())
                        return;

                    _.Ajax('Check', {
                        DefineSave: _self.dataParam
                    }, function (data) {
                        _this.showOne(_self.dataParam[_this.Key], function () {
                            iview.Message.info("审核成功");
                        });
                    });
                },
                seachList: function (callback) {
                    let _self = this;
                    _self.tbLoading = true;
                    _.Search({
                        Service: _this.service,
                        Method: _this.methodList,
                        Data: _this.searchParam,
                        Success: function (data) {
                            _self.tbLoading = false;
                            _self.data = data.rows;
                            callback && callback();
                        }, Error: function () {
                            _self.tbLoading = false;
                        }
                    })
                },
                currentChange: function (currentRow, oldCurrentRow) {
                    _this.showOne(currentRow[_this.Key]);
                },
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.vueObj = new Vue(options);
    }

    this.showlist = function (callback) {
        _this.vueObj.seachList(callback);
    }

    this.showOne = function (val, callback) {
        if (val) {
            var v = {};
            v[_this.Key] = val;
            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: v,
                Success: function (data) {
                    $.extend(_this.dataParam, data.rows[0]);
                    callback && callback();
                }
            });
        }
    }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.searchParam = {};
        _this.screenParam = {};
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.initDataParam();
        _this.vue();
    }, 100);
}
var define = new _Define();
