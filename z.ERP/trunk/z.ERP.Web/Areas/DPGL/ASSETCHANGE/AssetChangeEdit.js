editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       { DESCRIPTION: "已完成", OPERATION: "张三  2019-01-01" },
       { DESCRIPTION: "待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = true;
    editDetail.service = "DpglService";
    editDetail.method = "GetAssetChange";
    editDetail.Key = 'BILLID';

    editDetail.windowParam = {
        testwin1: false
    }

    editDetail.screenParam.colDef = [
    {
        title: "店铺ID", key: 'ASSETID', width: 160,
        render: function (h, params) {
            return h('div',
                [
            h('Input', {
                props: {
                    value: params.row.ASSETID
                },
                style: { marginRight: '5px', width: '80px' },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSETID = event.target.value;
                    }
                },
            }),
            h('Button', {
                props: { type: 'primary', size: 'small', disabled: false },

                style: { marginRight: '5px', width: '30px' },
                on: {
                    click: function () {
                        testwin1.Open(function (data) {
                            alert(data.Key);
                        });
                    }
                },
            }, '...'),

                ])
        },
    },
    {
        title: "原资产类型", key: 'ASSET_TYPE_OLD', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.ASSET_TYPE_OLD
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSET_TYPE_OLD = event.target.value;
                    }
                },
            })
        },
    },
        {
            title: "原建筑面积", key: 'AREA_BUILD_OLD', width: 100,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.AREA_BUILD_OLD
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_BUILD_OLD = event.target.value;
                        }
                    },
                })
            },
        },
                {
                    title: "原使用面积", key: 'AREA_USABLE_OLD', width: 100,
                    render: function (h, params) {
                        return h('Input', {
                            props: {
                                value: params.row.AREA_USABLE_OLD
                            },
                            on: {
                                'on-blur': function (event) {
                                    editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_USABLE_OLD = event.target.value;
                                }
                            },
                        })
                    },
                },
                        {
                            title: "原租赁面积", key: 'AREA_RENTABLE_OLD', width: 100,
                            render: function (h, params) {
                                return h('Input', {
                                    props: {
                                        value: params.row.AREA_RENTABLE_OLD
                                    },
                                    on: {
                                        'on-blur': function (event) {
                                            editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_RENTABLE_OLD = event.target.value;
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
        //                    editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSET_TYPE_NEW = event.target.value;
        //                }
        //            },
        //        })
        //    },
        //},
        //        {
        //            title: "新建筑面积", key: 'AREA_BUILD_NEW', width: 100,
        //            render: function (h, params) {
        //                return h('Input', {
        //                    props: {
        //                        value: params.row.AREA_BUILD_NEW
        //                    },
        //                    on: {
        //                        'on-blur': function (event) {
        //                            editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_BUILD_NEW = event.target.value;
        //                        }
        //                    },
        //                })
        //            },
        //        },
        //                        {
        //                            title: "新使用面积", key: 'AREA_USABLE_NEW', width: 100,
        //                            render: function (h, params) {
        //                                return h('Input', {
        //                                    props: {
        //                                        value: params.row.AREA_USABLE_NEW
        //                                    },
        //                                    on: {
        //                                        'on-blur': function (event) {
        //                                            editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_USABLE_NEW = event.target.value;
        //                                        }
        //                                    },
        //                                })
        //                            },
        //                        },
        //                {
        //                    title: "新租赁面积", key: 'AREA_RENTABLE_NEW', width: 100,
        //                    render: function (h, params) {
        //                        return h('Input', {
        //                            props: {
        //                                value: params.row.AREA_RENTABLE_NEW
        //                            },
        //                            on: {
        //                                'on-blur': function (event) {
        //                                    editDetail.dataParam.ASSETCHANGEITEM[params.index].AREA_RENTABLE_NEW = event.target.value;
        //                                }
        //                            },
        //                        })
        //                    },
        //                },
    //{ title: '资产类型', key: 'ASSET_TYPE_OLD', width: 200 },

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
        title: "店铺ID", key: 'ASSETID', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.ASSETID
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM2[params.index].ASSETID = event.target.value;
                    }
                },
            })
        },
    },
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
                        }
                    },
                })
            },
        },
    {
        title: "新资产类型", key: 'ASSET_TYPE_NEW', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.ASSET_TYPE_NEW
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM2[params.index].ASSET_TYPE_NEW = event.target.value;
                    }
                },
            })
        },
    },
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
    editDetail.screenParam.openPop = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM2 || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM2 = temp;
    }
}