import { useEffect, useState } from "react";
import { obtenerInscripciones } from "../services/inscripcionservice";

export default function Inscripciones() {
  const [inscripciones, setInscripciones] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const cargar = async () => {
      try {
        const datos = await obtenerInscripciones();
        setInscripciones(datos);
      } catch {
        setError("No se pudieron cargar las inscripciones.");
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
          <h1>Inscripciones</h1>
          <p>Consulta las inscripciones activas.</p>
        </div>
      </div>

      {error && <p className="error">{error}</p>}

      {cargando ? (
        <p>Cargando...</p>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Estudiante</th>
                <th>Curso</th>
                <th>Fecha</th>
                <th>Estado</th>
              </tr>
            </thead>
            <tbody>
              {inscripciones.map((item) => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  <td>{item.usuarioId}</td>
                  <td>{item.cursoId}</td>
                  <td>
                    {new Date(
                      item.fechaInscripcion
                    ).toLocaleDateString("es-EC")}
                  </td>
                  <td>
                    <span className="estado-activo">
                      Activa
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {inscripciones.length === 0 && (
            <p className="empty-message">
              No hay inscripciones activas.
            </p>
          )}
        </div>
      )}
    </div>
  );
}