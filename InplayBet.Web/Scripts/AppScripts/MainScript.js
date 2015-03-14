/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
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
                            data: { 'filter': request.term },
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
                    focus: function (event, ui) {
                        event.preventDefault();
                        this.value = ui.item.label;
                        return false;
                    },
                    select: function (event, ui) {
                        event.preventDefault();
                        $idField.val(ui.item.value);
                        this.value = ui.item.label;
                        return false;
                    }
                });

                $addbtn.on("click", function () {
                    try {
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

}(jQuery));

