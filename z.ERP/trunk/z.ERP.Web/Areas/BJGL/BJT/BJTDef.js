var map;
$(function () {
    var options = {
        Url: '/Content/Maps/fashion.jpg',  //底图
        width: 1600,   //这个尺寸是显示的尺寸,多大都行,不影响底下的坐标
        height: 1046,
        canEdit: true,   //是否可以编辑,如果不可以编辑,就不能拖动,删除,新增
        //假设一些数据,这些是要从后台取到的
        data: [    //这里的数据,不能删,但是可以任意的加,不要占用这4个属性就行了,方便显示的时候通过GetHtml渲染数据
            {
                name: "aaaaaaaaaaaaaaaaaa",
                x: 0.16,   //这俩是坐标,是个相对坐标,所以图片尺寸变了也没关系,存库就存这个,把这俩拼成一个字段存起来也行
                y: 0.62,
                html: GetHtml
            },
            {
                name: "bbbbbbbbbbbbb",
                html: GetHtml,
                x: 0.5,
                y: 0.36
            }
        ]
    };
    map = $("#div_map").zMapPoint(options);
});

function GetHtml(data) {
    return "店铺号:<br>" + data.name
}

function add() {
    var newname = prompt("请输入店铺号", "");
    map.Add({
        name: newname,
        html: GetHtml,
        x: 0,
        y: 0
    });
}

function save() {
    var a = map.GetData();
}
