import "./exerciseItem.css";
import { Dumbbell, Trash } from "lucide-react";

function ExerciseItem({ exercise, onDelete }) {
  return (
        <div className="exercise-item">
            <div className="exercise-item__info">
                <span className="exercise-item__icon">
                    <Dumbbell size={14} aria-hidden="true" />
                </span>
                <p>{exercise.exerciseName || `Exercício ${exercise.exerciseId}`}</p>
            </div>

            <span className="exercise-item__meta">
                {exercise.sets}x{exercise.reps} · {exercise.restTimeSeconds}s
            </span>

            {onDelete && (
                <button
                    type="button"
                    className="remove-exercise-btn"
                    onClick={() => onDelete(exercise)}
                    aria-label={`Excluir ${exercise.exerciseName || "exercício"}`}
                >
                    <Trash size={18} aria-hidden="true" />
                </button>
            )}
        </div>
    );
}

export default ExerciseItem;