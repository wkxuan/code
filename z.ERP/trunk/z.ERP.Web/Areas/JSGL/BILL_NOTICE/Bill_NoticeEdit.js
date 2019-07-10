editDetail.beforeVue = function () {
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';

    //初始化弹窗所要传递参数
    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/" + "Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.BILL_NOTICE_ITEM = [];

    editDetail.dataParam.NIANYUE = (new Date()).getFullYear() + ('0' + ((new Date()).getMonth() + 1)).substr(-2); //默认当前年月
    // editDetail.dataParam.TYPE = 1;

    editDetail.screenParam.colDef = [
    { title: '账单号', key: 'FINAL_BILLID', width: 100 },
    { title: '债权发生月', key: 'NIANYUE', width: 100 },
    { title: '收费项目', key: 'TERMMC', width: 200 },
    { title: "应收金额", key: 'MUST_MONEY', width: 100 },
    { title: "未付金额", key: 'UNPAID_MONEY', width: 100 },
    {
        title: "通知金额", key: 'NOTICE_MONEY', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.NOTICE_MONEY
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.BILL_NOTICE_ITEM[params.index].NOTICE_MONEY = event.target.value;
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
                            editDetail.dataParam.BILL_NOTICE_ITEM.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];
    if (!editDetail.dataParam.BILL_NOTICE_ITEM) {
        editDetail.dataParam.BILL_NOTICE_ITEM = [{
            FINAL_BILLID: "",
            NOTICE_MONEY: "",
            MUST_MONEY: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.BILL_NOTICE_ITEM || [];
        temp.push({});
        editDetail.dataParam.BILL_NOTICE_ITEM = temp;
    }
}

editDetail.newRecord = function () {
    editDetail.dataParam.NIANYUE = (new Date()).getFullYear() + ('0' + ((new Date()).getMonth() + 1)).substr(-2); //默认当前年月
    //  editDetail.dataParam.TYPE = 1;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Notice', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billNotice);
        editDetail.dataParam.BILL_NOTICE_ITEM = data.billNoticeItem;
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
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
    },
    ///选择合同
    SelContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        };
        editDetail.screenParam.showPopContract = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID
        };

    },
    SelBill: function () {
        if (!editDetail.dataParam.CONTRACTID) {
            iview.Message.info("请选择租约!");
            return;
        };
        if (!editDetail.dataParam.TYPE) {
            iview.Message.info("请选择类型!");
            return;
        };
        if (!editDetail.dataParam.FEE_ACCOUNTID) {
            iview.Message.info("请选择收费单位!");
            return;
        };


        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID,
            CONTRACTID: editDetail.dataParam.CONTRACTID,
            WFDJ: 1,
            SCFS_TZD: editDetail.dataParam.TYPE,
            FEE_ACCOUNTID: editDetail.dataParam.FEE_ACCOUNTID
        };
    },

    chgTYPE: function () {
        Vue.set(editDetail.dataParam, "BILL_NOTICE_ITEM", []);
    },
    chgFAT: function () {
        Vue.set(editDetail.dataParam, "BILL_NOTICE_ITEM", []);
    }

}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopContract) {
        editDetail.screenParam.showPopContract = false;
        //接收选中的数据

        editDetail.dataParam.CONTRACTID = data.sj[0].CONTRACTID;
        editDetail.dataParam.MERCHANTID = data.sj[0].MERCHANTID;
        editDetail.dataParam.MERCHANTNAME = data.sj[0].MERCHANTNAME;
    };
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;

        //删除空行
        if (editDetail.dataParam.BILL_NOTICE_ITEM.length > 0) {
            if (!editDetail.dataParam.BILL_NOTICE_ITEM[0].FINAL_BILLID) {
                editDetail.dataParam.BILL_NOTICE_ITEM.splice(0, 1);
            }
        }
        //接收选中的数据
        for (var i = 0; i < data.sj.length; i++) {
            if ((editDetail.dataParam.BILL_NOTICE_ITEM.length === 0)
                || (editDetail.dataParam.BILL_NOTICE_ITEM.length > 0
                && editDetail.dataParam.BILL_NOTICE_ITEM.filter(function (item) {
                return parseInt(item.FINAL_BILLID) === data.sj[i].BILLID;
            }).length === 0))
                editDetail.dataParam.BILL_NOTICE_ITEM.push({
                    FINAL_BILLID: data.sj[i].BILLID,
                    NIANYUE: data.sj[i].NIANYUE,
                    TERMMC: data.sj[i].TERMMC,
                    MUST_MONEY: data.sj[i].MUST_MONEY,
                    UNPAID_MONEY: data.sj[i].UNPAID_MONEY,
                    NOTICE_MONEY: data.sj[i].UNPAID_MONEY
                });
        }
    }
}
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700501"
    }, {
        id: "edit",
        authority: "10700501"
    }, {
        id: "del",
        authority: "10700501"
    }, {
        id: "save",
        authority: "10700501"
    }, {
        id: "abandon",
        authority: "10700501"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700502",
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
        authority: "10700500",
        fun: function () {
            _.Ajax('Searchprinturl', {
                1: 1
            }, function (data) {
                if (data != "") {
                    if (editDetail.dataParam.TYPE == "2") {
                        _.OpenPage({
                            id: 10700500,
                            title: '打印',
                            url: data + "/" + editDetail.dataParam.BILLID,
                        })
                    }
                    else {
                        _.OpenPage({
                            id: 10700200,
                            title: '打印',
                            url: data + "Other/" + editDetail.dataParam.BILLID,
                        })
                    }
                } else {
                    alert("未配置打印模版")
                }
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

    if (!editDetail.dataParam.CONTRACTID) {
        iview.Message.info("请选择租约!");
        return false;
    }

    if (editDetail.dataParam.BILL_NOTICE_ITEM.length == 0) {
        iview.Message.info("请选择账单!");
        return false;
    }
    else {
        for (var i = 0; i < editDetail.dataParam.BILL_NOTICE_ITEM.length; i++) {
            if (!editDetail.dataParam.BILL_NOTICE_ITEM[i].NOTICE_MONEY) {
                iview.Message.info("请录入通知金额!");
                return false;
            }
        }
    }
    return true;
}