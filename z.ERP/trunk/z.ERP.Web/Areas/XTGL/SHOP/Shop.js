search.beforeVue = function () {
    search.service = "DpglService";
    search.method = "SearchShop";
    search.indexShow = true;
    search.selectionShow = true;

    search.popConfig = {
        title: "资产单元信息",
        src: "",
        width: 800,
        height: 350,
        open: false
    };

    search.screenParam.branchData = [];
    search.screenParam.regionData = [];
    search.screenParam.floorData = [];
    
    search.screenParam.colDef = [
        { title: "代码", key: 'CODE' },
        { title: '名称', key: 'NAME' },
        { title: '门店', key: 'BRANCHNAME' },
        { title: '区域', key: 'REGIONNAME' },
        { title: '楼层', key: 'FLOORNAME' },
        { title: '单元类型', key: 'TYPEMC' },
        { title: '管理机构', key: 'ORGNAME' },
        { title: '业态', key: 'CATEGORYNAME' },
        { title: '租用面积', key: 'AREA_RENTABLE' },
        { title: '使用面积', key: 'AREA_USABLE' },
        { title: '建筑面积', key: 'AREA_BUILD' },
        { title: '单元状态', key: 'STATUSMC' },
        { title: '租用状态', key: 'RENT_STATUSMC' },
        {
            title: '操作', key: 'operate', authority: "", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/XTGL/SHOP/ShopDetail/" + row.SHOPID;
                search.popConfig.open = true;
            }
        }];
};

search.newCondition = function () {
    search.searchParam.BRANCHID = "";
    search.searchParam.REGIONID = "";
    search.searchParam.FLOORID = "";
    search.searchParam.CODE = "";
    search.searchParam.NAME = "";
    search.searchParam.AREA_RENTABLE_S = "";
    search.searchParam.AREA_RENTABLE_E = "";
    search.searchParam.STATUS = "";
    search.searchParam.RENT_STATUS = "";
};

search.otherMethods = {
    branchChange: function () {
        search.otherMethods.initRegion();
        search.searchParam.REGIONID = undefined;
        search.searchParam.FLOORID = undefined;
    },
    regionChange: function () {
        search.otherMethods.initFloor();
        search.searchParam.FLOORID = undefined;
    },
    floorChange: function () { },
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
    },
    initFloor: function () {
        _.Ajax('GetFloor', {
            Data: { REGIONID: search.searchParam.REGIONID }
        }, function (data) {
            let dt = data.dt;
            if (dt && dt.length) {
                search.screenParam.floorData = [];
                for (var i = 0; i < dt.length; i++) {
                    search.screenParam.floorData.push({ value: dt[i].ID, label: dt[i].NAME })
                }
            }
        });
    },
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
        authority: "",
    }];
};

search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/XTGL/SHOP/ShopDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};