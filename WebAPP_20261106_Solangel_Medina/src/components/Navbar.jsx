import { Link, useNavigate } from "react-router-dom";
import { obtenerSesion, cerrarSesion } from "../services/login";

export default function Navbar() {
  const navigate = useNavigate();
  const sesion = obtenerSesion();

  const instructor = sesion?.rol === "Instructor";

  const salir = () => {
    cerrarSesion();
    navigate("/login", { replace: true });
  };

  return (
    <nav className="navbar">
      <Link to="/cursos" className="brand">
        Gestión de Cursos
      </Link>

      <div className="nav-links">
        <Link to="/cursos">Cursos</Link>

        {instructor ? (
          <Link to="/inscripciones">Inscripciones</Link>
        ) : (
          <Link to="/mis-cursos">Mis cursos</Link>
        )}

        <button className="btn-secondary" onClick={salir}>
          Cerrar sesión
        </button>
      </div>
    </nav>
  );
}