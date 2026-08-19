const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:5011";

async function request(path, options = {}) {
  const token = localStorage.getItem("token");
  const response = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  });

  if (!response.ok) {
    let message = response.statusText;

    try {
      const data = await response.json();
      message = data?.erro || data?.message || message;
    } catch {
      // A resposta sem JSON mantém o statusText.
    }

    throw new Error(message);
  }

  return response.status === 204 ? null : response.json();
}

export function getAll() {
  return request("/Workouts");
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

export default { getAll, create, update, remove };