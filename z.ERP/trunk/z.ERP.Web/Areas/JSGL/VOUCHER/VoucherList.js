﻿search.beforeVue = function () {
    search.screenParam.colDef = [
        { title: "编号", key: "VOUCHERID", width: 105, sortable: true },
        { title: "名称", key: "VOUCHERNAME", width: 150,sortable: true},
        { title: "备注", key: "DISCRIPTION", width: 250 },
        { title: "登记人", key: "REPORTER_NAME", width: 90 },
        { title: "登记时间", key: "REPORTER_TIME", width: 150, sortable: true },
        {
            title: '操作', key: 'operate', onClick: function (index, row, data) {
                _.OpenPage({
                    id: 108001,
                    title: '凭证模板详情',
                    url: "JSGL/VOUCHER/VoucherEdit/" + row.VOUCHERID
                });
            }
        }
    ];
    search.service = "CwglService";
    search.method = "GetVoucherList";
}
search.newCondition = function () {
    search.searchParam.VOUCHERID = "";
    search.searchParam.VOUCHERNAME = "";
    search.searchParam.STATUS = "";
    search.searchParam.REPORTER = "";
    search.searchParam.REPORTER_TIME_START = "";
    search.searchParam.REPORTER_TIME_END = "";
}
search.addHref = function () {
    _.OpenPage({
        id: 108001,
        title: '添加凭证模板',
        url: "JSGL/VOUCHER/VoucherEdit/"
    });
}

