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

    public init = (editorInstance: any) => {
        this.editorInstance = editorInstance;
        this.editorInstance.value = (<HTMLInputElement>document.getElementById('PostInEdit_Content')).value;

        this.tags = JSON.parse((<HTMLInputElement>document.getElementById('PostInEdit_TagsInText')).value);
        console.log("Current Tags");
        console.log(this.tags);

        this.allTags = JSON.parse((<HTMLInputElement>document.getElementById('AllTagsInText')).value);
        console.log("All Tags");
        console.log(this.allTags);
    }

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
}

const service = new Service();
const postEdit = new PostEdit(service);