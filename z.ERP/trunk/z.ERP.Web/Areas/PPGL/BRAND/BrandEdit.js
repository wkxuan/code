editDetail.beforeVue = function () {   
    editDetail.service = "XtglService";
    editDetail.method = "GetBrandData";
    editDetail.branchid = false;

    editDetail.Key = "ID";

    editDetail.screenParam.CATEGORYIDCASCADER = [];
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchElement', {
        Data: { ID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.main[0]);
        editDetail.dataParam.BILLID = data.main[0].ID;

        editDetail.dataParam.CATEGORYID = data.main[0].CATEGORYID;
        var arr = data.main[0].CATEGORYIDCASCADER.split(",") || [];
        Vue.set(editDetail.screenParam, "CATEGORYIDCASCADER", arr);

        callback && callback(data);

    });
};



editDetail.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "CATEData", data.treeOrg.Obj);
    });
}


editDetail.otherMethods = {
    orgChange: function (value, selectedData) {
        editDetail.dataParam.CATEGORYID = value[value.length - 1]; 
    },
}