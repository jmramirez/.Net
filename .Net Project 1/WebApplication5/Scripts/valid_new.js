// add the rule here
$.validator.addMethod('valueNotEquals', function (value, element, arg) {
    return arg > value;
}, "Value must not equal arg.");

// configure your validation
$("form").validate({
    rules: {
        SelectName: { valueNotEquals: "0" }
    },
    messages: {
        SelectName: { valueNotEquals: "Please select an item!" }
    }
});