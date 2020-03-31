class HomeScreen {
    private readonly service: Service;

    constructor(service: Service) {
        this.service = service;
    }

    // Search
    public get searchBox(): HTMLInputElement {
        return <HTMLInputElement>document.getElementById('home-search-box');
    }

    public get searchDropDown(): HTMLElement {
        return <HTMLElement>document.getElementById('home-search-dropdown');
    }

    public get searchDropDownContent(): HTMLElement {
        return <HTMLElement>document.getElementById('home-search-dropdown-content');
    }

    public async startSearch() {
        if (!this.isSearchBoxFocused) {
            this.stopSearchDebounceTimer();
            return;
        }

        const keywords = this.searchBox.value;
        if (!keywords || keywords.trim() === '') {
            this.hideSearchdropDown();
            return;
        }

        this.showSearchDropDown();
        const entries = await this.service.listPostInfoEntriesByKeywords(keywords.trim());
        this.searchDropDownContent.innerHTML = '';
        if (!entries || entries.length === 0) {
            this.searchDropDownContent.innerHTML = '<div>No Results</div>'; //TODO: Add text map
            return;
        }

        this.searchDropDownContent.innerHTML = entries.reduce((prior, current) => {
            const titleWithJsOpen = `<a class="title" href="javascript:homeScreen.displayFullContent(\'${current.title}\',\'${current.urlFriendlyTitle}\')" title="${current.textClickToReadFullArticle}">${current.title}</a>`;
            const linkToNewWindow = `<a class="new-window-link" href="/posts/${current.urlFriendlyTitle}" title="${current.textOpenArticleInNewWindow}" target="${current.urlFriendlyTitle}">${current.textOpenArticleInNewWindow}</a>`;
            return `${prior}<div class="post">${titleWithJsOpen} ${linkToNewWindow}</div>`;
        }, '');
    }

    private searchBoxDebounceId: number = null;
    private isSearchBoxFocused: boolean = false;
    public onSearchBoxUpdated() {
        this.stopSearchDebounceTimer();

        this.searchBoxDebounceId = window.setTimeout(function () {
            this.startSearch();
        }.bind(this), 500);
    }

    private stopSearchDebounceTimer() {
        if (!!this.searchBoxDebounceId) {
            clearTimeout(this.searchBoxDebounceId);
            this.searchBoxDebounceId = null;
        }
    }

    public onSearchBoxFocused() {
        this.isSearchBoxFocused = true;
    }
    public onSearchBoxBlurred() {
        this.isSearchBoxFocused = false;
    }
    public clearSearch() {
        this.searchBox.value = null;
        this.searchDropDown.innerHTML = '';
        this.hideSearchdropDown();
    }
    public hideSearchdropDown() {
        this.searchDropDown.style.display = "none";
        this.columnContainer.style.overflow = 'auto';
        this.searchBoxDebounceId = null;
    }
    public showSearchDropDown() {
        const searchDropDown = this.searchDropDown;
        const viewportOffset = this.searchBox.getBoundingClientRect();
        const top = viewportOffset.bottom + 3;
        searchDropDown.style.top = `${top}px`;
        const left = viewportOffset.left + 7;
        searchDropDown.style.left = `${left}px`;
        searchDropDown.style.width = `${viewportOffset.width}px`;

        const searchDropDownContent = this.searchDropDownContent;
        searchDropDownContent.style.top = `${top + 65}px`;
        searchDropDownContent.style.left = `${left}px`;
        searchDropDownContent.style.width = `${viewportOffset.width - 50}px`;

        this.searchDropDown.style.display = "block";
        this.columnContainer.style.overflow = 'hidden';
    }

    public searchByTag(tag: string) {
        if (!tag) return;

        this.searchBox.value = tag;
        this.startSearch();
    }

    // Show post
    private get columnContainer(): HTMLElement {
        return document.getElementById('column-container');
    }
    private get modalFullArticle(): HTMLElement {
        return document.getElementById('modal-full-article');
    }
    private get modalPostTitle(): HTMLElement {
        return document.getElementById('modal-post-title');
    }
    private get modalPostContent(): HTMLElement {
        return document.getElementById('modal-post-content');
    }
    public displayFullContent(title: string, urlFriendlyTitle: string) {
        this.modalFullArticle.style.display = 'flex';
        this.columnContainer.style.overflow = 'hidden';
        this.modalPostTitle.innerHTML = title;
        this.modalPostContent.innerHTML = document.getElementById(`div-${urlFriendlyTitle}`).innerHTML;
    }
    public collapseFullContent() {
        this.modalFullArticle.style.display = 'none';
        this.columnContainer.style.overflow = 'auto';
        this.modalPostTitle.innerHTML = null;
        this.modalPostContent.innerHTML = null;
    }

}
