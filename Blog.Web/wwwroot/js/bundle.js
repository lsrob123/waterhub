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
var PostInfoEntry = /** @class */ (function () {
    function PostInfoEntry() {
    }
    return PostInfoEntry;
}());
/// <reference path="models.ts"/>
var Service = /** @class */ (function () {
    function Service() {
        var _this = this;
        this.upsertPost = function (key, title, content, isSticky, isPublished, tags) { return __awaiter(_this, void 0, void 0, function () {
            var rawResponse, data, message, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 5, , 6]);
                        return [4 /*yield*/, fetch('/api/posts', {
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
                            })];
                    case 1:
                        rawResponse = _a.sent();
                        if (!!!rawResponse.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, rawResponse.json()];
                    case 2:
                        data = _a.sent();
                        return [2 /*return*/, new ApiCallResult().withSuccess(data)];
                    case 3: return [4 /*yield*/, rawResponse.text()];
                    case 4:
                        message = _a.sent();
                        return [2 /*return*/, new ApiCallResult().withError(message, rawResponse.status, rawResponse.statusText)];
                    case 5:
                        e_1 = _a.sent();
                        console.error(e_1);
                        return [2 /*return*/, new ApiCallResult().withLocalError(e_1)];
                    case 6: return [2 /*return*/];
                }
            });
        }); };
        this.listLatestPosts = function () { return __awaiter(_this, void 0, void 0, function () {
            var rawResponse, data, e_2;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 4, , 5]);
                        return [4 /*yield*/, fetch('/api/posts/latest', {
                                method: 'GET',
                            })];
                    case 1:
                        rawResponse = _a.sent();
                        if (!!!rawResponse.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, rawResponse.json()];
                    case 2:
                        data = _a.sent();
                        return [2 /*return*/, data];
                    case 3: return [2 /*return*/, []];
                    case 4:
                        e_2 = _a.sent();
                        console.error(e_2);
                        return [2 /*return*/, null];
                    case 5: return [2 /*return*/];
                }
            });
        }); };
        this.listLatestPostInfoEntries = function (includeAllPosts) {
            if (includeAllPosts === void 0) { includeAllPosts = false; }
            return __awaiter(_this, void 0, void 0, function () {
                var path, rawResponse, data, e_3;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0:
                            _a.trys.push([0, 4, , 5]);
                            path = includeAllPosts ? '/api/posts/info/latest/all' : '/api/posts/info/latest';
                            return [4 /*yield*/, fetch(path, {
                                    method: 'GET',
                                })];
                        case 1:
                            rawResponse = _a.sent();
                            if (!!!rawResponse.ok) return [3 /*break*/, 3];
                            return [4 /*yield*/, rawResponse.json()];
                        case 2:
                            data = _a.sent();
                            return [2 /*return*/, data];
                        case 3: return [2 /*return*/, []];
                        case 4:
                            e_3 = _a.sent();
                            console.error(e_3);
                            return [2 /*return*/, null];
                        case 5: return [2 /*return*/];
                    }
                });
            });
        };
        this.listPostInfoEntriesByKeywords = function (keywords, includeAllPosts) {
            if (includeAllPosts === void 0) { includeAllPosts = false; }
            return __awaiter(_this, void 0, void 0, function () {
                var path, rawResponse, data, e_4;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0:
                            _a.trys.push([0, 4, , 5]);
                            path = includeAllPosts ? '/api/posts/info/all' : '/api/posts/info';
                            return [4 /*yield*/, fetch(path + "?keywords=" + keywords, {
                                    method: 'GET',
                                })];
                        case 1:
                            rawResponse = _a.sent();
                            if (!!!rawResponse.ok) return [3 /*break*/, 3];
                            return [4 /*yield*/, rawResponse.json()];
                        case 2:
                            data = _a.sent();
                            return [2 /*return*/, data];
                        case 3: return [2 /*return*/, []];
                        case 4:
                            e_4 = _a.sent();
                            console.error(e_4);
                            return [2 /*return*/, null];
                        case 5: return [2 /*return*/];
                    }
                });
            });
        };
        //public listPostsWithTitleContainingKeywords = async (keywords: string): Promise<Post[]> => {
        //    try {
        //        const rawResponse = await fetch(`/api/posts?keywords=${keywords}`, {
        //            method: 'GET',
        //        });
        //        if (!!rawResponse.ok) {
        //            const data = await rawResponse.json();
        //            return data;
        //        }
        //        return [];
        //    } catch (e) {
        //        console.error(e);
        //        return null;
        //    }
        //}
    }
    Service.prototype.getUrl = function (ralativePath) {
        var rootPath = new RegExp(/^.*\//).exec(window.location.href);
        return encodeURI("" + rootPath + ralativePath);
    };
    return Service;
}());
var HomeScreen = /** @class */ (function () {
    function HomeScreen(service) {
        var _this = this;
        this.startSearch = function () { return __awaiter(_this, void 0, void 0, function () {
            var keywords;
            return __generator(this, function (_a) {
                keywords = this.searchBox.value;
                return [2 /*return*/];
            });
        }); };
        this.searchBoxDebounceId = null;
        this.isSearchBoxFocused = false;
        this.onSearchBoxUpdated = function () {
            if (!!_this.searchBoxDebounceId) {
                clearTimeout(_this.searchBoxDebounceId);
                _this.searchBoxDebounceId = null;
            }
            if (!_this.isSearchBoxFocused) {
                return;
            }
            _this.searchBoxDebounceId = window.setTimeout(function () {
                if (!!this.isSearchBoxFocused) {
                    this.startSearch();
                }
            }, 500);
        };
        this.onSearchBoxFocused = function () {
            _this.isSearchBoxFocused = true;
        };
        this.onSearchBoxBlurred = function () {
            _this.isSearchBoxFocused = false;
            _this.startSearch();
        };
        this.service = service;
    }
    Object.defineProperty(HomeScreen.prototype, "searchBox", {
        get: function () {
            return document.getElementById('home-search-box');
        },
        enumerable: true,
        configurable: true
    });
    return HomeScreen;
}());
var AdminScreen = /** @class */ (function () {
    function AdminScreen(service) {
        var _this = this;
        this.tags = [];
        this.allTags = [];
        this.init = function (editorInstance) {
            _this.editorInstance = editorInstance;
            _this.editorInstance.value = document.getElementById('PostInEdit_Content').value;
            _this.tags = JSON.parse(document.getElementById('PostInEdit_TagsInText').value);
            if (!_this.tags)
                _this.tags = [];
            _this.renderTags();
            _this.allTags = JSON.parse(document.getElementById('AllTagsInText').value);
            if (!_this.allTags)
                _this.allTags = [];
            _this.renderAllTags();
        };
        this.loadDataAsync = function () { return __awaiter(_this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.loadLatestPostInfoEntries()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        this.deleteTag = function (index) {
            _this.tags.splice(index, 1);
            _this.renderTags();
        };
        this.addNewTags = function () {
            var value = document.getElementById('edit-new-tag').value;
            if (!value || value.trim() === '')
                return;
            var tags = value.split(/[ ,;:.]+/g);
            if (tags.length === 0)
                return;
            tags.map(function (tag) {
                var existing = _this.tags.findIndex(function (x) { return x === tag; });
                if (existing < 0)
                    _this.tags.push(tag);
                return true;
            });
            _this.renderTags();
        };
        this.selectTag = function () {
            var dropdown = _this.allTagsDropdown;
            if (dropdown.selectedIndex === 0)
                return;
            var tag = (dropdown.options[dropdown.options.selectedIndex].value);
            var existing = _this.tags.findIndex(function (x) { return x === tag; });
            if (existing >= 0)
                return;
            _this.tags.push(tag);
            _this.renderTags();
        };
        this.upsertPost = function () { return __awaiter(_this, void 0, void 0, function () {
            var response, url_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.service.upsertPost(document.getElementById('PostInEdit_Key').value, document.getElementById('PostInEdit_Title').value, this.editorInstance.value, !!document.getElementById('PostInEdit_IsSticky').checked, !!document.getElementById('PostInEdit_IsPublished').checked, this.tags)];
                    case 1:
                        response = _a.sent();
                        if (response.ok) {
                            url_1 = "/admin/" + response.data.urlFriendlyTitle;
                            document.getElementById('post-submit-result').innerHTML = "<div style=\"color:darkgreen;margin-bottom:15px;\">Post submitted successfully.</div><div><a href=\"" + url_1 + "\" target=\"_self\">Refresh Page</a></div>";
                            window.setTimeout(function () {
                                window.location.href = url_1;
                            }, 1000);
                        }
                        else {
                            document.getElementById('post-submit-result').innerHTML = "<span style=\"color:darkred;\">" + response.status + " " + response.statusText + "<br />" + response.message + "<span>";
                        }
                        window.setTimeout(function () {
                            document.getElementById('post-submit-result').innerHTML = '';
                        }, 10000);
                        return [2 /*return*/];
                }
            });
        }); };
        this.createPost = function () {
            window.location.href = _this.service.getUrl('');
        };
        this.loadLatestPostInfoEntries = function () { return __awaiter(_this, void 0, void 0, function () {
            var _a;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _a = this;
                        return [4 /*yield*/, this.service.listLatestPostInfoEntries(true)];
                    case 1:
                        _a.postInfoEntries = _b.sent();
                        this.isPostListFromLatest = true;
                        this.renderPostList();
                        return [2 /*return*/];
                }
            });
        }); };
        this.loadPostInfoEntriesByKeywords = function () { return __awaiter(_this, void 0, void 0, function () {
            var _a;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _a = this;
                        return [4 /*yield*/, this.service.listPostInfoEntriesByKeywords(this.keywords, true)];
                    case 1:
                        _a.postInfoEntries = _b.sent();
                        this.isPostListFromLatest = false;
                        this.renderPostList();
                        return [2 /*return*/];
                }
            });
        }); };
        this.service = service;
    }
    Object.defineProperty(AdminScreen.prototype, "hasAllTags", {
        get: function () {
            return !!this.allTags && this.allTags.length > 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdminScreen.prototype, "allTagsDropdown", {
        get: function () {
            return document.getElementById('edit-all-tags');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdminScreen.prototype, "postInfoEntriesElement", {
        get: function () {
            return document.getElementById('edit-post-list');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AdminScreen.prototype, "keywords", {
        get: function () {
            var value = document.getElementById('edit-post-search-keywords').value;
            return value;
        },
        enumerable: true,
        configurable: true
    });
    AdminScreen.prototype.renderPostList = function () {
        this.postInfoEntriesElement.innerHTML = '';
        if (!this.postInfoEntries)
            return;
        var html = this.isPostListFromLatest
            ? '<div class="current-post-list-type">Latest posts loaded.</div>'
            : '<div class="current-post-list-type">Search results loaded.</div>';
        if (this.postInfoEntries.length === 0) {
            html += '<div>(Empty Results)</div>';
        }
        else {
            for (var _i = 0, _a = this.postInfoEntries; _i < _a.length; _i++) {
                var post = _a[_i];
                html += "<div><a href=\"/admin/" + post.urlFriendlyTitle + "\" target=\"_self\">" + post.title + "</a></div>";
            }
        }
        this.postInfoEntriesElement.innerHTML = html;
    };
    AdminScreen.prototype.renderTags = function () {
        this.tags = this.tags.sort(function (a, b) { return AdminScreen.tagComparitor(a, b); });
        var tagsHtml = this.tags.reduce(function (previous, current, index) {
            previous = previous + " <div class=\"tag\">" + current + " <a href=\"javascript:adminScreen.deleteTag(" + index + ")\"><img src=\"/images/delete.svg\" /></a></div> ";
            return previous;
        }, '');
        document.getElementById('edit-tags').innerHTML = tagsHtml;
    };
    AdminScreen.prototype.renderAllTags = function () {
        var dropdown = this.allTagsDropdown;
        dropdown.options.length = 0;
        if (!this.allTags || this.allTags.length === 0)
            return;
        this.allTags = this.allTags.sort(function (a, b) { return AdminScreen.tagComparitor(a, b); });
        dropdown.add(new Option('', ''));
        this.allTags.map(function (x) {
            dropdown.add(new Option(x, x));
            return true;
        });
    };
    AdminScreen.tagComparitor = function (a, b) {
        if (a > b)
            return 1;
        if (a < b)
            return -1;
        return 0;
    };
    return AdminScreen;
}());
/// <reference path="service.ts"/>
/// <reference path="home-screen.ts"/>
/// <reference path="admin-screen.ts"/>
var service = new Service();
var homeScreen = new HomeScreen(service);
var adminScreen = new AdminScreen(service);
//# sourceMappingURL=bundle.js.map