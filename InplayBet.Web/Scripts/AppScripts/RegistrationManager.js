﻿/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


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
}(jQuery, window));