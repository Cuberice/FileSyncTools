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

    $(".watch-button").click(null, function () {
        $.ajax({
            url: "/WatchItem",
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

    $(".EditButton").click(null, function () {

        $(".partial-div-class").empty();
    });


//    $(".edit-item-cancel").click(null, function () {
//
//        $(".partial-div-class").empty();
//        return false;
//    });
//
//    $(".edit-item-save").click(null, function () {
//
//        debugger;
//        var $fm = $(this).parents(".edit-form");
//        $.ajax({
//            url: "/Home/UpdateItem",
//            type: 'POST',
//            data: {
//                id: $(this).parents("li").attr("id"),
//                season: $fm.find("#edit-season").val(),
//                episode: $fm.find("#edit-episode").val(),
//                issynced: $fm.find(".edit-issynced").prop("checked"),
//                ismissing: $fm.find(".edit-ismissing").prop("checked")
//            }
//            //            success: function () {
//            //                alert("done");
//            //            },
//        });
//
//        $(".partial-div-class").empty();
//        return false;
//    });


});


