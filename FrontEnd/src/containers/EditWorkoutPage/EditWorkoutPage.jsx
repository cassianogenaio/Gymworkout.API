import React from "react";
import "./EditWorkoutPage.css";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../components/Input/Input";
import Button from "../../components/Button/Button";

function EditWorkoutPage() {
  const navigate = useNavigate();

  return (
    <div className="edit-workout-page">
      <div className="edit-workout-container">
        <h1 className="Title-edit-workout">Edit Workout</h1>
        <form className="edit-workout-form">
          <Input type="text" id="workoutName" placeholder="Workout Name" />

          <div className="exercises-group">
            <div className="exercise-item">
              <select className="exercise-select">
                <option value="">Selecione um exercício</option>
                <option value="1">Agachamento livre</option>
                <option value="2">Leg press 45</option>
                <option value="3">Cadeira extensora</option>
              </select>
              <Input type="number" placeholder="Sets" />
              <Input type="number" placeholder="Reps" />
              <button type="button" className="remove-exercise-btn">
                Remover
              </button>
            </div>
          </div>

          <button type="button" className="add-exercise-btn">
            + Adicionar exercício
          </button>

          <Button type="submit">Edit Workout</Button>
        </form>
      </div>
    </div>
  );
}

export default EditWorkoutPage;