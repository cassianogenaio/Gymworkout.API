import "./ProfilePage.css";
import { useState, useEffect } from "react";
import { User, Mail, Lock, Pencil } from "lucide-react";
import { useNavigate } from "react-router-dom";
import profileImage from "../../assets/img/Profile_default.jpeg";
import NavBar from "../../components/NavBar/navbar";
import * as authService from "../../services/authService";
import userService from "../../services/userService";
// import * as userService from "../../services/userService"

function ProfilePage() {
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [user, setUser] = useState(null);

  const loadUser = async () => {
    try {
      const userId = authService.getUserId();

      console.log(Number(userId));
      if (!userId) return;

      const userData = await userService.getCurrentUser(userId);
      console.log(userData);
      setUser(userData);
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  // console.log(user)

  useEffect(() => {
    loadUser();
  }, []);

  if (!user) {
    return <p>Carregando perfil...</p>;
  }

  return (
    <>
      <NavBar />
      <div className="profile-page">
        <div className="profile-container">
          <section className="profile-header">
            <h2>Perfil</h2>
            <p>Gerencie suas informações pessoais e sua senha.</p>
          </section>
          <section className="profile-sections">
            <div className="profile-card--header profile-card">
              <div className="profile-card__image">
                <img src={profileImage} alt="Foto de perfil" />
              </div>
              <div className="profile-card__info">
                <label>{user.name}</label>
                <p>{user.email}</p>
              </div>
            </div>
            <div className="profile-card profile-card--personal-info">
              <div className="profile-card__info-header">
                <label>Informações pessoais</label>
                <button className="profile-card__edit-button">
                  <Pencil size={13} color="rgb(107, 107, 107)" /> Editar
                </button>
              </div>
              <div className="profile-card__info-block">
                <div className="profile-card__info-row">
                  <User size={15} color="rgb(107, 107, 107)" />
                  <p>Nome</p>
                </div>
                <p className="profile-card__value">{user.name}</p>
              </div>
              <div className="profile-card__info-block profile-card__info-block--email">
                <div className="profile-card__info-row">
                  <Mail size={15} color="rgb(107, 107, 107)" />
                  <p>Email</p>
                </div>
                <p className="profile-card__value">{user.email}</p>
              </div>
            </div>
            <div className="profile-card profile-card--password">
              <div className="profile-card__info-header">
                <label>Senha</label>
                <button className="profile-card__edit-button">
                  <Pencil size={13} color="rgb(107, 107, 107)" />
                  Alterar senha{" "}
                </button>
              </div>
              <div className="profile-card__info-block">
                <div className="profile-card__info-row">
                  <Lock size={15} color="rgb(107, 107, 107)" />
                  <p>********</p>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    </>
  );
}

export default ProfilePage;
