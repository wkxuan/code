editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';
    //保证金收款
    editDetail.dataParam.TYPE = 3;
    editDetail.dataParam.ALL_MONEY = 0;
    editDetail.dataParam.ADVANCE_MONEY = 0;
    editDetail.dataParam.MERCHANT_MONEY = 0;
    //初始化弹窗所要传递参数
    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopMerchant = false;
    editDetail.screenParam.showPopInvoice = false;
    editDetail.screenParam.srcPopInvoice = __BaseUrl + "/" + "Pop/Pop/PopInvoiceList/";
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";

    editDetail.screenParam.popParam = {};

    editDetail.screenParam.colDef = [
    { title: '账单号', key: 'FINAL_BILLID', width: 100 },
    { title: '权债年月', key: 'NIANYUE', width: 100 },
    { title: '收付实现月', key: 'YEARMONTH', width: 100 },
    { title: '租约号', key: 'CONTRACTID', width: 100 },
    { title: '收费项目', key: 'TERMMC', width: 200 },
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

    //发票pop
    editDetail.screenParam.colDefI = [
        { title: "发票号码", key: "INVOICENUMBER", width: 120},
        { title: "商户", key: "MERCHANTNAME", width: 200},
        { title: "发票类型", key: "TYPENAME", width: 100 },
        {
            title: "开票日期", key: "INVOICEDATE", width: 115,
            render: function (h, params) {
                return h('div',
                    this.row.INVOICEDATE.substr(0, 10));
            }
        },
        { title: "不含税金额", key: "NOVATAMOUNT", width: 100 },
        { title: "增值税金额", key: "VATAMOUNT", width: 100 },
        { title: "发票金额", key: "INVOICEAMOUNT", width: 100 },
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
                                editDetail.dataParam.BILL_OBTAIN_INVOICE.splice(params.index, 1);
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
    //发票数据初始化
    if (!editDetail.dataParam.BILL_OBTAIN_INVOICE) {
        debugger
        editDetail.dataParam.BILL_OBTAIN_INVOICE = []
    }
    editDetail.screenParam.addCol = function () {
        debugger
        var temp = editDetail.dataParam.BILL_OBTAIN_ITEM || [];
        var tempI = editDetail.dataParam.BILL_OBTAIN_INVOICE || [];
        temp.push({});
        tempI.push({});
        editDetail.dataParam.BILL_OBTAIN_ITEM = temp;
        editDetail.dataParam.BILL_OBTAIN_INVOICE = tempI;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtain);
        editDetail.dataParam.BILL_OBTAIN_ITEM = data.billObtainItem;
        editDetail.dataParam.BILL_OBTAIN_INVOICE = data.billObtainInvoice || [];
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
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID};
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
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID, FEE_ACCOUNTID: editDetail.dataParam.FEE_ACCOUNT_ID, WFDJ: 1 };
    },

    SelInvoice: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        };
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请选择商户!");
            return;
        };
        editDetail.screenParam.showPopInvoice = true;       
    },

    YfkChange: function () {
        if (parseFloat(editDetail.dataParam.ADVANCE_MONEY) > parseFloat(editDetail.dataParam.MERCHANT_MONEY)) {
            iview.Message.info("预付款金额不能大于商户余额!");
            editDetail.dataParam.ADVANCE_MONEY = "0";
            return;
        }
        let fkje = 0;
        for (var i = 0; i < editDetail.dataParam.BILL_OBTAIN_ITEM.length; i++) {
            fkje += parseFloat(editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY);
        };
        editDetail.dataParam.ADVANCE_MONEY = editDetail.dataParam.ADVANCE_MONEY.replace('-');   //限制输入负号
        editDetail.dataParam.ALL_MONEY = fkje - editDetail.dataParam.ADVANCE_MONEY;
    },
    balance: function () {
        //收款方式和商户不为空，验证余额，其余情况置未0
        if (editDetail.dataParam.MERCHANTNAME != null && editDetail.dataParam.MERCHANTNAME != undefined && editDetail.dataParam.FEE_ACCOUNT_ID != null && editDetail.dataParam.FEE_ACCOUNT_ID != undefined) {
            _.Ajax('SearchBalance', {
                Data: { MERCHANTID: editDetail.dataParam.MERCHANTID, FEE_ACCOUNT_ID: editDetail.dataParam.FEE_ACCOUNT_ID }
            }, function (data) {
                if (data.dt != null) {
                    editDetail.dataParam.MERCHANT_MONEY=data.dt.BALANCE;
                } else {
                    editDetail.dataParam.MERCHANT_MONEY= 0;
                }
            });
        } else {
            editDetail.dataParam.MERCHANT_MONEY= 0;
        }
        //收款方式和商户改变 账单置为空
        editDetail.dataParam.BILL_OBTAIN_ITEM = [];
    }
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;                
        if (editDetail.dataParam.BILL_OBTAIN_ITEM.length>0) {
            if (!editDetail.dataParam.BILL_OBTAIN_ITEM[0].FINAL_BILLID) {
                editDetail.dataParam.BILL_OBTAIN_ITEM.splice(0, 1);
            }            
        }
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_OBTAIN_ITEM.push({
                FINAL_BILLID: data.sj[i].BILLID, YEARMONTH: data.sj[i].YEARMONTH,
                NIANYUE: data.sj[i].NIANYUE,
                CONTRACTID: data.sj[i].CONTRACTID,
                TERMMC: data.sj[i].TERMMC,
                MUST_MONEY: data.sj[i].MUST_MONEY,
                UNPAID_MONEY: data.sj[i].UNPAID_MONEY,
                RECEIVE_MONEY: data.sj[i].UNPAID_MONEY,
                TYPE:1
            });            
        }
        let sumJE = 0;
        for (var i = 0; i < editDetail.dataParam.BILL_OBTAIN_ITEM.length; i++) {
            sumJE += parseInt(editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY);
        }
        editDetail.dataParam.ALL_MONEY = sumJE - editDetail.dataParam.ADVANCE_MONEY;
    }
    else if (editDetail.screenParam.showPopMerchant) {
        editDetail.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERCHANTNAME = data.sj[i].NAME;
            editDetail.dataParam.MERCHANT_MONEY = 0;
            editDetail.dataParam.FEE_ACCOUNT_ID = null;
        }
    } else if (editDetail.screenParam.showPopInvoice) {   //发票返回参数回填
        editDetail.screenParam.showPopInvoice = false;        
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_OBTAIN_INVOICE.push({
                INVOICENUMBER: data.sj[i].INVOICENUMBER,
                MERCHANTNAME: data.sj[i].MERCHANTNAME,
                TYPENAME: data.sj[i].TYPENAME,
                INVOICEDATE: data.sj[i].INVOICEDATE,
                NOVATAMOUNT: data.sj[i].NOVATAMOUNT,
                VATAMOUNT: data.sj[i].VATAMOUNT,
                INVOICEAMOUNT: data.sj[i].INVOICEAMOUNT,
                INVOICEID:data.sj[i].INVOICEID,
                TYPE: 1
            });
        }
    }
}
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700701"
    }, {
        id: "edit",
        authority: "10700701"
    }, {
        id: "del",
        authority: "10700701"
    }, {
        id: "save",
        authority: "10700701"
    }, {
        id: "abandon",
        authority: "10700701"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700702",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { BILLID: editDetail.dataParam.BILLID },
            }, function (data) {
                iview.Message.info("审核成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }, {
        id: "print",
        name: "打印",
        icon: "md-print",
        authority: "10700700",
        fun: function () {
            _.OpenPage({
                id: 10700200,
                title: '打印',
                url: "JSGL/BILL_OBTAIN_SK/Bill_Obtain_SkPrint/" + editDetail.dataParam.BILLID,
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS == 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};
editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.FKFSID) {
        iview.Message.info("请选择付款方式!");
        return false;
    };
    if (!editDetail.dataParam.FEE_ACCOUNT_ID) {
        iview.Message.info("请选收费单位!");
        return false;
    };
    if (!editDetail.dataParam.NIANYUE) {
        iview.Message.info("请选择收款年月!");
        return false;
    };
    if (editDetail.dataParam.BILL_OBTAIN_ITEM.length == 0) {
        iview.Message.info("请录入账单信息!");
        return false;
    } else {
        let fkje = 0;
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
            fkje += parseFloat(editDetail.dataParam.BILL_OBTAIN_ITEM[i].RECEIVE_MONEY);
        };
        if (!editDetail.dataParam.ALL_MONEY)
        {
            editDetail.dataParam.ALL_MONEY = 0;
        }
        if (!editDetail.dataParam.ADVANCE_MONEY) {
            editDetail.dataParam.ADVANCE_MONEY = 0;
        }
        if (!editDetail.dataParam.MERCHANT_MONEY) {
            editDetail.dataParam.MERCHANT_MONEY = 0;
        }
        if (parseFloat(editDetail.dataParam.ADVANCE_MONEY) > parseFloat(editDetail.dataParam.MERCHANT_MONEY))
        {
            iview.Message.info("冲销预付款金额不能大于商户余额");
            return false;
        }
        if (fkje != parseFloat(editDetail.dataParam.ALL_MONEY) + parseFloat(editDetail.dataParam.ADVANCE_MONEY))
        {
            iview.Message.info("付款金额" + editDetail.dataParam.ALL_MONEY + " + 冲销预付款金额" + editDetail.dataParam.ADVANCE_MONEY + " 不等于 明细付款金额之和" + fkje + "!");
            return false;
        }
    };

    return true;
}