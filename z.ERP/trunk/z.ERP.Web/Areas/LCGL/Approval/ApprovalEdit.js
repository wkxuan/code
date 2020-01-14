var myChart;
var id = GetQueryString("id");
var s = GetQueryString("s");
var bid = GetQueryString("bid");
var bname = GetQueryString("bname");
var name = GetQueryString("name");
var mosedata;
var BBID=bid+DY10(id);
var DataView = new Vue({
    el: "#DataView",
    data: {
        labelName: "",
        labelValue: "",
        disab:true,
        option : {
            title: {
                text: name
            },
            //tooltip: {},
            series: [{
                type: 'graph',
                layout: 'none',
                symbolSize: 50,
                label: {
                    show: true
                },
                edgeSymbol: ['circle', 'arrow'],
                edgeSymbolSize: [4, 10],
                edgeLabel: {
                    fontSize: 20
                },
                data: [{
                    name: '流程发起',
                    value: 0,
                    x: 100,
                    y: 300,
                }, {
                    name: '流程结束',
                    value: 99,
                    x: 200,
                    y: 300
                }],
                links: [{
                    source: 0,
                    target: 1,
                    symbolSize: [5, 20],
                    label: {
                        show: false,
                        formatter: '+',
                        fontSize: 50,
                        color: "#B53F00"
                    },
                }],
                lineStyle: {
                    opacity: 0.9,
                    width: 3,
                    curveness: 0
                }
            }]
        },
        val: "",   //节点输入框
        datauser: [],
        datarole: [],
        colDefrole: [
            { title: "角色编码", key: 'ROLECODE' },
            { title: '角色名称', key: 'ROLENAME' },
            { title: ' ',key: 'action',width: 150,align: 'center',
                render: (h, params) => {
                    return h('div', [                       
                        h('Button', {
                            props: {
                                type: 'error',
                                size: 'small',
                                disabled:DataView.disab,
                            },
                            on: {
                                click: () => {
                                    DataView.remove(params.row, 1)
                                }
                            }
                        }, '删除')
                    ]);
                }           
        }],
        colDefuser: [
            { title: "人员编码", key: 'USERCODE' },
            { title: '人员名称', key: 'USERNAME' },
            {
                title: ' ', key: 'action', width: 150, align: 'center',
                render: (h, params) => {
                    return h('div', [
                        h('Button', {
                            props: {
                                type: 'error',
                                size: 'small',
                                disabled: DataView.disab,
                            },
                            on: {
                                click: () => {
                                    DataView.remove(params.row, 2)
                                }
                            }
                        }, '删除')
                    ]);
                }
            }],
        popConfig: {
            title: "弹窗",
            src: "",
            width: 900,
            height: 550,
            open: false
        },
        dataParam: {
            APPRID: id,
            BRANCHID: bid,
            STATUS: s,           
        },
        APPROVAL_NODE: [],
        APPROVAL_NODE_OPER:[]
    },
    mounted: function () {
        this.echartinit();
    },
    methods: {
        echartinit: function () {
            myChart = echarts.init(document.getElementById('mainview'));
            myChart.setOption(this.option);
            this.ShowDetail();
        },
        ShowDetail:function(){
            _.Ajax('ShowDetail', {
                branchid:bid,apprid:id
            }, function (data) {              
                if (data.an.length > 0) {
                    DataView.APPROVAL_NODE_OPER = [];
                    for (let i = 0; i < data.ano.length; i++) {   //节点权限table数据
                        DataView.APPROVAL_NODE_OPER.push({ APPR_NODE_ID: data.ano[i].APPR_NODE_ID + "", OPER_TYPE: data.ano[i].OPER_TYPE, OPER_DATA: data.ano[i].OPER_DATA, OPER_NAME: data.ano[i].OPER_NAME, CODE: data.ano[i].CODE });
                    }
                    let edata = [];
                    for (let i = 0; i < data.an.length; i++) {   //节点echart数据
                        edata.push({
                            name: data.an[i].NODE_TITLE,
                            value: data.an[i].NODE_INX,
                            x: i*100+100,
                            y: 300,
                        })
                    }
                    var links = linkdata(data.an.length);
                    for (let i = 0; i < links.length; i++) {
                        links[i].label.show = false;
                    }
                    myChart.setOption({    //ECHART重新赋值
                        series: [{
                            data: edata,
                            links: links
                        }]
                    });                    
                }
            });
        },
        addUser: function () {
            DataView.popConfig.title = "选择人员";
            DataView.popConfig.src = __BaseUrl + "/Pop/Pop/PopSysuserList/";
            DataView.popConfig.open = true;
        },
        addRole:function(){
            DataView.popConfig.title = "选择角色";
            DataView.popConfig.src = __BaseUrl + "/Pop/Pop/PopRoleList/";
            DataView.popConfig.open = true;
        },
        popCallBack:function(data){
            if (DataView.popConfig.open) {
                DataView.popConfig.open = false;
                if (DataView.popConfig.title == "选择人员") {
                    let user = DataView.datauser;
                    for (let i = 0; i < data.sj.length; i++) {
                        if (user.filter(item=> { return (data.sj[i].USERID == item.USERID) }).length == 0) {
                            user.push(data.sj[i]);
                            DataView.addNPObject(data.sj[i],2);
                        }
                    };
                }
                if (DataView.popConfig.title == "选择角色") {
                    let role = DataView.datarole;
                    for (let i = 0; i < data.sj.length; i++) {
                        if (role.filter(item=> { return (data.sj[i].ROLEID == item.ROLEID) }).length == 0) {
                            role.push(data.sj[i]);
                            DataView.addNPObject(data.sj[i], 1);
                        }
                    };
                }
            }
        },
        remove (row, type) {     //type  1角色 2人员
            if (type == 1) {  //角色
                let temp = DataView.datarole;
                for (let i = 0; i < temp.length; i++) {
                    if (temp[i].ROLEID == row.ROLEID) {
                        temp.splice(i, 1);
                        DataView.delNPObject(row.ROLEID,1);
                        break;
                    }
                }
            } else {
                let temp = DataView.datauser;
                for (let i = 0; i < temp.length; i++) {
                    if (temp[i].USERID == row.USERID) {
                        temp.splice(i, 1);
                        DataView.delNPObject(row.USERID, 2);
                        break;
                    }
                }
            }
        },
        addNPObject: function (data, type) {//type  1角色 2人员
            if (type == 1) {
                DataView.APPROVAL_NODE_OPER.push({ APPR_NODE_ID: BBID+DY10(DataView.labelValue), OPER_TYPE: 1, OPER_DATA: data.ROLEID, OPER_NAME: data.ROLENAME , CODE:data.ROLECODE });
            }else{
                DataView.APPROVAL_NODE_OPER.push({ APPR_NODE_ID: BBID + DY10(DataView.labelValue), OPER_TYPE: 2, OPER_DATA: data.USERID, OPER_NAME: data.USERNAME, CODE: data.USERCODE });
            }
        },
        delNPObject: function (data, type) {
            let temp = DataView.APPROVAL_NODE_OPER;
            for (let i = 0; i < temp.length; i++) {
                if (temp[i].OPER_DATA == data && temp[i].APPR_NODE_ID.replace(BBID, "") == DY10(DataView.labelValue) && temp[i].OPER_TYPE == type) {
                    temp.splice(i, 1);
                    break;                
                }
            }
        },
        delNodeo:function(){  //节点删除时，对应人员角色删除
            let temp = DataView.APPROVAL_NODE_OPER;
            for (let i = 0; i < temp.length; i++) {
                if (temp[i].APPR_NODE_ID.replace(BBID,"") ==DY10(DataView.labelValue)) {
                    temp.splice(i, 1);
                    i--;
                }
            }
        },
        clickNode: function (e) {
            DataView.labelName = e.data.name;
            DataView.labelValue = e.data.value;
            DataView.datauser = [];
            DataView.datarole = [];
            let temp = DataView.APPROVAL_NODE_OPER;
            for (let i = 0; i < temp.length; i++) {
                if (temp[i].APPR_NODE_ID.replace(BBID,"") ==DY10(DataView.labelValue) && temp[i].OPER_TYPE == 1) {
                    DataView.datarole.push({ ROLECODE: temp[i].CODE, ROLENAME: temp[i].OPER_NAME, ROLEID: temp[i].OPER_DATA });
                    continue;
                }
                if (temp[i].APPR_NODE_ID.replace(BBID,"") ==DY10(DataView.labelValue) && temp[i].OPER_TYPE == 2) {
                    DataView.datauser.push({ USERCODE: temp[i].CODE, USERNAME: temp[i].OPER_NAME, USERID: temp[i].OPER_DATA });
                    continue;
                }
            }
        },
        save: function () {
            DataView.CreatANdata();
            if (!DataView.IsValidSave())
                return;
            _.Ajax('Save', {
                SaveData: DataView.dataParam, SaveDataDetail: DataView.APPROVAL_NODE, SaveDataOper: DataView.APPROVAL_NODE_OPER
            }, function (data) {
                DataView.disab = true;
                var options = myChart.getOption();
                var links = options.series[0].links;
                for (let i = 0; i < links.length; i++) {
                    links[i].label.show = false;
                }
                myChart.setOption({
                    series: [{
                        links: links
                    }]
                });
                iview.Message.info("保存成功");
            });
        },
        CreatANdata: function () {
            var options = myChart.getOption();
            var data = Object.assign(options.series[0].data);
            var links = Object.assign(options.series[0].links);
            if (data.length > 2) {
                DataView.APPROVAL_NODE = [];
                for (let i = 0; i < data.length; i++) {
                    if (data[i].value == 0) {
                        DataView.APPROVAL_NODE.push({
                            APPR_NODE_ID: BBID + DY10(data[i].value),
                            APPRID: id,
                            BRANCHID: bid,
                            NODE_INX: data[i].value,
                            NODE_TITLE: data[i].name,
                            NEXT_APPR_NODE_ID: BBID + DY10(links[0].target)
                        });
                    } else if (data[i].value == 99) {
                        DataView.APPROVAL_NODE.push({
                            APPR_NODE_ID: BBID + DY10(data[i].value),
                            APPRID: id,
                            BRANCHID: bid,
                            NODE_INX: data[i].value,
                            NODE_TITLE: data[i].name,
                            NEXT_APPR_NODE_ID: -1
                        });
                    } else {
                        DataView.APPROVAL_NODE.splice(data[i].value, 0, {
                            APPR_NODE_ID: BBID + DY10(data[i].value),
                            APPRID: id,
                            BRANCHID: bid,
                            NODE_INX: data[i].value,
                            NODE_TITLE: data[i].name,
                            NEXT_APPR_NODE_ID: BBID + DY10(links[i].target)
                        });
                    }
                }
                DataView.APPROVAL_NODE[data.length - 2].NEXT_APPR_NODE_ID = BBID + 99;
            } else {
                return;
            }
        },
        IsValidSave: function () {
            if (DataView.APPROVAL_NODE.length==0) {
                iview.Message.info("请添加审批节点!");
                return false;
            };
            let temp = DataView.APPROVAL_NODE;
            for (let i = 0; i < temp.length-1; i++) {
                var operdata = DataView.APPROVAL_NODE_OPER.find(item=>item.APPR_NODE_ID == temp[i].APPR_NODE_ID);
                if (operdata==undefined) {
                    iview.Message.info("节点:" + temp[i].NODE_TITLE + ",请添加人员或角色!");
                    return false;
                }
            }
            return true;
        },
        refresh: function () {
            DataView.disab = true;
            _.Ajax('ShowDetail', {
                branchid: bid, apprid: id
            }, function (data) {
                if (data.an.length > 0) {
                    DataView.APPROVAL_NODE_OPER = [];
                    for (let i = 0; i < data.ano.length; i++) {   //节点权限table数据
                        DataView.APPROVAL_NODE_OPER.push({ APPR_NODE_ID: data.ano[i].APPR_NODE_ID + "", OPER_TYPE: data.ano[i].OPER_TYPE, OPER_DATA: data.ano[i].OPER_DATA, OPER_NAME: data.ano[i].OPER_NAME, CODE: data.ano[i].CODE });
                    }
                    let edata = [];
                    for (let i = 0; i < data.an.length; i++) {   //节点echart数据
                        edata.push({
                            name: data.an[i].NODE_TITLE,
                            value: data.an[i].NODE_INX,
                            x: i * 100 + 100,
                            y: 300,
                        })
                    }
                    var links = linkdata(data.an.length);
                    for (let i = 0; i < links.length; i++) {
                        links[i].label.show = false;
                    }
                    myChart.setOption({    //ECHART重新赋值
                        series: [{
                            data: edata,
                            links: links
                        }]
                    });
                } else {
                    window.location.reload();
                }
            });
        },
        edit: function () {
            DataView.disab = false;
            var options = myChart.getOption();
            var links = options.series[0].links;
            for (let i = 0; i < links.length; i++) {
                links[i].label.show = true;
            }
            myChart.setOption({
                series: [{
                    links: links
                }]
            });
        }
    },
});
function GetQueryString(name)
{
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return decodeURIComponent(r[2]);
    return null;
}

//绑定鼠标事件
myChart.on("mousedown", function (e) {
    if (e.event.event.button === 0) { //左键单击事件
        if (e.event.target.name == "label" && DataView.disab == false) { //点击+号 添加审批
            DataView.labelName = "";
            DataView.labelValue = "";
            DataView.datauser = [];
            DataView.datarole = [];
            addn(e.data.source);
        } else if (e.dataType == "node" && e.data.value != 99) { //点击节点 显示人员角色信息            
            DataView.clickNode(e);
        } else if (e.data.value == 99) {
            DataView.labelName = e.data.name;
            DataView.labelValue = e.data.value;
            DataView.datauser = [];
            DataView.datarole = [];
        }
    }
    if (e.event.event.button === 2 && e.dataType == "node" && DataView.disab==false) { //右键node点击事件       
        if (e.dataIndex === 0) {
            DataView.clickNode(e);
            //e中有当前节点信息
            showMenu(e, [{
                "name": "添加角色",
                "fn": function () {
                    DataView.addRole(e.data);
                }
            }, {
                "name": "添加人员",
                "fn": function () {
                    DataView.addUser(e.data);
                }
            }, {
                "name": "重命名",
                "fn": function () {
                    cnn(e);
                }
            }]);
        } else if (e.data.value === 99) {
            DataView.labelName = e.data.name;
            DataView.labelValue = e.data.value;
            DataView.datauser = [];
            DataView.datarole = [];
            showMenu(e, [{
                "name": "重命名",
                "fn": function () {
                    cnn(e);
                }
            }]);
        } else {
            DataView.clickNode(e);
            //e中有当前节点信息
            showMenu(e, [{
                "name": "添加角色",
                "fn": function () {
                    DataView.addRole(e.data);
                }
            }, {
                "name": "添加人员",
                "fn": function () {
                    DataView.addUser(e.data);
                }
            }, {
                "name": "删除节点",
                "fn": function () {
                    var mosedata = e;
                    DataView.$Modal.confirm({
                        title: '提示',
                        content: '<p>是否删除节点？</p>',
                        onOk: () => {
                            delNode(mosedata.data);
                            DataView.labelName = "";
                            DataView.labelValue = "";
                            DataView.datauser = [];
                            DataView.datarole = [];
                            iview.Message.success("删除节点成功");
                        },
                    });                                       
                }
            }, {
                "name": "重命名",
                "fn": function () {
                    cnn(e);
                }
            }]);
        }
    }
})
var style_ul = "padding:0px;margin:0px;border: 1px solid #ccc;background-color: #fff;position: absolute;left: 0px;top: 0px;z-index: 2;display: none;";
var style_li = "list-style:none;padding: 5px; cursor: pointer; padding: 5px 20px;margin:0px;";
var style_li_hover = style_li + "background-color: #00A0E9; color: #fff;";

//右键菜单容器
var menubox = $("<div class='echartboxMenu' style='" + style_ul + "'><div style='text-align:center;background:#ccc'></div><ul style='margin:0px;padding:0px;'></ul></div>")
    .appendTo($(document.body));

//移除浏览器右键菜单
myChart.getDom().oncontextmenu = menubox[0].oncontextmenu = function () {
    return false;
}

//点击其他位置隐藏菜单
$(document).click(function () {
    menubox.hide()
});

//显示菜单
function showMenu(e, menus) {
    $("div", menubox).text(e.name);
    var menulistbox = $("ul", menubox).empty();
    $(menus).each(function (i, item) {
        var li = $("<li style='" + style_li + "'>" + item.name + "</li>")
            .mouseenter(function () {
                $(this).attr("style", style_li_hover);
            })
            .mouseleave(function () {
                $(this).attr("style", style_li);
            })
            .click(function () {
                item["fn"].call(this);
                menubox.hide();
            });
        menulistbox.append(li);
    });
    menubox.css({
        "left": event.x,
        "top": event.y
    }).show();
}
//添加节点弹窗 e.data.source
function addn(e) {
    mosedata = e;
    DataView.val = "";
    DataView.$Modal.confirm({
        title: "节点名称",
        render: (h) => {
            return h('Input', {
                props: {
                    value: "",
                    autofocus: true,
                },
                on: {
                    input: (a) => {
                        DataView.val = a;
                    }
                }
            })
        },
        onOk: () => {
            if (!DataView.val) {
                iview.Message.error("节点名称不能为空");
            } else {
                addNode(mosedata, DataView.val)
            }
        }
    });

}
//添加节点方法
function addNode(e,n) {    
    var options = myChart.getOption();
    var data = options.series[0].data;
    if (isRepect(n, data)) {
        iview.Message.error("节点名称不能重复！")
        return;
    }
    var inx = e + 1;
    var temp = Object.assign({}, data[e]);
    temp.name = n,
    temp.value = inx;
    temp.x = temp.x + 100;
    for (var i = inx; i < data.length; i++) {
        if (data[i].value == 99) {
            data[i].x += 100;
        } else {            
            data[i].value = i + 1;
            data[i].x += 100;
        }
    }
    data.splice(inx, 0, temp);
    var j = Object.assign(data.length);
    let te=DataView.APPROVAL_NODE_OPER;
    for (var i = j - 2; i > inx; i--) {     //增加节点后 后边节点权限随id后移
        for (let k = 0; k < te.length; k++) {
            if (te[k].APPR_NODE_ID.replace(BBID, "") == DY10(i - 1)) {
                te[k].APPR_NODE_ID = BBID + DY10(i);
            }
        }
    }
    var links = linkdata(data.length);
    myChart.setOption({
        series: [{
            data: data,
            links: links
        }]
    });
}
//删除节点方法
function delNode(e) {
    var options = myChart.getOption();
    var data = options.series[0].data;
    var inx = e.value;
    data.splice(inx, 1);
    for (var i = inx; i < data.length; i++) {
        if (data[i].value == 99) {
            data[i].x -= 100;
        } else {
            data[i].value = i;
            data[i].x -= 100;
        }
    }
    var links = linkdata(data.length);
    DataView.delNodeo();
    let te = DataView.APPROVAL_NODE_OPER;
    for (var i = inx + 1; i < data.length;i++) {     //删除节点后 后边节点权限随id前移
        for (let k = 0; k < te.length; k++) {
            if (te[k].APPR_NODE_ID.replace(BBID, "") == DY10(i)) {
                te[k].APPR_NODE_ID = BBID + DY10(i-1);
            }
        }
    }
    myChart.setOption({
        series: [{
            data: data,
            links: links
        }]
    });
}
//linkdata重组方法
function linkdata(length) {
    var links = [];
    for (var i = 0; i < length - 1; i++) {
        links.push({
            source: i,
            target: i + 1,
            symbolSize: [5, 20],
            label: {
                show: true,
                formatter: '+',
                fontSize: 50,
                color: "#B53F00"
            },
        });
    }
    return links;
}
//重命名弹窗
function cnn(e) {
    mosedata = e;
    DataView.val = e.name;
    DataView.$Modal.confirm({
        title: "节点名称",
        render: (h) => {
            return h('Input', {
                props: {
                    value: e.name,
                    autofocus: true,
                },
                on: {
                    input: (a) => {
                        DataView.val = a;
                    }
                }
            })
        },
        onOk: () => {
            if (!DataView.val) {
                iview.Message.error("节点名称不能为空");
            } else {
                changeNodeName(mosedata, DataView.val)
            }
        }
    });

}
//重命名方法
function changeNodeName(e, st) {
    var options = myChart.getOption();
    var data = options.series[0].data;
    var ds = data.find(item => item.value == e.value);
    var ds1 = data.find(item => item.name == st);
    if (ds1 != undefined && ds1 != ds) {
        iview.Message.error("节点名称不能重复！")
        return
    }
    ds.name = st;
    myChart.setOption({
        series: [{
            data: data,
        }]
    });
    iview.Message.success("修改成功！")
}
//是否具有重复项
function isRepect(st, data) {
    var ds = data.find(item => item.name == st);
    if (ds != undefined) {
        return true;
    } else {
        return false;
    }
}
//是否小于10
function DY10(I){
    if(I<10){
        return "0"+I
    }else{
        return I;
    }
}
