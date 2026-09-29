import "./ProfilePage.css";
import { useState, useEffect } from "react";
import { User, Mail, Lock, Pencil } from "lucide-react";
import { useNavigate } from "react-router-dom";
import profileImage from "../../assets/img/Profile_default.jpeg";
import NavBar from "../../components/NavBar/navbar";
import * as authService from "../../services/authService";
import userService from "../../services/userService";
import Modal from "../../components/Modal/modal";
import Input from "../../components/Input/Input";

function ProfilePage() {
  const navigate = useNavigate();
  const [modal, setModal] = useState(null);
  const [error, setError] = useState("");
  const [errors, setErrors] = useState({});
  const [user, setUser] = useState(null);
  const [form, setForm] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  const openPopUp = async (modal) => {
    if (modal === "change-password") {
      setModal("change-password");
    }
    if (modal == "edit-info") {
      setModal("edit-info");
    }
  };

  const closePopUp = () => {
    setModal(null);
  };

  const loadUser = async () => {
    try {
      const userId = authService.getUserId();

      if (!userId) return;

      const userData = await userService.getCurrentUser(userId);
      setUser(userData);
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  const validate = () => {
    const nextErrors = {};

    if (!form.currentPassword.trim()) {
      nextErrors.currentPassword = "Digite sua senha atual.";
    }

    if (!form.newPassword.trim()) {
      nextErrors.newPassword = "Digite sua nova senha.";
    } else if (form.newPassword.length < 6) {
      nextErrors.newPassword = "A nova senha deve ter pelo menos 6 caracteres.";
    }

    if (!form.confirmPassword.trim()) {
      nextErrors.confirmPassword = "Confirme sua nova senha.";
    } else if (form.newPassword !== form.confirmPassword) {
      nextErrors.confirmPassword = "As senhas não conferem.";
    }

    if (
      form.currentPassword &&
      form.newPassword &&
      form.currentPassword === form.newPassword
    ) {
      nextErrors.newPassword = "A nova senha deve ser diferente da atual.";
    }

    return nextErrors;
  };

  const handleChangePassword = async (event) => {
    event.preventDefault();
    setError("");

    const nextErrors = validate();
    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors);
      return;
    }

    setErrors({});

    try {
      await userService.changePassword(form.currentPassword, form.newPassword);
      setForm({ currentPassword: "", newPassword: "", confirmPassword: "" });
      setError("");
      setErrors({});
      closePopUp();
    } catch (requestError) {
      const message =
        requestError.message || "Não foi possível alterar a senha.";
      setError(message);
      setErrors({
        currentPassword: message,
      });
    }
  };

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
                <button
                  className="profile-card__edit-button"
                  onClick={() => openPopUp("edit-info")}
                >
                  <Pencil size={13} /> Editar
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
                  <p>E-mail</p>
                </div>
                <p className="profile-card__value">{user.email}</p>
              </div>
            </div>
            <div className="profile-card profile-card--password">
              <div className="profile-card__info-header">
                <label>Senha</label>
                <button
                  className="profile-card__edit-button"
                  onClick={() => openPopUp("change-password")}
                >
                  <Pencil size={13} />
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

            <Modal isOpen={modal === "edit-info"} onClose={closePopUp}>
              <div className="popup-editinfo-content">
                <div className="header-popup">
                  <h2>Editar suas informações</h2>
                </div>
                <div className="popup-form-change">
                  <Input type="text" id="name" placeholder="Seu nome" />
                  <Input type="email" id="email" placeholder="Seu e-mail" />
                </div>
                <div className="popup-buttons">
                  <button className="popup-cancel-button" onClick={closePopUp}>
                    Cancelar
                  </button>
                  <button className="popup-save-button">Salvar</button>
                </div>
              </div>
            </Modal>
            <Modal isOpen={modal === "change-password"} onClose={closePopUp}>
              <div className="popup-changepass-content">
                <div className="header-popup">
                  <h2>Alterar sua senha</h2>
                </div>
                <div className="popup-form-change">
                  <Input
                    type="password"
                    id="current-password"
                    placeholder="Sua senha atual"
                    value={form.currentPassword}
                    onChange={(e) =>
                      setForm({ ...form, currentPassword: e.target.value })
                    }
                  />
                  <Input
                    type="password"
                    id="new-password"
                    placeholder="Digite sua nova senha"
                    value={form.newPassword}
                    onChange={(e) =>
                      setForm({ ...form, newPassword: e.target.value })
                    }
                  />

                  <Input
                    type="password"
                    id="confirm-password"
                    placeholder="Confirme sua senha"
                    value={form.confirmPassword}
                    onChange={(e) =>
                      setForm({ ...form, confirmPassword: e.target.value })
                    }
                  />
                  {error && (
                    <small
                      style={{ color: "red", display: "block", marginTop: 6 }}
                    >
                      {error}
                    </small>
                  )}
                  {errors.currentPassword && (
                    <small
                      style={{ color: "red", display: "block", marginTop: 6 }}
                    >
                      {errors.currentPassword}
                    </small>
                  )}
                  {errors.newPassword && (
                    <small
                      style={{ color: "red", display: "block", marginTop: 6 }}
                    >
                      {errors.newPassword}
                    </small>
                  )}
                  {errors.confirmPassword && (
                    <small
                      style={{ color: "red", display: "block", marginTop: 6 }}
                    >
                      {errors.confirmPassword}
                    </small>
                  )}
                </div>
                <div className="popup-buttons">
                  <button className="popup-cancel-button" onClick={closePopUp}>
                    Cancelar
                  </button>
                  <button
                    className="popup-change-button"
                    onClick={handleChangePassword}
                  >
                    Alterar
                  </button>
                </div>
              </div>
            </Modal>
          </section>
        </div>
      </div>
    </>
  );
}

export default ProfilePage;
