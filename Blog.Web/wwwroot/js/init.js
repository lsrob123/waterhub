let postEditor;

window.onload = () => {
    if (!!document.getElementById('edit-post-search-keywords')) {
        postEdit.loadDataAsync();
    }

    const postEditorElement = document.getElementById('post-eidtor-content')
    if (!!postEditorElement) {
        postEdit.init(new Jodit(postEditorElement));
    }
}