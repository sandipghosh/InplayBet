/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
    //var options = {
    //    thumbBox: '.thumbBox',
    //    spinner: '.spinner',
    //    imgSrc: 'avatar.png'
    //};

    //var cropper = $('.imageBox').cropbox(options);

    //$('#file').on('change', function () {
    //    var reader = new FileReader();
    //    reader.onload = function (e) {
    //        options.imgSrc = e.target.result;
    //        cropper = $('.imageBox').cropbox(options);
    //    }
    //    reader.readAsDataURL(this.files[0]);
    //    this.files = [];
    //});

    //$('#btnCrop').on('click', function () {
    //    var img = cropper.getDataURL();
    //    var rawData = cropper.getDataURL().replace('data:image/jpeg;base64,', '');
    //    //$('.cropped').append('<img src="' + img + '">');
    //    UploadAvatarImage(rawData);
    //});

    //$('#btnZoomIn').on('click', function () {
    //    cropper.zoomIn();
    //});
    //$('#btnZoomOut').on('click', function () {
    //    cropper.zoomOut();
    //});

    //this.UploadAvatarImage = function (rawData) {
    //    try {
    //        $.ajax({
    //            type: 'POST',
    //            url: '{0}/Editor/SetWatermarkForPreview'.format(virtualDirectory),
    //            data: JSON.stringify({
    //                'imageRawData': rawData,
    //                'imageJSONData': jsonData
    //            }),
    //            contentType: 'application/json; charset=utf-8',
    //            dataType: 'json',
    //            success: function (draftImage) {
    //            }
    //        });
    //    } catch (ex) { log(ex.message); }
    //};

}((jQuery, window)));

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

