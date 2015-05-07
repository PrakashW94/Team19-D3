$(document).ready(function ()
{
    $("#TimetableType").change(function ()
    {
        if ($("#TimetableType").val() == "0")
        {
            var windowLocation = window.location.href + "/fillSelector";
            $.ajax(
            {
                url: windowLocation,
                type: "POST",
                data: { type: 0 },
                success: function (lecturers)
                {
                    $("#Selector").empty();
                    for (var i = 0; i < lecturers.length; i++)
                    {
                        $("#Selector").append("<option value="+lecturers[i].Value+">" + lecturers[i].Text + "</option>");
                    }
                },
                error: function ()
                {
                    alert("Error getting lecturers!");
                }
            });
        }
        else if ($("#TimetableType").val() == "1")
        {
            var windowLocation = window.location.href + "/fillSelector";
            $.ajax(
            {
                url: windowLocation,
                type: "POST",
                data: { type: 1 },
                success: function (programme) {
                    $("#Selector").empty();
                    for (var i = 0; i < programme.length; i++) {
                        $("#Selector").append("<option value =" + programme[i].Value + ">" + programme[i].Text + "</option>");
                    }
                },
                error: function () {
                    alert("Error getting lecturers!");
                }
            });
        }
        else
        {
            $("#Selector").empty();
            $("#Selector").append("<option value =-1> Please select a timetable type </option>");
        }
    });


    setSelectedWeeks();
})

function setSelectedWeeks() {
    var IdList = $("#RequestIdList").val().split(',');

    for (var c = 0; c < IdList.length; c++) {
        var selectedItems = document.getElementsByName(IdList[c])[0].value.split(',');
        var length = selectedItems.length;
        var output = [];
        var i, j;

        for (i = 0; i < length; i = j + 1) {
            // Beginning of range or single
            output.push(selectedItems[i]);

            // Find end of range
            for (var j = i + 1; j < length && parseInt(selectedItems[j]) == parseInt(selectedItems[j - 1]) + 1; j++);
            j--;

            if (i == j) {
                // single number
                output.push(", ");
            }
            else {
                if (i == j) {
                    // two numbers
                    output.push(", ", selectedItems[j], ", ");
                }
                else {
                    // range
                    output.push(" - ", selectedItems[j], ", ");
                }
            }
        }

        output.pop(); // remove trailing comma
        $(".WeekDisp-" + IdList[c]).append(output.join(""));
    }

}