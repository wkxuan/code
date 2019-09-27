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
    editDetail.screenParam.FEE_ACCOUNT = [];

    editDetail.dataParam.NIANYUE = (new Date()).Format("yyyyMM"); //默认当前年月
    // editDetail.dataParam.TYPE = 1;

    editDetail.screenParam.colDef = [
    { title: '账单号', key: 'FINAL_BILLID'},
    { title: '债权发生月', key: 'NIANYUE'},
    { title: '收费项目', key: 'TERMMC'},
    { title: "应收金额", key: 'MUST_MONEY'},
    { title: "未付金额", key: 'UNPAID_MONEY' },
    { title: "通知金额", key: 'NOTICE_MONEY', cellType: "input", cellDataType: "number" }
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
    editDetail.dataParam.NIANYUE =  (new Date()).Format("yyyyMM"); //默认当前年月
    //  editDetail.dataParam.TYPE = 1;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Notice', {
        Data: { BILLID: data }
    }, function (data) {
        editDetail.dataParam.BRANCHID = data.billNotice.BRANCHID;
        editDetail.otherMethods.branchChange(function () {
            $.extend(editDetail.dataParam, data.billNotice);
            editDetail.dataParam.NIANYUE += "";
            editDetail.dataParam.BILL_NOTICE_ITEM = data.billNoticeItem;
            callback && callback(data);
        });
    });
}

///html中绑定方法
editDetail.otherMethods = {
    ContractChange: function () {
        editDetail.dataParam.BILL_NOTICE_ITEM = [];
    },
    branchChange: function (func) {
        editDetail.dataParam.CONTRACTID = null;
        editDetail.dataParam.MERCHANTID = null;
        editDetail.dataParam.MERCHANTNAME = null;
        editDetail.dataParam.BILL_NOTICE_ITEM = [];
        _.Ajax('GETfee', {
            Data: { BRANCHID: editDetail.dataParam.BRANCHID }
        }, function (data) {
            var list = [];
            for (var i = 0; i < data.length; i++) {
                list.push({ value: Number(data[i].Key), label: data[i].Value })
            }
            editDetail.screenParam.FEE_ACCOUNT = list;

            if (typeof func == "function") {
                func();
            }
        });
    },
    SelMerchant: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        };
        editDetail.screenParam.showPopMerchant = true;
        editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
    },
    ///选择合同
    SelContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
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
            FEE_ACCOUNTID: editDetail.dataParam.FEE_ACCOUNTID,
            STATUS:[2,3]
        };
    },
    delBill: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.BILL_NOTICE_ITEM.length; j++) {
                    if (editDetail.dataParam.BILL_NOTICE_ITEM[j].FINAL_BILLID == selectton[i].FINAL_BILLID) {
                        editDetail.dataParam.BILL_NOTICE_ITEM.splice(j, 1);
                    }
                }
            }
        }
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
//数据初始化
editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.STATUSMC = "未审核";
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.MERCHANTNAME = null;
    editDetail.dataParam.FEE_ACCOUNTID = null;
    editDetail.dataParam.TYPE = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_NOTICE_ITEM = [];
}
//按钮初始化
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
            if (!disabled && data.STATUS <= 2) {
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
        iview.Message.info("请选择门店!");
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