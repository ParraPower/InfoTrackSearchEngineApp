import React from 'react';


const RadioList = ({ id, label, valueList, setValue, selectedValue }) => {
    

    const BarStyling = { width: "20rem", background: "#F2F1F9", border: "none", padding: "0.5rem" };

    var radioListJsx = [];
    valueList.forEach(val => {
        radioListJsx.push(
            <div className={"option"} style={{ float: "left" }}>

                <input
                    name={id}
                    id={id + val.name}
                    key={val.name}
                    type={"radio"}
                    value={val.name}
                    onChange={(e) => setValue(e.target.value)}
                    checked={selectedValue === val.name}
                />
                <label style={{ marginLeft: "5px" }}
                    htmlFor={id + val.name}>
                    {val.name}
                </label>
            </div>
        );
    });


    return (
        <div id={id} className={"app-radio-list"}>
            <div>
                <label>{label}</label>
            </div>
            <div>
                {radioListJsx}
                <div style={{ clear: "both" }}>

                </div>
            </div>
        </div>
    );
}

export default RadioList;