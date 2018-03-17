Vue.component('Merchant', {
    template: '<div>' +
                    '<i-input v-model="hehe1" style="width: 150px"></i-input>' +
                    '<i-input v-model="hehe2" style="width: 150px"></i-input>' +
                    '<i-table border highlight-row  ref="currentRowTable" :columns="perColumn" :data="perData" ></i-table>' +
                    '<i-button v-on:click="cx">查询</i-button>' +
                    '<i-button v-on:click="qk">清空</i-button>' +
                    '<i-button v-on:click="qr">确认</i-button>' +
	           '</div>',
    data: function () {
        return {
            hehe1: '',
            hehe2: '',
            perColumn: [
              {
                  type: 'selection',
                  width: 60,
                  align: 'center'
              },

        {
            title: '商户代码',
            key: 'MERCHANTID',
            width: 150,
        },
        {
            title: '商户名称',
            key: 'NAME',
            width: 150,

        },
        ],
            perData: [

            ],
        }
    },

    created: function () {
        this.perData = [];
    },
    methods: {
        qr: function () {
            var data = {};
            data.sj = this.$refs.currentRowTable.getSelection();
            this.perData = [];
            this.$refs.currentRowTable.makeObjData();
            this.$emit('setdialog', data)
        },
        cx: function () {
            _self = this;
            this.perData = [];
            _.Search({
                Service: "ShglService",
                Method: "GetMerchant",
                Data: {  },
                Success: function (data) {
                    Vue.set(_self, "perData", data.rows);   
                }
            })
            console.log(this.parenttochild);
        },
        qk: function () {
            this.perData = [];
        },
    },
    props: {
        parenttochild: {
            type: Object,
            required: true
        }
    }
})