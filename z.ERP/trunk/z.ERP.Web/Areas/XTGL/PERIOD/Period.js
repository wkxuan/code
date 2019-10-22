define.beforeVue = function () {
    define.screenParam.colDef = [
        { title: '年月', key: 'YEARMONTH' },
        { title: '开始日期', key: 'DATE_START' },
        { title: '结束日期', key: 'DATE_END' }];

    define.screenParam.nyList = [];
    var date = new Date;
    var curYear = date.getFullYear();
    define.searchParam.YEAR = curYear;
    for (var i = curYear - 10; i <= curYear + 40; i++) {
        define.screenParam.nyList.push({ value: i, label: i });
    };
    define.screenParam.Natrual = false;
    define.screenParam.LASTDAY = "";
    define.splitVal = 0.5;
    define.service = "XtglService";
    define.methodList = "GetPeriod";
}
define.showOne = function () { };
define.mountedInit = function () {
    define.btnConfig = [{
        id: "edit",
        fun: function () {
            if (!define.searchParam.YEAR) {
                iview.Message.info("请选择年度!");
                return;
            }
            define.vueObj.disabled = false;
        },
        enabled: function (disabled, data) {
            return disabled
        }
    }, {
        id: "save",
        fun: function () {
            _.Ajax('Save', {
                listPeriod: define.vueObj.data
            }, function (a, b, c) {
                iview.Message.info("保存成功!");
            })
        },
        enabled: function (disabled, data) {
            return !disabled
        }
    }, {
        id: "abandon",
        fun: function () {
            _.MessageBox("是否取消？", function () {
                define.otherMethods.getPeriod();
                define.vueObj.disabled = true;
                define.screenParam.Natrual = false;
                define.screenParam.LASTDAY = "";
            }); 
        }
    }];
}
define.otherMethods = {
    getPeriod: function () {
        _.Search({
            Service: 'XtglService',
            Method: 'GetPeriod',
            Data: define.searchParam,
            Success: function (data) {
                define.vueObj.data = data.rows;
            }
        })
    },
    natrualChange: function () {
        this.generateData();
    },
    lastdayChange: function () {
        this.generateData();
    },
    generateData: function () {
        var year = define.searchParam.YEAR;
        if (!year) {
            iview.Message.info("请选择年度!");
            return;
        }
        var itemList = [];
        if (define.screenParam.Natrual) {
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
        } else {
            var lastday = define.screenParam.LASTDAY;
            if (!lastday) {
                iview.Message.info("非自然月请确认每月结束日!");
                return;
            }
            if (lastday > 28) {
                iview.Message.info("非自然月每月结束日不能大于28!");
                return;
            }
            var firstday = lastday * 1 + 1;
            itemList.push({ YEARMONTH: year * 100 + 1, DATE_START: (year - 1 + '-' + 12 + '-' + firstday), DATE_END: (year + '-' + 01 + '-' + lastday) });
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
        }
        define.vueObj.data = itemList;
    }
};