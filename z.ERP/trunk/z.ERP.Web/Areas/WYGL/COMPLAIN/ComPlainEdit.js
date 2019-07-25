editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.Key = 'BILLID';

    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    editDetail.screenParam.showPopBrand = false;
    editDetail.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";

    editDetail.IsValidSave = function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请确门店!");
            return false;
        };
        if (!editDetail.dataParam.CPLAINDEPT) {
            iview.Message.info("请确认投诉处理部门!");
            return false;
        };
        if (!editDetail.dataParam.CPLAINTYPE) {
            iview.Message.info("请确认投诉类型!");
            return false;
        };
        if (!editDetail.dataParam.CPLAINTYPE) {
            iview.Message.info("请确认投诉类型!");
            return false;
        };
        return true;
    }
}

editDetail.otherMethods = {

    SelBrand: function () {
        editDetail.screenParam.showPopBrand = true;
        
    },
    SelShop: function () {
        editDetail.screenParam.showPopShop = true;
        editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, STATUS: 1 };
    },
}


editDetail.popCallBack = function (data) {

    if (editDetail.screenParam.showPopShop) {
        editDetail.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.SHOPID = data.sj[i].SHOPID;
            editDetail.dataParam.CODE = data.sj[i].CODE;
            editDetail.dataParam.NAME = data.sj[i].NAME;
        }
    }

    if (editDetail.screenParam.showPopBrand) {
        editDetail.screenParam.showPopBrand = false;
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.BRANDID = data.sj[i].BRANDID;
            editDetail.dataParam.BRANDNAME = data.sj[i].NAME;
        }
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.BRANDID = null;
    editDetail.dataParam.BRANDNAME = null;
    editDetail.dataParam.SHOPID = null;
    editDetail.dataParam.CODE = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.STATUS = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.DEPTID = null;
    editDetail.dataParam.PERSON_NAME = null;
    editDetail.dataParam.CPLAINDEPT = null;
    editDetail.dataParam.CPLAINTYPE = null;
    editDetail.dataParam.CPLAINDATE = null;
    editDetail.dataParam.CPLAINPERSON_NAME = null;
    editDetail.dataParam.CPLAINPHONE = null;
    editDetail.dataParam.CPLAINTEXT = null;
    editDetail.dataParam.DISPOSERESULT = null;
}




editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneComPlainEdit', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.main[0]);       
        editDetail.dataParam.BILLID = data.main[0].BILLID;
        //editDetail.dataParam.BRANCHID = null;
        //editDetail.dataParam.BRANDID = null;
        //editDetail.dataParam.BRANDNAME = null;
        //editDetail.dataParam.SHOPID = null;
        //editDetail.dataParam.CODE = null;
        //editDetail.dataParam.NAME = null;
        //editDetail.dataParam.STATUS = null;
        //editDetail.dataParam.DESCRIPTION = null;
        //editDetail.dataParam.DEPTID = null;
        //editDetail.dataParam.PERSON_NAME = null;
        //editDetail.dataParam.CPLAINDEPT = null;
        //editDetail.dataParam.CPLAINTYPE = null;
        //editDetail.dataParam.CPLAINDATE = null;
        //editDetail.dataParam.CPLAINPERSON_NAME = null;
        //editDetail.dataParam.CPLAINPHONE = null;
        //editDetail.dataParam.CPLAINTEXT = null;
        //editDetail.dataParam.DISPOSERESULT = null;
        callback && callback(data);
    });
}
