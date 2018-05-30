$(function () {
    init();
});
function init() {
    initmenu(function () {   
    });
}

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
                        element.tabAdd('yxadmin', {
                            title: name,
                            content: '<iframe data-frameid="' + id + '" scrolling="auto" frameborder="0" src="' + url + '" style="width:100%;height:99%;"></iframe>',
                            id: id 
                        })
                        CustomRightClick(id);
                        FrameWH();
                    },
                    tabChange: function (id) {

                        element.tabChange('yxadmin', id);
                    },
                    tabDelete: function (id) {
                        element.tabDelete("yxadmin", id);
                    }
                    , tabDeleteAll: function (ids) {
                        $.each(ids, function (i, item) {
                            element.tabDelete("yxadmin", item); 
                        })
                    }
                };



                $('.site-demo-active').on('click', function () {
                    var dataid = $(this);
                    if ($(".layui-tab-title li[lay-id]").length <= 0) {
                        active.tabAdd(dataid.attr("data-url"), dataid.attr("data-id"), dataid.attr("data-title"));
                    } else {
                        var isData = false; 
                        $.each($(".layui-tab-title li[lay-id]"), function () {
                            if ($(this).attr("lay-id") == dataid.attr("data-id")) {
                                isData = true;
                            }
                        })
                        if (isData == false) {
                            active.tabAdd(dataid.attr("data-url"), dataid.attr("data-id"), dataid.attr("data-title"));
                        }
                    }
                    active.tabChange(dataid.attr("data-id"));
                });

                function CustomRightClick(id) {
                    $('.layui-tab-title li').on('contextmenu', function () { return false; })
                    $('.layui-tab-title,.layui-tab-title li').click(function () {
                        $('.rightmenu').hide();
                    });
                    $('.layui-tab-title li').on('contextmenu', function (e) {
                        var popupmenu = $(".rightmenu");
                        popupmenu.find("li").attr("data-id", id); 
                        l = ($(document).width() - e.clientX) < popupmenu.width() ? (e.clientX - popupmenu.width()) : e.clientX;
                        t = ($(document).height() - e.clientY) < popupmenu.height() ? (e.clientY - popupmenu.height()) : e.clientY;
                        popupmenu.css({ left: l, top: t }).show();
                        return false;
                    });
                }

                $(".rightmenu li").click(function () {


                    if ($(this).attr("data-type") == "closethis") {

                        active.tabDelete($(this).attr("data-id"))
                    } else if ($(this).attr("data-type") == "closeall") {
                        var tabtitle = $(".layui-tab-title li");
                        var ids = new Array();
                        $.each(tabtitle, function (i) {
                            ids[i] = $(this).attr("lay-id");
                        })
    
                        active.tabDeleteAll(ids);
                    }

                    $('.rightmenu').hide(); 
                })

                $(".layui-nav-item").click(function () {
                    $(".layui-nav-item").removeClass("layui-nav-itemed");
                    $(".layui-nav-item").removeClass("layui-this");
                    if ($(this).has('dl').length) {
                        $(this).addClass("layui-nav-itemed");
                    } else {
                        $(this).addClass("layui-this");
                    }
                });

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

//function fillSpace(key) {
//    var obj = jmenus.get(key);
//    if (!obj) return;
//    var parent = $(obj).parent();
//    var height = parent.height() - (($(".accordionHeader", obj).size()) * ($(".accordionHeader:first-child", obj).outerHeight())) - 2;
//    var os = parent.children().not(obj);
//    $.each(os, function (i) {
//        height -= $(os[i]).outerHeight();
//    });
//    $(".accordionContent", obj).height(height);
//}

////递归子菜单
//function LoadMenu2(menus, pid) {
//    var html = "";
//    var menu = $.grep(menus, function (value) {
//        return value.PID == pid;
//    });
//    if (menu) {
//        $.each(menu, function (inx, obj) {
//            if (obj.URL)//有路径
//            {
//                html += '<li><a href="' + obj.URL + '" target="navTab" rel="' + obj.ID + '"  external="true"  fresh="true">' + obj.NAME + '</a></li>';
//            }
//            else {  //没路径
//                html += '<li><a>' + obj.NAME + '</a>\
//                                <ul>';
//                html += LoadMenu2(menus, obj.ID);
//                html += '            </ul>\
//                            </li>';
//            }
//        });
//    }
//    return html;
//}

////显示时间
//function inittime() {
//    var now = new Date();
//    var year = now.getFullYear();
//    var month = now.getMonth() + 1;
//    var day = now.getDate();
//    var hours = now.getHours();
//    var minutes = now.getMinutes();
//    var seconds = now.getSeconds();
//    $('#a_timenow').html("" + year + "年" + month + "月" + day + "日 " + hours + ":" + minutes + ":" + seconds + "");
//    //一秒刷新一次显示时间
//    var timeID = setTimeout(inittime, 1000);
//}

////个人中心
//function employee() {
//    alert("个人中心");
//}

////退出
//function loginout() {
//    window.location.href = __BaseUrl + "/HOME/Login/Login";
//}
