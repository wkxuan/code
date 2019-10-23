define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: "收费项目代码",
            key: 'TRIMID', width: 120
        },
        {
            title: '收费项目名称',
            key: 'NAME'
        }];

    define.service = "XtglService";
    define.method = "GetFeeSubjectElement";
    define.methodList = "GetFeeSubject";
    define.Key = 'TRIMID';
};

define.initDataParam = function () {
    define.dataParam.TRIMID = "";
    define.dataParam.NAME = "";
    define.dataParam.PYM = "";
    define.dataParam.TYPE = "";
    define.dataParam.DEDUCTION = "";
    define.dataParam.SUBJECT_CODE = "";
    define.dataParam.SCFS_TZD = "";
    define.dataParam.VOID_FLAG = "";
}

define.otherMethods = {
    NameChange: function () {
        if (define.dataParam.NAME)
            define.dataParam.PYM = define.dataParam.NAME.toPYM();
    },
};


define.newRecord = function () {
    define.dataParam.VOID_FLAG = 1;
    define.dataParam.ACCOUNT = "2";
};

define.beforeDel = function () {
    if (!define.dataParam.TRIMID>=2000) {
        iview.Message.info("预定义的收费项目不能删除!");
        return false;
    }
    return true;
}

