
/*----------------------------------------------------:upload-----------------------------------------------*/
function initUploadFile(purl, pUniqueId) {
    $("#upload_File_container_" + pUniqueId).find('a').ajaxUpload({
        url: purl,
        name: "file",
        dataType: "JSON",
        onSubmit: function () {
            $("#upload_File_container_" + pUniqueId).find('.upload-File-File-loading').show();
            return true;
        },
        onComplete: function (responce) {
            $("#upload_File_container_" + pUniqueId).find('.upload-File-File-loading').hide();

            jsonResponce = JSON.parse(responce);
            if (jsonResponce.Result == 0) {
                alert(jsonResponce.Message);
                return true;
            }
            $("#upload_File_container_" + pUniqueId).parent().attr("data-id", jsonResponce.Data.Id);
            $("#upload_File_container_" + pUniqueId).find('.upload-File-File').attr("href", jsonResponce.Data.file);
            $("#upload_File_container_" + pUniqueId).find('.upload-File-File').html(jsonResponce.Data.name);
            return true;
        }
    });
}

/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function File_on_after_update_function(pControl, gpostArray) {
    gpostArray[pControl.attr("data-name")] = pControl.attr("data-id");
    pControl.prev().find('.upload-File-File').attr("href", pControl.find('.upload-File-File').attr("href"));
    pControl.prev().find('.upload-File-File').html(pControl.find('.upload-File-File').html());
    return pControl.attr("data-id");
}
