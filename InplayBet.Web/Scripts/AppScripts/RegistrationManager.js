/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {
    this.SignUpBeforeSendHandler = function (context) {
        try {
            $.blockUI({ message: $("#dataloading") });
        } catch (ex) {
            log(ex.message);
        }
    }
    this.SignUpSuccessHandler = function (data, context) {
        try {
            var msgHtml = '<div style="display:block; float:left;">{0}{1}{2}</div>'.format(
                '<span style="display:block; float:left;font-weight: bold;font-size: 18px;margin-bottom: 10px;">Congratulation!</span>',
                '<p style="float: left;font-size: 14px;">You ara successfully register into inplay bet.<br/>Please visit your mail account to activate your inplay account.</p>',
                '<div style="display:block; float:left;"><a style="display: block;text-align: center;background: #63b222;padding: 3px 14px;text-decoration: none;color: #fff;margin-top: 10px;" href="javascript:window.location.assign(\'{0}Home/Index\')">Close</a><div>'.format(VirtualDirectory)
                );

            if (typeof data.UserKey != 'undefined') {
                if (data.UserKey > 0) {

                    modal.open({
                        content: msgHtml,
                        width: '400px',
                        openCallBack: function () {
                            $('.modal-close').unbind("click");
                            $('.modal-close').click(function () {
                                window.location.assign('{0}Home/Index'.format(VirtualDirectory));
                            });
                        }
                    });
                }
            }
            else {
                $('#frmSignUp').html($(data).find('#frmSignUp').html());
                $.validator.unobtrusive.parse($('#frmSignUp'));
            }
        } catch (ex) {
            log(ex.message);
        }
    }

    this.SignUpCompletionHandler = function (context) {
        try {
            $.unblockUI();
        } catch (ex) {
            log(ex.message);
        }
    }

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