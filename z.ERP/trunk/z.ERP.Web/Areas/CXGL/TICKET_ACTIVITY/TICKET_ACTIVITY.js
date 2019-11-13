define.beforeVue = function () {
    define.screenParam.colDef = [
        ];
    define.screenParam.dataDef = [];
    define.screenParam.itemDef = [
        { title: '终端号', key: 'POSNO', width: 180 },
        { title: '小票号', key: 'DEALID', width: 180 },
        { title: '交易时间', key: 'SALETIME', width: 300 },
    ]
    define.service = "CxglService";
    define.method = "GetNULL";
    define.methodList = "GetNULL";
    define.splitVal = 0;
    define.disabled = false;
}
define.initDataParam = function () {
    define.dataParam.PROMOTIONID = "",
    define.dataParam.PROMOTIONNAME = "",
    define.screenParam.TICKETNO = "";
    define.screenParam.POSNO = "";
    define.screenParam.DEALID = "";
    define.screenParam.START_DATE = "";
    define.screenParam.END_DATE = "";
    define.dataParam.ITEM = [];
}

define.newRecord = function () {
    define.dataParam.PROMOTIONID = "",
    define.dataParam.PROMOTIONNAME = "",
    define.screenParam.TICKETNO = "";
    define.screenParam.POSNO = "";
    define.screenParam.DEALID = "";
    define.screenParam.START_DATE = "";
    define.screenParam.END_DATE = "";
    define.dataParam.ITEM = [];
}
define.popCallBack = function (data) {
    if (define.popConfig.open) {
        define.popConfig.open = false;
        for (let i = 0; i < data.sj.length; i++) {
            if (define.popConfig.title == "选择营销活动") {
                define.newRecord();
                define.dataParam.PROMOTIONID = data.sj[i].ID;
                define.dataParam.PROMOTIONNAME = data.sj[i].NAME;
                define.screenParam.START_DATE = data.sj[i].START_DATE;
                define.screenParam.END_DATE = data.sj[i].END_DATE;
            }            
        };
    }
};
define.otherMethods = {
    clearinput: function () {
        define.screenParam.POSNO = "";
        define.screenParam.DEALID = "";
        define.screenParam.TICKETNO = "";
    },
    srchPromotion: function () {
        define.screenParam.popParam = { STATUS: 2 };
        define.popConfig.src = __BaseUrl + "/Pop/Pop/PopPromotionList/";
        define.popConfig.title = "选择营销活动";
        define.popConfig.open = true;
    },
    GetInfo: function (promotionid, posno, dealid) {
        _.Ajax('GetSaleTicket', {
            PROMOTIONID: promotionid, POSNO: posno, DEALID: dealid
        }, function (data) {
            if (data.length > 0) {
                if (data[0].SALE_TIME < define.screenParam.END_DATE && data[0].SALE_TIME > define.screenParam.START_DATE) {
                    if (define.dataParam.ITEM.filter(function (item) { return (data[0].POSNO == item.POSNO && data[0].DEALID == item.DEALID) }).length == 0) {     //已添加数据 跳过
                        define.dataParam.ITEM.push({
                            POSNO: data[0].POSNO,
                            DEALID: data[0].DEALID,
                            SALETIME: data[0].SALE_TIME,
                            PROMOTIONID: define.dataParam.PROMOTIONID
                        })
                        define.otherMethods.clearinput();
                    } else {
                        define.otherMethods.clearinput();
                        iview.Message.warning({
                            content: '该小票已再列表内!',
                            duration: 3,
                            closable: true
                        });
                    }
                } else {
                    define.otherMethods.clearinput();
                    iview.Message.warning({
                        content: '该小票交易时间不在活动时间内!',
                        duration: 3,
                        closable: true
                    });
                }
            } else {
                define.otherMethods.clearinput();
                iview.Message.warning({
                    content: '未找到小票数据或该小票已参加过该活动!',
                    duration: 3,
                    closable: true
                });
            }
        });
    },
    srchbtn: function () {
        if (!define.dataParam.PROMOTIONID) {
            iview.Message.info('请先确认营销活动!');
            return;
        }
        if (!define.screenParam.POSNO) {
            iview.Message.info('请先确认终端号!');
            return;
        }
        if (!define.screenParam.DEALID) {
            iview.Message.info('请先确认交易小票号!');
            return;
        }
        define.otherMethods.GetInfo(define.dataParam.PROMOTIONID, define.screenParam.POSNO, define.screenParam.DEALID);
    },
    delbtn: function () {
        var selectton = this.$refs.refGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < define.dataParam.ITEM.length; j++) {
                    if (define.dataParam.ITEM[j].POSNO == selectton[i].POSNO && define.dataParam.ITEM[j].DEALID == selectton[i].DEALID) {
                        define.dataParam.ITEM.splice(j, 1);
                    }
                }
            }
        }
    },
    SMsearch: function () {
        if (!define.dataParam.PROMOTIONID) {
            iview.Message.info('请先确认营销活动!');
            define.screenParam.TICKETNO = "";
            return;
        }
        if (!define.screenParam.TICKETNO) {
            iview.Message.info('请重新扫码!');
            return;
        }
        var str = define.screenParam.TICKETNO.split("|");
        define.otherMethods.GetInfo(define.dataParam.PROMOTIONID, str[0], str[1]);
    }
}

define.IsValidSave = function () {
    if (!define.dataParam.PROMOTIONID) {
        iview.Message.info("请确认营销活动!");
        return false;
    };    
    if (!define.dataParam.ITEM) {
        iview.Message.info("请确认活动小票!");
        return false;
    };
    return true;
}
define.mountedInit = function () {
    define.btnConfig = [{
        id: "search",
        authority: "",
        enabled: function (disabled, data) {
            return false; 
        }
    }, {
        id: "add",
        authority: "",
        enabled: function (disabled, data) {
            return false;
        }
    }, {
        id: "edit",
        authority: "",
        enabled: function (disabled, data) {
        return false;     
        },
    }, {
        id: "save",
        authority: "",
        enabled: function (disabled, data) {
            return false;
        },
    }, {
        id: "saves",
        name: "执行",
        icon: "md-star",
        authority: "",
        enabled: function (disabled, data) {
            return true;
        },
        fun: function () {
            if (!define.IsValidSave())
                return;
            _.Ajax('Saves', {
                DefineSave: define.dataParam.ITEM
            }, function (data) {
                debugger
                if (data == "True") {
                    define.newRecord();
                    iview.Message.success("执行成功!");
                } else {
                    iview.Message.error("执行失败!");
                }
            });
        },
        isNewAdd: true
    }, {
        id: "abandon",
        name: "清空",
        authority: "",
        enabled: function (disabled, data) {
            return true;
        },
    }];
}