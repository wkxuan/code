editDetail.beforeVue = function () {
    editDetail.service = "SpglService";
    editDetail.method = "GetGoodsElement"
    editDetail.Key = 'GOODID';
    editDetail.branchid = false;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneEdit', {
        Data: { GOODSID: data }
    }, function (data) {
        editDetail.dataParam.BILLID = data.goods[0].GOODSDM;
        editDetail.dataParam.GOODSID = data.goods[0].GOODSID;
        editDetail.dataParam.GOODSDM = data.goods[0].GOODSDM;
        editDetail.dataParam.NAME = data.goods[0].NAME;
        editDetail.dataParam.BARCODE = data.goods[0].BARCODE;
        editDetail.dataParam.PYM = data.goods[0].PYM;
        editDetail.dataParam.CONTRACTID = data.goods[0].CONTRACTID;
        editDetail.dataParam.MERCHANTID = data.goods[0].MERCHANTID;
        editDetail.dataParam.SHMC = data.goods[0].SHMC;
        editDetail.dataParam.KINDID = data.goods[0].KINDID;
        editDetail.dataParam.TYPE = data.goods[0].TYPE;
        editDetail.dataParam.REGION = data.goods[0].REGION;
        editDetail.dataParam.STYLE = data.goods[0].STYLE;
        editDetail.dataParam.JXSL = data.goods[0].JXSL;
        editDetail.dataParam.XXSL = data.goods[0].XXSL;
        editDetail.dataParam.PRICE = data.goods[0].PRICE;
        editDetail.dataParam.MEMBER_PRICE = data.goods[0].MEMBER_PRICE;
        editDetail.dataParam.DESCRIPTION = data.goods[0].DESCRIPTION;
        callback && callback(data);
    });
}
