import "./ProfilePage.css";
import { User, Mail, Lock, Pencil } from "lucide-react";
import { useNavigate } from "react-router-dom";
import profileImage from "../../assets/img/Profile_default.jpeg";

function ProfilePage() {
  const navigate = useNavigate();

  return (
    <div className="profile-page">
      <button className="profile-back" onClick={() => navigate("/login")}>
        &lt; Back
      </button>
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
              <label>Cassiano de Castro Genaio</label>
              <p>cassianosite@gmail.com</p>
            </div>
          </div>
          <div className="profile-card profile-card--personal-info">
            <div className="profile-card__info-header">
              <label>Informações pessoais</label>
              <button className="profile-card__edit-button"><Pencil size={13} color="rgb(107, 107, 107)"/> Editar</button>
            </div>
            <div className="profile-card__info-block">
              <div className="profile-card__info-row">
                <User size={15} color="rgb(107, 107, 107)" />
                <p>Nome</p>
              </div>
              <p className="profile-card__value">Cassiano de Castro Genaio</p>
            </div>
            <div className="profile-card__info-block profile-card__info-block--email">
              <div className="profile-card__info-row">
                <Mail size={15} color="rgb(107, 107, 107)" />
                <p>Email</p>
              </div>
              <p className="profile-card__value">cassianosite@gmail.com</p>
            </div>
          </div>
          <div className="profile-card profile-card--password">
            <div className="profile-card__info-header">
              <label>Senha</label>
              <button className="profile-card__edit-button"><Pencil size={13} color="rgb(107, 107, 107)"/>Alterar senha </button>
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
  );
}

export default ProfilePage;
