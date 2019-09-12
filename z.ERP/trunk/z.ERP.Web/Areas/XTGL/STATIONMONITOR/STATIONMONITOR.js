var DataShow = new Vue({
    el: "#DataShow",
    data: {
        splitVal: 0.2,   //切割面板宽度
        Listdata: [],
        lastrefreshtime: "",                    //数据刷新时间
        cardshow: false,
        colDef: [{ title: "收款方式", key: "PAYNAME" },
                { title: '交易金额', key: 'AMOUNT' }],
        SALEITEM: [],
        branchamount: "",                   //门店总销售
        stationbh: "",
        cashiername: "",                 //收银员
        stationrefreshtime:""          //pos更新时间
    },
    mounted: function () {
        this.initdata();
    },
    methods: {
        initdata: function () {
            _.Ajax('GetInitData', {
                1:1
            }, function (data) {
                DataShow.Listdata = data;
                DataShow.lastrefreshtime = new Date().Format("yyyy-MM-dd hh:mm:ss");
            });
        },
        refreshclick: function () {
            DataShow.cardshow = false;
            DataShow.initdata();
        },
        staionclick: function (id, name,time) {
            DataShow.cardshow = true;
            DataShow.stationbh = id;
            DataShow.cashiername = name;
            DataShow.stationrefreshtime = time;
            _.Ajax('GetSTATIONSALE', {
                stationid: id
            }, function (data) {
                DataShow.branchamount = data.BRANCHAMOUNT.BRANCHAMOUNT;
                DataShow.SALEITEM = data.STATIONPAYDATA;
            });
        }
    },
});
//定时器 2分钟刷新一次数据
var TwoMS = window.setInterval(function () {  
    DataShow.initdata();
}, 1000*60*2)