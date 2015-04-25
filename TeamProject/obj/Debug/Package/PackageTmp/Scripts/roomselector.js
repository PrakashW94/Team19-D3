var selectedRooms = [];
$(document).ready(function ()
{
    $("#Park").change(function ()
    {
        if ($("#Park").val() != "Any")
        {
            $.ajax(
            {
                url: "/zRequests/Building",
                type: "POST",
                data: { park: $("#Park").val() },
                success: function (buildings)
                {
                    $("#Building").empty();
                    $("#Building").append("<option> Any </option>");
                    for (var i = 0; i < buildings.length; i++)
                    {
                        $("#Building").append("<option>" + buildings[i] + "</option>");
                    }
                    $("#Room").empty();
                    $("#Room").append("<option> Please select a building </option>");
                },
                error: function ()
                {
                    alert("Error getting buildings!");
                }
            });
        }
        else
        {
            $.ajax(
            {
                url: "/zRequests/Building",
                type: "POST",
                data: { park: 'Any' },
                success: function (buildings) {
                    $("#Building").empty();
                    for (var i = 0; i < buildings.length; i++) {
                        $("#Building").append("<option>" + buildings[i] + "</option>");
                    }
                },
                error: function () {
                    alert("Error getting buildings!");
                }
            });

            $.ajax(
            {
                url: "/zRequests/Room",
                type: "POST",
                data: { building: 'Any' },
                success: function (rooms) {
                    $("#Room").empty();
                    for (var i = 0; i < rooms.length; i++) {
                        $("#Room").append("<option>" + rooms[i] + "</option>");
                    }
                },
                error: function () {
                    alert("Error getting rooms!");
                }
            });
        }
    });

    $("#Building").change(function () {
        if ($("#Building").val() != "Any")
        {
            buildingCode = $("#Building").val().split(" ")[0];
            $.ajax(
            {
                url: "/zRequests/Room",
                type: "POST",
                data: { building: buildingCode },
                success: function (rooms)
                {
                    $("#Room").empty();
                    $("#Room").append("<option> Any </option>");
                    for (var i = 0; i < rooms.length; i++)
                    {
                        $("#Room").append("<option>" + rooms[i] + "</option>");
                    }
                },
                error: function ()
                {
                    alert("Error getting rooms!");
                }
            });
        }
        else
        {
            if ($("#Park").val() != "Any")
            {        
                $("#Room").empty();
                $("#Room").append("<option> Please select a building </option>");
            }
            else
            {
                $.ajax(
                {
                    url: "/zRequests/Room",
                    type: "POST",
                    data: { building: 'Any' },
                    success: function (rooms) {
                        $("#Room").empty();
                        for (var i = 0; i < rooms.length; i++) {
                            $("#Room").append("<option>" + rooms[i] + "</option>");
                        }
                    },
                    error: function () {
                        alert("Error getting rooms!");
                    }
                });
            }
        }
    });

    $("#RoomDisp").empty();

    $("#AddRoom").click(function ()
    {
        park = $("#Park").val();
        building = $("#Building").val();
        room = $("#Room").val();
        roomDisp = $("#RoomDisp")
        if (park == "Any")
        {
            if (building == "Any")
            {
                if (room == "Any")
                {
                    selectedRooms.push("0");
                    roomDisp.append("<option> Any Room </option>");
                    //alert("any room");
                }
                else
                {
                    var dup = false;
                    for (var i = 0; i < selectedRooms.length; i++)
                    {
                        if (selectedRooms[i].substring(1) == room)
                        {
                            dup = true;
                        }
                    }
                    if (dup)
                    {
                        alert("Cannot add the same room twice!");
                    }
                    else
                    {
                        selectedRooms.push("3" + room);
                        roomDisp.append("<option> " + room + " </option>");
                        //alert("specific room");
                    } 
                }
            }
            else
            {
                if (room == "Any")
                {
                    var buildingCode = building.split(" ")[0];
                    selectedRooms.push("2" + buildingCode);
                    roomDisp.append("<option> " + "Any room in " + building + " </option>");
                    //alert("Any room in that building");
                }
                else
                {
                    var dup = false;
                    for (var i = 0; i < selectedRooms.length; i++) {
                        if (selectedRooms[i].substring(1) == room) {
                            dup = true;
                        }
                    }
                    if (dup) {
                        alert("Cannot add the same room twice!");
                    }
                    else {
                        selectedRooms.push("3" + room);
                        roomDisp.append("<option> " + room + " </option>");
                        //alert("specific room");
                    }
                }
            }
        }
        else
        {
            if (building == "Any")
            {
                selectedRooms.push("1" + park)
                roomDisp.append("<option> " + "Any room in the " + park + " Park" + " </option>");
                //alert("any room in that park");
            }
            else
            {
                if (room == "Any")
                {
                    buildingCode = building.split(" ")[0];
                    selectedRooms.push("2" + buildingCode);
                    roomDisp.append("<option> " + "Any room in " + building + " </option>");
                    //alert("any room in that building");
                }
                else
                {
                    var dup = false;
                    for (var i = 0; i < selectedRooms.length; i++) {
                        if (selectedRooms[i].substring(1) == room) {
                            dup = true;
                        }
                    }
                    if (dup) {
                        alert("Cannot add the same room twice!");
                    }
                    else {
                        selectedRooms.push("3" + room);
                        roomDisp.append("<option> " + room + " </option>");
                        //alert("specific room");
                    }
                }
            }
        }
        $("#Rooms").val(selectedRooms.toString());
    })
});

/*
Notes...
The contents of selectedRooms: 
if the string is 0, this indicates "Any Room" ie no preference on room
if the string begins with 1, this indicates "Any room in Park" 
if the string begins with 2, this indicates "Any room in Building"
if the string begins with 3, this indicates "Specific room"
This is used to make the processing server-side a bit easier. 
Prakash
*/