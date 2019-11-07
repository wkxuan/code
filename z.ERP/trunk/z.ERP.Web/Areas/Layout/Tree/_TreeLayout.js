function _Define() {
    var _this = this;
    this.vueObj;
    this.AddTar;
    this.Key;
    this.backData;
    this.btnConfig = [];
    this.beforeVue = function () { }
    this.enabled = function (val) { return val; }
    this.IsValidSave = function (_self) {
        return true;
    }
    this.IsValidDel = function (_self) {
        return true;
    }
    this.IsValidTj = function () {
        return true;
    }
    this.IsValidXj = function () {
        return true;
    }
    this.initDataParam = function () { }
    this.newRecord = function () { }

    this.vue = function VueOperate() {
        var options = {
            el: '#treeDef',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                disabled: _this.enabled(true),
                nodedisabled: false,
                splitVal: 0.3,
                data: [],
                toolBtnList: []
            },
            mounted(){
                this.seachList();
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
                        id: "addtj",
                        name: "加同级",
                        icon: "md-add",
                        fun: function () {
                            _self.addtj();
                        },
                        enabled: function (disabled, data) {
                            if (disabled && data && data[_this.Key]) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    }, {
                        id: "addxj",
                        name: "加下级",
                        icon: "md-add",
                        fun: function () {
                            _self.addxj();
                        },
                        enabled: function (disabled, data) {
                            if (disabled && data && data[_this.Key]) {
                                return true;
                            } else {
                                return false;
                            }
                        }
                    },{
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
                    }, {
                        id: "del",
                        name: "删除",
                        icon: "md-trash",
                        fun: function () {
                            _self.del();
                        },
                        enabled: function (disabled, data) {
                            if (disabled && data && data[_this.Key]) {
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
                    if (_this.btnConfig.length) {
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
                    } else {
                        data = baseBtn;
                    } 
                    _self.toolBtnList = data;
                },
                addtj: function (event) {
                    _this.backData = DeepClone(this.dataParam);
                    if (this.data.length) {
                        if (!this.dataParam[_this.Key]) {
                            iview.Message.error("请选择数据");
                            return;
                        };
                    }
                    if (!_this.IsValidTj())
                        return;
                    this.dataParam = ClearObject(this.dataParam);
                    _this.newRecord();
                    _this.AddTar = 'tj';
                    this.disabled = false;
                    this.nodedisabled = true;
                },
                addxj: function (event) {
                    _this.backData = DeepClone(this.dataParam);
                    if (!this.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    };
                    if (!_this.IsValidXj())
                        return;
                    this.dataParam = ClearObject(this.dataParam);
                    _this.newRecord();
                    _this.AddTar = 'xj';
                    this.disabled = false;
                    this.nodedisabled = true;
                },          
                edit: function (event) {
                    if (!this.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    };
                    _this.backData = DeepClone(this.dataParam);
                    //修改的时候值传' ',否则全局变量值上次按钮的值
                    _this.AddTar = " ";
                    this.disabled = false;
                    this.nodedisabled = true;
                },       
                save: function (event) {
                    var _self = this;
                    _.Ajax('Save', {
                        Tar: _this.AddTar,
                        Key: _this.backData[_this.Key],
                        DefineSave: _self.dataParam
                    }, function (data) {
                        iview.Message.info("保存成功");
                        _this.showOne(data);
                        _self.disabled = true;
                        _self.seachList();
                        _self.nodedisabled = false;
                    });
                },     
                quit: function (event) {
                    var _self = this;
                    _.MessageBox("是否取消？", function () {
                        $.extend(_this.dataParam, _this.backData);
                        _self.disabled = true;
                        _self.nodedisabled = false;
                    });
                },               
                del: function (event) {
                    var _self = this;
                    if (!_self.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    };
                    if (!_this.IsValidDel())
                        return;
                    _.MessageBox("是否删除？", function () {
                        _.Ajax('Delete', {
                            DefineDelete: _self.dataParam
                        }, function (data) {
                            _self.dataParam = ClearObject(_self.dataParam);
                            iview.Message.info("删除成功");
                            _self.seachList();
                        });
                    });
                },      
                onselectchange: function (selectArr, node) {                 
                    _this.showOne(node.code);
                },
                seachList: function () {
                    var _self = this;
                    _.SearchNoQuery({
                        Service: _this.service,
                        Method: _this.methodList,
                        Success: function (data) {
                            if (data && data.length) {
                                _self.data = data;
                            }
                        }
                    })
                },               
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);

        _this.vueObj = new Vue(options);  
    }

    this.showOne = function (key, callback) {
        _.Search({
            Service: _this.service,
            Method: _this.method,
            Data: {
                code: key
            },
            Success: function (data) {
                $.extend(_this.dataParam, data.rows[0]);
                callback && callback();
            }
        });
    }

    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.methodList = "";
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.initDataParam();
        _this.vue();
    }, 100);
}
var define = new _Define();
