search.beforeVue = function () {
    var col = [
        { title: "单据编号", key: "ID", width: 100 },
        { title: "调整开始时间", key: "STARTTIME", width: 150 },
        { title: "调整结束时间", key: "ENDTIME", width: 150 },
        { title: "状态", key: "STATUSMC", width: 100 },
        { title: "分店编号", key: "BRANCHID", width: 100 },
        { title: "分店名称", key: "BRANCHNAME", width: 150 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150 },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150 }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "SpglService";
    search.method = "GetRateAdjustList";

}

search.otherMethods = {

}

//search.browseHref = function (row, index) {
//    _.OpenPage({
//        id: 105004,
//        title: '浏览扣率调整单',
//        url: "SPGL/AdjustDiscount/AdjustDiscountDetail/" + row.ADID
//    })
//}

search.modHref = function (row, index) {
    if (row.STATUS == 1) {
        _.OpenPage({
            id: 105004,
            title: '编辑扣率调整单',
            url: "SPGL/RATE_ADJUST/Rate_AdjustEdit/" + row.ID
        })
    } else {
        iview.Message.info('当前销售补录单已审核,不能编辑!');
        return;
    }
}

search.addHref = function (row) {
    _.OpenPage({
        id: 105004,
        title: '新增扣率调整单',
        url: "SPGL/RATE_ADJUST/Rate_AdjustEdit/"
    })
}


