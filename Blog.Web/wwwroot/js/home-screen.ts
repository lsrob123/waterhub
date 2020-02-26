class HomeScreen {
    private readonly service: Service;

    constructor(service: Service) {
        this.service = service;
    }

    private get searchBox(): HTMLInputElement {
        return <HTMLInputElement>document.getElementById('home-search-box');
    }

    private get searchDropDown(): HTMLElement {
        return <HTMLElement>document.getElementById('home-search-dropdown');
    }

    private get searchDropDownContent(): HTMLElement {
        return <HTMLElement>document.getElementById('home-search-dropdown-content');
    }

    public startSearch = async () => {
        const keywords = this.searchBox.value;
        if (!keywords) return;

        const entries= await this.service.listPostInfoEntriesByKeywords(keywords);
        this.searchDropDown.innerHTML='';
        if (!entries||entries.length===0) return;

        this.searchDropDownContent.innerHTML=entries.reduce((prior, current)=>{
            const linkToJsOpen = `<a href="javascript:homeScreen.displayFullContent(\'${current.title}\',\'${current.urlFriendlyTitle}\')" title="${current.textClickToReadFullArticle}">${current.textReadFullArticle}</a>`;
            const linkToNewWindow =`<a href="~/posts/${current.urlFriendlyTitle}" title="${current.textOpenArticleInNewWindow}" target="${current.urlFriendlyTitle}">${current.textOpenArticleInNewWindow}</a>`;
            return `${prior}<div>${linkToJsOpen} ${linkToNewWindow}</div>`;
        }, '');
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
            this.showSearchDropDown();
            if (!!this.isSearchBoxFocused) {
                this.startSearch();
            }
        }, 500);
    }
    public onSearchBoxFocused = () => {
        this.isSearchBoxFocused = true;
        this.showSearchDropDown();
   }
    public onSearchBoxBlurred =  () => {
        this.isSearchBoxFocused = false;
    }
    public clearSearch = () => {
        this.searchBox.value=null;
        this.searchDropDown.innerHTML='';
        this.hideSearchdropDown();
    }
    private hideSearchdropDown() {
        this.searchDropDown.style.display = "none";
    }
    private showSearchDropDown() {
        const searchDropDown = this.searchDropDown;
        const viewportOffset = this.searchBox.getBoundingClientRect();
        const top = viewportOffset.bottom + 5;
        searchDropDown.style.top = `${top}px`;
        searchDropDown.style.left = `${viewportOffset.left-5}px`;
        searchDropDown.style.width = `${viewportOffset.width}px`;

        const viewportHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
        searchDropDown.style.height = `${viewportHeight-top-20}px`;

        this.searchDropDown.style.display = "block";
    }
}
