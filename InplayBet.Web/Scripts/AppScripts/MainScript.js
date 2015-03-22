/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
    $(document).ready(function () {
        $('body').css({ 'overflow': 'hidden' });
        $(window).load(function () {
            $('.loading-overlay').fadeOut('slow', function () {
                $('body').css({ 'overflow': '' });
            });
        });
        $('input[readonly]').on('keydown', function (e) {
            if (e.which === 8) {
                e.preventDefault();
            }
        });

        SetFollowingImage();
        ManageTopNavigation();
    });

    this.SetFollowingImage = function ($elm) {
        try {
            var processLink = function ($e) {
                var status = $e.data('status');
                $e.attr('title', (status == 1 ? 'Follow the user' : 'Un-follow the user'));
                $e.css('background-image', (status == 1 ? 'url({0}Styles/images/thumb-up-32.png)'
                    : 'url({0}Styles/images/thumb-down-32.png)').format(VirtualDirectory));
            };

            if (typeof $elm == 'undefined') {
                $('a.follow-link[data-status]').each(function (index, element) {
                    processLink($(element));
                });
            }
            else {
                processLink($elm);
            }

        } catch (ex) {
            log(ex.message);
        }
    };

    this.Follow = function (element, event, folowBy, folowTo, updateElement) {
        try {
            event.preventDefault();
            event.stopPropagation();

            var $self = $(element);
            var status = $self.data('status');
            $.ajax({
                url: $('#PagingUrl').val(),
                type: 'GET',
                dataType: "json",
                contentType: "application/json",
                data: {
                    "followBy": folowBy, "followTo": folowTo
                },
                success: function (result, textStatus, jqXHR) {
                    if (result) {
                        $self.data('status', status == 1 ? 2 : 1);
                        $(updateElement).text(result.followCount);
                        SetFollowingImage($self);
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
            return false;
        } catch (ex) {
            log(ex.message);
        }
    };

    this.ManageTopNavigation = function () {
        try {
            $('ul.nav li').removeClass('active');
            var urlPath = window.location.pathname;
            var $link = $('ul.nav li a[href="' + urlPath + '"]');

            if ($link.length > 0)
                $link.closest('li').addClass('active');
            else {
                if (window.location.pathname.contains('contains'))
                    $('#lnkRegisterUser').closest('li').addClass('active');
                else
                    $('ul.nav li:eq(0)').addClass('active');
            }

        } catch (ex) {
            log(ex.message);
        }
    };

    this.SignInSuccessHandler = function (data, context) {
        try {
            if (typeof data.Status != 'undefined') {
                if (data.Status) {
                    win.location.assign(data.Url);
                }
            }
            else {
                $('#frmSignIn').html($(data).find('#frmSignIn').html());
                $.validator.unobtrusive.parse($('#frmSignIn'));
            }
        } catch (ex) {
            log(ex.message);
        }
    };

    this.RedirevtToUserProfile = function (userId) {
        try {
            win.location.assign('{0}MemberProfile/ViewProfile/{1}'.format(VirtualDirectory, userId));
        } catch (ex) {
            log(ex.message);
        }
    };

    $.ajaxSetup({
        beforeSend: function (jqXHR, settings) {
            if (!settings.url.contains('Keepalive')) {
                loadingCounter += 1;
                $(document).css('cursor', 'wait !important');
                try {
                    $.blockUI({ message: $("#dataloading") });
                } catch (ex) { }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert('error');
        },
        complete: function (jqXHR, textStatus) {
            if (loadingCounter > 1) {
                loadingCounter -= 1
            }
            else {
                loadingCounter = 0;
                try {
                    $.unblockUI();
                } catch (ex) { }
            }
            $(document).css('cursor', 'default !important');
        }
    });

    this.ShowModal = function (uri, content, windowWidth, beforeShowCallback, afterShowCallback) {
        if (uri != null) {
            $.ajax({
                url: uri,
                type: 'GET',
                success: function (result, textStatus, jqXHR) {
                    if (result) {
                        if (typeof beforeShowCallback !== 'undefined' && beforeShowCallback != null) {
                            var callbackReturn = beforeShowCallback();
                            if (!callbackReturn) return;
                        }

                        modal.open({
                            content: result,
                            width: (windowWidth == undefined || windowWidth == null) ?
                                '735px' : windowWidth,
                            openCallBack: afterShowCallback
                        });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }
        else {
            modal.open({
                content: content,
                width: (windowWidth == undefined || windowWidth == null) ?
                    '735px' : windowWidth,
                openCallBack: afterShowCallback
            });
        }
    };
}(jQuery, window));

(function ($) {
    $.fn.GenericAutocomplete = function (options) {
        try {
            var settings = $.extend({
                getUrl: '',
                postUrl: '',
                minLength: 2,
                userKey: 0
            }, options);
            var $self = this;

            return $self.each(function () {
                var $item = $(this);
                var $addbtn = $item.next('a.btn');
                var $idField = $item.prev('input[type=hidden]');
                $addbtn.hide();

                $item.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: settings.getUrl,
                            dataType: "json",
                            contentType: "application/json",
                            data: {
                                'filter': request.term
                            },
                            success: function (data) {
                                if (data) {
                                    var re = $.ui.autocomplete.escapeRegex(request.term);
                                    var matcher = new RegExp("^" + re, "i");
                                    var result = $.grep(data, function (item) {
                                        return matcher.test(item.label);
                                    });
                                    $addbtn.toggle(result.length <= 0);
                                    $addbtn.data('searchtext', request.term);
                                    response($.map(data, function (item) {
                                        return item;
                                    }));
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                log(errorThrown);
                            }
                        });
                    },
                    minLength: settings.minLength,
                    change: function (event, ui) {
                        //$idField.val('').change();
                    },
                    focus: function (event, ui) {
                        event.preventDefault();
                        $idField.val(ui.item.value).change();
                        this.value = ui.item.label;
                        return false;
                    },
                    select: function (event, ui) {
                        event.preventDefault();
                        $idField.val(ui.item.value).change();
                        this.value = ui.item.label;
                        return false;
                    }
                });

                $addbtn.on("click", function (event) {
                    try {
                        event.preventDefault();
                        var $self = $(this);
                        $.ajax({
                            url: settings.postUrl,
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json",
                            data: JSON.stringify({ searchName: $self.data('searchtext'), userId: settings.userKey }),
                            success: function (data) {
                                if (data) {
                                    $item.val(data.label);
                                    $idField.val(data.value);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                log(errorThrown);
                            }
                        });
                        $self.hide();
                    } catch (ex) {
                        log(ex.message);
                    }
                });
            });
        } catch (ex) {
            log(ex.message);
        }
    };

    this.HotFixAutocomplete = function () {
        $('.ui-autocomplete').on('touchstart', 'li.ui-menu-item', function () {
            var $container = $(this).closest('.ui-autocomplete'),
                $item = $(this);

            //if we haven't closed the result box like we should have, simulate a click on the element they tapped on.
            function fixitifitneedsit() {
                if ($container.is(':visible') && $item.hasClass('ui-state-focus')) {

                    $item.trigger('click');
                    return true; // it needed it
                }
                return false; // it didn't
            }

            setTimeout(function () {
                if (!fixitifitneedsit()) {
                    setTimeout(fixitifitneedsit, 600);
                }
            }, 600);
        });
    };
}(jQuery));

