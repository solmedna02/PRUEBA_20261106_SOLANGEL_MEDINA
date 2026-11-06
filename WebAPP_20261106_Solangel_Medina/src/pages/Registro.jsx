import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { registrarUsuario } from "../services/login";

export default function Registro() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    nombre: "",
    correo: "",
    password: "",
  });

  const [error, setError] = useState("");

  const cambiar = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  };

  const registrar = async (e) => {
    e.preventDefault();
    setError("");

    try {
      await registrarUsuario(form);
      navigate("/login");
    } catch {
      setError("No se pudo registrar el usuario");
    }
  };

  return (
    <div className="auth-container">
      <form className="auth-card" onSubmit={registrar}>
        <h1>Crear cuenta</h1>
        <p>Regístrate como estudiante</p>

        <label>Nombre</label>
        <input
          name="nombre"
          value={form.nombre}
          onChange={cambiar}
          required
        />

        <label>Correo electrónico</label>
        <input
          type="email"
          name="correo"
          value={form.correo}
          onChange={cambiar}
          required
        />

        <label>Contraseña</label>
        <input
          type="password"
          name="password"
          minLength={8}
          value={form.password}
          onChange={cambiar}
          required
        />

        {error && <p className="error">{error}</p>}

        <button type="submit">Registrarse</button>

        <p>
          ¿Ya tienes cuenta? <Link to="/login">Inicia sesión</Link>
        </p>
      </form>
    </div>
  );
}