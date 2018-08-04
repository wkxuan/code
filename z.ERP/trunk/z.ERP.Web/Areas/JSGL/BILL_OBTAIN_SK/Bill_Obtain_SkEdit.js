editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //保证金收款
    editDetail.dataParam.TYPE = 3;
    editDetail.dataParam.ALL_MONEY = 0;
    //初始化弹窗所要传递参数
    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopMerchant = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";

    editDetail.screenParam.popParam = {};

    editDetail.screenParam.colDef = [
    { title: '账单号', key: 'FINAL_BILLID', width: 100 },
    { title: '权债年月', key: 'YEARMONTH', width: 100 },
    { title: '租约号', key: 'CONTRACTID', width: 100 },
    { title: '收费项目', key: 'TERMMC', width: 150 },
    { title: '应收金额', key: 'MUST_MONEY', width: 100 },
    { title: '未付金额', key: 'UNPAID_MONEY', width: 100 },
    {
        title: "付款金额", key: 'RECEIVE_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.RECEIVE_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_OBTAIN_ITEM[params.index].RECEIVE_MONEY = event.target.value;
                        let sumJE = 0;
                        for (var i = 0; i < editDetail.dataParam.BILL_OBTAIN_ITEM.length; i++) {
                            sumJE += parseInt(editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY);
                        }
                        editDetail.dataParam.ALL_MONEY = sumJE;
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
                            editDetail.dataParam.BILL_OBTAIN_ITEM.splice(params.index, 1);
                        let sumJE = 0;
                        for (var i = 0; i < editDetail.dataParam.BILL_OBTAIN_ITEM.length; i++) {
                            sumJE += parseInt(editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY);
                        }
                        editDetail.dataParam.ALL_MONEY = sumJE;
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];

    if (!editDetail.dataParam.BILL_OBTAIN_ITEM) {
        editDetail.dataParam.BILL_OBTAIN_ITEM = [{
            FINAL_BILLID: "",
            RECEIVE_MONEY: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_OBTAIN_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_OBTAIN__ITEM = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtain);
        editDetail.dataParam.BILL_OBTAIN_ITEM = data.billObtainItem;
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    SelMerchant: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        };
        editDetail.screenParam.showPopMerchant = true;
    },
    SelBill: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        };
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请选择商户!");
            return;
        };
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
    }
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;
        let SumJE = 0;
        var sjitem = [];
        for (var i = 0; i < data.sj.length; i++) {
            sjitem.push({ FINAL_BILLID: data.sj[i].BILLID,YEARMONTH:data.sj[i].YEARMONTH,CONTRACTID:data.sj[i].CONTRACTID,
                TERMMC:data.sj[i].TERMMC,
                MUST_MONEY:data.sj[i].MUST_MONEY,
                UNPAID_MONEY:data.sj[i].MUST_MONEY,
                RECEIVE_MONEY: data.sj[i].MUST_MONEY,
                TYPE:1
            });
            SumJE += data.sj[i].MUST_MONEY;
        }
        editDetail.dataParam.BILL_OBTAIN_ITEM = sjitem;
        editDetail.dataParam.ALL_MONEY = SumJE;
    }
    else if (editDetail.screenParam.showPopMerchant) {
        editDetail.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
}

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.YEARMONTH = null;
    editDetail.dataParam.PAYID = null;
    editDetail.dataParam.PAYNAME = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.MERCHANTNAME = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_OBTAIN_ITEM = [];
}

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.PAYID) {
        iview.Message.info("请选择付款方式!");
        return false;
    };
    if (!editDetail.dataParam.NIANYUE) {
        iview.Message.info("请选择付款年月!");
        return false;
    };
    if (editDetail.dataParam.BILL_OBTAIN_ITEM.length == 0) {
        iview.Message.info("请录入账单信息!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.BILL_OBTAIN_ITEM.length; i++) {
            if (!editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY) {
                iview.Message.info("请录入付款金额!");
                return false;
            };
            if (editDetail.dataParam.BILL_OBTAIN_ITEM[i].UNPAID_MONEY > 0 &&
                (editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY > editDetail.dataParam.BILL_OBTAIN_ITEM[i].UNPAID_MONEY))
            {
                iview.Message.info("单号[" + editDetail.dataParam.BILL_OBTAIN_ITEM[i].FINAL_BILLID + "] 的付款金额不能大于未付款金额!");
                return false;
            }
            if (editDetail.dataParam.BILL_OBTAIN_ITEM[i].UNPAID_MONEY < 0 &&
                (editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY < editDetail.dataParam.BILL_OBTAIN_ITEM[i].UNPAID_MONEY)) {
                iview.Message.info("单号[" + editDetail.dataParam.BILL_OBTAIN_ITEM[i].FINAL_BILLID + "]当为负数金额时，付款金额不能小于未付款金额!");
                return false;
            }
        };
    };

    return true;
}