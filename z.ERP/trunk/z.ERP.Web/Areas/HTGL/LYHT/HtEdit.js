editDetail.beforeVue = function () {

    editDetail.others = true;
    editDetail.branchid = true;
    editDetail.service = "HtglService";
    editDetail.method = "GetContract";
    editDetail.Key = 'CONTRACTID';
    editDetail.dataParam.STATUS = 1;
    editDetail.dataParam.JXSL = 0.17;
    editDetail.dataParam.XXSL = 0.17;
    editDetail.dataParam.CONT_START=formatDate(new Date());
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

    //扣率组
    editDetail.screenParam.colGroup=[
        { type: 'selection', width: 60, align: 'center',},
        { title: '扣点序号', key: 'GROUPNO', width: 100 },
        {
            title: "基础扣点", key: 'JSKL', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.JSKL
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.CONTRACT_GROUP[params.index].JSKL = event.target.value;
                        }
                    },
                })
            },           
        },

        {
            title: "描述", key: 'DESCRIPTION', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.DESCRIPTION
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.CONTRACT_GROUP[params.index].DESCRIPTION = event.target.value;
                        }
                    },
                })
            },           
        },
    ]

    //扣率信息

    editDetail.screenParam.colDefJskl=[
        { title: '扣点序号', key: 'GROUPNO', width: 100 },
        { title: '时间段', key: 'INX', width: 80 },
        { title: '开始日期', key: 'STARTDATE',width: 150},
        { title: '结束日期', key: 'ENDDATE', width: 150},
        {
            title: "扣点", key: 'JSKL', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.JSKL
                    },
                    on: {
                        'on-blur': function (event) {
                            editDetail.dataParam.CONTJSKL[params.index].JSKL = event.target.value;
                        }
                    },
                })
            },           
        },
    ]

    //保底表格
    editDetail.screenParam.colDefRENT = [
       { type: 'selection', width: 60, align: 'center',},
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
                        'on-blur': function (event) {
                            editDetail.dataParam.CONTRACT_RENT[params.index].RENTS = event.target.value;
                        }
                    },
                })
            },           
        },
        {
            title: "保底扣点", key: 'RENTS_JSKL', width: 120,
            render: function (h, params) {
                return h('Input', {
                    props: {
                        value: params.row.RENTS_JSKL
                    },
                    on: {
                        'on-blur': function (event) {
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
        //{  title: '清算标记', key: 'QSBJ',width: 150,
        //   render: function(h, params){
        //    return h('Select',{
        //        props:{
        //            value:params.row.QSBJ
        //        },
        //        on:{
        //            'on-change':function(event){
        //                Vue.set(editDetail.dataParam.CONTRACT_RENTITEM[params.index], 'QSBJ', event);
        //            }
        //        }},
        //        [ h('Option', {props: {value: '1'} }, '√'),
        //          h('Option', {props: {value: '2'} }, '')
        //        ],
        //    )}
        //},
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
            INX: 1,
            STARTDATE:editDetail.dataParam.CONT_START
        }]
    };

    if (!editDetail.dataParam.CONTRACT_RENTITEM) {
        editDetail.dataParam.CONTRACT_RENTITEM = [{
            INX: 1
        }]
    }; 

    if (!editDetail.dataParam.CONTRACT_GROUP) {
        editDetail.dataParam.CONTRACT_GROUP = [{
            GROUPNO:1
        }]
    };

    if (!editDetail.dataParam.CONTJSKL) {
        editDetail.dataParam.CONTJSKL = [{
            GROUPNO: 1,
            INX:1
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
        //判断是否存在并且结束日期是否有值
        if( editDetail.dataParam.CONTRACT_RENT){
            for(var i=0;i<editDetail.dataParam.CONTRACT_RENT.length;i++){
                if(!editDetail.dataParam.CONTRACT_RENT[i].ENDDATE){
                    iview.Message.info("请先维护结束日期!"); 
                    return;
                }
            }
        }
        var temp = editDetail.dataParam.CONTRACT_RENT || [];
        var maxINX=1;
        var maxSTARTDATE=null;
        if (editDetail.dataParam.CONTRACT_RENT){
            for (var j = 0; j < editDetail.dataParam.CONTRACT_RENT.length; j++) {
                maxINX = editDetail.dataParam.CONTRACT_RENT[0].INX;
                maxSTARTDATE=editDetail.dataParam.CONTRACT_RENT[0].ENDDATE;
                if (editDetail.dataParam.CONTRACT_RENT[j].INX>maxINX){
                    maxINX=editDetail.dataParam.CONTRACT_RENT[j].INX
                }
                if (editDetail.dataParam.CONTRACT_RENT[j].ENDDATE>maxSTARTDATE){
                    maxSTARTDATE=editDetail.dataParam.CONTRACT_RENT[j].ENDDATE
                }
                maxINX++;
            }
        }
        temp.push({INX:maxINX,STARTDATE:formatDate(addDate(maxSTARTDATE))});
        editDetail.dataParam.CONTRACT_RENT = temp;
    },

    //删除一行保底
    delColDefRENT:function(){
        var selectton = this.$refs.selectRent.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的数据!");
        }else{
            var maxRentInx=1;
            for (var j = 0; j < editDetail.dataParam.CONTRACT_RENT.length; j++) {
                if (editDetail.dataParam.CONTRACT_RENT[j].INX>maxRentInx){
                    maxRentInx=editDetail.dataParam.CONTRACT_RENT[j].INX;
                }
            }

            for(var i=0;i<selectton.length;i++){
                if(selectton[i].INX<maxRentInx){
                    iview.Message.info("只能按照时间段序号从大到小删除!");
                    return;
                }
            }

            for(var i=0;i<selectton.length;i++){
                if(selectton[i].STARTDATE==formatDate(editDetail.dataParam.CONT_START)){
                    iview.Message.info("开始日期和租约开始日期相同不能删除!");
                    return;
                }
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
                   ENDDATE: formatDate(editDetail.dataParam.CONT_END),
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
    decompose:function(){
        if(editDetail.dataParam.CONTRACT_RENT.length==0){
            iview.Message.info("请先维护时间段信息!");
            return;
        }
    },

    //添加扣率组
    addColGroup:function(){
        var temp = editDetail.dataParam.CONTRACT_GROUP || [];
        var maxGROUPNO=1;
        if (editDetail.dataParam.CONTRACT_GROUP){
            for (var j = 0; j < editDetail.dataParam.CONTRACT_GROUP.length; j++) {
                maxGROUPNO = editDetail.dataParam.CONTRACT_GROUP[0].GROUPNO;
                if (editDetail.dataParam.CONTRACT_GROUP[j].GROUPNO>maxGROUPNO){
                    maxGROUPNO=editDetail.dataParam.CONTRACT_GROUP[j].GROUPNO
                }
                maxGROUPNO++;
            }
        }
        temp.push({GROUPNO:maxGROUPNO});
        editDetail.dataParam.CONTRACT_GROUP = temp;

    },
    //删除扣率组
    delColGroup:function(){
        var selectton = this.$refs.selectGroup.getSelection();
        if (selectton.length == 0) {
            iview.Message.info("请选中要删除的扣点组!");
        }else{
            for(var i=0;i<selectton.length;i++){
                for (var j = 0; j < editDetail.dataParam.CONTRACT_GROUP.length; j++) {
                    if (editDetail.dataParam.CONTRACT_GROUP[j].GROUPNO==selectton[i].GROUPNO){
                        editDetail.dataParam.CONTRACT_GROUP.splice(j, 1);
                    }
                }
            }
        }
    },
    //自动生成扣率信息
    autoMakeGroup:function(){
        editDetail.dataParam.CONTJSKL = [];
        if(editDetail.dataParam.CONTRACT_RENT.length==0){
            iview.Message.info("请先维护时间段信息!");
            return;
        }
        if(editDetail.dataParam.CONTRACT_GROUP.length==0){
            iview.Message.info("请先维护扣点组信息!");
            return;
        }

        if( editDetail.dataParam.CONTRACT_RENT){
            for(var i=0;i<editDetail.dataParam.CONTRACT_RENT.length;i++){
                if(!editDetail.dataParam.CONTRACT_RENT[i].ENDDATE){
                    iview.Message.info("请先维护时间段完整结束日期!");
                    return;
                }
                if(!editDetail.dataParam.CONTRACT_RENT[i].STARTDATE){
                    iview.Message.info("请先维护时间段完整开始日期!");
                    return;
                }
            }
        }
        //先循环时间段信息,再循环扣点组信息
        for(var i=0;i<editDetail.dataParam.CONTRACT_RENT.length;i++){
            for(j=0;j<editDetail.dataParam.CONTRACT_GROUP.length;j++){
                editDetail.dataParam.CONTJSKL.push({
                    GROUPNO:editDetail.dataParam.CONTRACT_GROUP[j].GROUPNO,
                    INX:editDetail.dataParam.CONTRACT_RENT[i].INX,
                    STARTDATE:editDetail.dataParam.CONTRACT_RENT[i].STARTDATE,
                    ENDDATE:formatDate(editDetail.dataParam.CONTRACT_RENT[i].ENDDATE),
                    JSKL:editDetail.dataParam.CONTRACT_GROUP[j].JSKL
                });
            }
        }
    }
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

        editDetail.dataParam.CONTRACT_RENT = data.contract_rent;
        editDetail.dataParam.CONTRACT_GROUP = data.contract_group;
        editDetail.dataParam.CONTJSKL = data.contract_jskl;
        editDetail.dataParam.CONTRACT_RENTITEM = data.contract_rentitem;
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
