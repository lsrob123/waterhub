let adminScreenor;

window.onload = () => {
    if (!!document.getElementById('edit-post-search-keywords')) {
        adminScreen.loadDataAsync();
    }

    const adminScreenorElement = document.getElementById('post-eidtor-content')
    if (!!adminScreenorElement) {
        adminScreen.init(new Jodit(adminScreenorElement));
    }
}