editDetail.beforeVue = function () {
    editDetail.service = "SpglService";
    editDetail.method = "GetGoodsElement"
    editDetail.Key = 'GOODID';
    editDetail.branchid = false;

    editDetail.dataParam.TYPE = 1;
    
    editDetail.screenParam.colDef = [
        { title: "商铺代码", key: "CODE", width: 100,       },
        { title: '业态代码', key: 'CATEGORYCODE', width: 100 },
        { title: '业态名称', key: 'CATEGORYNAME', width: 100 },        

    ]

    if (!editDetail.dataParam.GOODS_SHOP) {
        editDetail.dataParam.GOODS_SHOP = [{
            GOODSID: ""
        }]
    };

    //KINDID
    editDetail.screenParam.Kind = [];

    editDetail.screenParam.ParentContract = {};
    editDetail.screenParam.ParentBrand = {};
    
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneEdit', {
        Data: { GOODSID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.goods[0]);
        editDetail.dataParam.BILLID = data.goods[0].GOODSDM;
        editDetail.dataParam.KINDID = data.goods[0].KINDID;
        editDetail.dataParam.GOODS_SHOP = data.goods_shop[0];

        var arr = data.goods[0].ALLID.split(",") || [];
        console.log(arr);
        //editDetail.screenParam.Kind = arr;
        Vue.set(editDetail.screenParam, "Kind", arr);
        console.log(editDetail.screenParam.Kind);

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
            }
            else
            {
                iview.Message.info("输入的租约号不存在!");
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }

        })

    },

    //点击合同弹窗
    Contract: function () {
        Vue.set(editDetail.screenParam, "PopContract", true);
    },
    //合同弹窗返回
    ContractBack: function (val) {
        Vue.set(editDetail.screenParam, "PopContract", false);
        editDetail.dataParam.CONTRACTID = val.sj[0].CONTRACTID;
        _.Ajax('GetContract', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length > 0) {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;
                editDetail.dataParam.STYLE = data.contract[0].STYLE;
                editDetail.dataParam.STYLEMC = data.contract[0].STYLEMC;
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
                editDetail.dataParam.GOODS_SHOP = data.shop;
            }
            else {
                editDetail.dataParam.MERCHANTID = null;
                editDetail.dataParam.SHMC = null;
            }

        })
    },
    //点击品牌
    Brand: function () {
        Vue.set(editDetail.screenParam, "PopBrand", true);
    },
    //品牌弹窗返回
    BrandBack: function (val) {
        Vue.set(editDetail.screenParam, "PopBrand", false);
        editDetail.dataParam.BRANDID = val.sj[0].BRANDID;
        editDetail.dataParam.BRANDMC = val.sj[0].NAME;
    },
    Getpym: function () {
        editDetail.dataParam.PYM = editDetail.dataParam.NAME.toPYM();
    },
    changeKind: function (value, selectedData) {
        editDetail.dataParam.KINDID = value[value.length - 1];
    },
}



editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.TYPE) {
        iview.Message.info("请确认类型!");
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
    if (!editDetail.dataParam.JXSL) {
        iview.Message.info("请确认进项税率!");
        return false;
    };
    if (!editDetail.dataParam.XXSL) {
        iview.Message.info("请确认销项税率!");
        return false;
    };
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
    return true;
}
