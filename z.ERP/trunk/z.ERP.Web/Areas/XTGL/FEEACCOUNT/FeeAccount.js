define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '单位编号', key: 'ID', width: 90 },
        { title: '单位名称', key: 'NAME' },
        { title: "门店", key: 'BRANCHNAME' }
    ];
    define.screenParam.branchData = [];
    define.service = "XtglService";
    define.method = "GetFeeAccount";
    define.methodList = "GetFeeAccount";
    define.Key = 'ID';
}
define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.ADDRESS = "";
    define.dataParam.CONTACT = "";
    define.dataParam.CONTACT_NUM = "";
    define.dataParam.BANK = "";
    define.dataParam.ACCOUNT = "";
    define.dataParam.BRANCHID = "";
}
define.mountedInit = function () {
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            Vue.set(define.dataParam, "BRANCHID", define.searchParam.BRANCHID);
            define.showlist();
        }
    });
}
define.otherMethods = {
    branchChange: function (value) {
        debugger
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.initDataParam();
        define.showlist();
    },    
};
define.IsValidSave = function () {
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("门店不能为空!");
        return false;
    }
    if (!define.dataParam.NAME) {
        iview.Message.info("单位名称不能为空!");
        return false;
    }
    return true;
}