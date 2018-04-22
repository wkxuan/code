
$(function () {
    init();
});
function init() {
    initmenu(function () {
        DWZ.init(__BaseUrl + "/Areas/Home/Index/dwz.frag.xml");
    });
    inittime();
    $("#btn_ChangePassword").attr("href",__BaseUrl + "/HOME/ChangePassword/ChangePassword");
}

//初始化菜单
function initmenu(callback) {
    var pram = {
        op: 'ShowMenu'
    };
    _.Search({
        Service: "HomeService",
        Method: "GetMenu",
        Success: function (data) {
            if (data) {
                var menus = data.rows;
                LoadMenu(menus);
                callback && callback();
            }
            else {
                alert("没有任何可用的菜单");
            }
        }
    });
}

//顶部主菜单
function LoadMenu(menus) {
    //回头再这里加一步权限处理,当没有所有子菜单权限的时候,隐藏父节点
    var TopMenu = $.grep(menus, function (value) {
        return value.PID == 0;
    });
    var topmenuhtml = '';
    $.each(TopMenu, function (inx, obj) {
        topmenuhtml += '<li><a menuid="' + obj.ID + '" ><span>' + obj.NAME + '</span></a></li>';
    });
    $('#menu_top').html(topmenuhtml);
    //每个菜单的点击事件
    $('#menu_top').find('[menuid]').each(function (inx, obj) {
        $(obj).click(function () {
            $('#menu_top').find('li').each(function (i, o) {
                $(o).removeClass('selected');
            });
            $(obj).parent().addClass('selected');
            //加载下一级
            var Menu2 = $.grep(menus, function (value) {
                return value.PID == $(obj).attr("menuid");
            });
            var menu2html = '';
            if (Menu2) {
                $.each(Menu2, function (inx2, obj2) {
                    menu2html += '<div class="accordionHeader">\
                        <h2><span>Folder</span>'+ obj2.NAME + '</h2>\
                    </div>';
                    menu2html += '<div class="accordionContent">\
                        <ul class="tree treeFolder">';
                    //递归加载子菜单
                    menu2html += LoadMenu2(menus, obj2.ID);
                    menu2html += '</ul>\
                    </div>';
                });
            }
            $('#menu_2').html(menu2html);
            //initUI();
            initEnv();
            //initLayout();
        });
    });
    //进页面,先点一个菜单
    $('#menu_top').find('[menuid]')[0].click();
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
