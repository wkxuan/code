function _Define() {
    var _this = this;
    //界面打开的查询以及保存完之后调用的查询
    this.search = function () { }
    //vue之前的操作(主要是实现v-model绑定数据的声明)
    this.beforeVue = function () { }

    //功能页面有子表表格在列表信息选中后单独赋值处理
    this.afterCurrentData = function (currentRow) { }

    //控件是否可用，扩展函数
    this.enabled = function (val) { return val; }

    //校验保存前数据是否合法
    this.IsValidSave = function (_self) {
        return true;
    }
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
                screenParam: _this.screenParam,
                disabled: _this.enabled(true),
            },
            mounted: function () {
                //页面打开先查询列表信息
                _this.search();
            },
            methods: {
                //dataOldParam过渡信息,在添加,修改放弃后定位到原始信息
                //添加
                add: function (event) {
                    //copy添加前界面绑定的数据(深拷贝,浅拷贝)
                    _this.dataOldParam = JSON.parse(JSON.stringify(_this.dataParam));
                    _this.dataParam = {};
                    _this.newRecord();
                    ve.dataParam = _this.dataParam;
                    ve.disabled = _this.enabled(false);
                },
                //修改
                mod: function (event) {
                    //copy修改前界面绑定的数据(深拷贝,浅拷贝)
                    _this.dataOldParam = JSON.parse(JSON.stringify(_this.dataParam));
                    ve.disabled = _this.enabled(false);
                },
                //保存
                save: function (event) {
                    var _self = this;
                    if (!_this.IsValidSave(_self))
                        return;
                    _.Ajax('Save', {
                        DefineSave: ve.dataParam
                    }, function (data) {
                        //返回主键右边元素的数据可以,左边的列表数据如何刷新？
                        _this.search();
                        ve.disabled = _this.enabled(true);
                        _self.$Message.info("保存成功");
                    });
                },
                quit: function (event) {
                    this.$Modal.confirm({
                        title: '提示',
                        content: '是否取消',
                        onOk: function () {
                            _this.dataParam = _this.dataOldParam;
                            ve.dataParam = _this.dataParam;
                            ve.disabled = _this.enabled(true);
                        },
                        onCancel: function () {
                            ve.disabled = _this.enabled(false);
                            this.id = "关闭"
                        }
                    });

                },
                //删除
                del: function (event) {
                    var _self = this;
                    this.$Modal.confirm({
                        title: '提示',
                        content: '是否删除',
                        onOk: function () {
                            _.Ajax('Delete', {
                                DefineDelete: ve.dataParam
                            }, function (data) {
                                _this.search();
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
                currentData: function (currentRow, oldCurrentRow) {
                    _this.dataParam = currentRow;
                    ve.dataParam = _this.dataParam;
                    _this.afterCurrentData(currentRow);
                }
            }
        });
    }

    //初始化vue绑定的对象
    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
        _this.dataOldParam = {};
    }

    //延时
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var define = new _Define();