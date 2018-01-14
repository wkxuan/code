function _Define() {
    var _this = this;

    //vue之前的操作(主要是实现v-model绑定数据的声明)
    this.beforeVue = function () { }

    //控件是否可用，扩展函数,待完善
    this.enabled = function (val) { return val; }

    //校验保存前数据是否合法,前端验证，非空的可以加上
    this.IsValidSave = function (_self) {
        return true;
    }

    //添加的标记
    this.AddTar;

    this.Key;

    //添加后初始化数据信息
    this.newRecord = function () { }

    //vue操作
    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                //dataParam 数据库交互需要传输的内容
                //screenParam屏幕显示的内容
                dataParam: _this.dataParam,
                windowParam: _this.windowParam,
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                disabled: _this.enabled(true),
                _key: undefined
            },
            mounted: showlist,
            methods: {
                //添加
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
                //修改
                mod: function (event) {
                    if (!ve._key) {
                        this.$Message.error("请选择数据");
                        return;
                    }
                    ve._key = define.dataParam[_this.Key];
                    ve.disabled = _this.enabled(false);
                },
                //保存
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
                //取消
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
                //删除
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
                                //删除完,界面清空
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
                //列表选中
                //参数:currentRow当前数据
                //oldCurrentRow上一次选中的数据
                showlist: function (currentRow, oldCurrentRow) {
                    var p = currentRow && currentRow[0] && currentRow[0];
                    this.Key = p.code;
                    if (p.children.length == 0)
                        showone(p.code);
                },
                seachList: function (event) {
                    showlist();
                }
            }
        });

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
                Key: ve.Key,
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

    //初始化vue绑定的对象
    this.vueInit = function () {
        _this.dataParam = {};
        _this.windowParam = {};
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.methodList = "";
    }

    //延时
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var define = new _Define();
