/*
1>按钮增加如何扩展? 简单定义就这些按钮，若需要增加比如审核那就走单据模式
2>对于按钮enable包括初始是否可以点击,点击后是否可以点击(以及对其他编辑框的影响)两个地方(enabled方法)
3>_this.dataOldParam = JSON.parse(JSON.stringify(_this.dataParam)); 是否合适?
4>样式,当前样式是否合适,包括card是否适用  ok
5>保存后返回主键怎么同时刷新左边列表和右边元素(界面上不输入的信息) 两次查询
6>界面上是否要显示记录创建时间修改时间   
7>后端如何判断是第一次数据给创建时间数据信息(当主键是输入的时候)  
创建,修改时间简单定义暂时不用，统一方法记录变更信息。
8>数据库端查询数据的控件DataService是否是那种写法  是
9>验证子表绑定对象的一个属性,后端是否能够解析?  验证
10>整体的js模板还是否有优化的地方   简单定义模板在处理一下,左侧列表查询看一下
11>树形模板,弹窗如何处理方式?
12>编辑,查询不同业务操作控件展示形式?  
13>两个字段主键
14>删除时后端判断是否使用了?  提供一个公共的方法

以上问题的解答:
1:简单定义就保持这些按钮，若需要增加比如审核等权限控制的信息单据模式。
2:保持现有的enable属性
3:取消这样处理,放弃的时候查询一次.在添加,修改的时候记下当时的主键
4:保持目前样式
5:两次查询，左边列表查询回来的SQL语句只需要显示的字段,右边根据主键在次查询刷新右边元素信息(保存完之后左边的列表和右边都去数据库查一次去处理赋值)
6,7:ID，CODE保留一个。创建,修改时间简单定义暂时不用,要记录也用子表记录信息(理由:要记录也应该记录数据的修改情况)
8:是
9:待验证
10:定义模板左侧列表筛选的地方有待完善
11:优先处理弹窗
12:找一个功能画一个页面布局(单据维护页面)
13:联合主键在service里面用DbHelper操作数据库,简单通用方法使用不了,因为除了简单定义简单的处理之外，稍复杂的验证以及DB层操作都放在
service层操作
14:后端提供公共方法去调用
*/
function _Define() {
    var _this = this;
    //界面打开的查询
    this.search = function () { }

    //保存完之后根据主键查询右边元素
    this.searchElement = function (param) { }

    //vue之前的操作(主要是实现v-model绑定数据的声明)
    this.beforeVue = function () { }

    //控件是否可用，扩展函数
    this.enabled = function (val) { return val; }

    //校验保存前数据是否合法
    this.IsValidSave = function (_self) {
        return true;
    }
    //添加后初始化数据信息
    this.newRecord = function () { }
    
    //添加,修改预存主键信息
    this.getKey=function(){}

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
                _key: null
            },
            mounted: function () {
                //页面打开先查询列表信息
                _this.search();
            },
            methods: {
                //添加
                add: function (event) {
                    if (!ve.dataParam) {
                        ve._key = _this.getKey();
                    }
                    _this.dataParam = {};
                    _this.newRecord();
                    ve.dataParam = _this.dataParam;
                    ve.disabled = _this.enabled(false);
                },
                //修改
                mod: function (event) {
                    //点击修改肯定已经选中数据了
                    if (!ve.dataParam) {
                        ve._key = _this.getKey();
                    }
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
                        //返回左边列表
                        _this.search();
                        //返回右边元素
                        _this.searchElement(param);
                        ve.disabled = _this.enabled(true);
                        _self.$Message.info("保存成功");
                    });
                },
                quit: function (event) {
                    this.$Modal.confirm({
                        title: '提示',
                        content: '是否取消',
                        onOk: function () {
                            //取消后查原来的元素列表
                            if (!ve._key) {
                                _this.dataParam = _this.searchElement(ve._key);
                            }
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
                    _this.searchElement(_this.getKey());
                    ve.dataParam = _this.dataParam;
                }
            }
        });
    }

    //初始化vue绑定的对象
    this.vueInit = function () {
        _this.dataParam = {};
        _this.screenParam = {};
    }

    //延时
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var define = new _Define();