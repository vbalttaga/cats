var gCkEditorConfig = {
    toolbar:
            [
                ["Bold", "Italic", "Underline", "P", "BulletedList", "Indent", "Outdent", "Maximize","Source"]
            ],
    removePlugins: 'elementspath',
    resize_enabled: false,
    allowedContent: true,
    ignoreEmptyParagraph: true,
    enterMode: CKEDITOR.ENTER_BR,
    autoParagraph: false,
    width: '100%',
    height: 400
};
/*----------------------------------------------------:Error Handling--------------------------------------------*/

/*----------------------------------------------------:validation-----------------------------------------------*/

/*----------------------------------------------------:edit-----------------------------------------------------*/
function html_on_after_update_function(pControl, gpostArray) {
    gpostArray[pControl.find("textarea").attr("name")] = pControl.find("textarea").val();
    pControl.prev().html(pControl.find("textarea").val());
    return encodeURIComponent(pControl.find("textarea").val());
}