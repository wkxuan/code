editDetail.beforeVue = function () {
    //小票列表
    editDetail.screenParam.colDefticket = [
       { title: '款台号', key: 'POSNO', width: 100 },
       { title: '小票号', key: 'DEALID',width: 100 },
       { title: '消费金额', key: 'AMOUNT'},
       { title: '消费时间', key: 'SALE_TIME'},
    ];
    //赠品列表
    editDetail.screenParam.colDefpresent = [
       { title: '规则编码', key: 'FG_RULEID', width: 100 },
       { title: '满额', key: 'FULL', width: 100 },
       { title: '赠品编码', key: 'PRESENTID', width: 100 },
       { title: '赠品名称', key: 'PRESENTNAME' },
       { title: '价值', key: 'PRICE', cellType: "input", cellDataType: "number", width: 100 },
    ];
    //赠品发放明细
    editDetail.screenParam.colDefpresents = [
       { title: '赠品编码', key: 'PRESENTID', width: 100 },
       { title: '赠品名称', key: 'PRESENTNAME' },
       { title: '数量', key: 'COUNT', width: 100 },
    ];
    editDetail.dataParam.PRESENT_SEND_TICKET = [];
    editDetail.dataParam.PRESENT_SEND_ITEM = [];
    editDetail.screenParam.POSNO = "";
    editDetail.screenParam.DEALID = "";
    editDetail.screenParam.AMOUNTS = "";
    editDetail.screenParam.PRESENT = [];
}
editDetail.branchChange = function () {
    editDetail.dataParam.PRESENT_SEND_TICKET = [];
    editDetail.dataParam.PRESENT_SEND_ITEM = [];
    editDetail.screenParam.POSNO = "";
    editDetail.screenParam.DEALID = "";
    editDetail.screenParam.AMOUNTS = "";
    editDetail.screenParam.PRESENT = [];
};
editDetail.otherMethods = {
    addticket: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info('请先确认门店!');
            return;
        }
        if (!editDetail.screenParam.POSNO) {
            iview.Message.info('请先确认终端号!');
            return;
        }
        if (!editDetail.screenParam.DEALID) {
            iview.Message.info('请先确认交易小票号!');
            return;
        }
        _.Ajax('GetSaleTicket', {
            BRANCHID: editDetail.dataParam.BRANCHID, POSNO: editDetail.screenParam.POSNO, DEALID: editDetail.screenParam.DEALID
        }, function (data) {
            if (data.length > 0) { //判断是否又有效数据
                if (editDetail.dataParam.PRESENT_SEND_TICKET.filter(function (item) { return (data[0].POSNO == item.POSNO, data[0].DEALID == item.DEALID) }).length == 0) {     //已添加数据 跳过
                    editDetail.dataParam.PRESENT_SEND_TICKET.push({
                        POSNO: data[0].POSNO,
                        DEALID: data[0].DEALID,
                        AMOUNT: data[0].AMOUNT,
                        SALE_TIME: data[0].SALE_TIME,
                    })
                    iview.Message.success('交易数据添加成功!');
                } else {
                    iview.Message.warning('该交易数据已添加!');
                }
            } else {
                iview.Message.warning('暂无有效交易数据!');
            }
        });
    },
    delticket: function () {
        let selection = this.$refs.reftickets.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.PRESENT_SEND_TICKET;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].POSNO == selection[i].POSNO && temp[j].DEALID == selection[i].DEALID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            };
        }
    },
    SUMticketamount: function () {
        var sum = 0;
        for (let i = 0; i < editDetail.dataParam.PRESENT_SEND_TICKET.length; i++) {
            sum += parseFloat(editDetail.dataParam.PRESENT_SEND_TICKET[i].AMOUNT);
        }
        editDetail.screenParam.AMOUNTS = sum;
    },
    addpresentss: function () {

    },
    delpresentss: function () {

    },
};

editDetail.clearKey = function () {
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.PRESENT_SEND_TICKET = [];
    editDetail.dataParam.PRESENT_SEND_ITEM = [];
    editDetail.screenParam.POSNO = "";
    editDetail.screenParam.DEALID = "";
    editDetail.screenParam.AMOUNTS = "";
    editDetail.screenParam.PRESENT = [];
};

editDetail.newRecord = function () {
    editDetail.clearKey();
};

editDetail.IsValidSave = function () {
    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请确认门店!");
        return false;
    };
    if (!editDetail.dataParam.PRESENT_SEND_TICKET.length) {
        iview.Message.info("请确认消费明细!");
        return false;
    };
    let itemData = editDetail.dataParam.PRESENT_SEND_ITEM;
    if (!itemData.length) {
        iview.Message.info("请确认活动赠品!");
        return false;
    }
    for (var i = 0; i < itemData.length; i++) {
        if (!itemData[i].COUNT || parseFloat(itemData[i].COUNT) < 0) {
            iview.Message.info(`第${i + 1}行的赠品的满额不能小于0!`);
            return false;
        }
    }
    return true;
};

editDetail.showOne = function (data, callback) {
    //_.Ajax('ShowOneData', {
    //    Data: { BILLID: data }
    //}, function (data) {
    //    $.extend(editDetail.dataParam, data.mainData);
    //    editDetail.dataParam.PROMOBILL_FG_RULE = data.itemData;
    //});
};

editDetail.mountedInit = function () {
    editDetail.btnConfig = [{
        id: "add",
        authority: "10600501"
    }, {
        id: "edit",
        authority: "10600501"
    }, {
        id: "del",
        authority: "10600501"
    }, {
        id: "save",
        authority: "10600501"
    }, {
        id: "abandon",
        authority: "10600501"
    }, {
        id: "confirm",
        name: "审核",
        icon: "md-star",
        authority: "10600502",
        fun: function () {
            _.Ajax('ExecData', {
                Data: editDetail.dataParam
            }, function (data) {
                iview.Message.info("审核成功");
                editDetail.refreshDataParam(data);
            });
        },
        enabled: function (disabled, data) {
            if (!disabled && data.STATUS == 1) {
                return true;
            } else {
                return false;
            }
        },
        isNewAdd: true
    }];
};