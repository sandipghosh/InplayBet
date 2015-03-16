/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
    $(document).ready(function () {
        try {
            $('.rank-pagger').pagination({
                items: parseInt($('#TotalRecord').val()),
                itemsOnPage: parseInt($('#PageSize').val()),
                cssStyle: 'light-theme',
                onPageClick: function (pageNumber, event) {

                },
                onInit: function () {
                    PagerInitiation(parseInt($('#PageSize').val()));
                }
            });
        } catch (ex) {
            log(ex.message);
        }
    });
    
    this.PagerInitiation = function (selectedOption) {
        var $list = $('<select>');
        var $items = [10, 20, 50, 100];
        $.each($items, function (index, item) {
            $list.append(new Option(item, item, (selectedOption == item)));
        });
        
        $list.change(function () {
            var optionSelected = parseInt($("option:selected", this).val());
            $('.rank-pagger').pagination('updateItemsOnPage', optionSelected);
            PagerInitiation(optionSelected);
        });
        $('.rank-pagger ul').first().append($list.wrap('<li></li>'));
    };
}(jQuery, window));