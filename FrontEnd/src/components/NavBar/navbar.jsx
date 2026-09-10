import "./navbar.css";
import Button from "../../components/Button/Button";
import { UserRound, LogOut } from "lucide-react";
import { Navigate, useLocation, useNavigate } from "react-router-dom";

function NavBar() {
    const location = useLocation();
    const navigate = useNavigate();
    
    function handleLogout() {
        localStorage.removeItem("token");
        navigate("/login", { replace: true });
    }

  return (
    <nav className="container-nav">
      <div className="navbar navbar-logo">
        <img src="/logo.png" alt="Logo" className="navbar-logo-image" />
      </div>
      <div className="navbar navbar-pages">
        <Button
          className={`button-nav home-btn ${
            location.pathname === "/home" ? "active" : ""
          }`}
          onClick={() => navigate("/home")}
        >
          Home
        </Button>
      </div>
      <div className="navbar navbar-actions">
        <Button
          class="button-nav profile-btn"
          onClick={() => navigate("/profile")}
        >
          <UserRound size={18} />
        </Button>
        <Button class="button-nav logout-btn" onClick={handleLogout}>
          <LogOut size={18} />
        </Button>
      </div>
    </nav>
  );
}

export default NavBar;
