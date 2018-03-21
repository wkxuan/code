editDetail.beforeVue = function () {
    editDetail.service = "JsglService";
    editDetail.method = "GetJoinBillElement"
    editDetail.Key = 'BILLID';
    editDetail.branchid = false;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('GetJoinBillElement', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.joinbill[0]);
        editDetail.dataParam.BILLID = data.joinbill[0].BILLID;
        callback && callback(data);
    });
}