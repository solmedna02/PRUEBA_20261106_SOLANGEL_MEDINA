
import conexion from "./conexion";

export const inscribirse = async (cursoId) => {
  const response = await conexion.post("/Inscripciones", {
    cursoId,
  });

  return response.data;
};

export const obtenerMisCursos = async () => {
  const response = await conexion.get("/Inscripciones/mis-cursos");
  return response.data;
};

export const obtenerInscripciones = async () => {
  const response = await conexion.get("/Inscripciones");
  return response.data;
};