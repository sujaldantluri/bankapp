import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import './Account.css'; // Reusing the same styles for consistency

const Transactions = ({ username }) => {
  const [transactions, setTransactions] = useState([]);

  useEffect(() => {
    const storedTransactions = localStorage.getItem(`transactions_${username}`);
    if (storedTransactions) {
      setTransactions(JSON.parse(storedTransactions));
    }
  }, [username]);

  return (
    <div className="account-container">
      <h1>{username}'s Transactions</h1>
      <div className="transaction-list">
        {transactions.length > 0 ? (
          transactions.map((transaction, index) => (
            <div key={index} className="transaction-item">
              {transaction}
            </div>
          ))
        ) : (
          <p>No transactions found.</p>
        )}
      </div>
      <Link to="/account">Back to Account</Link>
    </div>
  );
};

export default Transactions;
