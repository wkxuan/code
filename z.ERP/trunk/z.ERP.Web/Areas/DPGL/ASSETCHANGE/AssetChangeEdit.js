editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       //{ DESCRIPTION: "已完成", OPERATION: "张三  2019-01-01" },
       //{ DESCRIPTION: "待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.service = "DpglService";
    editDetail.method = "GetAssetChange";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.CHANGE_TYPE = 1;
    editDetail.screenParam.componentVisible = false;
    editDetail.screenParam.showPopShop = false;
    editDetail.screenParam.srcPopShop = __BaseUrl + "/" + "Pop/Pop/PopShopList/";
    editDetail.screenParam.popParam = {};


    editDetail.screenParam.colDef = [
    //{
    //    title: "店铺ID", key: 'ASSETID', width: 160,
    //    render: function (h, params) {
    //        return h('div',
    //            [
    //        h('Input', {
    //            props: {
    //                value: params.row.ASSETID
    //            },
    //            style: { marginRight: '5px', width: '80px' },

    //            on: {
    //                'on-enter': function (event) {
    //                    _self = this;
    //                    editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSETID = event.target.value;
    //                    _.Ajax('GetShop', {
    //                        Data: { SHOPID: event.target.value }
    //                    }, function (data) {
    //                        Vue.set(editDetail.dataParam.ASSETCHANGEITEM[params.index], 'CODE', data.dt.CODE),
    //                        Vue.set(editDetail.dataParam.ASSETCHANGEITEM[params.index], 'AREA_BUILD_OLD', data.dt.AREA_BUILD),
    //                        Vue.set(editDetail.dataParam.ASSETCHANGEITEM[params.index], 'AREA_USABLE_OLD', data.dt.AREA_USABLE),
    //                        Vue.set(editDetail.dataParam.ASSETCHANGEITEM[params.index], 'AREA_RENTABLE_OLD', data.dt.AREA_RENTABLE)
    //                    });
    //                }
    //            },
    //        }),
    //        h('Button', {
    //            props: { type: 'primary', size: 'small', disabled: false },

    //            style: { marginRight: '5px', width: '30px' },
    //            on: {
    //                click: editDetail.screenParam.openPop
    //            },
    //        }, '...'),

    //            ])
    //    },
    //},
    { title: '店铺代码', key: 'CODE', width: 100 },
    { title: '原建筑面积', key: 'AREA_BUILD_OLD', width: 100 },
    { title: '原使用面积', key: 'AREA_USABLE_OLD', width: 100 },
    { title: '原租赁面积', key: 'AREA_RENTABLE_OLD', width: 100 },
   {
       title: "新建筑面积", key: 'AREA_BUILD_NEW', width: 100,
       render: function (h, params) {
           return h('Input', {
               props: {
                   value: params.row.AREA_BUILD_NEW
               },
               on: {
                   'on-blur': function (event) {
                       editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_BUILD_NEW = event.target.value;
                   }
               },
           })
       },
   },
    {
        title: "新使用面积", key: 'AREA_USABLE_NEW', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.AREA_USABLE_NEW
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_USABLE_NEW = event.target.value;
                    }
                },
            })
        },
    },
    {
        title: "新租赁面积", key: 'AREA_RENTABLE_NEW', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.AREA_RENTABLE_NEW
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_RENTABLE_NEW = event.target.value;
                    }
                },
            })
        },
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
                            editDetail.dataParam.ASSETCHANGEITEM.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];

    if (!editDetail.dataParam.ASSETCHANGEITEM) {
        editDetail.dataParam.ASSETCHANGEITEM = [{
            ASSETID: "",
            ASSET_TYPE_OLD: "",
        }]
    }

    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM = temp;
        //        editDetail.dataParam.ASSETCHANGEITEM.CHANGE_TYPE = '1';
    }
}

editDetail.otherMethods = {
    SelShop: function () {
        if (!editDetail.dataParam.BRANCHID) {
            iview.Message.info("请选择分店!");
            return;
        } else {
            editDetail.screenParam.showPopShop = true;
            editDetail.screenParam.popParam = { BRANCHID: editDetail.dataParam.BRANCHID, STATUS: "2" };
        }
    }
}
editDetail.newRecord = function () {
    editDetail.dataParam.AREAID = "2";
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchAssetChange', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.assetchange);
        editDetail.dataParam.BILLID = data.assetchange.BILLID;
        editDetail.dataParam.ASSETCHANGEITEM = data.assetchangeitem;
        callback && callback(data);
    });
}
//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopShop = false;

    //删除空行
    if (editDetail.dataParam.ASSETCHANGEITEM.length > 0) {
        if (!editDetail.dataParam.ASSETCHANGEITEM[0].ASSETID) {
            editDetail.dataParam.ASSETCHANGEITEM.splice(0, 1);
        }
    }
    //接收选中的数据
    for (var i = 0; i < data.sj.length; i++) {
        if ((editDetail.dataParam.ASSETCHANGEITEM.length === 0)
            || (editDetail.dataParam.ASSETCHANGEITEM.length > 0
            && editDetail.dataParam.ASSETCHANGEITEM.filter(function (item) {
            return parseInt(item.ASSETID) === data.sj[i].SHOPID;
        }).length === 0))
            editDetail.dataParam.ASSETCHANGEITEM.push({
                ASSETID: data.sj[i].SHOPID,
                CODE: data.sj[i].SHOPCODE,
                AREA_BUILD_OLD: data.sj[i].AREA_BUILD,
                AREA_USABLE_OLD: data.sj[i].AREA_USABLE,
                AREA_RENTABLE_OLD: data.sj[i].AREA_RENTABLE
            });
    }
};


editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.ASSETCHANGEITEM = [];
}

editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.BRANCHID) {
        iview.Message.info("请选择分店!");
        return false;
    };

    if (editDetail.dataParam.ASSETCHANGEITEM.length == 0) {
        iview.Message.info("请选择单元!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.ASSETCHANGEITEM.length; i++) {
            if (!editDetail.dataParam.ASSETCHANGEITEM[i].ASSETID) {
                iview.Message.info("请选择单元!");
                return false;
            };
            if (!editDetail.dataParam.ASSETCHANGEITEM[i].AREA_RENTABLE_NEW) {
                iview.Message.info("请输入新租赁面积！!");
                return false;
            };
        };
    };

    return true;
}

