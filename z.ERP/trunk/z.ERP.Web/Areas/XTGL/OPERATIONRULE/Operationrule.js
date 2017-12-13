define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 150 },
        { title: "名称", key: "NAME", width: 150 }
    ];
    define.screenParam.dataDef = [];
}

define.search = function () {
    _.Search({
        Service: "TestService",
        Method: "GetOperationrule",
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}