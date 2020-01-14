var DataView = new Vue({
    el: "#DataView",
    data: {
        BRANCHID: "",
        BRANCHNAME:"",
        branchData: [],
        ListData:[],
    },
    mounted: function () {
        this.initdata();
    },
    methods: {
        initdata: function () {
            _.Ajax('GetBranch', {}, function (data) {
                if (data.dt) {
                    DataView.branchData = [];
                    for (var i = 0; i < data.dt.length; i++) {
                        DataView.branchData.push({ value: data.dt[i].ID, label: data.dt[i].NAME })
                    }
                    DataView.BRANCHID = data.dt[0].ID;
                    DataView.BRANCHNAME = data.dt[0].NAME;
                    DataView.showlist();
                }
            });
        },
        showlist: function () {
            _.Ajax('GetApprovalData', { branchid: DataView.BRANCHID }, function (data) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length;i++){
                        if (data[i].STATUS == "1") {
                            data[i]['isON'] = true;
                        } else data[i]['isON'] = false;
                    }
                    DataView.ListData = data;
                }
            });
        },
        branchChange: function (e) {
            DataView.BRANCHNAME = DataView.branchData.find(item=>item.value == e).label;
            DataView.showlist();
        },
        Switchchange: function (id) {
            _.Ajax('Switchchange', { branchid: DataView.BRANCHID,apprid:id }, function (data) {
                if (data == "1") {
                    iview.Message.success("流程启用成功");
                } else {
                    iview.Message.success("流程关闭成功");
                }
            });           
        },
        itemclick: function (id, name, s) {                        
            DataView.Editapproval(id, s,DataView.BRANCHID, DataView.BRANCHNAME, name);   //打开新增页                       
        },
        Editapproval: function (id,s,bid,bname,name) {
            _.OpenPage({
                //id: 10600200,
                title: '(' + name + ')审批流详情',
                url: "/LCGL/Approval/ApprovalEdit?id=" + id + "&s=" + s + "&bid=" + bid + "&bname=" + bname + "&name=" + name
            });
        }
    },
});