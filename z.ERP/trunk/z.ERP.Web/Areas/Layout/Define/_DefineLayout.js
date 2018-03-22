function _Define() {
    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.IsValidSave = function () {
        return true;
    }

    //添加后初始化数据信息
    this.newRecord = function () { }

    //得到主键
    this.Key = undefined;


    this.vue = function VueOperate() {
        var options = {
            el: '#def_Main',
            data: {
                dataParam: _this.dataParam,
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                disabled: _this.enabled(true),
                _key: undefined,
            },
            mounted: function () {
                let h = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
                this.$refs.cardHeigth.style.height = (h - 40) + 'px';
                this.$refs.tableHeight.style.height = (h - 40) + 'px';

                _.Search({
                    Service: _this.service,
                    Method: _this.methodList,
                    Data: {},
                    Success: function (data) {
                        define.screenParam.dataDef = data.rows;
                    }
                })
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
                                _this.showone(ve._key);
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
                    $.extend(_this.dataParam , currentRow);
                    ve._key = define.dataParam[_this.Key];
                    _this.showone(ve._key);
                    ve.dataParam = _this.dataParam;
                },
                seachList: function (event) {
                    showlist();
                }
            }
        };
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);

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
    }

    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var define = new _Define();
