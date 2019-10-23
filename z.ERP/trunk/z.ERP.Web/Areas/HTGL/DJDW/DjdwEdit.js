editDetail.beforeVue = function () {
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";

    editDetail.dataParam.STATUS = 1;
    editDetail.dataParam.STYLE = 3;
    editDetail.dataParam.JXSL = 0;
    editDetail.dataParam.XXSL = 0;
    editDetail.dataParam.STANDARD = 1;
    editDetail.dataParam.OPERATERULE = 1;

    editDetail.screenParam.showPopMerchant = false;
    editDetail.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/Pop/Pop/PopShopList/";
    editDetail.screenParam.showPopFeeSubject = false;
    editDetail.screenParam.srcPopFeeSubject = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";

    editDetail.screenParam.popParam = {};
    //商铺表格
    editDetail.screenParam.colDefSHOP = [
       {
           title: "商铺代码", key: 'CODE', cellType: "input",
           onEnter: function (index, row, data) {
               if (!row.CODE) {
                   return;
               }
               let tbData = data;
               _.Ajax('GetShop', {
                   Data: { CODE: row.CODE, BRANCHID: editDetail.dataParam.BRANCHID }
               }, function (data) {
                   if (data.dt) {
                       if (tbData.filter(item=> { return data.dt.SHOPID == item.SHOPID }).length) {
                           iview.Message.info('当前商铺代码已存在!');
                           return;
                       }
                       row.SHOPID = data.dt.SHOPID;
                       row.CATEGORYID = data.dt.CATEGORYID;
                       row.CATEGORYCODE = data.dt.CATEGORYCODE;
                       row.CATEGORYNAME = data.dt.CATEGORYNAME;
                       row.AREA = data.dt.AREA_BUILD;
                       row.AREA_RENTABLE = data.dt.AREA_RENTABLE;
                   } else {
                       for (let item in row) {
                           row[item] = null;
                       }
                       iview.Message.info('当前单元代码不存在或者不属于当前门店!');
                   }
                   editDetail.vueObj.calculateArea();
               });
           }
       },
       { title: '业态代码', key: 'CATEGORYCODE' },
       { title: '业态名称', key: 'CATEGORYNAME' },
       { title: '建筑面积', key: 'AREA' },
       { title: '租用面积', key: 'AREA_RENTABLE' }
    ];
    //收费项目
    editDetail.screenParam.colDefCOST = [
       {
           title: "费用项目", key: 'TREMID', cellType: "input",
           onEnter: function (index, row, data) {
               let tbData = data;
               _.Ajax('GetFeeSubject', {
                   Data: { TRIMID: row.TREMID }
               }, function (data) {
                   if (data.dt) {
                       if (tbData.filter(item=> { return data.dt.NAME == item.NAME }).length) {
                           iview.Message.info('当前费用项目已存在!');
                           return;
                       }
                       row.NAME = data.dt.NAME;
                   } else {
                       row.TREMID = null;
                       row.NAME = null;
                       iview.Message.info('当前费用项目不存在!');
                   }
               });
           }
       },
       { title: "费用项目名称", key: 'NAME' },
       { title: "金额", key: 'COST', cellType: "input", cellDataType: "number" },
    ];
}
editDetail.branchChange = function () {
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.otherMethods.calculateArea();
};
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopMerchant) {
        editDetail.screenParam.showPopMerchant = false;
        for (let i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.MERNAME = data.sj[i].NAME;
        };
    }
    if (editDetail.screenParam.showPopShop) {
        editDetail.screenParam.showPopShop = false;
        let shop = editDetail.dataParam.CONTRACT_SHOP;
        for (let i = 0; i < data.sj.length; i++) {
            if (shop.filter(item=> { return (data.sj[i].SHOPID == item.SHOPID) }).length == 0) {
                shop.push(data.sj[i]);
            }
        };
        editDetail.vueObj.calculateArea();
    }
    if (editDetail.screenParam.showPopFeeSubject) {
        editDetail.screenParam.showPopFeeSubject = false;
        let cost = editDetail.dataParam.CONTRACT_COST_DJDW;
        for (let i = 0; i < data.sj.length; i++) {
            if (cost.filter(item=> { return (data.sj[i].TERMID == item.TERMID) }).length == 0) {
                let loc = {};
                editDetail.screenParam.colDefCOST.forEach(item=> {
                    switch (item.key) {
                        case "TREMID":
                            loc[item.key] = data.sj[i].TERMID;
                            break;
                        case "NAME":
                            loc[item.key] = data.sj[i].NAME;
                            break;
                        default:
                            loc[item.key] = null;
                            break;
                    }
                });
                cost.push(loc);
            }
        };
    }
};

editDetail.otherMethods = {
    srchMerchant: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        Vue.set(editDetail.screenParam, "showPopMerchant", true);
    },
    srchShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID };
        Vue.set(editDetail.screenParam, "showPopShop", true);
    },
    addRowShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        let temp = editDetail.dataParam.CONTRACT_SHOP || [];
        let loc = {};
        editDetail.screenParam.colDefSHOP.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
    },
    delShop: function () {
        let selection = this.$refs.refShop.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的商铺!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.CONTRACT_SHOP;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].SHOPID == selection[i].SHOPID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
            editDetail.vueObj.calculateArea();
        }
    },
    srchCost: function () {
        Vue.set(editDetail.screenParam, "showPopFeeSubject", true);
    },
    addRowCost: function () {
        let temp = editDetail.dataParam.CONTRACT_COST_DJDW || [];
        let loc = {};
        editDetail.screenParam.colDefCOST.forEach(item=> {
            loc[item.key] = null;
        });
        temp.push(loc);
    },
    delCost: function () {
        let selection = this.$refs.refCost.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            let temp = editDetail.dataParam.CONTRACT_COST_DJDW;
            for (let i = 0; i < selection.length; i++) {
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].NAME == selection[i].NAME) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    },
    calculateArea: function () {
        let shop = editDetail.dataParam.CONTRACT_SHOP;
        let areaBuild = 0, arear = 0;
        for (var i = 0; i < shop.length; i++) {
            if (shop[i].SHOPID) {
                areaBuild += shop[i].AREA;
                arear += shop[i].AREA_RENTABLE;
            }
        }
        editDetail.dataParam.AREA_BUILD = areaBuild;
        editDetail.dataParam.AREAR = arear;
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.MERNAME = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.CONT_START = null;
    editDetail.dataParam.CONT_END = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.STATUSMC = null;
    editDetail.dataParam.CONTRACT_SHOP = [];
    editDetail.dataParam.CONTRACT_COST_DJDW = [];
};

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请确认门店!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请选择商户!");
        return false;
    };
    if (!editDetail.dataParam.CONT_START) {
        iview.Message.info("请维护开始日期!");
        return false;
    };

    if (!editDetail.dataParam.CONT_END) {
        iview.Message.info("请维护结束日期!");
        return false;
    };
    let shop = editDetail.dataParam.CONTRACT_SHOP;
    if (shop.length == 0) {
        iview.Message.info("商铺信息不能为空!");
        return false;
    }
    for (let i = 0; i < shop.length; i++) {
        if (!shop[i].SHOPID) {
            iview.Message.info(`请确定商铺信息中第${i + 1}行的商铺!`);
            return false;
        };
    };
    let cost = editDetail.dataParam.CONTRACT_COST_DJDW;
    if (cost.length) {
        for (let i = 0; i < cost.length; i++) {
            if (!cost[i].NAME) {
                iview.Message.info(`请确定收费项目信息中第${i + 1}行的项目!`);
                return false;
            };
            if (!cost[i].COST || Number(cost[i].COST) < 0) {
                iview.Message.info(`请确定收费项目${cost[i].NAME}的金额,且大于0!`);
                return false;
            };
        };
    };
    return true;
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchContract', {
        Data: { CONTRACTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.contract);
        editDetail.dataParam.BILLID = data.contract.CONTRACTID;
        editDetail.dataParam.CONTRACT_SHOP = data.contractShop;
        editDetail.dataParam.CONTRACT_COST_DJDW = data.contractCostDjdw;
        callback && callback(data);
    });
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10600501"
    }, {
        id: "edit",
        authority: "10600501"
    }, {
        id: "del",
        authority: "10600501"
    }, {
        id: "save",
        authority: "10600501"
    }, {
        id: "abandon",
        authority: "10600501"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10600502",
        fun: function () {
            _.Ajax('ExecData', {
                Data: editDetail.dataParam,
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