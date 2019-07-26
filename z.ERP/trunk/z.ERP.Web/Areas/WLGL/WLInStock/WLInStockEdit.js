editDetail.beforeVue = function () {
    editDetail.service = "WyglService";
    editDetail.method = "GetWlInStock";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.STATUS = "1";


    editDetail.screenParam.showPopWLMerchant = false;
    editDetail.screenParam.srcPopWLMerchant = __BaseUrl + "/" + "Pop/Pop/PopWLMerchantList/";

    editDetail.screenParam.showPopWLGoods = false;
    editDetail.screenParam.srcPopWLGoods = __BaseUrl + "/" + "Pop/Pop/PopWLGoodsList/";
    editDetail.dataParam.WLINSTOCKITETM = [];


    //品牌表格
    editDetail.screenParam.colDefWL = [
        { title: "购进单单号", key: 'BILLID', width: 100 },
        { title: "物料代码", key: 'GOODSDM', width: 100 },
        { title: '物料名称', key: 'NAME', width: 200 },
        { title: '含税采购价', key: 'TAXINPRICE', width: 100 },

        { title: '使用价', key: 'USEPRICE', width: 100 },
        {
            title: "采购数量", key: 'QUANTITY', width: 120, cellType: "input",
            
        },
    ];
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWLINSTOCK', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.STOCK);
        editDetail.dataParam.WLINSTOCKITETM = data.STOCKITEM;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.STATUS = "1";
    editDetail.dataParam.WLINSTOCKITETM = [];
    editDetail.screenParam.popParam = {};
    editDetail.dataParam.DESCRIPTION = null;
}


editDetail.otherMethods = {
    srchWlxx: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择供应商!");
        }
        else {
            editDetail.screenParam.showPopWLGoods = true;
            editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
        }

    },
    delColWlxx: function () {
        var selectton = this.$refs.selectWLxx.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的物料信息!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.WLINSTOCKITETM.length; j++) {
                    if (editDetail.dataParam.WLINSTOCKITETM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.WLINSTOCKITETM.splice(j, 1);
                    }
                }
            }
        }
    },
    Merchant: function () {
        editDetail.screenParam.showPopWLMerchant = true;
    },
};

editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopWLMerchant) {
        editDetail.screenParam.showPopWLMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.MERCHANTID = data.sj[i].MERCHANTID;
            editDetail.dataParam.NAME = data.sj[i].NAME;
        }
    } else if (editDetail.screenParam.showPopWLGoods) {
        editDetail.screenParam.showPopWLGoods = false;

        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.WLINSTOCKITETM.push(data.sj[i]);
        }
    }
};
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入供货商名称!");
        return false;
    };

    if (editDetail.dataParam.WLINSTOCKITETM.length == 0) {
        iview.Message.info("请确定采购信息!");
        return false;
    }
    return true;
}
//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10900301"
    }, {
        id: "edit",
        authority: "10900301"
    }, {
        id: "del",
        authority: "10900301"
    }, {
        id: "save",
        authority: "10900301"
    }, {
        id: "abandon",
        authority: "10900301"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10900302",
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