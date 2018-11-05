search.beforeVue = function () {
    var col = [
        { title: "编号", key: "VOUCHERID", width: 105, sortable: true },
        { title: "名称", key: "VOUCHERNAME", width: 150,sortable: true},
        //{ title: "类型", key: "VOUCHERTYPEMC", width: 80, sortable: true },
        { title: "状态", key: "STATUSMC", width: 80 },
        { title: "备注", key: "DISCRIPTION", width: 200 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        { title: "审核人", key: "VERIFY_NAME", width: 90 },
        { title: "审核时间", key: "VERIFY_TIME", width: 150, sortable: true }
    ];
    search.screenParam.colDef = col.concat(search.colOperate).concat(search.colMul);
    search.service = "CwglService";
    search.method = "GetVoucherList";
}

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 108001,
        title: '浏览凭证模板',
        url: "JSGL/VOUCHER/VoucherDetail/" + row.VOUCHERID
    });
}

search.addHref = function (row) {
    _.OpenPage({
        id: 108001,
        title: '新增凭证模板',
        url: "JSGL/VOUCHER/VoucherEdit/"
    });
}
search.modHref = function (row, index) {
    _.OpenPage({
        id: 108001,
        title: '编辑凭证模板',
        url: "JSGL/VOUCHER/VoucherEdit/" + row.VOUCHERID
    });
}

