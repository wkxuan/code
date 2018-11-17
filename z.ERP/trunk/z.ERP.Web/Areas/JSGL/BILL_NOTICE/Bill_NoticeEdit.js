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
    editDetail.dataParam.TYPE = 1;

    editDetail.screenParam.colDef = [
    //{
    //    title: "账单号", key: 'FINAL_BILLID', width: 160,
    //    render: function (h, params) {
    //        return h('div',
    //            [
    //        h('Input', {
    //            props: {
    //                value: params.row.FINAL_BILLID
    //            },
    //            style: { marginRight: '5px', width: '80px' },
    //            on: {
    //                'on-blur': function (event) {
    //                    editDetail.dataParam.BILL_NOTICE_ITEM[params.index].FINAL_BILLID = event.target.value;
    //                }
    //            },
    //        }),
    //            ])
    //    },
    //},
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
    editDetail.dataParam.NIANYUE = (new Date()).getFullYear() + ('0'+((new Date()).getMonth() + 1)).substr(-2); //默认当前年月
    editDetail.dataParam.TYPE = 1;
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
    ///选择合同
    SelContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return ;
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
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID,
            CONTRACTID: editDetail.dataParam.CONTRACTID,
            WFDJ: 1
        };
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
                return parseInt(item.FINAL_BILLID) === data.sj[i].BILLID; }).length === 0))
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

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_NOTICE_ITEM = [];
}

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
    else
    {
        for(var i=0;i<editDetail.dataParam.BILL_NOTICE_ITEM.length;i++){
            if (!editDetail.dataParam.BILL_NOTICE_ITEM[i].NOTICE_MONEY)
            {
                iview.Message.info("请录入通知金额!");
                return false;
            }
        }
    }

    //if (editDetail.dataParam.BILL_ADJUST_ITEM.length == 0) {
    //    iview.Message.info("请录入费用信息!");
    //    return false;
    //} else {
    //    for (var i = 0; i < editDetail.dataParam.BILL_ADJUST_ITEM.length; i++) {
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].CONTRACTID) {
    //            iview.Message.info("请录入租约!");
    //            return false;
    //        };
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].TERMID) {
    //            iview.Message.info("请选择收费项目!");
    //            return false;
    //        };
    //        if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].MUST_MONEY) {
    //            iview.Message.info("请录入费用金额!");
    //            return false;
    //        };
    //    };
    //};

    return true;
}