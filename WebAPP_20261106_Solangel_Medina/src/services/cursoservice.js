import conexion from "./conexion";

export const obtenerCursos = async () => {
  const response = await conexion.get("/Cursos");
  return response.data;
};

export const obtenerCurso = async (id) => {
  const response = await conexion.get(`/Cursos/${id}`);
  return response.data;
};

export const crearCurso = async (curso) => {
  const response = await conexion.post("/Cursos", curso);
  return response.data;
};

export const actualizarCurso = async (id, curso) => {
  const response = await conexion.put(`/Cursos/${id}`, curso);
  return response.data;
};

export const eliminarCurso = async (id) => {
  const response = await conexion.delete(`/Cursos/${id}`);
  return response.data;
};