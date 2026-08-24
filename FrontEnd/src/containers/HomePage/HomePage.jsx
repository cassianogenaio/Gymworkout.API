import "./HomePage.css";
import { useNavigate } from "react-router-dom";
import { Plus, Pencil, Dumbbell, ChevronDown } from "lucide-react";
import Button from "../../components/Button/Button";
import Input from "../../components/Input/Input";
import React, { useEffect, useState } from 'react';
import * as authService from "../../services/authService";
import * as workoutService from "../../services/workoutService";
import * as workoutExercisesService from "../../services/workoutExercisesService";

function HomePage() {
  const [isOpen, setIsOpen] = useState(false);
  const [error, setError] = useState("");
  const [name, setName] = useState("");
  const [workouts, setWorkouts] = useState([]);
  const navigate = useNavigate();

  const openPopUp = (id_user) => {
    setIsOpen(true);
  };

  const closePopUp = () => {
      setIsOpen(false);
  }

  const loadWorkouts = async () => {
    try {
      setWorkouts(await workoutService.getAll());
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  useEffect(() => {
    workoutService.getAll()
      .then((loadedWorkouts) => setWorkouts(loadedWorkouts))
      .catch((requestError) => setError(requestError.message));
  }, []);

  const handleCreateWorkout = async (e) => {
    e.preventDefault();
    setError("");
    
    try {
      const userId = authService.getUserId();
      if (!userId) {
        throw new Error("Sessão inválida. Faça login novamente.");
      }

      await workoutService.create(name, userId);
      await loadWorkouts();
      setName("");
      closePopUp();

    } catch (requestError) {
      setError(requestError.message);
    }
  };

  const PageEdit = (id_workout) => {
    navigate(`/edit-workout/${id_workout}`);
  }

  return (
    <div className="home-page">
      <div className="home-page__container">
        <div className="home-page__topbar">
          <div>
            <h1>Meus treinos</h1>
            <p>{workouts.length} {workouts.length === 1 ? "treino criado" : "treinos criados"}</p>
            {error && <p role="alert">{error}</p>}
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
                <button className="workout-card__edit-button" onClick={() => { PageEdit(workout.id) }}>
                  <Pencil size={14}/> Editar
                </button>
                <button className="workout-card__toggle-button">
                  <ChevronDown size={18} className="ChevroDown Up"/>
                </button>
              </div>
            </div>

            {/* Expansão do card  */}
            <div className="workout-card__exercises">
              {workout.workoutExercises?.map((exercise) => (
                <div className="exercise-item" key={exercise.id}>
                  <div className="exercise-item__info">
                    <span className="exercise-item__icon">
                      <Dumbbell size={14} />
                    </span>

                    <p>{exercise.exerciseName}</p>
                  </div>

                  <span className="exercise-item__meta">{exercise.sets}x{exercise.reps} · {exercise.restTimeSeconds}s</span>
                </div>
              ))}
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