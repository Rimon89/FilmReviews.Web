function movieSearch() {
    var action_src = "https://localhost:44367/movie/" + document.getElementsByName("keywords")[0].value;
    var searchForm = document.getElementById('searchForm');
    searchForm.action = action_src;
}