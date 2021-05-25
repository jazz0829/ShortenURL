import React from 'react';
import { useState } from 'react';
import './custom.css';
import { Container } from 'reactstrap';
import { NavMenu } from './components/NavMenu';

function App() {

    const [userInput, setUserInput] = useState(true);
    const [convert, setConvert] = useState(null);

    function getData(val) {
        setConvert("")
        setUserInput(val.target.value)
        console.warn(userInput)
    }

    function callAPI() {
        fetch("https://localhost:44327/api/ShortenURL?OriginalURL="+userInput).then(res => res.json()).then(
            result => {
                try {
                    setConvert(result.startsWith("https://shortenURL/") ? result : "Invalid URL");
                }
                catch {
                    setConvert("Invalid URL");
                }
            }
        )
    }

    return (
        <div>
            <NavMenu />
            <Container>
                <div className="App">
                    <label>Original URL</label><br />

                    <input type="text" className="OriginalURLInput" onChange={getData} /><br /><br />

                    <button className="btn-primary" onClick={callAPI} >Convert</button><br /><br />

                    <label>Shorten URL : </label>&nbsp;
                    <input type="text" className="ResultOutput" value={convert} />
                </div>
            </Container>
         </div>
        )
}

export default App;
