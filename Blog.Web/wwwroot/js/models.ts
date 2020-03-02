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

class PostInfoEntry {
    key: string;
    isPublished: boolean;
    isSticky: boolean;
    tags: string[];
    title: string;
    urlFriendlyTitle: string;
    textClickToReadFullArticle: string;
    textReadFullArticle: string;
    textOpenArticleInNewWindow: string;
}

class PostImage {
    key: string;
    public extension: string;
    public internalId: number;
    public displayName: string;
}

