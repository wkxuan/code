function _Define() {
    var _this = this;
    //界面打开的查询以及保存完之后调用的查询
    this.search = function () { }
    //vue之前的操作
    this.beforeVue = function () { }

    //功能页面有子表表格在列表信息选中后单独赋值处理
    this.afterCurrentData = function (currentRow) { }

    //vue操作
    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#def_Main',
            data: {
                //dataParam 数据库交互需要传输的内容
                //screenParam屏幕显示的内容
                dataParam: _this.dataParam,
                screenParam: _this.screenParam
            },
            mounted: function () {
                //页面打开先查询列表信息
                _this.search();
            },
            methods: {
                //dataOldParam过渡信息,在添加,修改放弃后定位到原始信息
                //添加
                add: function (event) {
                    _this.dataOldParam = _this.dataParam;
                    _this.dataParam = {};
                    ve.dataParam = _this.dataParam
                },
                //修改
                mod: function (event) {
                    _this.dataOldParam = _this.dataParam;
                },
                //保存
                save: function (event) {
                    _.Ajax('Save', {
                        DefineSave: ve.dataParam
                    }, function (data) {
                        _this.search();
                        alert("保存成功");
                    });
                },
                quit: function (event) {
                    _this.dataParam = _this.dataOldParam;
                    ve.dataParam = _this.dataParam
                },
                //删除
                del: function (event) {
                    _.Ajax('Delete', {
                        DefineDelete: ve.dataParam
                    }, function (data) {
                        _this.search();
                        alert("删除成功");
                    });
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