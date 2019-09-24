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
       { title: '赠品编码', key: 'PRESENTID', width: 100 },
       { title: '赠品名称', key: 'PRESENTNAME' },
       { title: '价值', key: 'PRICE', width: 100 },
    ];
    //赠品发放明细
    editDetail.screenParam.colDefpresents = [
       { title: '赠品编码', key: 'PRESENTID', width: 100 },
       { title: '赠品名称', key: 'PRESENTNAME' },
       { title: '数量', key: 'COUNT', cellType: "input", cellDataType: "number", width: 150 },
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
            switch (data.Status) {
                case 1:
                    if (editDetail.dataParam.PRESENT_SEND_TICKET.filter(function (item) { return (data.ticketinfo.POSNO == item.POSNO&&data.ticketinfo.DEALID == item.DEALID) }).length == 0) {     //已添加数据 跳过
                        editDetail.dataParam.PRESENT_SEND_TICKET.push({
                            POSNO: data.ticketinfo.POSNO,
                            DEALID: data.ticketinfo.DEALID,
                            AMOUNT: data.ticketinfo.AMOUNT,
                            SALE_TIME: data.ticketinfo.SALE_TIME,
                            FGID: data.ticketinfo.FGID,
                        })
                        editDetail.otherMethods.cleardata();  //小票列表数据改变，清空赠品列表
                        iview.Message.success('交易数据添加成功!');
                    } else {
                        iview.Message.warning('该交易数据已添加!');
                    }
                    break;
                case 2:
                    iview.Message.warning('暂无有效交易数据!');
                    break;
                case 3:
                    iview.Message.warning('小票交易时间未在活动内!');
                    break;
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
            editDetail.otherMethods.cleardata();  //小票列表数据改变，清空赠品列表
        }
    },
    SUMticketamount: function () {        
        var PROMOBILL_FG_RULE = [];
        var sum = 0;
        let itemd = editDetail.dataParam.PRESENT_SEND_TICKET;
        if (!itemd.length) {
            iview.Message.warning("请确认交易小票!");
            return;
        };
        for (let i = 0; i < itemd.length; i++) {
            sum += parseFloat(itemd[i].AMOUNT);
            if (PROMOBILL_FG_RULE.filter(function (item) { return (itemd[i].FGID == item.BILLID) }).length == 0) {
                PROMOBILL_FG_RULE.push({
                    BILLID: itemd[i].FGID,
                    FULL: itemd[i].AMOUNT,
                })
            } else {
                for (let j = 0; j < PROMOBILL_FG_RULE.length; j++) {
                    if (itemd[i].FGID == PROMOBILL_FG_RULE[j].BILLID) {
                        PROMOBILL_FG_RULE[j].FULL += itemd[i].AMOUNT;
                    }
                }
            }
        }
        editDetail.screenParam.AMOUNTS = sum;    //计算合计金额
        _.Ajax('GetPresentList', {
            Data: PROMOBILL_FG_RULE
        }, function (data) {
            editDetail.screenParam.PRESENT = data;
        });
    },
    addpresentss: function () {
        let selection = this.$refs.refpresents.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选择赠品列表中的赠品!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                if (editDetail.dataParam.PRESENT_SEND_ITEM.filter(
                    function (item)
                    { return (selection[i].PRESENTID == item.PRESENTID ) }).length == 0){     //已添加数据 跳过
                    editDetail.dataParam.PRESENT_SEND_ITEM.push({
                        PRESENTID: selection[i].PRESENTID,
                        PRESENTNAME: selection[i].PRESENTNAME,
                        COUNT: 1,
                    })
                }
            };
        }       
    },
    delpresentss: function () {
        let selection = this.$refs.refpresentss.getSelection();
        if (selection.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        } else {
            for (let i = 0; i < selection.length; i++) {
                let temp = editDetail.dataParam.PRESENT_SEND_ITEM;
                for (let j = 0; j < temp.length; j++) {
                    if (temp[j].PRESENTID == selection[i].PRESENTID) {
                        temp.splice(j, 1);
                        break;
                    }
                }
            };
            editDetail.cleardata();  //小票列表数据改变，清空赠品列表
        }
    },
    cleardata: function () {
        editDetail.dataParam.PRESENT_SEND_ITEM = [];
        editDetail.screenParam.PRESENT = [];
    }
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
    _.Ajax('ShowOneData', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.mainData);
        editDetail.dataParam.PRESENT_SEND_TICKET = data.ticketData;
        editDetail.dataParam.PRESENT_SEND_ITEM = data.itemData;
        //editDetail.otherMethods.SUMticketamount();
    });
};
//editDetail.afterAbandon = function () {
//    editDetail.otherMethods.SUMticketamount();
//}
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