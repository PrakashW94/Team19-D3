var selectedModules = [];
$(document).ready(function ()
{
    $("#AddModule").click(function ()
    {
        var mod = $("#ModList").val();
        var modCode = mod.split(' - ')[0];
        selectedModules.push(modCode);
        $("#ModDisp").append("<option> " + mod + " </option>");
        $("#Modules").val(selectedModules.toString());
    })

    if ($("#Modules").val() != "") {
        selectedModules = $("#Modules").val().split(",");
    }

    $("#RemoveModule").click(function () {
        var module = document.getElementById("ModDisp").selectedIndex;
        $("#ModDisp option:eq(" + module + ")").remove()
        selectedModules.splice(module, 1);
        $("#Modules").val(selectedModules.toString());
    })
});