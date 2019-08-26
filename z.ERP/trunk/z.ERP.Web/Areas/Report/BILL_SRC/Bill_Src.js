srch.beforeVue = function () {
    srch.searchParam.Bill_Src = "";
    var col = [
        { title: '门店', key: 'BRANCHNAME', width: 180 },
{ title: '商户', key: 'MERCHANTNAME', width: 120 },
{ title: '账单号', key: 'BILLID', width: 100 },
{ title: '收费项目名称', key: 'FEENAME', width: 120 },
 { title: '租约号', key: 'CONTRACTID', width: 100 },
{ title: '债权发生月', key: 'NIANYUE', width: 100 },
{ title: '收付实现月', key: 'YEARMONTH', width: 100 },
{ title: '合同类型', key: 'TYPEMC', width: 100 },
{ title: '合同状态', key: 'STATUSMC', width: 100 },
{ title: '核算单位名称', key: 'UNITNAME', width: 120 },
 { title: '应收金额', key: 'MUST_MONEY', width: 100 },
 { title: '已收金额', key: 'RECEIVE_MONEY', width: 100 },
 { title: '返回金额', key: 'RETURN_MONEY', width: 100 },
 { title: '描述', key: 'DESCRIPTION', width: 200 }
 
        
         
         
        

    ];

    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "Bill_Src";
};




