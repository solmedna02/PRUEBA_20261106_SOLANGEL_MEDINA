import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import {
  obtenerCursos,
  eliminarCurso,
} from "../services/cursoservice";

import { inscribirse } from "../services/inscripcionservice";

import { obtenerSesion } from "../services/login";

export default function Cursos() {
  const [cursos, setCursos] = useState([]);
  const [error, setError] = useState("");
  const [mensaje, setMensaje] = useState("");
  const [cargando, setCargando] = useState(true);

  const sesion = obtenerSesion();
  const instructor = sesion?.rol === "Instructor";

  const cargarCursos = async () => {
    try {
      const datos = await obtenerCursos();
      setCursos(datos);
    } catch {
      setError("No se pudieron cargar los cursos.");
    } finally {
      setCargando(false);
    }
  };

  useEffect(() => {
    const cargar = async () => {
      try {
        const datos = await obtenerCursos();
        setCursos(datos);
      } catch {
        setError("No se pudieron cargar los cursos.");
      } finally {
        setCargando(false);
      }
    };

    cargar();
  }, []);

  const eliminar = async (id) => {
    const confirmar = window.confirm(
      "¿Deseas eliminar este curso?"
    );

    if (!confirmar) return;

    setError("");
    setMensaje("");

    try {
      await eliminarCurso(id);
      setMensaje("Curso eliminado correctamente.");
      await cargarCursos();
    } catch {
      setError("No se pudo eliminar el curso.");
    }
  };

  const inscribir = async (id) => {
    setError("");
    setMensaje("");

    try {
      await inscribirse(id);
      setMensaje("Inscripción realizada correctamente.");
    } catch (err) {
      setError(
        err.response?.status === 409
          ? "Ya existe una inscripción para este curso."
          : "No se pudo realizar la inscripción."
      );
    }
  };

  return (
    <div className="container">
      <div className="page-header">
        <div>
          <h1>Cursos</h1>
          <p>Consulta los cursos disponibles.</p>
        </div>

        {instructor && (
          <Link to="/cursos/nuevo" className="primary-link">
            + Nuevo curso
          </Link>
        )}
      </div>

      {error && <p className="error">{error}</p>}
      {mensaje && <p className="success">{mensaje}</p>}

      {cargando ? (
        <p>Cargando cursos...</p>
      ) : cursos.length === 0 ? (
        <p>No existen cursos disponibles.</p>
      ) : (
        <div className="cursos-grid">
          {cursos.map((curso) => (
            <div className="curso-card" key={curso.id}>
              <h2>{curso.titulo}</h2>
              <p>{curso.descripcion}</p>

              <p>
                <strong>Duración:</strong> {curso.duracion}
              </p>

              <p>
                <strong>Materiales:</strong> {curso.materiales}
              </p>

              <div className="card-actions">
                {instructor ? (
                  <>
                    <Link
                      to={`/cursos/editar/${curso.id}`}
                      className="link-button"
                    >
                      Editar
                    </Link>

                    <button
                      className="btn-danger"
                      onClick={() => eliminar(curso.id)}
                    >
                      Eliminar
                    </button>
                  </>
                ) : (
                  <button onClick={() => inscribir(curso.id)}>
                    Inscribirme
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}