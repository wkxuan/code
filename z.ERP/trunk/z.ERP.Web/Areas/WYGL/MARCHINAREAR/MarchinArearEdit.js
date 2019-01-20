editDetail.beforeVue = function () {

    editDetail.service = "WyglService";
    editDetail.method = "GetMarchinArearElement";
    editDetail.Key = 'BILLID';
    editDetail.branchid = false;

    editDetail.screenParam.colDef = [        
        { title: "商铺代码", key: "CODE", width: 100, },
        { title: "商铺名称", key: "NAME", width: 100, },
        { title: "建筑面积", key: "AREA_BUILD", width: 100, }
    ]

    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";

    editDetail.IsValidSave = function () {
        var d = new Date(editDetail.dataParam.MARCHINDATE);
        editDetail.dataParam.MARCHINDATE = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
        return true;
    }
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchElement', {
        Data: { BILLID: data }
    }, function (data) {
        editDetail.dataParam.BILLID = data.main[0].BILLID;
        editDetail.dataParam.BRANCHID = data.main[0].BRANCHID;
        editDetail.dataParam.CONTRACTID = data.main[0].CONTRACTID;
        editDetail.dataParam.MARCHINDATE = data.main[0].MARCHINDATE;
        editDetail.dataParam.STATUS = data.main[0].STATUS;
        editDetail.dataParam.DESCRIPTION = data.main[0].DESCRIPTION;

        editDetail.dataParam.MARCHINAREARITEM = data.item[0];
        callback && callback(data);
    });
}


editDetail.otherMethods = {    
    SelContract: function () {
        editDetail.screenParam.showPopContract = true;
    },

}

editDetail.popCallBack = function (data) {
     if (editDetail.screenParam.showPopContract) {
        editDetail.dataParam.CONTRACTID = data.sj[0].CONTRACTID;
        editDetail.screenParam.showPopContract = false;
        _.Ajax('GetContract', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length > 0) {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;                
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
                editDetail.dataParam.GOODS_SHOP = data.shop;
            }
            else {
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }
        })
    }
}