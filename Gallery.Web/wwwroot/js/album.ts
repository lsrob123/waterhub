class Album {
    private readonly settings: Settings;
    private readonly service: Service;

    private getDisplayOrderSpan(processedFileName: string): HTMLElement {
        return document.getElementById(`span_${processedFileName}`);
    }

    private getDisplayOrderNumber(processedFileName: string): HTMLElement {
        return document.getElementById(`number_${processedFileName}`);
    }

    private getEditIcon(processedFileName: string): HTMLElement {
        return document.getElementById(`icon_${processedFileName}`);
    }

    constructor(settings: Settings, service: Service) {
        this.settings = settings;
        this.service = service;
    }

    public async updateUploadImageDisplayOrderAsync(albumName: string, processedFileName: string, value: any) {
        const result: ApiCallResult = await this.service.updateUploadImageDisplayOrderAsync(albumName, processedFileName, value);

        if (!result.ok) {
            alert(result.message);
            return;
        }

        this.getDisplayOrderSpan(processedFileName).innerText = result.data.displayOrder;
        this.toggleEditState(processedFileName, false);
    }

    public toggleEditState(processedFileName: string, isEditing: boolean) {
        const number = this.getDisplayOrderNumber(processedFileName);
        const span = this.getDisplayOrderSpan(processedFileName);
        const icon = this.getEditIcon(processedFileName);
        const none = "none";
        const inline = "inline";

        if (!isEditing) {
            number.style.display = none;
            span.style.display = inline;
            icon.style.display = inline;
        } else {
            number.style.display = inline;
            span.style.display = none;
            icon.style.display = none;
            number.focus();
        }
    }

    public showToolTip(htmlContent: string, associatedElementId: string) {
        const tooltipContent = document.getElementById('tooltip-content');
        tooltipContent.innerHTML = htmlContent;

        const selection = window.getSelection();
        const range = document.createRange();
        range.selectNodeContents(tooltipContent);
        selection.removeAllRanges();
        selection.addRange(range);

        try {
            document.execCommand("copy");
        } catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();

        const associatedElement = document.getElementById(associatedElementId);
        var viewportOffset = associatedElement.getBoundingClientRect();

        const tooltip = document.getElementById('tooltip');
        tooltip.style.top = `${viewportOffset.top + 20}px`;
        tooltip.style.left = `${viewportOffset.left - 5}px`;

        tooltip.style.display = 'block';
    }

    public copyToolTip() {
        const tooltipContent = document.getElementById('tooltip-content');
        const selection = window.getSelection();
        const range = document.createRange();
        range.selectNodeContents(tooltipContent);
        selection.removeAllRanges();
        selection.addRange(range);

        try {
            document.execCommand("copy");
        } catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();
        this.hideToolTip();
    }

     public hideToolTip() {
        const tooltip = document.getElementById('tooltip');
        tooltip.style.display = 'none';
    }

    public copyHyperLink(elementId) {
        const element = document.getElementById(elementId);
        const content = element.innerHTML;
        element.innerHTML = this.service.getUrl(element.innerHTML);

        const selection = window.getSelection();
        const range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);

        try {
            document.execCommand("copy");
        } catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();
        element.innerHTML = content;
    }

}