function _Search() {
    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.newCondition = function () { }

    this.vue = function VueOperate() {
        var ve = new Vue({
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                panelName: 'condition',
                disabled: _this.enabled(true),

            },
            methods: {
                //查询
                seach: function (event) {
                    var _self = this;

                    event.stopPropagation();

                    _this.newCondition();

                    _.Search({
                        Service: _this.service,
                        Method: _this.method,
                        Data: _this.searchParam,
                        Success: function (data) {
                            if (data.rows.length > 0) {
                                ve.screenParam.dataDef = data.rows;
                                ve.panelName = 'result';
                            }
                            else {
                                _self.$Message.info("没有满足当前查询条件的结果!");
                            }
                        }
                    })
                },
                //清空
                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.screenParam.dataDef = [];
                    ve.panelName = 'condition';
                },
                //导出
                exp: function (event) {
                    event.stopPropagation();
                },
                //取消
                print: function (event) {
                    event.stopPropagation();
                },
            }
        });
    }
    this.vueInit = function () {
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
        _this.colBase = [
            {
                type: 'selection',
                width: 60,
                align: 'center',
                fixed: 'left',
            }
            ,{
            title: '操作',
            key: 'action',
            width: 200,
            align: 'center',
            fixed: 'right',
            render: function(h, params){
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled:false },

                        style: { marginRight: '5px' },
                        on: {
                            click: function (event) {
                                alert("1");
                            }
                        },
                    }, '查看'),

                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled: false },
                        style: { marginRight: '5px' },
                        on: {
                            click: function (event) {
                                alert("2");
                            }
                        },
                        
                    }, '编辑'),

                    h('Button', {
                        props: { type: 'error', size: 'small', disabled: false },
                        style: { marginRight: '5px' },
                        on: {
                            click: function (event) {
                                alert("3");
                            }
                        },
                    }, '删除')

                    ]);
            }
        }];
    }
    setTimeout(function () {
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 200);
}
var search = new _Search();