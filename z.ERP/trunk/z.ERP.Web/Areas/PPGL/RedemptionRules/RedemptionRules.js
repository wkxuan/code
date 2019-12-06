search.beforeVue = function () {
    search.service = "XtglService";
    search.method = "SearchRedemptionRules";
    search.indexShow = true;
    search.selectionShow = false;

    search.popConfig = {
        title: "积分抵现规则设置",
        src: "",
        width: 500,
        height: 350,
        open: false
    };

    search.screenParam.colDef = [
        { title: "代码", key: 'ID' },
        { title: '门店', key: 'BRANCHNAME' },
        { title: '开始日期', key: 'START_DATE', cellType: "date" },
        { title: '结束日期', key: 'END_DATE', cellType: "date" },
        { title: '积分', key: 'CENT' },
        { title: '金额', key: 'MONEY' },
        { title: '状态', key: 'STATUSMC' },
        {
            title: '操作', key: 'operate', authority: "", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/PPGL/RedemptionRules/RedemptionRulesDetail/" + row.ID;
                search.popConfig.open = true;
            }
        }];
};

search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.ID = "";
    search.searchParam.START_DATE_S= "";
    search.searchParam.START_DATE_E = "";
    search.searchParam.END_DATE_S = "";
    search.searchParam.END_DATE_E = "";
    search.searchParam.CENT_S = "";
    search.searchParam.CENT_E = "";
    search.searchParam.MONEY_S = "";
    search.searchParam.MONEY_E = "";
    search.searchParam.STATUS = "";
};

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }
    //, {
    //    id: "del",
    //    authority: "",
    //}
    ];
};

search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/PPGL/RedemptionRules/RedemptionRulesDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};