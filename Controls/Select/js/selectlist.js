/*----------------------------------------------------:Init--------------------------------------------*/
function load_autocomplete(item) {
    var control = $(item).autocomplete({
        source: function (request, response) {
            $(item).parent().find(".clear-link").addClass("autocolmplete-loading");
            $.ajax({
                dataType: "json",
                type: 'Get',
                url: gRootUrl + "AutoComplete/" + $(item).attr("data-namespace") + "/?term=" + request.term + "&cond=" + $(item).attr("data-cond"),
                success: function (data) {
                    $(item).parent().find(".clear-link").removeClass("autocolmplete-loading");
                    $(item).removeClass('ui-autocomplete-loading');
                    // hide loading image
                    //response(data);
                    response($.map(data, function (item) {
                        item.label = latinizeDecode(unescape(JSON.parse('"' + item.label + '"')));
                        return item;
                        // your operation on data
                    }));
                },
                error: function (data) {
                    $(item).parent().find(".clear-link").removeClass("autocolmplete-loading");
                }
            });
        },
        _renderItem: function (ul, item) {
            return $("<li>")
                .append(unescape(JSON.parse('"' + item.label + '"')))
                .appendTo(ul);
        },
        formatResult: function (row) {
            alert(row);
            return $('<div/>').html(row).html();
        },
        minLength: $(item).attr("data-AutocompleteMinLen"),
        select: function (event, ui) {
            var name = $("<li></li>").html(ui.item.label).text();
            $(this).val(name);
            $(this).parent().find("input[type=hidden]").val(ui.item.value);
            if ($(this).attr("onchange") != null && $(this).attr("onchange") != "") {
                eval($(this).attr("onchange"));
            }
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
        },
        change: function (event, ui) {
            if (ui == null || ui.item == null) {
                $(this).parent().find("input[type=hidden]").val("0");
            }
        }
    }).focus(function () {
        $(this).autocomplete("search");
    });
    control.data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li></li>").data("item.autocomplete", item)
            .append($('<div/>').html(item.label).text())
            .appendTo(ul);
    };
}

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function selectlist_on_after_update_function(pControl, gpostArray) {
    if (pControl.find(".autocomplete-input").size() != 0) {
        gpostArray[pControl.find(".autocomplete-input").attr("name")] = pControl.find("[name=" + pControl.find(".autocomplete-input").attr("name") + "_id]").val();
        gpostArray[pControl.find(".autocomplete-input").attr("name") + "_id"] = pControl.find("[name=" + pControl.find(".autocomplete-input").attr("name") + "_id]").val();

        if (pControl.prev().hasClass("control-view")) {
            pControl.prev().html(pControl.find(".autocomplete-input").val());
        }

        return { "name": pControl.find(".autocomplete-input").attr("name") + "_id", "value": pControl.find("[name=" + pControl.find(".autocomplete-input").attr("name") + "_id]").val() };
    }
    else {
        gpostArray[pControl.find("input").attr("name")] = pControl.find("input").val();

        return { "name": pControl.find("input").attr("name"), "value": pControl.find("input").val() };
    }

    return null;
}


function selectlist_on_clear(pControl) {
    pControl.find(".autocomplete-input").val("");
    pControl.find("name=[" + pControl.find(".autocomplete-input").attr("name") + "_id]").val("");
}
