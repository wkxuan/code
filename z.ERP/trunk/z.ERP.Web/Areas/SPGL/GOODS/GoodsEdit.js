editDetail.beforeVue = function () {
    editDetail.service = "SpglService";
    editDetail.method = "GetGoodsElement"
    editDetail.Key = 'GOODID';
    editDetail.branchid = false;

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

    editDetail.screenParam.ParentContract = {};
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneEdit', {
        Data: { GOODSID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.goods[0]);
        editDetail.dataParam.BILLID = data.goods[0].GOODSDM;
        editDetail.dataParam.GOODS_SHOP = data.goods_shop[0];
        callback && callback(data);
    });
}


editDetail.otherMethods = {
    changge: function () {
        _.Ajax('GetContract', {
            Data: { CONTRACTID: editDetail.dataParam.CONTRACTID }
        }, function (data) {
            if (data.contract.length>0)
            {
                editDetail.dataParam.MERCHANTID = data.contract[0].MERCHANTID;
                editDetail.dataParam.STYLE = data.contract[0].STYLE;
                editDetail.dataParam.STYLEMC = data.contract[0].STYLEMC;
                editDetail.dataParam.SHMC = data.contract[0].SHMC;
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
}