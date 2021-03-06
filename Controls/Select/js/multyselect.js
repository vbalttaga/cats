/*----------------------------------------------------:Init--------------------------------------------*/
function load_multyselect(item) {
    if ($.trim($(item).html()) == "") {

        gpostArray = {};
        gpostArray["values"] = $(item).attr("data-values");

        $.ajax({
            type: 'POST',
            url: gRootUrl + "MultySelect/" + $(item).attr("data-namespace"),
            async: true,
            data: gpostArray,
            success: function (responce) {
                $(item).html(responce);
                $(item).select2();
            }
        });
    }
    else
        $(item).select2();
}
/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function multyselect_on_after_update_function(pControl, gpostArray) {
    if (pControl.find("select").size() != 0) {
        gpostArray[pControl.find("select").attr("name")] = pControl.find("select").val();

        if (pControl.prev().hasClass("control-view")) {
            pControl.prev().html(pControl.find("select  option:selected").html());
        }

        return { "name": pControl.find("select").attr("name"), "value": pControl.find("select").val() };
    }

    return null;
}


function multyselect_on_clear(pControl) {
    pControl.find("select").val(0);
}
