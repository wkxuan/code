defineNew.beforeVue = function () {
    defineNew.service = "XtglService";
    defineNew.method = "TicketInfo";
    defineNew.screenParam.defineDetailSrc = null;
    defineNew.screenParam.showDefineDetail = false;
    defineNew.screenParam.title = "交易小票信息设置";
    defineNew.screenParam.branchData = [];
    defineNew.key = 'BRANCHID';

    defineNew.columnsDef = [
                { title: '门店编号', key: 'BRANCHID' },      
            { title: '门店名称', key: 'NAME' },
            { title: '打印次数', key: 'PRINTCOUNT' },
            { title: '票头文字', key: 'HEAD' },
            { title: '票尾文字', key: 'TAIL' },
            { title: '二维码广告', key: 'ADQRCODE' },
            { title: '文字广告位', key: 'ADCONTENT' },

        {
            title: '操作', key: 'operate', authority: "104004", onClick: function (index, row, data) {
                defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/TicketInfo/TicketInfoDetail/" + row.BRANCHID;
                defineNew.screenParam.showDefineDetail = true;
            }
        }];
};


defineNew.mountedInit = function () {
  

    defineNew.btnConfig = [{
        id: "select",
        authority: "104004"
    }, {
        id: "clear",
        authority: "104004"
    }, {
        id: "add",
        authority: "104004"
    }, {
        id: "del",
        authority: "104004"
    }];
};

defineNew.add = function () {
    defineNew.screenParam.defineDetailSrc = __BaseUrl + "/XTGL/TicketInfo/TicketInfoDetail/";
    defineNew.screenParam.showDefineDetail = true;
};

defineNew.popCallBack = function (data) {
    if (defineNew.screenParam.showDefineDetail) {
        defineNew.screenParam.showDefineDetail = false;
        defineNew.searchList();
    }
};