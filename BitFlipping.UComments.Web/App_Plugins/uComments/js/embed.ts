namespace uComments {
    export interface IEmbedOptions {
        url: string,
        pageId: string,
        template: string
    }
    interface ICommentModel {

    }

    class Embed {
        private comments: ICommentModel[] = [];
        private form: HTMLFormElement;

        constructor(private element: HTMLElement, private options?: IEmbedOptions) {
            if (!this.options) {
                this.options = <any>element.dataset;
            }

            this.comments = [];

            this.getComments();

            this.form = <HTMLFormElement>document.getElementById("new-comment");
            //$(this.form).on('submit', )
        }

        public getComments() {
            var url = this.options.url + "/getComments?pageId=" + this.options.pageId;
            $.ajax({
                url: url,
                async: true,
                method: 'GET',
                contentType: 'application/json'
            }).done((response) => {
                this.comments = response;
                this.render();
            });
        }

        public validateForm() {

        }

        public createComment() {
            var formData = $(this.form).serialize();
            $.ajax({
                url: this.options.url + "/createComment",
                async: true,
                method: 'POST',
                contentType: 'application/json',
                dataType: 'application/json',
                data: formData
            }).done((response) => {

            });
        }

        public render() {
            var template = $(this.options.template).html();
            Mustache.parse(template);   // optional, speeds up future uses
            var rendered = Mustache.render(template, this.comments);
            $(this.element).html(rendered);
        }
    }

    (function () {
        window.onload = () => {
            var container = document.getElementById('ucomments');
            var instance = new Embed(container);
        }
    })();
}

declare interface String {
    startsWith(search: string, pos?: number): boolean;
}

/**
* Polyfill
* This method has been added to the ECMAScript 2015 specification and may not be available in all JavaScript implementations yet.
* However, you can polyfill String.prototype.startsWith() with the following snippet:
*/
if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (search, pos) {
        return this.substr(!pos || pos < 0 ? 0 : +pos, search.length) === search;
    };
}