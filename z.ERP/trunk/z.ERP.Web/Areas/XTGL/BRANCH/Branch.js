define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "分店代码",
            key: 'ID', width: 150
        },
        {
            title: '门店名称',
            key: 'NAME', width: 250
        }];

    define.screenParam.dataDef = [];
}


define.search = function () {
    _.Search({
        Service: 'TestService',
        Method: 'GetBranch',
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}

define.newRecord = function () {
    define.dataParam.STATUS = "1";
}
