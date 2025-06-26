// Client-side validation and confirmation enhancements
$(function () {
    $('form').each(function () {
        $(this).validate({
            errorClass: 'text-danger',
            errorElement: 'span',
            highlight: function (element) {
                $(element).addClass('is-invalid');
            },
            unhighlight: function (element) {
                $(element).removeClass('is-invalid');
            }
        });
    });
});
