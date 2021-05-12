import React from 'react';
import './App.css';
import CurrencyConverter from './components/currency-converter/currency-converter';

function App() {
  return (
    <div>
      <CurrencyConverter baseUrl="http://localhost" port="5000" currenciesPath="api/currencyconverter/currencies" conversionPath="api/currencyconverter"/>
    </div>
  )
}

export default App;