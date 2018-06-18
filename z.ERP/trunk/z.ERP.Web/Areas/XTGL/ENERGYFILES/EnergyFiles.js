define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'FILECODE', width: 150
        },
        {
            title: '名称',
            key: 'FILENAME', width: 250
        }];

    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetEnergyFilesElement";
    define.methodList = "GetEnergyFiles";
    define.Key = 'FILEID';
    define.screenParam.componentVisible = false;
    define.screenParam.showPopShop = false;
    define.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    define.screenParam.popParam = {};
    define.dataParam.SHOPCODE = "";
}
define.otherMethods = {
    SelShop: function () {
        define.screenParam.showPopShop = true;
    }
}
define.newRecord = function () {
    define.dataParam.AREAID = "2";
}

//接收子页面返回值
define.popCallBack = function (data) {
    define.screenParam.showPopShop = false;
    for (var i = 0; i < data.sj.length; i++) {
        define.dataParam.SHOPID = data.sj[i].SHOPID;
        define.dataParam.SHOPCODE = data.sj[i].SHOPCODE;
        //define.dataParam.ENERGY_FILES_SHOP.push(data.sj[i]);
    };
};

