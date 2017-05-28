function news() {
    var news = {
        bingnewsId: ko.observable(),
        delNews() {
            alert(this.bingnewsId);
        }
    };
    ko.applyBindings(news);
}
//# sourceMappingURL=index.js.map