/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {
    $(document).ready(function () {
        try {
            SetAutoSuggession($('.yellow-box .scroll .outer'));
        } catch (ex) { log(ex.message); }
    });

    this.SetAutoSuggession = function (container) {
        $(container).find('#txtTesmA, #txtTesmB').GenericAutocomplete({
            getUrl: '{0}Team/GetTeams'.format(VirtualDirectory),
            postUrl: '{0}Team/SetTeams'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });

        $(container).find('#txtLegue').GenericAutocomplete({
            getUrl: '{0}Legue/GetLegues'.format(VirtualDirectory),
            postUrl: '{0}Legue/SetLegues'.format(VirtualDirectory),
            userKey: $('#CurrentUserKey').val()
        });
    };

    this.InsertBetBeforeSend = function (context) {
        try {
            return true;
        } catch (ex) {
            log(ex.message);
        }
    };

    this.InsertUpdateBetSuccessHandler = function (data, context) {
        try {
            if (data && data.BetId > 0) {
                if (data.BetStatus.toLowerCase() == 'won' &&
                    data.WiningTotal > parseFloat(appData.WiningBetAmount)) {
                    win.location.reload();
                }
                else if (data.BetStatus.toLowerCase() == 'lost') {
                    win.location.reload();
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

        } catch (ex) {
            log(ex.message);
        }
    };

    this.UpdateBetSuccessHandler = function (data, context) {
        try {

        } catch (ex) {
            log(ex.message);
        }
    };

}(jQuery, window));