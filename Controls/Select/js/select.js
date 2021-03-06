/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function select_on_after_update_function(pControl, gpostArray) {
    if (pControl.find("select").size() != 0) {
        gpostArray[pControl.find("select").attr("name")] = pControl.find("select").val();

        if (pControl.prev().hasClass("control-view")) {
            pControl.prev().html(pControl.find("select  option:selected").html());
        }

        return { "name": pControl.find("select").attr("name"), "value": pControl.find("select").val() };
    }
    else {
        gpostArray[pControl.find("input").attr("name")] = pControl.find("input").val();

        return { "name": pControl.find("input").attr("name"), "value": pControl.find("input").val() };
    }

    return null;
}


function select_on_clear(pControl) {
    pControl.find("select").val(0);
}
