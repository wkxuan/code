define.beforeVue = function () {

    define.screenParam.colDef = [
    { title: '单位编号', key: 'ID', width: 90 },
    { title: '单位名称', key: 'NAME' },
    { title: "门店", key: 'BRANCHNAME' }

    ];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetFeeAccount";
    define.methodList = "GetFeeAccount";
    define.Key = 'ID';

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
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        define.showlist();
    },    
};
define.IsValidSave = function () {
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
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