Vue.component('Merchant', {
    template: '<div>' +
                    '<row>' +
                        '<i-col span="3" class="RowTitle">' +
                           '商户编号' +
                        '</i-col>' +
                        '<i-col span="4">' +
                           '<i-input v-model="MERCHANTID"></i-input>' +
                        '</i-col>' +
                        '<i-col span="3" class="RowTitle">' +
                           '商户名称' +
                        '</i-col>' +
                        '<i-col span="4">' +
                           '<i-input v-model="NAME"></i-input>' +
                        '</i-col>' +
                    '</row>' +
                    '<row>' +
                        '<i-table border highlight-row  ref="currentRowTable" :columns="Column" :data="Data" height=300></i-table>' +
                    '</row>' +
                    '<row>' +
                        '<i-col span="9">' +
                            '      ' +
                        '</i-col>' +
                        '<i-col span="2">' +
                            '<i-button v-on:click="cx">查询</i-button>' +
                        '</i-col>' +
                        '<i-col span="2">' +
                            '<i-button v-on:click="qk">清空</i-button>' +
                        '</i-col>' +
                        '<i-col span="2">' +
                            '<i-button v-on:click="qr">确认</i-button>' +
                        '</i-col>' +
                    '</row>' +
	           '</div>',
    data: function () {
        return {
            MERCHANTID: '',
            NAME: '',
            Column: [
                { type: 'selection', width: 60, align: 'center' },
                { title: '商户代码', key: 'MERCHANTID', width: 150 },
                { title: '商户名称', key: 'NAME', width: 150 },
            ],
            Data: [],
        }
    },
    created: function () {
        this.Data = [];
    },
    methods: {
        qr: function () {
            var data = {};
            data.sj = this.$refs.currentRowTable.getSelection();
            this.Data = [];
            this.$refs.currentRowTable.makeObjData();
            this.$emit('setdialog', data)
        },
        cx: function () {
            _self = this;
            this.Data = [];
            _.Search({
                Service: "ShglService",
                Method: "GetMerchant",
                Data: {
                    MERCHANTID: _self.MERCHANTID,
                    NAME: _self.NAME
                },
                Success: function (data) {
                    if (data.rows.length != 0) {
                        Vue.set(_self, "Data", data.rows);
                    } else {
                        iview.Message.info("没有满足条件的记录");
                    };
                }
            });
        },
        qk: function () {
            this.Data = [];
        },
    },
    props: {
        parenttochild: {
            type: Object,
            required: true
        }
    }
})