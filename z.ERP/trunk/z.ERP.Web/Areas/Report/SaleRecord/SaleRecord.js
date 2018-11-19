srch.beforeVue = function () {
    var col = [
        { title: '终端号', key: 'POSNO', width: 95 },
        { title: '交易号', key: 'DEALID', width: 95 },
        {
            title: '交易时间', key: 'SALE_TIME', width: 150,
        //    render: function (h, params) {
        //        return h('div',
        //         new Date(this.row.SALE_TIME).Format('yyyy-MM-dd hh:mm:ss'));
        //    }
        },
        {
            title: '记账日期', key: 'ACCOUNT_DATE', width: 100,
                render: function (h, params) {
                    return h('div',
                      this.row.ACCOUNT_DATE.substr(0,10));
                }
        //    render: function (h, params) {
        //        return h('div',
        //          new Date(this.row.ACCOUNT_DATE).Format('yyyy-MM-dd'));
        //    }
        },
        { title: '收款员编码', key: 'CASHIERCODE', width: 100 },
        { title: '收款员名称', key: 'CASHIERNAME', width: 110 },
        { title: '收款金额', key: 'SALE_AMOUNT', width: 120, align :"right" },
        { title: '找零金额', key: 'CHANGE_AMOUNT', width: 100, align: "right" },
        { title: '原终端号', key: 'POSNO_OLD', width: 100 },
        { title: '原交易号', key: 'DEALID_OLD', width: 100 },

    ];
    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "SaleRecord";

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/" + "Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopShop = false;
    srch.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    srch.screenParam.popParam = {};
};

srch.otherMethods = {
    SelMerchant: function(){
        srch.screenParam.showPopMerchant = true;
    },
    SelShop: function () {
        srch.screenParam.showPopShop = true;
        if (srch.searchParam.BRANCHID)
            srch.screenParam.popParam = { BRANCHID: srch.searchParam.BRANCHID };
    }
}

srch.popCallBack = function (data) {
    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }
    if (srch.screenParam.showPopShop) {
        srch.screenParam.showPopShop = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.SHOPID = data.sj[i].SHOPID;
            srch.searchParam.SHOPCODE = data.sj[i].SHOPCODE;
            srch.searchParam.SHOPNAME = data.sj[i].NAME;
        }
    }
} 