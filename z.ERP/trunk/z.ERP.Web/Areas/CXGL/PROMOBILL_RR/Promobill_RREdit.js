editDetail.beforeVue = function () {
    //商铺表格
    editDetail.screenParam.colDefBrand = [
       { title: '序号', key: 'INX', width: 100 },
       { title: "品牌代码", key: 'BRANDID', width: 100 },
       { title: '品牌名称', key: 'BRANDNAME' },
       {
           title: '订单金额限制', key: 'AMOUNT_LIMIT', cellType: "input", cellDataType: "number",
           onBlur: function (index, row, data) {
               if (!isInteger(row.AMOUNT_LIMIT) && xsnumber(row.AMOUNT_LIMIT)) {
                   row.AMOUNT_LIMIT = Number(row.AMOUNT_LIMIT).toFixed(2);
               }
               if (!row.AMOUNT_LIMIT) {
                   row.AMOUNT_LIMIT = 0;
               }
           }
       },
       {
           title: '立减下限', key: 'REDUCE_DOWN', cellType: "input", cellDataType: "number",
           onBlur: function (index, row, data) {
               if (!isInteger(row.REDUCE_DOWN) && xsnumber(row.REDUCE_DOWN)) {
                   row.REDUCE_DOWN = Number(row.REDUCE_DOWN).toFixed(2);
               }
               if (!row.REDUCE_DOWN) {
                   row.REDUCE_DOWN = 0;
               }
           }
       },
       {
           title: '立减上限', key: 'REDUCE_UP', cellType: "input", cellDataType: "number",
           onBlur: function (index, row, data) {
               if (!isInteger(row.REDUCE_UP) && xsnumber(row.REDUCE_UP)) {
                   row.REDUCE_UP = Number(row.REDUCE_UP).toFixed(2);
               }
               if (!row.REDUCE_UP) {
                   row.REDUCE_UP = 0;
               }
           }
       },
       {
           title: '立减上限(订单金额比例%)', key: 'REDUCE_UP_RATE', cellType: "input", cellDataType: "number",
           onBlur: function (index, row, data) {
               if (!isInteger(row.REDUCE_UP_RATE) && xsnumber(row.REDUCE_UP_RATE)) {
                   row.REDUCE_UP_RATE = Number(row.REDUCE_UP_RATE).toFixed(2);
               }
               if (!row.REDUCE_UP_RATE) {
                   row.REDUCE_UP_RATE = 0;
               }
           }
       },
       {
           title: '预算费用', key: 'BUDGET', cellType: "input", cellDataType: "number",
           onBlur: function (index, row, data) {
               if (!isInteger(row.BUDGET) && xsnumber(row.BUDGET)) {
                   row.BUDGET = Number(row.BUDGET).toFixed(2);
               }
               if (!row.BUDGET) {
                   row.BUDGET = 0;
               }
           }
       }
    ];
    editDetail.screenParam.ntypeList = [
        { value: 'AMOUNT_LIMIT', label: '订单金额限制' },
        { value: 'REDUCE_DOWN', label: '立减下限' },
        { value: 'REDUCE_UP', label: '立减上限' },
        { value: 'REDUCE_UP_RATE', label: '立减上限(订单金额比例%)' },
        { value: 'BUDGET', label: '预算费用' },
    ];
    editDetail.screenParam.ntype = [];
    editDetail.dataParam.PROMOBILL_RR_BRAND = [];
}
editDetail.branchChange = function () {
    editDetail.dataParam.PROMOBILL_RR_BRAND = [];
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

                editDetail.dataParam.START_DATE_LIMIT = data.sj[i].START_DATE;
                editDetail.dataParam.END_DATE_LIMIT = data.sj[i].END_DATE;
            }
            if (editDetail.popConfig.title == "选择品牌") {
                let itemData = editDetail.dataParam.PROMOBILL_RR_BRAND;
                for (let i = 0; i < data.sj.length; i++) {
                    if (itemData.filter(function (item) { return (data.sj[i].BRANDID == item.BRANDID) }).length == 0) {
                        itemData.push({
                            INX: itemData.length + 1,
                            BRANDID: data.sj[i].ID,
                            BRANDNAME: data.sj[i].NAME,
                            AMOUNT_LIMIT: 0,
                            REDUCE_DOWN: null,
                            REDUCE_UP: null,
                            REDUCE_UP_RATE: 0,
                            BUDGET:0
                        });
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
    srchBrand: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.screenParam.popParam = {};
        editDetail.popConfig.src = __BaseUrl + "/Pop/Pop/PopBrandList/";
        editDetail.popConfig.title = "选择品牌";
        editDetail.popConfig.open = true;
    },
    delBrand: function () {
        let selection = this.$refs.refBrand.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.PROMOBILL_RR_BRAND;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].BRANDID == selection[i].BRANDID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
        let data = editDetail.dataParam.PROMOBILL_RR_BRAND;
        for (let i = 0; i < data.length; i++) {
            data[i].INX = i + 1;
        }
    },
    valonblur: function () {
        if (!isInteger(editDetail.screenParam.setVal) && xsnumber(editDetail.screenParam.setVal)) {
            editDetail.screenParam.setVal = Number(editDetail.screenParam.setVal).toFixed(2);
        }
    },
    GLOBAL_BUDGETonblur: function () {
        if (!isInteger(editDetail.dataParam.GLOBAL_BUDGET) && xsnumber(editDetail.dataParam.GLOBAL_BUDGET)) {
            editDetail.dataParam.GLOBAL_BUDGET = Number(editDetail.dataParam.GLOBAL_BUDGET).toFixed(2);
        }
    },
    settingVal: function () {
        let selection = this.$refs.refBrand.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要批量设置的品牌!");
            return;
        }
        if (editDetail.screenParam.ntype.length == 0) {
            iview.Message.info("请选中要批量设置的数据!");
            return;
        }
        var valn=Number(editDetail.screenParam.setVal);
        if (editDetail.screenParam.setVal == "" || valn < 0) {
            iview.Message.info("请设置批量设置的参数!");
            return;
        }        
        for (let i = 0; i < selection.length; i++) {
            let itemData = editDetail.dataParam.PROMOBILL_RR_BRAND;
            for (let j = 0; j < itemData.length; j++) {
                if (itemData[j].BRANDID == selection[i].BRANDID) {
                    let temp = editDetail.screenParam.ntype;
                    for (let k = 0; k < temp.length; k++) {
                        switch (temp[k]) {
                            case "AMOUNT_LIMIT": itemData[j].AMOUNT_LIMIT = valn; break;
                            case "REDUCE_DOWN": itemData[j].REDUCE_DOWN = valn; break;
                            case "REDUCE_UP": itemData[j].REDUCE_UP = valn; break;
                            case "REDUCE_UP_RATE": itemData[j].REDUCE_UP_RATE = valn; break;
                            case "BUDGET": itemData[j].BUDGET = valn; break;
                            default: break;
                        }
                    }
                }
            }           
        }
    },
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.PROMOTYPE = 5;
    editDetail.dataParam.PROMOTIONID = null;
    editDetail.dataParam.PROMOTIONNAME = null;
    editDetail.dataParam.START_DATE = null;
    editDetail.dataParam.END_DATE = null;
    editDetail.dataParam.START_TIME = 0;
    editDetail.dataParam.END_TIME = 1439;
    editDetail.dataParam.WEEK = "1,2,3,4,5,6,7";
    editDetail.dataParam.PROMOBILL_RR_BRAND = [];

    editDetail.dataParam.START_DATE_LIMIT = null;
    editDetail.dataParam.END_DATE_LIMIT = null;
    editDetail.dataParam.GLOBAL_BUDGET = 0;
    editDetail.dataParam.USED_BUDGET = 0;
    editDetail.screenParam.ntype = [];
    editDetail.screenParam.setVal = "";
};

editDetail.newRecord = function () {
    editDetail.clearKey();
};

editDetail.IsValidSave = function () {
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
    if (!editDetail.dataParam.GLOBAL_BUDGET || Number(editDetail.dataParam.GLOBAL_BUDGET)<=0) {
        iview.Message.info("请确认预算总额,预算总额必须大于0!");
        return false;
    };
    let itemData = editDetail.dataParam.PROMOBILL_RR_BRAND;
    if (!itemData.length) {
        iview.Message.info("请确认随机立减品牌!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].REDUCE_DOWN||Number(itemData[i].REDUCE_DOWN)<0) {
            iview.Message.info(`请确认第${i + 1}行品牌的立减下限,并且立减下限必须大于0!`);
            return false;
        }
        if (!itemData[i].REDUCE_UP || Number(itemData[i].REDUCE_UP)<0) {
            iview.Message.info(`请确认第${i + 1}行品牌的立减上限,并且立减下限必须大于0!`);
            return false;
        }
        if (Number(itemData[i].REDUCE_DOWN)> Number(itemData[i].REDUCE_UP)) {
            iview.Message.info(`第${i + 1}的立减上限不能小于立减下限!`);
            return false;
        }
        if (Number(itemData[i].REDUCE_UP_RATE) < 0 || Number(itemData[i].REDUCE_UP_RATE) > 100) {
            iview.Message.info(`第${i + 1}行的商品的折扣率不能小于0、大于100!`);
            return false;
        }
        if (Number(itemData[i].BUDGET) > Number(editDetail.dataParam.GLOBAL_BUDGET)) {
            iview.Message.info(`第${i + 1}的预算费用不能大于预算总额!`);
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
        editDetail.dataParam.PROMOBILL_RR_BRAND = data.itemData;
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "11000901"
    }, {
        id: "edit",
        authority: "11000901"
    }, {
        id: "del",
        authority: "11000901"
    }, {
        id: "save",
        authority: "11000901"
    }, {
        id: "abandon",
        authority: "11000901"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "11000902",
        fun: function () {
            _.Ajax('ExecData', {
                SaveData: editDetail.dataParam,
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
        authority: "11000903",
        fun: function () {
            _.Ajax('BeginData', {
                SaveData: editDetail.dataParam,
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
        authority: "11000904",
        fun: function () {
            _.Ajax('StopData', {
                SaveData: editDetail.dataParam,
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
function isInteger(n) {
    return parseInt(n) == parseFloat(n)
}
function xsnumber(n) {
    var x = String(n).indexOf('.') + 1; //小数点的位置
    var y = String(n).length - x; //小数的位数
    if (y > 2) {
        return true;
    } else {
        return false;
    }
}