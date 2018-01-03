editDetail.beforeVue = function () {
    //测试
    editDetail.stepParam = [
       { DESCRIPTION:"已完成", OPERATION:"张三  2019-01-01" },
       { DESCRIPTION:"待完成", OPERATION: "审核待完成" }
    ];
    editDetail.stepsCurrent = 0;
    editDetail.others = false;
    editDetail.branchid = false;


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
                        app.dataParam.MERCHANT_BRAND[params.index].BRANDID = event.target.value;
                    }
                },
            })
        },
    },
    { title: '品牌名称', key: 'NAME', width: 200 }
    ];
}