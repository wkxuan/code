editDetail.beforeVue = function () {
    //保证金收款
    editDetail.dataParam.TYPE = 2;
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
        cellType: "input", cellDataType: "number",
        onChange: function (index, row, data) {
            if (Number(row.RECEIVE_MONEY) > Number(row.UNPAID_MONEY)) {
                row.RECEIVE_MONEY = null;
                iview.Message.info("此账单的付款金额不能大于未付金额!");
                return;
            }
            editDetail.veObj.computeAllmoney();
        }
    }];
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Obtain', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billObtain);
        editDetail.dataParam.BILL_OBTAIN_ITEM = data.billObtainItem;
        callback && callback(data);
    });
};
///html中绑定方法
editDetail.otherMethods = {
    selMerchant: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        }
        editDetail.screenParam.showPopMerchant = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
    },
    selBill: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请选择商户!");
            return;
        };
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID,
            MERCHANTID: editDetail.dataParam.MERCHANTID,
            FTYPE: [1],   //保证金类型
            STATUS:[2,3]
        };
    },
    delBill: function () {
        let selection = this.$refs.refZd.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的账单!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.BILL_OBTAIN_ITEM;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].FINAL_BILLID == selection[i].FINAL_BILLID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
            editDetail.veObj.computeAllmoney();
        }
    },
    //计算收款金额
    computeAllmoney: function () {
        let itemData = editDetail.dataParam.BILL_OBTAIN_ITEM;
        let sum = 0;
        for (let i = 0; i < itemData.length; i++) {
            sum += Number(itemData[i].RECEIVE_MONEY);
        }
        editDetail.dataParam.ALL_MONEY = sum;
    }
};
///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;
        let itemData = editDetail.dataParam.BILL_OBTAIN_ITEM;
        //接收选中的数据
        for (var i = 0; i < data.sj.length; i++) {
            if (!itemData.length || (itemData.length && !itemData.filter(function (item) {
                return item.FINAL_BILLID == data.sj[i].BILLID;
            }).length))
                itemData.push({
                    FINAL_BILLID: data.sj[i].BILLID,
                    YEARMONTH: data.sj[i].YEARMONTH,
                    CONTRACTID: data.sj[i].CONTRACTID,
                    TERMMC: data.sj[i].TERMMC,
                    MUST_MONEY: data.sj[i].MUST_MONEY,
                    UNPAID_MONEY: data.sj[i].UNPAID_MONEY,
                    TYPE: data.sj[i].TYPE,
                    RECEIVE_MONEY: data.sj[i].UNPAID_MONEY,
                });
        }
    }
    if (editDetail.screenParam.showPopMerchant) {
        editDetail.screenParam.showPopMerchant = false;
        editDetail.dataParam.BILL_OBTAIN_ITEM = [];
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.YEARMONTH = null;
    editDetail.dataParam.STATUSMC = null;
    editDetail.dataParam.ALL_MONEY = null;
    editDetail.dataParam.FKFSID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.PAYNAME = null; 
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.MERCHANTNAME = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_OBTAIN_ITEM = [];
};

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
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
    if (!editDetail.dataParam.NIANYUE) {
        iview.Message.info("请选择收款年月!");
        return false;
    };
    let itemData = editDetail.dataParam.BILL_OBTAIN_ITEM;
    if (itemData.length == 0) {
        iview.Message.info("请录入保证金付款信息!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].RECEIVE_MONEY) {
            iview.Message.info(`请录入账单号为${itemData[i].FINAL_BILLID}的付款金额!`);
            return false;
        };
        if (Number(itemData[i].RECEIVE_MONEY) > Number(itemData[i].UNPAID_MONEY)) {
            iview.Message.info(`请录入账单号为${itemData[i].FINAL_BILLID}的付款金额不能大于未付金额!`);
            return false;
        };
    };

    return true;
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700301"
    }, {
        id: "edit",
        authority: "10700301"
    }, {
        id: "del",
        authority: "10700301"
    }, {
        id: "save",
        authority: "10700301"
    }, {
        id: "abandon",
        authority: "10700301"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700302",
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
            if (!disabled && data.STATUS == 1) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};