/// <reference path="constants.ts"/>

class BusinessesScreen {
    private readonly service: Service;

    constructor(service: Service) {
        this.service = service;
        this.setCaptchaBox = this.setCaptchaBox.bind(this);
    }

    public get captchaBox(): HTMLElement {
        return <HTMLElement>document.getElementById('c-businesses');
    }

    public setCaptchaBox() {
        this.captchaBox.innerHTML = Constants.captachaBusinesses;
    }

    public init() {
        if (!this.captchaBox) return;

        window.setTimeout(this.setCaptchaBox, 100);
    }
}