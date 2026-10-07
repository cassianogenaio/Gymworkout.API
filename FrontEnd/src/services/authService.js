import { apiRequest } from "./apiClient";

async function request(path, body) {
  return apiRequest(path, {
    method: "POST",
    body: JSON.stringify(body),
  });
}

export async function login(email, password) {
  const data = await request("/Auth/login", { email, password });
  return data?.Token ?? data?.token;
}

export async function register(name, email, password) {
  const data = await request("/Auth/register", { name, email, password });
  return data?.Token ?? data?.token;
}

export async function logout() {
  try {
    await request("/Auth/logout", {});
  } finally {
    localStorage.removeItem("token");
  }
}

export function getUserId() {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return Number(payload.nameid || payload.sub) || null;
  } catch {
    return null;
  }
}

export default { login, register, logout, getUserId };
