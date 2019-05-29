editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.dataParam.othersName = "商品扣率详情";
    editDetail.Key = 'BILLID';
    //初始化参数
    editDetail.screenParam.DISCOUNT = 0;

    //初始化弹窗所要传递参数

    editDetail.screenParam.showPopGoods = false;
    editDetail.screenParam.srcPopGoods = __BaseUrl + "/" + "Pop/Pop/PopGoodsList/";

    editDetail.screenParam.colDef = [
        //{ title: '商品ID', key: 'GOODSID', width: 100 },
        { title: "商品代码", key: "GOODSDM", width: 100 },
        { title: '商品名称', key: 'NAME', width: 150 },       
        {
            title: '折扣下限(%)', key: 'DISCOUNT_LOWER', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DISCOUNT_LOWER, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            //editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_LOWER = event.target.value;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_LOWER = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                        }
                    },
                })
            },
        },
        {
            title: '折扣上限(%)', key: 'DISCOUNT_CEILING', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DISCOUNT_CEILING, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            //editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_CEILING = event.target.value;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_CEILING = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                        }
                    },
                })
            },
        },
        {
            title: '折扣降点(%)', key: 'DISCOUNT_DROP_POINTS', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DISCOUNT_DROP_POINTS, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_RISE_POINTS = 0;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_DROP_POINTS = event.target.value==""||event.target.value==undefined ? 0 : event.target.value;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].NEW_DISCOUNT = parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].OLD_DISCOUNT) - parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_DROP_POINTS);
                        }
                    },
                })
            },
        },
        {
            title: '折扣升点(%)', key: 'DISCOUNT_RISE_POINTS', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DISCOUNT_RISE_POINTS, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_DROP_POINTS = 0;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_RISE_POINTS = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                            editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].NEW_DISCOUNT = parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].OLD_DISCOUNT) + parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].DISCOUNT_RISE_POINTS);
                        }
                    },
                })
            },
        },
        //{
        //    title: '新结算扣率(%)', key: 'NEW_DISCOUNT', width: 120,
        //    render: function (h, params) {
        //        return h('Input', {
        //            props: {
        //                value: params.row.NEW_DISCOUNT,type:'number'
        //            },
        //            on: {
        //                'on-blur': function (event) {
        //                    editDetail.dataParam.ADJUSTDISCOUNTITEM[params.index].NEW_DISCOUNT = event.target.value;
        //                }
        //            },
        //        })
        //    },
        //},
        {
            title: '新结算扣率(%)', key: 'NEW_DISCOUNT', width: 120
        },
        {
            title: '原结算扣率(%)', key: 'OLD_DISCOUNT', width: 120
        },
        {
            title: '操作',
            key: 'action',
            width: 80,
            align: 'center',
            render: function (h, params) {
                return h('div',
                    [
                    h('Button', {
                        props: { type: 'primary', size: 'small', disabled: false },

                        style: { marginRight: '50px' },
                        on: {
                            click: function (event) {
                                editDetail.dataParam.ADJUSTDISCOUNTITEM.splice(params.index, 1);
                            }
                        },
                    }, '删除')
                    ]);
            }
        }
    ]
    if (!editDetail.dataParam.ADJUSTDISCOUNTITEM) {
        editDetail.dataParam.ADJUSTDISCOUNTITEM = []
    };
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneAdjustDiscountEdit', {
        Data: { ADID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.AdjustDiscount[0]);
        editDetail.dataParam.BILLID = data.AdjustDiscount[0].ADID;
        editDetail.dataParam.ADJUSTDISCOUNTITEM = data.AdjustDiscountItem[0];
        callback && callback(data);
    });
}

///html中绑定方法
editDetail.otherMethods = {
    srchColGoods: function () {
        editDetail.screenParam.showPopGoods = true;
    },
    delColGoods: function () {
        var selectton = this.$refs.selectBrand.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的商品!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
                    if (editDetail.dataParam.ADJUSTDISCOUNTITEM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.ADJUSTDISCOUNTITEM.splice(j, 1);
                    }
                }
            }
        }
    },
    //批量设置折扣下限
    cDISCOUNT_LOWER: function () {
        var discoount = editDetail.screenParam.DISCOUNT == undefined || editDetail.screenParam.DISCOUNT=="" ? 0 : editDetail.screenParam.DISCOUNT;
        for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_LOWER = discoount;
        }
    },
    //批量设置折扣上限
    cDISCOUNT_CEILING: function () {
        var discoount = editDetail.screenParam.DISCOUNT == undefined || editDetail.screenParam.DISCOUNT == "" ? 0 : editDetail.screenParam.DISCOUNT;
        for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_CEILING = discoount;
        }
    },
    //批量设置折扣降点
    cDISCOUNT_DROP_POINTS: function () {
        var discoount = editDetail.screenParam.DISCOUNT == undefined || editDetail.screenParam.DISCOUNT == "" ? 0 : editDetail.screenParam.DISCOUNT;
        for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_DROP_POINTS = discoount;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_RISE_POINTS = 0;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].NEW_DISCOUNT = parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[j].OLD_DISCOUNT) - parseFloat(discoount);
        }
    },
    //批量设置折扣升点
    cDISCOUNT_RISE_POINTS: function () {
        var discoount = editDetail.screenParam.DISCOUNT == undefined || editDetail.screenParam.DISCOUNT == "" ? 0 : editDetail.screenParam.DISCOUNT;
        for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_RISE_POINTS = discoount;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_DROP_POINTS = 0;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].NEW_DISCOUNT = parseFloat(editDetail.dataParam.ADJUSTDISCOUNTITEM[j].OLD_DISCOUNT) + parseFloat(discoount);
        }
    },
    //重置
    cleardata: function () {
        for (var j = 0; j < editDetail.dataParam.ADJUSTDISCOUNTITEM.length; j++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_CEILING = 0;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_LOWER = 0;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].NEW_DISCOUNT = editDetail.dataParam.ADJUSTDISCOUNTITEM[j].OLD_DISCOUNT;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_DROP_POINTS = 0;
            editDetail.dataParam.ADJUSTDISCOUNTITEM[j].DISCOUNT_RISE_POINTS = 0;
        }
    },
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {

    if (editDetail.screenParam.showPopGoods) {
        editDetail.screenParam.showPopGoods = false;       
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.ADJUSTDISCOUNTITEM.push({
                GOODSID: data.sj[i].GOODSID,
                GOODSDM: data.sj[i].GOODSDM,
                NAME: data.sj[i].NAME,
                DISCOUNT_LOWER: 0,
                DISCOUNT_CEILING: 0,
                DISCOUNT_DROP_POINTS: 0,
                DISCOUNT_RISE_POINTS:0,
                NEW_DISCOUNT: data.sj[i].JSKL,
                OLD_DISCOUNT: data.sj[i].JSKL
            });
        }
    }
}

editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.BRANCHID = null;
    editDetail.dataParam.STARTTIME = null;
    editDetail.dataParam.ENDTIME = null;
    editDetail.dataParam.STATUS = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.ADJUSTDISCOUNTITEM = [];
}

editDetail.IsValidSave = function () {

    if (!editDetail.dataParam.STARTTIME) {
        iview.Message.info("请确认开始时间!");
        return false;
    };
    if (!editDetail.dataParam.ENDTIME) {
        iview.Message.info("请确认结束时间!");
        return false;
    };

    var stime = new Date(editDetail.dataParam.STARTTIME.replace(/\-/g, "\/"));
    var etime = new Date(editDetail.dataParam.ENDTIME.replace(/\-/g, "\/"));
    if (stime >= etime) {
        iview.Message.info("结束时间不能小于开始时间！");
        return false;
    }
    if (editDetail.dataParam.ADJUSTDISCOUNTITEM.length == 0) {
        iview.Message.info("请确认商品明细!");
        return false;
    };

    return true;
}