function ObjectDevelopmentDocument_dynamic_section_array(pBlock) {
    gpostArray["ObjectDevelopmentId"] = $("[name=ObjectDevelopmentId]").val();
}

function PrintDocument(Id, type, Namespace) {
    var postArray = "ObjectFolderId=" + Id + "&type=" + type + "&Namespace=" + Namespace;
    return print(postArray);
}