define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'FILECODE', width: 150
        },
        {
            title: '名称',
            key: 'FILENAME'
        }];
    define.service = "XtglService";
    define.method = "GetEnergyFilesElement";
    define.methodList = "GetEnergyFiles";
    define.Key = 'FILEID';
}

define.otherMethods = {
    SelShop: function () {
        define.screenParam.popParam = {};
        define.popConfig.title = "选择单元";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopShopList/";
        define.popConfig.open = true;
    }
}

define.initDataParam = function () {
    define.dataParam.FILEID = "";
    define.dataParam.FILECODE = "";
    define.dataParam.FILENAME = "";
    define.dataParam.SHOPCODE = "";
    define.dataParam.SHOPID = "";
    define.dataParam.FACTORY_DATE = "";
    define.dataParam.PRICE = "";
    define.dataParam.MULTIPLE = "";
    define.dataParam.VALUE_LAST = "";
    define.dataParam.DATE_LAST = "";
    define.dataParam.AREAID = "";
    define.dataParam.DESCRIPTION = "";
}

define.newRecord = function () {
    define.dataParam.AREAID = "2";
}

//接收子页面返回值
define.popCallBack = function (data) {
    define.popConfig.open = false;
    if (define.popConfig.title == "选择单元") {
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.SHOPID = data.sj[i].SHOPID;
            define.dataParam.SHOPCODE = data.sj[i].SHOPCODE;
        };
    }
};

define.IsValidSave = function () {
    if (!define.dataParam.FILECODE) {
        iview.Message.info("设备代码不能为空!");
        return false;
    }
    if (!define.dataParam.FILENAME) {
        iview.Message.info("设备名称不能为空!");
        return false;
    }
    if (!define.dataParam.SHOPCODE) {
        iview.Message.info("所属单元不能为空!");
        return false;
    }
    if (!define.dataParam.PRICE) {
        iview.Message.info("单价不能为空!");
        return false;
    }
    if (isNaN(define.dataParam.PRICE)) {
        iview.Message.info("单价必须为数字!");
        return false;
    }
    if (!define.dataParam.MULTIPLE) {
        iview.Message.info("倍率不能为空!");
        return false;
    }
    if (isNaN(define.dataParam.MULTIPLE)) {
        iview.Message.info("倍率必须为数字!");
        return false;
    }
    if (!define.dataParam.VALUE_LAST) {
        iview.Mesaage.info("最后读数不能为空!");
        return false;
    }
    if (isNaN(define.dataParam.VALUE_LAST)) {
        iview.Message.info("最后读数必须为数字!");
        return false;
    }

    return true;
};