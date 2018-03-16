define.beforeVue = function () {
    define.screenParam.colDef = [
        {
            title: '支付方式代码',
            key: 'PAYID', width: 150,
        },
        {
            title: '支付方式名称',
            key: 'NAME', width: 250,
        }];

    define.screenParam.dataDef = [];



    define.screenParam.ParentMerchant = {};

    define.service = "XtglService";
    define.method = "GetPayElement";
    define.methodList = "GetPay";
    define.Key = 'PAYID';



    define.screenParam.MerchantBack = function (val) {
        Vue.set(define.screenParam, "PopMerchant",false);
        console.log(val);
    };

    //点击打开弹窗
    define.screenParam.Merchant = function () {
        Vue.set(define.screenParam, "PopMerchant", true);
        define.screenParam.ParentMerchant = { A: '1', B: '2' };
    };
}

//define.otherMethods = {
//    MerchantBack: function (val) {
//        editDetail.screenParam.PopMerchant = false;
//        console.log(val);
//    },

//    //点击打开弹窗
//    Merchant: function () {
//        editDetail.screenParam.PopMerchant = true;
//        editDetail.screenParam.ParentMerchant = { A: '1', B: '2' };
//    },
//}



define.newRecord = function () {
    define.dataParam.VOID_FLAG = "1";
}


define.IsValidSave = function (param) {
    if (!define.dataParam.NAME) {
        param.$Message.info("名称不能为空!");
        return false;
    }
    if (!define.dataParam.TYPE) {
        param.$Message.info("类型不能为空!");
        return false;
    }

    if (!define.dataParam.FK) {
        param.$Message.info("返款标记不能为空!");
        return false;
    }
    if (!define.dataParam.JF) {
        param.$Message.info("积分标记不能为空!");
        return false;
    }

    if (!define.dataParam.ZLFS) {
        param.$Message.info("找零方式不能为空!");
        return false;
    }

    if (!define.dataParam.FLAG) {
        param.$Message.info("显示顺序不能为空!");
        return false;
    }
    return true;
}

