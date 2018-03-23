﻿define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "收费项目代码",
            key: 'TRIMID', width: 150
        },
        {
            title: '收费项目名称',
            key: 'NAME', width: 250
        }];

    define.screenParam.dataDef = [];

    define.service = "XtglService";
    define.method = "GetFeeSubjectElement";
    define.methodList = "GetFeeSubject";
    define.Key = 'TRIMID';
};

define.otherMethods = {
    NameChange: function(){ 
        define.dataParam.PYM=define.dataParam.NAME.toPYM();
        //Vue.set(define.dataParam, "PYM", define.dataParam.NAME.toPYM());
    },
};


define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
};
