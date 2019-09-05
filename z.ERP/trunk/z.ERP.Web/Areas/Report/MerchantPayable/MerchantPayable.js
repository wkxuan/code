var cols = [
    { title: '商户代码', key: 'MERCHANTID', minWidth: 100,sortable:true},
    { title: '商户名称', key: 'MERCHANTNAME', minWidth: 100, sortable: true },
    { title: '债权发生月', key: 'NIANYUE', minWidth: 100, sortable: true },
    { title: '收付实现月', key: 'YEARMONTH', minWidth: 100, sortable: true },

];
srch.mountedInit = function () {
    _.Ajax('GetSfxmList', {
        Data: {}
    }, function (data) {
        let list = $.map(data.res, (item, index) => {
            return {
                value: item.TRIMID,
                label: item.NAME
            };
        });
        Vue.set(srch.screenParam, "sfxmList", list);
    });
}
srch.beforeVue = function () {
    srch.service = "ReportService";
    srch.method = "MerchantPayable";
    srch.screenParam.colDef = cols;
    srch.screenParam.showPopMerchant = false;
    srch.screenParam.srcPopMerchant = __BaseUrl + "/Pop/Pop/PopMerchantList/";
};
srch.newCondition = function () {
    srch.searchParam.BRANCHID = "";
    srch.searchParam.MERCHANTNAME = "";
    srch.searchParam.SFXMLX = "";
    srch.searchParam.NIANYUE = "";
    srch.searchParam.SFXM = "";
    srch.searchParam.YEARMONTH = "";
};
srch.afterResult = function (data) {
    if (data.length) {
        let obj = data[0];
        let sfxmList = [];
        for (let item in obj) {
            if (item.indexOf("MUST_MONEY") > -1) {
                let arr = item.split("MUST_MONEY");
                let lx = srch.screenParam.sfxmList.filter(item=> {
                    if (item.value == arr[1]) {
                        return true;
                    }
                });
                if (lx.length) {
                    let loc = {
                        title: lx[0].label,
                        align: 'center',
                        children: [
                            {
                                title: '应交',
                                key: 'MUST_MONEY' + lx[0].value,
                                minWidth: 100,
                                ellipsis: true,
                                tooltip: true
                            }, {
                                title: '已付',
                                key: 'RECEIVE_MONEY' + lx[0].value,
                                minWidth: 100,
                                ellipsis: true,
                                tooltip: true
                            }
                        ]
                    };
                    sfxmList.push(loc);
                }
            }
        }
        let sumloc = {
            title: "合计",
            align: 'center',
            children: [
                {
                    title: '应交',
                    key: 'MUST_MONEYSUM',
                    minWidth: 100,
                    ellipsis: true,
                    tooltip: true
                }, {
                    title: '已付',
                    key: 'RECEIVE_MONEYSUM',
                    minWidth: 100,
                    ellipsis: true,
                    tooltip: true
                }
            ]
        };
        sfxmList.push(sumloc);
        srch.screenParam.colDef = cols.concat(sfxmList);
    }
}
srch.otherMethods = {
    //商户弹窗click
    SelMerchant: function () {
        srch.screenParam.showPopMerchant = true;
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
};