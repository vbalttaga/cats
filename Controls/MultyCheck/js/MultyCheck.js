/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function MultyCheck_on_after_update_function(pControl, gpostArray) {
    var returnValue = "";
    pControl.find("input:checked").each(function () {
        returnValue += $(this).val() + ";";
    })
    gpostArray[pControl.find(".multycheck").attr("name")] = returnValue;

    var display = "";
    pControl.find("input:checked").each(function () {
        display += $(this).parent().next().html() + "<br/>";
    })

    if (pControl.prev().hasClass("control-view")) {
        pControl.prev().html(display);
    }

    return { "name": pControl.find(".multycheck").attr("name"), "value": returnValue };
}
