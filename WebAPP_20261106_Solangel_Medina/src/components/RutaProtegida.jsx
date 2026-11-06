import { Navigate, Outlet } from "react-router-dom";
import { obtenerSesion } from "../services/login";

export default function RutaProtegida({ roles }) {
  const sesion = obtenerSesion();

  // Si no ha iniciado sesión
  if (!sesion) {
    return <Navigate to="/login" replace />;
  }

  // Si el usuario no tiene el rol permitido
  if (roles && !roles.includes(sesion.rol)) {
    return <Navigate to="/cursos" replace />;
  }

  // Permitir acceso a la pantalla
  return <Outlet />;
}