var cols = [
    { title: '商户代码', key: 'MERCHANTID', minWidth: 100,sortable:true},
    { title: '商户名称', key: 'MERCHANTNAME', minWidth: 100, sortable: true },
    { title: '债权发生月', key: 'NIANYUE', minWidth: 100, sortable: true },
    { title: '收付实现月', key: 'YEARMONTH', minWidth: 100, sortable: true },

];
search.mountedInit = function () {
    _.Ajax('GetSfxmList', {
        Data: {}
    }, function (data) {
        let list = $.map(data.res, (item, index) => {
            return {
                value: item.Key,
                label: item.Value
            };
        });
        Vue.set(search.screenParam, "sfxmList", list);
    });

    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "export",
        authority: ""
    }];
}
search.beforeVue = function () {
    search.service = "ReportService";
    search.method = "MerchantPayable";
    search.indexShow = true;
    search.selectionShow = false;
    search.popConfig = {
        title: "",
        src: "",
        width: 800,
        height: 550,
        open: false
    };

    search.screenParam.popParam = {};
    search.screenParam.colDef = cols;
};
search.newCondition = function () {
    search.searchParam.BRANCHID = [];
    search.searchParam.SFXMLX = [];
    search.searchParam.SFXM = [];
    search.searchParam.MERCHANTNAME = "";
    search.searchParam.NIANYUE_START = "";
    search.searchParam.NIANYUE_END = "";
    search.searchParam.YEARMONTH_START = "";
    search.searchParam.YEARMONTH_END = "";
};
search.searchDataAfter = function (data) {
    if (data.length) {
        let obj = data[0];
        let sfxmList = [];
        for (let item in obj) {
            if (item.indexOf("MUST_MONEY") > -1) {
                let arr = item.split("MUST_MONEY");
                let lx = search.screenParam.sfxmList.filter(item=> {
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
                                align: "right"
                            }, {
                                title: '已付',
                                key: 'RECEIVE_MONEY' + lx[0].value,
                                align: "right"
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
                    align: "right"
                }, {
                    title: '已付',
                    key: 'RECEIVE_MONEYSUM',
                    align: "right"
                }
            ]
        };
        sfxmList.push(sumloc);
        search.screenParam.colDef = cols.concat(sfxmList);
    }
}

search.otherMethods = {
    //商户弹窗click
    SelMerchant: function () {
        search.screenParam.popParam = {};
        search.popConfig.title = "选择商户";
        search.popConfig.src = __BaseUrl + "/Pop/Pop/PopMerchantList/";
        search.popConfig.open = true;
    }
}
search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        for (var i = 0; i < data.sj.length; i++) {
            switch (search.popConfig.title) {
                case "选择商户":
                    search.searchParam.MERCHANTNAME = data.sj[i].NAME;
                    break;
            }
        }
    }
};