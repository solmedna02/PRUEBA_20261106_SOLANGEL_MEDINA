import conexion from "./conexion";

const ROLE_CLAIM =
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

export function obtenerSesion() {
  const token = sessionStorage.getItem("token");

  if (!token) return null;

  try {
    const payload = token.split(".")[1];

    const base64 = payload
      .replace(/-/g, "+")
      .replace(/_/g, "/");

    const datos = JSON.parse(
      atob(base64)
    );

    if (datos.exp && datos.exp * 1000 <= Date.now()) {
      sessionStorage.removeItem("token");
      return null;
    }

    return {
      token,
      nombre: datos.unique_name ?? datos.name ?? "",
      rol: datos[ROLE_CLAIM] ?? datos.role ?? "",
    };

  } catch {
    sessionStorage.removeItem("token");
    return null;
  }
}

export function cerrarSesion() {
  sessionStorage.removeItem("token");
}

export async function iniciarSesion(correo, password) {
  const response = await conexion.post("/Usuarios/login", {
    correo,
    password,
  });

  sessionStorage.setItem("token", response.data.token);

  return response.data;
}

export async function registrarUsuario(datos) {
  const response = await conexion.post("/Usuarios/registro", datos);

  return response.data;
}