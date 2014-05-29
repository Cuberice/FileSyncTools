
$(document).ready(function () {
    
    var $m = $(".IsMissing ").filter(function () { return this.value === "True"; }).parents("li");
    var $n = $m.not(".ui-collapsible");
    var $p = $m.find("a"); 
    
    $n.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
    $n.addClass("ui-btn-up-e");

    $p.removeClass("ui-btn-up-a").removeClass("ui-btn-up-b").removeClass("ui-btn-up-c");
    $p.addClass("ui-btn-up-c");

    $n.find(".ui-block-b").addClass("ui-disabled");
});

//$(document).ready(function () {
//    $("a").click(function (event) {
//        alert("Clicked!!");
//    });
//});


