var DefaultNew = new Vue({
    el: '#main',
    data: {
        box1colDef: [
                { title: ' ', key: 'TIME', width: 70, },
                {
                    title: '营业额(万元)', key: 'AMOUNT', width: 120,
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNT.toFixed(2));
                    }
                },
                {
                    title: '同比(%)', key: 'AMOUNTHB', width: 100,
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNTHB.toFixed(2));
                    }
                },
                {
                    title: '环比(%)', key: 'AMOUNTTB', width: 100,
                    render: function (h, params) {
                        return h('div',
                            this.row.AMOUNTTB.toFixed(2));
                    }
                }, ],
        box1dataDef: [],
        box2colDef: [
                { title: ' ', key: 'TYPE', width: 100, },
                { title: '店铺数量(个)', key: 'NUMBERS', width: 130, },
                {
                    title: '租赁面积(m²)', key: 'AREA', width: 130,
                    render: function (h, params) {
                        return h('div',
                            this.row.AREA.toFixed(2));
                    }
                }, ],
        box2dataDef: [],
        box3colDef: [
                { title: ' ', key: 'NO', width: 80, },
                { title: '商铺名称', key: 'SHOPNAME', width: 200, },
                { title: '租赁面积(m²)', key: 'AREA', width: 150, },
                { title: '营业额(元)', key: 'AMOUNT', width: 150, }, ],
        box3dataDef: [],
        box6colDef: [
                { title: ' ', key: 'NO', width: 80, },
                { title: '业态名称', key: 'AREANAME', width: 200, },
                { title: '租赁面积(m²)', key: 'AREA', width: 150, },
                { title: '营业额(元)', key: 'AMOUNT', width: 150, }, ],
        box6dataDef: [],
        tableH: 300
    },
    mounted: function () {
        var type = '1';   //初始化默认昨日数据
        _.Ajax('BoxData', {
            type:type
        }, function (data) {
            DefaultNew.box1dataDef = data.box1data;
            DefaultNew.box2dataDef = data.box2data;
            DefaultNew.box3dataDef = data.box3data;
            DefaultNew.box6dataDef = data.box6data;
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
        box6datechange: function (event) {
            var type = event;
            _.Ajax('Box6Data', {
                type: type
            }, function (data) {
                DefaultNew.box6dataDef = data;
            });
        }
    }
})