let postEditor;

window.onload = () => {
    const postEditorElement = document.getElementById('post-eidtor-content')

    if (!!postEditorElement) {
        postEdit.init(new Jodit(postEditorElement));
    }
}