/// <reference path="models.ts"/>

class Service {
    public upsertPost = async (key: string, title: string, abstract: string, content: string, isSticky: boolean, isPublished: boolean,
        tags: string[])
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
                    abstract: abstract,
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

    public listLatestPostInfoEntries = async (includeAllPosts: boolean = false): Promise<PostInfoEntry[]> => {
        try {
            const path = includeAllPosts ? '/api/posts/info/latest/all' : '/api/posts/info/latest';
            const rawResponse = await fetch(path, {
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

    public listPostInfoEntriesByKeywords = async (keywords: string, includeAllPosts: boolean = false): Promise<PostInfoEntry[]> => {
        try {
            const path = includeAllPosts ? '/api/posts/info/all' : '/api/posts/info';
            const rawResponse = await fetch(`${path}?keywords=${keywords}`, {
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
