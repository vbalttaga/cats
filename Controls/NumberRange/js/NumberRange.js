/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function numberrange_on_after_update_function(pControl, gpostArray) {
    gpostArray[$(pControl.find("input")[0]).attr("name")] = $(pControl.find("input")[0]).val();
    gpostArray[$(pControl.find("input")[1]).attr("name")] = $(pControl.find("input")[1]).val();
    pControl.prev().html(pControl.find("NumberRange").val());
    return $(pControl.find("input")[0]).val();
}