var selectedRooms = [];
var selectedRoomsCap = [];
$(document).ready(function ()
{
    $("#Park").change(function ()
    {
        if ($("#Park").val() != "Any")
        {
            $.ajax(
            {
                url: "../zRequests/Building",
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
                url: "../zRequests/Building",
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
                url: "../zRequests/Room",
                type: "POST",
                data: { buildingCode: 'Any', facilities: selectedFacs.toString() },
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

    $("#Building").change(function ()
    {
        if ($("#Building").val() != "Any")
        {
            buildingCode = $("#Building").val().split(" ")[0];
            $.ajax(
            {
                url: "../zRequests/Room",
                type: "POST",
                data: { buildingCode: buildingCode, facilities: selectedFacs.toString() },
                success: function (rooms)
                {
                    $("#Room").empty();
                    if (rooms.length == 0) {
                        $("#Room").append("<option> None available </option>");
                    }
                    else
                    {
                        $("#Room").append("<option> Any </option>");
                    }
                    
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
                    url: "../zRequests/Room",
                    type: "POST",
                    data: { building: 'Any', facilities: selectedFacs.toString() },
                    success: function (rooms)
                    {
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

    $("#Room").change(function ()
    {
        if ($("#Room").val() != "Any")
        {
            $.ajax(
            {
                url: "../zRequests/getCapacity",
                type: "POST",
                data: { room: $("#Room").val() },
                success: function (capacity)
                {
                    $("#capacity").val(parseInt(capacity));
                    $("#capOutput").val(parseInt(capacity));
                },
                error: function ()
                {
                    alert("Error setting capacity value!");
                }
            });
        }
    });

    if ($("#Rooms").val() != "")
    {
        selectedRooms = $("#Rooms").val().split(",");
        selectedRoomsCap = $("#Capacities").val().split(",");
    }

    $("#AddRoom").click(function ()
    {
        if ($("#RoomDisp option").length < 3)
        {
            if ($("#Room").val() != "None available") {
                park = $("#Park").val();
                building = $("#Building").val();
                room = $("#Room").val();
                capacity = $("#capOutput").val();
                roomDisp = $("#RoomDisp");
                if (park == "Any") {
                    if (building == "Any") {
                        if (room == "Any") {
                            selectedRooms.push("0");
                            selectedRoomsCap.push(capacity);
                            roomDisp.append("<option>" + capacity + " in any Room </option>");
                            //alert("any room");
                        }
                        else {
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
                                $.ajax(
                                {
                                    url: "../zRequests/RoomCapacityCheck",
                                    type: "POST",
                                    data: { room: room, capacity: capacity },
                                    success: function (result) {
                                        if (result.correct) {
                                            selectedRooms.push("3" + room);
                                            selectedRoomsCap.push(capacity);
                                            roomDisp.append("<option> " + capacity + " in " + room + "</option>");
                                            updateOutputValues();
                                        }
                                        else {
                                            alert("Selected room is not big enough!");
                                        }
                                    },
                                    error: function () {
                                        alert("Error checking capacity!");
                                    }
                                });
                            }
                        }
                    }
                    else {
                        if (room == "Any") {
                            var buildingCode = building.split(" ")[0];
                            selectedRooms.push("2" + buildingCode);
                            selectedRoomsCap.push(capacity);
                            roomDisp.append("<option> " + capacity + " in any room in " + building + " </option>");
                            //alert("Any room in that building");
                        }
                        else {
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
                                $.ajax(
                                {
                                    url: "../zRequests/RoomCapacityCheck",
                                    type: "POST",
                                    data: { room: room, capacity: capacity },
                                    success: function (result) {
                                        if (result.correct) {
                                            selectedRooms.push("3" + room);
                                            selectedRoomsCap.push(capacity);
                                            roomDisp.append("<option> " + capacity + " in " + room + "</option>");
                                            updateOutputValues();
                                        }
                                        else {
                                            alert("Selected room is not big enough!");
                                        }
                                    },
                                    error: function () {
                                        alert("Error checking capacity!");
                                    }
                                });
                            }
                        }
                    }
                }
                else {
                    if (building == "Any") {
                        selectedRooms.push("1" + park)
                        selectedRoomsCap.push(capacity);
                        roomDisp.append("<option> " + capacity + " in any room in the " + park + " Park" + " </option>");
                        //alert("any room in that park");
                    }
                    else {
                        if (room == "Any") {
                            buildingCode = building.split(" ")[0];
                            selectedRooms.push("2" + buildingCode);
                            selectedRoomsCap.push(capacity);
                            roomDisp.append("<option> " + capacity + " in any room in " + building + " </option>");
                            //alert("any room in that building");
                        }
                        else {
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
                                $.ajax(
                                {
                                    url: "../zRequests/RoomCapacityCheck",
                                    type: "POST",
                                    data: { room: room, capacity: capacity },
                                    success: function (result) {
                                        if (result.correct) {
                                            selectedRooms.push("3" + room);
                                            selectedRoomsCap.push(capacity);
                                            roomDisp.append("<option> " + capacity + " in " + room + "</option>");
                                            updateOutputValues();
                                        }
                                        else
                                        {
                                            alert("Selected room is not big enough!");
                                        }
                                    },
                                    error: function ()
                                    {
                                        alert("Error checking capacity!");
                                    }
                                });
                            }
                        }
                    }
                }
                updateOutputValues();
            }
            else
            {
                alert("Please select a valid room!")
            }
        }
        else
        {
            alert("You cannot add more than three rooms in a single request!")
        }
    })

    function updateOutputValues()
    {
        $("#Rooms").val(selectedRooms.toString());
        $("#Capacities").val(selectedRoomsCap.toString());
    }

    $("#RemoveRoom").click(function ()
    {
        var room = document.getElementById("RoomDisp").selectedIndex;
        $("#RoomDisp option:eq(" + room +")").remove()
        selectedRooms.splice(room, 1);
        selectedRoomsCap.splice(room, 1);
        updateOutputValues();
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

var selectedFacs = [];

function filterByFacs()
{
    selectedFacs = [];
    $("#FacilitiesChkBox input:checked").each(function ()
    {
        selectedFacs.push($(this).attr('value').split(".").join(" "));
    })

    buildingCode = $("#Building").val().split(" ")[0];

    $.ajax(
    {
        url: "../zRequests/FilterByFacilities",
        type: "POST",
        data: { facilities: selectedFacs.toString(), building: buildingCode },
        success: function (rooms)
        {
            if ($("#Park").val() != "Any" && $("#Building").val() == "Any")
            {
                $("#Room").empty();
                $("#Room").append("<option> Please select a building </option>");
            }
            else
            {
                $("#Room").empty();
                if (rooms.length == 0)
                {
                    $("#Room").append("<option>None available</option>");
                }
                else
                {
                    $("#Room").append("<option>Any</option>");
                }
                for (var i = 0; i < rooms.length; i++)
                {
                    $("#Room").append("<option>" + rooms[i] + "</option>");
                }
            }
        },
        error: function () {
            alert("Error getting rooms!");
        }
    });

}