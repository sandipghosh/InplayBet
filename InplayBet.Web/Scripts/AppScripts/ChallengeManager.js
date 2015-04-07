/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {

    this.AddCustomValidationRules = function () {
        $.validator.addMethod("greaterThan", function (value, element, params) {
            if ($(params[0]).val() != '') {
                if (!/Invalid|NaN/.test(parseFloat(value.replace(/[^0-9-.]/g, '')))) {
                    return parseFloat(value.replace(/[^0-9-.]/g, '')) > parseFloat($(params[0]).val().replace(/[^0-9-.]/g, ''));
                }
                return false;
            };
            return true;
        }, 'Must be greater than {1}.');

        $.validator.addMethod("notEqual", function (value, element, param) {
            if (!/Invalid|NaN/.test(parseFloat(value).replace(/[^0-9-.]/g, ''))) {
                return this.optional(element) || value != $(param).val();
            }
            return false
        }, "Please specify a different team.");

        $.validator.addMethod("currency", function (value, element) {
            return this.optional(element) || !/Invalid|NaN/.test(parseFloat(value.replace(/[^0-9-.]/g, '')));
        }, "Must be numeric.");

        $.validator.addMethod("notEqualStr", function (value, element, param) {
            return this.optional(element) || value != $(param).val();
        }, "Please specify a different team.");

        $.validator.addMethod("regexp", function (value, element) {
            return this.optional(element) || /^(([1-9][0-9]*)\/([1-9][0-9]*))$/ig.test(value);
        }, 'Invalid entry. (Allowed num/num)');
    };

    $(document).ready(function () {
        try {
            SetAutoSuggession($('.yellow-box .scroll .outer'));
            AddCustomValidationRules();

            BetFormSubmitHandler();
            HotFixAutocomplete();
        } catch (ex) { log(ex.message); }
    });

    $(document).on('change', '#TeamAId, #TeamBId, #LegueId', function () {
        try {
            var $self = $(this);
            $self.closest('.left-input, .input-full-outer')
                .find('span.field-validation-error').remove();
            $self.closest('.left-input, .input-full-outer')
                .find('input.input-validation-error')
                .removeClass('input-validation-error');
        } catch (ex) {
            log(ex.message);
        }
    })

    $(document).on('change', '#Odds', function () {
        if ($(this).val() != '') {
            $('#WiningTotal').val(CalculateWinningAmount
                (parseFloat($('#BetPlaced').val().replace(/[^0-9-.]/g, '')), $(this).val()))
        }
    });

    this.BetFormSubmitHandler = function () {
        try {
            $('#frmInsertBet').on('submit', function () {
                validationSetup();
                var isValid = $(this).valid();

                if (isValid) {
                    var betPlaced = $(this).find('#BetPlaced').val().replace(/[^0-9-.]/g, '');
                    var winingTotal = $(this).find('#WiningTotal').val().replace(/[^0-9-.]/g, '');

                    $(this).find('#BetPlaced').val(betPlaced);
                    $(this).find('#WiningTotal').val(winingTotal);
                }

                return isValid;
            });
        } catch (ex) {
            log(ex.message);
        }
    }

    this.SetAutoSuggession = function (container) {
        $('a.btn').hide();
        /*$(container).find('#txtTesmA, #txtTesmB').GenericAutocomplete({
            getUrl: '{0}Team/GetTeams'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });

        $(container).find('#txtLegue').GenericAutocomplete({
            getUrl: '{0}Legue/GetLegues'.format(VirtualDirectory),
            postUrl: '{0}Legue/SetLegues'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });*/
    };

    this.InsertUpdateBetSuccessHandler = function (data, context) {
        try {
            var url = '{0}Bet/ShowChallengeStatusMessage?status={{0}}'.format(VirtualDirectory);
            if (data && data.BetId > 0) {
                if (data.BetStatus.toLowerCase() == 'won' &&
                    data.WiningTotal >= parseFloat(appData.WiningBetAmount)) {

                    ShowModal(url.format('Won'), null, '400px',
                        null, null, function () { win.location.reload(); });
                }
                else if (data.BetStatus.toLowerCase() == 'lost') {
                    ShowModal(url.format('Lost'), null, '400px',
                        null, null, function () { win.location.reload(); });
                }
                else {
                    $.ajax({
                        url: '{0}Bet/GetBetsByChallenge'.format(VirtualDirectory),
                        contentType: "application/json",
                        data: { 'challengeId': data.ChallengeId },
                        success: function (result) {
                            if (result) {
                                $('.yellow-box .scroll .outer').html(result);
                                SetAutoSuggession($('.yellow-box .scroll .outer'));
                                BetFormSubmitHandler();
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            log(errorThrown);
                        }
                    });
                }
            }
            else {
                ShowModal(null, '<p>Unable to save. please try it again.</p>', '400px',
                    null, null, null);
            }

        } catch (ex) {
            log(ex.message);
        }
    };

    this.InsertUpdateBetBeginHandler = function () {
        $.blockUI({ message: $("#dataloading") });
    }

    this.InsertUpdateBetCompleteHandler = function () {
        if ($("#dataloading").is(':visible'))
            $.unblockUI();
    }

    this.validationSetup = function () {
        try {
            $('#frmInsertBet').validate({
                debug: true,
                ignore: 'input[name=""]',
                rules: {
                    /*TeamAId: { required: true, min: 1 },
                    TeamBId: { required: true, notEqual: '#TeamAId' },
                    LegueId: { required: true, min: 1 },*/
                    TeamAName: { required: true },
                    TeamBName: { required: true, notEqualStr: '#txtTesmA' },
                    LegueName: { required: true },

                    BetType: { required: true },
                    Odds: { required: true, regexp: true },
                    BetPlaced: { required: true, currency: true },
                    WiningTotal: { required: true, currency: true, greaterThan: ["#BetPlaced", "Bet placed"] }
                },
                messages: {
                    /*TeamAId: {
                        required: "Please select or create new left team",
                        min: "Please select or create new left team"
                    },
                    TeamBId: {
                        required: "Please select or create new right team",
                        min: "Please select or create new right team",
                        notEqual: "Please specify a different team."
                    },
                    LegueId: {
                        required: "Please select or create new legue",
                        min: "Please select or create new legue"
                    },*/
                    TeamAName: {
                        required: "Please select or create new left team"
                    },
                    TeamBName: {
                        required: "Please select or create new right team",
                        notEqual: "Please specify a different team."
                    },
                    LegueName: {
                        required: "Please select or create new legue"
                    },
                    BetType: { required: "Bet type is required" },
                    Odds: { required: "Odds is required" },
                    BetPlaced: {
                        required: "Bet placed is required",
                        currency: "Bet placed must be numeric"
                    },
                    WiningTotal: {
                        required: "Wining total is required",
                        currency: "Wining total must be numeric"
                    }
                },
                errorElement: 'span',
                errorPlacement: function (error, element) {
                    error.addClass('field-validation-error');
                    if ($(element).is('input[type="hidden"]')) {
                        element.next('input[type="text"]').addClass('input-validation-error');
                        error.insertAfter(element.next('input[type="text"]'));
                    }
                    else {
                        element.addClass('input-validation-error');
                        error.insertAfter(element);
                    }
                },
                success: function (element) {
                    element.prev('input, select, textarea')
            	        .removeClass('input-validation-error');
                    element.remove();
                }
            });
        } catch (ex) {
            log(ex.message);
        }
    };

    this.ShowReportWindow = function (reportToUserKey, challengeId, challengeStatus) {
        try {
            $.ajax({
                url: '{0}Bet/ShowReportWindow'.format(VirtualDirectory),
                contentType: "application/json",
                data: {
                    "reportToUserKey": reportToUserKey,
                    "challengeId": challengeId,
                    "challengeStatus": challengeStatus
                },
                success: function (result) {
                    if (result) {
                        ShowModal(null, result, '400px');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    log(errorThrown);
                }
            });
        } catch (ex) {
            log(ex.message);
        }
    };

    this.SubmitReportSuccessHandler = function (data, context) {
        try {
            if (data) {
                if (data.ReportId > 0) {
                    modal.close({
                        closeCallBack: function () {
                            ShowModal(null, '<p style="font-size:16px;">Your report has been successfully submitted.<p>', '400px', null, function () {
                                //setTimeout(modal.close({}), 3000);
                            });
                        }
                    });
                }
            }
        } catch (ex) {
            log(ex.message);
        }
    }

    this.ResetUserAccount = function (url) {
        try {
            var r = confirm("Are you sure to reset your account?");
            if (r == true) {
                window.location.assign(Base64Decode(url));
            }
        } catch (ex) {
            log(ex.message);
        }
    }

    this.CalculateWinningAmount = function (placesAmount, odds) {
        try {
            if (/^(([1-9][0-9]*)\/([1-9][0-9]*))$/ig.test(odds)) {
                var oddsData = odds.split('/');
                if (oddsData.length > 1) {
                    var percentage = ((100 / parseInt(oddsData[1])) * parseInt(oddsData[0]));
                    if (percentage > 0) {
                        var profitAmount = ((percentage / 100) * placesAmount);
                        return (placesAmount + profitAmount).formatMoney(2, $('#currencySymbol').val());
                    }
                }
            }
        } catch (ex) {
            log(ex.message);
        }
        return (0).formatMoney(2, $('#currencySymbol').val());
    }

}(jQuery, window));