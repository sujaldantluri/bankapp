import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import Account from './components/Account';
import Transactions from './components/Transactions';

const App = () => {
  const [username, setUsername] = useState(localStorage.getItem('loggedInUser') || '');

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login setUsername={setUsername} />} />
        <Route path="/register" element={<Register setUsername={setUsername} />} />
        <Route path="/account" element={<Account username={username} />} />
        <Route path="/transactions" element={<Transactions username={username} />} />
      </Routes>
    </Router>
  );
};

export default App;
