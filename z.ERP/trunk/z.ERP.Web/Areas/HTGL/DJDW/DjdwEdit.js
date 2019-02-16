editDetail.beforeVue = function () {
    editDetail.branchid = true;
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.dataParam.STATUS = 1;
    editDetail.dataParam.STYLE = 3;
    editDetail.dataParam.JXSL = 0;
    editDetail.dataParam.XXSL = 0;
    editDetail.dataParam.STANDARD = 1;

    editDetail.dataParam.OPERATERULE = 1;

    //初始化弹窗所要传递参数

    editDetail.screenParam.ParentMerchant = {};
    editDetail.screenParam.ParentShop = {};
    editDetail.screenParam.ParentFeeSubject = {};

    editDetail.screenParam.PopSysuser = false;
    editDetail.screenParam.srcPopSigner = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    editDetail.screenParam.popParam = {};

    //商铺表格
    editDetail.screenParam.colDefSHOP = [
    { type: 'selection', width: 60, align: 'center' },
    {
        title: "商铺代码", key: 'CODE', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.CODE
                },
                on: {
                    'on-enter': function (event) {
                        _self = this;
                        editDetail.dataParam.CONTRACT_SHOP[params.index].CODE = event.target.value;
                        _.Ajax('GetShop', {
                            Data: { CODE: event.target.value, BRANCHID: editDetail.dataParam.BRANCHID }
                        }, function (data) {
                            if (data.dt) {
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'SHOPID', data.dt.SHOPID);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYID', data.dt.CATEGORYID);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYCODE', data.dt.CATEGORYCODE);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYNAME', data.dt.CATEGORYNAME);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'AREA', data.dt.AREA_BUILD);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'AREA_RENTABLE', data.dt.AREA_RENTABLE);
                                calculateArea();
                            }
                            else {
                                iview.Message.info('当前单元代码不存在或者不属于当前分店卖场!');
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CODE', "");
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'SHOPID', "");
                            }
                        });
                    }
                },
            })
        },
    },
    { title: '业态代码', key: 'CATEGORYCODE', width: 100 },
    { title: '业态名称', key: 'CATEGORYNAME', width: 100 },
    { title: '建筑面积', key: 'AREA', width: 100 },
    { title: '租用面积', key: 'AREA_RENTABLE', width: 100 }
    ];

    //收费项目
    editDetail.screenParam.colDefCOST = [
        { type: 'selection', width: 60, align: 'center', },
        {
            title: "费用项目", key: 'TREMID', width: 95,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.TREMID
                    },
                    on: {
                        'on-enter': function (event) {
                            _self = this;
                            editDetail.dataParam.CONTRACT_COST_DJDW[params.index].TREMID = event.target.value;

                            _.Ajax('GetFeeSubject', {
                                Data: { TRIMID: event.target.value }
                            }, function (data) {
                                if (data.dt) {
                                    Vue.set(editDetail.dataParam.CONTRACT_COST_DJDW[params.index], 'NAME', data.dt.NAME);
                                } else {
                                    iview.Message.info('当前费用项目不存在!');
                                }
                            });
                        }
                    },
                })
            },
        },
        { title: "费用项目名称", key: 'NAME', width: 120 },
        {
            title: "金额", key: 'COST', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.COST
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.CONTRACT_COST_DJDW[params.index].COST = event.target.value;
                        }
                    },
                })
            },
        }
    ]


    //表格数据初始化
    editDetail.dataParam.CONTRACT_SHOP = editDetail.dataParam.CONTRACT_SHOP || [];

    editDetail.dataParam.CONTRACT_COST_DJDW = editDetail.dataParam.CONTRACT_COST_DJDW || [];

    calculateArea = function () {
        editDetail.dataParam.AREA_BUILD = 0;
        editDetail.dataParam.AREAR = 0;
        for (var i = 0; i < editDetail.dataParam.CONTRACT_SHOP.length; i++) {
            if (editDetail.dataParam.CONTRACT_SHOP[i].SHOPID) {
                editDetail.dataParam.AREA_BUILD += editDetail.dataParam.CONTRACT_SHOP[i].AREA;
                editDetail.dataParam.AREAR += editDetail.dataParam.CONTRACT_SHOP[i].AREA_RENTABLE;
            }
        }
    }
}


editDetail.otherMethods = {
    //点击商户弹窗
    Merchant: function () {
        Vue.set(editDetail.screenParam, "PopMerchant", true);
    },
    //商户弹窗返回
    MerchantBack: function (val) {
        Vue.set(editDetail.screenParam, "PopMerchant", false);
        editDetail.dataParam.MERCHANTID = val.sj[0].MERCHANTID;
        editDetail.dataParam.MERNAME = val.sj[0].NAME;
    },

    //选择商铺弹窗
    srchColSHOP: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请确认分店卖场!");
            return false;
        } else {
            Vue.set(editDetail.screenParam, "PopShop", true);
            editDetail.screenParam.ParentShop = { BRANCHID: editDetail.dataParam.BRANCHID };
        }
    },
    //商铺返回弹窗
    ShopBack: function (val) {
        Vue.set(editDetail.screenParam, "PopShop", false);
        for (var i = 0; i < val.sj.length; i++) {
            editDetail.dataParam.CONTRACT_SHOP.push(val.sj[i]);
        };
        calculateArea();
    },

    srchCost: function () {
        Vue.set(editDetail.screenParam, "PopFeeSubject", true);
    },

    FeeSubjectBack: function (val) {
        Vue.set(editDetail.screenParam, "PopFeeSubject", false);
        var maxIndex = 1;
        for (var i = 0; i < val.sj.length; i++) {
            editDetail.dataParam.CONTRACT_COST_DJDW.push({
                TREMID: val.sj[i].TERMID,
                NAME: val.sj[i].NAME
            });
        };
    },

    //添加商铺
    addColSHOP: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认分店!');
            return;
        }
        var temp = editDetail.dataParam.CONTRACT_SHOP || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_SHOP = temp;
    },
    //删除商铺
    delColSHOP: function () {
        var selectton = this.$refs.selectShop.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的商铺!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.CONTRACT_SHOP.length; j++) {
                    if (editDetail.dataParam.CONTRACT_SHOP[j].SHOPID == selectton[i].SHOPID) {
                        editDetail.dataParam.CONTRACT_SHOP.splice(j, 1);
                        calculateArea();
                    }
                }
            }
        }
    },

    //添加租约收费项目信息
    addColCost: function () {
        var temp = editDetail.dataParam.CONTRACT_COST_DJDW || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_COST_DJDW = temp;
    },
    //删除租约收费项目信息
    delColCost: function () {
        var selectton = this.$refs.selectCost.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.CONTRACT_COST_DJDW.length; j++) {
                    if (editDetail.dataParam.CONTRACT_COST_DJDW[j].TREMID == selectton[i].TREMID) {
                        editDetail.dataParam.CONTRACT_COST_DJDW.splice(j, 1);
                    }
                }
            }
        }
    },
}


editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.dataParam.CONTRACT_COST_DJDW = [];
}

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请确认分店卖场!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.CONT_START) {
        iview.Message.info("请维护开始日期!");
        return false;
    };

    if (!editDetail.dataParam.CONT_END) {
        iview.Message.info("请维护结束日期!");
        return false;
    };

    if (editDetail.dataParam.CONTRACT_SHOP.length == 0) {
        iview.Message.info("请确定商铺!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.CONTRACT_SHOP.length; i++) {
            if (!editDetail.dataParam.CONTRACT_SHOP[i].SHOPID) {
                iview.Message.info("请确定商铺!");
                return false;
            };
        };
    };
    return true;
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchContract', {
        Data: { CONTRACTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.contract);
        editDetail.dataParam.BILLID = data.contract.CONTRACTID;
        editDetail.dataParam.CONTRACT_SHOP = data.contractShop;
        editDetail.dataParam.CONTRACT_COST_DJDW = data.contractCostDjdw;
        callback && callback(data);
    });
};