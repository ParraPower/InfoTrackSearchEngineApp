import React from 'react';

const SearchBar = ({ keyword, setKeyword, label }) => {
    const BarStyling = { width: "20rem", background: "#F2F1F9", border: "none", padding: "0.5rem" };
    return (
        <div className={"app-search-bar"}>
            <div>
                <label>{label}</label>
            </div>
            <div>
            <input
                style={BarStyling}
                key="random1"
                value={keyword}
                placeholder={"search country"}
                onChange={(e) => setKeyword(e.target.value)}
                />
            </div>
        </div>
    );
}

export default SearchBar;