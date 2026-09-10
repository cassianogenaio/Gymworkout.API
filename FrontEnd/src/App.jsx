import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { Suspense, lazy } from "react";

const LoginPage = lazy(() => import("./containers/LoginPage/LoginPage"));
const RegisterPage = lazy(() => import("./containers/RegisterPage/RegisterPage"));
const ProfilePage = lazy(() => import("./containers/ProfilePage/ProfilePage"));
const HomePage = lazy(() => import("./containers/HomePage/HomePage"));
const EditWorkoutPage = lazy(() => import("./containers/EditWorkoutPage/EditWorkoutPage"));

function IsAuthenticated ({ children }) {
  const token = localStorage.getItem("token");

  if (!token) {
    return <Navigate to="/login" replace></Navigate>
  }
  return children
}

function App() {
  return (
    <BrowserRouter>
      <Suspense fallback={<div>Carregando...</div>}>
        <Routes>
          <Route path="/login" element={<LoginPage />}/>
          <Route path="/register" element={<RegisterPage />} />
          
          {/* authenticated pages */}
          <Route path="/profile" element={<IsAuthenticated><ProfilePage /></IsAuthenticated> }/>
          <Route path="/home" element={<IsAuthenticated><HomePage /></IsAuthenticated> }/>
          <Route path="/edit-workout/:id" element={<IsAuthenticated><EditWorkoutPage /></IsAuthenticated> }/>
          
          <Route path="/" element={<Navigate to="/home" />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
}

export default App;
