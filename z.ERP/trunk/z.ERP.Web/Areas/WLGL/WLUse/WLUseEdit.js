editDetail.beforeVue = function () {
    editDetail.service = "WyglService";
    editDetail.method = "GetWlUses";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.STATUS = "1";


    editDetail.screenParam.showPopWLMerchant = false;
    editDetail.screenParam.srcPopWLMerchant = __BaseUrl + "/" + "Pop/Pop/PopWLMerchantList/";

    editDetail.screenParam.showPopWLGoods = false;
    editDetail.screenParam.srcPopWLGoods = __BaseUrl + "/" + "Pop/Pop/PopWLGoodsStockList/";
    editDetail.dataParam.WLUSESITETM = [];


    //品牌表格
    editDetail.screenParam.colDefWL = [
        { type: 'selection', width: 60, align: 'center' },
        { title: "物料代码", key: 'GOODSDM', width: 100 },
        { title: '物料名称', key: 'NAME', width: 200 },
        { title: '可领用数量', key: 'CANQTY', width: 100 },
        {
            title: "领用数量", key: 'QUANTITY', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.QUANTITY
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.WLUSESITETM[params.index].QUANTITY = event.target.value;
                        }
                    },
                })
            },
        },
    ];
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWLUSES', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.WLUSES);
        editDetail.dataParam.WLUSESITETM = data.WLUSESITEM;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.STATUS = "1";
    editDetail.dataParam.WLUSESITETM = [];
    editDetail.screenParam.popParam = {};
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
                for (var j = 0; j < editDetail.dataParam.WLUSESITETM.length; j++) {
                    if (editDetail.dataParam.WLUSESITETM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.WLUSESITETM.splice(j, 1);
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
            editDetail.dataParam.WLUSESITETM.push(data.sj[i]);
        }
    }
};
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入供货商名称!");
        return false;
    };

    if (editDetail.dataParam.WLUSESITETM.length == 0) {
        iview.Message.info("请确定领用信息!");
        return false;
    }
    for (var i = 0; i < editDetail.dataParam.WLUSESITETM.length; i++) {
        if (editDetail.dataParam.WLUSESITETM[i].QUANTITY > editDetail.dataParam.WLUSESITETM[i].CANQTY) {
            iview.Message.info("领用数量不能大于可领用数量!");
            return false;
        }
    }
    return true;
}