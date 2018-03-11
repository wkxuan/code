define.beforeVue = function () {

    define.screenParam.colDef = [
        {
            title: "代码",
            key: 'ROLECODE', width: 100
        },
        {
            title: '名称',
            key: 'ROLENAME', width: 150
        }];

    define.screenParam.dataDef = [];
    define.service = "XtglService";
    define.method = "GetRoleElement";
    define.methodList = "GetRole";
    define.Key = "ROLEID";
    define.screenParam.MENU = [];
    if (!define.screenParam.FEESUBJECT) {
        define.screenParam.FEESUBJECT = [{
            CHECKED: "false",
            TRIMID: "",
            NAME: "",
        }]
    }
    define.screenParam.colDefFeeSubject = [
        {
            type: 'selection',
            width: 60,
            align: 'center',
            key: 'CHECKED',
            _checked: define.screenParam.FEESUBJECT[0]["CHECKED"]
        },
        {
            title: "收费项目代码",
            key: 'TRIMID', width: 150
        },
        {
            title: '收费项目名称',
            key: 'NAME', width: 250
        }];
    //define.dataParam.FeeSubject = [];

    define.dataParam.value = "1";
    define.dataParam.MENU = [];
    define.dataParam.FEESUBJECT = [];

}

define.newRecord = function () {
    define.dataParam.VOID_FLAG = "2";
}

define.showone = function (key, callback) {
    if (key) {
        var v = {};
        v["ROLEID"] = key;
        _.Ajax('SearchRole', {
            Data: v
        }, function (data) {
            $.extend(define.screenParam, data.role);
            define.screenParam.MENU = data.menu.Obj;
            define.screenParam.FEESUBJECT = data.sfxm;
            for (var i = 0; i < define.screenParam.FEESUBJECT.length; i++) {
                if (define.screenParam.FEESUBJECT[i].DISABLED == 0) {
                    define.screenParam.FEESUBJECT[i].DISABLED = true;
                }
                else {
                    define.screenParam.FEESUBJECT[i].DISABLED = false;
                }
                if (define.screenParam.FEESUBJECT[i].CHECKED == 0) {
                    define.screenParam.FEESUBJECT[i].CHECKED = false;
                }
                else {
                    define.screenParam.FEESUBJECT[i].CHECKED = true;
                }
            }
            callback && callback();
        });
    }
}

define.IsValidSave = function () {
    define.dataParam.FEESUBJECT = this.$refs.selectData.getSelection();
}
