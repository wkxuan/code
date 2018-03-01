﻿editDetail.beforeVue = function () {

    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.Key = 'CONTRACTID';
    editDetail.dataParam.STATUS = 1;
    editDetail.dataParam.JXSL = 0.17;
    editDetail.dataParam.XXSL = 0.17;
    editDetail.dataParam.othersName = "品牌商铺信息";

    //品牌表格
    editDetail.screenParam.colDefPP = [
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
                        editDetail.dataParam.CONTRACT_BRAND[params.index].BRANDID = event.target.value;

                        _.Ajax('GetBrand', {
                            Data: { ID: event.target.value }
                        }, function (data) {
                            if(data.dt){
                                Vue.set(editDetail.dataParam.CONTRACT_BRAND[params.index], 'NAME', data.dt.NAME);
                            }else{
                                iview.Message.info('当前品牌不存在!');
                                Vue.set(editDetail.dataParam.CONTRACT_BRAND[params.index], 'BRANDID', "");
                            }
                        });
                    }
                },
            })
        },
    },
    { title: '品牌名称', key: 'NAME', width: 200 },
    ];




    //商铺表格
    editDetail.screenParam.colDefSHOP = [
    { type: 'selection', width: 60, align: 'center'},
    {
        title: "商铺代码", key: 'CODE', width: 100,
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
                            Data: { CODE: event.target.value,BRANCHID:editDetail.dataParam.BRANCHID }
                        }, function (data) {
                            if(data.dt){
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'SHOPID', data.dt.SHOPID);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYID', data.dt.CATEGORYID);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYCODE', data.dt.CATEGORYCODE);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CATEGORYNAME', data.dt.CATEGORYNAME);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'AREA', data.dt.AREA_BUILD);
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'AREA_RENTABLE', data.dt.AREA_RENTABLE);
                                calculateArea();
                            }
                            else{
                                iview.Message.info('当前单元代码不存在或者不属于当前分店卖场!');
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'CODE', "");
                                Vue.set(editDetail.dataParam.CONTRACT_SHOP[params.index], 'SHOPID', "");
                            }
                        });
                    }
                },
            })
        },
    },
    { title: '业态代码', key: 'CATEGORYCODE', width: 100 },
    { title: '业态名称', key: 'CATEGORYNAME', width: 100 },
    { title: '建筑面积', key: 'AREA', width: 100 },
    { title: '租用面积', key: 'AREA_RENTABLE', width: 100 }
    ];


    //保底表格
    editDetail.screenParam.colDefRENT = [
       { type: 'selection', width: 60, align: 'center'},
       { title: '时间段', key: 'INX', width: 80 },
       {
            title: '开始日期',
            key: 'STARTDATE',
            width: 150,
        },
        {
           title: '结束日期',
           key: 'ENDDATE',
           width: 150,
           render: function (h, params) {
               return h('DatePicker', {
                   props: {
                       value: params.row.ENDDATE
                   },
                   on: {
                       'on-change': function (event) {
                           Vue.set(editDetail.dataParam.CONTRACT_RENT[params.index], 'ENDDATE', event);
                       }
                   },
               })
           },
        },
        {
            title: "保底(毛利or销售)", key: 'RENTS', width: 150,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.RENTS
                    },
                    on: {
                        'on-enter': function (event) {
                            editDetail.dataParam.CONTRACT_RENT[params.index].RENTS = event.target.value;
                        }
                    },
                })
            },           
        },
        {
            title: "保底扣率", key: 'RENTS_JSKL', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.RENTS_JSKL
                    },
                    on: {
                        'on-enter': function (event) {
                            editDetail.dataParam.CONTRACT_RENT[params.index].RENTS_JSKL = event.target.value;
                        }
                    },
                })
            },           
        },
        { title: "总保底(毛利or销售)", key: 'SUMRENTS', width: 150},
    ];


    //月度分解表格
    editDetail.screenParam.colDefRENTITEM = [
        { title: '时间段', key: 'INX', width: 80 },
        { title: '开始日期', key: 'STARTDATE',width: 150},
        { title: '结束日期', key: 'ENDDATE', width: 150},
        { title: '年月', key: 'YEARMONTH', width: 100},
        { title: '保底销售or毛利', key: 'RENTS', width: 200},
        { title: '生成日期', key: 'CREATEDATE',width: 170},
        {  title: '清算标记', key: 'QSBJ',width: 150,
           render: function(h, params){
            return h('Select',{
                props:{
                    value:params.row.QSBJ
                },
                on:{
                    'on-change':function(event){
                        Vue.set(editDetail.dataParam.CONTRACT_RENTITEM[params.index], 'QSBJ', event);
                    }
                }},
                [ h('Option', {props: {value: '1'} }, '√'),
                  h('Option', {props: {value: '2'} }, '')
                ],
            )}
        },
    ];


    //表格数据初始化
    if (!editDetail.dataParam.CONTRACT_BRAND) {
        editDetail.dataParam.CONTRACT_BRAND = [{
            BRANDID: ""
        }]
    };

    if (!editDetail.dataParam.CONTRACT_SHOP) {
        editDetail.dataParam.CONTRACT_SHOP = [{
            CODE: ""
        }]
    };

    if (!editDetail.dataParam.CONTRACT_RENT) {
        editDetail.dataParam.CONTRACT_RENT = [{
            PRICE: 0
        }]
    };

    if (!editDetail.dataParam.CONTRACT_RENTITEM) {
        editDetail.dataParam.CONTRACT_RENTITEM = [{
            RENTS: 0
        }]
    }; 
    
    calculateArea = function(){
        editDetail.dataParam.AREA_BUILD = 0;
        editDetail.dataParam.AREAR = 0;
        for (var i = 0; i < editDetail.dataParam.CONTRACT_SHOP.length; i++) {
            if (editDetail.dataParam.CONTRACT_SHOP[i].SHOPID){
                editDetail.dataParam.AREA_BUILD+=editDetail.dataParam.CONTRACT_SHOP[i].AREA;
                editDetail.dataParam.AREAR+=editDetail.dataParam.CONTRACT_SHOP[i].AREA_RENTABLE;
            }
        }
    }
}

editDetail.otherMethods={

    //添加品牌
    addColPP: function () {
        var temp = editDetail.dataParam.CONTRACT_BRAND || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_BRAND = temp;
    },
    //删除品牌
    delColPP:function(){
        var selectton = this.$refs.selectBrand.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的品牌!");
        }else{
            for(var i=0;i<selectton.length;i++){
                for (var j = 0; j < editDetail.dataParam.CONTRACT_BRAND.length; j++) {
                    if (editDetail.dataParam.CONTRACT_BRAND[j].BRANDID==selectton[i].BRANDID){
                        editDetail.dataParam.CONTRACT_BRAND.splice(j, 1);
                    }
                }
            }
        }
    },
    //添加商铺
    addColSHOP : function () {
        if (!editDetail.dataParam.BRANCHID){
            iview.Message.info('请先确认分店!');
            return;
        }
        var temp = editDetail.dataParam.CONTRACT_SHOP || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_SHOP = temp;
    },
    //删除商铺
    delColSHOP:function(){
        var selectton = this.$refs.selectShop.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的商铺!");
        }else{
            for(var i=0;i<selectton.length;i++){
                for (var j = 0; j < editDetail.dataParam.CONTRACT_SHOP.length; j++) {
                    if (editDetail.dataParam.CONTRACT_SHOP[j].SHOPID==selectton[i].SHOPID){
                        editDetail.dataParam.CONTRACT_SHOP.splice(j, 1);
                        calculateArea();
                    }
                }
            }
        }
    },
    //添加一行保底信息
    addColDefRENT:function(){
        var temp = editDetail.dataParam.CONTRACT_RENT || [];
        temp.push({});
        editDetail.dataParam.CONTRACT_RENT = temp;
    },

    //删除一行保底
    delColDefRENT:function(){
        var selectton = this.$refs.selectRent.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        }else{
            for(var i=0;i<selectton.length;i++){
                for (var j = 0; j < editDetail.dataParam.CONTRACT_RENT.length; j++) {
                    if (editDetail.dataParam.CONTRACT_RENT[j].INX==selectton[i].INX){
                        editDetail.dataParam.CONTRACT_RENT.splice(j, 1);
                        calculateArea();
                    }
                }
            }
        }
    },

    //按年度分解
    operPeriod : function(){
        if (!editDetail.dataParam.CONT_START || !editDetail.dataParam.CONT_END) {
            iview.Message.info("请先维护租约开始结束日期!");
            return;
        }
        if (editDetail.dataParam.CONT_START > editDetail.dataParam.CONT_END) {
            iview.Message.info("租约结束日期不能小于开始日期！");
            return;
        }
        editDetail.dataParam.CONTRACT_RENT = [];
        var yearsValue = getYears(new Date(editDetail.dataParam.CONT_START), new Date(editDetail.dataParam.CONT_END)); 
        var nestYear = null;
        var rentData = null;
        var beginHtq = editDetail.dataParam.CONT_START; 
        
        //循环年数
        for (var i = 0; i <= yearsValue; i++) {
            var copyHtQsr = formatDate(beginHtq); 
            nestYear = getNextYears(beginHtq);
            if (nestYear < formatDate(editDetail.dataParam.CONT_END)) { 
                rentData = {
                   INX: i + 1,
                   STARTDATE: copyHtQsr,
                   ENDDATE: nestYear,
                   RENTS: 0,
                   RENTS_JSKL: 0,
                   SUMRENTS: 0
                }
               editDetail.dataParam.CONTRACT_RENT.push(rentData);
               beginHtq = addDate(nestYear);
            } else {
                rentData = {
                   INX: i + 1,
                   STARTDATE: copyHtQsr,
                   ENDDATE: editDetail.dataParam.CONT_END,
                   RENTS: 0,
                   RENTS_JSKL: 0,
                   SUMRENTS: 0
                }
                editDetail.dataParam.CONTRACT_RENT.push(rentData);
                break;
            }
        }
    },

    //分解月度数据
    decompose:function(){},
}

editDetail.showOne = function (data, callback) {
}

editDetail.clearKey = function () {

}

editDetail.IsValidSave = function () {
    var d = new Date(editDetail.dataParam.CONT_START);
    editDetail.dataParam.CONT_START =formatDate(editDetail.dataParam.CONT_START);

    var d = new Date(editDetail.dataParam.CONT_END);
    editDetail.dataParam.CONT_END = formatDate(editDetail.dataParam.CONT_END);

    return true;
}


editDetail.showOne = function (data, callback) {
    _.Ajax('SearchContract', {
        Data: { CONTRACTID: data }
    }, function (data) {
        $.extend(editDetail.dataParam, data.contract);
        editDetail.dataParam.BILLID = data.contract.CONTRACTID;
        editDetail.dataParam.CONTRACT_BRAND = data.contractBrand;
        editDetail.dataParam.CONTRACT_SHOP = data.contractShop;
        callback && callback(data);
    });
}

function formatDate( date, isfull ) {
    if ( !date )
        return '';
    var d = new Date(date);
    if (!isfull){
        return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
    }
}


function getYears(date1, date2) { //获取两个年份之差
    var years = date2.getFullYear() - date1.getFullYear();
    return years;
};

function getNextYears(date) { //获取当前日前的下一年上一天
    var tomYear = new Date(date);
    tomYear.setFullYear(tomYear.getFullYear() + 1); //下一年的今天
    tomYear.setDate(tomYear.getDate() - 1); //下一年的昨天
    return formatDate(tomYear);
};

function addDate(date, days) {
    if (days == undefined || days == '') {
        days = 1;
    }
    var lastDay = new Date(date); //日前复制防止原来日期发生变化
    lastDay.setDate(lastDay.getDate() + days); //日期加天数
    return lastDay;
};
