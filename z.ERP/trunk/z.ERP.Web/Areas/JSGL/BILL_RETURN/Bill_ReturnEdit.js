editDetail.beforeVue = function () {
    editDetail.service = "JsglService";
    editDetail.method = "GetBillReturn";

    editDetail.screenParam.showPopBill = false;
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopBill = __BaseUrl + "/Pop/Pop/PopBillList/";
    editDetail.screenParam.srcPopContract = __BaseUrl + "/Pop/Pop/PopContractList/";
    editDetail.screenParam.popParam = {};

    editDetail.screenParam.colDef = [
    {
        title: "账单号", key: 'FINAL_BILLID'
    },
    { title: '应收金额', key: 'MUST_MONEY' },
    { title: '已收金额', key: 'RECEIVE_MONEY' },
    { title: '已返金额', key: 'HIS_RETURN_MONEY' },
    {
        title: "本次返还金额", key: 'RETURN_MONEY',
        cellType: "input", cellDataType: "number",
        onChange: function (index, row, data) {
            if (Number(row.RETURN_MONEY) > Number(row.RECEIVE_MONEY) - Number(row.HIS_RETURN_MONEY)) {
                row.RETURN_MONEY = null;
                iview.Message.info("返还金额不能大于已收金额-已返金额!");
                return;
            }
        }
    }];
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Return', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billReturn);
        editDetail.dataParam.BILL_RETURN_ITEM = data.billReturnItem;
        callback && callback(data);
    });
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.MERCHANTNAME = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.DESCRIPTION = null; 
    editDetail.dataParam.BILL_RETURN_ITEM = [];
};
///html中绑定方法
editDetail.otherMethods = {
    ///选择合同
    selContract: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        };
        editDetail.dataParam.BILL_RETURN_ITEM = [];
        editDetail.screenParam.showPopContract = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
    },
    selBill: function () {
        if (!editDetail.dataParam.CONTRACTID) {
            iview.Message.info("请选择租约!");
            return;
        };
        editDetail.screenParam.showPopBill = true;
        editDetail.screenParam.popParam = {
            BRANCHID: editDetail.dataParam.BRANCHID,
            CONTRACTID: editDetail.dataParam.CONTRACTID,
            FTYPE: [1],    //保证金类型
            STATUS:[3,4],  //状态
            RRETURNFLAG: "1"  //返还标记 暂时判断 RECEIVE_MONEY <> 0 的记录
        };
    },
    delBill: function () {
        let selection = this.$refs.refZd.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的账单!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.BILL_RETURN_ITEM;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].FINAL_BILLID == selection[i].FINAL_BILLID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    }
};
///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopContract) {
        editDetail.screenParam.showPopContract = false;
        editDetail.dataParam.BILL_RETURN_ITEM = [];
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.CONTRACTID = data.sj[i].CONTRACTID;
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERCHANTNAME = data.sj[i].MERCHANTNAME;
        }
    };
    if (editDetail.screenParam.showPopBill) {
        editDetail.screenParam.showPopBill = false;
        let itemData = editDetail.dataParam.BILL_RETURN_ITEM;
        //接收选中的数据
        for (var i = 0; i < data.sj.length; i++) {
            if (!itemData.length|| (itemData.length && !itemData.filter(function (item) {
                return item.FINAL_BILLID == data.sj[i].BILLID;
            }).length))
                itemData.push({
                    FINAL_BILLID: data.sj[i].BILLID,
                    TERMMC: data.sj[i].TERMMC,
                    MUST_MONEY: data.sj[i].MUST_MONEY,
                    RECEIVE_MONEY: data.sj[i].RECEIVE_MONEY,
                    HIS_RETURN_MONEY: data.sj[i].RETURN_MONEY

                });
        }
    }
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700101"
    }, {
        id: "edit",
        authority: "10700101"
    }, {
        id: "del",
        authority: "10700101"
    }, {
        id: "save",
        authority: "10700101"
    }, {
        id: "abandon",
        authority: "10700101"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700102",
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
    if (!editDetail.dataParam.NIANYUE) {
        iview.Message.info("请输入债权发年月!");
        return false;
    }
    let itemData = editDetail.dataParam.BILL_RETURN_ITEM;
    if (itemData.length == 0) {
        iview.Message.info("请录入保证金付款信息!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].RETURN_MONEY) {
            iview.Message.info(`请录入账单号为${itemData[i].FINAL_BILLID}的返还金额!`);
            return false;
        };
    };

    return true;
};