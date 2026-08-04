import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { Suspense, lazy } from "react";

const LoginPage = lazy(() => import("./containers/LoginPage/LoginPage"));
const RegisterPage = lazy(() => import("./containers/RegisterPage/RegisterPage"));
const ProfilePage = lazy(() => import("./containers/ProfilePage/ProfilePage"));
const HomePage = lazy(() => import("./containers/HomePage/HomePage"));
const CreateWorkoutPage = lazy(() => import("./containers/CreateWorkoutPage/CreateWorkoutPage"));

function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<div>Carregando...</div>}>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/home" element={<HomePage />} />
          <Route path="/create-workout" element={<CreateWorkoutPage />} />
          <Route path="/" element={<Navigate to="/home" />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}

export default App;
