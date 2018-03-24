function _Define() {
    var _this = this;

    this.beforeVue = function () { }


    this.enabled = function (val) { return val; }


    this.IsValidSave = function (_self) {
        return true;
    }


    this.AddTar;

    this.Key;


    this.newRecord = function () { }


    this.vue = function VueOperate() {
        var options = {
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                disabled: _this.enabled(true),
                _key: undefined
            },
            mounted: showlist,
            methods: {
                
                addtj: function (event) {
                    _this.dataParam = {};
                    _this.newRecord();
                    this.AddTar = 'tj';
                    ve.dataParam = _this.dataParam;
                    ve.disabled = _this.enabled(false);
                },
                addxj: function (event) {
                    _this.dataParam = {};
                    _this.newRecord();
                    this.AddTar = 'xj';
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
                    save(function (data) {
                        showlist(function () {
                            showone(data, function () {
                                ve.disabled = _this.enabled(true);
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
                                showone(ve._key);
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
                    }
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
                    var p = currentRow && currentRow[0] && currentRow[0];
                    ve._key = p.code;
                    showone(p.code);
                },
                seachList: function (event) {
                    showlist();
                }
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showlist(callback) {
            _.SearchNoQuery({
                Service: _this.service,
                Method: _this.methodList,
                Success: function (data) {
                    define.screenParam.dataDef = data;
                    callback && callback();
                }
            })
        }

        function showone(key, callback) {
            if (key) {
                var v = {};
                _.Search({
                    Service: _this.service,
                    Method: _this.method,
                    Data: {
                        code: key
                    },
                    Success: function (data) {
                        _this.dataParam = data.rows[0];
                        ve.dataParam = _this.dataParam;
                        callback && callback();
                    }
                });
            }
        }

        function save(callback) {
            _.Ajax('Save', {
                Tar: ve.AddTar,
                Key: ve._key,
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
    }


    this.vueInit = function () {
        _this.dataParam = {};
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.methodList = "";
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var define = new _Define();
