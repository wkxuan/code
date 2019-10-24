/*
alter table JOIN_BILL rename column JE_17 to JE_16;
alter table JOIN_BILL rename column ZZSJE_17 to ZZSJE_16;
alter table JOIN_BILL rename column JE_11 to JE_10;
alter table JOIN_BILL rename column ZZSJE_11 to ZZSJE_10;

*/

editDetail.beforeVue = function () {
    editDetail.service = "JsglService";
    editDetail.method = "GetJoinBillElement"
    editDetail.branchid = false;

    editDetail.screenParam.goodscolDef = [
        { title: "商品代码", key: "GOODSDM", width: 100, },
        { title: "商品名称", key: "NAME", width: 100, },
        { title: "扣率", key: "DRATE", width: 80, },
        { title: "税率", key: "JXSL", width: 80, },
        { title: "销售数量", key: "SELL_SL", width: 90, },
        { title: "销售金额", key: "SELL_JE", width: 100, },
        { title: "优惠金额", key: "YHJE", width: 90, },
        { title: "结算金额价款", key: "SELL_COST", width: 100, },
        { title: "税金", key: "ZZSJE", width: 100, },
    ];
    editDetail.screenParam.trimcolDef = [
        { title: "扣款项目ID", key: "TRIMID", width: 100, },
        { title: "扣款项目名称", key: "NAME", width: 100, },
        { title: "序号", key: "INX", width: 70, },
        { title: "类型", key: "TYPE", width: 70, },
        { title: "金额", key: "JE", width: 100, },
    ];
}
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: ""
    }, {
        id: "edit",
        authority: ""
    }, {
        id: "del",
        authority: ""
    }, {
        id: "save",
        authority: ""
    }, {
        id: "abandon",
        authority: ""
    }]
};
editDetail.showOne = function (data, callback) {
    _.Ajax('GetJoinBillElement', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.joinbill[0]);
        editDetail.dataParam.BILLID = data.joinbill[0].BILLID;
        editDetail.dataParam.BILL_GOODS = data.bill_goods[0];
        editDetail.dataParam.BILL_TRIM = data.bill_trim[0];
        callback && callback(data);
    });
}