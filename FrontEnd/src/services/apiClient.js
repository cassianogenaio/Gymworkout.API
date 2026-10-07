const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:5011";

let refreshRequest;

async function refreshAccessToken() {
  if (!refreshRequest) {
    refreshRequest = fetch(`${API_BASE}/Auth/refresh`, {
      method: "POST",
      credentials: "include",
    })
      .then(async (response) => {
        if (!response.ok) return null;

        const data = await response.json();
        const token = data?.Token ?? data?.token;
        if (token) localStorage.setItem("token", token);
        return token || null;
      })
      .catch(() => null)
      .finally(() => {
        refreshRequest = null;
      });
  }

  return refreshRequest;
}

async function readError(response) {
  const fallback = "Não foi possível concluir a solicitação.";

  try {
    const text = await response.text();
    if (!text) return fallback;

    const data = JSON.parse(text);
    if (data?.errors && typeof data.errors === "object") {
      const validationErrors = Object.values(data.errors)
        .flat()
        .filter(Boolean);
      if (validationErrors.length) return validationErrors.join(" ");
    }

    return (
      data?.erro || data?.message || data?.title || data?.detail || fallback
    );
  } catch {
    return fallback;
  }
}

export async function apiRequest(path, options = {}) {
  const sendRequest = () => {
    const token = localStorage.getItem("token");
    return fetch(`${API_BASE}${path}`, {
      ...options,
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options.headers,
      },
    });
  };

  let response = await sendRequest();
  if (response.status === 401 && !path.startsWith("/Auth/")) {
    const refreshedToken = await refreshAccessToken();
    if (refreshedToken) {
      response = await sendRequest();
    } else {
      localStorage.removeItem("token");
      window.location.assign("/login");
    }
  }

  if (!response.ok) {
    throw new Error(await readError(response));
  }

  return response.status === 204 ? null : response.json();
}
