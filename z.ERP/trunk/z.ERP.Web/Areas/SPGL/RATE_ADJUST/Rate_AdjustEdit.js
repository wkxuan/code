﻿editDetail.beforeVue = function () {
    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.dataParam.othersName = "商品扣率详情";
    editDetail.Key = 'ID';
    //初始化参数
    editDetail.screenParam.RATE = 0;

    //初始化弹窗所要传递参数

    editDetail.screenParam.showPopGoods = false;
    editDetail.screenParam.srcPopGoods = __BaseUrl + "/" + "Pop/Pop/PopGoodsList/";

    editDetail.screenParam.colDef = [
        //{ title: '商品ID', key: 'GOODSID', width: 100 },
        { title: "商品代码", key: "GOODSDM", width: 100 },
        { title: '商品名称', key: 'NAME', width: 150 },

        {
            title: '扣率降点(%)', key: 'DOWN_RATE', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DOWN_RATE, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            
                            if (editDetail.dataParam.RATE_ADJUST_ITEM[params.index].DOWN_RATE != event.target.value) {
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].UP_RATE = 0;
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].DOWN_RATE = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].RATE_NEW = parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[params.index].RATE_OLD) - parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[params.index].DOWN_RATE);
                            }
                           }
                    },
                })
            },
        },
        {
            title: '扣率升点(%)', key: 'UP_RATE', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.UP_RATE, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            if (editDetail.dataParam.RATE_ADJUST_ITEM[params.index].UP_RATE != event.target.value){
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].DOWN_RATE = 0;
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].UP_RATE = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                                editDetail.dataParam.RATE_ADJUST_ITEM[params.index].RATE_NEW = parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[params.index].RATE_OLD) + parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[params.index].UP_RATE);
                            }
                     }
                    },
                })
            },
        },
        {
            title: '扣率下限(%)', key: 'LIMIT_DOWN', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.LIMIT_DOWN, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            //editDetail.dataParam.RATE_ADJUST_ITEM[params.index].LIMIT_DOWN = event.target.value;
                            editDetail.dataParam.RATE_ADJUST_ITEM[params.index].LIMIT_DOWN = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                        }
                    },
                })
            },
        },
        {
            title: '扣率上限(%)', key: 'LIMIT_UP', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.LIMIT_UP, type: 'number'
                    },
                    on: {
                        'on-blur': function (event) {
                            //editDetail.dataParam.RATE_ADJUST_ITEM[params.index].LIMIT_UP = event.target.value;
                            editDetail.dataParam.RATE_ADJUST_ITEM[params.index].LIMIT_UP = event.target.value == "" || event.target.value == undefined ? 0 : event.target.value;
                        }
                    },
                })
            },
        },

        //{
        //    title: '新结算扣率(%)', key: 'RATE_NEW', width: 120,
        //    render: function (h, params) {
        //        return h('Input', {
        //            props: {
        //                value: params.row.RATE_NEW,type:'number'
        //            },
        //            on: {
        //                'on-blur': function (event) {
        //                    editDetail.dataParam.RATE_ADJUST_ITEM[params.index].RATE_NEW = event.target.value;
        //                }
        //            },
        //        })
        //    },
        //},
        {
            title: '新扣率(%)', key: 'RATE_NEW', width: 120
        },
        {
            title: '原扣率(%)', key: 'RATE_OLD', width: 120
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
                                editDetail.dataParam.RATE_ADJUST_ITEM.splice(params.index, 1);
                            }
                        },
                    }, '删除')
                    ]);
            }
        }
    ]
    if (!editDetail.dataParam.RATE_ADJUST_ITEM) {
        editDetail.dataParam.RATE_ADJUST_ITEM = []
    };
}

editDetail.showOne = function (data, callback) {
    _.Ajax('ShowOneRateAdjustEdit', {
        Data: { ID: data }
    }, function (data) {  
        $.extend(editDetail.dataParam, data.RATE_ADJUST[0]);
        editDetail.dataParam.BILLID = data.RATE_ADJUST[0].ID;
        editDetail.dataParam.RATE_ADJUST_ITEM = data.RATE_ADJUST_ITEM[0];
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
                for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
                    if (editDetail.dataParam.RATE_ADJUST_ITEM[j].GOODSID == selectton[i].GOODSID) {
                        editDetail.dataParam.RATE_ADJUST_ITEM.splice(j, 1);
                    }
                }
            }
        }
    },
    //批量设置扣率下限
    cLIMIT_DOWN: function () {
        var discoount = editDetail.screenParam.RATE == undefined || editDetail.screenParam.RATE == "" ? 0 : editDetail.screenParam.RATE;
        for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
            editDetail.dataParam.RATE_ADJUST_ITEM[j].LIMIT_DOWN = discoount;
        }
    },
    //批量设置扣率上限
    cLIMIT_UP: function () {
        var discoount = editDetail.screenParam.RATE == undefined || editDetail.screenParam.RATE == "" ? 0 : editDetail.screenParam.RATE;
        for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
            editDetail.dataParam.RATE_ADJUST_ITEM[j].LIMIT_UP = discoount;
        }
    },
    //批量设置扣率降点
    cDOWN_RATE: function () {
        var discoount = editDetail.screenParam.RATE == undefined || editDetail.screenParam.RATE == "" ? 0 : editDetail.screenParam.RATE;
        for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
            editDetail.dataParam.RATE_ADJUST_ITEM[j].DOWN_RATE = discoount;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].UP_RATE = 0;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_NEW = parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_OLD) - parseFloat(discoount);
        }
    },
    //批量设置扣率升点
    cUP_RATE: function () {
        var discoount = editDetail.screenParam.RATE == undefined || editDetail.screenParam.RATE == "" ? 0 : editDetail.screenParam.RATE;
        for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
            editDetail.dataParam.RATE_ADJUST_ITEM[j].UP_RATE = discoount;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].DOWN_RATE = 0;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_NEW = parseFloat(editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_OLD) + parseFloat(discoount);
        }
    },
    //重置
    cleardata: function () {
        for (var j = 0; j < editDetail.dataParam.RATE_ADJUST_ITEM.length; j++) {
            editDetail.dataParam.RATE_ADJUST_ITEM[j].LIMIT_UP = 0;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].LIMIT_DOWN = 0;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_NEW = editDetail.dataParam.RATE_ADJUST_ITEM[j].RATE_OLD;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].DOWN_RATE = 0;
            editDetail.dataParam.RATE_ADJUST_ITEM[j].UP_RATE = 0;
        }
    },
}

///接收弹窗返回参数
editDetail.popCallBack = function (data) {

    if (editDetail.screenParam.showPopGoods) {
        editDetail.screenParam.showPopGoods = false;       
        for (var i = 0; i < data.sj.length; i++) {
            editDetail.dataParam.RATE_ADJUST_ITEM.push({
                GOODSID: data.sj[i].GOODSID,
                GOODSDM: data.sj[i].GOODSDM,
                NAME: data.sj[i].NAME,
                LIMIT_DOWN: 0,
                LIMIT_UP: 0,
                DOWN_RATE: 0,
                UP_RATE:0,
                RATE_NEW: data.sj[i].JSKL,
                RATE_OLD: data.sj[i].JSKL
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
    editDetail.dataParam.RATE_ADJUST_ITEM = [];
}

editDetail.IsValidSave = function () {

    if (!editDetail.dataParam.STARTTIME) {
        iview.Message.info("请选择开始时间!");
        return false;
    };
    if (!editDetail.dataParam.ENDTIME) {
        iview.Message.info("请选择结束时间!");
        return false;
    };

    var stime = new Date(editDetail.dataParam.STARTTIME.replace(/\-/g, "\/"));
    var etime = new Date(editDetail.dataParam.ENDTIME.replace(/\-/g, "\/"));

    if (stime >= etime) {
        iview.Message.info("结束时间不能小于开始时间！");
        return false;
    }




    if (editDetail.dataParam.RATE_ADJUST_ITEM.length == 0) {
        iview.Message.info("请录入商品明细!");
        return false;
    };
    //判断是否超上限，下限
    for (var i = 0; editDetail.dataParam.RATE_ADJUST_ITEM.length > i;i++){
        if (editDetail.dataParam.RATE_ADJUST_ITEM[i].LIMIT_DOWN != 0 && editDetail.dataParam.RATE_ADJUST_ITEM[i].DOWN_RATE > editDetail.dataParam.RATE_ADJUST_ITEM[i].LIMIT_DOWN)
        {
            iview.Message.info("商品编码：" + editDetail.dataParam.RATE_ADJUST_ITEM[i].GOODSDM + ",折扣降点不能大于折扣下限!");
            return false;
        }
        if (editDetail.dataParam.RATE_ADJUST_ITEM[i].LIMIT_UP != 0 && editDetail.dataParam.RATE_ADJUST_ITEM[i].UP_RATE > editDetail.dataParam.RATE_ADJUST_ITEM[i].LIMIT_UP) {
            iview.Message.info("商品编码：" + editDetail.dataParam.RATE_ADJUST_ITEM[i].GOODSDM + ",折扣升点不能大于折扣上限!");
            return false;
        }
    };

    return true;
}