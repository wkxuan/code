srch.beforeVue = function () {
    srch.searchParam.PAYINFO = "";
    var col = [
        { title: '门店', key: 'BRANCHNAME', width: 150 },
        { title: '终端号', key: 'POSNO', width: 90 },
        { title: '交易号', key: 'DEALID', width: 90 },
        { title: '收款方式', key: 'NAME', width: 110 },
        { title: '支付金额', key: 'AMOUNT', width: 90 },
        { title: '银行', key: 'BANK', width: 100 },
        { title: '卡号', key: 'CARDNO', width: 150 },
        { title: '交易开始时间', key: 'OPERTIME', width: 150 },
        { title: '交易结束时间', key: 'OPERTIME', width: 150 },
         //{ title: '序号', key: 'INX', width: 70 },
       // { title: '支付方式编号', key: 'PAYID', width: 110 },  
         { title: '流水号', key: 'SERIALNO', width: 150 },
        { title: '参考号', key: 'REFNO', width: 250 }
        

    ];

    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "PAYINFO";
};




