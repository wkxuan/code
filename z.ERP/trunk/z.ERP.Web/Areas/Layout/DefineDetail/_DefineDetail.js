function _DefineDetail() {
    var _this = this;
    this.Key;
    this.service = "";
    this.method = "";
    this.vueObj = {};
    this.backData = {}; //新增或编辑前的原数据
    this.btnConfig = [{
        id: "add"
    }, {
        id: "edit"
    }, {
        id: "save"
    }, {
        id: "abandon"
    }];

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }
    //保存前调用函数
    this.IsValidSave = function () {
        return true;
    }
    //添加后初始化数据信息
    this.newRecord = function () { }
    //删除前调用函数
    this.beforeDel = function () {
        return true;
    }
    //取消成功后调用函数
    this.cancelAfter = function () { }
    //初始化vue data.dataParam
    this.initDataParam = function () { };

    this.mountedInit = function () { };

    this.vue = function VueOperate() {
        var options = {
            el: '#DefineDetail',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                disabled: _this.enabled(true),
                toolBtnList: [],
            },
            mounted: function () {                
                _this.mountedInit();
                this.initBtn();
            },
            methods: {
                //初始化功能按钮
                initBtn: function () {
                    let _self = this;
                    let baseBtn = [{
                        id: "add",
                        name: "新增",
                        icon: "md-add",
                        fun: function () {
                            _self.add();
                        },
                        enabled: function (disabled, data) {
                            return disabled;
                        }
                    },{
                        id: "edit",
                        name: "编辑",
                        icon: "md-create",
                        fun: function () {
                            _self.mod();
                        },
                        enabled: function (disabled, data) {
                            if (disabled && data[_this.Key]) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    }, {
                    //    id: "del",
                    //    name: "删除",
                    //    icon: "md-trash",
                    //    fun: function () {
                    //        _self.del();
                    //    },
                    //    enabled: function (disabled, data) {
                    //        if (disabled && data[_this.Key]) {
                    //            return true;
                    //        } else {
                    //            return false;
                    //        }
                    //    }
                    //}, {
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
                add: function (event) {
                    let _self = this;
                    _self.disabled = false;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.dataParam = ClearObject(_self.dataParam);  
                    _this.newRecord();
                },
                mod: function (event) {
                    let _self = this;
                    _self.disabled = false;
                    _this.backData = DeepClone(_self.dataParam);                   
                },
                save: function (event) {
                    let _self = this;

                    if (!_this.IsValidSave())
                        return;

                    _.Ajax('Save', {
                        DefineSave: _self.dataParam
                    }, function (data) {
                        _self.disabled = true;
                        iview.Message.info("保存成功");

                        if (window.parent.search != undefined) {
                            window.parent.search.popCallBack(data);
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

                            _this.cancelAfter();
                        }
                        if (!flag) {
                            _this.initDataParam();
                        }

                        _self.disabled = true;
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
                            _self.disabled = true;
                            iview.Message.info("删除成功");

                            if (window.parent.search != undefined) {
                                window.parent.search.popCallBack(data);
                            }
                        });
                    });
                },
            }
        };

        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.vueObj = new Vue(options);
    }

    this.showOne = function (data, callback) { }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.initDataParam();
        _this.vue();

        if (_this.Id) {
            _this.showOne(_this.Id);
            _this.vueObj.disabled = true;
        } else {
            _this.vueObj.disabled = false;
        }
    }, 100);
}
var defineDetail = new _DefineDetail();
