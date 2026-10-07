import { apiRequest } from "./apiClient";

async function request(path, options = {}) {
  return apiRequest(path, options);
}

export function create(workoutId, exerciseId, sets, reps, restTimeSeconds) {
  return request("/WorkoutExercises", {
    method: "POST",
    body: JSON.stringify({
      workoutId,
      exerciseId,
      sets,
      reps,
      restTimeSeconds,
    }),
  });
}

export function remove(id) {
  return request(`/WorkoutExercises/${id}`, { method: "DELETE" });
}
