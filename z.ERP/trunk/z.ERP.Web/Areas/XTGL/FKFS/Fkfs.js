define.beforeVue = function () {
    define.screenParam.colFkfs = [
        { title: '代码', key: 'ID', width: 150 },
        { title: '名称', key: 'NAME', width: 150 },
    ]
    define.screenParam.dataFkfs = [];
}

define.search = function ()
{
    _.Search({
        Service: "TestService",
        Method: "GetFkfs",
        Data: {},
        Success: function (data) {
            define.screenParam.dataFkfs = data.rows;
        }
    })
}