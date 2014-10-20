$(document).ready(function () {
    $(".click").click(function () {
        var target = $(this).parent().children(".expand");
        var targetAddIcon = $(this).parent().find(".addIcon");
        var targetMinusIcon = $(this).parent().find(".minusIcon");
        $(target).slideToggle();
        if (targetAddIcon.hasClass("hidden")) {
            $(targetAddIcon).removeClass("hidden");
        } else {
            $(targetAddIcon).addClass("hidden");
        }
        if (targetMinusIcon.hasClass("hidden")) {
            $(targetMinusIcon).removeClass("hidden");
        } else {
            $(targetMinusIcon).addClass("hidden");
        }

    });
});