﻿function _EditDetail() {

    var _this = this;
    this.veObj;            //vue实例对象
    this.backData = {}; //新增或编辑前的原数据
    this.beforeVue = function () { }
    //存档前的验证数据函数
    this.IsValidSave = function () {
        return true;
    }
    //是否显示其它信息折叠面板
    this.otherPanel = true;
    //单据底部制单人等信息是否使用默认显示
    this.defaultFooter = true;
    //弹窗返回数据回调函数
    this.popCallBack = function (data) { };
    this.enabled = function (val) { return val; }
    this.clearKey = function () { }
    this.newRecord = function () { }
    //取消成功后
    this.afterAbandon = function () { }
    //功能按钮配置数组
    this.btnConfig = [];
    this.branchChange = function () { };
    this.vue = function VueOperate() {
        var options = {
            el: '#edit',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                branchid: _this.branchid,
                disabled: _this.enabled(false),
                collapseValue: [1, 2],
                toolBtnList: [],
                otherPanel: _this.otherPanel,
                defaultFooter: _this.defaultFooter,
                branchDisabled: false
            },
            mounted: function () {
                _this.mountedInit();
                this.initBtn();
            },
            watch: {},
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
                    }, {
                        id: "edit",
                        name: "编辑",
                        icon: "md-create",
                        fun: function () {
                            _self.edit();
                        },
                        enabled: function (disabled, data) {
                            if (!disabled && data && data.BILLID && data.STATUS < 2) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    }, {
                        id: "del",
                        name: "删除",
                        icon: "md-trash",
                        fun: function () {
                            _self.del();
                        },
                        enabled: function (disabled, data) {
                            if (!disabled && data && data.BILLID && data.STATUS < 2) {
                                return true;
                            } else {
                                return false;
                            }
                        }
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
                            _self.abandon();
                        },
                        enabled: function (disabled, data) {
                            return disabled;
                        }
                    }];
                    let data = [];
                    for (let j = 0, jlen = _this.btnConfig.length; j < jlen; j++) {
                        for (let i = 0, ilen = baseBtn.length; i < ilen; i++) {
                            if (baseBtn[i].id == _this.btnConfig[j].id) {
                                let loc = {};
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
                //添加
                add: function () {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.disabled = true;
                    _self.branchDisabled = false;
                    _self.dataParam = _self.clearObj(_self.dataParam);
                    _this.newRecord();                 
                },
                //编辑
                edit: function () {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.disabled = true;
                    _self.branchDisabled = false;
                },
                //删除
                del: function () {
                    let _self = this;
                    _.MessageBox("确认删除当前内容？", function () {
                        _.Ajax('Delete', {
                            DeleteData: [_self.dataParam]
                        }, function (data) {
                            iview.Message.info("删除成功");

                            let win = window.location;
                            let pathnameArr = win.pathname.split("/");
                            pathnameArr[pathnameArr.length - 1] = null;
                            win.replace(pathnameArr.join("/"));
                        });
                    });
                },
                //放弃
                abandon: function () {
                    let _self = this;
                    _.MessageBox("确认放弃正在编辑的内容？", function () {
                        _self.disabled = false;
                        _self.branchDisabled = false;
                        let flag = false;
                        for (let item in _this.backData) {
                            flag = true;
                            _self.dataParam[item] = _this.backData[item];
                        }
                        if (!flag) {
                            _self.dataParam = _self.clearObj(_self.dataParam);
                        }
                        _this.afterAbandon();
                    }, function () {
                        _self.disabled = true;
                    });
                },
                //存档
                save: function (event) {
                    let _self = this;
                    if (!_this.IsValidSave())
                        return;
                    _.Ajax('Save', {
                        SaveData: _this.veObj.dataParam
                    }, function (data) {
                        iview.Message.info("保存成功");

                        let win = window.location;
                        let pathnameArr = win.pathname.split("/");
                        pathnameArr[pathnameArr.length - 1] = data;
                        win.replace(pathnameArr.join("/"));
                    });
                },
                branchChange: function () {
                    _this.branchChange();
                },
                clearObj: function (obj) {
                    for (let item in obj) {
                        if (Array.isArray(obj[item])) {
                            obj[item] = [];
                        } else {
                            obj[item] = null;
                        }
                    }
                    return obj;
                }
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        _this.otherComputed && $.extend(options.computed, _this.otherComputed);
        _this.otherWatch && $.extend(options.watch, _this.otherWatch);

        _this.veObj = new Vue(options);
    }

    this.showOne = function (data, callback) { }

    this.mountedInit = function () { }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.branchid = true;
    };

    setTimeout(function () {
        _this.vueInit();
        _this.clearKey();
        _this.beforeVue();
        _this.vue();
        if (editDetail.Id) {
            editDetail.showOne(editDetail.Id);
        } else {
            _this.veObj.disabled = true;
        }
    }, 200);
}
var editDetail = new _EditDetail();