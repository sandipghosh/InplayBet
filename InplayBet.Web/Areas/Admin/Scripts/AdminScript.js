/// <reference path="../../../Scripts/jquery-2.1.3.min.js" />
/// <reference path="../../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../../Scripts/jquery-2.1.3.js" />
/// <reference path="../../../Scripts/consolelog.min.js" />

(function ($, win) {
    this.SummaryReportGridInitialization = function (schemaData) {
        try {
            if (schemaData != '') {
                var schemaJsonData = JSON.parse(Base64Decode(schemaData));
                if (typeof schemaJsonData != 'undefined' &&
                    schemaJsonData instanceof Object == true) {

                    var $gridElement = $('#grid');

                    $gridElement.SetupGrid({
                        modelSchema: schemaJsonData,
                        datatype: 'json',
                        pagerid: '#pager',
                        renderURL: '{0}Admin/SummaryReport/GetUserSummary'.format(VirtualDirectory),
                        recordtext: 'Total Orders {2}',
                        searchOperators: true,
                        //autowidth: false,
                        //shrinkToFit: false,
                        insertMode: 'none',
                        delete_func: function (rowid) {
                            deleteGridRow(rowid);
                        }
                    });
                }
            }
        } catch (ex) {
            log(ex.message);
        }
    };

    this.CheatReportGridInitialization = function (schemaData, schemaSubData) {
        try {
            if (schemaData != '' && schemaSubData != '') {
                var schemaJsonData = JSON.parse(Base64Decode(schemaData));
                var schemaJsonSubData = JSON.parse(Base64Decode(schemaSubData));

                if (schemaJsonData instanceof Object == true &&
                    schemaJsonSubData instanceof Object == true) {
                    $gridElement = $('#grid');

                    var subGridHandler = subGridRowExpandedHandler({
                        modelData: schemaJsonSubData,
                        renderURL: '{0}Admin/CheatReport/GetReportingUsers'.format(VirtualDirectory),
                        parentGridElement: $gridElement,
                        keyFieldName: 'UserKey',
                        recordtext: '',
                    });

                    $gridElement.SetupGrid({
                        modelSchema: schemaJsonData,
                        datatype: 'json',
                        pagerid: '#pager',
                        renderURL: '{0}Admin/CheatReport/GetReportedUsers'.format(VirtualDirectory),
                        recordtext: 'Total Users {2}',
                        searchOperators: true,
                        grouping: true,
                        subGrid: true,
                        //subGridOptions: defaultSubGridOptions,
                        //subGridRowExpanded: subGridHandler.handler,
                        insertMode: 'none'
                    });
                }
            }
        } catch (ex) {
            log(ex.message);
        }
    };

    this.deleteGridRow = function (rowid) {
        alert(rowid);
    };
}(jQuery, window));