editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "ShglService";
    editDetail.method = "GetMerchant";
    editDetail.Key = 'MERCHANTID';
    editDetail.dataParam.STATUS = "1";

    editDetail.screenParam.ParentBrand = {};
    editDetail.screenParam.dataCas = [];

    editDetail.screenParam.Orgid = [];

    editDetail.screenParam.colDef = [
    { type: 'selection', width: 60, align: 'center' },
    {
        title: "品牌代码", key: 'BRANDID', width: 100,
    },
    { title: '品牌名称', key: 'NAME', width: 200 },
    { title: '业态代码', key: 'CATEGORYCODE', width: 200 },
    { title: '业态名称', key: 'CATEGORYNAME', width: 200 }
    ];
    editDetail.screenParam.showPopBrand = false;
    editDetail.screenParam.srcPopBrand = __BaseUrl + "/Pop/Pop/PopBrandList/";
    editDetail.dataParam.MERCHANT_BRAND = editDetail.dataParam.MERCHANT_BRAND || [];
};

editDetail.otherMethods = {
    delColPP: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.MERCHANT_BRAND.length; j++) {
                    if (editDetail.dataParam.MERCHANT_BRAND[j].BRANDID == selectton[i].BRANDID) {
                        editDetail.dataParam.MERCHANT_BRAND.splice(j, 1);
                    }
                }
            }
        }
    },
    srchColPP: function () {
        Vue.set(editDetail.screenParam, "showPopBrand", true);
    },
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.merchant);
        editDetail.dataParam.BILLID = data.merchant.MERCHANTID;
        editDetail.dataParam.MERCHANT_BRAND = data.merchantBrand;
        callback && callback(data);
    });
}

editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopBrand) {
        editDetail.screenParam.showPopBrand = false;
        let brand = editDetail.dataParam.MERCHANT_BRAND;
        for (let i = 0; i < data.sj.length; i++) {
            if (brand.filter(item=> { return (data.sj[i].BRANDID == item.BRANDID) }).length == 0) {
                brand.push(data.sj[i]);
            }
        };
    }
};

editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.SH = null;
    editDetail.dataParam.BANK_NAME = null;
    editDetail.dataParam.BANK = null;
    editDetail.dataParam.ADRESS = null;
    editDetail.dataParam.CONTACTPERSON = null;
    editDetail.dataParam.PHONE = null;
    editDetail.dataParam.PIZ = null;
    editDetail.dataParam.WEIXIN = null;
    editDetail.dataParam.QQ = null;
    editDetail.dataParam.MERCHANT_BRAND = [];
}

//按钮初始化
editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10200101"
    }, {
        id: "edit",
        authority: "10200101"
    }, {
        id: "del",
        authority: "10200101"
    }, {
        id: "save",
        authority: "10200101"
    }, {
        id: "abandon",
        authority: "10200101"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10200102",
        fun: function () {
            _.Ajax('ExecData', {
                Data: { MERCHANTID: editDetail.dataParam.MERCHANTID },
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

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请商户名称!");
        return false;
    };

    if (editDetail.dataParam.MERCHANT_BRAND.length == 0) {
        iview.Message.info("请维护品牌!");
        return false;
    };
    for (var i = 0; i < editDetail.dataParam.MERCHANT_BRAND.length; i++) {
        if (!editDetail.dataParam.MERCHANT_BRAND[i].BRANDID) {
            iview.Message.info("请确定品牌!");
            return false;
        };

        for (var j = i + 1; j < editDetail.dataParam.MERCHANT_BRAND.length; j++) {
            if (editDetail.dataParam.MERCHANT_BRAND[i].BRANDID == editDetail.dataParam.MERCHANT_BRAND[j].BRANDID) {
                iview.Message.info("品牌" + editDetail.dataParam.MERCHANT_BRAND[i].NAME + "重复!");
                return;
            }
        }
    };

    return true;
}