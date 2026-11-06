import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { obtenerMisCursos } from "../services/inscripcioneservice";

export default function MisCursos() {
  const [cursos, setCursos] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const cargar = async () => {
      try {
        const datos = await obtenerMisCursos();
        setCursos(datos);
      } catch {
        setError("No se pudieron consultar tus cursos.");
      } finally {
        setCargando(false);
      }
    };

    cargar();
  }, []);

  return (
    <div className="container">
      <div className="page-header">
        <div>
          <h1>Mis cursos</h1>
          <p>Cursos en los que estás inscrito.</p>
        </div>

        <Link to="/cursos" className="link-button">
          Ver cursos
        </Link>
      </div>

      {error && <p className="error">{error}</p>}

      {cargando ? (
        <p>Cargando...</p>
      ) : cursos.length === 0 ? (
        <div className="empty-state">
          <h3>No tienes cursos inscritos</h3>
          <p>Puedes explorar los cursos disponibles.</p>
          <Link to="/cursos" className="primary-link">
            Explorar cursos
          </Link>
        </div>
      ) : (
        <div className="cursos-grid">
          {cursos.map((curso) => (
            <div className="curso-card" key={curso.id}>
              <h2>{curso.titulo}</h2>
              <p>{curso.descripcion}</p>
              <p>
                <strong>Duración:</strong> {curso.duracion}
              </p>
              <span className="estado-activo">
                Inscrito
              </span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}