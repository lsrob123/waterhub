class ApiCallResult {
    public data: any;
    public ok: boolean;
    public status: number;
    public statusText: string;
    public message: string;

    public withSuccess(data: any, status: number = 200): ApiCallResult {
        this.ok = true;
        this.status = 200;
        this.data = data;
        return this;
    }

    public withError(message: string, status: number, statusText: string): ApiCallResult {
        this.ok = false;
        this.status = status;
        this.statusText = statusText;
        this.data = undefined;
        this.message = message;
        return this;
    }

    public withLocalError(message: string): ApiCallResult {
        this.ok = false;
        this.data = undefined;
        this.message = message;
        return this;
    }
}

class Tag {
    key: string;
    text: string;
}

class Post {
    key: string;
    content: string
    isPublished: boolean;
    isSticky: boolean;
    tags: Tag[];
    title: string;
    urlFriendlyTitle: string;
}

class Service {
    public upsertPost = async (key: string, title: string, content: string, isSticky: boolean, isPublished: boolean, tags: string[])
        : Promise<ApiCallResult> => {
        try {
            const rawResponse = await fetch('/api/posts', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    key: key,
                    title: title,
                    content: content,
                    isSticky: !!isSticky,
                    isPublished: !!isPublished,
                    tags: tags
                })
            });

            if (!!rawResponse.ok) {
                const data = await rawResponse.json();
                return new ApiCallResult().withSuccess(data);
            }

            const message = await rawResponse.text();
            return new ApiCallResult().withError(message, rawResponse.status, rawResponse.statusText);
        } catch (e) {
            console.error(e);
            return new ApiCallResult().withLocalError(e);
        }
    }

    public getUrl(ralativePath: string): string {
        var rootPath = new RegExp(/^.*\//).exec(window.location.href);
        return encodeURI(`${rootPath}${ralativePath}`);
    }

    public listLatestPosts = async (): Promise<Post[]> => {
        try {
            const rawResponse = await fetch('/api/posts/latest', {
                method: 'GET',
            });

            if (!!rawResponse.ok) {
                const data = await rawResponse.json();
                return data;
            }

            return [];
        } catch (e) {
            console.error(e);
            return null;
        }
    }

    public listPostsWithTitleContainingKeywords = async (keywords: string): Promise<Post[]> => {
        try {
            const rawResponse = await fetch(`/api/posts?keywords=${keywords}`, {
                method: 'GET',
            });

            if (!!rawResponse.ok) {
                const data = await rawResponse.json();
                return data;
            }

            return [];
        } catch (e) {
            console.error(e);
            return null;
        }
    }
}

class PostEdit {
    private readonly service: Service;
    private tags: string[] = [];
    private allTags: string[] = [];
    private editorInstance: any;
    private postList: Post[];
    private isPostListFromLatest: boolean;

    constructor(service: Service) {
        this.service = service;
    }

    public get hasAllTags() {
        return !!this.allTags && this.allTags.length > 0;
    }

    public get allTagsDropdown(): HTMLSelectElement {
        return <HTMLSelectElement>document.getElementById('edit-all-tags');
    }

    public get postListElement(): HTMLElement {
        return document.getElementById('edit-post-list');
    }

    public get keywords(): string {
        const value = (<HTMLInputElement>document.getElementById('edit-post-search-keywords')).value;
        return value;
    }

    public init = (editorInstance: any) => {
        this.editorInstance = editorInstance;
        this.editorInstance.value = (<HTMLInputElement>document.getElementById('PostInEdit_Content')).value;

        this.tags = JSON.parse((<HTMLInputElement>document.getElementById('PostInEdit_TagsInText')).value);
        if (!this.tags) this.tags = [];
        this.renderTags();

        this.allTags = JSON.parse((<HTMLInputElement>document.getElementById('AllTagsInText')).value);
        if (!this.allTags) this.allTags = [];
        this.renderAllTags();
    }

    public loadDataAsync = async () => {
        await this.loadLatestPosts();
    }

    public deleteTag = (index: number) => {
        this.tags.splice(index, 1);
        this.renderTags();
    };

    public addNewTags = () => {
        const value = (<HTMLInputElement>document.getElementById('edit-new-tag')).value;
        if (!value || value.trim() === '') return;

        const tags = value.split(/[ ,;:.]+/g);
        if (tags.length === 0) return;
        tags.map(tag => {
            const existing = this.tags.findIndex(x => x === tag);
            if (existing < 0)
                this.tags.push(tag);
            return true;
        });
        this.renderTags();
    };

    public selectTag = () => {
        const dropdown = this.allTagsDropdown;
        if (dropdown.selectedIndex === 0) return;

        const tag = (dropdown.options[dropdown.options.selectedIndex].value);
        const existing = this.tags.findIndex(x => x === tag);
        if (existing >= 0)
            return;
        this.tags.push(tag);
        this.renderTags();
    }

    public upsertPost = async () => {
        const response = await this.service.upsertPost(
            (<HTMLInputElement>document.getElementById('PostInEdit_Key')).value,
            (<HTMLInputElement>document.getElementById('PostInEdit_Title')).value,
            this.editorInstance.value,
            !!(<HTMLInputElement>document.getElementById('PostInEdit_IsSticky')).checked,
            !!(<HTMLInputElement>document.getElementById('PostInEdit_IsPublished')).checked,
            this.tags
        );

        if (response.ok) {
            const url = this.service.getUrl(`${response.data.urlFriendlyTitle}`) // TODO: Dbl-check needed
            document.getElementById('post-submit-result').innerHTML = `<div style="color:darkgreen;margin-bottom:15px;">Post submitted successfully.</div><div><a href="${url}" target="_self">Refresh Page</a></div>`;
        } else {
            document.getElementById('post-submit-result').innerHTML = `<span style="color:darkred;">${response.status} ${response.statusText}<br />${response.message}<span>`;
        }

        window.setTimeout(function () {
            document.getElementById('post-submit-result').innerHTML = '';
        }, 10000);
    }

    public createPost = () => {
        window.location.href = this.service.getUrl('');
    }

    public loadLatestPosts = async () => {
        this.postList = await this.service.listLatestPosts();
        this.isPostListFromLatest = true;
        this.renderPostList();
    }

    public loadPostsByKeywords = async () => {
        this.postList = await this.service.listPostsWithTitleContainingKeywords(this.keywords);
        this.isPostListFromLatest = false;
        this.renderPostList();
    }

    private renderPostList() {
        this.postListElement.innerHTML = '';
        if (!this.postList) return;

        let html = this.isPostListFromLatest
            ? '<div class="current-post-list-type">Latest posts loaded.</div>'
            : '<div class="current-post-list-type">Search results loaded.</div>';

        if (this.postList.length === 0) {
            html += '<div>(Empty Results)</div>';
        }
        else {
            for (const post of this.postList) {
                html += `<div><a href="/admin/${post.urlFriendlyTitle}" target="_self">${post.title}</a></div>`;
            }
        }
        this.postListElement.innerHTML = html;
    }

    private renderTags() {
        this.tags = this.tags.sort((a, b) => PostEdit.tagComparitor(a, b));

        const tagsHtml = this.tags.reduce((previous, current, index) => {
            previous = `${previous} <div class="tag">${current} <a href="javascript:postEdit.deleteTag(${index})"><img src="/images/delete.svg" /></a></div> `;
            return previous;
        }, '');
        document.getElementById('edit-tags').innerHTML = tagsHtml;
    }

    private renderAllTags() {
        const dropdown = this.allTagsDropdown;

        dropdown.options.length = 0;
        if (!this.allTags || this.allTags.length === 0) return;

        this.allTags = this.allTags.sort((a, b) => PostEdit.tagComparitor(a, b));
        dropdown.add(new Option('', ''));
        this.allTags.map(x => {
            dropdown.add(new Option(x, x));
            return true;
        });
    }

    private static tagComparitor(a: string, b: string): number {
        if (a > b) return 1;
        if (a < b) return -1;
        return 0;
    }
}

const service = new Service();
const postEdit = new PostEdit(service);