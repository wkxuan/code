var period = new Vue({
    el: "#List_Main",
    data: {
        jzqj:{CODE:'',XXX:false},
        colDef: [{ title: '年月',key: 'YEARMONTH', width: 149,},
                 { title: '开始日期', key: 'KSRQ', width: 150, },
                 { title: '结束日期', key: 'JSRQ', width: 150, }, ],
        dataDef: [
                    {
                        YEARMONTH: 201801,
                        KSRQ: '2018-1-1',
                        JSRQ: '2018-1-31'
                    },
                    {
                        YEARMONTH: 201802,
                        KSRQ: '2018-2-1',
                        JSRQ: '2018-2-28'
                    },
                    {
                        YEARMONTH: 201803,
                        KSRQ: '2018-3-1',
                        JSRQ: '2018-3-31'
                    },
                    {
                        YEARMONTH: 201804,
                        KSRQ: '2018-4-1',
                        JSRQ: '2018-4-30'
                    },
                    {
                        YEARMONTH: 201805,
                        KSRQ: '2018-5-1',
                        JSRQ: '2018-5-31'
                    },
                    {
                        YEARMONTH: 201806,
                        KSRQ: '2018-6-1',
                        JSRQ: '2018-6-30'
                    },
                    {
                        YEARMONTH: 201807,
                        KSRQ: '2018-1-1',
                        JSRQ: '2018-1-31'
                    },
                    {
                        YEARMONTH: 201808,
                        KSRQ: '2018-2-1',
                        JSRQ: '2018-2-28'
                    },
                    {
                        YEARMONTH: 201809,
                        KSRQ: '2018-3-1',
                        JSRQ: '2018-3-31'
                    },
                    {
                        YEARMONTH: 201810,
                        KSRQ: '2018-4-1',
                        JSRQ: '2018-4-30'
                    },
                    {
                        YEARMONTH: 201811,
                        KSRQ: '2018-5-1',
                        JSRQ: '2018-5-31'
                    },
                    {
                        YEARMONTH: 201812,
                        KSRQ: '2018-6-1',
                        JSRQ: '2018-6-30'
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
            frameTar: 'tj',
            period.dataDef = [];
            period.dataDef = [
                    {
                        YEARMONTH: 201801,
                        KSRQ: '2018-1-1',
                        JSRQ: '2018-1-31'
                    },
            ]
        },
        change: function ()
        {
            var year = 2018;
            var itemList = [];

            if (period.jzqj.XXX)
            {
                var februaryDay = 28;
                if ((year % 4 == 0 && year % 100 != 0) || (year % 4 == 0 && year % 400 == 0)) {
                    februaryDay = 29;
                }
                
                itemList.push({ YEARMONTH: year * 100 + 1, KSRQ: (year + '-' + 01 + '-' + 01), JSRQ: (year + '-' + 01 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 2, KSRQ: (year + '-' + 02 + '-' + 01), JSRQ: (year + '-' + 02 + '-' + februaryDay) });
                itemList.push({ YEARMONTH: year * 100 + 3, KSRQ: (year + '-' + 03 + '-' + 01), JSRQ: (year + '-' + 03 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 4, KSRQ: (year + '-' + 04 + '-' + 01), JSRQ: (year + '-' + 04 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 5, KSRQ: (year + '-' + 05 + '-' + 01), JSRQ: (year + '-' + 05 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 6, KSRQ: (year + '-' + 06 + '-' + 01), JSRQ: (year + '-' + 06 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 7, KSRQ: (year + '-' + 07 + '-' + 01), JSRQ: (year + '-' + 07 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 8, KSRQ: (year + '-' + 08 + '-' + 01), JSRQ: (year + '-' + 08 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 9, KSRQ: (year + '-' + 09 + '-' + 01), JSRQ: (year + '-' + 09 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 10, KSRQ: (year + '-' + 10 + '-' + 01), JSRQ: (year + '-' + 10 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 11, KSRQ: (year + '-' + 11 + '-' + 01), JSRQ: (year + '-' + 11 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 12, KSRQ: (year + '-' + 12 + '-' + 01), JSRQ: (year + '-' + 12 + '-' + 31) });
                //itemList.push({ YEARMONTH: year * 100 + 1, KSRQ: new Date(year, 0, 1), JSRQ: new Date(year, 0, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 2, KSRQ: new Date(year, 1, 1), JSRQ: new Date(year, 1, 28) });
                //itemList.push({ YEARMONTH: year * 100 + 3, KSRQ: new Date(year, 2, 1), JSRQ: new Date(year, 2, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 4, KSRQ: new Date(year, 3, 1), JSRQ: new Date(year, 3, 30) });
                //itemList.push({ YEARMONTH: year * 100 + 5, KSRQ: new Date(year, 4, 1), JSRQ: new Date(year, 4, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 6, KSRQ: new Date(year, 5, 1), JSRQ: new Date(year, 5, 30) });
                //itemList.push({ YEARMONTH: year * 100 + 7, KSRQ: new Date(year, 6, 1), JSRQ: new Date(year, 6, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 8, KSRQ: new Date(year, 7, 1), JSRQ: new Date(year, 7, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 9, KSRQ: new Date(year, 8, 1), JSRQ: new Date(year, 8, 30) });
                //itemList.push({ YEARMONTH: year * 100 + 10, KSRQ: new Date(year, 9, 1), JSRQ: new Date(year, 9, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 11, KSRQ: new Date(year, 10, 1), JSRQ: new Date(year, 10, 30) });
                //itemList.push({ YEARMONTH: year * 100 + 12, KSRQ: new Date(year, 11, 1), JSRQ: new Date(year, 11, 31) });
                //itemList.push({ YEARMONTH: year * 100 + 1, KSRQ: new Date(year, 0, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 0, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 2, KSRQ: new Date(year, 1, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 1, februaryDay).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 3, KSRQ: new Date(year, 2, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 2, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 4, KSRQ: new Date(year, 3, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 3, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 5, KSRQ: new Date(year, 4, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 4, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 6, KSRQ: new Date(year, 5, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 5, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 7, KSRQ: new Date(year, 6, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 6, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 8, KSRQ: new Date(year, 7, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 7, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 9, KSRQ: new Date(year, 8, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 8, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 10, KSRQ: new Date(year, 9, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 9, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 11, KSRQ: new Date(year, 10, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 10, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 12, KSRQ: new Date(year, 11, 1).format("yyyy-MM-dd"), JSRQ: new Date(year, 11, 31).format("yyyy-MM-dd") });

                period.dataDef = itemList;
            }
            else
            {
                var lastday = 12;
                var firstday = lastday - 1;
                itemList.push({ YEARMONTH: year * 100 + 1, KSRQ: (year + '-' + 01 + '-' + 01), JSRQ: (year + '-' + 01 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 2, KSRQ: (year + '-' + 02 + '-' + firstday), JSRQ: (year + '-' + 02 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 3, KSRQ: (year + '-' + 03 + '-' + firstday), JSRQ: (year + '-' + 03 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 4, KSRQ: (year + '-' + 04 + '-' + firstday), JSRQ: (year + '-' + 04 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 5, KSRQ: (year + '-' + 05 + '-' + firstday), JSRQ: (year + '-' + 05 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 6, KSRQ: (year + '-' + 06 + '-' + firstday), JSRQ: (year + '-' + 06 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 7, KSRQ: (year + '-' + 07 + '-' + firstday), JSRQ: (year + '-' + 07 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 8, KSRQ: (year + '-' + 08 + '-' + firstday), JSRQ: (year + '-' + 08 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 9, KSRQ: (year + '-' + 09 + '-' + firstday), JSRQ: (year + '-' + 09 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 10, KSRQ: (year + '-' + 10 + '-' + firstday), JSRQ: (year + '-' + 10 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 11, KSRQ: (year + '-' + 11 + '-' + firstday), JSRQ: (year + '-' + 11 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 12, KSRQ: (year + '-' + 12 + '-' + firstday), JSRQ: (year + '-' + 12 + '-' + 31) });

                period.dataDef= itemList;
            }

            
        }
    }
})