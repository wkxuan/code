var tempItem2;
var itemCurRow;
editDetail.beforeVue = function () {
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.service = "DpglService";
    editDetail.method = "SearchAssetSpilt";
    editDetail.Key = 'BILLID';
    editDetail.dataParam.CHANGE_TYPE = 3;
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
    //            //on: {
    //            //    'on-blur': function (event) {
    //            //        editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSETID = event.target.value;
    //            //    }
    //            //},
    //            on: {
    //                'on-enter': function (event) {
    //                    _self = this;
    //                    editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSETID = event.target.value;
    //                    _.Ajax('GetShop', {
    //                        Data: { SHOPID: event.target.value }
    //                    }, function (data) {
    //                        //editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_BUILD_OLD = data.dt.AREA_BUILD,
    //                        //editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_USABLE_OLD = data.dt.AREA_USABLE,
    //                        //editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_RENTABLE_OLD = data.dt.AREA_RENTABLE
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
    //                //    function () {
    //                //    testwin1.Open(function (data) {
    //                //        alert(data.Key); 
    //                //    });
    //                //}
    //            },
    //        }, '...'),
    //            ])
    //    },
    //},
    { title: '店铺ID', key: 'ASSETID', width: 100 ,hidden:true},
    { title: '店铺代码', key: 'CODE', width: 100 },
    { title: '原建筑面积', key: 'AREA_BUILD_OLD', width: 100 },
    { title: '原使用面积', key: 'AREA_USABLE_OLD', width: 100 },
    { title: '原租赁面积', key: 'AREA_RENTABLE_OLD', width: 100 },
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
    editDetail.screenParam.colDef2 = [

        {
            title: "新店铺代码", key: 'ASSETCODE_NEW', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.ASSETCODE_NEW
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.ASSETCHANGEITEM2[params.index].ASSETCODE_NEW = event.target.value;
                            editDetail.dataParam.ASSETCHANGEITEM2[params.index].ASSETID = itemCurRow.ASSETID;
                        }
                    },
                })
            },
        },
    //{
    //    title: "新资产类型", key: 'ASSET_TYPE_NEW', width: 100,
    //    render: function (h, params) {
    //        return h('Input', {
    //            props: {
    //                value: params.row.ASSET_TYPE_NEW
    //            },
    //            on: {
    //                'on-blur': function (event) {
    //                    editDetail.dataParam.ASSETCHANGEITEM2[params.index].ASSET_TYPE_NEW = event.target.value;
    //                }
    //            },
    //        })
    //    },
    //},
            {
                title: "新建筑面积", key: 'AREA_BUILD_NEW', width: 100,
                render: function (h, params) {
                    return h('Input', {
                        props: {
                            value: params.row.AREA_BUILD_NEW
                        },
                        on: {
                            'on-blur': function (event) {
                                editDetail.dataParam.ASSETCHANGEITEM2[params.index].AREA_BUILD_NEW = event.target.value;
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
                                editDetail.dataParam.ASSETCHANGEITEM2[params.index].AREA_USABLE_NEW = event.target.value;
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
                                editDetail.dataParam.ASSETCHANGEITEM2[params.index].AREA_RENTABLE_NEW = event.target.value;
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
                                    editDetail.dataParam.ASSETCHANGEITEM2.splice(params.index, 1);
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
    if (!editDetail.dataParam.ASSETCHANGEITEM2) {
        editDetail.dataParam.ASSETCHANGEITEM2 = [{
            ASSETID: "",
            ASSET_TYPE_NEW: "",
        }]
    }
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM = temp;
    }

    editDetail.screenParam.addCol2 = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM2 || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM2 = temp;
    }
}
editDetail.showOne = function (data, callback) {
    _.Ajax('SearchAssetSpilt', {
        Data: { BILLID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.assetSpilt);
        editDetail.dataParam.BILLID = data.assetSpilt.BILLID;
        editDetail.dataParam.ASSETCHANGEITEM = data.assetSpiltitem;
        editDetail.dataParam.ASSETCHANGEITEM2 = data.assetSpiltitem2;
        //editDetail.filter(editDetail.dataParam.ASSETCHANGEITEM[0], 1);
        callback && callback(data);
    });
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
        };
    };

    //处理拆分信息
    tempItem2 = tempItem2.filter(function (row2) {
        return parseInt(row2.ASSETID) !== itemCurRow.ASSETID;
    })
    for (inx in editDetail.dataParam.ASSETCHANGEITEM2) {
        tempItem2.push(editDetail.dataParam.ASSETCHANGEITEM2[inx]);
    }
    editDetail.dataParam.ASSETCHANGEITEM2 = tempItem2;
    return true;
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
    },
    filter: function (curRow, oldRow) {
        itemCurRow = curRow;
        if (tempItem2 === undefined) {
            tempItem2 = editDetail.dataParam.ASSETCHANGEITEM2
        }
        else {
            for (inx in editDetail.dataParam.ASSETCHANGEITEM2)
            { tempItem2.push(editDetail.dataParam.ASSETCHANGEITEM2[inx]); }
        }
        filterdata = tempItem2.filter(function (row2) {
            return parseInt(row2.ASSETID) === curRow.ASSETID;
        });
        Vue.set(editDetail.dataParam, "ASSETCHANGEITEM2", filterdata);
        if (tempItem2 !== undefined) {
            tempItem2 = tempItem2.filter(function (row2) {
                return parseInt(row2.ASSETID) !== curRow.ASSETID;
            })
        }

    }
}

//接收子页面返回值
editDetail.popCallBack = function (data) {
    editDetail.screenParam.showPopShop = false;
    for (var i = 0; i < data.sj.length; i++) {
        var shop = {};
        shop.ASSETID = data.sj[i].SHOPID;
        shop.CODE = data.sj[i].SHOPCODE;
        shop.AREA_BUILD_OLD = data.sj[i].AREA_BUILD;
        shop.AREA_USABLE_OLD = data.sj[i].AREA_USABLE;
        shop.AREA_RENTABLE_OLD = data.sj[i].AREA_RENTABLE;
        editDetail.dataParam.ASSETCHANGEITEM.push(shop);
    };
};


editDetail.clearKey = function () {
    editDetail.dataParam.BILLID = null;
    editDetail.dataParam.DESCRIPTION = null;
    editDetail.dataParam.ASSETCHANGEITEM = [];
    editDetail.dataParam.ASSETCHANGEITEM2 = [];
}