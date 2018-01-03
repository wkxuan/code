editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       { DESCRIPTION:"已完成", OPERATION:"张三  2019-01-01" },
       { DESCRIPTION:"待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "ShglService";
    editDetail.method = "GetMerchant";

    editDetail.screenParam.colDef = [
    {
        title: "品牌代码", key: 'BRANDID', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.BRANDID
                },
                on: {
                    'on-blur': function (event) {
                        editDetail.dataParam.MERCHANT_BRAND[params.index].BRANDID = event.target.value;
                    }
                },
            })
        },
    },
    { title: '品牌名称', key: 'NAME', width: 200 },
    { title: '业态代码', key: 'CATEGORYID', width: 200 },
    { title: '业态名称', key: 'CATEGORYNAME', width: 200 },

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
                            editDetail.dataParam.MERCHANT_BRAND.splice(params.index,1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];

    if (!editDetail.dataParam.MERCHANT_BRAND) {
        editDetail.dataParam.MERCHANT_BRAND = [{
            BRANDID: "",
            NAME: "",
            CATEGORYID: "",
            CATEGORYNAME: ""
        }]
    }

    editDetail.screenParam.addCol = function () {
        var  temp = editDetail.dataParam.MERCHANT_BRAND||[];
        temp.push({});
        editDetail.dataParam.MERCHANT_BRAND = temp;
    }
}