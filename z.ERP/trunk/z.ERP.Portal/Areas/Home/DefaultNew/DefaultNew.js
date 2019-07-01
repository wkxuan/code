var DefaultNew = new Vue({
    el: '#main',
    data: {
        box1colDef: [
                { title: ' ', key: 'TIME', },
                {
                    title: '营业额(元)', key: 'AMOUNT', 
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNT.toFixed(2));
                    }
                },
                {
                    title: '同比(%)', key: 'AMOUNTHB',
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNTHB.toFixed(2));
                    }
                },
                {
                    title: '环比(%)', key: 'AMOUNTTB', 
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNTTB.toFixed(2));
                    }
                }, ],
        box1dataDef: [],
        box2colDef: [
                { title: ' ', key: 'TYPE',},
                { title: '店铺数量(个)', key: 'NUMBERS', },
                {
                    title: '租赁面积(m²)', key: 'AREA', 
                    render: function (h, params) {
                        return h('div',
                            this.row.AREA.toFixed(2));
                    }
                }, ],
        box2dataDef: [],
        box3colDef: [
                { title: ' ', key: 'NO', width: 60 },
                { title: '品牌名称', key: 'SHOPNAME', tooltip: true },
                {
                    title: '租赁面积(m²)', key: 'AREA', width: 150,
                    render: function (h, params) {
                        return h('div',
                            this.row.AREA.toFixed(2));
                    }
                },
                {
                    title: '营业额(元)', key: 'AMOUNT', width: 150,
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNT.toFixed(2));
                    }
                }, ],
        box3dataDef: [],
        box4colDef: [
                { title: ' ', key: 'NO', width: 60 },
                { title: '业态名称', key: 'AREANAME', tooltip: true },
                {
                    title: '租赁面积(m²)', key: 'AREA', width: 150,
                    render: function (h, params) {
                        return h('div',
                            this.row.AREA.toFixed(2));
                    }
                },
                {
                    title: '营业额(元)', key: 'AMOUNT', width: 150,
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNT.toFixed(2));
                    }
                }, ],
        box4dataDef: [],
        tableH: 200,
        Switch: true,
        Echart1Ydata: [],
        Echart1Xdata: [],
        Echart2Numberdata: [],
        Echart2Areadata: [],
        Echart3Ydata: [],
        Echart3Xdata: [],
    },
    mounted: function () {
        var type = '1';   //初始化默认昨日数据
        _.Ajax('BoxData', {
            type:type
        }, function (data) {
            DefaultNew.box1dataDef = data.box1data;
            DefaultNew.box2dataDef = data.box2data;
            DefaultNew.box3dataDef = data.box3data;
            DefaultNew.box4dataDef = data.box4data;
            DefaultNew.Echart1Xdata = data.Echart1Xdata;
            DefaultNew.Echart1Ydata = data.Echart1Ydata;
            DefaultNew.Echart2Areadata = data.Echart2Areadata;
            DefaultNew.Echart2Numberdata = data.Echart2Numberdata;
            DefaultNew.Echart3Xdata = data.Echart3Xdata;
            DefaultNew.Echart3Ydata = data.Echart3Ydata;
            DefaultNew.createEchart();    //获取数据后再注册echart
        });
        
    },
    methods: {
        //根据日期筛选数据
        box3datechange: function (event) {
            var type = event;
            _.Ajax('Box3Data', {
                type: type
            }, function (data) {
                DefaultNew.box3dataDef = data;
            });
        },
        box4datechange: function (event) {
            var type = event;
            _.Ajax('Box4Data', {
                type: type
            }, function (data) {
                DefaultNew.box4dataDef = data;
            });
        },
        //echart模型
        createEchart: function () {
            //业态经营榜
            var echart1 = echarts.init(document.getElementById('echart1'), 'macarons');
            echart1.setOption({
                title: {
                    text: '品牌经营榜',
                    subtext: '昨日销售额前15名',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'axis'
                },
                xAxis: [
                    {
                        name: '金额',
                        type: 'value',
                        boundaryGap: [0, 0.01]
                    }
                ],
                yAxis: [
                    {
                        name:'品牌名称',
                        type: 'category',
                        data: this.Echart1Ydata,
                        axisLabel: {
                            interval: 0, 
                            rotate: "45",
                        }
                    }
                ],
                series: [
                    {
                        name: '金额',
                        type: 'bar',
                        itemStyle: {
                            normal: {
                                color: function (params) {
                                    var colorList = [
                                      '#26C0C0', '#B5C334', '#FCCE10', '#E87C25', '#27727B',
                                       '#FE8463', '#9BCA63', '#FAD860', '#F3A43B', '#60C0DD',
                                       '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#C1232B'
                                    ];
                                    return colorList[params.dataIndex]
                                },
                                label: { show: true, position: 'inside' }
                            }
                        },
                        data: this.Echart1Xdata
                    }
                ]
            });
            //店铺状态，饼图
            var echart2 = echarts.init(document.getElementById('echart2'), 'macarons');
            echart2.setOption({
                title: {
                    text: '店铺状态',
                    subtext: '左：铺位个数  右：铺位面积',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    x: 'center',
                    y: 'bottom',
                    data: ['闲置招租', '正在经营']
                },
                calculable: true,
                series: [
                    {
                        name: '铺位模式',
                        type: 'pie',
                        radius: [20, 70],
                        center: ['25%', 150],
                        width: '40%',       // for funnel
                        max: 40,            // for funnel
                        data: this.Echart2Numberdata
                    },
                    {
                        name: '面积模式',
                        type: 'pie',
                        radius: [30, 70],
                        center: ['75%', 150],
                        x: '50%',               // for funnel
                        max: 40,                // for funnel
                        sort: 'ascending',     // for funnel
                        data: this.Echart2Areadata
                    }
                ]
            });

            //经营总况
            var echart3 = echarts.init(document.getElementById('echart3'), 'macarons');
            echart3.setOption({
                title: {
                    text: '经营总况',
                    subtext: '前30天数据',
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
                        data: this.Echart3Xdata
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
                        data: this.Echart3Ydata
                    }
                ]
            });
        },
        
    }
})