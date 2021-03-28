import React from 'react';

const SearchBarResults = ({ show, label, results }) => {

    function shouldWeDisplayResultsComponent(flag) {
        return flag === true;
    }

    function outputResults(results) {
        var copy = JSON.parse(JSON.stringify(results));
        if (copy.length == 0)
            copy.push(0);
        else
            copy = copy.map(index => index + 1);
        
        return copy.join(", ");
    }

    const BarStyling = { width: "20rem", background: "#F2F1F9", border: "none", padding: "0.5rem" };

    return (
        <div style={{ marginTop: "20px", display: shouldWeDisplayResultsComponent(show) ? "block" : "none", userSelect: "none" }}>
            <div>
                <label>{label}</label>
            </div>
            <div style={BarStyling}>
                {outputResults(results)}
            </div>
        </div>
    );
}

export default SearchBarResults;