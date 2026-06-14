import "./LoginPage.css";
import { Link } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";

function LoginPage() {
  return (
    <div className="login-page">
      <div className="login-container">
        <form action="">
          <h1 className="Title-login">Login</h1>
          <div className="input-group">
            <div className="input-box">
              <Input type="email" id="email" placeholder="Email" />
            </div>
            <div className="input-box">
              <Input type="password" id="password" placeholder="Password" />
            </div>
          </div>
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
