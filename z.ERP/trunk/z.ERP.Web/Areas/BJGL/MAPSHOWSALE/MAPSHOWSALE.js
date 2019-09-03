var echart1;
var echart2;
var SHOPID;
var Mapshow = new Vue({
    el: "#MapShow",
    data: {
        splitVal: 0.2,   //切割面板宽度
        data: [],
        Floorid: "",
        Tabs: "selectfloor",
        DrawerModel: false,      //右弹出抽屉
        tool: false,        //工具栏
        starttime: "",
        endtime: "",
        SHOPINFO: Object,
        salelist: "",
        Radiovalue:"1",
        salelistX: [],
        salelistY: [],
        echart2legend: ['店铺总计', '楼层总计'],
        echart2seriesname: '楼层占比',
        echart2seriesdata:[],
        B_Floorid: "",        //三个参数备份，防止图已生成，参数改变
        B_starttime: "",
        B_endtime: "",
    },
    mounted: function () {
        this.initTree();
    },
    methods: {
        initTree: function () {
            _.SearchNoQuery({
                Service: "DpglService",
                Method: "TreeFloorData",
                Success: function (data) {
                    Mapshow.data = data;
                }
            })
        },
        onselectchange: function (selectArr, node) {
            if (node.parentId == "REGION") {
                Mapshow.Floorid = node.code;
                Mapshow.Tabs = "selecttime";
            }
        },
        searchclick: function () {
            if (isEmpty(Mapshow.Floorid)) {
                iview.Message.info("请选择楼层!");
            } else {
                Mapshow.B_Floorid = Mapshow.Floorid; Mapshow.B_starttime = Mapshow.starttime; Mapshow.B_endtime = Mapshow.endtime; //三个参数备份，防止图已生成，参数改变
                Mapshow.INITMAP();
            }
        },
        INITMAP: function () {
            var $this = this.$refs;    //目标元素
            $this.maps.innerHTML = "";
            _.Ajax('GetInitMAPDATA', {
                floorid: Mapshow.Floorid, starttime: Mapshow.starttime, endtime: Mapshow.endtime
            }, function (data) {
                if (data.floorInfo.MAPSHOPLIST.length > 0 || data.labelArray.length > 0) {
                    ThreeMapInit(data.floorInfo, data.labelArray, $this);
                    Mapshow.salelist = data.salelist;
                    Mapshow.tool = true;
                } else {
                    Mapshow.tool = false;
                    iview.Message.info("暂无布局图数据，请联系管理员!");
                }
            });
        },
        clickli: function (event) {
            ThreeMapClick(event);
        },
        createEchart1: function () {
            //销售历史数据
            if (!isEmpty(echart1)) {
                echart1.dispose();    //清空数据
            }
            $('#echart1').width(window.innerWidth * 0.4-32);
            echart1 = echarts.init(document.getElementById('echart1'), 'macarons');
            echart1.setOption({
                title: {
                    text: '经营情况',
                    subtext: '选择日期内历史数据',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'axis'
                },
                dataZoom: {
                    show: true,
                    realtime: true,
                    start: 20,
                    end: 80
                },
                xAxis: [
                    {
                        name: '日期',
                        type: 'category',
                        boundaryGap: false,
                        data: this.salelistX
                    }
                ],
                yAxis: [
                    {
                        name: '销售金额',
                        type: 'value'
                    }
                ],
                series: [
                    {
                        name: '最高',
                        type: 'line',
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: this.salelistY
                    }
                ],
                //动画效果
                animationDuration: 4000,
                animationDurationUpdate: 1000,
                animationEasing: "bounceIn"
            });
        },
        radiochange: function (event) {
            switch (event) {
                case '1':
                    Mapshow.echart2legend = ['店铺总计', '楼层总计'];
                    Mapshow.echart2seriesname = "楼层占比";
                    Mapshow.TypeChange();
                    break;
                case '2':
                    Mapshow.echart2legend = ['店铺总计', '门店单业态总计'];
                    Mapshow.echart2seriesname = "业态占比";
                    Mapshow.TypeChange();
                    break;
                case '3':
                    Mapshow.echart2legend = ['店铺总计', '门店总计'];
                    Mapshow.echart2seriesname = "门店占比";
                    Mapshow.TypeChange();
                    break;
            }
        },
        TypeChange: function () {
            _.Ajax('GetTypeChange', {
                shopid: SHOPID, starttime: Mapshow.B_starttime, endtime: Mapshow.B_endtime, type: Mapshow.Radiovalue
            }, function (data) {
                Mapshow.echart2seriesdata = data.shopsalepercent;
                Mapshow.createEchart2();
            });
        },
        createEchart2: function () {
            //销售历史数据
            if (!isEmpty(echart2)) {
                echart2.dispose();    //清空数据
            }
            $('#echart2').width(window.innerWidth * 0.4-32);
            echart2 = echarts.init(document.getElementById('echart2'), 'macarons');
            echart2.setOption({
                title: {
                    text: '销售占比',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    x: 'center',
                    y: 'bottom',
                    data: Mapshow.echart2legend
                },
                calculable: true,
                series: [
                    {
                        name: Mapshow.echart2seriesname,
                        type: 'pie',
                        radius: '55%',
                        center: ['50%','60%'],
                        data: Mapshow.echart2seriesdata
                    }
                ],
                //动画效果
                animationDuration: 4000,
                animationDurationUpdate: 1000,
                animationEasing: "backIn"
            });
        },
    },
});
function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}
function ThreeMapClick(id) {
    SHOPID = id;
    _.Ajax('GetSHOPSALE', {
        shopid: id, starttime: Mapshow.B_starttime, endtime: Mapshow.B_endtime, type: Mapshow.Radiovalue
    }, function (data) {
        Mapshow.SHOPINFO = data.shopdata;
        if (Mapshow.SHOPINFO.RENT_STATUS == "1") {
            iview.Message.info("空铺，暂无销售数据!");
        } else {
            Mapshow.DrawerModel = true;
            Mapshow.salelistY = data.shopsalelistY;
            Mapshow.salelistX = data.shopsalelistX;
            Mapshow.echart2seriesdata = data.shopsalepercent;
            Mapshow.createEchart1();
            Mapshow.createEchart2();
        }
    });
};
