function _Search() {

    var _this = this;

    this.beforeVue = function () { }

    this.enabled = function (val) { return val; }

    this.newCondition = function () { }


    this.IsValidSrch = function () {
        return true;
    }


    this.canEdit = function (mess) {
        return true;
    }
    this.mountedInit = function () { }
    this.vue = function VueOperate() {
        var options = {
            el: '#search',
            data: {
                screenParam: _this.screenParam,
                searchParam: _this.searchParam,
                pageInfo:_this.pageInfo,
                panelName: 'condition',
                disabled: _this.enabled(true),

                screenParamData: {
                    dataDef: []
                },
                pagedataCount: 0,
                pageSize: 10,
            },

            mounted: function () {
                _this.mountedInit();        
            },
            methods: {
                seach: function (event) {
                    event.stopPropagation();
                    var mess = this;
                    if (!_this.IsValidSrch())
                        return;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = 0;
                    Vue.set(ve.screenParamData, "dataDef", []);
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });
                },

                clear: function (event) {
                    event.stopPropagation();
                    _this.searchParam = {};
                    ve.searchParam = _this.searchParam;
                    ve.screenParamData.dataDef = [];
                    ve.panelName = 'condition';
                    _this.newCondition();
                },
                //导出待完善
                exp: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要导出的数据!");
                    } else {
                        this.$Message.error("尚未提供导出方法!");
                    }

                },
                //打印待完善
                print: function (event) {
                    event.stopPropagation();
                    if (notExistsData()) {
                        this.$Message.error("没有要打印的数据!");
                    } else {
                        this.$Message.error("尚未提供打印方法!");
                    }

                },

                add: function (event) {
                    event.stopPropagation();
                    _this.addHref();
                },
                browse: function (row, index) {
                    //if (CanEdit)
                   // _this.browseHref(row, index); //放按钮出来
                },

                del: function (event) {
                    event.stopPropagation();
                    var _self = this;
                    console.log(this);
                    var selectton = this.$refs.selectData.getSelection();
                    if (selectton.length == 0) {
                        this.$Message.info("请选中要删除的数据!");
                    }
                    else {
                        this.$Modal.confirm({
                            title: '提示',
                            content: '是否删除',
                            onOk: function () {
                                _.Ajax('Delete', {
                                    DeleteData: selectton
                                }, function (data) {
                                    showList(function (data) {
                                        Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                                        _self.$Message.info("删除成功");
                                    });
                                });
                            },
                            onCancel: function () {
                                this.id = "关闭"
                            }
                        });
                    }
                },
                changePageCount: function (index) {
                    let mess = this;
                    _this.pageInfo.PageSize = mess.pageSize;
                    _this.pageInfo.PageIndex = (index - 1);

                    Vue.set(ve.screenParamData, "dataDef", []);
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });

                },

          /*      changePageSizer: function (value) {
                    let mess = this;
                    _this.pageInfo.PageSize = value;
                    Vue.set(ve.screenParamData, "dataDef", []);
                    showList(function (data) {
                        if (_this.screenParam.dataDef.length > 0) {
                            ve.panelName = 'result';
                            Vue.set(ve.screenParamData, "dataDef", _this.screenParam.dataDef);
                        }
                        else {
                            mess.$Message.info("没有满足当前查询条件的结果!");
                        }
                    });
                } */
            }
        }
        _this.otherMethods && $.extend(options.methods, _this.otherMethods);
        var ve = new Vue(options);
        function showList(callback) {
            ve.searchParam = _this.searchParam;
            ve.pageInfo=_this.pageInfo;

            _.Search({
                Service: _this.service,
                Method: _this.method,
                Data: ve.searchParam,
                PageInfo:ve.pageInfo,
                Success: function (data) {
                    _this.screenParam.dataDef = data.rows;
                    ve.pagedataCount = data.total;
                    callback && callback();
                }
            })
        };
        function notExistsData() {
            return (!ve.screenParamData.dataDef) || (ve.screenParamData.dataDef.length == 0)
        }
    }

    this.addHref = function () {
    }

    this.modHref = function (row, index) {
    }



    this.browseHref = function (row, index) {
    }


    this.colDefInit = function () {
        _this.colMul = [{
            type: 'selection',
            width: 60,
            align: 'center',
            fixed: 'left',
        }];

        _this.colOperate = [{
            title: '操作',
            key: 'action',
            width: 160,
            align: 'center',
            fixed: 'right',
            render: function (h, params) {
                    return h('div',
                          [
                           (CanBrowse)  &&  h('Button',
                                {
                                    props: { type: 'primary', size: 'small', disabled: false },
                                    style: { marginRight: '1px' },
                                    on: { click: function (event) { _this.browseHref(params.row, params.index) } },
                                }, '浏览'),
                           (CanEdit) &&  h('Button',
                                  {
                                      props: { type: 'primary', size: 'small', disabled: false },
                                      style: { marginRight: '1px' },
                                      on: { click: function (event) { _this.modHref(params.row, params.index) } },

                                  }, '修改'),
                           (CanBg) && h('Button',
                                  {
                                      props: { type: 'primary', size: 'small', disabled: false },
                                      style: { marginRight: '1px' },
                                      on: { click: function (event) { _this.bgHref(params.row, params.index) } },

                                  }, '变更'),
                          ]
                    )

                /*  if ((!CanEdit) && (!CanExec)) {
                      return h('div',
                          []
                      );
                  }
                  else { 

                     if ((CanEdit) && (!CanExec)) {
                        return h('div',
                            [
                                h('Button',
                                {

                                    props: { type: 'primary', size: 'small', disabled: false },
                                    style: { marginRight: '5px' },

                                    on: { click: function (event) { _this.modHref(params.row, params.index) } },

                                }, '修改')
                            ]
                        );
                    };
                    if ((!CanEdit) && (CanExec)) {
                        return h('div',
                            [
                                 h('Button',
                                 {
                                     props: { type: 'error', size: 'small', disabled: false },
                                     style: { marginRight: '5px' },
                                     on: { click: function (event) { _this.browseHref(params.row, params.index) } },
                                 }, '浏览')

                            ]
                        );
                    };
                    if ((CanEdit) && (CanExec)) {
                        return h('div',
                            [
                                h('Button',
                                {

                                    props: { type: 'primary', size: 'small', disabled: false },
                                    style: { marginRight: '5px' },

                                    on: { click: function (event) { _this.modHref(params.row, params.index) } },

                                }, '修改'),


                                 h('Button',
                                 {
                                     props: { type: 'error', size: 'small', disabled: false },
                                     style: { marginRight: '5px' },
                                     on: { click: function (event) { _this.browseHref(params.row, params.index) } },
                                 }, '浏览')

                            ]
                        );
                    }   
                }*/
            }
        }]
    };



    this.vueInit = function () {
        _this.pageInfo = {};
        _this.searchParam = {};
        _this.screenParam = {};
        _this.service = "";
        _this.method = "";
    };

    setTimeout(function () {
        _this.colDefInit();
        _this.vueInit();
        _this.beforeVue();
        _this.vue();
    }, 100);
}
var search = new _Search();