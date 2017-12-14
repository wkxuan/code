define.beforeVue = function()
{
    define.screenParam.colDef = [
        { title: "代码", key: "CODE", width: 200 },
        { title: "名称", key: "NAME", width: 250 },
    ];
    define.screenParam.dataDef = [];
}
define.search = function () {
    _.Search({
        Service: "TestService",
        Method: "GetFeeRule",
        Data: {},
        Success: function (data) {
            define.screenParam.dataDef = data.rows;
        }
    })
}