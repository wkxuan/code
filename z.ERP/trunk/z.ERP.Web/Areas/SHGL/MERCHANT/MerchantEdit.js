//_self.iview.Message.info('请先确定年度!');

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
                    'on-blur': function (event) {
                        _self = this;
                        editDetail.dataParam.MERCHANT_BRAND[params.index].BRANDID = event.target.value;

                        _.Ajax('GetBrand', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            editDetail.dataParam.MERCHANT_BRAND[params.index].NAME = data.dt[0].NAME;
                            //_self.Vue.set(editDetail.dataParam, editDetail.dataParam);
                            // _self.Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'NAME', data.dt[0].NAME);
                            //editDetail.screenParam.addCol();
                        });
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
                            editDetail.dataParam.MERCHANT_BRAND.splice(params.index, 1);
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
        var temp = editDetail.dataParam.MERCHANT_BRAND || [];
        temp.push({});
        editDetail.dataParam.MERCHANT_BRAND = temp;
    }
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        editDetail.dataParam.BILLID = data.merchant[0].MERCHANTID;
        editDetail.dataParam.NAME = data.merchant[0].NAME;
        editDetail.dataParam.SH = data.merchant[0].SH;
        editDetail.dataParam.BANK = data.merchant[0].BANK;
        editDetail.dataParam.BANK_NAME = data.merchant[0].BANK_NAME;
        editDetail.dataParam.ADRESS = data.merchant[0].ADRESS;
        editDetail.dataParam.CONTACTPERSON = data.merchant[0].CONTACTPERSON;
        editDetail.dataParam.PHONE = data.merchant[0].PHONE;
        editDetail.dataParam.PIZ = data.merchant[0].PIZ;
        editDetail.dataParam.WEIXIN = data.merchant[0].WEIXIN;
        editDetail.dataParam.QQ = data.merchant[0].QQ;
        editDetail.dataParam.STATUS = data.merchant[0].STATUS;
        editDetail.dataParam.MERCHANT_BRAND = data.merchantBrand[0];
        callback && callback(data);
    });
}

editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
}
