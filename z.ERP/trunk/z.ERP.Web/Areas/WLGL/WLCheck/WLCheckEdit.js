editDetail.beforeVue = function () {
    editDetail.service = "WyglService";
    editDetail.method = "GetWlCheck";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.STATUS = "1";


    editDetail.screenParam.showPopWLMerchant = false;
    editDetail.screenParam.srcPopWLMerchant = __BaseUrl + "/" + "Pop/Pop/PopWLMerchantList/";

    editDetail.screenParam.showPopWLGoods = false;
    editDetail.screenParam.srcPopWLGoods = __BaseUrl + "/" + "Pop/Pop/PopWLGoodsStockList/";
    editDetail.dataParam.WLCHECKITEM = [];


    //品牌表格
    editDetail.screenParam.colDefWL = [
        { type: 'selection', width: 60, align: 'center' },
        { title: "物料代码", key: 'GOODSDM', width: 100 },
        { title: '物料名称', key: 'NAME', width: 200 },
        { title: '账面数量', key: 'CANQTY', width: 100 },
        {
            title: "损溢数量", key: 'QUANTITY', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.QUANTITY
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.WLCHECKITEM[params.index].QUANTITY = event.target.value;
                        }
                    },
                })
            },
        },
    ];
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWLCHECKE', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.WLCHECK);
        editDetail.dataParam.WLCHECKITEM = data.WLCHECKITEM;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.STATUS = "1";
    editDetail.dataParam.WLCHECKITEM = [];
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
                for (var j = 0; j < editDetail.dataParam.WLCHECKITEM.length; j++) {
                    if (editDetail.dataParam.WLCHECKITEM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.WLCHECKITEM.splice(j, 1);
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
            editDetail.dataParam.WLCHECKITEM.push(data.sj[i]);
        }
    }
};
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入供货商名称!");
        return false;
    };

    if (editDetail.dataParam.WLCHECKITEM.length == 0) {
        iview.Message.info("请损溢信息!");
        return false;
    }
    for (var i = 0; i < editDetail.dataParam.WLCHECKITEM.length; i++) {
        if (editDetail.dataParam.WLCHECKITEM[i].QUANTITY > editDetail.dataParam.WLCHECKITEM[i].CANQTY) {
            iview.Message.info("损溢用数量不能大于账面数量!");
            return false;
        }
    }
    return true;
}