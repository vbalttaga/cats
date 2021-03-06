function SearchReport() {
    $("#page_num").val(0);
    $("#sSearch").val("");
    return doSearchSubbmit("");
}

function SortReport(pCol, propName) {
    $("#page_num").val(0);
    $("#sort_col").val(propName);
    $("#sort_dir").val($(pCol).hasClass("data-grid-title-control-sortable-asc") ? "desc" : "asc");
    return doSearchSubbmit($("#sSearch").val());
}

function doSearchSubbmit(sSearch) {

    if (!$(".loading").is(":visible")) {

        $('.ajax-loading-overlay').fadeIn("slow");

        var id = 0;

        gpostArray = {};
        gpostArray["bo_type"] = $(".search-data-grid").attr("data-type");
        gpostArray["__RequestVerificationToken"] = $("[name=__RequestVerificationToken]").val();

        gpostArray["CountPerPage"] = $("#dpdwn_count_per_page").val();
        gpostArray["PageNum"] = $("#page_num").val();
        gpostArray["SortCol"] = $("#sort_col").val();
        gpostArray["SortDir"] = $("#sort_dir").val();
        gpostArray["sSearch"] = sSearch;

        $(".search-data-grid").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $.ajax({
            type: 'POST',
            url: gRootUrl + "Report/Search/",
            async: true,
            dataType: "JSON",
            data: gpostArray,
            success: function (responce) {
                if (responce["Result"] == 1) {
                    $('.ajax-loading-overlay').fadeOut();
                    $(".search-results").fadeOut("slow", function () {
                        $(".search-results").html(responce["Data"]["Search_Result"]);
                        window.setTimeout(function () { $(".search-results").fadeIn("slow") }, 1);
                    })
                }
                if (responce["Result"] == 2) {
                    window.location.reload();
                    return;
                }
            }
        });
    }
    return false;
}

var gTmpVar = 0;
function doPrintReport() {
    if (!$(".ajax-loading-overlay").is(":visible")) {

        $('.ajax-loading-overlay').fadeIn("slow");

        var id = 0;

        gpostArray = {};
        gpostArray["bo_type"] = $(".search-data-grid").attr("data-type");
        gpostArray["__RequestVerificationToken"] = $("[name=__RequestVerificationToken]").val();

        gpostArray["CountPerPage"] = $("#dpdwn_count_per_page").val();
        gpostArray["PageNum"] = $("#page_num").val();
        gpostArray["SortCol"] = $("#sort_col").val();
        gpostArray["SortDir"] = $("#sort_dir").val();
        gpostArray["sSearch"] = $("#sSearch").val();

        $(".search-data-grid").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $.ajax({
            type: 'POST',
            url: gRootUrl + "Report/Print/",
            async: true,
            dataType: "JSON",
            data: gpostArray,
            success: function (responce) {
                if (responce["Result"] == 1) {
                    $('.ajax-loading-overlay').fadeOut();

                    $("#printArea").html(responce["Data"]["Print_Result"]);

                    var timer = setInterval(function () {
                        if ($("#printArea").find(".print-end").size() > 0) {
                            $('.ajax-loading-overlay').fadeOut();
                            $("#printArea").printArea();
                            clearInterval(timer);
                        }
                    }, 200);
                }
                if (responce["Result"] == 2) {
                    window.location.reload();
                    return;
                }
            }
        });
    }

    return false;
}

function doExportExcellReport() {
    if (!$(".ajax-loading-overlay").is(":visible")) {

        $('.ajax-loading-overlay').fadeIn("slow");

        var id = 0;

        gpostArray = {};
        gpostArray["bo_type"] = $(".search-data-grid").attr("data-type");
        gpostArray["__RequestVerificationToken"] = $("[name=__RequestVerificationToken]").val();

        gpostArray["CountPerPage"] = $("#dpdwn_count_per_page").val();
        gpostArray["PageNum"] = $("#page_num").val();
        gpostArray["SortCol"] = $("#sort_col").val();
        gpostArray["SortDir"] = $("#sort_dir").val();
        gpostArray["sSearch"] = $("#sSearch").val();

        $(".search-data-grid").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $.fileDownload(gRootUrl + "Report/ExportExcell/", {
            httpMethod: "POST",
            data: gpostArray,
            successCallback: function (url) {
                $('.ajax-loading-overlay').fadeOut();
            },
            failCallback: function (html, url) {

                $('.ajax-loading-overlay').fadeOut();

                ShowMessage(html, "error", true);

            }
        });
    }

    return false;
}

function doExportCsvReport() {
    if (!$(".ajax-loading-overlay").is(":visible")) {

        $('.ajax-loading-overlay').fadeIn("slow");

        var id = 0;

        gpostArray = {};
        gpostArray["bo_type"] = $(".search-data-grid").attr("data-type");
        gpostArray["__RequestVerificationToken"] = $("[name=__RequestVerificationToken]").val();

        gpostArray["CountPerPage"] = $("#dpdwn_count_per_page").val();
        gpostArray["PageNum"] = $("#page_num").val();
        gpostArray["SortCol"] = $("#sort_col").val();
        gpostArray["SortDir"] = $("#sort_dir").val();
        gpostArray["sSearch"] = $("#sSearch").val();

        $(".search-data-grid").find(".control-edit").each(function (i) {
            var control = $(this).attr("data-control");
            eval("if (window." + control + "_on_after_update_function) " + control + "_on_after_update_function($(this),gpostArray);");
        });

        $.fileDownload(gRootUrl + "Report/ExportCSV/", {
            httpMethod: "POST",
            data: gpostArray,
            successCallback: function (url) {
                $('.ajax-loading-overlay').fadeOut();
            },
            failCallback: function (html, url) {

                $('.ajax-loading-overlay').fadeOut();

                ShowMessage(html, "error", true);

            }
        });
    }

    return false;
}
function show_report_page(page) {
    $("#page_num").val(page);
    doSearchSubbmit($("#sSearch").val());
    return false;
}
function count_per_page() {
    $("#page_num").val(0);
    doSearchSubbmit($("#sSearch").val());
}

function ShowQueryOptions(Model) {
    $.ajax({
        type: 'POST',
        url: gRootUrl + "Report/Options/" + Model,
        async: true,
        success: function (data) {
            ShowConfirmMessage(data, function () {
                $.ajax({
                    type: 'POST',
                    url: gRootUrl + "Report/OptionsSave/" + Model,
                    async: true,
                    data: $("#printOptions").serialize(),
                    success: function (data) {
                        if (data["Result"] == 0) {
                            ShowAlertMessage(data["Message"], function () {
                                return true;
                            });
                        }
                    }
                });
                return true;
            }, function () {
                return true;
            }, "OK", "Inchide");
        }
    });
    return false;
}