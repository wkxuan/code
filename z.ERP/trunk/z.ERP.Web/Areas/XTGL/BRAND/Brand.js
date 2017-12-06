var Search = new Vue({
    el: "#List_Main",
    data: {
        SearchData: {
            NAME:''
        },
        colBRAND: [
            { title: '代码', key: 'CODE' },
            { title: '名称', key: 'NAME' },
            { title: '业态代码', key: 'CATEGORYCODE' },
            { title: '业态名称', key: 'CATEGORYNAME' },
            { title: '地址', key: 'ADRESS' },
            { title: '联系人', key: 'CONTACTPERSON' },
            { title: '电话', key: 'PHONENUM' },
            { title: '邮编', key: 'PIZ' },
            { title: '微信', key: 'WEIXIN' },
            { title: 'QQ', key: 'QQ' }                       
        ],
        dataBRAND: [],
        frameTar: 'tj',
    },
    methods: {
        Search: function (event) {
            event.stopPropagation();
            _.Search({
                Service: 'TestService',
                Method: 'GetBrandData',
                Data: Search.SearchData,
                Success: function (data) {
                    if (data.total == 0)
                    {
                        Search.frameTar = 'tj';
                        Search.dataBRAND = [];
                        alert("查询无结果！");
                    }
                    else
                    {
                        Search.frameTar = 'jg';
                        Search.dataBRAND = data.rows;
                    }

                }
            })
        },
        Clear: function (event) {
            event.stopPropagation();
            Search.frameTar = 'tj';
            Search.SearchData = {};
            Search.dataBRAND = [];
        },
        Save: function (event) {
            event.stopPropagation();
            _.Ajax('Save', {
                BRAND: Search.SearchData
            }, function (a,b,c) {
                alert('成功！');
                _.Search({
                    Service: 'TestService',
                    Method: 'GetBrandData',
                    Data: Search.SearchData,
                    Success: function (data) {
                        Search.frameTar = 'jg';
                        Search.dataBRAND = data.rows;
                    }
                })
            })
        }
    }
}) 