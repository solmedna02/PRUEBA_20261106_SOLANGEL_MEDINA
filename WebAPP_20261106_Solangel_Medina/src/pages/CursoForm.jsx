import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import {
  obtenerCurso,
  crearCurso,
  actualizarCurso,
} from "../services/cursosService";

const inicial = {
  titulo: "",
  descripcion: "",
  duracion: 0,
  materiales: "",
  cronograma: "",
};

export default function CursoForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const editando = Boolean(id);

  const [form, setForm] = useState(inicial);
  const [error, setError] = useState("");
  const [guardando, setGuardando] = useState(false);
  const [cargando, setCargando] = useState(editando);

  useEffect(() => {
    if (!id) return;

    const cargar = async () => {
      try {
        const curso = await obtenerCurso(id);

        setForm({
          titulo: curso.titulo ?? "",
          descripcion: curso.descripcion ?? "",
          duracion: curso.duracion ?? 0,
          materiales: curso.materiales ?? "",
          cronograma: curso.cronograma
            ? curso.cronograma.substring(0, 10)
            : "",
        });
      } catch {
        setError("No se pudo cargar el curso.");
      } finally {
        setCargando(false);
      }
    };

    cargar();
  }, [id]);

  const cambiar = (e) => {
    const { name, value } = e.target;

    setForm((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  const guardar = async (e) => {
    e.preventDefault();
    setError("");
    setGuardando(true);

    const datos = {
      ...form,
      duracion: Number(form.duracion),
      cronograma: new Date(
        `${form.cronograma}T00:00:00`
      ).toISOString(),
    };

    try {
      if (editando) {
        await actualizarCurso(id, datos);
      } else {
        await crearCurso(datos);
      }

      navigate("/cursos");
    } catch {
      setError("No se pudo guardar el curso.");
    } finally {
      setGuardando(false);
    }
  };

  if (cargando) return <div className="container">Cargando...</div>;

  return (
    <div className="container">
      <div className="page-header">
        <div>
          <h1>{editando ? "Editar curso" : "Nuevo curso"}</h1>
          <p>Completa la información del curso.</p>
        </div>
        <Link to="/cursos" className="link-button">
          Volver
        </Link>
      </div>

      <form className="form-card" onSubmit={guardar}>
        <label>Título</label>
        <input
          name="titulo"
          value={form.titulo}
          onChange={cambiar}
          required
        />

        <label>Descripción</label>
        <textarea
          name="descripcion"
          value={form.descripcion}
          onChange={cambiar}
          rows={4}
        />

        <label>Duración</label>
        <input
          type="number"
          name="duracion"
          min="1"
          value={form.duracion}
          onChange={cambiar}
          required
        />

        <label>Materiales</label>
        <textarea
          name="materiales"
          value={form.materiales}
          onChange={cambiar}
          rows={3}
        />

        <label>Cronograma</label>
        <input
          type="date"
          name="cronograma"
          value={form.cronograma}
          onChange={cambiar}
          required
        />

        {error && <p className="error">{error}</p>}

        <div className="form-actions">
          <Link to="/cursos" className="link-button">
            Cancelar
          </Link>

          <button disabled={guardando}>
            {guardando ? "Guardando..." : "Guardar curso"}
          </button>
        </div>
      </form>
    </div>
  );
}