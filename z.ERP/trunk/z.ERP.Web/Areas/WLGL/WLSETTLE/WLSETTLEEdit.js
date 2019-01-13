editDetail.beforeVue = function () {
    editDetail.service = "WyglService";
    editDetail.method = "GetWLSETTLE";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.STATUS = "1";


    editDetail.screenParam.showPopWLMerchant = false;
    editDetail.screenParam.srcPopWLMerchant = __BaseUrl + "/" + "Pop/Pop/PopWLMerchantList/";

    editDetail.screenParam.showPopWLGoodsDjxx = false;
    editDetail.screenParam.srcPopWLGoodsDjxx = __BaseUrl + "/" + "Pop/Pop/PopWLGoodsDjxxList/";
    editDetail.dataParam.WLSETTLEITEM = [];


    //品牌表格
    editDetail.screenParam.colDefWL = [
        { type: 'selection', width: 60, align: 'center' },
        { title: "业务单单号", key: "DH", width: 100 },
        { title: "业务类型", key: "LXMC", width: 100 },
        { title: "物料编号", key: "GOODSDM", width: 100 },
        { title: "物料名称", key: "NAME", width: 200 },
        { title: "含税采购价", key: "TAXINPRICE", width: 100 },
        { title: "数量", key: "QUANTITY", width: 100 }
    ];
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWLSETTLE', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.MAIN);
        editDetail.dataParam.WLSETTLEITEM = data.ITEM;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.STATUS = "1";
    editDetail.dataParam.WLSETTLEITEM = [];
    editDetail.screenParam.popParam = {};
}


editDetail.otherMethods = {
    srchWlxx: function () {
        if (!editDetail.dataParam.MERCHANTID) {
            iview.Message.info("请先选择供应商!");
        }
        else {
            editDetail.screenParam.showPopWLGoodsDjxx = true;
            editDetail.screenParam.popParam = { MERCHANTID: editDetail.dataParam.MERCHANTID };
        }

    },
    delColWlxx: function () {
        var selectton = this.$refs.selectWLxx.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的物料信息!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.WLSETTLEITEM.length; j++) {
                    if ((editDetail.dataParam.WLSETTLEITEM[j].GOODSID == selectton[i].GOODSID)
                        && (editDetail.dataParam.WLSETTLEITEM[j].DH == selectton[i].DH)
                        && (editDetail.dataParam.WLSETTLEITEM[j].LX == selectton[i].LX)
                    ) {
                        editDetail.dataParam.WLSETTLEITEM.splice(j, 1);
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
    } else if (editDetail.screenParam.showPopWLGoodsDjxx) {
        editDetail.screenParam.showPopWLGoodsDjxx = false;

        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.WLSETTLEITEM.push(data.sj[i]);
        }
    }
};
editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入供货商名称!");
        return false;
    };

    if (editDetail.dataParam.WLSETTLEITEM.length == 0) {
        iview.Message.info("请确定结算信息!");
        return false;
    }
    return true;
}