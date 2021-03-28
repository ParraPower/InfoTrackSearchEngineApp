import React, { Component } from 'react';
import SearchBar from './shared/Search';
import SearchBarResults from './shared/SearchResults';
import searchEngineServiceInstance from '../services/searchEngine';

export class Home extends Component {
  static displayName = Home.name;

    keyword = "online title search";
    url = null;
    searchResults = [];
    hasRunSearch = false;

    constructor() {
        super();
        this.state = {
            searchResults: []
        };
        this.onSetKeyword = this.onSetKeyword.bind(this);
        this.onSetUrl = this.onSetUrl.bind(this);
        this.recordSearchResults = this.recordSearchResults.bind(this);
    }

    onSetKeyword = function (keyword) {
        this.keyword = keyword;

        var that = this;
        
        searchEngineServiceInstance.Search("Google", this.keyword, this.url)
            .then(resp => {
                that.recordSearchResults(resp);
            });
    }

    onSetUrl = function (keyword) {
        this.url = keyword;

        var that = this;

        searchEngineServiceInstance.Search("Google", this.keyword, this.url)
            .then(resp => {
                that.recordSearchResults(resp);
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
        <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
        </div>
    );
    }
}
