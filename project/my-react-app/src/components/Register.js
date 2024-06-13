import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axios from 'axios';
import './Login.css'; // Ensure this import is present

const Register = ({ setUsername }) => {
  const [username, setUsernameInput] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:5289/api/auth/register', { username, password });
      if (response.data) {
        setUsername(username);
        localStorage.setItem('loggedInUser', username);
        navigate('/account');
      }
    } catch (error) {
      console.error('Error registering user:', error);
      alert('Registration failed. Please try again.');
    }
  };

  return (
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleRegister}>
        <h2>Register</h2>
        <input 
          type="text" 
          value={username} 
          onChange={(e) => setUsernameInput(e.target.value)} 
          placeholder="Username" 
          required 
        />
        <input 
          type="password" 
          value={password} 
          onChange={(e) => setPassword(e.target.value)} 
          placeholder="Password" 
          required 
        />
        <button type="submit">Register</button>
        <p>
          Already have an account? <Link to="/login">Login here</Link>
        </p>
      </form>
    </div>
  );
};

export default Register;
