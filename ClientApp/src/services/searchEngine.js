class SearchEngineService {
    async Search(searchEngine, keyword, url) {
        var response = await fetch("searchEngine?searchEngineName=" + searchEngine + "&keyword=" + keyword + "&url=" + url);
        return response.json(); 
    }

    async GetSearchEngines() {
        var response = await fetch("searchengine/list");
        return response.json(); 
    }
}

var searchEngineServiceInstance = new SearchEngineService();
export default searchEngineServiceInstance;