import "./HomePage.css";
import { useNavigate } from "react-router-dom";
import { Plus, Pencil, Dumbbell, ChevronDown } from "lucide-react";
import Input from "../../components/Input/Input";
import NavBar from "../../components/NavBar/navbar";
import ExerciseItem from "../../components/Exercise-item/exerciseItem";
import React, { useEffect, useState } from "react";
import * as authService from "../../services/authService";
import * as workoutService from "../../services/workoutService";
import * as workoutExercisesService from "../../services/workoutExercisesService";

function HomePage() {
  const [isOpen, setIsOpen] = useState(false);
  const [error, setError] = useState("");
  const [name, setName] = useState("");
  const [workouts, setWorkouts] = useState([]);
  const [openWorkouts, setOpenWorkouts] = useState([]);
  const navigate = useNavigate();

  const openPopUp = (id_user) => {
    setIsOpen(true);
  };

  const closePopUp = () => {
    setIsOpen(false);
  };

  const loadWorkouts = async () => {
    try {
      setWorkouts(await workoutService.getAll());
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  const handleOpenExercise = (workoutId) => {
    setOpenWorkouts((current) =>
      current.includes(workoutId)
        ? current.filter((id) => id !== workoutId)
        : [...current, workoutId],
    );
  };

  useEffect(() => {
    workoutService
      .getAll()
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
  };

  return (
    <>
      <NavBar />
      <div className="home-page">
        <div className="home-page__container">
          <div className="home-page__topbar">
            <div>
              <h1>Meus treinos</h1>
              <p>
                {workouts.length}{" "}
                {workouts.length === 1 ? "treino criado" : "treinos criados"}
              </p>
              {error && <p role="alert">{error}</p>}
            </div>
          </div>

          <div className="home-page__workouts-list">
            {workouts.map((workout) => (
              <article
                className="workout-card workout-card--expanded"
                key={workout.id}
              >
                <div className="workout-card__header">
                  <div>
                    <p className="workout-card__title">{workout.name}</p>
                    <span className="workout-card__subtitle">
                      {workout.Exercises?.length}
                    </span>
                  </div>
                  <div className="workout-card__actions">
                    <button
                      className="workout-card__edit-button"
                      onClick={() => {
                        PageEdit(workout.id);
                      }}
                    >
                      <Pencil size={14} /> Editar
                    </button>
                    <button
                      type="button"
                      className="workout-card__toggle-button"
                      onClick={() => handleOpenExercise(workout.id)}
                    >
                      <ChevronDown
                        size={18}
                        className={
                          openWorkouts.includes(workout.id)
                            ? "ChevroDown--enable"
                            : "ChevroDown"
                        }
                      />
                    </button>
                  </div>
                </div>

                {/* Expansão do card  */}
                <div
                  className={
                    openWorkouts.includes(workout.id)
                      ? "workout-card__exercises workout-card__exercises--enable"
                      : "workout-card__exercises"
                  }
                >
                  {workout?.workoutExercises && workout.workoutExercises.length > 0 ? (
                    workout.workoutExercises.map((exercise) => (
                      <ExerciseItem key={exercise.id} exercise={exercise} />
                    ))
                  ) : (
                    <div className="no-exercises">
                      <p>Nenhum exercício cadastrado para este treino. 💪</p>
                    </div>
                  )}
                </div>
              </article>
            ))}

            <article className="workout-card workout-card--new">
              <button
                className="workout-card__new-workout"
                onClick={() => {
                  openPopUp();
                }}
              >
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
                    <Input
                      className="input-popup"
                      placeholder="Nome do treino"
                      value={name}
                      onChange={(e) => setName(e.target.value)}
                    />
                    <div className="popup-buttons">
                      <button className="popup-button" onClick={closePopUp}>
                        Cancelar
                      </button>
                      <button
                        className="popup-button popup-button--primary"
                        onClick={handleCreateWorkout}
                      >
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
    </>
  );
}

export default HomePage;
