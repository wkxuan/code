define.beforeVue = function()
{
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 200 },
        { title: "名称", key: "NAME", width: 250 },
    ];
    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetLateFeeRule";
    define.methodList = "GetLateFeeRule";
}

define.getKey = function (data) {
    if (typeof (data) == "undefined") {
        return { ID: define.dataParam.ID }
    }
    else {
        return { ID: data }
    }
}