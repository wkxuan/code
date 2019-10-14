function _DefineDetail() {
    var _this = this;
    this.backData = {}; //新增或编辑前的原数据
    this.beforeVue = function () { }
    this.enabled = function (val) { return val; }
    this.IsValidSave = function () {
        return true;
    }
    //添加后初始化数据信息
    this.newRecord = function () { }
    this.beforeDel = function () {
        return true;
    }
    this.clearKey = function () { };
    this.Key = null;
    this.mountedInit = function () { };
    this.btnConfig = [];
    this.vue = function VueOperate() {
        var options = {
            el: '#DefineDetail',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                disabled: _this.enabled(true),
                _key: undefined,
                toolBtnList: [],
            },
            mounted: function () {                
                _this.mountedInit();
                this.initBtn();
            },
            computed: {
                list() {
                    return this.toolBtnList || [];
                }
            },
            methods: {
                //初始化功能按钮
                initBtn() {
                    let _self = this;
                    let baseBtn = [{
                        id: "add",
                        name: "新增",
                        icon: "md-add",
                        fun: function () {
                            _self.add();
                        },
                        enabled: function (disabled, data) {
                            return !disabled;
                        }
                    },{
                        id: "edit",
                        name: "编辑",
                        icon: "md-create",
                        fun: function () {
                            _self.mod();
                        },
                        enabled: function (disabled, data) {
                            if (!disabled && data[_this.Key]) {
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
                    //        if (!disabled && data[_this.Key]) {
                    //            return true;
                    //        } else {
                    //            return false;
                    //        }
                    //        return true;
                    //    }
                    }, {
                        id: "save",
                        name: "存档",
                        icon: "md-checkmark-circle",
                        fun: function () {
                            _self.save();
                        },
                        enabled: function (disabled, data) {
                            if (disabled) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    }, {
                        id: "abandon",
                        name: "放弃",
                        icon: "md-refresh",
                        fun: function () {
                            _self.quit();
                        },
                        enabled: function (disabled, data) {
                            if (disabled) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    }];
                    let data = [];
                    for (let i = 0, ilen = baseBtn.length; i < ilen; i++) {
                        for (let j = 0, jlen = _this.btnConfig.length; j < jlen; j++) {
                            if (baseBtn[i].id == _this.btnConfig[j].id) {
                                let loc = {};
                                $.extend(loc, baseBtn[i], _this.btnConfig[j]);
                                data.push(loc);
                            }
                        }
                    }
                    for (let j = 0, jlen = _this.btnConfig.length; j < jlen; j++) {
                        if ((_this.btnConfig[j].id != "add" ||
                            _this.btnConfig[j].id != "edit" ||
                            _this.btnConfig[j].id != "del" ||
                            _this.btnConfig[j].id != "save" ||
                            _this.btnConfig[j].id != "abandon") && _this.btnConfig[j].isNewAdd) {
                            data.push(_this.btnConfig[j]);
                        }
                    }
                    _self.toolBtnList = data;
                },
                add: function (event) {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.dataParam = ClearObject(_self.dataParam);
                    this.disabled = true;
                    _this.clearKey();
                    _this.newRecord();
                },
                mod: function (event) {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    this.disabled = true;
                },
                save: function (event) {
                    let _self = this;
                    if (!_this.IsValidSave())
                        return;

                    _.Ajax('Save', {
                        DefineSave: _self.dataParam
                    }, function (data) {
                        _self.disabled = false;
                        _self.dataParam = _this.dataParam;
                        iview.Message.info("保存成功");

                        if (window.parent.defineNew != undefined) {
                            window.parent.defineNew.popCallBack(data);
                        }
                    });
                },
                quit: function (event) {
                    let _self = this;
                    _.MessageBox("确认放弃正在编辑的内容？", function () {
                        let flag = false;
                        for (let item in _this.backData) {
                            flag = true;
                            _self.dataParam[item] = _this.backData[item];
                        }
                        if (!flag) {
                            _self.dataParam = ClearObject(_self.dataParam);
                        }
                        _self.disabled = false;
                    });
                },
                del: function (event) {
                    let _self = this;

                    if (!_this.beforeDel())
                        return;

                    _.MessageBox("是否删除？", function () {
                        _.Ajax('Delete', {
                            DefineDelete: [_self.dataParam]
                        }, function (data) {
                            _self.disabled = false;
                            _this.dataParam = {};
                            _self.dataParam = _this.dataParam;
                            iview.Message.info("删除成功");

                            if (window.parent.defineNew != undefined) {
                                window.parent.defineNew.popCallBack(data);
                            }
                        });
                    });
                },
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.myvue = new Vue(options);
    }

    this.showOne = function (data, callback) { }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.myvue = null;
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.clearKey();
        _this.vue();

        if (_this.Id) {
            _this.showOne(_this.Id);
            _this.myvue.disabled = false;
        }
    }, 100);
}
var defineDetail = new _DefineDetail();
