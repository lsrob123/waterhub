var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var Settings = /** @class */ (function () {
    function Settings() {
        this.uriBase = '';
    }
    return Settings;
}());
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
    ApiCallResult.prototype.withError = function (message, status) {
        this.ok = false;
        this.status = status;
        this.data = undefined;
        this.message = message;
        return this;
    };
    return ApiCallResult;
}());
var Service = /** @class */ (function () {
    function Service(settings) {
        var _this = this;
        this.updateUploadImageDisplayOrderAsync = function (albumName, processedFileName, value) { return __awaiter(_this, void 0, void 0, function () {
            var request, uri, rawResponse, data;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        request = {
                            displayOrder: parseInt(value)
                        };
                        processedFileName = processedFileName.toLowerCase();
                        uri = "/api/album/" + albumName + "/image/" + processedFileName;
                        return [4 /*yield*/, fetch(uri, {
                                method: 'PUT',
                                headers: {
                                    'Content-Type': 'application/json'
                                },
                                body: JSON.stringify(request)
                            })];
                    case 1:
                        rawResponse = _a.sent();
                        if (!!!rawResponse.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, rawResponse.json()];
                    case 2:
                        data = _a.sent();
                        return [2 /*return*/, new ApiCallResult().withSuccess(data)];
                    case 3: return [2 /*return*/, new ApiCallResult().withError(rawResponse.statusText, rawResponse.status)];
                }
            });
        }); };
        this.settings = settings;
    }
    Service.prototype.getUrl = function (ralativePath) {
        var rootPath = new RegExp(/^.*\//).exec(window.location.href);
        return "" + rootPath + ralativePath;
    };
    return Service;
}());
var Album = /** @class */ (function () {
    function Album(settings, service) {
        this.settings = settings;
        this.service = service;
    }
    Album.prototype.getDisplayOrderSpan = function (processedFileName) {
        return document.getElementById("span_" + processedFileName);
    };
    Album.prototype.getDisplayOrderNumber = function (processedFileName) {
        return document.getElementById("number_" + processedFileName);
    };
    Album.prototype.getEditIcon = function (processedFileName) {
        return document.getElementById("icon_" + processedFileName);
    };
    Album.prototype.updateUploadImageDisplayOrderAsync = function (albumName, processedFileName, value) {
        return __awaiter(this, void 0, void 0, function () {
            var result;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.service.updateUploadImageDisplayOrderAsync(albumName, processedFileName, value)];
                    case 1:
                        result = _a.sent();
                        if (!result.ok) {
                            alert(result.message);
                            return [2 /*return*/];
                        }
                        this.getDisplayOrderSpan(processedFileName).innerText = result.data.displayOrder;
                        this.toggleEditState(processedFileName, false);
                        return [2 /*return*/];
                }
            });
        });
    };
    Album.prototype.toggleEditState = function (processedFileName, isEditing) {
        var number = this.getDisplayOrderNumber(processedFileName);
        var span = this.getDisplayOrderSpan(processedFileName);
        var icon = this.getEditIcon(processedFileName);
        var none = "none";
        var inline = "inline";
        if (!isEditing) {
            number.style.display = none;
            span.style.display = inline;
            icon.style.display = inline;
        }
        else {
            number.style.display = inline;
            span.style.display = none;
            icon.style.display = none;
            number.focus();
        }
    };
    Album.prototype.showToolTip = function (htmlContent, associatedElementId) {
        var tooltipContent = document.getElementById('tooltip-content');
        tooltipContent.innerHTML = htmlContent;
        var selection = window.getSelection();
        var range = document.createRange();
        range.selectNodeContents(tooltipContent);
        selection.removeAllRanges();
        selection.addRange(range);
        try {
            document.execCommand("copy");
        }
        catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();
        var associatedElement = document.getElementById(associatedElementId);
        var viewportOffset = associatedElement.getBoundingClientRect();
        var tooltip = document.getElementById('tooltip');
        tooltip.style.top = viewportOffset.top + 20 + "px";
        tooltip.style.left = viewportOffset.left - 5 + "px";
        tooltip.style.display = 'block';
    };
    Album.prototype.copyToolTip = function () {
        var tooltipContent = document.getElementById('tooltip-content');
        var selection = window.getSelection();
        var range = document.createRange();
        range.selectNodeContents(tooltipContent);
        selection.removeAllRanges();
        selection.addRange(range);
        try {
            document.execCommand("copy");
        }
        catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();
        this.hideToolTip();
    };
    Album.prototype.hideToolTip = function () {
        var tooltip = document.getElementById('tooltip');
        tooltip.style.display = 'none';
    };
    Album.prototype.copyHyperLink = function (elementId) {
        var element = document.getElementById(elementId);
        var content = element.innerHTML;
        element.innerHTML = this.service.getUrl(element.innerHTML);
        var selection = window.getSelection();
        var range = document.createRange();
        range.selectNodeContents(element);
        selection.removeAllRanges();
        selection.addRange(range);
        try {
            document.execCommand("copy");
        }
        catch (e) {
            console.error(e);
        }
        selection.removeAllRanges();
        element.innerHTML = content;
    };
    return Album;
}());
var settings = new Settings();
var service = new Service(settings);
var album = new Album(settings, service);
//# sourceMappingURL=bundle.js.map