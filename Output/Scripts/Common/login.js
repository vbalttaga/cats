/*----------------------------------------------------:Login--------------------------------------------*/
$(document).ready(function () {
    $("[name=Login]").focus();
    setupInputBehavior();
});

function doLogin() {
    $(".input").removeClass("input-error", 100);    
    $(".error-message").fadeOut();
    if (form_validation($("form")),true) {
        $(".loading").show();
        $.post($('form').attr("action"), $('form').serialize(), function (data) {

            if (data["Result"] == 1) {
                window.location = data["RedirectURL"];
            } else {
                if (data["ErrorFields"]) {
                    $.each(data["ErrorFields"], function (i, item) {
                        $("" + item).addClass("input-error", "slow");
                        if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                            $("" + item).next().find(".error-message-text").html(data["Message"]);
                            $(".error-message").fadeIn("slow");
                        }
                        else {
                            $("" + item).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                            $("" + item).next().find(".error-message-text").html(data["Message"]);
                        }
                    });
                }
                $(".loading ").hide();
            }

        });
    }

    return false;
}

function doLoginMpass(url) {

    $(".input").removeClass("input-error", 100);
    $(".error-message").fadeOut();
    $(".loading").show();
    $.post(url, $('form').serialize(), function (data) {

        if (data["Result"] == 1) {
            window.location = data["RedirectURL"];
        } else {
            if (data["ErrorFields"]) {
                $.each(data["ErrorFields"], function (i, item) {
                    $("" + item).addClass("input-error", "slow");
                    if ($("" + item).next().size() > 0 && $("" + item).next().hasClass("error-message")) {
                        $("" + item).next().find(".error-message-text").html(data["Message"]);
                        $(".error-message").fadeIn("slow");
                    }
                    else {
                        $("" + item).after("<div class='error-message'><div class='error-message-decor'></div><div class='error-message-text'></div></div>");
                        $("" + item).next().find(".error-message-text").html(data["Message"]);
                    }
                });
            }
            $(".loading ").hide();
        }

    });

    return false;
}


function login_error(messsage) {
    if ($(".password-links-box").is(":visible")) {
        $(".password-links-box").animate({ opacity: 0 }, "slow", function () {
            $(".password-links-box").hide();
            show_error_message(messsage);
        });
    }
    else {
        show_error_message(messsage);
    }
}

function setupInputBehavior() {
    $(".input").on("input", function(e) {
        $(".input").removeClass("input-error").siblings(".error-message").fadeOut();
    });
}