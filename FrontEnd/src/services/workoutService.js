import { apiRequest } from "./apiClient";

async function request(path, options = {}) {
  return apiRequest(path, options);
}

function isAdmin() {
  const token = localStorage.getItem("token");

  if (!token) return false;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return (
      payload?.role === "Admin" ||
      payload?.role === "admin" ||
      payload?.is_admin === "true"
    );
  } catch {
    return false;
  }
}

export function getAll() {
  const path = isAdmin() ? "/Workouts/admin/all" : "/Workouts";
  return request(path);
}

export function getById(id) {
  return request(`/Workouts/${id}`);
}

export function create(name, userId) {
  return request("/Workouts", {
    method: "POST",
    body: JSON.stringify({ name, userId }),
  });
}

export function update(id, name, userId) {
  return request(`/Workouts/${id}`, {
    method: "PUT",
    body: JSON.stringify({ name, userId }),
  });
}

export function remove(id) {
  return request(`/Workouts/${id}`, { method: "DELETE" });
}

export default { getAll, getById, create, update, remove };
