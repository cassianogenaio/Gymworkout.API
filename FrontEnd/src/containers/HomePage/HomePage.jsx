import "./HomePage.css";
// import { useNavigate } from "react-router-dom";
import { Plus, Pencil, Dumbbell, ChevronDown } from "lucide-react";
import Button from "../../components/Button/Button";
import Input from "../../components/Input/Input";
import React, { useEffect, useState } from 'react';
import * as authService from "../../services/authService";
import * as workoutService from "../../services/workoutService";

function HomePage() {
  const [isOpen, setIsOpen] = useState(false);
  const [error, setError] = useState("");
  const [name, setName] = useState("");
  const [workouts, setWorkouts] = useState([]);

  const openPopUp = (id_user) => {
    setIsOpen(true);
  };

  const closePopUp = () => {
      setIsOpen(false);
    }

  useEffect(() => {
    const loadWorkouts = async () => {
      try {
        setWorkouts(await workoutService.getAll());
      } catch (requestError) {
        setError(requestError.message);
      }
    };
    loadWorkouts();
  }, []);

  const handleCreateWorkout = async (e) => {
    e.preventDefault();
    setError("");
    
    try {
      const userId = authService.getUserId();
      if (!userId) {
        throw new Error("Sessão inválida. Faça login novamente.");
      }

      const createdWorkout = await workoutService.create(name, userId);
      setWorkouts((currentWorkouts) => [...currentWorkouts, createdWorkout]);
      setName("");
      closePopUp();

    } catch (requestError) {
      setError(requestError.message);
    }
  };

  return (
    <div className="home-page">
      <div className="home-page__container">
        <div className="home-page__topbar">
          <div>
            <h1>Meus treinos</h1>
            <p>{workouts.length} {workouts.length === 1 ? "treino criado" : "treinos criados"}</p>
          </div>
        </div>

        <div className="home-page__workouts-list">
          {workouts.map((workout) => (
          <article className="workout-card workout-card--expanded" key={workout.id}>
            <div className="workout-card__header">
              <div>
                <p className="workout-card__title">{workout.name}</p>
                <span className="workout-card__subtitle">
                  {workout.Exercises?.length }
                </span>
              </div>
              <div className="workout-card__actions">
                <button className="workout-card__edit-button">
                  <Pencil size={14}/> Editar
                </button>
                <button className="workout-card__toggle-button">
                  <ChevronDown size={18} />
                </button>
              </div>
            </div>

            {/* Expansão do card  */}
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
          ))}

          <article className="workout-card workout-card--new">
            <button className="workout-card__new-workout" onClick={() => { openPopUp() }}>
              <Plus size={16} /> Criar treino
            </button>
          </article>

          {isOpen && (
            <div className="popup-overlay" onClick={closePopUp}>
              <div
                className="popup"
                role="dialog"
                aria-modal="true"
                aria-labelledby="create-workout-title"
                onClick={(event) => event.stopPropagation()}
              >
                <div className="popup-content">
                  <h2 id="create-workout-title">Criar treino</h2>
                  <p>Deseja criar um novo treino?</p>
                  <Input className="input-popup" placeholder="Nome do treino" value={name} onChange={(e) => setName(e.target.value)} />
                  <div className="popup-buttons">
                    <button className="popup-button" onClick={closePopUp}>
                      Cancelar
                    </button>
                    <button className="popup-button popup-button--primary" onClick={handleCreateWorkout}>
                      Criar
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default HomePage;