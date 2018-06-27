define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "描述", key: "DESCRIPTION", width: 200 },
        { title: "当前值", key: "CUR_VAL", width: 80 },
        { title: "缺省值", key: "DEF_VAL", width: 80 },
        { title: "最大值", key: "MAX_VAL", width: 80 },
        { title: "最小值", key: "MIN_VAL", width: 80 },
        { title: "代码", key: "ID", width: 50 },
    ];
    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetConfig";
    define.methodList = "GetConfig";
    define.Key = "ID";
}

