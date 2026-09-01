import "./EditWorkoutPage.css";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";
import * as authService from "../../services/authService";
import * as workoutService from "../../services/workoutService";
import * as workoutExercisesService from "../../services/workoutExercisesService";
import * as exerciseService from "../../services/ExerciseService";
import ExerciseItem from "../../components/exercise-item/exerciseItem";

function EditWorkoutPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const [workout, setWorkout] = useState(null);
  const [name, setName] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [modal, setModal] = useState(null);
  const [selectedExercise, setSelectedExercise] = useState(null);
  const [exercises, setExercises] = useState([]);
  const [pendingExercises, setPendingExercises] = useState([]);
  const [exerciseForm, setExerciseForm] = useState({ exerciseId: "", sets: 3, reps: 10, restTimeSeconds: 60 });
  const [isAddingExercise, setIsAddingExercise] = useState(false);


  const openPopUp = async (modal, exercise) => {
    if (modal === "delete-workout") {
      setModal("delete-workout");
    } 
    
    if (modal === "delete-exercise") {
      setSelectedExercise(exercise);
      setModal("delete-exercise");
    } 
    
    if (modal === "add-exercise") {
      setError("");
      try {
        const availableExercises = await exerciseService.getAll();
        setExercises(availableExercises);
        setExerciseForm((currentForm) => ({
          ...currentForm,
          exerciseId: currentForm.exerciseId || String(availableExercises[0]?.id || ""),
        }));
        setModal("add-exercise");
      } catch (requestError) {
        setError(requestError.message);
      }
    };
  }

  const closePopUp = () => {
    setModal(null);
    setSelectedExercise(null);
  }

  ////////////////////

  const handleExerciseFormChange = (event) => {
    const { name: fieldName, value } = event.target;
    setExerciseForm((currentForm) => ({ ...currentForm, [fieldName]: value }));
  };


  const handleAddExercise = async (event) => {
    event.preventDefault();
    setError("");

    const exercise = exercises.find((item) => item.id === Number(exerciseForm.exerciseId));

    setPendingExercises((currentPending) => ([
    ...currentPending,
    {
      tempId: `temp-${Date.now()}`, // id temporário só pra usar como key e permitir deletar antes de salvar
      exerciseId: Number(exerciseForm.exerciseId),
      sets: Number(exerciseForm.sets),
      reps: Number(exerciseForm.reps),
      restTimeSeconds: Number(exerciseForm.restTimeSeconds),
      exerciseName: exercise?.name,
    },
    ]));

  closePopUp();

  };

  const handleDeleteExercise = async () => {
    if (!selectedExercise) {
      return;
    }

    try {
      await workoutExercisesService.remove(selectedExercise.id);
      setWorkout((currentWorkout) => ({
        ...currentWorkout,
        workoutExercises: currentWorkout.workoutExercises.filter(
          (currentExercise) => currentExercise.id !== selectedExercise.id
        ),
      }));
      
      closePopUp();
    } catch (requestError) {
      setError(requestError.message);
    }
  };
  
  const handleDeleteWorkout = async () => {
    try {
      await workoutService.remove(id);
      navigate("/home");
    } catch (requestError) {
      setError(requestError.message);
    }
  };
  
  //////////////////////////

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    try {
      const userId = authService.getUserId();
      await workoutService.update(id, name, userId);

      await Promise.all(
        pendingExercises.map((exercise) => 
          workoutExercisesService.create(
          Number(id),
          exercise.exerciseId,
          exercise.sets,
          exercise.reps,
          exercise.restTimeSeconds
          )
        )
      );

      navigate("/home");
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  useEffect(() => {
    const loadWorkout = async () => {
      try {
        const workout = await workoutService.getById(id);
        setWorkout(workout);
        setName(workout.name);
      } catch (requestError) {
        setError(requestError.message);
      } finally {
        setIsLoading(false);
      }
    };
    
    loadWorkout();
  }, [id]);

  ////////////////////

  return (
    <div className="edit-workout-page">
      <div className="edit-workout-container">
        <h1 className="Title-edit-workout">Editar treino</h1>
        {error && <p role="alert">{error}</p>}
        {isLoading ? (
          <p>Carregando treino...</p>
        ) : (
          <form className="edit-workout-form" onSubmit={handleSubmit}>
            <Input type="text" id="workoutName" placeholder="Nome do treino" value={name} onChange={(event) => setName(event.target.value)} />

            <div className="exercises-group">
              {workout?.workoutExercises?.map((exercise) => (
                <ExerciseItem
                  key={exercise.id}
                  exercise={exercise}
                  onDelete={(selected) => openPopUp("delete-exercise", selected)}
                />
              ))}
              {pendingExercises.map((exercise) => (
                <ExerciseItem
                  key={exercise.tempId}
                  exercise={{
                    id: exercise.tempId,
                    exerciseId: exercise.exerciseId,
                    sets: exercise.sets,
                    reps: exercise.reps,
                    restTimeSeconds: exercise.restTimeSeconds,
                    exerciseName: exercise.exerciseName,
                  }}
                  onDelete={() =>
                    setPendingExercises((currentPending) =>
                      currentPending.filter((item) => item.tempId !== exercise.tempId)
                    )
                  }
                />
              ))}
              {workout?.workoutExercises?.length === 0 && <p>Este treino ainda não possui exercícios.</p>}
            </div>

            <button type="button" className="add-exercise-btn" onClick={() => openPopUp("add-exercise")}>
              + Adicionar exercício
            </button>

            <Button type="submit">Salvar alterações</Button>
            <button type="button" className="delete-workout-btn" onClick={() => openPopUp("delete-workout")}>
              Excluir treino inteiro
            </button>
            
            {modal === "add-exercise" && (
              <div className="popup-add-overlay" onClick={closePopUp}>
                <div className="popup-add" onClick={(event) => event.stopPropagation()}>
                  <div className="popup-add-content">
                    <h2 id="add-exercise-title">Adicionar exercício</h2>
                    <label htmlFor="exerciseId">Exercício
                    <select id="exerciseId" name="exerciseId" value={exerciseForm.exerciseId} onChange={handleExerciseFormChange} required>
                      {exercises.map((exercise) => <option key={exercise.id} value={exercise.id}>{exercise.name}</option>)}
                    </select>
                    </label>
                    <div className="exercise-form-grid">
                      <label>Séries<input type="number" name="sets" min="1" max="1000" value={exerciseForm.sets} onChange={handleExerciseFormChange} required /></label>
                      <label>Repetições<input type="number" name="reps" min="1" max="1000" value={exerciseForm.reps} onChange={handleExerciseFormChange} required /></label>
                    </div>
                    <label>Descanso (segundos)<input type="number" name="restTimeSeconds" min="0" max="3600" value={exerciseForm.restTimeSeconds} onChange={handleExerciseFormChange} required /></label>
                    <div className="popup-add-buttons">
                      <button type="button" className="popup-add-button" onClick={closePopUp}>Cancelar</button>
                      <button type="button" className="popup-add-button popup-add-button--primary" onClick={handleAddExercise} disabled={isAddingExercise}>{isAddingExercise ? "Adicionando..." : "Adicionar"}</button>
                    </div>
                  </div>
                </div>
              </div>
            )}

            {modal === "delete-exercise" && selectedExercise && (
              <div className="popup-delete-overlay" onClick={closePopUp}>
                <div
                  className="popup-delete"
                  role="dialog"
                  aria-modal="true"
                  aria-labelledby="delete-workout-title"
                  onClick={(event) => event.stopPropagation()}
                >
                  <div className="popup-delete-content">
                    <h2 id="delete-workout-title">Deseja excluir esse exercicio?</h2>
                    <p>Se sim, clique em deletar.</p>
                    <div className="popup-delete-buttons">
                      <button type="button" className="popup-delete-button" onClick={closePopUp}>
                        Cancelar
                      </button>
                      <button type="button" className="popup-delete-button popup-delete-button--primary" onClick={handleDeleteExercise}>
                        Deletar
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            )}

            {modal === "delete-workout" && (
              <div className="popup-delete-overlay" onClick={closePopUp}>
                <div
                  className="popup-delete"
                  role="dialog"
                  aria-modal="true"
                  aria-labelledby="delete-workout-title"
                  onClick={(event) => event.stopPropagation()}
                >
                  <div className="popup-delete-content">
                    <h2 id="delete-workout-title">Deseja excluir esse treino?</h2>
                    <p>Não terá mais volta.</p>
                    <div className="popup-delete-buttons">
                      <button type="button" className="popup-delete-button" onClick={closePopUp}>
                        Cancelar
                      </button>
                      <button type="button" className="popup-delete-button popup-delete-button--primary" onClick={handleDeleteWorkout}>
                        Deletar
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            )}
          </form>
        )}
      </div>
    </div>
  );
}

export default EditWorkoutPage;