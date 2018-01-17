editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       { DESCRIPTION:"已完成", OPERATION:"张三  2019-01-01" },
       { DESCRIPTION:"待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "DpglService";
    editDetail.method = "GetAssetChange";
    editDetail.Key = 'BILLID';

    editDetail.screenParam.colDef = [
    {
        title: "店铺代码", key: 'ASSETID', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.ASSETID
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.ASSETCHANGEITEM[params.index].ASSETID = event.target.value;
                    }
                },
            })
        },
    },
    { title: '资产类型', key: 'ASSET_TYPE_OLD', width: 200 },

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
            SHOPID: "",
            ASSET_TYPE_OLD: "",
        }]
    }

    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.ASSETCHANGEITEM || [];
        temp.push({});
        editDetail.dataParam.ASSETCHANGEITEM = temp;
    }
}