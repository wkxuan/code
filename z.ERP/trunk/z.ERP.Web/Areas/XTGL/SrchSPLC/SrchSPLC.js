search.beforeVue = function () {
    search.searchParam.MENUID = "";
    var col = [
        { title: '单号', key: 'BILLID', width: 95, sortable: true },
        { title: '菜单号', key: 'MENUIDMC', width: 100, sortable: true },
        { title: '状态', key: 'STATUSNAME', width: 80 },
        { title: '登记人', key: 'REPORTER_NAME', width: 90 },
        { title: '登记时间', key: 'REPORTER_TIME', width: 150, sortable: true },
        { title: '审核人', key: 'VERIFY_NAME', width: 90, },
        { title: '审核时间', key: 'VERIFY_TIME', width: 150, sortable: true },
        { title: '终止人', key: 'TERMINATE_NAME', width: 90, },
        { title: '终止时间', key: 'TERMINATE_TIME', width: 150, sortable: true },
        {
            title: '浏览', key: 'action', width: 70,
            align: 'center', fixed: 'right',
            render: function (h, params) {
                return h('div',
                [
                    h('Button',
                   {

                       props: { type: 'primary', size: 'small', disabled: false },
                       style: { marginRight: '5px' },

                       on: { click: function (event) { search.browseHref(params.row, params.index) } },

                   }, '浏览')
                ]
              );
            }
        }

    ];

    search.screenParam.colDef = col;
    search.service = "XtglService";
    search.method = "SrchSplc";


    search.screenParam.showPopSysuser = false;
    search.screenParam.srcPopSysuser = __BaseUrl + "/" + "Pop/Pop/PopSysuserList/";
    search.screenParam.popParam = {};
}

search.otherMethods = {
    SelSigner: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "SIGNER";
        search.screenParam.popParam = {};
    },

    SelVerify: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "VERIFY";
        search.screenParam.popParam = {};
    },
    SelTerm: function () {
        search.screenParam.showPopSysuser = true;
        btnFlag = "TERM";
        search.screenParam.popParam = {};
    },
}

//接收子页面返回值
search.popCallBack = function (data) {

    if (search.screenParam.showPopSysuser) {
        search.screenParam.showPopSysuser = false;
        for (var i = 0; i < data.sj.length; i++) {
            if (btnFlag == "SIGNER") {
                search.searchParam.SIGNER_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "TERM") {
                search.searchParam.TERMINATE_NAME = data.sj[i].USERNAME;
            }
            else if (btnFlag == "VERIFY") {
                search.searchParam.VERIFY_NAME = data.sj[i].USERNAME;
            }
        };
    }
};

search.browseHref = function (row, index) {
    _.OpenPage({
        id: 101018,
        title: '审批流程定义',
        url: "XTGL/SPLC/SPLC/" + row.BILLID
    })
};


search.addHref = function (row) {
    _.OpenPage({
        id: 101018,
        title: '审批流程定义',
        url: "XTGL/SPLC/SPLC/"
    });

}
