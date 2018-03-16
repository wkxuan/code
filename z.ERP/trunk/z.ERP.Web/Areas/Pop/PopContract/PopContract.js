Vue.component('Contract', {
    template: '<div>' +
                    '<i-input v-model="hehe1" style="width: 150px"></i-input>' +
                    '<i-input v-model="hehe2" style="width: 150px"></i-input>' +
                    '<i-input v-model="parenttochild.A" style="width: 150px"></i-input>' +
                    '<i-input v-model="parenttochild.A1111" style="width: 150px"></i-input>' +
                    '<i-table border highlight-row  ref="currentRowTable" :columns="perColumn" :data="perData" ></i-table>' +
                    '<i-button id="asd" a="b" v-on:click="cx">查询</i-button>' +
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
            data.hehe1 = this.hehe1;
            data.PopMerchant = false;
            data.sj = this.$refs.currentRowTable.getSelection();
            this.perData = [];
            this.$refs.currentRowTable.makeObjData();
            this.$emit('setdialog', data)
        },
        cx: function () {
            _self = this;
            this.perData = [];
            _.Search({
                Service: "HtglService",
                Method: "GetContract",
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