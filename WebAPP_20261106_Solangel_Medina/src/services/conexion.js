import axios from "axios";

const conexion = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

conexion.interceptors.request.use((config) => {
  const token = sessionStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default conexion;