/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../consolelog.min.js" />
/// <reference path="MainScript.js" />


(function ($, win) {

    $(document).ready(function () {
        try {
            $('#txtTesmA, #txtTesmB').GenericAutocomplete({
                getUrl: '{0}Team/GetTeams'.format(VirtualDirectory),
                postUrl: '{0}Team/SetTeams'.format(VirtualDirectory),
                userKey: $('#CurrentUserKey').val()
            });

            $('#txtLegue').GenericAutocomplete({
                getUrl: '{0}Legue/GetLegues'.format(VirtualDirectory),
                postUrl: '{0}Legue/SetLegues'.format(VirtualDirectory),
                userKey: $('#CurrentUserKey').val()
            });
        } catch (ex) { log(ex.message); }
    });

    //this.BetSubmission = function () {
    //    try {
    //        var data = JSON.stringify($('').serializeArray());
    //    } catch (ex) {
    //        log(ex.message);
    //    }
    //};

    //this.BetMarking = function () {
    //    try {

    //    } catch (ex) {
    //        log(ex.message);
    //    }
    //};

    this.InsertBetSuccessHandler = function (frmInsert) {
        try {

        } catch (ex) {
            log(ex.message);
        }
    };
}(jQuery, window));