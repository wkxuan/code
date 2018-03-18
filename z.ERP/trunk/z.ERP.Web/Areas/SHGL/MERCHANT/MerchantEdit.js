editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "ShglService";
    editDetail.method = "GetMerchant";
    editDetail.Key = 'MERCHANTID';
    editDetail.dataParam.STATUS = "1";


    editDetail.screenParam.colDef = [
    {
        title: "品牌代码", key: 'BRANDID', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.BRANDID
                },
                on: {
                    // 'on-blur': 焦点离开 'on-enter' 回车事件
                    'on-enter': function (event) {
                        _self = this;
                        editDetail.dataParam.MERCHANT_BRAND[params.index].BRANDID = event.target.value;

                        _.Ajax('GetBrand', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'NAME', data.dt.NAME);
                            Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'CATEGORYCODE', data.dt.CATEGORYCODE);
                            Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'CATEGORYNAME', data.dt.CATEGORYNAME);
                        });
                    }
                },
            })
        },
    },
    { title: '品牌名称', key: 'NAME', width: 200 },
    { title: '业态代码', key: 'CATEGORYCODE', width: 200 },
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
                            editDetail.dataParam.MERCHANT_BRAND.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];

    //相当于初始化
    if (!editDetail.dataParam.MERCHANT_BRAND) {
        editDetail.dataParam.MERCHANT_BRAND = [{
            BRANDID: "",
            NAME: "",
            CATEGORYID: "",
            CATEGORYNAME: ""
        }]
    }

    //新增加一行
    editDetail.screenParam.addCol = function () {
        var temp = editDetail.dataParam.MERCHANT_BRAND || [];
        temp.push({});
        editDetail.dataParam.MERCHANT_BRAND = temp;
    }
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.merchant);
        editDetail.dataParam.BILLID = data.merchant.MERCHANTID;
        editDetail.dataParam.MERCHANT_BRAND = data.merchantBrand;
        callback && callback(data);
    });
}

editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
}
