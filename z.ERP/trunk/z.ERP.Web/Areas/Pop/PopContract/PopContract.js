Vue.component('Contract', {
    template: '<div>' +
                    '<row>' +
                        '<i-col span="2" class="RowTitle">' +
                           '租约号' +
                        '</i-col>' +
                        '<i-col span="4">' +
                           '<i-input v-model="CONTRACTID"></i-input>' +
                        '</i-col>' +
                        '<i-col span="2" class="RowTitle">' +
                           '商户编号' +
                        '</i-col>' +
                        '<i-col span="4">' +
                           '<i-input v-model="MERCHANTID"></i-input>' +
                        '</i-col>' +
                        '<i-col span="2" class="RowTitle">' +
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
            CONTRACTID: '',
            MERCHANTID: '',
            NAME:'',
            Column: [
                {
                  type: 'selection',
                  width: 60,
                  align: 'center'
                },
               {
                  title: '租约号',
                  key: 'CONTRACTID',
                  width: 150,
                },
                {
                    title: '分店名称',
                    key: 'NAME',
                    width: 150,
                },
                {
                    title: '商户名称',
                    key: 'MERNAME',
                    width: 150,
                }
            ],
            Data: [

            ],
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
            console.log(this.parenttochild);
            //在查询之前用这里传递的参数
            _self = this;
            this.Data = [];
            _.Search({
                Service: "HtglService",
                Method: "GetContract",
                Data: {
                    CONTRACTID: _self.CONTRACTID,
                    MERCHANTID: _self.MERCHANTID,
                    NAME: _self.NAME,
                },
                Success: function (data) {
                    Vue.set(_self, "Data", data.rows);
                }
            })
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