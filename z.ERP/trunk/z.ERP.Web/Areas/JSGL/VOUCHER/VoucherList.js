search.beforeVue = function () {
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
search.addHref = function (row) {
    _.OpenPage({
        id: 108001,
        title: '凭证模板详情',
        url: "JSGL/VOUCHER/VoucherEdit/"
    });
}

