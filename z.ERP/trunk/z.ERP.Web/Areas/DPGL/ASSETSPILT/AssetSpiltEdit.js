editDetail.beforeVue = function () {
    editDetail.service = "DpglService";
    editDetail.method = "SearchAssetSpilt";
    editDetail.dataParam.CHANGE_TYPE = 3;  //资产拆分

    editDetail.screenParam.showPop = false;
    editDetail.screenParam.srcPop = "";
    editDetail.screenParam.title = "";
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.ASSETCHANGEITEM = [];
    editDetail.dataParam.ASSETCHANGEITEM2 = [];

    editDetail.screenParam.colDef = [
        { title: '店铺代码', key: 'CODE',width:200 },
        { title: '原建筑面积', key: 'AREA_BUILD_OLD', width: 200 },
        { title: '原使用面积', key: 'AREA_USABLE_OLD', width: 200 },
        { title: '原租赁面积', key: 'AREA_RENTABLE_OLD', width: 200 }
    ];
    editDetail.screenParam.colDef2 = [
        {
            title: "原店铺代码", key: 'CODE_OLD', width: 200
        },
        {
            title: "新店铺代码", key: 'ASSETCODE_NEW', cellType: "input", width: 200
        },
        {
            title: "新建筑面积", key: 'AREA_BUILD_NEW', cellType: "input", cellDataType: "number", width: 200,
            onChange: function (index, row, data) {
                row.AREA_USABLE_NEW = row.AREA_BUILD_NEW;
                row.AREA_RENTABLE_NEW = row.AREA_BUILD_NEW;
            }
        },
        {
            title: "新使用面积", key: 'AREA_USABLE_NEW', cellType: "input", cellDataType: "number", width: 200
        },
        {
            title: "新租赁面积", key: 'AREA_RENTABLE_NEW', cellType: "input", cellDataType: "number", width: 200
        }
    ];
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchAssetSpilt', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.assetSpilt);
        editDetail.dataParam.ASSETCHANGEITEM = data.assetSpiltitem;
        editDetail.dataParam.ASSETCHANGEITEM2 = data.assetSpiltitem2;
        callback && callback(data);
    });
}

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    let item = editDetail.dataParam.ASSETCHANGEITEM;
    if (item.length == 0) {
        iview.Message.info("请确定旧单元!");
        return false;
    }
    let item2 = editDetail.dataParam.ASSETCHANGEITEM2;
    if (item2.length == 0) {
        iview.Message.info("请确定新单元!");
        return false;
    }
    for (let i = 0; i < item.length; i++) {
        item[i].AREA_BUILD_NEW = 0;
        item[i].AREA_USABLE_NEW = 0;
        item[i].AREA_RENTABLE_NEW = 0;
        let num = 0;
        for (let j = 0; j < item2.length; j++) {
            if (!item2[j].ASSETCODE_NEW) {
                iview.Message.info("请确定第"+(j+1)+"行新店铺代码!");
                return false;
            };           
            if (!item2[j].AREA_BUILD_NEW) {
                iview.Message.info("请确定第" + (j + 1) + "行新建筑面积!");
                return false;
            };
            if (!item2[j].AREA_USABLE_NEW) {
                iview.Message.info("请确定第" + (j + 1) + "行新使用面积!");
                return false;
            };
            if (!item2[j].AREA_RENTABLE_NEW) {
                iview.Message.info("请确定第" + (j + 1) + "行新租赁面积!");
                return false;
            };
            if (item[i].ASSETID == item2[j].ASSETID) {
                num++;
                item[i].AREA_BUILD_NEW += Number(item2[j].AREA_BUILD_NEW);
                item[i].AREA_USABLE_NEW += Number(item2[j].AREA_USABLE_NEW);
                item[i].AREA_RENTABLE_NEW += Number(item2[j].AREA_RENTABLE_NEW);
            }
        };
        if (item[i].AREA_BUILD_NEW > item[i].AREA_BUILD_OLD) {
            iview.Message.info(`原店铺代码 ${item[i].CODE} 的拆分建筑面积和不能大于原建筑面积!`);
            return false;
        }
        if (item[i].AREA_USABLE_NEW > item[i].AREA_USABLE_OLD) {
            iview.Message.info(`原店铺代码 ${item[i].CODE} 的拆分使用面积和不能大于原使用面积!`);
            return false;
        }
        if (item[i].AREA_RENTABLE_NEW > item[i].AREA_RENTABLE_OLD) {
            iview.Message.info(`原店铺代码 ${item[i].CODE} 的拆分可租赁面积和不能大于原可租赁面积!`);
            return false;
        }
        if (num < 2) {
            iview.Message.info(`原店铺代码“${item[i].CODE}的至少拆成2个店铺”!`);
            return false;
        }
    };

    return true;
}

editDetail.branchChange = function () {
    editDetail.dataParam.ASSETCHANGEITEM = [];
    editDetail.dataParam.ASSETCHANGEITEM2 = [];
};

editDetail.otherMethods = {
    SelShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择门店!");
            return;
        } else  {
            editDetail.screenParam.title = "选择单元";
            editDetail.screenParam.srcPop = __BaseUrl + "/Pop/Pop/PopShopList/";
            editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, RENT_STATUS: "1" ,STATUS:"2"};
            editDetail.screenParam.showPop = true;
        }         
    },
    delShop: function () {
        let selection = this.$refs.refOldCell.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的单元!");
        } else {
            let data = [];
            for (let i = 0; i < selection.length; i++) {
                let item = editDetail.dataParam.ASSETCHANGEITEM;
                for (let j = 0; j < item.length; j++) {
                    if (item[j].ASSETID == selection[i].ASSETID) {
                        item.splice(j, 1);
                        break;
                    }
                }
                let item2 = editDetail.dataParam.ASSETCHANGEITEM2;
                editDetail.dataParam.ASSETCHANGEITEM2 = item2.filter(function (a) {
                    return a.ASSETID != selection[i].ASSETID
                });
            }          
        }
    },
    addCol2: function () {
        let itemCurRow = this.$refs.refOldCell.getSelection();
        if (itemCurRow.length == 0) {
            iview.Message.info("请选择需要拆分的单元!");
            return;
        }
        if (itemCurRow.length > 1) {
            iview.Message.info("请选择一条需要拆分的单元!");
            return;
        }
        editDetail.dataParam.ASSETCHANGEITEM2.push({
            ASSETID: itemCurRow[0]["ASSETID"],
            CODE_OLD: itemCurRow[0]["CODE"],
            ASSETCODE_NEW: null,
            AREA_BUILD_NEW: null,
            AREA_USABLE_NEW: null,
            AREA_RENTABLE_NEW: null
        });
    },
    delCol2: function () {
        var selection = this.$refs.refNewCell.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的新单元!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.ASSETCHANGEITEM2;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].ASSETID == selection[i].ASSETID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            }
        }
    },
}

//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPop = false;
    if (editDetail.screenParam.title == "选择单元") {
        let itemData = editDetail.dataParam.ASSETCHANGEITEM;
        for (let i = 0; i < data.sj.length; i++) {
            if (itemData.filter(item=> { return (data.sj[i].SHOPID == item.ASSETID) }).length == 0) {
                let shop = {};
                shop.ASSETID = data.sj[i].SHOPID;
                shop.CODE = data.sj[i].SHOPCODE;
                shop.AREA_BUILD_OLD = data.sj[i].AREA_BUILD;
                shop.AREA_USABLE_OLD = data.sj[i].AREA_USABLE;
                shop.AREA_RENTABLE_OLD = data.sj[i].AREA_RENTABLE;
                itemData.push(shop);
            }
        };
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.CHANGE_TYPE = 3;  //资产拆分
    editDetail.dataParam.ASSETCHANGEITEM = [];
    editDetail.dataParam.ASSETCHANGEITEM2 = [];
}
//按钮初始化
editDetail.btnConfig = [{
    id: "add",
    authority: "10400201"
}, {
    id: "edit",
    authority: "10400201"
}, {
    id: "del",
    authority: "10400201"
}, {
    id: "save",
    authority: "10400201"
}, {
    id: "abandon",
    authority: "10400201"
}, {
    id: "confirm",
    name: "审核",
    icon: "md-star",
    authority: "10400202",
    fun: function () {
        _.Ajax('ExecData', {
            Data: { BILLID: editDetail.dataParam.BILLID, CHANGE_TYPE: editDetail.dataParam.CHANGE_TYPE },
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