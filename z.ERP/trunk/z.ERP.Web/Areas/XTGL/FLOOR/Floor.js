define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'CODE', width: 75
        },
        {
            title: '名称',
            key: 'NAME', width: 200
        }];
    define.screenParam.dataDef = [];
    define.service = "DpglService";
    define.method = "SearchFloor";
    define.methodList = "SearchFloor";
    define.Key = 'ID';

    define.screenParam.componentVisible = false;
    define.screenParam.branchData = [];
    define.searchParam.BRANCHID = 0;
    define.dataParam.ORGIDCASCADER = [];
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
        }
        else {

        }
    });
}


define.newRecord = function () {
    if (define.searchParam.BRANCHID == 0) {
        iview.Message.info("请请选择门店!");
        return;
    };
    define.dataParam.STATUS = 1;
    define.dataParam.ORGIDCASCADER = [];
    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
}

define.otherMethods = {
    branchChange: function (value) {
        define.showlist();
    },
    orgChange: function (value, selectedData) {
        define.dataParam.ORGID = value[value.length - 1];
    },
    kindChange: function (value, selectedData) {
        editDetail.dataParam.KINDID = value[value.length - 1];
    },
}

define.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(define.screenParam, "ORGData", data.treeOrg.Obj);
    });
}

define.showone = function (data, callback) {
    _.Ajax('GetFloor', {
        Data: { ID: data }
    }, function (data) {
        $.extend(define.dataParam, data.floorelement);
        define.dataParam.ORGIDCASCADER = define.dataParam.ORGIDCASCADER.split(",");
        callback && callback();
    });
}