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
        dclrwcolDef: [
                { title: '菜单号', key: 'MENUID'},
                { title: '菜单名称', key: 'NAME'},
                { title: '分店', key: 'BRANCHMC'},
                { title: '数量', key: 'COUNT' }],
        dclrwdataDef: [],
    },
    mounted: function () {
        //_.Ajax('AllTopData', function (data) {
        //    debugger
        //    this.BadgeNO = data.dclrwcount;
        //    this.dclrw = (h) => {
        //        return h('div', [
        //            h('span', '待处理任务'),
        //            h('Badge', {
        //                props: {
        //                    count: data.dclrwcount
        //                }
        //            })
        //        ])
        //    };
        //    this.dclrwdataDef = data.dclrwdata;
        //    Vue.set(this, 'BadgeNO', data.dclrwcount);
        //});
        this.BadgeNO = 10;
        this.dclrw = (h) => {
            return h('div', [
                h('span', '待处理任务'),
                h('Badge', {
                    props: {
                        count: 10
                    }
                })
            ])
        };
    },
    methods: {
        Badgeclick: function () {
            Index.DrawerModel = true;
        },
    },
})