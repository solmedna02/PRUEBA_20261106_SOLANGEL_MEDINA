import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { iniciarSesion } from "../services/login";

export default function Login() {
  const navigate = useNavigate();

  const [correo, setCorreo] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const ingresar = async (e) => {
    e.preventDefault();
    setError("");

    try {
      await iniciarSesion(correo, password);
      navigate("/cursos");
    } catch {
      setError("Correo o contraseña incorrectos");
    }
  };

  return (
    <div className="auth-container">
      <form className="auth-card" onSubmit={ingresar}>
        <h1>Iniciar sesión</h1>
        <p>Plataforma de cursos</p>

        <label>Correo electrónico</label>
        <input
          type="email"
          value={correo}
          onChange={(e) => setCorreo(e.target.value)}
          required
        />

        <label>Contraseña</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        {error && <p className="error">{error}</p>}

        <button type="submit">Ingresar</button>

        <p>
          ¿No tienes cuenta? <Link to="/registro">Regístrate</Link>
        </p>
      </form>
    </div>
  );
}