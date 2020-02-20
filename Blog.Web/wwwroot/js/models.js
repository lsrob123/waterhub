var ApiCallResult = /** @class */ (function () {
    function ApiCallResult() {
    }
    ApiCallResult.prototype.withSuccess = function (data, status) {
        if (status === void 0) { status = 200; }
        this.ok = true;
        this.status = 200;
        this.data = data;
        return this;
    };
    ApiCallResult.prototype.withError = function (message, status, statusText) {
        this.ok = false;
        this.status = status;
        this.statusText = statusText;
        this.data = undefined;
        this.message = message;
        return this;
    };
    ApiCallResult.prototype.withLocalError = function (message) {
        this.ok = false;
        this.data = undefined;
        this.message = message;
        return this;
    };
    return ApiCallResult;
}());
var Tag = /** @class */ (function () {
    function Tag() {
    }
    return Tag;
}());
var Post = /** @class */ (function () {
    function Post() {
    }
    return Post;
}());
//# sourceMappingURL=models.js.map