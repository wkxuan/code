
$(function () {
    init();
});
function init() {
    initmenu(function () {
       
    });
   // inittime();
  
}

//初始化菜单
function initmenu(callback) {
    _.Ajax('GetMenu', {
        Data: {}
    }, function (data) {
        if (data) {
            var menus = data;
            LoadMenu(menus);

            layui.use('element', function () {
                var $ = layui.jquery;
                var element = layui.element;


                var active = {

                    tabAdd: function (url, id, name) {
                        //新增一个Tab项 传入三个参数，分别对应其标题，tab页面的地址，还有一个规定的id，是标签中data-id的属性值
                        //关于tabAdd的方法所传入的参数可看layui的开发文档中基础方法部分
                        element.tabAdd('yxadmin', {
                            title: name,
                            content: '<iframe data-frameid="' + id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:99%;"></iframe>',
                            id: id //规定好的id
                        })
                        CustomRightClick(id);
                        FrameWH();
                    },
                    tabChange: function (id) {

                        element.tabChange('yxadmin', id);
                    },
                    tabDelete: function (id) {
                        element.tabDelete("yxadmin", id);//删除
                    }
                    , tabDeleteAll: function (ids) {//删除所有
                        $.each(ids, function (i, item) {
                            element.tabDelete("yxadmin", item); //ids是一个数组，里面存放了多个id，调用tabDelete方法分别删除
                        })
                    }
                };


                //当点击有site-demo-active属性的标签时，即左侧菜单栏中内容 ，触发点击事件
                $('.site-demo-active').on('click', function () {
                    var dataid = $(this);

                    //这时会判断右侧.layui-tab-title属性下的有lay-id属性的li的数目，即已经打开的tab项数目
                    if ($(".layui-tab-title li[lay-id]").length <= 0) {
                        //如果比零小，则直接打开新的tab项
                        active.tabAdd(dataid.attr("data-url"), dataid.attr("data-id"), dataid.attr("data-title"));
                    } else {
                        //否则判断该tab项是否以及存在

                        var isData = false; //初始化一个标志，为false说明未打开该tab项 为true则说明已有
                        $.each($(".layui-tab-title li[lay-id]"), function () {
                            //如果点击左侧菜单栏所传入的id 在右侧tab项中的lay-id属性可以找到，则说明该tab项已经打开
                            if ($(this).attr("lay-id") == dataid.attr("data-id")) {
                                isData = true;
                            }
                        })
                        if (isData == false) {
                            //标志为false 新增一个tab项
                            active.tabAdd(dataid.attr("data-url"), dataid.attr("data-id"), dataid.attr("data-title"));
                        }
                    }
                    //最后不管是否新增tab，最后都转到要打开的选项页面上
                    active.tabChange(dataid.attr("data-id"));
                });

                function CustomRightClick(id) {
                    //取消右键  rightmenu属性开始是隐藏的 ，当右击的时候显示，左击的时候隐藏
                    $('.layui-tab-title li').on('contextmenu', function () { return false; })
                    $('.layui-tab-title,.layui-tab-title li').click(function () {
                        $('.rightmenu').hide();
                    });
                    //桌面点击右击 
                    $('.layui-tab-title li').on('contextmenu', function (e) {
                        var popupmenu = $(".rightmenu");
                        popupmenu.find("li").attr("data-id", id); //在右键菜单中的标签绑定id属性

                        //判断右侧菜单的位置 
                        l = ($(document).width() - e.clientX) < popupmenu.width() ? (e.clientX - popupmenu.width()) : e.clientX;
                        t = ($(document).height() - e.clientY) < popupmenu.height() ? (e.clientY - popupmenu.height()) : e.clientY;
                        popupmenu.css({ left: l, top: t }).show(); //进行绝对定位
                        //alert("右键菜单")
                        return false;
                    });
                }

                $(".rightmenu li").click(function () {

                    //右键菜单中的选项被点击之后，判断type的类型，决定关闭所有还是关闭当前。
                    if ($(this).attr("data-type") == "closethis") {
                        //如果关闭当前，即根据显示右键菜单时所绑定的id，执行tabDelete
                        active.tabDelete($(this).attr("data-id"))
                    } else if ($(this).attr("data-type") == "closeall") {
                        var tabtitle = $(".layui-tab-title li");
                        var ids = new Array();
                        $.each(tabtitle, function (i) {
                            ids[i] = $(this).attr("lay-id");
                        })
                        //如果关闭所有 ，即将所有的lay-id放进数组，执行tabDeleteAll
                        active.tabDeleteAll(ids);
                    }

                    $('.rightmenu').hide(); //最后再隐藏右键菜单
                })
                function FrameWH() {
                    var h = $(window).height() - 41 - 10 - 60 - 10 - 44 - 10;
                    $("iframe").css("height", h + "px");
                }

                $(window).resize(function () {
                    FrameWH();
                })
            });
        };
    });
}


function LoadMenu(menus) {
    var menuhtml = '';
    for (var i = 0; i < menus.MENU.length ; i++) {
        menuhtml += '<li class="layui-nav-item">';
        menuhtml += '  <a>' + menus.MENU[i].NAME + '</a>';
        menuhtml += ' <dl class="layui-nav-child">';

        for (var j = 0; j < menus.MENU[i].MENUList.length ; j++) {
            menuhtml += '     <dd>';

            menuhtml += '  <a data-url="' + menus.MENU[i].MENUList[j].URL + '" data-id="' + menus.MENU[i].MENUList[j].ID + '" data-title="' + menus.MENU[i].MENUList[j].NAME + '" href="#"  class="site-demo-active" data-type="tabAdd">' + menus.MENU[i].MENUList[j].NAME + '</a>';
            menuhtml += '     </dd>';
        }
        menuhtml += '</dl> ';
        menuhtml += ' </li> ';
    };
    document.getElementById("taburl").innerHTML = menuhtml;
}

function fillSpace(key) {
    var obj = jmenus.get(key);
    if (!obj) return;
    var parent = $(obj).parent();
    var height = parent.height() - (($(".accordionHeader", obj).size()) * ($(".accordionHeader:first-child", obj).outerHeight())) - 2;
    var os = parent.children().not(obj);
    $.each(os, function (i) {
        height -= $(os[i]).outerHeight();
    });
    $(".accordionContent", obj).height(height);
}

//递归子菜单
function LoadMenu2(menus, pid) {
    var html = "";
    var menu = $.grep(menus, function (value) {
        return value.PID == pid;
    });
    if (menu) {
        $.each(menu, function (inx, obj) {
            if (obj.URL)//有路径
            {
                html += '<li><a href="' + obj.URL + '" target="navTab" rel="' + obj.ID + '"  external="true"  fresh="true">' + obj.NAME + '</a></li>';
            }
            else {  //没路径
                html += '<li><a>' + obj.NAME + '</a>\
                                <ul>';
                html += LoadMenu2(menus, obj.ID);
                html += '            </ul>\
                            </li>';
            }
        });
    }
    return html;
}

//显示时间
function inittime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    $('#a_timenow').html("" + year + "年" + month + "月" + day + "日 " + hours + ":" + minutes + ":" + seconds + "");
    //一秒刷新一次显示时间
    var timeID = setTimeout(inittime, 1000);
}

//个人中心
function employee() {
    alert("个人中心");
}

//退出
function loginout() {
    window.location.href = __BaseUrl + "/HOME/Login/Login";
}
