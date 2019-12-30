var UrlDoMain = {};
$(function ($) {
    $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
    $(window).resize(function (e) {
        $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
    });
    //$('#sidebar-nav,#nav-col-submenu').on('click', '.dropdown-toggle', function (e) {
    //    e.preventDefault();
    //    var $item = $(this).parent();
    //    if (!$item.hasClass('open')) {
    //        $item.parent().find('.open .submenu').slideUp('fast');
    //        $item.parent().find('.open').toggleClass('open');
    //    }
    //    $item.toggleClass('open');
    //    if ($item.hasClass('open')) {
    //        $item.children('.submenu').slideDown('fast', function () {
    //            var _height1 = $(window).height() - 92 - $item.position().top;
    //            var _height2 = $item.find('ul.submenu').height() + 10;
    //            var _height3 = _height2 > _height1 ? _height1 : _height2;
    //            $item.find('ul.submenu').css({
    //                overflow: "auto",
    //                height: _height3
    //            })
    //        });
    //    }
    //    else {
    //        $item.children('.submenu').slideUp('fast');
    //    }
    //});
    // 新菜单点击事件
    $('#sidebar-nav,#nav-col-submenu a').on('click', '.dropdown-toggle', function () {
        var $obj = $(this);
        var $ul = $obj.next();       
        if ($ul.is(':visible')) {
            $ul.slideUp(100, function () {
                $obj.removeClass('open');
                $('#sidebar-nav').find('.three-menu-list').css({display: "none"})
            });
        }
        else {
            if ($ul.hasClass('second-menu-list')) {
                $('#sidebar-nav .second-menu-list').slideUp(100, function () {
                    $(this).prev().removeClass('open');
                });
            }
            else {
                $ul.parents('.second-menu-list').find('.three-menu-list').slideUp(100, function () {
                    $(this).prev().removeClass('open');
                });
            }
            $ul.slideDown(300, function () {
                $obj.addClass('open');
            });
        }
    });
    $('body').on('mouseenter', '#page-wrapper.nav-small #sidebar-nav .dropdown-toggle', function (e) {
        if ($(document).width() >= 992) {
            var $item = $(this).parent();
            if ($('body').hasClass('fixed-leftmenu')) {
                var topPosition = $item.position().top;

                if ((topPosition + 4 * $(this).outerHeight()) >= $(window).height()) {
                    topPosition -= 6 * $(this).outerHeight();
                }
                $('#nav-col-submenu').html($item.children('.submenu').clone());
                $('#nav-col-submenu > .submenu').css({ 'top': topPosition });
            }

            $item.addClass('open');
            $item.children('.submenu').slideDown('fast');
        }
    });
    $('body').on('mouseleave', '#page-wrapper.nav-small #sidebar-nav > .nav-pills > li', function (e) {
        if ($(document).width() >= 992) {
            var $item = $(this);
            if ($item.hasClass('open')) {
                $item.find('.open .submenu').slideUp('fast');
                $item.find('.open').removeClass('open');
                $item.children('.submenu').slideUp('fast');
            }
            $item.removeClass('open');
        }
    });
    $('body').on('mouseenter', '#page-wrapper.nav-small #sidebar-nav a:not(.dropdown-toggle)', function (e) {
        if ($('body').hasClass('fixed-leftmenu')) {
            $('#nav-col-submenu').html('');
        }
    });
    $('body').on('mouseleave', '#page-wrapper.nav-small #nav-col', function (e) {
        if ($('body').hasClass('fixed-leftmenu')) {
            $('#nav-col-submenu').html('');
        }
    });
    $('body').find('#make-small-nav').click(function (e) {
        $('#page-wrapper').toggleClass('nav-small');
        $('.submenu').toggleClass('menui');  //切换小图标模式 2.3级菜单图标隐藏
        var $item = $('#page-wrapper.nav-small #sidebar-nav > .nav-pills > .open');
        if ($item.hasClass('open')) {
            $item.find('.open .submenu').slideUp('fast');
            $item.find('.open').removeClass('open');
            $item.children('.submenu').slideUp('fast');
        }
        $item.removeClass('open');
        $('#sidebar-nav').find('.submenu').css({ display: "none" });
    });
    $('body').find('.mobile-search').click(function (e) {
        e.preventDefault();
        $('.mobile-search').addClass('active');
        $('.mobile-search form input.form-control').focus();
    });
    $(document).mouseup(function (e) {
        var container = $('.mobile-search');
        if (!container.is(e.target) && container.has(e.target).length === 0)
        {
            container.removeClass('active');
        }
    });

    addTab = function (name, url) {
        $.nfinetab.addTabM(name, url);
    }
    closeTab = function () {
        $('.menuTabs').find('.menuTab i').click();
    }

    $(window).load(function () {
        window.setTimeout(function () {
            $('#ajax-loader').fadeOut();
        }, 300);
    });
    $("#nav-col").height($(window).height() - 55);
});
//加载系统
function GetLoadMenu() {
    _.Ajax('GetPLATFORM', {}, function (data) {
        if (data.length > 0) {
            var $html = "";           
            for (i = 0; i < data.length; i++) {
                UrlDoMain[data[i].ID] = data[i].DOMAIN;
                $html += '<ul class="nav navbar-nav pull-left">';
                $html += '<li><a class="btn system' + data[i].ID + '" id="make-small-nav" onclick="GetLoadNav(' + data[i].ID + ')">' + data[i].SYSNAME + '<i class="hidden-xs"></i></a></li></ul>';
            }            
            $(".clearfix .syslist").append($html);
            GetLoadNav(data[0].ID);
        }
    });
}
//加载菜单
function GetLoadNav(systemid) {
    var $item = $(".clearfix .syslist");
    $item.find('.sysnameOnblur').removeClass('sysnameOnblur');
    $item.find('.system' + systemid).addClass('sysnameOnblur');    //系统之间切换css

    _.Ajax('GetMenu', {
        Data: {PLATFORMID: systemid}
    }, function (data) {
        if (data) {
            var datas = toMap(data);  //菜单转换成有序map
            var _html = "";
            $.each(datas, function (i, row) {  //循环一级菜单
                _html += '<li data-type="m">';
                _html += '<a data-id="' + row.MENUID + '" href="#" class="dropdown-toggle" style="height:34px"><i style="color: #66D2D3" class="' + row.ICON + ' fa-fw "></i>';
                _html += '<span>' + row.MODULENAME + '</span><i class="fa fa-angle-right drop-icon"></i></a>';
                var childMenus = row.children;   
                if (childMenus && childMenus.length > 0) { //循环二级菜单
                    _html += '<ul class="submenu second-menu-list">';
                    $.each(childMenus, function (i) {
                        var trow = childMenus[i];
                        if (trow.MENUID) {   //有MENUID的时菜单
                            _html += '<li><i style="color: #66D2D3;position: absolute;padding-left: 30px;line-height: 30px;" class="' + trow.ICON + ' fa-fw p-icon"></i>';
                            _html += '<a class="menuItem" data-id="' + UrlDoMain[systemid] + trow.URL + '" href="' + UrlDoMain[systemid] + trow.URL + '" data-index="' + trow.MENUID + '">' + trow.MODULENAME + '</a>';
                            _html += '</li>';
                        } else {
                            _html += '<li data-type="m"><i style="color: #66D2D3;position: absolute;padding-left: 30px;line-height: 30px;" class="' + trow.ICON + ' fa-fw p-icon"></i>';
                            _html += '<a data-id="' + trow.MENUID + '" href="#" class="dropdown-toggle" style="height:34px;">';
                            _html += '<span>' + trow.MODULENAME + '</span><i class="fa fa-angle-right drop-icon"></i></a>';
                            var tchildMenus = trow.children;
                            if (tchildMenus && tchildMenus.length > 0) { //循环三级菜单
                                _html += '<ul class="submenu three-menu-list">';
                                $.each(tchildMenus, function (i) {
                                    var srow = tchildMenus[i];
                                    _html += '<li><i style="color: #66D2D3;position: absolute;padding-left: 45px;line-height: 30px;" class="' + srow.ICON + ' fa-fw p-icon"></i>';
                                    _html += '<a class="menuItem" data-id="' + UrlDoMain[systemid] + srow.URL + '" href="' + UrlDoMain[systemid] + srow.URL + '" data-index="' + srow.MENUID + '">' + srow.MODULENAME + '</a>';
                                    _html += '</li>';
                                });
                                _html += '</ul>';
                            }
                            _html += '</li>';
                        }
                    });
                    _html += '</ul>';
                }
                _html += '</li>';
            });
            $('#sidebar-nav ul li[data-type="m"]').remove();
            $("#sidebar-nav ul").prepend(_html);
            $.nfinetab.init();
        };
    });
}
function toMap(data) {
    //转化成树结构 
    let result = []
    if (!Array.isArray(data)) {
        return result
    }
    data.forEach(item => {
        delete item.children;
    });
    let map = {};
    data.forEach(item => {
        map[item.MODULEID] = item;
    });
    data.forEach(item => {
        let parent = map[item.PMODULEID];
        if (parent) {
            (parent.children || (parent.children = [])).push(item);
        } else {
            result.push(item);
        }
    });
    return result;
}
var erpDomain = "";
//添加vue模块
var Index = new Vue({
    el: '#Badge',
    data: {
        DrawerModel: false,
        BadgeNO: 0,
        notices: (h) => {
            return h('div', [
                h('span', '通知公告'),
                h('Badge', {
                    props: {
                        count: 0
                    }
                })
            ])
        },
        dclrw: (h) => {
            return h('div', [
                h('span', '待处理任务'),
                h('Badge', {
                    props: {
                        count: 0
                    }
                })
            ])
        },
        alerts: (h) => {
            return h('div', [
                h('span', '预警'),
                h('Badge', {
                    props: {
                        count: 0
                    }
                })
            ])
        },
        dclrwcolDef: [
                { title: '菜单名称', key: 'NAME'},
                { title: '单据号', key: 'BILLID'},
                { title: '门店', key: 'BRANCHMC'}],
        dclrwdataDef: [],
        noticesdata: [],
        alertcolDef: [
                { title: '预警名称', key: 'MC' },
                {
                    title: '预警数量', key: 'COUNT',
                    render: (h, params) => {
                        const row = params.row;
                        const color = row.COUNT ===0 ? 'success' : 'error';
                        const text = row.COUNT;
                        return h('Tag', {
                            props: {
                                type: 'dot',
                                color: color
                            }
                        }, text);
                    }
                },
                {
                    title: ' ', key: '',
                    render: (h, params) => {
                        return h('div',
                            [params.row.COUNT>0 && h('Button', {
                                props: {
                                    type: 'primary'
                                },                                
                                on: {
                                    click: () => {
                                        Index.AlertClick(params.row)
                                    }
                                }
                            }, '浏览')
                        ]);
                    }
                }],
        alertdataDef: [],
        tableh: document.documentElement.clientHeight-100
    },
    mounted: function () {
        this.AllTopData();
        GetLoadMenu();
    },
    methods: {
        Badgeclick: function () {
            this.AllTopData();
            Index.DrawerModel = true;
        },
        AllTopData: function () {
            let _Index = this;
            _.AjaxT('AllTopData', { 1: 1 }, function (data) {     
                erpDomain = data.erpdomain;
                _Index.BadgeNO = parseInt(data.noticecount) + parseInt(data.dclrwcount) + parseInt(data.alertcount);
                _Index.dclrw = (h) => {
                    return h('div', [
                        h('span', '待处理任务'),
                        h('Badge', {
                            props: {
                                count: data.dclrwcount
                            }
                        })
                    ])
                };
                _Index.dclrwdataDef = data.dclrwdata;
                _Index.notices = (h) => {
                    return h('div', [
                        h('span', '通知公告'),
                        h('Badge', {
                            props: {
                                count: data.noticecount
                            }
                        })
                    ])
                };
                _Index.noticesdata = data.noticedata;
                _Index.alerts = (h) => {
                    return h('div', [
                        h('span', '预警'),
                        h('Badge', {
                            props: {
                                count: data.alertcount
                            }
                        })
                    ])
                };
                _Index.alertdataDef = data.alertdata;
            });
        },
        dclrwClick: function (event) {
            Index.DrawerModel = false;   //先关闭抽屉在打开tab
            _.OpenPage({
                id: event.MENUID,
                title: event.NAME,
                url: erpDomain + event.URL + event.BILLID
            })
        },
        //消息详情
        noticeinfo: function (id) {
            let _Index = this;
            Index.DrawerModel = false;   //先关闭抽屉在打开tab            
            _.Ajax('GetNoticeInfo', {
                 id:id
            }, function (data) {
                _Index.$Modal.info({     //打开消息
                    title: data["0"].TITLE,
                    content: "<p style='text-align: right;'><em>发布时间："+data["0"].RELEASE_TIME+"</em></p>"+data["0"].CONTENT,    //拼接发布时间在内容右上角
                    width: 60,
                    scrollable: true,
                    //closable: true,    //是否显示右上关闭按钮
                    onOk: function () {
                        Index.noticeRead(id);
                    }
                });
            });
        },
        //消息已读,并更新消息
        noticeRead: function (id) {
            let _Index = this;
            _.Ajax('NoticeRead', {
                id: id
            }, function (data) {
                _Index.AllTopData();
            });
        },
        //已读未读
        noticeisread: function (type) {
            let _Index = this;
            _.Ajax('GetNoticeData', {
                type: type
            }, function (data) {
                _Index.noticesdata = data.noticedata;
            });
        },
        //打开预警展示
        AlertClick: function (event) {
            Index.DrawerModel = false;   //先关闭抽屉在打开tab
            _.OpenPage({
                id: event.ID,
                title: event.MC+ "浏览", 
                url: erpDomain + "XTGL/AlertShow/AlertShow/" + event.ID
            })
        },
    },
})