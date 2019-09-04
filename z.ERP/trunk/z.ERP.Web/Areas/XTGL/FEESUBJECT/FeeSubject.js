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

    define.dataParam.PYM = "";
    define.dataParam.NAME = "";
};

define.otherMethods = {
    NameChange: function () {
        if (define.myve.dataParam.NAME)
            define.myve.dataParam.PYM = define.myve.dataParam.NAME.toPYM();
    },
};


define.newRecord = function () {
    define.myve.dataParam.VOID_FLAG = 1;
    define.myve.dataParam.ACCOUNT = "2";

};



define.beforeDel = function () {
    if (!define.dataParam.TRIMID>=2000) {
        iview.Message.info("预定义的收费项目不能删除!");
        return false;
    }
    return true;
}

