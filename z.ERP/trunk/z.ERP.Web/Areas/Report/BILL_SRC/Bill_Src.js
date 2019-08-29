srch.beforeVue = function () {
    srch.searchParam.Bill_Src = "";
    var col = [
       
        { title: '门店', key: 'BRANCHNAME', width: 180 },
      //{ title: '商户编码', key: 'MERCHANTIDE', width: 120 },
{ title: '商户名称', key: 'MERCHANTNAME', width: 120 },
{ title: '账单号', key: 'BILLID', width: 100 },
{ title: '收费项目名称', key: 'FEENAME', width: 120 },
 { title: '租约号', key: 'CONTRACTID', width: 100 },
{ title: '权责发生月', key: 'NIANYUE', width: 100 },
{ title: '收付实现月', key: 'YEARMONTH', width: 100 },
{ title: '账单类型', key: 'TYPEMC', width: 100 },
{ title: '账单状态', key: 'STATUSMC', width: 100 },
{ title: '核算单位名称', key: 'UNITNAME', width: 120 },
 { title: '应收金额', key: 'MUST_MONEY', width: 100, align: "right" },
 { title: '已收金额', key: 'RECEIVE_MONEY', width: 100, align: "right" },
 { title: '返还金额', key: 'RETURN_MONEY', width: 100, align: "right" },
 { title: '描述', key: 'DESCRIPTION', width: 200 }
 
    ];

    srch.screenParam.colDef = col;
    srch.service = "ReportService";
    srch.method = "Bill_Src";
    //srch.echartResult = true;
    srch.screenParam.popParam = {};
    //srch.searchParam.SrchTYPE = 1;

    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
    srch.screenParam.showPopContract = false;
    srch.screenParam.srcPopContract = __BaseUrl + "/" + "Pop/Pop/PopContractList/";

};

srch.newCondition = function () {
    srch.searchParam.BRANCHID = ""; 
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.CONTRACTID = "";
    srch.searchParam.BILLID = "";
    srch.searchParam.TYPE = "";
    srch.searchParam.STATUS = "";
    srch.searchParam.TRIMID = "";
    srch.searchParam.NIANYUE_START = "";
    srch.searchParam.YEARMONTH_START = "";
};



srch.otherMethods = {
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
    },
    SelContract: function () {
        srch.screenParam.showPopContract = true;
    },
 
    

}

srch.popCallBack = function (data) {

    if (srch.screenParam.showPopMerchant) {
        srch.screenParam.showPopMerchant = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.MERCHANTID = data.sj[i].MERCHANTID;
            srch.searchParam.MERCHANTNAME = data.sj[i].NAME;
        }
    }

    if (srch.screenParam.showPopContract) {
        srch.screenParam.showPopContract = false;
        for (var i = 0; i < data.sj.length; i++) {
            srch.searchParam.CONTRACTID = data.sj[i].CONTRACTID;
        }
    }

};