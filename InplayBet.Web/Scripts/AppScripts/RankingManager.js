/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../consolelog.min.js" />

(function ($, win) {
    var orderByStr = '';

    $(document).ready(function () {
        try {
            PagerInitialization(parseInt($('#PageSize').val()), parseInt($('#TotalRecord').val()));
            InitiateBetRange(parseInt($('#maxBet').val()));
            $('#UserIdSearch').GenericAutocomplete({
                getUrl: '{0}RegisterUser/GetUsers'.format(VirtualDirectory)
            });
        } catch (ex) {
            log(ex.message);
        }
    });

    this.SetPositionIndex = function (currentPage, pageItemCount) {
        try {
            $('.position').each(function (index, element) {
                var startingPos = (currentPage == 1) ? 1 : ((pageItemCount * (currentPage - 1)) + 1);
                var position = (index + startingPos);

                switch (position) {
                    case 1:
                        $(element).text(position + 'st');
                        break;
                    case 2:
                        $(element).text(position + 'nd');
                        break;
                    case 3:
                        $(element).text(position + 'rd');
                        break;
                    default:
                        $(element).text(position + 'th');
                        break;
                }
            });
        }
        catch (ex) { log(ex.message); }
    }

    this.PagerInitialization = function (pageSize, totalRecord) {
        try {
            $('.rank-pagger').pagination({
                items: totalRecord,
                itemsOnPage: pageSize,
                cssStyle: 'light-theme',
                onPageClick: function (pageNumber, event) {
                    SearchMember(pageNumber, orderByStr);
                },
                onInit: function () {
                    //PagerAfterInitiation(pageSize);
                }
            });
        } catch (ex) {
            log(ex.message);
        }
    };

    this.SearchMember = function (pageNumber, sortOrder) {
        try {
            $.ajax({
                url: $('#PagingUrl').val(),
                type: 'GET',
                contentType: "application/json",
                data: GetSearchDataFilter(pageNumber, sortOrder),
                success: function (result, textStatus, jqXHR) {
                    if (result) {
                        $('.leadbroad-block').html($(result).filter('.leadbroad-block').html());
                        ImageError();
                        SetFollowingImage();
                        $('.rank-pagger').pagination('updateItems', parseInt($(result).filter('#extTotalRecord').val()));
                        SetPositionIndex(parseInt($('.rank-pagger').pagination('getCurrentPage')), parseInt($('#PageSize').val()));
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

            orderByStr = sortOrder;
        } catch (ex) {
            log(ex.message);
        }
    };

    var GetSearchDataFilter = function (pageIndex, orderby) {
        try {
            var url = $('#PagingUrl').val();
            var filter = '';
            var rangeData = $("#betRange").data('ionRangeSlider');

            if (typeof rangeData != 'undefined') {
                filter = ($('#UserIdSearch').val() != '') ?
                    'UserId.StartsWithSearchEx("{0}")'.format($('#UserIdSearch').val()) : '';

                if (filter != '')
                    filter = Base64Encode('{0} and (TotalBets >= {1} and TotalBets <= {2})'.format(filter, rangeData.old_from, rangeData.old_to));
                else
                    filter = Base64Encode('(TotalBets >= {0} and TotalBets <= {1})'.format(rangeData.old_from, rangeData.old_to));
            }

            if (url.contains("GetRankByPage")) {
                return {
                    "pageIndex": pageIndex,
                    "filter": filter
                }
            }
            else if (url.contains("GetMemberByPage")) {
                return {
                    "pageIndex": pageIndex,
                    "filter": filter,
                    "orderBy": orderby
                }
            }
            return null;
        } catch (ex) {
            log(ex.message);
        }
    };

    this.PagerAfterInitiation = function (selectedOption) {
        try {
            var $listContainer = $('<li></li>')
            var $list = $('<select class="itemCount">');
            var $items = [10, 20, 30, 50, 100];
            $.each($items, function (index, item) {
                $list.append(new Option(item, item, (selectedOption == item)));
            });

            $list.change(function () {
                var optionSelected = parseInt($("option:selected", this).val());
                $('.rank-pagger').pagination('updateItemsOnPage', optionSelected);
                PagerInitiation(optionSelected);
            });
            $listContainer.append($list);
            $('.rank-pagger ul li:has(select.itemCount)').remove();
            $('.rank-pagger ul').first().append($listContainer);
        } catch (ex) {
            log(ex.message);
        }
    };

    this.InitiateBetRange = function (maxRange) {
        try {
            $("#betRange").ionRangeSlider({
                type: "double",
                min: 0,
                max: maxRange,
                grid: true,
                force_edges: true,
                onFinish: function (data) {
                    /*var searchString = '(TotalBets >= {0} and TotalBets <= {1})'.format(data.from, data.to);
                    if (orderByStr == '') {
                        orderByStr = searchString;
                    }
                    else {
                        orderByStr = '{0} and {1}'.format(orderByStr, searchString);
                    }*/
                }
            });
        } catch (ex) {
            log(ex.message);
        }
    }
}(jQuery, window));