editDetail.beforeVue = function () {
    editDetail.screenParam.showPop = false;
    editDetail.screenParam.srcPop = "";
    editDetail.screenParam.title = "";
    editDetail.screenParam.popParam = {};
    //商铺表格
    editDetail.screenParam.colDefGoods = [
       { title: '序号', key: 'INX',width:100 },
       { title: "商品代码", key: 'GOODSDM'},
       { title: '商品名称', key: 'GOODSNAME' },
       { title: '品牌', key: 'BRANDMC' },
       { title: '满减方案', key: 'VALUE2MC' }
    ];
}
editDetail.branchChange = function () {
    editDetail.dataParam.PROMOBILL_GOODS = [];
};
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPop) {
        editDetail.screenParam.showPop = false;
        for (let i = 0; i < data.sj.length; i++) {
            if (editDetail.screenParam.title == "选择营销活动") {
                editDetail.dataParam.PROMOTIONID = data.sj[i].ID;
                editDetail.dataParam.PROMOTIONNAME = data.sj[i].NAME;
                editDetail.dataParam.START_DATE = data.sj[i].START_DATE;
                editDetail.dataParam.END_DATE = data.sj[i].END_DATE;

                editDetail.dataParam.START_DATE_LIMIT = data.sj[i].START_DATE;
                editDetail.dataParam.END_DATE_LIMIT = data.sj[i].END_DATE;
            }
            if (editDetail.screenParam.title == "选择商品") {
                let itemData = editDetail.dataParam.PROMOBILL_GOODS;
                for (let i = 0; i < data.sj.length; i++) {
                    if (itemData.filter(function (item) { return (data.sj[i].GOODSID == item.GOODSID) }).length == 0) {
                        itemData.push({
                            INX: itemData.length + 1,
                            GOODSID: data.sj[i].GOODSID,
                            GOODSDM: data.sj[i].GOODSDM,
                            GOODSNAME: data.sj[i].NAME,
                            BRANDMC: data.sj[i].BRANDMC,
                            VALUE2: null,
                            VALUE2MC: null
                        });
                    }
                };
            }
            if (editDetail.screenParam.title == "选择满减方案") {
                let itemData = editDetail.dataParam.PROMOBILL_GOODS;
                for (let i = 0; i < itemData.length; i++) {
                    itemData[i].VALUE2 = data.sj[data.sj.length-1].ID;
                    itemData[i].VALUE2MC = data.sj[data.sj.length - 1].NAME;
                };
            }
        };   
    }
};

editDetail.otherMethods = {
    srchPromotion: function () {
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopPromotionList/";
        editDetail.screenParam.title = "选择营销活动";
        editDetail.screenParam.popParam = { STATUS: 2 };
        editDetail.screenParam.showPop = true;
    },
    srchGoods: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, STATUS: 2 };
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopGoodsList/";
        editDetail.screenParam.title = "选择商品";
        editDetail.screenParam.showPop = true;
    },
    delGoods: function () {
        let selection = this.$refs.refGoods.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的商品!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.PROMOBILL_GOODS;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].GOODSID == selection[i].GOODSID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
        let data = editDetail.dataParam.PROMOBILL_GOODS;
        for (let i = 0; i < data.length; i++) {
            data[i].INX = i + 1;
        }
    },
    settingVal: function () {
        let selection = this.$refs.refGoods.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要设置满减方案的商品!");
            return;
        }
        editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopFR_PlanList/";
        editDetail.screenParam.title = "选择满减方案";
        editDetail.screenParam.popParam = {};
        editDetail.screenParam.showPop = true;
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.PROMOTYPE = 3;
    editDetail.dataParam.PROMOTIONID = null;
    editDetail.dataParam.PROMOTIONNAME = null;
    editDetail.dataParam.START_DATE = null;
    editDetail.dataParam.END_DATE = null;
    editDetail.dataParam.START_TIME = 0;
    editDetail.dataParam.END_TIME = 1439;
    editDetail.dataParam.WEEK = "1,2,3,4,5,6,7";
    editDetail.dataParam.PROMOBILL_GOODS = [];

    editDetail.dataParam.START_DATE_LIMIT = null;
    editDetail.dataParam.END_DATE_LIMIT = null;
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

    let itemData = editDetail.dataParam.PROMOBILL_GOODS;
    if (!itemData.length) {
        iview.Message.info("请确认促销商品!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].GOODSID) {
            iview.Message.info(`请确认第${i + 1}行的商品!`);
            return false;
        }
        if (!itemData[i].VALUE2) {
            iview.Message.info(`第${i + 1}行的商品的满减方案!`);
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
        editDetail.dataParam.PROMOBILL_GOODS = data.itemData;
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "11000401"
    }, {
        id: "edit",
        authority: "11000401"
    }, {
        id: "del",
        authority: "11000401"
    }, {
        id: "save",
        authority: "11000401"
    }, {
        id: "abandon",
        authority: "11000401"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "11000402",
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
        authority: "11000403",
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
        authority: "11000404",
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