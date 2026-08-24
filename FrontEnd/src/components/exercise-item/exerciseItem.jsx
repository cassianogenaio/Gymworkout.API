import React from 'react';


function ExerciseItem({ exercise, onDelete }) {
  return (
    <div className="exercise-item">
      <span>
        {exercise.exerciseName || `Exercício ${exercise.exerciseId}`}
      </span>

      <span>
        {exercise.sets} séries x {exercise.reps} repetições
      </span>

      <span>
        Descanso: {exercise.restTimeSeconds}s
      </span>

      {onDelete && (
        <button
          type="button"
          className="remove-exercise-btn"
          onClick={() => onDelete(exercise)}
        >
          Excluir exercício
        </button>
      )}
    </div>
  );
}