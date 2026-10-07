import { apiRequest } from "./apiClient";

async function request(path, options = {}) {
  return apiRequest(path, options);
}

export function getAll() {
  return request("/Exercises");
}

export default { getAll };
