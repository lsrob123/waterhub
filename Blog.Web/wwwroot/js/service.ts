/// <reference path="models.ts"/>

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

    public listLatestPostInfoEntries = async (): Promise<PostInfoEntry[]> => {
        try {
            const rawResponse = await fetch('/api/posts/latest/info', {
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
