alert("测试信息11111");

var Search = new Vue({
    el: "#List_Main",
    data: {
        SearchData: {
            DEPTID: '',
            DEPT_NAME: ''
        },
        ResultData: {
            rows: [], 
            total: 0
        },
        frameTar: 'tj', 
    },
    methods: {
        Search: function (event) {
            event.stopPropagation();
            //_.Ajax('Func1', {
            //    s: '111'
            //}, function (a,b,c) {

            //}, function (a, b, c) {

            //});
            _.Search({
                Service: 'TestService',
                Method: 'GetData',
                Data: Search.SearchData,
                Success: function (data) {
                    Search.ResultData = data;
                    Search.frameTar = 'jg';
                }
            });
        },
        Save: function (event) {
            event.stopPropagation();
            _.Ajax('Save1', {
                bm: Search.SearchData
            }, function (a, b, c) {
                alert("成功");
            });
        }
    }
});
