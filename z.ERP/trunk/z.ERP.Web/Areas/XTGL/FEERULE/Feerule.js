﻿define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: "代码", key: "ID", width: 100 },
        { title: "名称", key: "NAME" },
    ];

    define.service = "XtglService";
    define.method = "GetFeeRule";
    define.methodList = "GetFeeRule";
    define.Key = "ID";
}

define.initDataParam = function () {
    define.dataParam.ID = "";
    define.dataParam.NAME = "";
    define.dataParam.PAY_CYCLE = "";
    define.dataParam.FEE_DAY = "";
    define.dataParam.ADVANCE_CYCLE = "";
    define.dataParam.UP_DATE = "";
    define.dataParam.PAY_UP_CYCLE = "";
    define.dataParam.VOID_FLAG = "";
}

define.newRecord = function () {
    define.dataParam.ADVANCE_CYCLE = "1";
    define.dataParam.UP_DATE = "1";
    define.dataParam.PAY_UP_CYCLE = "1";
    define.dataParam.VOID_FLAG = 1;

}