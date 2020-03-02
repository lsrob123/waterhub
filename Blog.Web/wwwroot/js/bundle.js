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
var PostImage = /** @class */ (function () {
    function PostImage() {
    }
    return PostImage;
}());
/// <reference path="models.ts"/>
var Service = /** @class */ (function () {
    function Service() {
        var _this = this;
        this.deletePostImage = function (postUrlFriendlyTitle, postImageKey) { return __awaiter(_this, void 0, void 0, function () {
            var rawResponse, message, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 3, , 4]);
                        return [4 /*yield*/, fetch("/posts/" + postUrlFriendlyTitle + "/images/" + postImageKey, {
                                method: 'DELETE',
                                headers: {
                                    'Content-Type': 'application/json'
                                }
                            })];
                    case 1:
                        rawResponse = _a.sent();
                        if (!!rawResponse.ok) {
                            return [2 /*return*/, new ApiCallResult().withSuccess(true)];
                        }
                        return [4 /*yield*/, rawResponse.text()];
                    case 2:
                        message = _a.sent();
                        return [2 /*return*/, new ApiCallResult().withError(message, rawResponse.status, rawResponse.statusText)];
                    case 3:
                        e_1 = _a.sent();
                        console.error(e_1);
                        return [2 /*return*/, new ApiCallResult().withLocalError(e_1)];
                    case 4: return [2 /*return*/];
                }
            });
        }); };
        this.upsertPost = function (key, title, abstract, content, isSticky, isPublished, tags, postImages) { return __awaiter(_this, void 0, void 0, function () {
            var rawResponse, data, message, e_2;
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
                                    abstract: abstract,
                                    content: content,
                                    isSticky: !!isSticky,
                                    isPublished: !!isPublished,
                                    tags: tags,
                                    images: postImages
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
                        e_2 = _a.sent();
                        console.error(e_2);
                        return [2 /*return*/, new ApiCallResult().withLocalError(e_2)];
                    case 6: return [2 /*return*/];
                }
            });
        }); };
        this.listLatestPosts = function () { return __awaiter(_this, void 0, void 0, function () {
            var rawResponse, data, e_3;
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
                        e_3 = _a.sent();
                        console.error(e_3);
                        return [2 /*return*/, null];
                    case 5: return [2 /*return*/];
                }
            });
        }); };
        this.listLatestPostInfoEntries = function (includeAllPosts) {
            if (includeAllPosts === void 0) { includeAllPosts = false; }
            return __awaiter(_this, void 0, void 0, function () {
                var path, rawResponse, data, e_4;
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
                            e_4 = _a.sent();
                            console.error(e_4);
                            return [2 /*return*/, null];
                        case 5: return [2 /*return*/];
                    }
                });
            });
        };
        this.listPostInfoEntriesByKeywords = function (keywords, includeAllPosts) {
            if (includeAllPosts === void 0) { includeAllPosts = false; }
            return __awaiter(_this, void 0, void 0, function () {
                var path, rawResponse, data, e_5;
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
                            e_5 = _a.sent();
                            console.error(e_5);
                            return [2 /*return*/, null];
                        case 5: return [2 /*return*/];
                    }
                });
            });
        };
    }
    Service.prototype.getUrl = function (ralativePath) {
        var rootPath = new RegExp(/^.*\//).exec(window.location.href);
        return encodeURI("" + rootPath + ralativePath);
    };
    return Service;
}());
var HomeScreen = /** @class */ (function () {
    function HomeScreen(service) {
        this.me = this;
        this.searchBoxDebounceId = null;
        this.isSearchBoxFocused = false;
        this.service = service;
    }
    Object.defineProperty(HomeScreen.prototype, "searchBox", {
        // Search
        get: function () {
            return document.getElementById('home-search-box');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeScreen.prototype, "searchDropDown", {
        get: function () {
            return document.getElementById('home-search-dropdown');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeScreen.prototype, "searchDropDownContent", {
        get: function () {
            return document.getElementById('home-search-dropdown-content');
        },
        enumerable: true,
        configurable: true
    });
    HomeScreen.prototype.startSearch = function () {
        return __awaiter(this, void 0, void 0, function () {
            var keywords, entries;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.isSearchBoxFocused) {
                            this.stopSearchDebounceTimer();
                            return [2 /*return*/];
                        }
                        keywords = this.searchBox.value;
                        if (!keywords || keywords.trim() === '') {
                            this.hideSearchdropDown();
                            return [2 /*return*/];
                        }
                        this.showSearchDropDown();
                        return [4 /*yield*/, this.service.listPostInfoEntriesByKeywords(keywords.trim())];
                    case 1:
                        entries = _a.sent();
                        this.searchDropDownContent.innerHTML = '';
                        if (!entries || entries.length === 0) {
                            this.searchDropDownContent.innerHTML = '<div>No Results</div>'; //TODO: Add text map
                            return [2 /*return*/];
                        }
                        this.searchDropDownContent.innerHTML = entries.reduce(function (prior, current) {
                            var titleWithJsOpen = "<a class=\"title\" href=\"javascript:homeScreen.displayFullContent('" + current.title + "','" + current.urlFriendlyTitle + "')\" title=\"" + current.textClickToReadFullArticle + "\">" + current.title + "</a>";
                            var linkToNewWindow = "<a class=\"new-window-link\" href=\"/posts/" + current.urlFriendlyTitle + "\" title=\"" + current.textOpenArticleInNewWindow + "\" target=\"" + current.urlFriendlyTitle + "\">" + current.textOpenArticleInNewWindow + "</a>";
                            return prior + "<div class=\"post\">" + titleWithJsOpen + " " + linkToNewWindow + "</div>";
                        }, '');
                        return [2 /*return*/];
                }
            });
        });
    };
    HomeScreen.prototype.onSearchBoxUpdated = function () {
        this.stopSearchDebounceTimer();
        this.searchBoxDebounceId = window.setTimeout(function () {
            this.startSearch();
        }.bind(this), 500);
    };
    HomeScreen.prototype.stopSearchDebounceTimer = function () {
        if (!!this.searchBoxDebounceId) {
            clearTimeout(this.searchBoxDebounceId);
            this.searchBoxDebounceId = null;
        }
    };
    HomeScreen.prototype.onSearchBoxFocused = function () {
        this.isSearchBoxFocused = true;
    };
    HomeScreen.prototype.onSearchBoxBlurred = function () {
        this.isSearchBoxFocused = false;
    };
    HomeScreen.prototype.clearSearch = function () {
        this.searchBox.value = null;
        this.searchDropDown.innerHTML = '';
        this.hideSearchdropDown();
    };
    HomeScreen.prototype.hideSearchdropDown = function () {
        this.searchDropDown.style.display = "none";
        this.columnContainer.style.overflow = 'auto';
        this.searchBoxDebounceId = null;
    };
    HomeScreen.prototype.showSearchDropDown = function () {
        var searchDropDown = this.searchDropDown;
        var viewportOffset = this.searchBox.getBoundingClientRect();
        var top = viewportOffset.bottom + 3;
        searchDropDown.style.top = top + "px";
        var left = viewportOffset.left + 7;
        searchDropDown.style.left = left + "px";
        searchDropDown.style.width = viewportOffset.width + "px";
        var searchDropDownContent = this.searchDropDownContent;
        searchDropDownContent.style.top = top + 65 + "px";
        searchDropDownContent.style.left = left + "px";
        searchDropDownContent.style.width = viewportOffset.width - 50 + "px";
        this.searchDropDown.style.display = "block";
        this.columnContainer.style.overflow = 'hidden';
    };
    HomeScreen.prototype.searchByTag = function (tag) {
        if (!tag)
            return;
        this.searchBox.value = tag;
        this.startSearch();
    };
    Object.defineProperty(HomeScreen.prototype, "columnContainer", {
        // Show post
        get: function () {
            return document.getElementById('column-container');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeScreen.prototype, "modalFullArticle", {
        get: function () {
            return document.getElementById('modal-full-article');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeScreen.prototype, "modalPostTitle", {
        get: function () {
            return document.getElementById('modal-post-title');
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HomeScreen.prototype, "modalPostContent", {
        get: function () {
            return document.getElementById('modal-post-content');
        },
        enumerable: true,
        configurable: true
    });
    HomeScreen.prototype.displayFullContent = function (title, urlFriendlyTitle) {
        this.modalFullArticle.style.display = 'flex';
        this.columnContainer.style.overflow = 'hidden';
        this.modalPostTitle.innerHTML = title;
        this.modalPostContent.innerHTML = document.getElementById("div-" + urlFriendlyTitle).innerHTML;
    };
    HomeScreen.prototype.collapseFullContent = function () {
        this.modalFullArticle.style.display = 'none';
        this.columnContainer.style.overflow = 'auto';
        this.modalPostTitle.innerHTML = null;
        this.modalPostContent.innerHTML = null;
    };
    return HomeScreen;
}());
var AdminScreen = /** @class */ (function () {
    function AdminScreen(service) {
        var _this = this;
        this.tags = [];
        this.postImages = [];
        this.allTags = [];
        this.init = function (editorInstance) {
            _this.editorInstance = editorInstance;
            _this.editorInstance.value = document.getElementById('PostInEdit_Content').value;
            var tagsInText = document.getElementById('PostInEdit_TagsInText').value;
            if (!!tagsInText) {
                _this.tags = JSON.parse(tagsInText);
                if (!_this.tags)
                    _this.tags = [];
            }
            else {
                _this.tags = [];
            }
            var postImagesInText = document.getElementById('PostImagesInText').value;
            if (!!postImagesInText) {
                _this.postImages = JSON.parse(postImagesInText);
            }
            else {
                _this.postImages = [];
            }
            var allTagsInText = document.getElementById('AllTagsInText').value;
            if (!!allTagsInText) {
                _this.allTags = JSON.parse(allTagsInText);
                if (!_this.allTags)
                    _this.allTags = [];
            }
            else {
                _this.allTags = [];
            }
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
                    case 0: return [4 /*yield*/, this.service.upsertPost(document.getElementById('PostInEdit_Key').value, document.getElementById('PostInEdit_Title').value, document.getElementById('PostInEdit_Abstract').value, this.editorInstance.value, !!document.getElementById('PostInEdit_IsSticky').checked, !!document.getElementById('PostInEdit_IsPublished').checked, this.tags, this.postImages)];
                    case 1:
                        response = _a.sent();
                        if (response.ok) {
                            url_1 = "/admin/" + response.data.urlFriendlyTitle;
                            document.getElementById('post-submit-result').innerHTML = "<div style=\"margin-bottom:15px;\">Post submitted successfully.</div><div><a style=\"color:white;\" href=\"" + url_1 + "\" target=\"_self\">Refresh Page</a></div>";
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
        this.copyPostImageUrl = function (urlField, messageField) {
            var messageBlock = document.getElementById(messageField);
            messageBlock.style.display = "none";
            var content = document.getElementById(urlField);
            var selection = window.getSelection();
            var range = document.createRange();
            range.selectNodeContents(content);
            selection.removeAllRanges();
            selection.addRange(range);
            try {
                document.execCommand("copy");
                messageBlock.style.display = "block";
                window.setTimeout(function () { messageBlock.style.display = "none"; }.bind(_this), 3000);
            }
            catch (e) {
                console.error(e);
            }
            selection.removeAllRanges();
        };
        this.deletePostImage = function (posturlFriendlyTitle, postImageKey, messageField) { return __awaiter(_this, void 0, void 0, function () {
            var result, messageBlock;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.service.deletePostImage(posturlFriendlyTitle, postImageKey)];
                    case 1:
                        result = _a.sent();
                        if (result.ok) {
                            window.location.href = "/admin/" + posturlFriendlyTitle;
                            return [2 /*return*/];
                        }
                        messageBlock = document.getElementById(messageField);
                        messageBlock.innerText = result.status + " " + result.message;
                        messageBlock.style.display = "block";
                        window.setTimeout(function () { messageBlock.style.display = "none"; }.bind(this), 3000);
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