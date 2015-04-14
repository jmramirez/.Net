$(function () {
    $.validator.addMethod('Required', function (value, element) {
        var $module = $(element).parents('form');
        
        return $module.find('input.checkbox:checked').length;
    },"Select One or More Courses");

    $.validator.addClassRules('Required', { 'Required': true});
});