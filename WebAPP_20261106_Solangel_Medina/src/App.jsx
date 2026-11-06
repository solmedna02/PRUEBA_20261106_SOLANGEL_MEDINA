import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  Outlet,
} from "react-router-dom";

import Login from "./pages/Login";
import Registro from "./pages/Registro";
import Cursos from "./pages/Curso";
import CursoForm from "./pages/CursoForm";
import MisCursos from "./pages/MiCurso";
import Inscripciones from "./pages/Inscripciones";

import Navbar from "./components/Navbar";
import RutaProtegida from "./components/RutaProtegida";

function Layout() {
  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/registro" element={<Registro />} />

        <Route element={<RutaProtegida />}>
          <Route element={<Layout />}>
            <Route path="/cursos" element={<Cursos />} />

            <Route element={<RutaProtegida roles={["Estudiante"]} />}>
              <Route path="/mis-cursos" element={<MisCursos />} />
            </Route>

            <Route element={<RutaProtegida roles={["Instructor"]} />}>
              <Route path="/cursos/nuevo" element={<CursoForm />} />
              <Route path="/cursos/editar/:id" element={<CursoForm />} />
              <Route path="/inscripciones" element={<Inscripciones />} />
            </Route>
          </Route>
        </Route>

        <Route path="/" element={<Navigate to="/cursos" replace />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
