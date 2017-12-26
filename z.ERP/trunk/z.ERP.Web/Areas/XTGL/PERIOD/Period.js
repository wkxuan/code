new Vue({
    el: "#List_Main",
    data: {
        jzqj:{CODE:''},
        colDef: [{ title: '年月',key: 'YEARMONTH', width: 149,},
                 { title: '开始日期', key: 'KSRQ', width: 150, },
                 { title: '结束日期', key: 'JSRQ', width: 150, }, ],
        dataDef: [
                    {
                        YEARMONTH: 201801,
                        KSRQ: '2018.1.1',
                        JSRQ: '2018.1.31'
                    },
                    {
                        YEARMONTH: 201802,
                        KSRQ: '2018.2.1',
                        JSRQ: '2018.2.28'
                    },
                    {
                        YEARMONTH: 201803,
                        KSRQ: '2018.3.1',
                        JSRQ: '2018.3.31'
                    },
                    {
                        YEARMONTH: 201804,
                        KSRQ: '2018.4.1',
                        JSRQ: '2018.4.30'
                    },
                    {
                        YEARMONTH: 201805,
                        KSRQ: '2018.5.1',
                        JSRQ: '2018.5.31'
                    },
                    {
                        YEARMONTH: 201806,
                        KSRQ: '2018.6.1',
                        JSRQ: '2018.6.30'
                    },
                    {
                        YEARMONTH: 201807,
                        KSRQ: '2018.1.1',
                        JSRQ: '2018.1.31'
                    },
                    {
                        YEARMONTH: 201808,
                        KSRQ: '2018.2.1',
                        JSRQ: '2018.2.28'
                    },
                    {
                        YEARMONTH: 201809,
                        KSRQ: '2018.3.1',
                        JSRQ: '2018.3.31'
                    },
                    {
                        YEARMONTH: 201810,
                        KSRQ: '2018.4.1',
                        JSRQ: '2018.4.30'
                    },
                    {
                        YEARMONTH: 201811,
                        KSRQ: '2018.5.1',
                        JSRQ: '2018.5.31'
                    },
                    {
                        YEARMONTH: 201812,
                        KSRQ: '2018.6.1',
                        JSRQ: '2018.6.30'
                    },
        ],
        frameTar: 'tj',
    },
    methods: {
        mod: function (event) {

        },
        save: function (event) {
 
        },
        quit: function (event) {

        },
    }
})