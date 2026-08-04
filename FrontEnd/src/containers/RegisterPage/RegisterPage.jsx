import React, { useState } from "react";
import "./RegisterPage.css";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";
import * as authService from "../../services/authService";

function RegisterPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    setError(null);

    if (!name || !email || !password || !confirmPassword) {
      setError("Preencha todos os campos.");
      return;
    }

    if (password !== confirmPassword) {
      setError("As senhas não coincidem.");
      return;
    }

    try {
      const token = await authService.register(name, email, password);
      console.log("Registration successful. Token:", token);

      if (token) {
        console.log("Storing token in localStorage:", token);
        localStorage.setItem("token", token);
      }

      navigate("/home");
    } catch (err) {
      console.error("Register error:", err);
      setError(err.message || "Registration failed. Please try again.");
    }
  };

  return (
    <div className="register-page">
      <div className="register-container">
        <form onSubmit={handleRegister}>
          <h1 className="Title-register">Register</h1>
          <div className="input-group">
            <div className="input-box">
              <Input
                type="text"
                id="name"
                placeholder="Full Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </div>
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
            <div className="input-box">
              <Input
                type="password"
                id="confirm-password"
                placeholder="Confirm Password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
              />
            </div>
          </div>
          {error && <p className="form-error">{error}</p>}
          <Button type="submit" className="btn-register">
            Register
          </Button>
          <div className="login-link">
            <span>Already have an account?</span>
            <Link to="/login">Login</Link>
          </div>
        </form>
      </div>
    </div>
  );
}

export default RegisterPage;