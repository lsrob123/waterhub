let postEditor;

window.onload = () => {
    const postEditorElementId = 'post-eidtor-content';
    const postEditorElement = document.getElementById(postEditorElementId)

    if (!!postEditorElement)
        //postEditor = new Jodit(`#${postEditorElementId}`);
        postEditor = new Jodit(postEditorElement);
}