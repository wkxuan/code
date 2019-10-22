define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: "项目代码",
            key: 'TERMID', width: 150
        },
        {
            title: '费用项目名称',
            key: 'TERMNAME'
        },
        {
            title: '门店',
            key: 'BRANCHNAME'
        }];

    define.service = "XtglService";
    define.method = "GetFEESUBJECT_ACCOUNT";
    define.methodList = "GetFEESUBJECT_ACCOUNT";
    define.screenParam.showPopFeeSubject = false;
    define.screenParam.srcPopFeeSubject = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
    define.Key = 'TERMID';


};
define.initDataParam = function () {
    define.dataParam.TERMID = null;
    define.dataParam.TERMNAME = null;
    define.dataParam.FEE_ACCOUNTID = null;
    define.dataParam.NOTICE_CREATE_WAY = null;
}
define.mountedInit = function () {
    _.Ajax('GetBranch', {
        Data: { ID: "" }
    }, function (data) {
        if (data.dt) {
            define.screenParam.branchData = [];
            for (var i = 0; i < data.dt.length; i++) {
                define.screenParam.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
            }
            define.searchParam.BRANCHID = data.dt[0].ID;
            Vue.set(define.dataParam, "BRANCHID", define.searchParam.BRANCHID);
            define.otherMethods.FEE_ACCOUNTDATA(define.searchParam.BRANCHID);
            define.showlist();
        }
    });
}

define.showOne = function (data, callback) {
    _.Ajax('GetFEESUBJECT_ACCOUNTOne', {
        Data: { TERMID: data, BRANCHID: define.searchParam.BRANCHID }
    }, function (data) {
        $.extend(define.dataParam, data[0]);
        define.dataParam.FEE_ACCOUNTID = data[0].FEE_ACCOUNTID + "";
        callback && callback();
    });
}

define.otherMethods = {
    branchChange: function (value) {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        this.FEE_ACCOUNTDATA(define.searchParam.BRANCHID);
        define.initDataParam();
        define.showlist();
    },
    FEE_ACCOUNTDATA: function (value) {
        _.Ajax('GetFEE_ACCOUNTDATA', {
            Data: { BRANCHID: value }
        }, function (data) {
            define.screenParam.FEE_ACCOUNTData = [];
            for (var i = 0; i < data.length; i++) {
                define.screenParam.FEE_ACCOUNTData.push({ value: data[i].Key, label: data[i].Value })
            }
        });
    },
    //点击费用项目弹窗
    srchFeeSubject: function () {
        define.dataParam.BRANCHID = define.searchParam.BRANCHID;
        if (!define.dataParam.BRANCHID) {
            return iview.Message.info("请选择门店");
        }
        define.screenParam.popParam = { SqlCondition: " not exists(select 1 from FEESUBJECT_ACCOUNT  where FEESUBJECT_ACCOUNT.TERMID=FEESUBJECT.TRIMID AND FEESUBJECT_ACCOUNT.BRANCHID=" + define.dataParam.BRANCHID + ")" };
        define.popConfig.title = "选择费用项目";
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopFeeSubjectList/";
        define.popConfig.open = true;
    },
};


//define.newRecord = function () {
//    define.dataParam.BRANCHID = define.searchParam.BRANCHID;
//    define.dataParam.TERMID = null;
//    define.dataParam.TERMNAME = null;
//    define.dataParam.FEE_ACCOUNTID = null;
//    define.dataParam.NOTICE_CREATE_WAY = null;
//};

define.popCallBack = function (data) {
    define.popConfig.open = false;
    if (define.popConfig.title == "选择费用项目") {
        for (var i = 0; i < data.sj.length; i++) {
            define.dataParam.TERMID = data.sj[i].TRIMID;
            define.dataParam.TERMNAME = data.sj[i].NAME;
        };
    }
};

define.IsValidSave = function () {
    if (!define.dataParam.BRANCHID) {
        iview.Message.info("请选择门店!");
        return false;
    };
    if (!define.dataParam.TERMID) {
        iview.Message.info("请选费用项目!");
        return false;
    };
    if (!define.dataParam.NOTICE_CREATE_WAY) {
        iview.Message.info("请选通知单生成方式!");
        return false;
    };
    if (!define.dataParam.FEE_ACCOUNTID) {
        iview.Message.info("请选收费单位!");
        return false;
    };
    return true;
}