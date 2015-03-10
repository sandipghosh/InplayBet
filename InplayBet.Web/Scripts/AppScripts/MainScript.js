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
            url: '',
            minLength: 2
        }, options);
        var $self = this;

        return $self.each(function () {
            var $item = this;
            $item.autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: settings.url,
                        dataType: "json",
                        success: function (data) {
                            var re = $.ui.autocomplete.escapeRegex(request.term);
                            var matcher = new RegExp("^" + re, "i");
                            var result = $.grep(data, function (item) { return matcher.test(item.value); });
                            $item.next('a.btn').toggle($.inArray(request.term, result) < 0);
                            response(result);
                        }
                    });
                },
                minLength: settings.minLength
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