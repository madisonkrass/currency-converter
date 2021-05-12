import React, { Component } from 'react';
import "@alaskaairux/design-tokens/dist/tokens/CSSCustomProperties.css"
import "@alaskaairux/auro-header";
import "@alaskaairux/auro-input";
import "@alaskaairux/auro-button";


export default class CurrencyConverter extends Component {
    constructor(props) {
        super(props);
        const { baseUrl, port, currenciesPath, conversionPath } = props;
        this.state = ({
            error: null,
            isLoaded: false,
            currencies: [],
            currenciesUrl: baseUrl + ":" + port + "/" + currenciesPath,
            conversionUrl: baseUrl + ":" + port + "/" + conversionPath,
            fromCurrency: undefined,
            toCurrency: undefined,
            amountToConvert: 1,
            convertedAmount: 1
        })

        this.handleFromCurrencyChange = this.handleFromCurrencyChange.bind(this);
        this.handleToCurrencyChange = this.handleToCurrencyChange.bind(this);
        this.handleAmountChange = this.handleAmountChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        const { currenciesUrl } = this.state;
        fetch(currenciesUrl)
            .then(res => res.json())
            .then(
                (result) => {
                    this.setState({
                        isLoaded: true,
                        currencies: result,
                        fromCurrency: result[0]?.currencyCode ?? undefined,
                        toCurrency: result[0]?.currencyCode ?? undefined
                    });
                },
                (error) => {
                    this.setState({
                        isLoaded: true,
                        error
                    });
                }
            )
    }

    clearConvertedAmount() {
        this.setState({
            convertedAmount: undefined
        })
    }

    handleFromCurrencyChange(event) {
        this.setState({ fromCurrency: event.target.value });
        this.clearConvertedAmount();
    }

    handleToCurrencyChange(event) {
        this.setState({ toCurrency: event.target.value });
        this.clearConvertedAmount();
    }

    handleAmountChange(event) {
        this.setState({ amountToConvert: event.target.value });
        this.clearConvertedAmount();
    }

    handleSubmit(event) {
        const { conversionUrl, fromCurrency, toCurrency, amountToConvert } = this.state;
        console.log("Amount to Convert: " + { amountToConvert });
        fetch(conversionUrl + "?from=" + fromCurrency + "&to=" + toCurrency + "&amount=" + amountToConvert)
            .then((res) => {
                if (res.ok) {
                    return res.json();
                } else {
                    throw new Error("Something went wrong. :(");
                }
            })
            .then(
                (result) => {
                    console.log(result);
                    this.setState({
                        convertedAmount: result,
                    });
                    console.log("Converted Amount: " + this.state.convertedAmount);
                },
                (error) => {
                    this.setState({
                        error
                    })
                }
            )
        event.preventDefault();
    }

    render() {
        const { currencies, error, isLoaded, amountToConvert, convertedAmount } = this.state;
        if (!isLoaded) {
            return <div>Loading...</div>;
        } else {
            return (
                <div className="container">
                    <auro-header>Currency Converter</auro-header>
                    <form>
                        <div className="row">
                            <input type="number" key="amountToConvert" value={amountToConvert} onChange={this.handleAmountChange}></input>
                        </div>
                        <div className="row">
                            <select value={this.state.fromCurrency} onChange={this.handleFromCurrencyChange}>
                                {currencies.map(c => (
                                    <option key={c.currencyCode} value={c.currencyCode}>{c.currencyName}</option>
                                ))}
                            </select>
                            <label> to </label>
                            <select value={this.state.toCurrency} onChange={this.handleToCurrencyChange}>
                                {currencies.map(c => (
                                    <option key={c.currencyCode} value={c.currencyCode}>{c.currencyName}</option>
                                ))}
                            </select>
                        </div>
                        <div className="container" >
                            <auro-button onClick={this.handleSubmit}>Convert</auro-button>
                        </div>
                    </form>
                    <div className="row">
                        <input disabled="true" value={convertedAmount}></input>
                    </div>
                </div>
            );
        }
    }
}