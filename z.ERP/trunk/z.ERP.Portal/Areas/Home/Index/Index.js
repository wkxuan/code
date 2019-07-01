$(function ($) {
    $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
    $(window).resize(function (e) {
        $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
    });
    $('#sidebar-nav,#nav-col-submenu').on('click', '.dropdown-toggle', function (e) {
        e.preventDefault();
        var $item = $(this).parent();
        if (!$item.hasClass('open')) {
            $item.parent().find('.open .submenu').slideUp('fast');
            $item.parent().find('.open').toggleClass('open');
        }
        $item.toggleClass('open');
        if ($item.hasClass('open')) {
            $item.children('.submenu').slideDown('fast', function () {
                var _height1 = $(window).height() - 92 - $item.position().top;
                var _height2 = $item.find('ul.submenu').height() + 10;
                var _height3 = _height2 > _height1 ? _height1 : _height2;
                $item.find('ul.submenu').css({
                    overflow: "auto",
                    height: _height3
                })
            });
        }
        else {
            $item.children('.submenu').slideUp('fast');
        }
    });
    GetLoadNav(1);
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
        var $item = $('#page-wrapper.nav-small #sidebar-nav > .nav-pills > .open');
        if ($item.hasClass('open')) {
            $item.find('.open .submenu').slideUp('fast');
            $item.find('.open').removeClass('open');
            $item.children('.submenu').slideUp('fast');
        }
        $item.removeClass('open');
    });
    $('body').find('.mobile-search').click(function (e) {
        e.preventDefault();
        $('.mobile-search').addClass('active');
        $('.mobile-search form input.form-control').focus();
    });
    $(document).mouseup(function (e) {
        var container = $('.mobile-search');
        if (!container.is(e.target) && container.has(e.target).length === 0) // ... nor a descendant of the container
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
});
function GetLoadNav(systemid) {

    _.Ajax('GetMenu', {
        Data: {
            PLATFORMID: systemid
        }
    }, function (data) {
        if (data) {
            var menus = data;

            var _html = "";
            $.each(menus.MENU, function (i, row) {
                _html += '<li data-type="m">';
                _html += '<a data-id="' + row.ID + '" href="#" class="dropdown-toggle" style="height:34px"><i class="' + row.ICON + '"></i>';
                _html += '<span>' + row.NAME + '</span><i class="fa fa-angle-right drop-icon"></i></a>';
                var childMenus = row.MENUList;
                if (childMenus && childMenus.length > 0) {
                    _html += '<ul class="submenu">';
                    $.each(childMenus, function (i) {
                        var subrow = childMenus[i];
                        _html += '<li>';
                        _html += '<a class="menuItem" data-id="' + subrow.URL + '" href="' + subrow.URL + '" data-index="' + subrow.ID + '">' + subrow.NAME + '</a>';
                        _html += '</li>';
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
                { title: '分店', key: 'BRANCHMC'}],
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
    },
    created: function () {
        this.AllTopData();    //加载数据不能放到mounted里，会和加载目录异步方法冲突
    },
    mounted: function () {
    },
    methods: {
        Badgeclick: function () {
            this.AllTopData();
            Index.DrawerModel = true;
        },
        AllTopData: function () {
            let _Index = this;
            _.AjaxT('AllTopData', { 1: 1 }, function (data) {     //为防止与目录加载冲突，用同步加载   AjaxT
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
                url: event.DOMAIN+event.URL + event.BILLID
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
                url: event.DOMAIN + "XTGL/AlertShow/AlertShow/" + event.ID
            })
        },
    },
})