const API_BASE = import.meta.env.VITE_API_URL || "http://localhost:5011";

async function request(path, body) {
    const token = localStorage.getItem("token");
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