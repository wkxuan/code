var period = new Vue({
    el: "#List_Main",
    data: {
        jzqj: { LASTDAY: '', Natrual: false },
        colDef: [{ title: '年月',key: 'YEARMONTH', width: 149,},
                 { title: '开始日期', key: 'DATE_START', width: 150, },
                 { title: '结束日期', key: 'DATE_END', width: 150, }, ],
        dataDef: [],
        frameTar: 'tj',
        disabled: true,
        Year: '',
        nyList: [],
    },
    mounted: function () {
        var date = new Date;
        var curYear = date.getFullYear();
        for (var minYear = curYear - 10; minYear <= curYear + 40; minYear++) {
            this.nyList.push({ value: minYear, label: minYear });
        }
    },
    methods: {
        
        mod: function (event) {
            if (!period.Year) {
                this.$Message.info({
                    title: '提示',
                    content: '请选择年度!'
                });
                return;
            }
            frameTar: 'tj',
            period.disabled = false;
        },
        save: function (event) {
            event.stopPropagation();
            _.Ajax('Save', {
                listPeriod: period.dataDef
            }, function (a, b, c) {
                alert('成功！');
                period.disabled = true;
            })
        },
        quit: function (event) {
            frameTar: 'tj',
            period.disabled = true;
        },
        change: function ()
        {
            var year = period.Year;
            if (!year) {
                this.$Message.info({
                    title: '提示',
                    content: '请选择年度!'
                });
                return;
            }
            var itemList = [];

            if (period.jzqj.Natrual)
            {
                var februaryDay = 28;
                if ((year % 4 == 0 && year % 100 != 0) || (year % 4 == 0 && year % 400 == 0)) {
                    februaryDay = 29;
                }
                itemList.push({ YEARMONTH: year * 100 + 1, DATE_START: (year + '-' + 01 + '-' + 01), DATE_END: (year + '-' + 01 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 2, DATE_START: (year + '-' + 02 + '-' + 01), DATE_END: (year + '-' + 02 + '-' + februaryDay) });
                itemList.push({ YEARMONTH: year * 100 + 3, DATE_START: (year + '-' + 03 + '-' + 01), DATE_END: (year + '-' + 03 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 4, DATE_START: (year + '-' + 04 + '-' + 01), DATE_END: (year + '-' + 04 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 5, DATE_START: (year + '-' + 05 + '-' + 01), DATE_END: (year + '-' + 05 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 6, DATE_START: (year + '-' + 06 + '-' + 01), DATE_END: (year + '-' + 06 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 7, DATE_START: (year + '-' + 07 + '-' + 01), DATE_END: (year + '-' + 07 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 8, DATE_START: (year + '-' + 08 + '-' + 01), DATE_END: (year + '-' + 08 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 9, DATE_START: (year + '-' + 09 + '-' + 01), DATE_END: (year + '-' + 09 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 10, DATE_START: (year + '-' + 10 + '-' + 01), DATE_END: (year + '-' + 10 + '-' + 31) });
                itemList.push({ YEARMONTH: year * 100 + 11, DATE_START: (year + '-' + 11 + '-' + 01), DATE_END: (year + '-' + 11 + '-' + 30) });
                itemList.push({ YEARMONTH: year * 100 + 12, DATE_START: (year + '-' + 12 + '-' + 01), DATE_END: (year + '-' + 12 + '-' + 31) });
                //itemList.push({ YEARMONTH: year * 100 + 1, DATE_START: new Date(year, 0, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 0, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 2, DATE_START: new Date(year, 1, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 1, februaryDay).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 3, DATE_START: new Date(year, 2, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 2, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 4, DATE_START: new Date(year, 3, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 3, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 5, DATE_START: new Date(year, 4, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 4, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 6, DATE_START: new Date(year, 5, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 5, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 7, DATE_START: new Date(year, 6, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 6, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 8, DATE_START: new Date(year, 7, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 7, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 9, DATE_START: new Date(year, 8, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 8, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 10, DATE_START: new Date(year, 9, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 9, 31).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 11, DATE_START: new Date(year, 10, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 10, 30).format("yyyy-MM-dd") });
                //itemList.push({ YEARMONTH: year * 100 + 12, DATE_START: new Date(year, 11, 1).format("yyyy-MM-dd"), DATE_END: new Date(year, 11, 31).format("yyyy-MM-dd") });

                period.dataDef = itemList;
            }
            else
            {
                var lastday = period.jzqj.LASTDAY;
                if (!lastday) {
                    this.$Message.info({
                        title: '提示',
                        content: '非自然月请确认每月结束日!'
                    });
                    return;
                }
                if (lastday > 28) {
                    this.$Message.info({
                        title: '提示',
                        content: '非自然月每月结束日不能大于28!'
                    });
                    return;
                }                
                var firstday = lastday*1 + 1;
                itemList.push({ YEARMONTH: year * 100 + 1, DATE_START: (year-1 + '-' + 12 + '-' + firstday), DATE_END: (year + '-' + 01 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 2, DATE_START: (year + '-' + 01 + '-' + firstday), DATE_END: (year + '-' + 02 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 3, DATE_START: (year + '-' + 02 + '-' + firstday), DATE_END: (year + '-' + 03 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 4, DATE_START: (year + '-' + 03 + '-' + firstday), DATE_END: (year + '-' + 04 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 5, DATE_START: (year + '-' + 04 + '-' + firstday), DATE_END: (year + '-' + 05 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 6, DATE_START: (year + '-' + 05 + '-' + firstday), DATE_END: (year + '-' + 06 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 7, DATE_START: (year + '-' + 06 + '-' + firstday), DATE_END: (year + '-' + 07 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 8, DATE_START: (year + '-' + 07 + '-' + firstday), DATE_END: (year + '-' + 08 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 9, DATE_START: (year + '-' + 08 + '-' + firstday), DATE_END: (year + '-' + 09 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 10, DATE_START: (year + '-' + 09 + '-' + firstday), DATE_END: (year + '-' + 10 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 11, DATE_START: (year + '-' + 10 + '-' + firstday), DATE_END: (year + '-' + 11 + '-' + lastday) });
                itemList.push({ YEARMONTH: year * 100 + 12, DATE_START: (year + '-' + 11 + '-' + firstday), DATE_END: (year + '-' + 12 + '-' + lastday) });

                period.dataDef= itemList;
            }

            
        },

        getPeriod:function(){
            var year = period.Year;
            var data = { YEAR: year };
            _.Search({
                Service: 'XtglService',
                Method: 'GetPeriod',
                Data: data,
                Success: function (data) {
                    
                    period.dataDef = data.rows;
                }
            })
            period.frameTar = 'tj';
        }
    }
})