import "./navbar.css";
import Button from "../../components/Button/Button";
import { UserRound, LogOut } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import * as authService from "../../services/authService";

function NavBar() {
  const location = useLocation();
  const navigate = useNavigate();

  async function handleLogout() {
    try {
      await authService.logout();
    } finally {
      navigate("/login", { replace: true });
    }
  }

  return (
    <nav className="container-nav">
      <div className="navbar navbar-logo">
        <img src="/logo.png" alt="Logotipo" className="navbar-logo-image" />
      </div>
      <div className="navbar navbar-pages">
        <Button
          className={`button-nav home-btn ${
            location.pathname === "/home" ? "active" : ""
          }`}
          onClick={() => navigate("/home")}
        >
          Início
        </Button>
      </div>
      <div className="navbar navbar-actions">
        <Button
          className="button-nav profile-btn"
          onClick={() => navigate("/profile")}
          aria-label="Perfil"
        >
          <UserRound size={18} />
        </Button>
        <Button
          className="button-nav logout-btn"
          onClick={handleLogout}
          aria-label="Sair"
        >
          <LogOut size={18} />
        </Button>
      </div>
    </nav>
  );
}

export default NavBar;
