import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Account.css';

const Account = ({ username }) => {
  const [account, setAccount] = useState({ username, checking: 1000, savings: 2000 });
  const [checkingAmount, setCheckingAmount] = useState('');
  const [savingsAmount, setSavingsAmount] = useState('');
  const [transactions, setTransactions] = useState([]);

  useEffect(() => {
    const fetchAccountData = () => {
      const storedData = localStorage.getItem(`account_${account.username}`);
      if (storedData) {
        setAccount(JSON.parse(storedData));
      }
      const storedTransactions = localStorage.getItem(`transactions_${account.username}`);
      if (storedTransactions) {
        setTransactions(JSON.parse(storedTransactions));
      }
    };

    fetchAccountData();
  }, [account.username]);

  const handleDepositChecking = () => {
    const newChecking = account.checking + parseFloat(checkingAmount);
    setAccount(prevState => ({ ...prevState, checking: newChecking }));
    setTransactions([...transactions, `User deposited $${checkingAmount} into checking`]);
    setCheckingAmount('');
  };

  const handleWithdrawChecking = () => {
    const newChecking = account.checking - parseFloat(checkingAmount);
    setAccount(prevState => ({ ...prevState, checking: newChecking }));
    setTransactions([...transactions, `User withdrew $${checkingAmount} from checking`]);
    setCheckingAmount('');
  };

  const handleDepositSavings = () => {
    const newSavings = account.savings + parseFloat(savingsAmount);
    setAccount(prevState => ({ ...prevState, savings: newSavings }));
    setTransactions([...transactions, `User deposited $${savingsAmount} into savings`]);
    setSavingsAmount('');
  };

  const handleWithdrawSavings = () => {
    const newSavings = account.savings - parseFloat(savingsAmount);
    setAccount(prevState => ({ ...prevState, savings: newSavings }));
    setTransactions([...transactions, `User withdrew $${savingsAmount} from savings`]);
    setSavingsAmount('');
  };

  const saveCheckingToLocalStorage = () => {
    localStorage.setItem(`account_${account.username}`, JSON.stringify(account));
    localStorage.setItem(`transactions_${account.username}`, JSON.stringify(transactions));
  };

  const saveSavingsToLocalStorage = () => {
    localStorage.setItem(`account_${account.username}`, JSON.stringify(account));
    localStorage.setItem(`transactions_${account.username}`, JSON.stringify(transactions));
  };

  return (
    <div className="account-container">
      <h1>{account.username}'s Account</h1>
      <div className="balance-box">
        <h2>Checking: ${account.checking.toFixed(2)}</h2>
        <div className="transaction">
          <input 
            type="number" 
            value={checkingAmount} 
            onChange={(e) => setCheckingAmount(e.target.value)} 
            placeholder="Amount" 
          />
          <button onClick={handleDepositChecking}>Deposit</button>
          <button onClick={handleWithdrawChecking}>Withdraw</button>
        </div>
        <button onClick={saveCheckingToLocalStorage}>SAVE DATA</button>
      </div>
      <div className="balance-box">
        <h2>Savings: ${account.savings.toFixed(2)}</h2>
        <div className="transaction">
          <input 
            type="number" 
            value={savingsAmount} 
            onChange={(e) => setSavingsAmount(e.target.value)} 
            placeholder="Amount" 
          />
          <button onClick={handleDepositSavings}>Deposit</button>
          <button onClick={handleWithdrawSavings}>Withdraw</button>
        </div>
        <button onClick={saveSavingsToLocalStorage}>SAVE DATA</button>
      </div>
      <Link to="/transactions">View Transactions</Link>
    </div>
  );
};

export default Account;
