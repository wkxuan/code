function _Define() {
    var _this = this;
    //得到主键
    this.Key = undefined;
    this.splitVal = 0.3;
    this.backData;
    this.beforeVue = function () { }
    this.enabled = function (val) { return val; }
    this.isvisible = function (val) { return val; }
    this.btnChkvisible = false;
    this.alwaysenabled = true;
    this.IsValidSave = function () {
        return true;
    }
    this.IsValidMod = function () {
        return true;
    }
    //添加后初始化数据信息
    this.newRecord = function () { }
    this.beforeDel = function () {
        return true;
    }
    this.IsValidChk = function () {
        return true;
    }
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
                data: [],
                columns: [],
                splitVal: _this.splitVal,
            },
            mounted: function () {
                _this.showlist();
                _this.mountedInit();
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
            methods: {
                add: function (event) {
                    _this.backData = DeepClone(this.dataParam);
                    this.dataParam = ClearObject(this.dataParam);
                    this.disabled = _this.enabled(false);
                    _this.newRecord();
                },
                mod: function (event) {
                    if (!_this.IsValidMod())
                        return;
                    _this.dataParam = this.dataParam;
                    if (!this.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }                  
                    _this.backData = DeepClone(this.dataParam);

                    this.disabled = _this.enabled(false);
                },
                save: function (event) {
                    var _self = this;
                    if (!_this.IsValidSave())
                        return;
                    save(function (data) {
                        _self.disabled = _this.enabled(true);
                        _this.showlist();
                        _this.showOne(data);
                        iview.Message.info("保存成功");
                    })
                },
                quit: function (event) {
                    var _self = this;
                    _.MessageBox("是否取消？", function () {
                        let flag = false;
                        for (let item in _this.backData) {
                            flag = true;
                            _self.dataParam[item]= _this.backData[item];
                        }
                        if (!flag) {
                            _self.dataParam = ClearObject(_self.dataParam);
                        }
                        _self.disabled = _this.enabled(true);
                    });
                },
                del: function (event) {
                    var _self = this;
                    if (!_self.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }

                    if (!_this.beforeDel())
                        return;

                    _.MessageBox("是否删除？", function () {
                        deleteone(_self.dataParam, function () {
                            _self.dataParam = {};
                            _this.showlist();
                            iview.Message.info("删除成功");
                        });
                    });
                },
                chk: function (event) {
                    var _self = this;
                    if (!_self.dataParam[_this.Key]) {
                        iview.Message.error("请选择数据");
                        return;
                    }
                    if (!_this.IsValidChk())
                        return;

                    check(function (data) {
                        _this.showlist(function () {
                            _this.showOne(data, function () {
                                iview.Message.info("审核成功");
                            });
                        });
                    })
                },
                seachList: function () {
                    _this.showlist();
                },
                currentChange: function (currentRow, oldCurrentRow) {
                    _this.showOne(currentRow[_this.Key]);
                },
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        _this.myve = ve;
    
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
                _this.myve.data = data.rows;
                callback && callback();
            }
        })
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
                    _this.myve.dataParam = data.rows[0];
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
