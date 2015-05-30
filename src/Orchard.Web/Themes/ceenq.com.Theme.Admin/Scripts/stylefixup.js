$(document).ready(
    function() {
        $("input[type=text]").addClass("form-control");
        $("input[type=text][name='Options.Search']").addClass("text-medium"); //hack to fix search box on user interface
        $("select").addClass("form-control");
        $("textarea").addClass("form-control");
        $("form").addClass("form-horizontal");
        
    }
);