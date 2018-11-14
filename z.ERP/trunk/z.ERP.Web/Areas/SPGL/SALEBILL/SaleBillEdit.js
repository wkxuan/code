editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //初始化弹窗所要传递参数

    editDetail.screenParam.showPopSyyuser = false;
    editDetail.screenParam.srcPopSyyuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";

    editDetail.screenParam.showPopGoods = false;
    editDetail.screenParam.srcPopGoods = __BaseUrl + "/" + "Pop/Pop/PopGoodsShopList/";


    editDetail.screenParam.popParam = {};
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.POSNO = null;
    editDetail.dataParam.ACCOUNT_DATE = null;
    editDetail.dataParam.STATUS = null;
    editDetail.dataParam.CASHIERID = null;
    editDetail.dataParam.CLERKID = null;
    editDetail.dataParam.DESCRIPTION = null;

    editDetail.screenParam.colDef = [        
        { title: "商品代码", key: "GOODSDM", width: 150, },
        { title: '商品名称', key: 'NAME', width: 100 },
        { title: '商铺代码', key: 'CODE', width: 100 },
        {
            title: '收款方式', key: 'PAYID', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.PAYID
                    },
                    on: {
                        'on-enter': function (event) {
                            _self = this;
                            editDetail.dataParam.SALEBILLITEM[params.index].PAYID = event.target.value;
                            _.Ajax('GetPay', {
                                Data: { PAYID: event.target.value }
                            }, function (data) {
                                if (data.dt) {
                                    Vue.set(editDetail.dataParam.SALEBILLITEM[params.index], 'PAYNAME', data.dt.NAME);
                                } else {
                                    iview.Message.info('当前收款方式不存在!');
                                }
                            });
                        }
                    },
                })
            },
        },
        { title: '收款方式名称', key: 'PAYNAME', width: 100 },
        {
            title: '收款金额', key: 'AMOUNT', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.AMOUNT
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.SALEBILLITEM[params.index].AMOUNT = event.target.value;
                        }
                    },
                })
            },
        },
        {
            title: '操作',
            key: 'action',
            width: 80,
            align: 'center',
            render: function (h, params) {
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled: false },

                        style: { marginRight: '50px' },
                        on: {
                            click: function (event) {
                                editDetail.dataParam.SALEBILLITEM.splice(params.index, 1);
                            }
                        },
                    }, '删除')
                    ]);
            }
        }
    ]
    if (!editDetail.dataParam.SALEBILLITEM) {
        editDetail.dataParam.SALEBILLITEM = [{
        }]
    };
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneSaleBillEdit', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.saleBill[0]);
        editDetail.dataParam.SALEBILLITEM = data.saleBillItem[0];
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    SelSyysuser: function () {        
        editDetail.screenParam.showPopSyyuser = true;
        editDetail.screenParam.popParam = { USER_TYPE: "1"};
        btnFlag = "SYYFlag";
    },
    SelYyysuser: function () {
        editDetail.screenParam.showPopSyyuser = true;
        editDetail.screenParam.popParam = { USER_TYPE: "1,2" };
        btnFlag = "YYYFlag";
    },
    srchColGoods: function () {
        if (!editDetail.dataParam.CLERKID)
        {
            iview.Message.info("营业员不能为空!");
            exit;
        }
        editDetail.screenParam.showPopGoods = true;
        editDetail.screenParam.popParam = { YYY: editDetail.dataParam.CLERKID, STATUS:"2"};
    },
    delColGoods: function () {
        var selectton = this.$refs.selectBrand.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的商品!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.SALEBILLITEM.length; j++) {
                    if (editDetail.dataParam.SALEBILLITEM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.SALEBILLITEM.splice(j, 1);
                    }
                }
            }
        }
    }
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {

    if (editDetail.screenParam.showPopSyyuser) {
        editDetail.screenParam.showPopSyyuser = false;
        if (btnFlag == "SYYFlag") {
            editDetail.dataParam.CASHIERID = data.sj[0].USERID;
            editDetail.dataParam.SYYMC = data.sj[0].USERNAME;
        }
        else if (btnFlag == "YYYFlag") {
            editDetail.dataParam.CLERKID = data.sj[0].USERID;
            editDetail.dataParam.YYYMC = data.sj[0].USERNAME;
        }
    }
    if (editDetail.screenParam.showPopGoods) {
        editDetail.screenParam.showPopGoods = false;
        if (editDetail.dataParam.SALEBILLITEM.length > 0) {
            if (!editDetail.dataParam.SALEBILLITEM[0].GOODSID) {
                editDetail.dataParam.SALEBILLITEM.splice(0, 1);
            }
        }
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.SALEBILLITEM.push({
                GOODSID: data.sj[i].GOODSID,
                GOODSDM: data.sj[i].GOODSDM,
                NAME: data.sj[i].NAME,
                CODE: data.sj[i].CODE,
                SHOPID: data.sj[i].SHOPID,
                QUANTITY: 1,
                PAYID: 1,
                PAYNAME: '现金',
                AMOUNT:0
            });
        }
    }
}

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.POSNO = null;
    editDetail.dataParam.ACCOUNT_DATE = null;
    editDetail.dataParam.STATUS = null;
    editDetail.dataParam.CASHIERID = null;
    editDetail.dataParam.CLERKID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.SALEBILLITEM = [];
}

editDetail.IsValidSave = function () {

    editDetail.dataParam.POSNO = ("000000" + editDetail.dataParam.BRANCHID + '0999').substr(-6);
    if (!editDetail.dataParam.ACCOUNT_DATE) {
        iview.Message.info("请确认记账日期!");
        return false;
    };

    if (!editDetail.dataParam.CASHIERID) {
        iview.Message.info("请确认收银员!");
        return false;
    };
    if (!editDetail.dataParam.CLERKID) {
        iview.Message.info("请确认营业员!");
        return false;
    };
    if (editDetail.dataParam.SALEBILLITEM.length == 0) {
        iview.Message.info("请确认销售明细!");
        return false;
    };

    return true;
}