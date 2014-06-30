$().ready(function () {

    var $li = $(".ItemLi");
    var $liAnchor = $(".ItemAnchor");
    
    var $watched = $(".is-watched").filter(function () { return this.value === "True"; }); //e
    var $missing = $(".is-missing").filter(function () { return this.value === "True"; }); //a
    var $synced = $(".is-synced").filter(function () { return this.value === "False"; });  //!a
    var $error = $(".is-error").filter(function () { return this.value === "True"; }); //d

    $li.removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d").removeClass("ui-btn-e");
    $liAnchor.addClass("ui-btn-b");

    //Not Synced
    $synced.parents("a").removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d").removeClass("ui-btn-e").addClass("ui-btn-a");

    //Missing
    $missing.parents("a").removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d").removeClass("ui-btn-e").addClass("ui-btn-d");

    //Error
    $error.parents("a").removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d").removeClass("ui-btn-e").addClass("ui-btn-d");
    
    //Watched
    $watched.parents("a").removeClass("ui-btn-a").removeClass("ui-btn-b").removeClass("ui-btn-c").removeClass("ui-btn-d").removeClass("ui-btn-e").addClass("ui-btn-e");

    $(".watch-button").click(null, function () {
        $.ajax({
            url: "/Home/WatchItem",
            type: 'POST',
            data: { id: $(this).parents("li").attr("id"), value: $(this).prop("checked") },

//            success: function () { alert("done"); },
            error: function () { alert("An Error Occured!!"); }
        });
        return false;
    });

    $(".EditButton").click(null, function () {

        $(".partial-div-class").empty();
    });

});


