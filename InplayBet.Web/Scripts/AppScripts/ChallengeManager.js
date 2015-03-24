/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {

    this.AddCustomValidationRules = function () {
        $.validator.addMethod("greaterThan", function (value, element, params) {
            if ($(params[0]).val() != '') {
                if (!/Invalid|NaN/.test(parseFloat(value))) {
                    return parseFloat(value) > parseFloat($(params[0]).val());
                }
                return false;
            };
            return true;
        }, 'Must be greater than {1}.');

        jQuery.validator.addMethod("notEqual", function (value, element, param) {
            if (!/Invalid|NaN/.test(parseFloat(value))) {
                return this.optional(element) || value != $(param).val();
            }
            return false
        }, "Please specify a different team.");
    };

    $(document).ready(function () {
        try {
            SetAutoSuggession($('.yellow-box .scroll .outer'));
            AddCustomValidationRules();

            $('#frmInsertBet').submit(function () {
                validationSetup();
                var isValid = $('#frmInsertBet').valid();
                return isValid;
            });
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

    this.SetAutoSuggession = function (container) {
        $(container).find('#txtTesmA, #txtTesmB').GenericAutocomplete({
            getUrl: '{0}Team/GetTeams'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });

        $(container).find('#txtLegue').GenericAutocomplete({
            getUrl: '{0}Legue/GetLegues'.format(VirtualDirectory),
            postUrl: '{0}Legue/SetLegues'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });
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
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            log(errorThrown);
                        }
                    });
                }
            }
            else
            {
                ShowModal(null, '<p>Unable to save. please try it again.</p>', '400px',
                    null, null, null);
            }

        } catch (ex) {
            log(ex.message);
        }
    };

    this.validationSetup = function () {
        try {
            $('#frmInsertBet').validate({
                debug: true,
                ignore: 'input[name=""]',
                rules: {
                    TeamAId: { required: true, min: 1 },
                    TeamBId: { required: true, notEqual: '#TeamAId' },
                    LegueId: { required: true, min: 1 },
                    BetType: { required: true },
                    Odds: { required: true },
                    BetPlaced: { required: true, number: true },
                    WiningTotal: { required: true, number: true, greaterThan: ["#BetPlaced", "Bet placed"] }
                },
                messages: {
                    TeamAId: {
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
                    },
                    BetType: { required: "Bet type is required" },
                    Odds: { required: "Bet type is required" },
                    BetPlaced: {
                        required: "Bet placed is required",
                        number: "Bet placed must be numeric"
                    },
                    WiningTotal: {
                        required: "Wining total is required",
                        number: "Wining total must be numeric"
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
                        ShowModal(null, result,'400px');
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
                            ShowModal(null, "<p>Your report has been successfully submitted.<p>", '400px', null, function () {
                                setTimeout(modal.close({}), 3000);
                            });
                        }
                    });
                }
            }
        } catch (ex) {
            log(ex.message);
        }
    }

}(jQuery, window));