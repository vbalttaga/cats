/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function DateRange_on_after_update_function(pControl, gpostArray) {

    gpostArray[$(pControl.find("input")[0]).attr("name")] = $(pControl.find("input")[0]).val();
    gpostArray[$(pControl.find("input")[1]).attr("name")] = $(pControl.find("input")[1]).val();
    if (pControl.find("input").size() > 2) {
        gpostArray[$(pControl.find("input")[2]).attr("name")] = $(pControl.find("input")[2]).val();
        gpostArray[$(pControl.find("input")[3]).attr("name")] = $(pControl.find("input")[3]).val();
        gpostArray[$(pControl.find("input")[4]).attr("name")] = $(pControl.find("input")[4]).val();
        gpostArray[$(pControl.find("input")[5]).attr("name")] = $(pControl.find("input")[5]).val();
    }

    if (pControl.prev().hasClass("control-view")) {
        pControl.prev().html(pControl.find("DateRange").val());
    }
    
    if (pControl.find("input").size() > 2) {
        gaoSearchData.push({ "name": $(pControl.find("input")[1]).attr("name"), "value": $(pControl.find("input")[1]).val() },
                { "name": $(pControl.find("input")[2]).attr("name"), "value": $(pControl.find("input")[2]).val() },
                { "name": $(pControl.find("input")[3]).attr("name"), "value": $(pControl.find("input")[3]).val() },
                { "name": $(pControl.find("input")[4]).attr("name"), "value": $(pControl.find("input")[4]).val() },
                { "name": $(pControl.find("input")[5]).attr("name"), "value": $(pControl.find("input")[5]).val() });
    }
    else {
        
        gaoSearchData.push({ "name": $(pControl.find("input")[1]).attr("name"), "value": $(pControl.find("input")[1]).val() });
    }

    return { "name": $(pControl.find("input")[0]).attr("name"), "value": $(pControl.find("input")[0]).val() };
}


function DateRange_on_clear(pControl) {
    $(pControl.find("input")[0]).val("");
    $(pControl.find("input")[1]).val("");
    if (pControl.find("input").size() > 2) {
        $(pControl.find("input")[2]).val("");
        $(pControl.find("input")[3]).val("");
        $(pControl.find("input")[4]).val("");
        $(pControl.find("input")[5]).val("");
    }
}