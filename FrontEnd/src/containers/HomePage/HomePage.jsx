import "./HomePage.css";
import { useNavigate } from "react-router-dom";
import { Plus, Pencil, Dumbbell, ChevronDown } from "lucide-react";
import Button from "../../components/Button/Button";

function HomePage() {
  const navigate = useNavigate();

  return (
    <div className="home-page">
      <div className="home-page__container">
        <div className="home-page__topbar">
          <div>
            <h1>Meus treinos</h1>
            <p>2 treinos criados</p>
          </div>
        </div>

        <div className="home-page__workouts-list">
          <article className="workout-card workout-card--expanded">
            <div className="workout-card__header">
              <div>
                <p className="workout-card__title">Pernas e glúteos</p>
                <span className="workout-card__subtitle">3 exercícios</span>
              </div>
              <div className="workout-card__actions">
                <button className="workout-card__edit-button">
                  <Pencil size={14} /> Editar
                </button>
                <button className="workout-card__toggle-button">
                  <ChevronDown size={18} />
                </button>
              </div>
            </div>

            <div className="workout-card__exercises">
              <div className="exercise-item">
                <div className="exercise-item__info">
                  <span className="exercise-item__icon">
                    <Dumbbell size={14} />
                  </span>
                  <p>Agachamento livre</p>
                </div>
                <span className="exercise-item__meta">4x10 · 90s</span>
              </div>
              <div className="exercise-item">
                <div className="exercise-item__info">
                  <span className="exercise-item__icon">
                    <Dumbbell size={14} />
                  </span>
                  <p>Leg press 45</p>
                </div>
                <span className="exercise-item__meta">3x12 · 60s</span>
              </div>
              <div className="exercise-item">
                <div className="exercise-item__info">
                  <span className="exercise-item__icon">
                    <Dumbbell size={14} />
                  </span>
                  <p>Cadeira extensora</p>
                </div>
                <span className="exercise-item__meta">3x15 · 45s</span>
              </div>
            </div>
          </article>

          <article className="workout-card">
            <div className="workout-card__header">
              <div>
                <p className="workout-card__title">
                  Push (peito, ombro, tríceps)
                </p>
                <span className="workout-card__subtitle">2 exercícios</span>
              </div>
              <div className="workout-card__actions">
                <button className="workout-card__edit-button">
                  <Pencil size={14} /> Editar
                </button>
                <button className="workout-card__toggle-button">
                  <ChevronDown size={18} />
                </button>
              </div>
            </div>
          </article>

          <article className="workout-card workout-card--new">
            <button className="workout-card__new-workout">
              <Plus size={16} to="/create-workout" /> Criar treino
            </button>
          </article>
        </div>
      </div>
    </div>
  );
}

export default HomePage;
