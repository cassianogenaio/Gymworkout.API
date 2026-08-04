import React, { useState } from "react";
import "./LoginPage.css";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";
import * as authService from "../../services/authService";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      const token = await authService.login(email, password);
      if (token) { 
        localStorage.setItem("token", token);
        navigate("/home");
      }
    } catch (err) {
      if (password === "") {
        setError("Password is required.");
      }
      if (email === "") {
        setError("Email is required.");
      }

      setError("Email or password is incorrect.");
    }
  }
   
  return (
    <div className="login-page">
      <div className="login-container">
        <form onSubmit={handleLogin}>
          <h1 className="Title-login">Login</h1>
          <div className="input-group">
            <div className="input-box">
              <Input 
                type="email" 
                id="email" 
                placeholder="Email" 
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="input-box">
              <Input 
                type="password" 
                id="password" 
                placeholder="Password" 
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>
          {error && <p className="form-error">{error}</p>}
          <Button type="submit" className="btn-login">Login</Button>
          <div className="register-link">
            <span>Don't have an account?</span>
            <Link to="/register">Register</Link>
          </div>
        </form>
      </div>
    </div>
  );
}

export default LoginPage;
