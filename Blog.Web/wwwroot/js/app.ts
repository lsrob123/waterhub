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
}

class Service {
    public upsertPost = async (key: string, title: string, content: string, isSticky: boolean, tags: string[])
        : Promise<ApiCallResult> => {
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
                tags: tags
            })
        });

        if (!!rawResponse.ok) {
            const data = await rawResponse.json();
            return new ApiCallResult().withSuccess(data);
        }

        const message = await rawResponse.text();
        return new ApiCallResult().withError(message, rawResponse.status, rawResponse.statusText);
    }

    public getUrl(ralativePath: string): string {
        var rootPath = new RegExp(/^.*\//).exec(window.location.href);
        return encodeURI(`${rootPath}${ralativePath}`);
    }
}

class PostEdit {
    private readonly service: Service
    private tags: string[] = [];
    private allTags: string[] = [];
    private editorInstance: any;

    constructor(service: Service) {
        this.service = service;
    }

    public get hasAllTags() {
        return !!this.allTags && this.allTags.length > 0;
    }

    public init = (editorInstance: any) => {
        this.editorInstance = editorInstance;
        this.editorInstance.value = (<HTMLInputElement>document.getElementById('PostInEdit_Content')).value;

        this.tags = JSON.parse((<HTMLInputElement>document.getElementById('PostInEdit_TagsInText')).value);
        if (!this.tags) this.tags = [];
        console.log("Current Tags");
        console.log(this.tags);

        this.allTags = JSON.parse((<HTMLInputElement>document.getElementById('AllTagsInText')).value);
        if (!this.allTags) this.allTags = [];
        console.log("All Tags");
        console.log(this.allTags);

        this.renderTags();
    }

    public deleteTag = (index:number) => {
        this.tags.splice(index, 1);
        this.renderTags();
    };

    public addNewTag = () => {
        const value = (<HTMLInputElement>document.getElementById('PostInEdit_Key')).value;
        if (!value || value.trim() === '') return;

        const tags = value.split(/[ ,;:.]+/g);
        if (tags.length === 0) return;
        tags.map(x => {
            this.tags.push(x);
            return true;
        });
        this.tags = this.tags.sort((a, b) => {
            if (a > b) return 1;
            if (a < b) return -1;
            return 0;
        });
        this.renderTags();
    };

    public upsertPost = async () => {
        const response = await this.service.upsertPost(
            (<HTMLInputElement>document.getElementById('PostInEdit_Key')).value,
            (<HTMLInputElement>document.getElementById('PostInEdit_Title')).value,
            this.editorInstance.value,
            !!(<HTMLInputElement>document.getElementById('PostInEdit_IsSticky')).checked,
            this.tags
        );

        if (response.ok) {
            const url = this.service.getUrl(`${response.data.urlFriendlyTitle}`)
            document.getElementById('post-submit-result').innerHTML = `<div style="color:darkgreen;margin-bottom:15px;">Post submitted successfully.</div><div><a href="${url}" target="_self">Refresh Page</a></div>`;
        } else {
            document.getElementById('post-submit-result').innerHTML = `<span style="color:darkred;">${response.status} ${response.statusText}<br />${response.message}<span>`;
        }

        window.setTimeout(function () {
            document.getElementById('post-submit-result').innerHTML = '';
        }, 10000);
    }

    private renderTags() {
        const tagsHtml = this.tags.reduce((previous, current, index) => {
            return `${previous} <span>${current} <a href="javascript:postEdit.deleteTag(${index})">x</a></span> `;
        });
        document.getElementById('edit-tags').innerHTML = tagsHtml;
    }
}

const service = new Service();
const postEdit = new PostEdit(service);