let adminScreenor;

window.onload = () => {
    if (!!document.getElementById('edit-post-search-keywords')) {
        adminScreen.loadDataAsync();
    }

    const adminScreenEditorElement = document.getElementById('post-eidtor-content')
    if (!!adminScreenEditorElement) {
        adminScreen.init(new Jodit(adminScreenEditorElement));
    }

    businessesScreen.init();
    contactScreen.init();
}