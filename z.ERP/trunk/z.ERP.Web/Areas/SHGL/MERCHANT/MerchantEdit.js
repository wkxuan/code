editDetail.beforeVue = function () {

    editDetail.others = false;
    editDetail.branchid = false;
    editDetail.service = "ShglService";
    editDetail.method = "GetMerchant";
    editDetail.Key = 'MERCHANTID';
    editDetail.dataParam.STATUS = "1";

    editDetail.screenParam.ParentBrand = {};
    editDetail.screenParam.dataCas = [];

    editDetail.screenParam.Orgid = [];

    editDetail.screenParam.colDef = [
    { type: 'selection', width: 60, align: 'center'},
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
                        editDetail.dataParam.MERCHANT_BRAND[params.index].BRANDID = event.target.value;

                        _.Ajax('GetBrand', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            if (data.dt) {
                                Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'NAME', data.dt.NAME);
                                Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'CATEGORYCODE', data.dt.CATEGORYCODE);
                                Vue.set(editDetail.dataParam.MERCHANT_BRAND[params.index], 'CATEGORYNAME', data.dt.CATEGORYNAME);
                            }
                            else {
                                iview.Message.info('当前品牌不存在!');
                            }
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

    editDetail.dataParam.MERCHANT_BRAND = editDetail.dataParam.MERCHANT_BRAND || [];
};

editDetail.otherMethods = {
    addColPP: function () {
        var temp = editDetail.dataParam.MERCHANT_BRAND || [];
        temp.push({});
        editDetail.dataParam.MERCHANT_BRAND = temp;
    },
    delColPP: function () {
        var selectton = this.$refs.selectBrand.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
        } else {
            for (var i = 0; i < selectton.length; i++) {
                for (var j = 0; j < editDetail.dataParam.MERCHANT_BRAND.length; j++) {
                    if (editDetail.dataParam.MERCHANT_BRAND[j].BRANDID == selectton[i].BRANDID) {
                        editDetail.dataParam.MERCHANT_BRAND.splice(j, 1);
                    }
                }
            }
        }
    },
    srchColPP: function () {
        Vue.set(editDetail.screenParam, "PopBrand", true);
    },


    BrandBack: function (val) {
        Vue.set(editDetail.screenParam, "PopBrand", false);
        for (var i = 0; i < val.sj.length; i++) {
            editDetail.dataParam.MERCHANT_BRAND.push(val.sj[i]);
        }
    },

    changeOrg: function (value, selectedData) {
        console.log(value[value.length - 1]);
        editDetail.dataParam.ORGID = value[value.length - 1];
        console.log(editDetail.dataParam.ORGID);
    },
    CasFZ: function () {
        var obj = findItemById("15", editDetail.screenParam.dataCas);
        console.log(obj);
        console.log(obj.code);
        console.log(obj.__value);
        var str=obj.__value;

        var arr = str.split(",") || [];
        console.log(arr);
        //var str1 = "1";
        //str1 = str1 + ",";
        //var arr1 = str1.split(",") || [];
        //console.log(arr1);

        editDetail.screenParam.Orgid = arr;
        console.log(editDetail.screenParam.Orgid);
    }
};

editDetail.showOne = function (data, callback) {
    _.Ajax('SearchMerchant', {
        Data: { MERCHANTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.merchant);
        editDetail.dataParam.BILLID = data.merchant.MERCHANTID;
        editDetail.dataParam.MERCHANT_BRAND = data.merchantBrand;
        editDetail.dataParam.ORGID = "15";
        editDetail.screenParam.dataCas = data.treeorg.Obj;//这块放在页面初始化得到
        editDetail.otherMethods.CasFZ();
   
        callback && callback(data);
    });
}



function findItemById(id, treeNode) {
    if (!id && id !== 0)
        return;
    var rootNode = treeNode;

    for (var inx = 0; inx < rootNode.length; inx++) {
        var subNode = rootNode[inx];
        if (subNode.value === id) {
            return subNode;
        } else {
            var findNode = findItemById(id, rootNode[inx].children);
            if (findNode)
                return findNode;
        }
    }
}


editDetail.clearKey = function () {
    editDetail.dataParam.MERCHANTID = null;
    editDetail.dataParam.NAME = null;
    editDetail.dataParam.SH = null;
    editDetail.dataParam.BANK_NAME = null;
    editDetail.dataParam.BANK = null;
    editDetail.dataParam.ADRESS = null;
    editDetail.dataParam.CONTACTPERSON = null;
    editDetail.dataParam.PHONE = null;
    editDetail.dataParam.PIZ = null;
    editDetail.dataParam.WEIXIN = null;
    editDetail.dataParam.QQ = null;
    editDetail.dataParam.MERCHANT_BRAND = [];
}


editDetail.IsValidSave = function () {


    if (!editDetail.dataParam.NAME) {
        iview.Message.info("请商户名称!");
        return false;
    };
    
    if (editDetail.dataParam.MERCHANT_BRAND.length == 0) {
        iview.Message.info("请确定品牌!");
        return false;
    } else {
        for (var i = 0; i < editDetail.dataParam.MERCHANT_BRAND.length; i++) {
            if (!editDetail.dataParam.MERCHANT_BRAND[i].BRANDID) {
                iview.Message.info("请确定品牌!");
                return false;
            };
        };
    };
    return true;
}