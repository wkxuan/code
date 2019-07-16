var tempItem2;
var itemCurRow;
editDetail.beforeVue = function () {
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.service = "DpglService";
    editDetail.method = "SearchAssetSpilt";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.CHANGE_TYPE = 3;
    editDetail.screenParam.componentVisible = false;
    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.ASSETCHANGEITEM = [];
    editDetail.dataParam.ASSETCHANGEITEM2 = [];

    editDetail.screenParam.colDef = [
    { title: '店铺ID', key: 'ASSETID', width: 100, hidden: true },
    { title: '店铺代码', key: 'CODE', width: 100 },
    { title: '原建筑面积', key: 'AREA_BUILD_OLD', width: 100 },
    { title: '原使用面积', key: 'AREA_USABLE_OLD', width: 100 },
    { title: '原租赁面积', key: 'AREA_RENTABLE_OLD', width: 100 }
    ];
    editDetail.screenParam.colDef2 = [

        {
            title: "新店铺代码", key: 'ASSETCODE_NEW', width: 100, cellType: "input", cellDataType: "number",
            onBlur: function (index, row, data) {
                editDetail.dataParam.ASSETCHANGEITEM[index].ASSETCODE_NEW = row.ASSETCODE_NEW;
            }

        },
            {
                title: "新建筑面积", key: 'AREA_BUILD_NEW', width: 100, cellType: "input", cellDataType: "number",
                onBlur: function (index, row, data) {
                    editDetail.dataParam.ASSETCHANGEITEM[index].AREA_BUILD_NEW = row.AREA_BUILD_NEW;
                }
            },
            {
                title: "新使用面积", key: 'AREA_USABLE_NEW', width: 100, cellType: "input", cellDataType: "number",
                onBlur: function (index, row, data) {
                    editDetail.dataParam.ASSETCHANGEITEM[index].AREA_USABLE_NEW = row.AREA_USABLE_NEW;
                }
            },
            {
                title: "新租赁面积", key: 'AREA_RENTABLE_NEW', width: 100, cellType: "input", cellDataType: "number",
                onBlur: function (index, row, data) {
                    editDetail.dataParam.ASSETCHANGEITEM[index].AREA_RENTABLE_NEW = row.AREA_RENTABLE_NEW;
                }
            }
    ];

    if (!editDetail.dataParam.ASSETCHANGEITEM) {
        editDetail.dataParam.ASSETCHANGEITEM = [{
            ASSETID: "",
            ASSET_TYPE_OLD: "",
        }]
    }
    if (!editDetail.dataParam.ASSETCHANGEITEM2) {
        editDetail.dataParam.ASSETCHANGEITEM2 = [{
            ASSETID: "",
            ASSET_TYPE_NEW: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM = temp;
    }

}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchAssetSpilt', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.assetSpilt);
        editDetail.dataParam.BILLID = data.assetSpilt.BILLID;
        editDetail.dataParam.ASSETCHANGEITEM = data.assetSpiltitem;
        editDetail.dataParam.ASSETCHANGEITEM2 = data.assetSpiltitem2;
        //editDetail.filter(editDetail.dataParam.ASSETCHANGEITEM[0], 1);
        callback && callback(data);
    });
}
editDetail.IsValidSave = function () {

    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };

    if (editDetail.dataParam.ASSETCHANGEITEM.length == 0) {
        iview.Message.info("请选择单元!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.ASSETCHANGEITEM.length; i++) {
            if (!editDetail.dataParam.ASSETCHANGEITEM[i].ASSETID) {
                iview.Message.info("请选择单元!");
                return false;
            };
        };
    };

    //处理拆分信息
    tempItem2 = tempItem2.filter(function (row2) {
        return parseInt(row2.ASSETID) !== itemCurRow.ASSETID;
    })
    for (inx in editDetail.dataParam.ASSETCHANGEITEM2) {
        tempItem2.push(editDetail.dataParam.ASSETCHANGEITEM2[inx]);
    }
    editDetail.dataParam.ASSETCHANGEITEM2 = tempItem2;
    return true;
}

editDetail.otherMethods = {
    SelShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        } else {
            editDetail.screenParam.showPopShop = true;
            editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, STATUS: "2" };
        }
    },
    delShop: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的单元!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.ASSETCHANGEITEM.length; j++) {
                    if (editDetail.dataParam.ASSETCHANGEITEM[j].ASSETID == selectton[i].ASSETID) {
                        editDetail.dataParam.ASSETCHANGEITEM.splice(j, 1);
                    }
                };
                //删除单元，同时删除新增单元
                for (var j = 0; j < editDetail.dataParam.ASSETCHANGEITEM2.length; j++) {
                    if (editDetail.dataParam.ASSETCHANGEITEM2[j].ASSETID == selectton[i].ASSETID) {
                        editDetail.dataParam.ASSETCHANGEITEM2.splice(j, 1);
                    }
                }
            }
        }
    },
    addCol2: function () {
        if (!itemCurRow) {
            iview.Message.info("请选择单元!");
            return;
        }
        var temp = editDetail.dataParam.ASSETCHANGEITEM2 || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM2 = temp;
    },
    delCol2: function () {
        var selectton = this.$refs.refCol2.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的单元!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.ASSETCHANGEITEM2.length; j++) {
                    if (editDetail.dataParam.ASSETCHANGEITEM2[j].ASSETID == selectton[i].ASSETID) {
                        editDetail.dataParam.ASSETCHANGEITEM2.splice(j, 1);
                    }
                }
            }
        }
    },
    currentChange: function (curRow, oldRow) {
         debugger
        itemCurRow = curRow;
        if (tempItem2 === undefined) {
            tempItem2 = editDetail.dataParam.ASSETCHANGEITEM2
        }
        else {
            for (inx in editDetail.dataParam.ASSETCHANGEITEM2) {
                if ((tempItem2.length === 0) || (tempItem2.length > 0 && tempItem2.filter(function (item) {
              return item.ASSETCODE_NEW === editDetail.dataParam.ASSETCHANGEITEM2[inx].ASSETCODE_NEW;
                }).length === 0)) {
                    tempItem2.push(editDetail.dataParam.ASSETCHANGEITEM2[inx]);
                }
            }
        }
        filterdata = tempItem2.filter(function (row2) {
            return parseInt(row2.ASSETID) === curRow.ASSETID;
        });
        Vue.set(editDetail.dataParam, "ASSETCHANGEITEM2", filterdata);
        if (tempItem2 !== undefined) {
            tempItem2 = tempItem2.filter(function (row2) {
                return parseInt(row2.ASSETID) !== curRow.ASSETID;
            })
        }
    }
}

//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopShop = false;
    for (var i = 0; i < data.sj.length; i++) {
        if ((editDetail.dataParam.ASSETCHANGEITEM.length === 0)
              || (editDetail.dataParam.ASSETCHANGEITEM.length > 0
              && editDetail.dataParam.ASSETCHANGEITEM.filter(function (item) {
              return parseInt(item.ASSETID) === data.sj[i].SHOPID;
        }).length === 0)) {
            var shop = {};
            shop.ASSETID = data.sj[i].SHOPID;
            shop.CODE = data.sj[i].SHOPCODE;
            shop.AREA_BUILD_OLD = data.sj[i].AREA_BUILD;
            shop.AREA_USABLE_OLD = data.sj[i].AREA_USABLE;
            shop.AREA_RENTABLE_OLD = data.sj[i].AREA_RENTABLE;
            editDetail.dataParam.ASSETCHANGEITEM.push(shop);
        }
    };
};


editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.DESCRIPTION = null;
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