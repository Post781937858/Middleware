(function ($) {
    //$.fn.extend({
    //    autoScrollSidebar: function (options) {
    //        var option = $.extend({ target: null, offsetTop: 0 }, options);
    //        var $navItem = option.target;
    //        if ($navItem === null || $navItem.length === 0) return this;

    //        // sidebar scroll animate
    //        var middle = this.outerHeight() / 2;
    //        var top = $navItem.offset().top + option.offsetTop - this.offset().top;
    //        var $scrollInstance = this[0]["__overlayScrollbars__"];
    //        if (top > middle) {
    //            if ($scrollInstance) $scrollInstance.scroll({ x: 0, y: top - middle }, 500, "swing");
    //            else this.animate({ scrollTop: top - middle });
    //        }
    //        return this;
    //    },
    //    addNiceScroll: function () {
    //        if ($(window).width() > 768) {
    //            this.overlayScrollbars({
    //                className: 'os-theme-light',
    //                scrollbars: {
    //                    autoHide: 'leave',
    //                    autoHideDelay: 100
    //                },
    //                overflowBehavior: {
    //                    x: "hidden",
    //                    y: "scroll"
    //                }
    //            });
    //        }
    //        else {
    //            this.css('overflow', 'auto');
    //        }
    //        return this;
    //    }
    //});

    $.fn.extend({
        nestMenu: function (callback) {
            var $this = $(this);
        },
        clearWidgetItems: function () {
            setBadge(false);
            this.children('.dropdown').each(function () {
                $(this).children('.dropdown-menu').each(function () {
                    $(this).children('a').remove();
                });
            });
            return this;
        }
    });

    var formatCategoryName = function (menu) {
        var ret = "";
        if (menu.IsResource === 2) ret = "按钮";
        else if (menu.IsResource === 1) ret = "资源";
        else ret = menu.CategoryName;
        return ret;
    };

    var cascadeMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length === 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, formatCategoryName(menu));
            }
            else {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{5}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{6}</span><span class="menuOrder">{5}</span></div><ol class="dd-list">{4}</ol></li>', menu.Id, menu.Icon, menu.Name, menu.Category, cascadeSubMenu(menu.Menus), menu.Order, formatCategoryName(menu));
            }
        });
        return html;
    };

    var cascadeSubMenu = function (menus) {
        var html = "";
        $.each(menus, function (index, menu) {
            if (menu.Menus.length === 0) {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{4}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{5}</span><span class="menuOrder">{4}</span></div></li>', menu.Id, menu.Icon, menu.Name, menu.Category, menu.Order, formatCategoryName(menu));
            }
            else {
                html += $.format('<li class="dd-item dd3-item" data-id="{0}" data-order="{5}" data-category="{3}"><div class="dd-handle dd3-handle"></div><div class="dd3-content"><div class="checkbox"><label><input type="checkbox" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><div class="radio"><label><input type="radio" name="menu" value="{0}"><span><i class="{1}"></i>{2}</span></label></div><span class="menuType">{6}</span><span class="menuOrder">{5}</span></div><ol class="dd-list">{4}</ol></li>', menu.Id, menu.Icon, menu.Name, menu.Category, cascadeSubMenu(menu.Menus), menu.Order, formatCategoryName(menu));
            }
        });
        return html;
    };

    var setBadge = function (source) {
        var data = $.extend({
            TasksCount: 0,
            AppExceptionsCount: 0,
            DbExceptionsCount: 0,
            MessagesCount: 0,
            NewUsersCount: 0
        }, source);
        $('#msgHeaderTaskBadge').text(data.TasksCount === 0 ? "" : data.TasksCount);
        $('#msgHeaderUserBadge').text(data.NewUsersCount === 0 ? "" : data.NewUsersCount);
        $('#msgHeaderAppBadge').text(data.AppExceptionsCount === 0 ? "" : data.AppExceptionsCount);
        $('#msgHeaderDbBadge').text(data.DbExceptionsCount === 0 ? "" : data.DbExceptionsCount);
        $('#msgHeaderMsgBadge').text(data.MessagesCount === 0 ? "" : data.MessagesCount);
        $('#logoutNoti').text(data.NewUsersCount === 0 ? "" : data.NewUsersCount);
    };
})(jQuery);

$(function () {
    // enable animoation effect
    $('body').removeClass('trans-mute');

    $('button[data-method="clear"]').on('click', function () {
        $(this).parent('.input-group-append').prev('input').val('');
    });

    //// 自动锁屏功能
    //var mousePosition = { screenX: 0, screenY: 0 };
    //var count = 1;
    //var lockScreenPeriod = parseInt($('#lockScreenPeriod').val());
    //if (typeof lockScreenPeriod === 'number' && !isNaN(lockScreenPeriod)) {
    //    var traceMouseOrKey = window.setInterval(function () {
    //        $(document).off('mousemove').one('mousemove', function (e) {
    //            if (mousePosition.screenX !== e.screenX || mousePosition.screenY !== e.screenY) {
    //                mousePosition.screenX = e.screenX;
    //                mousePosition.screenY = e.screenY;

    //                // 计数器归零
    //                count = 1;
    //                return;
    //            }
    //        });
    //    }, 2000);
    //    var lockHandler = window.setInterval(function () {
    //        if (count++ * 5 > lockScreenPeriod) {
    //            window.clearInterval(lockHandler);
    //            window.clearInterval(traceMouseOrKey);
    //            this.location.href = $.formatUrl("Account/Lock");
    //        }
    //    }, 5000);
    //}

    //var $sideMenu = $(".sidebar ul");

    //// breadcrumb
    //var $breadNav = $('#breadNav, .main-header .breadcrumb-item:last');
    //var arch = $sideMenu.find('a.active').last();
    //$breadNav.removeClass('d-none').text(arch.text() || $('title').text());

    //// custom scrollbar
    //$sidebar = $('.sidebar').addNiceScroll().autoScrollSidebar({ target: arch.parent(), offsetTop: arch.parent().innerHeight() / 2 });

    // 大于 768 时考虑网站设置 收缩侧边栏
    if ($(window).width() > 768) {
        var $ele = $('aside');
        var collapsed = $ele.hasClass('collapsed');
        if (collapsed) {
            $('body').addClass('sidebar-open');
            $ele.removeClass('collapsed');
        }
    }

    $('.sidebar-toggle-box').on('click', function () {
        $('body').toggleClass('sidebar-open');
    });

    window.CheckboxHtmlTemplate = '<div class="form-group col-md-3 col-sm-4 col-6"><label title="{3}" data-toggle="tooltip" role="checkbox" aria-checked="false" class="form-checkbox is-{2}"><span class="checkbox-input"><span class="checkbox-inner"></span><input type="checkbox" value="{0}" {2} /></span><span class="checkbox-label">{1}</span></label></div>';

    //$(window).on('resize', function () {
    //    $sidebar.addNiceScroll();
    //});
});