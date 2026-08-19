const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:5011";

async function request(path, body) {
  const res = await fetch(`${API_BASE}${path}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    let message = res.statusText;

    try {
      const data = await res.json();
      message = data?.erro || data?.message || message;
    } catch {
      // resposta não era JSON válido, mantém o statusText
    }

    throw new Error(message);
  }

  return res.json();
}

export async function login(email, password) {
  const data = await request("/Auth/login", { email, password });
  return data?.Token ?? data?.token;
}

export async function register(name, email, password) {
  const data = await request("/Auth/register", { name, email, password });
  return data?.Token ?? data?.token;
}

export function getUserId() {
  const token = localStorage.getItem("token");
  if (!token) {
    return null;
  }

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return Number(payload.nameid || payload.sub) || null;
  } catch {
    return null;
  }
}

export default { login, register, getUserId };