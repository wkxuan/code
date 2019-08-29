editDetail.beforeVue = function () {
    //初始化弹窗所要传递参数
    editDetail.screenParam.showPopSyyuser = false;
    editDetail.screenParam.srcPopSyyuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    editDetail.screenParam.showPopGoods = false;
    editDetail.screenParam.srcPopGoods = __BaseUrl + "/" + "Pop/Pop/PopGoodsShopList/";
    editDetail.screenParam.popParam = {};
    editDetail.screenParam.colDef = [];
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneSaleBillEdit', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.saleBill[0]);
        editDetail.dataParam.SALEBILLITEM = data.saleBillItem[0];
        callback && callback(data);
    });
}
///html中绑定方法
editDetail.otherMethods = {
    SelSyysuser: function () {
        editDetail.screenParam.showPopSyyuser = true;
        editDetail.screenParam.popParam = { USER_TYPE: "1" };
        btnFlag = "SYYFlag";
    },
    SelYyysuser: function () {
        editDetail.screenParam.showPopSyyuser = true;
        editDetail.screenParam.popParam = { USER_TYPE: "1,2" };
        btnFlag = "YYYFlag";
    },
    srchColGoods: function () {
        if (!editDetail.dataParam.CLERKID) {
            iview.Message.info("营业员不能为空!");
            return;
        }
        editDetail.screenParam.showPopGoods = true;
        editDetail.screenParam.popParam = { YYY: editDetail.dataParam.CLERKID, STATUS: "2" };
    },
    delColGoods: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的商品!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.SALEBILLITEM.length; j++) {
                    if (editDetail.dataParam.SALEBILLITEM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.SALEBILLITEM.splice(j, 1);
                    }
                }
            }
        }
    },
    initItem: function () {
        _.Ajax('GetPay', {
            1: 1
        }, function (data) {
            let payList = $.map(data, item => {
                return {
                    label: item.NAME,
                    value: item.PAYID
                }
            });
            editDetail.screenParam.colDef = [
                { title: "商品代码", key: "GOODSDM" },
                { title: '商品名称', key: 'NAME' },
                { title: '商铺代码', key: 'CODE' },
                { title: '收款方式', key: 'PAYID', cellType: "select", enableCellEdit: true, selectList: payList },
                { title: '收款金额', key: 'AMOUNT', cellType: "input", cellDataType: "number" }
            ]
        });
    }
}
//数据初始化
editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.POSNO = null;
    editDetail.dataParam.ACCOUNT_DATE = null;
    editDetail.dataParam.STATUS = null;
    editDetail.dataParam.CASHIERID = null;
    editDetail.dataParam.CLERKID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.CASHIERID = null;
    editDetail.dataParam.SYYMC = null;
    editDetail.dataParam.CLERKID = null;
    editDetail.dataParam.YYYMC = null;
    editDetail.dataParam.SALEBILLITEM = [];
}
//按钮初始化
editDetail.mountedInit = function () {
    editDetail.otherMethods.initItem();
    editDetail.btnConfig = [{
        id: "add",
        authority: "10500401"
    }, {
        id: "edit",
        authority: "10500401"
    }, {
        id: "del",
        authority: "10500401"
    }, {
        id: "save",
        authority: "10500401"
    }, {
        id: "abandon",
        authority: "10500401"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10500402",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { BILLID: editDetail.dataParam.BILLID },
            }, function (data) {
                iview.Message.info("审核成功");
                setTimeout(function () {
                    window.location.reload();
                }, 100);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS < 2) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};
///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopSyyuser) {
        editDetail.screenParam.showPopSyyuser = false;
        if (btnFlag == "SYYFlag") {
            editDetail.dataParam.CASHIERID = data.sj[0].USERID;
            editDetail.dataParam.SYYMC = data.sj[0].USERNAME;
        }
        else if (btnFlag == "YYYFlag") {
            editDetail.dataParam.CLERKID = data.sj[0].USERID;
            editDetail.dataParam.YYYMC = data.sj[0].USERNAME;
        }
    }
    if (editDetail.screenParam.showPopGoods) {
        editDetail.screenParam.showPopGoods = false;
        let itemData = editDetail.dataParam.SALEBILLITEM;
        for (let i = 0; i < data.sj.length; i++) {
            if (itemData.filter(function (item) { return (data.sj[i].GOODSID == item.GOODSID) }).length == 0) {
                itemData.push({
                    GOODSID: data.sj[i].GOODSID,
                    GOODSDM: data.sj[i].GOODSDM,
                    NAME: data.sj[i].NAME,
                    CODE: data.sj[i].CODE,
                    SHOPID: data.sj[i].SHOPID,
                    QUANTITY: 1,
                    PAYID: null,
                    AMOUNT: 0
                });
            }
        };
    }
}

editDetail.IsValidSave = function () {

    editDetail.dataParam.POSNO = ("000000" + editDetail.dataParam.BRANCHID + '0999').substr(-6);
    if (!editDetail.dataParam.ACCOUNT_DATE) {
        iview.Message.info("请确认记账日期!");
        return false;
    };

    if (!editDetail.dataParam.CASHIERID) {
        iview.Message.info("请确认收银员!");
        return false;
    };
    if (!editDetail.dataParam.CLERKID) {
        iview.Message.info("请确认营业员!");
        return false;
    };
    let itemData = editDetail.dataParam.SALEBILLITEM;

    if (!itemData.length) {
        iview.Message.info("请确认销售明细!");
        return false;
    }
    for (let i = 0; i < itemData.length; i++) {
        if (!itemData[i].PAYID) {
            iview.Message.info(`请确认商品“${itemData[i].NAME}”的收款方式!`);
            return false;
        }
    }

    return true;
}