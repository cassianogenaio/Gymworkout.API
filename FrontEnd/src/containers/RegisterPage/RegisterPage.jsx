import "./RegisterPage.css";
import { Link } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";

function RegisterPage() {
  return (
    <div className="register-page">
      <div className="register-container">
        <form action="">
          <h1 className="Title-register">Register</h1>
          <div className="input-group">
            <div className="input-box">
              <Input type="text" id="name" placeholder="Full Name" />
            </div>
            <div className="input-box">
              <Input type="email" id="email" placeholder="Email" />
            </div>
            <div className="input-box">
              <Input type="password" id="password" placeholder="Password" />
            </div>
            <div className="input-box">
              <Input
                type="password"
                id="confirm-password"
                placeholder="Confirm Password"
              />
            </div>
          </div>
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
