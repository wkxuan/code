editDetail.beforeVue = function () {
    editDetail.service = "CxglService";
    editDetail.method = "";

    editDetail.screenParam.INX = null;
    //商铺表格
    editDetail.screenParam.colDefPRESENT = [
       { title: '序号', key: 'INX', width: 100 },
       { title: '满额', key: 'FULL', cellType: "input", cellDataType: "number", width: 200  },
       {
           title: "赠品名称", key: 'PRESENTNAME', cellType: "pop", enableCellEdit: true, width: 300 ,tooltip: true,
           onClick: function (index, row, data) {
               editDetail.screenParam.INX = index;
               editDetail.otherMethods.srchpop();
           },
       }
    ];
}
editDetail.branchChange = function () {
    editDetail.dataParam.PROMOBILL_FG_RULE = [];
};
editDetail.popCallBack = function (data) {
    if (editDetail.popConfig.open) {
        editDetail.popConfig.open = false;
        for (let i = 0; i < data.sj.length; i++) {
            if (editDetail.popConfig.title == "选择营销活动") {
                editDetail.dataParam.PROMOTIONID = data.sj[i].ID;
                editDetail.dataParam.PROMOTIONNAME = data.sj[i].NAME;
                editDetail.dataParam.START_DATE = data.sj[i].START_DATE;
                editDetail.dataParam.END_DATE = data.sj[i].END_DATE;
            }
            if (editDetail.popConfig.title == "选择赠品") {
                let itemData = editDetail.dataParam.PROMOBILL_FG_RULE;
                for (let i = 0; i < data.sj.length; i++) {
                    if (itemData.filter(function (item) { return (data.sj[i].ID == item.PRESENTID) }).length == 0) {
                        editDetail.dataParam.PROMOBILL_FG_RULE[editDetail.screenParam.INX].PRESENTID = data.sj[i].ID;
                        editDetail.dataParam.PROMOBILL_FG_RULE[editDetail.screenParam.INX].PRESENTNAME = data.sj[i].NAME;
                    }
                };                
            }
        };
    }
};

editDetail.otherMethods = {
    srchPromotion: function () {
        editDetail.screenParam.popParam = { STATUS: 2 };
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopPromotionList/";
        editDetail.popConfig.title = "选择营销活动";
        editDetail.popConfig.open = true;
    },
    srchpop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopPRESENTList/";
        editDetail.popConfig.title = "选择赠品";
        editDetail.popConfig.open = true;
    },
    addPRESENT: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.dataParam.PROMOBILL_FG_RULE.push({
            INX: editDetail.dataParam.PROMOBILL_FG_RULE.length + 1,
            FULL: "",
            PRESENTID:"",
            PRESENTNAME: "",
        });
    },
    delPRESENT: function () {
        let selection = this.$refs.refPRESENTs.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的赠品!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.PROMOBILL_FG_RULE;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].PRESENTID == selection[i].PRESENTID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            };
            for (var j = 0; j < editDetail.dataParam.PROMOBILL_FG_RULE.length; j++) {
                editDetail.dataParam.PROMOBILL_FG_RULE[j].INX = j + 1;
            };
        }
    },
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.PROMOTIONID = null;
    editDetail.dataParam.PROMOTIONNAME = null;
    editDetail.dataParam.START_DATE = null;
    editDetail.dataParam.END_DATE = null;
    editDetail.dataParam.START_TIME = 0;
    editDetail.dataParam.END_TIME = 1439;
    editDetail.dataParam.WEEK = "1,2,3,4,5,6,7";
    editDetail.dataParam.PROMOBILL_FG_RULE = [];
};

editDetail.newRecord = function () {
    editDetail.clearKey();
};

editDetail.IsValidSave = function () {
    editDetail.dataParam.PROMOTYPE = 4;  //4.满赠
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请确认门店!");
        return false;
    };

    if (!editDetail.dataParam.PROMOTIONID) {
        iview.Message.info("请确认营销活动!");
        return false;
    };
    if (!editDetail.dataParam.START_DATE) {
        iview.Message.info("请确认开始日期!");
        return false;
    };
    if (!editDetail.dataParam.END_DATE) {
        iview.Message.info("请确认结束日期!");
        return false;
    };
    if (new Date(editDetail.dataParam.START_DATE).Format('yyyy-MM-dd') > new Date(editDetail.dataParam.END_DATE).Format('yyyy-MM-dd')) {
        iview.Message.info(`结束日期不能小于开始日期!`);
        return false;
    };
    if (!editDetail.dataParam.WEEK) {
        iview.Message.info("请确认促销周期!");
        return false;
    };
    if (editDetail.dataParam.START_TIME == null) {
        iview.Message.info("请确认开始时间!");
        return false;
    };

    if (editDetail.dataParam.END_TIME == null) {
        iview.Message.info("请确认结束时间!");
        return false;
    };
    if (editDetail.dataParam.START_TIME > editDetail.dataParam.END_TIME) {
        iview.Message.info("结束时间不能小于开始时间!");
        return false;
    }

    let itemData = editDetail.dataParam.PROMOBILL_FG_RULE;
    if (!itemData.length) {
        iview.Message.info("请确认活动赠品!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].FULL || parseFloat(itemData[i].FULL) < 0) {
            iview.Message.info(`第${i + 1}行的赠品的满额不能小于0!`);
            return false;
        }
        if (!itemData[i].PRESENTID) {
            iview.Message.info(`请确认第${i +1}行的赠品!`);
            return false;
        }
    }
    return true;
};

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneData', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.mainData);
        editDetail.dataParam.PROMOBILL_FG_RULE = data.itemData;
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "11000601"
    }, {
        id: "edit",
        authority: "11000601"
    }, {
        id: "del",
        authority: "11000601"
    }, {
        id: "save",
        authority: "11000601"
    }, {
        id: "abandon",
        authority: "11000601"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "11000602",
        fun: function () {
            _.Ajax('ExecData', {
                Data: editDetail.dataParam
            }, function (data) {
                iview.Message.info("审核成功");
                editDetail.refreshDataParam(data);
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
    }, {
        id: "begin",
        name: "启动",
        icon: "md-star",
        authority: "11000603",
        fun: function () {
            _.Ajax('BeginData', {
                Data: editDetail.dataParam,
            }, function (data) {
                iview.Message.info("启动成功");
                editDetail.refreshDataParam(data);
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
    }, {
        id: "stop",
        name: "终止",
        icon: "md-star",
        authority: "11000604",
        fun: function () {
            _.Ajax('StopData', {
                Data: editDetail.dataParam,
            }, function (data) {
                iview.Message.info("终止成功");
                editDetail.refreshDataParam(data);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS == 3) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};