var Mapshow = new Vue({
    el: "#MapShow",
    data: {
        splitVal: 0.2,   //切割面板宽度
        data: [],
        SHOPINFO: Object,
        MERCHANTINFO: Object,
        Floorid:"",
        merchant:true,
        DrawerModel: false,      //右弹出抽屉
        buttontool: false,        //工具栏
        buttons: ["1", "2"],
        ISRENT: 0,
        NOTRENT:0
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
                Mapshow.Floorid = node.code;
                Mapshow.INITMAP();
            }
        },
        INITMAP: function () {
            var $this = this.$refs;    //目标元素
            $this.maps.innerHTML = "";
            _.Ajax('GetInitMAPDATA', {
                floorid: Mapshow.Floorid, shopstatus: Mapshow.buttons.join()
            }, function (data) {
                if (data.floorInfo.MAPSHOPLIST.length > 0 || data.labelArray.length > 0) {
                    ThreeMapInit(data.floorInfo, data.labelArray, $this);
                    Mapshow.buttontool = true;
                    Mapshow.ISRENT = data.floorInfo.IS_RENT;
                    Mapshow.NOTRENT = data.floorInfo.NOT_RENT;
                } else {
                    Mapshow.buttontool = false;
                    iview.Message.info("暂无布局图数据，请联系管理员!");
                }
            });
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
