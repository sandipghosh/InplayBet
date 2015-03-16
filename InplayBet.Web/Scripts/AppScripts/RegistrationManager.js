/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {
    var options = {
        thumbBox: '.thumbBox',
        spinner: '.spinner',
        imgSrc: avaterImage
    };
    var cropper;

    $(document).ready(function () {
        try {
            
        } catch (ex) {
            log(ex.message);
        }
    });

    this.OpenAvaterDialog = function () {
        try {
            ShowModal('{0}RegisterUser/ShowImageCropper'.format(VirtualDirectory),
                null, '435px', null, function ($modal) {
                    cropper = $modal.find('.imageBox').cropbox(options);

                    $('#uploadBtn').on('change', function () {
                        try {
                            $('#uploadFile').val($(this).val());
                            var reader = new FileReader();
                            reader.onload = function (e) {
                                options.imgSrc = e.target.result;
                                cropper = $('.imageBox').cropbox(options);
                            }
                            reader.readAsDataURL(this.files[0]);
                            this.files = [];
                        } catch (ex) {
                            log(ex.message);
                        }
                    });

                    $('#btnCrop').on('click', function () {
                        try {
                            var img = cropper.getDataURL();
                            var rawData = cropper.getDataURL().replace('data:image/jpeg;base64,', '');
                            //$('.cropped').append('<img src="' + img + '">');
                            $('#frmSignUp #AvatarPath').val(img);
                            $('.cropped img').attr('src', img);
                        } catch (ex) {
                            log(ex.message);
                        }
                    });

                    $('#btnZoomIn').on('click', function () {
                        try {
                            cropper.zoomIn();
                        } catch (ex) {
                            log(ex.message);
                        }
                    });

                    $('#btnZoomOut').on('click', function () {
                        try {
                            cropper.zoomOut();
                        } catch (ex) {
                            log(ex.message);
                        }
                    });
                }
            );
        } catch (ex) {
            log(ex.message);
        }
    }

    this.SignUpBeforeSendHandler = function (context) {
        try {
            $.blockUI({ message: $("#dataloading") });
        } catch (ex) {
            log(ex.message);
        }
    }

    this.SignUpSuccessHandler = function (data, context) {
        try {
            var redirectionUrl = '{0}Home/Index'.format(VirtualDirectory);
            var msgTemplate = '<div style="display:block; float:left;">{0}{1}{2}</div>'.format(
                '<span style="display:block; float:left;font-weight: bold;font-size: 18px;margin-bottom: 10px;">Congratulation!</span>',
                '<p style="float: left;font-size: 14px;">{0}</p>',
                '<div style="display:block; float:left;"><a style="display: block;text-align: center;background: #63b222;padding: 3px 14px;text-decoration: none;color: #fff;margin-top: 10px;" href="javascript:window.location.assign(\'{1}\')">Close</a><div>');
            var msg = msgTemplate.format('You ara successfully register into inplay bet.<br/>Please visit your mail account to activate your inplay account.', redirectionUrl);

            if (typeof data.UserKey != 'undefined') {
                if (data.UserKey > 0) {
                    if (typeof data.UpdatedBy != 'undefined') {
                        redirectionUrl = '{0}MemberProfile/Index'.format(VirtualDirectory);
                        msg = msgTemplate.format('Your profile has been successfully updated.<br/>Keep betting.', redirectionUrl);
                    }
                    else {
                        if (data.StatusId == 1) {
                            redirectionUrl = '{0}MemberProfile/Index'.format(VirtualDirectory);
                            msg = msgTemplate.format('You are successfully register into inplay bet.<br/>Start betting.', redirectionUrl);
                        }
                    }
                    modal.open({
                        content: msg,
                        width: '400px',
                        openCallBack: function () {
                            $('.modal-close').unbind("click");
                            $('.modal-close').click(function () {
                                window.location.assign(redirectionUrl);
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

}(jQuery, window));