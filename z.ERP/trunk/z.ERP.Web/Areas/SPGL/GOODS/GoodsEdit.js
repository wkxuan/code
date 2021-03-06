﻿editDetail.beforeVue = function () {
    editDetail.service = "SpglService";
    editDetail.method = "GetGoodsElement"
    editDetail.Key = 'GOODID';
    editDetail.branchid = false;

    editDetail.dataParam.TYPE = 1;
    
    editDetail.screenParam.colDef = [
        { title: "商铺代码", key: "CODE", width: 150,       },
        { title: '业态代码', key: 'CATEGORYCODE', width: 100 },
        { title: '业态名称', key: 'CATEGORYNAME', width: 100 },        

    ]
    editDetail.screenParam.colDefGroup = [
        { title: "时间段", key: "INX", width: 90, },
        { title: '开始日期', key: 'STARTDATE', width: 100 },
        { title: '结束日期', key: 'ENDDATE', width: 100 },
        { title: '销售金额起', key: 'SALES_START', width: 100 },
        { title: '销售金额止', key: 'SALES_END', width: 100 },
        { title: '结算扣率', key: 'JSKL', width: 100 },

    ]

    if (!editDetail.dataParam.GOODS_SHOP) {
        editDetail.dataParam.GOODS_SHOP = [{
            GOODSID: ""
        }]
    };
    if (!editDetail.dataParam.GOODS_GROUP) {
        editDetail.dataParam.GOODS_GROUP = [{
            GOODSID: ""
        }]
    };
    //KINDID
   // editDetail.screenParam.Kind = [];
    editDetail.dataParam.ORGIDCASCADER = null;
    editDetail.screenParam.showPopJsklGroup = false;
    editDetail.screenParam.srcPopJsklGroup = __BaseUrl + "/" + "Pop/Pop/PopJsklGroupList/";
    editDetail.screenParam.showPopContract = false;
    editDetail.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";
    editDetail.screenParam.showPopBrand = false;
    editDetail.screenParam.srcPopBrand = __BaseUrl + "/" + "Pop/Pop/PopBrandList/";
    
    //editDetail.screenParam.ParentContract = {};
    //editDetail.screenParam.ParentBrand = {};
    //editDetail.screenParam.ParentJsklGroup = {};
    
}

editDetail.clearKey = function () {
    editDetail.dataParam.GOODSID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.GOODSDM = null;
    editDetail.dataParam.BARCODE = null;
    editDetail.dataParam.TYPE = null;
    editDetail.dataParam.PYM = null;
    editDetail.dataParam.STYLE = null;
    editDetail.dataParam.STYLEMC = null;
    editDetail.dataParam.CONTRACTID = null;
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.JXSL = null;
    editDetail.dataParam.SHMC = null;
    editDetail.dataParam.BRANDID = null;
    editDetail.dataParam.BRANDMC = null;
    editDetail.dataParam.PKIND_ID = null;
    editDetail.dataParam.XXSL = null;
    editDetail.dataParam.JSKL_GROUP = null;
    editDetail.dataParam.PRICE = null;
    editDetail.dataParam.MEMBER_PRICE = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.REGION = null;
    editDetail.dataParam.GOODS_GROUP = [];
    editDetail.dataParam.GOODS_SHOP = [];
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneEdit', {
        Data: { GOODSID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.goods[0]);
        editDetail.dataParam.BILLID = data.goods[0].GOODSDM;
        editDetail.dataParam.KINDID = data.goods[0].KINDID;
        editDetail.dataParam.GOODS_SHOP = data.goods_shop[0];
        editDetail.dataParam.GOODS_GROUP = data.goods_group[0];
        editDetail.dataParam.PKIND_ID = editDetail.dataParam.PKIND_ID.split(",");
       // var arr = data.goods[0].CODE.split(",") || [];
       // console.log(arr);
        //editDetail.screenParam.Kind = arr;
      //  Vue.set(editDetail.screenParam, "Kind", arr);
      //  console.log(editDetail.screenParam.Kind);

        callback && callback(data);
    });
};

editDetail.mountedInit = function () {
    _.Ajax('SearchInit', {
        Data: {}
    }, function (data) {
        Vue.set(editDetail.screenParam, "dataKind", data.treeorg.Obj);
    });
}


editDetail.otherMethods = {
    htChange: function () {
        _.Ajax('GetContract', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length>0)
            {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;
                editDetail.dataParam.STYLE = data.contract[0].STYLE;
                editDetail.dataParam.STYLEMC = data.contract[0].STYLEMC;
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
                editDetail.dataParam.JXSL = data.contract[0].JXSL;
                editDetail.dataParam.XXSL = data.contract[0].XXSL;
                editDetail.dataParam.GOODS_SHOP = data.shop;
                if (data.jsklGroup) {
                    editDetail.dataParam.JSKL_GROUP = data.jsklGroup[0].GROUPNO;
                    editDetail.dataParam.GOODS_GROUP = data.jsklGroup;
                }
            }
            else
            {
                iview.Message.info("输入的租约号不存在!");
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }

        })

    },    
    SelJsklGroup: function () {
        if (!editDetail.dataParam.CONTRACTID) {
            iview.Message.info("请先选择租约!");
            return;
        };
        editDetail.screenParam.showPopJsklGroup = true;
        editDetail.screenParam.popParam = { CONTRACTID: editDetail.dataParam.CONTRACTID };
    },
    SelContract: function () {
        editDetail.screenParam.showPopContract = true;
    },
    SelBrand: function () {
        if (!editDetail.dataParam.CONTRACTID) {
            iview.Message.info("请先选择租约!");
            return;
        };

        editDetail.screenParam.showPopBrand = true;
        editDetail.screenParam.popParam = { CONTRACTID: editDetail.dataParam.CONTRACTID };
    },
    Getpym: function () {
        editDetail.dataParam.PYM = editDetail.dataParam.NAME.toPYM().substr(0,6);
    },
    changeKind: function (value, selectedData) {
        editDetail.dataParam.KINDID = value[value.length - 1];
    },
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {
    if (editDetail.screenParam.showPopJsklGroup) {
        editDetail.screenParam.showPopJsklGroup = false;
        editDetail.dataParam.JSKL_GROUP = data.sj[0].GROUPNO;
    }
    else if (editDetail.screenParam.showPopContract)
    {
        editDetail.dataParam.CONTRACTID = data.sj[0].CONTRACTID;
        editDetail.screenParam.showPopContract = false;
        _.Ajax('GetContract', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length > 0) {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;
                editDetail.dataParam.STYLE = data.contract[0].STYLE;
                editDetail.dataParam.STYLEMC = data.contract[0].STYLEMC;
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
                editDetail.dataParam.JXSL = data.contract[0].JXSL;
                editDetail.dataParam.XXSL = data.contract[0].XXSL;
                editDetail.dataParam.GOODS_SHOP = data.shop;
                if (data.jsklGroup)
                {
                    editDetail.dataParam.JSKL_GROUP = data.jsklGroup[0].GROUPNO;
                    editDetail.dataParam.GOODS_GROUP = data.jsklGroup;
                }
                
            }
            else {
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }
        })
    }
    else if (editDetail.screenParam.showPopBrand)
    {
        editDetail.screenParam.showPopBrand = false;
        editDetail.dataParam.BRANDID = data.sj[0].BRANDID;
        editDetail.dataParam.BRANDMC = data.sj[0].NAME;
    }
}


editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.TYPE) {
        iview.Message.info("请确认类型!");
        return false;
    };

    if (!editDetail.dataParam.GOODSDM) {
        iview.Message.info("请输入商品代码!");
        return false;
    };

    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请确认商品名称!");
        return false;
    };
    
    if (!editDetail.dataParam.PYM) {
        iview.Message.info("请确认商品拼音码!");
        return false;
    };

    if (!editDetail.dataParam.CONTRACTID) {
        iview.Message.info("请确认租约!");
        return false;
    };
    if (!editDetail.dataParam.MERCHANTID) {
        iview.Message.info("请确认商户!");
        return false;
    };
    //if (!editDetail.dataParam.JXSL) {
    //    iview.Message.info("请确认进项税率!");
    //    return false;
    //};
    //if (!editDetail.dataParam.XXSL) {
    //    iview.Message.info("请确认销项税率!");
    //    return false;
    //};
    if (!editDetail.dataParam.BRANDID) {
        iview.Message.info("请确认品牌!");
        return false;
    };
    if (!editDetail.dataParam.KINDID) {
        iview.Message.info("请确认商品分类!");
        return false;
    };
    if (editDetail.dataParam.GOODS_SHOP.length == 0) {
        iview.Message.info("请确定商铺!");
        return false;
    }
    if (!editDetail.dataParam.JSKL_GROUP) {
        iview.Message.info("请确定扣率组!");
        return false;
    }
    return true;
}
