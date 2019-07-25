var index;
editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "JsglService";
    editDetail.method = "GetBillAdjust";
    editDetail.Key = 'BILLID';

    //初始化弹窗所要传递参数
    editDetail.screenParam.ParentFeeSubject = {};
    editDetail.screenParam.showPopFeeSubject = false;
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopFeeSubject = __BaseUrl + "/" + "Pop/Pop/PopFeeSubjectList/";
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    editDetail.screenParam.popParam = {};

    ///账单类型初始化默认给1
    editDetail.dataParam.TYPE = 1;
    _.Ajax('GetFEESUBJECT', {
        1: 1
    }, function (data) {
        let TERMList = $.map(data, item => {
            return {
                label: item.Value,
                value: item.Key
            }
        });
        editDetail.screenParam.colDef = [
            { title: "租约号", key: 'CONTRACTID', width: 150 },
            { title: "商户名称", key: 'MERCHANTNAME', width: 150 },
            { title: "收费项目", key: 'TERMID', width: 150, cellType: "select", enableCellEdit: true, selectList: TERMList },
            { title: "调整金额", key: 'MUST_MONEY', width: 100, cellType: "input", cellDataType: "number" }
        ];
    });
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchBill_Adjust', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.billAdjust);
        editDetail.dataParam.BILL_ADJUST_ITEM = data.billAdjustItem;
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    srchContract: function () {
        editDetail.screenParam.showPopContract = true;
    },
    delContract: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.BILL_ADJUST_ITEM.length; j++) {
                    if (editDetail.dataParam.BILL_ADJUST_ITEM[j].CONTRACTID == selectton[i].CONTRACTID && editDetail.dataParam.BILL_ADJUST_ITEM[j].TERMID == selectton[i].TERMID) {
                        editDetail.dataParam.BILL_ADJUST_ITEM.splice(j, 1);
                    }
                }
            }
        }
    },
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopContract) {
        editDetail.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BILL_ADJUST_ITEM.push({
                CONTRACTID: data.sj[i].CONTRACTID,
                MERCHANTNAME: data.sj[i].MERCHANTNAME,
                TERMID: "",
                MUST_MONEY: 0,
            });
        }
    };
}
//数据初始化
editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.NIANYUE = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.STATUSMC = "未审核";
    editDetail.dataParam.TYPE = null;
    editDetail.dataParam.YEARMONTH = null;
    editDetail.dataParam.START_DATE = null;
    editDetail.dataParam.END_DATE = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.BILL_ADJUST_ITEM = [];
}
//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10700201"
    }, {
        id: "edit",
        authority: "10700201"
    }, {
        id: "del",
        authority: "10700201"
    }, {
        id: "save",
        authority: "10700201"
    }, {
        id: "abandon",
        authority: "10700201"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10700202",
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

    if (editDetail.dataParam.BILL_ADJUST_ITEM.length == 0) {
        iview.Message.info("请录入费用信息!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.BILL_ADJUST_ITEM.length; i++) {
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].CONTRACTID) {
                iview.Message.info("请录入租约!");
                return false;
            };
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].TERMID) {
                iview.Message.info("请选择收费项目!");
                return false;
            };
            if (!editDetail.dataParam.BILL_ADJUST_ITEM[i].MUST_MONEY) {
                iview.Message.info("请录入费用金额!");
                return false;
            };
        };
    };
    return true;
}