function _EditDetail() {

    var _this = this;
    this.veObj;            //vue实例对象
    this.backData = {}; //新增或编辑前的原数据
    this.beforeVue = function () { }
    //存档前的验证数据函数
    this.IsValidSave = function () {
        return true;
    }
    //弹窗返回数据回调函数
    this.popCallBack = function (data) { };
    this.enabled = function (val) { return val; }
    this.clearKey = function () { }
    this.newRecord = function () { }
    //功能按钮配置数组
    this.btnConfig = [];
    this.vue = function VueOperate() {
        var options = {
            el: '#EditDetail',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                branchid: _this.branchid,
                disabled: _this.enabled(false),
                collapseValue: [1, 2],
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
                    }, {
                        id: "edit",
                        name: "编辑",
                        icon: "md-create",
                        fun: function () {
                            _self.edit();
                        },
                        enabled: function (disabled, data) {
                            if (!disabled && data.BILLID.length > 0 && data.STATUS < 2) {
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
                            if (!disabled && data.BILLID.length > 0 && data.STATUS < 2) {
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
                //添加
                add: function () {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.disabled = true;
                    _self.setObjItemEmpty(_self.dataParam);
                    _this.clearKey();
                    _this.newRecord();
                },
                //编辑
                edit: function () {
                    let _self = this;
                    _this.backData = DeepClone(_self.dataParam);
                    _self.disabled = true;
                },
                //删除
                del: function () {
                    let _self = this;
                    _self.$Modal.confirm(_.MessageBox("确认删除当前内容？", () => {
                        //_.Ajax('Delete', {
                        //    DeleteData: selectton
                        //}, function (data) {
                        //    _self.setObjItemEmpty(_self.dataParam);
                        //    _self.$Message.info("删除成功");                          
                        //});
                    }, () => {
                    }));
                },
                //清空对象属性的值
                setObjItemEmpty(obj) {
                    for (let item in obj) {
                        if (Array.isArray(obj[item])) {
                            obj[item] = [];
                        } else {
                            obj[item] = null;
                        }
                    }
                },
                //放弃
                abandon: function () {
                    let _self = this;
                    _self.$Modal.confirm(_.MessageBox("确认放弃正在编辑的内容？", () => {
                        _self.disabled = false;
                        _self.dataParam = _this.backData;
                    }, () => {
                        _self.disabled = true;
                    }));
                },
                //存档
                save: function (event) {
                    let _self = this;
                    _self.disabled = true;
                    if (!_this.IsValidSave())
                        return;
                    //_.Ajax('Save', {
                    //    SaveData: veObj.dataParam
                    //}, function (data) {
                    //    _this.showOne(data, function () {
                    //        veObj.dataParam = _this.dataParam;
                    //        _self.$Message.info("保存成功");
                    //    });
                    //});
                },
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
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
        _this.beforeVue();
        _this.vue();
        if (editDetail.Id) {
            editDetail.showOne(editDetail.Id);
        }
    }, 200);
}
var editDetail = new _EditDetail();