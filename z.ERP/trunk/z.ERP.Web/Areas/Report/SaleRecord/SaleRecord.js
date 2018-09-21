srch.beforeVue = function () {
    var col = [
        { title: '终端号', key: 'POSNO', width: 95 },
        { title: '交易号', key: 'DEALID', width: 95 },
        {
            title: '交易时间', key: 'SALE_TIME', width: 150,
            render: function (h, params) {
                return h('div',
                  new Date(this.row.SALE_TIME).Format('yyyy-MM-dd hh:mm:ss'));
            }
        },
        {
            title: '记账日期', key: 'ACCOUNT_DATE', width: 100,
            render: function (h, params) {
                return h('div',
                  new Date(this.row.ACCOUNT_DATE).Format('yyyy-MM-dd'));
            }
        },
        { title: '收款员编码', key: 'CASHIERCODE', width: 100 },
        { title: '收款员名称', key: 'CASHIERNAME', width: 110 },
        { title: '收款金额', key: 'SALE_AMOUNT', width: 100 },
        { title: '找零金额', key: 'CHANGE_AMOUNT', width: 100 },
        { title: '原终端号', key: 'POSNO_OLD', width: 100 },
        { title: '原交易号', key: 'DEALID_OLD', width: 100 },

    ];
    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "SaleRecord";

  /*  srch.screenParam.showPopSysuser = false;
    srch.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    srch.screenParam.popParam = {}; */
};
/*
srch.otherMethods = {
    SelSysuser: function () {
        srch.screenParam.showPopSysuser = true;
    }
}

search.popCallBack = function (data) {
    if (srch.screenParam.showPopSysuser) {
        srch.screenParam.showPopSysuser = false;
        srch.searchParam.CASHIERID = data.sj[i].USERID;
        srch.searchParam.CASHIERNAME = data.sj[i].USERNAME;
        srch.screenParam.popParam = { USER_TYPE: '1' };
    }
} */