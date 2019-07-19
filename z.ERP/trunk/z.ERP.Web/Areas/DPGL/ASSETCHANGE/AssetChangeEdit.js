editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       //{ DESCRIPTION: "已完成", OPERATION: "张三  2019-01-01" },
       //{ DESCRIPTION: "待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.service = "DpglService";
    editDetail.method = "GetAssetChange";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.CHANGE_TYPE = 1;
    editDetail.screenParam.componentVisible = false;
    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.ASSETCHANGEITEM = [];


    editDetail.screenParam.colDef = [
    { title: '店铺代码', key: 'CODE', width: 100 },
    { title: '原建筑面积', key: 'AREA_BUILD_OLD', width: 100 },
    { title: '原使用面积', key: 'AREA_USABLE_OLD', width: 100 },
    { title: '原租赁面积', key: 'AREA_RENTABLE_OLD', width: 100 },
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

    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM = temp;
        //        editDetail.dataParam.ASSETCHANGEITEM.CHANGE_TYPE = '1';
    }
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
                }
            }
        }
    }
}
editDetail.newRecord = function () {
    editDetail.dataParam.AREAID = "2";
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchAssetChange', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.assetchange);
        editDetail.dataParam.BILLID = data.assetchange.BILLID;
        editDetail.dataParam.ASSETCHANGEITEM = data.assetchangeitem;
        callback && callback(data);
    });
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopShop = false;

    //接收选中的数据
    for (var i = 0; i < data.sj.length; i++) {
        if ((editDetail.dataParam.ASSETCHANGEITEM.length === 0)
            || (editDetail.dataParam.ASSETCHANGEITEM.length > 0
            && editDetail.dataParam.ASSETCHANGEITEM.filter(function (item) {
            return parseInt(item.ASSETID) === data.sj[i].SHOPID;
        }).length === 0))
            editDetail.dataParam.ASSETCHANGEITEM.push({
                ASSETID: data.sj[i].SHOPID,
                CODE: data.sj[i].SHOPCODE,
                AREA_BUILD_OLD: data.sj[i].AREA_BUILD,
                AREA_USABLE_OLD: data.sj[i].AREA_USABLE,
                AREA_RENTABLE_OLD: data.sj[i].AREA_RENTABLE
            });
    }
};


editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.STATUS = "未审核";
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.ASSETCHANGEITEM = [];
}
//按钮初始化
editDetail.btnConfig = [{
    id: "add",
    authority: "10400101"
}, {
    id: "edit",
    authority: "10400101"
}, {
    id: "del",
    authority: "10400101"
}, {
    id: "save",
    authority: "10400101"
}, {
    id: "abandon",
    authority: "10400101"
}, {
    id: "confirm",
    name: "审核",
    icon: "md-star",
    authority: "10400102",
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

            if (!editDetail.dataParam.ASSETCHANGEITEM[i].AREA_RENTABLE_NEW) {
                iview.Message.info("请输入新租赁面积!");
                return false;
            }
        };
    };

    return true;
}

