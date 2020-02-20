class HomeScreen {
    private readonly service: Service;

    constructor(service: Service) {
        this.service = service;
    }


    private get searchBox(): HTMLInputElement {
        return <HTMLInputElement>document.getElementById('home-search-box');
    }

    private startSearch = async () => {
        const keywords = this.searchBox.value;

    }

    private searchBoxDebounceId: number = null;
    private isSearchBoxFocused: boolean = false;
    public onSearchBoxUpdated = () => {
        if (!!this.searchBoxDebounceId) {
            clearTimeout(this.searchBoxDebounceId);
            this.searchBoxDebounceId = null;
        }

        if (!this.isSearchBoxFocused) {
            return;
        }

        this.searchBoxDebounceId = window.setTimeout(function () {
            if (!!this.isSearchBoxFocused) {
                this.startSearch();
            }
        }, 500);
    }
    public onSearchBoxFocused = () => {
        this.isSearchBoxFocused = true;
    }
    public onSearchBoxBlurred = () => {
        this.isSearchBoxFocused = false;
        this.startSearch();
    }

}
