search.beforeVue = function () {
    search.service = "XtglService";
    search.method = "TicketInfo";
    search.indexShow = true;
    search.selectionShow = false;

    search.popConfig = {
        title: "交易小票信息设置",
        src: "",
        width: 800,
        height: 350,
        open: false
    };
    search.screenParam.colDef = [
        { title: '门店编号', key: 'BRANCHID' },      
        { title: '门店名称', key: 'NAME' },
        { title: '打印次数', key: 'PRINTCOUNT' },
        { title: '票头文字', key: 'HEAD' },
        { title: '票尾文字', key: 'TAIL' },
        { title: '二维码广告', key: 'ADQRCODE' },
        { title: '文字广告位', key: 'ADCONTENT' },
        {
            title: '操作', key: 'operate', authority: "104004", onClick: function (index, row, data) {
                search.popConfig.src = __BaseUrl + "/XTGL/TicketInfo/TicketInfoDetail/" + row.BRANCHID;
                search.popConfig.open = true;
            }
        }];
};

search.mountedInit = function () {
    search.btnConfig = [{
        id: "search",
        authority: "10102000"
    }, {
        id: "clear",
        authority: "10102000"
    }, {
        id: "add",
        authority: "10102001"
    }, {
        id: "del",
        enabled: function () {
            return false;
        },
        authority: "10102001"
    }];
};

search.addHref = function () {
    search.popConfig.src = __BaseUrl + "/XTGL/TicketInfo/TicketInfoDetail/";
    search.popConfig.open = true;
};

search.popCallBack = function (data) {
    if (search.popConfig.open) {
        search.popConfig.open = false;
        search.searchList();
    }
};