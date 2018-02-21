editDetail.beforeVue = function () {

    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.Key = 'CONTRACTID';
    editDetail.dataParam.STATUS = "1";
    editDetail.dataParam.othersName = "品牌商铺信息";


    editDetail.screenParam.colDefPP = [
    {
        title: "品牌代码", key: 'BRANDID', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.BRANDID
                },
                on: {
                    'on-enter': function (event) {
                        _self = this;
                        editDetail.dataParam.CONTRACT_BRAND[params.index].BRANDID = event.target.value;
                        _.Ajax('GetBrand', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            Vue.set(editDetail.dataParam.CONTRACT_BRAND[params.index], 'NAME', data.dt.NAME);
                        });
                    }
                },
            })
        },
    },
    { title: '品牌名称', key: 'NAME', width: 200 },
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
                            editDetail.dataParam.CONTRACT_BRAND.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];



    editDetail.screenParam.colDefSHOP = [
    {
        title: "单元代码", key: 'CODE', width: 100,
        render: function (h, params) {
            return h('Input', {
                props: {
                    value: params.row.CODE
                },
                on: {
                    'on-enter': function (event) {
                        _self = this;
                        editDetail.dataParam.CONTRACT_SHOP[params.index].CODE = event.target.value;
                        _.Ajax('GetShop', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'NAME', data.dt.NAME);
                        });
                    }
                },
            })
        },
    },
    { title: '单元名称', key: 'NAME', width: 200 },
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
                            editDetail.dataParam.CONTRACT_SHOP.splice(params.index, 1);
                        }
                    },
                }, '删除')
                ]);
        }
    }
    ];


    if (!editDetail.dataParam.CONTRACT_BRAND) {
        editDetail.dataParam.CONTRACT_BRAND = [{
            BRANDID: "",
            NAME: "",
        }]
    };

    if (!editDetail.dataParam.CONTRACT_SHOP) {
        editDetail.dataParam.CONTRACT_SHOP = [{
            CODE: "",
            NAME: "",
        }]
    };

    
    editDetail.screenParam.addColPP = function () {
        var temp = editDetail.dataParam.CONTRACT_BRAND || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_BRAND = temp;
    };
    editDetail.screenParam.addColSHOP = function () {
        var temp = editDetail.dataParam.CONTRACT_SHOP || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_SHOP = temp;
    };
}

editDetail.showOne = function (data, callback) {
}

editDetail.clearKey = function () {

}

editDetail.IsValidSave = function () {
    var d = new Date(editDetail.dataParam.CONT_START);
    editDetail.dataParam.CONT_START = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();

    var d = new Date(editDetail.dataParam.CONT_END);
    editDetail.dataParam.CONT_END = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();

    return true;
}

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchContract', {
        Data: { CONTRACTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.contract);
        editDetail.dataParam.BILLID = data.contract.CONTRACTID;
        callback && callback(data);
    });
}
