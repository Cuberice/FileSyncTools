$().ready(function () {

    var $watch = $(".watch-button");
    var $watchdiv = $(".watch-div");
    var sync = $(".is-missing");

    var $m = $(".IsMissing").filter(function () { return this.value === "True"; }).parents("li");
    var $n = $m.not(".ui-collapsible");
    var $p = $m.find("a"); 
    
    $n.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
    $n.addClass("ui-btn-up-b");

    $p.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
    $p.addClass("ui-btn-up-c");

    $n.find(".ui-block-b").addClass("ui-disabled");


//    var watchLi = $watch.parents("li");
//    var watchId = watchLi.attr("id");

    $(".watch-button").click(null, function () {
        $.ajax({
            url: "/Home/WatchItem",
            type: 'POST',
            data: { id: $(this).parents("li").attr("id"), value: $(this).prop("checked") }
//            success: function () {
//                alert("done");
//            },
//            error: function () {
//                alert("error");
//            }
        });
        return false;
    });
});


