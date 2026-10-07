import { apiRequest } from "./apiClient";

async function request(path, options = {}) {
  return apiRequest(path, options);
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
