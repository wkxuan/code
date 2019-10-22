search.beforeVue = function () {
    search.service = "DpglService";
    search.method = "SearchFloor";
    search.indexShow = true;
    search.selectionShow = false;
    search.popConfig = {
        title: "楼层信息",
        src: "",
        width: 600,
        height: 260,
        open: false
    };
    search.screenParam.colDef = [
       { title: "楼层代码", key: 'CODE' },
       { title: '楼层名称', key: 'NAME' },
       { title: '门店', key: 'BRANCHNAME' },
       { title: '区域', key: 'REGIONNAME' },
       { title: '管理机构', key: 'ORGNAME' },
       { title: '建筑面积', key: 'AREA_BUILD' },
       { title: '租用面积', key: 'AREA_RENTABLE' },
       { title: '使用面积', key: 'AREA_USABLE' },      
       { title: '使用标记', key: 'STATUSMC' },
       {
           title: '操作', key: 'operate', onClick: function (index, row, data) {
               search.popConfig.src = __BaseUrl + "/XTGL/FLOOR/FloorDetail/" + row.ID;
               search.popConfig.open = true;
           }
       }];

    search.screenParam.branchData = [];
    search.screenParam.regionData = [];
}
search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.REGIONID = "";
    search.searchParam.CODE = "";
    search.searchParam.NAME = "";
    search.searchParam.AREA_BUILD_S = "";
    search.searchParam.AREA_BUILD_E = "";
};
search.mountedInit = function () {
    search.otherMethods.initBranch();

    search.btnConfig = [{
        id: "search",
        authority: ""
    }, {
        id: "clear",
        authority: ""
    }, {
        id: "add",
        authority: ""
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: ""
    }];
};

search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/XTGL/FLOOR/FloorDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};

search.otherMethods = {
    branchChange: function () {
        search.otherMethods.initRegion();
        search.searchParam.REGIONID = undefined;
    },
    initBranch: function () {
        _.Ajax('GetBranch', {
            Data: { ID: "" }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                search.screenParam.branchData = [];
                for (var i = 0; i < dt.length; i++) {
                    search.screenParam.branchData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
    initRegion: function () {
        _.Ajax('GetRegion', {
            Data: { BRANCHID: search.searchParam.BRANCHID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                search.screenParam.regionData = [];
                for (var i = 0; i < dt.length; i++) {
                    search.screenParam.regionData.push({ value: dt[i].REGIONID, label: dt[i].NAME })
                }
            }
        });
    }
}