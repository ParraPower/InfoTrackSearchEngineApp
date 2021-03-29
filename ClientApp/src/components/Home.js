import React, { Component } from 'react';
import RadioList from './shared/RadioList';
import SearchBar from './shared/Search';
import SearchBarResults from './shared/SearchResults';
import searchEngineServiceInstance from '../services/searchEngine';

export class Home extends Component {
  static displayName = Home.name;

    keyword = "online title search";
    url = null;
    searchResults = [];
    searchEngines = [];
    hasRunSearch = false;
    selectedSearchEngine = "";

    constructor() {
        super();
        this.state = {
            selectedSearchEngine: "",
            searchResults: [],
            searchEngines: []
        };
        this.onSetKeyword = this.onSetKeyword.bind(this);
        this.onSetUrl = this.onSetUrl.bind(this);
        this.onSetSearchEngine = this.onSetSearchEngine.bind(this);
        this.recordSearchResults = this.recordSearchResults.bind(this);        
    }

    componentDidMount() {
        var that = this;

        searchEngineServiceInstance.GetSearchEngines()
            .then(resp => {
                that.searchEngines = resp;

                var defaultSearchEngine = resp.find(x => x.isDefault);

                if (typeof defaultSearchEngine === "object")
                    that.selectedSearchEngine = defaultSearchEngine.name;
                else if (resp.length > 0)
                    that.selectedSearchEngine = resp[0].name;
                    
                that.setState({
                    searchEngines: resp
                });
            });
    }

    onSetKeyword = function (keyword) {
        this.keyword = keyword;

        var that = this;

        searchEngineServiceInstance.Search(that.selectedSearchEngine, this.keyword, this.url)
            .then(resp => {
                that.recordSearchResults(resp);
            });
    }

    onSetUrl = function (keyword) {
        this.url = keyword;

        var that = this;

        searchEngineServiceInstance.Search(that.selectedSearchEngine, this.keyword, this.url)
            .then(resp => {
                that.recordSearchResults(resp);
            });
    }

    onSetSearchEngine = function (name) {
        var that = this;

        that.selectedSearchEngine = name;
        that.setState({
            selectedSearchEngine: name
        });
    }

    recordSearchResults = function (results) {
        console.log(results);

        this.hasRunSearch = true;
        this.searchResults = results;
        this.setState({
            searchResults: this.searchResults
        });
    }

    render () {

    return (
        <div>
            <RadioList
                id={"search_engines"}
                label={"Search Engines"}
                valueList={this.searchEngines}
                setValue={this.onSetSearchEngine}
                selectedValue={this.selectedSearchEngine}
                />

            <SearchBar
                keyword={this.keyword}
                setKeyword={this.onSetKeyword}
                label={"Keyword"}
            />
            <SearchBar
                setKeyword={this.onSetUrl}
                label={"Url"}
            />
            <SearchBarResults
                show={this.hasRunSearch}
                label={"Keyword and Url Search Results"}
                results={this.searchResults}
                
            />
            <br />
            <p>The <code>Application</code> will search <code>search engines</code> for the respective matches against the <code>keyword</code> and <code>url</code> fields. The search <code>fires automatically</code> when you type into the text boxes. No need to click a button.</p>
        </div>
    );
    }
}
