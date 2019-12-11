﻿editDetail.beforeVue = function () {

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
        title: "品牌代码", key: 'BRANDID'
    },
    { title: '品牌名称', key: 'NAME'},
    { title: '业态代码', key: 'CATEGORYCODE' },
    { title: '业态名称', key: 'CATEGORYNAME' }
    ];
    editDetail.screenParam.colDefpay = [
    {
        title: "编号", key: 'PAYMENTID'
    },
    { title: '银行卡号', key: 'CARDNO', cellType: "input", cellDataType: "number", },
    { title: '银行名称', key: 'BANKNAME', cellType: "input", },
    { title: '开户人', key: 'HOLDERNAME', cellType: "input", },
    { title: '身份证号', key: 'IDCARD', cellType: "input", },
    ];
    editDetail.screenParam.showPopBrand = false;
    editDetail.screenParam.srcPopBrand = __BaseUrl + "/Pop/Pop/PopBrandList/";
    editDetail.dataParam.MERCHANT_BRAND = editDetail.dataParam.MERCHANT_BRAND || [];
    editDetail.dataParam.MERCHANT_PAYMENT = editDetail.dataParam.MERCHANT_PAYMENT || [];
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
    addColPay: function () {
        editDetail.dataParam.MERCHANT_PAYMENT.push({
            PAYMENTID: editDetail.dataParam.MERCHANT_PAYMENT.length + 1,
            CARDNO: "",
            BANKNAME: "",
            HOLDERNAME: "",
            IDCARD:""
        });
    },
    delColPay: function () {
        let selection = this.$refs.refGrouppay.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.MERCHANT_PAYMENT;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].PAYMENTID == selection[i].PAYMENTID) {
                        if (!editDetail.dataParam.MERCHANTID) {
                            temp.splice(j, 1);
                        }else{
                            _.Ajax('SearchCMP', {
                                MERCHANTID: editDetail.dataParam.MERCHANTID, PAYMENTID: selection[i].PAYMENTID
                            }, function (data) {
                                if (data.length > 0) {
                                    iview.Message.info("编号" + selection[i].PAYMENTID + "的付款信息,已被合同引用不能删除");
                                } else {
                                    temp.splice(j, 1);
                                }
                            });
                        }
                    }
                }
            };
        }
    }
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.merchant);
        editDetail.dataParam.BILLID = data.merchant.MERCHANTID;
        editDetail.dataParam.MERCHANT_BRAND = data.merchantBrand;
        editDetail.dataParam.MERCHANT_PAYMENT = data.payment;
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
    editDetail.dataParam.TYPE = null;
    editDetail.dataParam.IDCARD = null;
    editDetail.dataParam.LICENSE = null;
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
    editDetail.dataParam.MERCHANT_PAYMENT = [];
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

    if (!editDetail.dataParam.TYPE) {
        iview.Message.info("请选择商户类型!");
        return false;
    }

    if (editDetail.dataParam.TYPE == "1" && !editDetail.dataParam.LICENSE) {
        iview.Message.info("商户类型为公司时，营业执照号不能为空!");
        return false;
    }

    if (editDetail.dataParam.TYPE == "2" && !editDetail.dataParam.IDCARD) {
        iview.Message.info("商户类型为个人时，身份证号不能为空!");
        return false;
    }

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
    for (var i = 0; i < editDetail.dataParam.MERCHANT_PAYMENT.length; i++) {
        if (!editDetail.dataParam.MERCHANT_PAYMENT[i].CARDNO) {
            iview.Message.info("请确认第" + (i + 1) + "行银行卡号!");
            return false;
        };
        if (!editDetail.dataParam.MERCHANT_PAYMENT[i].BANKNAME) {
            iview.Message.info("请确认第" + (i + 1) + "行银行名称!");
            return false;
        };
        if (!editDetail.dataParam.MERCHANT_PAYMENT[i].HOLDERNAME) {
            iview.Message.info("请确认第" + (i + 1) + "行开户人!");
            return false;
        };
        if (!editDetail.dataParam.MERCHANT_PAYMENT[i].IDCARD) {
            iview.Message.info("请确认第" + (i +1)+ "行身份证号!");
            return false;
        };
    };
    //付款信息判断重复项
    if (searchdata()) {
        iview.Message.info("付款信息中有重复项！");
        return false;
    }
    
    return true;
}
function searchdata() {
    //付款信息判断重复项
    var find = false;
    let p = editDetail.dataParam.MERCHANT_PAYMENT;
    for (var i = 0; i < p.length; i++) {
        for (var j = i + 1; j < p.length; j++) {
            if (p[i].CARDNO == p[j].CARDNO && p[i].BANKNAME == p[j].BANKNAME && p[i].HOLDERNAME == p[j].HOLDERNAME && p[i].IDCARD == p[j].IDCARD) {
                find = true; break;
            }
        }
        if (find) break;
    }
    return find;
}