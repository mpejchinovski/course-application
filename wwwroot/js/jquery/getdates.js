function setDateOptions(id, url) {
    $.ajax({
        type: "GET",
        url: url + "/" + id + "/dates",
        //data: { id },
        dataType: "json",
        success: function (data) {
            console.log(data);
            $(".course-date-options").empty();
            $.each(data, function (index, row) {
                row.date = row.date.split("T")[0];
                $(".course-date-options").append("<option value='" + row.id + "'>" + row.date + "</option>")
            });
        },
        error: function (req, status, error) {
            console.log(msg);
        }
    });
}

$(document).ready(function() {
    $('.row').ready(function () {
        setDateOptions(1, $('.course-id-select').data('url'));
    });
    $('.course-id-select').change(function () {
        setDateOptions($(this).val(), $(this).data('url'))
    });
});