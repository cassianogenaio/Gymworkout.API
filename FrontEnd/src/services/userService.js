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
      const text = await response.text();
      if (text) {
        const data = JSON.parse(text);

        if (data?.errors && typeof data.errors === "object") {
          const allErrors = Object.values(data.errors).flat().filter(Boolean);

          if (allErrors.length) {
            message = allErrors.join(" ");
          }
        }

        if (!message || message === response.statusText) {
          message =
            data?.erro ||
            data?.message ||
            data?.title ||
            data?.detail ||
            response.statusText;
        }
      }
    } catch {
      message = response.statusText;
    }

    throw new Error(message);
  }

  return response.status === 204 ? null : response.json();
}

export function getCurrentUser(userId) {
  return request(`/Users/${userId}`);
}

export async function changePassword(currentPassword, newPassword) {
  return request("/Users/change-password", {
    method: "PUT",
    body: JSON.stringify({ currentPassword, newPassword }),
  });
}

export default { getCurrentUser, changePassword };
