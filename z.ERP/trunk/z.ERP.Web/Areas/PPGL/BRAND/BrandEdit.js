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
        $.extend(editDetail.dataParam, data.main[0]);
        editDetail.dataParam.BILLID = data.main[0].ID;
        editDetail.clearKey();
        callback && callback(data);
        
    });
}
