"use strict";
var uComments;
(function (uComments) {
    var Embed = (function () {
        function Embed(element, options) {
            this.element = element;
            this.options = options;
            this.comments = [];
            if (!this.options) {
                this.options = element.dataset;
            }
            this.comments = [];
            this.getComments();
            this.form = document.getElementById("new-comment");
        }
        Embed.prototype.getComments = function () {
            var _this = this;
            var url = this.options.url + "/getComments?pageId=" + this.options.pageId;
            $.ajax({
                url: url,
                async: true,
                method: 'GET',
                contentType: 'application/json'
            }).done(function (response) {
                _this.comments = response;
                _this.render();
            });
        };
        Embed.prototype.validateForm = function () {
        };
        Embed.prototype.createComment = function () {
            var formData = $(this.form).serialize();
            $.ajax({
                url: this.options.url + "/createComment",
                async: true,
                method: 'POST',
                contentType: 'application/json',
                dataType: 'application/json',
                data: formData
            }).done(function (response) {
            });
        };
        Embed.prototype.render = function () {
            var template = $(this.options.template).html();
            Mustache.parse(template);
            var rendered = Mustache.render(template, this.comments);
            $(this.element).html(rendered);
        };
        return Embed;
    }());
    (function () {
        window.onload = function () {
            var container = document.getElementById('ucomments');
            var instance = new Embed(container);
        };
    })();
})(uComments || (uComments = {}));
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (search, pos) {
        return this.substr(!pos || pos < 0 ? 0 : +pos, search.length) === search;
    };
}
//# sourceMappingURL=embed.js.map