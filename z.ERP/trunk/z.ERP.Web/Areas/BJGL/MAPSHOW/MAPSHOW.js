var Mapshow = new Vue({
    el: "#MapShow",
    data: {
        splitVal: 0.2,
        data: [],
        SHOPINFO: Object,
        MERCHANTINFO: Object,
        merchant:true,
        DrawerModel:false
    },
    mounted: function () {
        this.initTree();        
    },
    methods: {
        initTree: function () {
            _.SearchNoQuery({
                Service: "DpglService",
                Method: "TreeFloorData",
                Success: function (data) {
                    Mapshow.data = data;
                }
            })
        },
        onselectchange: function (selectArr, node) {
            if (node.parentId == "REGION") {
                var $this = this.$refs;    //目标元素
                $this.maps.innerHTML = "";
                _.Ajax('GetInitMAPDATA', {
                    Data: { FLOORID: node.code }
                }, function (data) {
                    if (data.floorInfo.MAPSHOPLIST.length > 0 || data.labelArray.length > 0) {
                        ThreeMapInit(data.floorInfo, data.labelArray, $this);
                    } else {
                        iview.Message.info("暂无布局图数据，请联系管理员!");
                    }
                });
            }
        },
    },
    filters:{
        formatDate: function (val) {
            if (!isEmpty(val)) {
                return val.substr(0, 10)
            } else {
                return "\\";
            }
        }
    }
});
function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}
function ThreeMapClick(id) {
    _.Ajax('GetSHOPINFO', {
        shopid: id
    }, function (data) {
        Mapshow.SHOPINFO = data.shopdata;
        if (Mapshow.SHOPINFO.RENT_STATUS == "1") {
            Mapshow.merchant = false;
        } else {
            Mapshow.merchant = true;
            Mapshow.MERCHANTINFO = data.merchantdata;
        }
        Mapshow.DrawerModel = true;
    });
};
