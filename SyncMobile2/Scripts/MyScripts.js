$().ready(function () {

    var $li = $(".ItemLi");
    var $liAnchor = $(".ItemAnchor");
    var $btn_watch = $(".watch-button");  //wathced e
   
    
    var $watched = $(".is-watched").filter(function () { return this.value === "True"; }); //e
    var $missing = $(".is-missing").filter(function () { return this.value === "True"; }); //a
    var $synced = $(".is-synced").filter(function () { return this.value === "True"; });  //!a
    var $error = $(".is-error").filter(function () { return this.value === "True"; }); //d

    $li.removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d");
    $liAnchor.addClass("ui-btn-b");

    //Watched
    $watched.parents("a").removeClass("ui-btn-b").addClass("ui-btn-e");

    //Missing
    $missing.parents("a").removeClass("ui-btn-b").removeClass("ui-btn-e").addClass("ui-btn-a");

    //Error
    $error.parents("a").removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-e").addClass("ui-btn-d");

//    var $m = $(".is-missing").filter(function () { return this.value === "True"; }).parents("li");
//    var $n = $m.not(".ui-collapsible");
//    var $p = $m.find("a"); 
//    
//    $n.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
//    $n.addClass("ui-btn-up-b");
//
//    $p.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
//    $p.addClass("ui-btn-up-c");
//
//    $n.find(".ui-block-b").addClass("ui-disabled");

//    $watch.addClass("ui-disabled");
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


