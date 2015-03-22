
// Responsive Prestashop
(function (doc) {
    var addEvent = 'addEventListener',
    type = 'gesturestart',
    qsa = 'querySelectorAll',
    scales = [1, 1],
    meta = qsa in doc ? doc[qsa]('meta[name=viewport]') : [];
    function fix() {
        meta.content = 'width=device-width,minimum-scale=' + scales[0] + ',maximum-scale=' + scales[1];
        doc.removeEventListener(type, fix, true);
    }
    if ((meta = meta[meta.length - 1]) && addEvent in doc) {
        fix();
        scales = [.25, 1.6];
        doc[addEvent](type, fix, true);
    }
}(document));
//*************************************************************************************************************************************************************************************************************************

//Tooltip
$(document).ready(function () {
    $("#tmsocial li a img").easyTooltip();
    $("#header_user_info a").easyTooltip();
    $("#tmheaderlinks li:first-child a").easyTooltip();
});

$(document).ready(function () {
    if (jQuery('.container_24').width() > 780) {
        $(this).find("#featured_products ul li .vky").mouseover(function () {
            $(this).next("#featured_products ul li .v").stop(true, true).fadeIn(600, 'linear');
        });

        //$dequeue;

        $("#featured_products ul li .v").mouseleave(function () {
            $("#featured_products ul li .v").stop(true, true).fadeOut(600, 'linear');
        });
    }
});

$(document).ready(function () {
    $('#order_steps li:even').addClass('even');
    $('#order_steps li:odd').addClass('odd');
    $('.list-order-step li').last().addClass('last');
    $('.iosSlider .slider #item').last().addClass('last');
    $('.iosSlider2 .slider #item').last().addClass('last');
    $('.main-mobile-menu ul ul').addClass('menu-mobile-2');
});

jQuery(document).ready(function () {
    jQuery('.menu-mobile  li').has('.menu-mobile-2').prepend('<span class="open-mobile-2"></span>');
});

/* responsive table */
jQuery(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        jQuery('#order-list td.history_link').prepend('<strong>Order Reference:</strong>');
        jQuery('#order-list td.history_date').prepend('<strong>Date: </strong>');
        jQuery('#order-list td.history_price').prepend('<strong>Total price:</strong>');
        jQuery('#order-list td.history_method').prepend('<strong>Payment:</strong>');
        jQuery('#order-list td.history_state').prepend('<strong>Status: </strong>');
        jQuery('#order-list td.history_invoice').prepend('<strong>Invoice: </strong>');
        jQuery('#order-list td.history_detail').prepend('<strong></strong>');
    }
});


jQuery(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        jQuery('#order-detail-content table.multishipping-cart td.cart_product').prepend('<strong>Product:</strong>');
        jQuery('#order-detail-content table.multishipping-cart  td.cart_description').prepend('<strong>Description: </strong>');
        jQuery('#order-detail-content table.multishipping-cart  td.cart_ref').prepend('<strong>Ref.:</strong>');
        jQuery('#order-detail-content table.multishipping-cart  td.cart_quantity').prepend('<strong>Qty:</strong>');
        jQuery('#order-detail-content table.multishipping-cart  td.ship-adress').prepend('<strong>Addresses: </strong>');
        jQuery('#order-detail-content table.multishipping-cart  td.cart_delete').prepend('<strong>Delete: </strong>');
    }
});

// #content column
$(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        $(".column h4,this").toggle(
        function () {
            $(this).next('.column .block_content,this').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            jQuery(this).addClass('mobile-open');
        },
        function () {
            $(this).next('.column .block_content,this').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            jQuery(this).removeClass('mobile-open');
        });
    }
});

// #social_block script

$(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        $(this).find("#social_block h4 ").toggle(
        function () {
            $(this).next('#social_block ul').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('#social_block h4 ').addClass('mobile-open');
        },
        function () {
            $(this).next('#social_block ul').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('#social_block h4 ').removeClass('mobile-open');
        });
    }
});


// #social_block script
$(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        $(this).find("#block_contact_infos h4").toggle(
        function () {
            $(this).next('#block_contact_infos ul').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('#block_contact_infos h4 ').addClass('mobile-open');
        },
        function () {
            $(this).next('#block_contact_infos ul').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('#block_contact_infos h4 ').removeClass('mobile-open');
        }
        );
    }
});

// menu-mobile script
$(document).ready(function () {
    $(this).find(".wrap-title").toggle(
    function () {
        $(this).next('.menu-mobile').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        $('.open-mobile').addClass('mobile-close');
    },
    function () {
        $(this).next('.menu-mobile').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        $('.open-mobile').removeClass('mobile-close');
    }
    );
});

// menu-mobile-2 script
$(document).ready(function () {
    $(".menu-mobile > li  .open-mobile-2,this").toggle(
    function () {
        $(this).next().next('.menu-mobile-2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-close-2');
        $('.menu-mobile-2 .open-mobile-2').addClass('mobile-close-2-2');
    },
    function () {
        $(this).next().next('.menu-mobile-2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-close-2');
    });
});

// #tmfooterlinks script
$(document).ready(function () {
    if (jQuery('.container_24').width() < 450) {
        $("#tmfooterlinks h4,this").toggle(
        function () {
            $(this).next('#tmfooterlinks ul,this').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            jQuery(this).addClass('mobile-open');
        },
        function () {
            $(this).next('#tmfooterlinks ul,this').slideToggle("slow"), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            jQuery(this).removeClass('mobile-open');
        }
        );
    }
});

// menu-mobile-3 script
$(document).ready(function () {
    $(".menu-mobile .menu-mobile-2 .open-mobile-2,this").toggle(
    function () {
        $(this).next('.menu-mobile-2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-close-3');
    },
    function () {
        $(this).next('.menu-mobile .menu-mobile-2 .menu-mobile-2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-close-3');
    });
});

// Desc 
$(document).ready(function () {
    $(".more_info_inner h3 ,this").toggle(
    function () {
        $(this).next('#idTab1,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('#idTab1,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// Data sheet
$(document).ready(function () {
    $(".more_info_inner2 h3 ,this").toggle(
    function () {
        $(this).next('#idTab22,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('#idTab22,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    }
    );
});

// same category

$(document).ready(function () {
    $(".blockproductscategory h3 ,this").toggle(
    function () {
        $(this).next('.container2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('.container2,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// page-product
$(document).ready(function () {
    $(".more_info_inner3 h3 ,this").toggle(
    function () {
        $(this).next('#idTab9,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('#idTab9,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// page-product
$(document).ready(function () {
    $(".more_info_inner4 h3 ,this").toggle(
    function () {
        $(this).next('#idTab4,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('#idTab4,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// page-product
$(document).ready(function () {
    $("#more_info_block > li > a ,this").toggle(
    function () {
        $(this).parent().next('#more_info_sheets,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).parent().next('#more_info_sheets,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// page-product
$(document).ready(function () {
    $("#more_info_block5 h3 ,this").toggle(
    function () {
        $(this).next('.customization_block,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('.customization_block,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// quantityDiscount 
$(document).ready(function () {
    $("#product_comments_block_tab p a").addClass('button');
    $("div#quantityDiscount h3 ,this").toggle(
    function () {
        $(this).next('.table-block,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).addClass('mobile-open');
    },
    function () {
        $(this).next('.table-block,this').slideToggle("slow"), {
            duration: 'slow',
            easing: 'easeOutBounce'
        };
        jQuery(this).removeClass('mobile-open');
    });
});

// language script
$(document).ready(function () {
    if (jQuery('.container_24').width() < 780) {
        $('.inner-carrencies').on('click', function (event) {
            event.stopPropagation();
            if ($('.selected_language.mobile-open').length > 0) {
                $('.countries_ul:visible').slideToggle("slow");
                $('.selected_language').removeClass('mobile-open');
            }
        });
        $('.mobile-link-top h4').on('click', function (event) {
            event.stopPropagation();
            if ($('.selected_language.mobile-open').length > 0) {
                $('.countries_ul:visible').slideToggle("slow");
                $('.selected_language').removeClass('mobile-open');
            }
        });
        $('#header_user').on('click', function (event) {
            event.stopPropagation();
            if ($('.selected_language.mobile-open').length > 0) {
                $('.countries_ul:visible').slideToggle("slow");
                $('.selected_language').removeClass('mobile-open');
            }
        });

        $('.selected_language').click(function (event) {
            event.stopPropagation();
            if ($(this).hasClass('mobile-open')) {
                $(this).removeClass('mobile-open');
                $(this).siblings('.countries_ul').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            } else {
                $('.selected_language.mobile-open').removeClass('.mobile-open').siblings('.countries_ul').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
                $(this).addClass('mobile-open');
                $(this).siblings('.countries_ul').stop(true, true).slideDown(400), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            }
        });
    }
});



// carrencies script
$(document).ready(function () {
    if (jQuery('.container_24').width() < 780) {
        $('.selected_language').on('click', function (event) {
            event.stopPropagation();
            if ($('.inner-carrencies.mobile-open').length > 0) {
                $('.currencies_ul:visible').slideToggle("slow");
                $('.inner-carrencies').removeClass('mobile-open');
            }
        });
        $('.mobile-link-top h4').on('click', function (event) {
            event.stopPropagation();
            if ($('.inner-carrencies.mobile-open').length > 0) {
                $('.currencies_ul:visible').slideToggle("slow");
                $('.inner-carrencies').removeClass('mobile-open');
            }
        });

        $('#header_user').on('click', function (event) {
            event.stopPropagation();
            if ($('.inner-carrencies.mobile-open').length > 0) {
                $('.currencies_ul:visible').slideToggle("slow");
                $('.inner-carrencies').removeClass('mobile-open');
            }
        });

        $('.inner-carrencies').click(function (event) {
            event.stopPropagation();
            if ($(this).hasClass('mobile-open')) {
                $(this).removeClass('mobile-open');
                $(this).siblings('.currencies_ul').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            } else {
                $('.inner-carrencies.mobile-open').removeClass('.mobile-open').siblings('.currencies_ul').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
                $(this).addClass('mobile-open');
                $(this).siblings('.currencies_ul').stop(true, true).slideDown(400), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            }
        });
    }
});


// carrencies script
$(document).ready(function () {
    if (jQuery('.container_24').width() < 780) {
        $('.selected_language').on('click', function (event) {
            event.stopPropagation();
            if ($('.mobile-link-top h4.act').length > 0) {
                $('#mobilelink:visible').slideToggle("slow");
                $('.mobile-link-top h4').removeClass('act');
            }
        });

        $('.inner-carrencies').on('click', function (event) {
            event.stopPropagation();
            if ($('.mobile-link-top h4.act').length > 0) {
                $('#mobilelink:visible').slideToggle("slow");
                $('.mobile-link-top h4').removeClass('act');
            }
        });

        $('#header_user').on('click', function (event) {
            event.stopPropagation();
            if ($('.mobile-link-top h4.act').length > 0) {
                $('#mobilelink:visible').slideToggle("slow");
                $('.mobile-link-top h4').removeClass('act');
            }
        });

        $('.mobile-link-top h4').click(function (event) {
            event.stopPropagation();
            if ($(this).hasClass('act')) {
                $(this).removeClass('act');
                $(this).siblings('#mobilelink').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            } else {
                $('.mobile-link-top h4.act').removeClass('.act').siblings('#mobilelink').stop(true, true).delay(400).slideUp(300), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
                $(this).addClass('act');
                $(this).siblings('#mobilelink').stop(true, true).slideDown(400), {
                    duration: 'slow',
                    easing: 'easeOutBounce'
                };
            }
        });
    }
});

// carrencies script
$(document).ready(function () {
    $('.selected_language').on('click', function (event) {
        event.stopPropagation();
        if ($('#header_user.close-cart').length > 0) {
            $('#cart_block:visible').slideToggle("slow");
            $('#header_user').removeClass('close-cart');
        }
    });

    $('.mobile-link-top h4').on('click', function (event) {
        event.stopPropagation();
        if ($('#header_user.close-cart').length > 0) {
            $('#cart_block:visible').slideToggle("slow");
            $('#header_user').removeClass('close-cart');
        }
    });

    $('.inner-carrencies').on('click', function (event) {
        event.stopPropagation();
        if ($('#header_user.close-cart').length > 0) {
            $('#cart_block:visible').slideToggle("slow");
            $('#header_user').removeClass('close-cart');
        }
    });

    $('#header_user').click(function (event) {
        event.stopPropagation();
        if ($(this).hasClass('close-cart')) {
            $(this).removeClass('close-cart');
            $(this).siblings('#cart_block').stop(true, true).delay(400).slideUp(300), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
        } else {
            $('#header_user.close-cart').removeClass('.close-cart').siblings('#cart_block').stop(true, true).delay(400).slideUp(300), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $(this).addClass('close-cart');
            $(this).siblings('#cart_block').stop(true, true).slideDown(400), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
        }
    });
});

// carrencies 960 script
$(document).ready(function () {
    if (jQuery('.container_24').width() > 780) {
        $(this).find("#currencies_block_top").hover(
        function () {
            $(this).find('.currencies_ul').stop(true, true).slideDown(400), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('.inner-carrencies').addClass('mobile-open');

        },
        function () {
            $(this).find('.currencies_ul').stop(true, true).delay(400).slideUp(300), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('.inner-carrencies').removeClass('mobile-open');
        });
    }
});

// language 960 script
$(document).ready(function () {
    if (jQuery('.container_24').width() > 780) {
        $(this).find("#languages_block_top").hover(
        function () {
            $(this).find('.countries_ul').stop(true, true).slideDown(400), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('.selected_language').addClass('mobile-open');

        },
        function () {
            $(this).find('.countries_ul').stop(true, true).delay(400).slideUp(300), {
                duration: 'slow',
                easing: 'easeOutBounce'
            };
            $('.selected_language').removeClass('mobile-open');
        });
    }
});

// back-top and special script
jQuery(document).ready(function () {
    jQuery('#tmspecials').css({ visibility: 'visible', display: 'block' });
    // hide #back-top first
    jQuery("#back-top").hide();
    // fade in #back-top
    jQuery(function () {
        jQuery(window).scroll(function () {
            if (jQuery(this).scrollTop() > 100) {
                jQuery('#back-top').fadeIn();
            } else {
                jQuery('#back-top').fadeOut();
            }
        });
        // scroll body to 0px on click
        jQuery('#back-top a').click(function () {
            jQuery('body,html').animate({
                scrollTop: 0
            }, 800);
            return false;
        });
    });
});


