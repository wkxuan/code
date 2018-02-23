editDetail.beforeVue = function () {   
    editDetail.service = "XtglService";
    editDetail.method = "GetBrandData";
    editDetail.branchid = false;
    editDetail.Key = "ID";
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchElement', {
        Data: { ID: data }
    }, function (data) {
        editDetail.dataParam.BILLID = data.main[0].ID;
        editDetail.dataParam.ID = data.main[0].ID;
        editDetail.dataParam.NAME = data.main[0].NAME;
        editDetail.dataParam.CATEGORYID = data.main[0].CATEGORYID;
        editDetail.dataParam.ADRESS = data.main[0].ADRESS;
        editDetail.dataParam.CONTACTPERSON = data.main[0].CONTACTPERSON;
        editDetail.dataParam.PHONENUM = data.main[0].PHONENUM;
        editDetail.dataParam.WEIXIN = data.main[0].WEIXIN;
        editDetail.dataParam.PIZ = data.main[0].PIZ;
        editDetail.dataParam.QQ = data.main[0].QQ;

        callback && callback(data);
    });
}
