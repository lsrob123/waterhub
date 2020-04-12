/// <reference path="constants.ts"/>

class ContactScreen {
    private readonly service: Service;

    constructor(service: Service) {
        this.service = service;
        this.setCaptchaBox = this.setCaptchaBox.bind(this);
    }

    public get captchaBox(): HTMLElement {
        return <HTMLElement>document.getElementById('c-contact');
    }

    public get submitButton(): HTMLButtonElement {
        return <HTMLButtonElement>document.getElementById('submit-contact');
    }

    public setCaptchaBox() {
        this.captchaBox.innerHTML = Constants.captachaContact;
    }

    public init() {
        if (!this.captchaBox) return;

        window.setTimeout(this.setCaptchaBox, 100);
    }

    public handleCaptcha(checkbox: HTMLInputElement) {
        this.submitButton.disabled = !checkbox.checked;
    }
}