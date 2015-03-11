/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
    var options = {
        thumbBox: '.thumbBox',
        spinner: '.spinner',
        imgSrc: 'avatar.png'
    };

    var cropper = $('.imageBox').cropbox(options);

    $(document).ready(function () {
        try {

        } catch (ex) { log(ex.message); }
    });

    $('#file').on('change', function () {
        var reader = new FileReader();
        reader.onload = function (e) {
            options.imgSrc = e.target.result;
            cropper = $('.imageBox').cropbox(options);
        }
        reader.readAsDataURL(this.files[0]);
        this.files = [];
    });

    $('#btnCrop').on('click', function () {
        var img = cropper.getDataURL();
        var rawData = cropper.getDataURL().replace('data:image/jpeg;base64,', '');
        //$('.cropped').append('<img src="' + img + '">');
        UploadAvatarImage(rawData);
    });

    $('#btnZoomIn').on('click', function () {
        cropper.zoomIn();
    });
    $('#btnZoomOut').on('click', function () {
        cropper.zoomOut();
    });

    this.UploadAvatarImage = function (rawData) {
        try {
            $.ajax({
                type: 'POST',
                url: '{0}/Editor/SetWatermarkForPreview'.format(virtualDirectory),
                data: JSON.stringify({
                    'imageRawData': rawData,
                    'imageJSONData': jsonData
                }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (draftImage) {
                }
            });
        } catch (ex) { log(ex.message); }
    };

}((jQuery, window)));

var source = ["Apples", "Oranges", "Bananas"];

(function ($) {
    $.fn.genericAutocomplete = function (options) {
        var settings = $.extend({
            getUrl: '',
            postUrl: '',
            minLength: 3,
            userKey: 0
        }, options);
        var $self = this;

        return $self.each(function () {
            var $item = this;
            var cache = {};

            $item.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: settings.getUrl,
                        dataType: "json",
                        success: function (data) {
                            var re = $.ui.autocomplete.escapeRegex(request.term);
                            var matcher = new RegExp("^" + re, "i");
                            var result = $.grep(data, function (item) { return matcher.test(item.label); });
                            $item.next('a.btn').toggle(result.length <= 0)
                                .data('searchtext', request.term);
                            response($.map(result, function (c) {
                                return {
                                    label: c.Name,
                                    value: c.Id
                                }
                            }));
                        }
                    });
                },
                minLength: settings.minLength,
                select: function (event, ui) {
                    $item.previous('input[type=text]').val(ui.item.value);
                }
            });

            $item.next('a.btn').on("click", function () {
                var $self = $(this);
                $.ajax({
                    url: settings.postUrl,
                    type: "POST",
                    dataType: "json",
                    data: { name: $self.data('searchtext'), userId: settings.userKey },
                    success: function (data) {
                        $('#empf').autocomplete("option", { source: colors });
                    }
                });
                source.push($("#auto").val());
                $(this).hide();
            });
        });
    };

}(jQuery));

$(function () {
    //$('#txtTesmA, #txtTesmB')


    $("#auto").autocomplete({
        source: function (request, response) {
            var result = $.ui.autocomplete.filter(source, request.term);
            $("#add").toggle($.inArray(request.term, result) < 0);
            response(result);
        }
    });

    $("#auto").autocomplete({
        source: function (request, response) {
            $.getJSON("search.php", { // get the json here
                term: extractLast(request.term) // function further, up not important
            }, response);
        }
    });

    $("#add").on("click", function () {
        source.push($("#auto").val());
        $(this).hide();
    });
});