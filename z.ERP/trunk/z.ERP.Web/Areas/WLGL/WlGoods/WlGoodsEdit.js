﻿editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "WyglService";
    editDetail.method = "GetWlGoods";
    editDetail.Key = 'GOODSID';
    editDetail.dataParam.STATUS = "1";
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchWlGoods', {
        Data: { GOODSDM: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.goods);
        editDetail.dataParam.BILLID = data.goods.GOODSDM;
        callback && callback(data);
    });
}


editDetail.clearKey = function () {
    editDetail.dataParam.GOODSDM = null;
    editDetail.dataParam.GOODSID = null;
    editDetail.dataParam.NAME = null;

    editDetail.dataParam.PYM = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.JXSL = null;
    editDetail.dataParam.TAXINPRICE = null;
    editDetail.dataParam.NOTAXINPRICE = null;
    editDetail.dataParam.USEPRICE = null;
    editDetail.dataParam.DESCRIPTION = null;
}


editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请输入物料名称!");
        return false;
    };
    return true;
}